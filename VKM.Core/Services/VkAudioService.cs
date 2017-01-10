using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform;
using VKM.Core.JsonClasses;

namespace VKM.Core.Services
{
    internal class VkAudioService : IVkAudioService
    {
        private const string ApiKey = "seIF0hkkjLPlGwHkWvVIkpP9ZfbeSQub";
        private const string ClientSecret = "WkF1h5BxtwoybDEeykOs0nRclZAwxnDM";
        private const string ApiUrl = "https://api.soundcloud.com";
        private const string RefreshRequestType = "refresh_token";
        private const string PasswordLoginRequestType = "password";

        private readonly IRestApiService _restApi;
        private readonly IStorageService _storage;

        public VkAudioService(IRestApiService restApi, IStorageService storage)
        {
            _restApi = restApi;
            _storage = storage;
        }

        public void Search(string searchTerm, Action<List<Audio>> succesAction, Action<Exception> errorAction)
        {
            string orderBy = null;
            switch (_storage.AudioSorting)
            {
                case AudioSorting.Date:
                    orderBy = "created_at";
                    break;
                case AudioSorting.Duration:
                    orderBy = "duration";
                    break;
                case AudioSorting.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var searchUrl =
                $"{ApiUrl}/tracks?q={searchTerm.Replace(' ', '+')}&client_id={ApiKey}{(orderBy != null ? $"&order={orderBy}" : "")}";
            _restApi.MakeRequest<List<Track>>(searchUrl, "GET", "",
                result =>
                {
                    var audioList = result.Where(tr => tr.streamable);
                    if (_storage.FiltersActive)
                        if (string.IsNullOrWhiteSpace(_storage.FilterString))
                            audioList =
                                audioList.Where(tr => !tr.title.ToLower().Contains(_storage.FilterString.ToLower()));
                    var selectedList = audioList.Select(tr =>
                    {
                        var streamUrl = tr.stream_url + "?client_id=" + ApiKey;
                        return new Audio(tr.id, tr.user.username, tr.title, tr.duration, streamUrl);
                    }).ToList();
                    succesAction(selectedList);
                },
                error => errorAction?.Invoke(error));
        }

        public void Login(string login = null, string password = null, Action authSuccessAction = null,
            Action<Exception> authFailureAction = null)
        {
            if (!TokenExpired())
            {
                authSuccessAction?.Invoke();
                return;
            }
            string postData;
            if (_storage.RefreshToken != null)
            {
                postData =
                    $"client_id={ApiKey}&client_secret={ClientSecret}&grant_type={RefreshRequestType}&refresh_token={_storage.RefreshToken}";
            }
            else if (login != null && password != null)
            {
                postData =
                    $"client_id={ApiKey}&client_secret={ClientSecret}&grant_type={PasswordLoginRequestType}&username={login}&password={password}";
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
                        _storage.OAuthToken = result.access_token;
                        _storage.RefreshToken = result.refresh_token;
                        _storage.TokenExpireTime = DateTime.Now.AddSeconds(result.expires_in);
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
            _restApi.MakeRequest<List<Playlist>>($"{ApiUrl}/me/playlists?oauth_token={_storage.OAuthToken}", "GET", "",
                result =>
                {
                    var audioList = new List<Audio>();
                    foreach (var pl in result)
                        audioList.AddRange(from tr in pl.tracks
                            where tr.streamable
                            let streamUrl = tr.stream_url + "?client_id=" + ApiKey
                            select new Audio(tr.id, tr.user.username, tr.title, tr.duration, streamUrl));
                    succesAction(audioList);
                },
                error => { errorAction?.Invoke(error); });
        }

        private bool TokenExpired()
        {
            return DateTime.Now >= _storage.TokenExpireTime;
        }
    }
}