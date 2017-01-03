using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using VKM.Core.JsonClasses;

namespace VKM.Core.Services
{
    class VkAudioService : IVkAudioService
    {
        private const string ApiKey = "seIF0hkkjLPlGwHkWvVIkpP9ZfbeSQub";
        private const string ClientSecret = "WkF1h5BxtwoybDEeykOs0nRclZAwxnDM";
        private const string ApiUrl = "https://api.soundcloud.com";
        private const string RefreshRequestType = "refresh_token";
        private const string PasswordLoginRequestType = "password";
        private DateTime _tokenRefreshTime;
        public string OAuthToken { get; set; }
        public string RefreshToken { get; set; }


        public void Search(string searchTerm, Action<List<Audio>> succesAction, Action<Exception> errorAction)
        {
            var searchUrl = $"{ApiUrl}/tracks?q={searchTerm.Replace(' ', '+')}&client_id={ApiKey}";
            MakeRequest<List<Track>>(searchUrl, "GET", "",
                result =>
                {
                    var audioList = result.Where(tr => tr.streamable).Select(tr =>
                    {
                        var streamUrl = tr.stream_url + "?client_id=" + ApiKey;
                        return new Audio(tr.user.username, tr.title, tr.duration, streamUrl);
                    }).ToList();
                    succesAction(audioList);
                },
                error => errorAction?.Invoke(error));
        }

        private readonly IMvxJsonConverter _jsonConverter;

        public VkAudioService(IMvxJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        public void MakeRequest<T>(string url, string verb, string postData, Action<T> successAction,
            Action<Exception> errAction)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = verb;
            request.Accept = "application/json";
            if (postData != "" && verb == "POST")
            {
                Mvx.Warning(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream((tocken) =>
                {
                    using (var reqStr = request.EndGetRequestStream(tocken))
                    {
                        var writer = new StreamWriter(reqStr);
                        writer.Write(postData, 0 , postData.Length);
                        writer.Flush();
                        BeginRequest(request, successAction, errAction);
                    }
                }, null);
                return;
            }
            BeginRequest(request, successAction, errAction);
        }

        private void BeginRequest<T>(HttpWebRequest request, Action<T> successAction, Action<Exception> errAction)
        {
            MakeRequest(request,
                response =>
                {
                    T retVal;
                    try {
                        retVal = Desirealize<T>(response);
                    }
                    catch (Exception ex) {
                        errAction(ex);
                        return;
                    }
                    successAction?.Invoke(retVal);
                },
                error => errAction?.Invoke(error));
        }

        private void MakeRequest(HttpWebRequest request, Action<string> successAction, Action<Exception> errAction)
        {
            request.BeginGetResponse(token =>
            {
                try
                {
                    using (var response = request.EndGetResponse(token))
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            var reader = new StreamReader(stream);
                            successAction(reader.ReadToEnd());
                        }
                    }
                }
                catch (Exception e)
                {
                    Mvx.Error("Error: '{0}' when making {1} request to {2}", e.Message, request.Method, request.RequestUri);
                    errAction(e);
                }
            }, null);
        }

        private T Desirealize<T>(string response)
        {
            var result = _jsonConverter.DeserializeObject<T>(response);
            return result;
        }

        private bool TokenExpired()
        {
            return DateTime.Now >= _tokenRefreshTime;
        }

        public void Login(string login, string password, Action authSuccessAction, Action<Exception> authFailureAction)
        {
            if (!TokenExpired())
            {
                return;
            }
            var authType = RefreshToken != null ? RefreshRequestType : PasswordLoginRequestType;
            MakeRequest<AuthInfo>($"{ApiUrl}/oauth2/token", "POST",
                $"client_id={ApiKey}&client_secret={ClientSecret}&grant_type={authType}&username={login}&password={password}",
                result =>
                {
                    if (result.error == null)
                    {
                        Mvx.Warning($"Login succcess. Granted token - {result.access_token}");
                        OAuthToken = result.access_token;
                        RefreshToken = result.refresh_token;
                        _tokenRefreshTime = DateTime.Now.AddMilliseconds(result.expires_in);
                        authSuccessAction();
                    }
                    else
                    {
                        authFailureAction?.Invoke(new Exception(result.error));
                    }
                }, error => authFailureAction?.Invoke(error));
        }

        public void GetMyPlaylist(Action<List<Audio>> succesAction, Action<Exception> errorAction)
        {
            MakeRequest<List<Playlist>>($"{ApiUrl}/me/playlists?oauth_token={OAuthToken}", "GET", "",
                result =>
                {
                    var audioList = new List<Audio>();
                    foreach (var pl in result)
                    {
                        audioList.AddRange(from tr in pl.tracks where tr.streamable let streamUrl = tr.stream_url + "?client_id=" + ApiKey select new Audio(tr.user.username, tr.title, tr.duration, streamUrl));
                    }
                    succesAction(audioList);
                },
                error =>
                {
                    errorAction?.Invoke(error);
                });
        }
    }
}
