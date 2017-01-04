using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

        private readonly IRestApiService _restApi;

        public VkAudioService(IRestApiService restApi)
        {
            _restApi = restApi;
        }

        public void Search(string searchTerm, Action<List<Audio>> succesAction, Action<Exception> errorAction)
        {
            var searchUrl = $"{ApiUrl}/tracks?q={searchTerm.Replace(' ', '+')}&client_id={ApiKey}";
            _restApi.MakeRequest<List<Track>>(searchUrl, "GET", "",
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

        private bool TokenExpired()
        {
            return DateTime.Now >= _tokenRefreshTime;
        }

        public void Login(string login = null, string password = null , Action authSuccessAction = null, Action<Exception> authFailureAction = null)
        {
            if (!TokenExpired())
            {
                authSuccessAction?.Invoke();
                return;
            }
            string postData;
            if (RefreshToken != null)
            {
                postData = $"client_id={ApiKey}&client_secret={ClientSecret}&grant_type={RefreshRequestType}&refresh_token={RefreshToken}";
            }
            else if (login != null && password != null)
            {
                postData = $"client_id={ApiKey}&client_secret={ClientSecret}&grant_type={PasswordLoginRequestType}&username={login}&password={password}";
            }
            else
            {
                authFailureAction?.Invoke(new Exception("Not enough auth data"));
                return;
            }
            _restApi.MakeRequest<AuthInfo>($"{ApiUrl}/oauth2/token", "POST",
                postData,
                result =>
                {
                    if (result.error == null)
                    {
                        Mvx.Warning($"Login succcess. Granted token - {result.access_token}");
                        OAuthToken = result.access_token;
                        RefreshToken = result.refresh_token;
                        _tokenRefreshTime = DateTime.Now.AddMilliseconds(result.expires_in);
                        authSuccessAction?.Invoke();
                    }
                    else
                    {
                        authFailureAction?.Invoke(new Exception(result.error));
                    }
                }, error => authFailureAction?.Invoke(error));
        }

        public void GetMyPlaylist(Action<List<Audio>> succesAction, Action<Exception> errorAction)
        {
            _restApi.MakeRequest<List<Playlist>>($"{ApiUrl}/me/playlists?oauth_token={OAuthToken}", "GET", "",
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
