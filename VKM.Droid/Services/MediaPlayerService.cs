using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Media;
using Android.Media.Session;
using Android.Runtime;
using Android.Net.Wifi;
using Android.Net;

using VKM.Core.Services;

namespace VKM.Droid.Services
{
    [Service]
    [IntentFilter(new[] { ActionPlay, ActionPause, ActionStop, ActionNext, ActionPrev, ActionSetPlayList })]
    class MediaPlayerService : Service
    {
        public const string ActionPlay = "com.vkm.player.action.play";
        public const string ActionPause = "com.vkm.player.action.pause";
        public const string ActionStop = "com.vkm.player.action.stop";
        public const string ActionNext = "com.vkm.player.action.next";
        public const string ActionPrev = "com.vkm.player.action.prev";
        public const string ActionSetPlayList = "com.vkm.player.action.setplaylist";
        public const string ActionSetCurrent = "com.vkm.player.action.setcurrent";

        public const string PlaylistValueName = "PLAY_LIST";
        public const string CurrentIndexValueName = "INDEX";

        private MediaPlayer _player;
        private List<AudioInfo> _playlist;
        private VkmPlaybackState _state = VkmPlaybackState.Stoped; //PlaybackState.NoMedia
        private int _currentSourceIdx = 0;
        private WifiManager.WifiLock _wifiLock;

        private MediaSession _mediaSession;
        private MediaController _mediaController;

        public override void OnCreate()
        {
            _playlist = new List<AudioInfo>();
            _player = new MediaPlayer();
            int version = (int) Build.VERSION.SdkInt;
            if (!(Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat)) {
                _mediaSession = new MediaSession(Application.Context, "VKM player");
                _mediaController = new MediaController(Application.Context, _mediaSession.SessionToken);
                var callback = new VkmMediaSessionCallBack();
                callback.OnPlayImpl = Play;
                callback.OnPauseImpl = Pause;
                callback.OnSkipToNextImpl = Next;
                callback.OnSkipToPreviousImpl = Prev;
                callback.OnSeekToImpl = Seek;
                callback.OnStopImpl = Stop;
                _mediaSession.SetCallback(callback);
            }
        }

        private void Pause()
        {
            if (_state == VkmPlaybackState.Playing) {
                _player.Pause();
                _state = VkmPlaybackState.Paused;
            }
        }

        private void Play()
        {
            if (_state == VkmPlaybackState.Stoped) {
                _player.SetAudioStreamType(Stream.Music);
                _player.SetDataSource(_playlist[_currentSourceIdx].source);
                _player.PrepareAsync();
                _player.Prepared += (sender, e) => {
                    _player.Start();
                    _player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);
                    AquireWifiLock();
                    _state = VkmPlaybackState.Playing;
                };
                _player.Completion += (sender, e) => Next();
            } else if (_state == VkmPlaybackState.Paused) {
                _player.Start();
                _state = VkmPlaybackState.Playing;
            }
        }

        private void Seek(long msPos)
        {
            if (_state == VkmPlaybackState.Paused || _state == VkmPlaybackState.Playing) {
                _player.SeekTo((int)msPos);
            }
        }

        private void Stop()
        {
            if (_state == VkmPlaybackState.Paused || _state == VkmPlaybackState.Playing) {
                _player.Stop();
                _player.Reset();
                ReleaseWifiLock();
                _state = VkmPlaybackState.Stoped;
            }
        }

        private void Next()
        {
            var prSt = _state;
            Stop();
            _currentSourceIdx++;
            if (_currentSourceIdx >= _playlist.Count) {
                _currentSourceIdx = 0;
            }
            if (prSt == VkmPlaybackState.Playing) {
                Play();
            }
        }

        private void Prev()
        {
            var prSt = _state;
            Stop();
            _currentSourceIdx--;
            if (_currentSourceIdx < 0) {
                _currentSourceIdx = _playlist.Count-1;
            }
            if (prSt == VkmPlaybackState.Playing) {
                Play();
            }
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            switch (intent.Action) {
                case ActionPlay:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Play();
                    } else {
                       _mediaController.GetTransportControls().Play();
                    }
                    //Play();
                    break;
                case ActionPause:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Pause();
                    } else {
                        _mediaController.GetTransportControls().Pause();
                    }
                    //Pause();
                    break;
                case ActionStop:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Pause();
                    }
                    else {
                        _mediaController.GetTransportControls().Stop();
                    }
                    //Stop();
                    break;
                case ActionPrev:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Pause();
                    }
                    else {
                        _mediaController.GetTransportControls().SkipToPrevious();
                    }
                    //Prev();
                    break;
                case ActionNext:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Pause();
                    }
                    else {
                        _mediaController.GetTransportControls().SkipToNext();
                    }
                    //Next();
                    break;
                case ActionSetPlayList:
                    _playlist = intent.GetStringArrayListExtra(PlaylistValueName).Select(x => AudioInfo.UnPack(x)).ToList();
                    break;
                case ActionSetCurrent:
                    Stop();
                    _currentSourceIdx = intent.GetIntExtra(CurrentIndexValueName, 0);
                    Play();
                    break;
            }
            return StartCommandResult.Sticky;
        }

        private void AquireWifiLock()
        {
            WifiManager wifiManager = Application.Context.GetSystemService(Context.WifiService) as WifiManager;
            if (_wifiLock == null) {
                _wifiLock = wifiManager.CreateWifiLock(WifiMode.Full, "xamarin_wifi_lock");
            }
            _wifiLock.Acquire();
        }

        private void ReleaseWifiLock()
        {
            if (_wifiLock == null) {
                return;
            }
            _wifiLock.Release();
            _wifiLock = null;
        }
    }

    class VkmMediaSessionCallBack : MediaSession.Callback
    {
        public Action OnPlayImpl;
        public Action OnPauseImpl;
        public Action OnStopImpl;
        public Action OnSkipToNextImpl;
        public Action OnSkipToPreviousImpl;
        public Action<long> OnSeekToImpl;

        public override void OnPlay()
        {
            OnPlayImpl();
        }

        public override void OnPause()
        {
            OnPauseImpl();
        }

        public override void OnStop()
        {
            OnStopImpl();
        }

        public override void OnSkipToNext()
        {
            OnSkipToNextImpl();
        }

        public override void OnSkipToPrevious()
        {
            OnSkipToPreviousImpl();
        }

        public override void OnSeekTo(long pos)
        {
            OnSeekToImpl(pos);
        }
    }
}