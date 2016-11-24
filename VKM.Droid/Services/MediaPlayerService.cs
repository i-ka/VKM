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
    [IntentFilter(new[] { ActionPlay, ActionPause, ActionStop, ActionNext, ActionPrev, ActionSetSource })]
    public class MediaPlayerService : Service
    {
        public const string ActionPlay = "com.vkm.player.action.play";
        public const string ActionPause = "com.vkm.player.action.pause";
        public const string ActionStop = "com.vkm.player.action.stop";
        public const string ActionNext = "com.vkm.player.action.next";
        public const string ActionPrev = "com.vkm.player.action.prev";
        public const string ActionSetSource = "com.vkm.player.action.setsource";

        public const string SourceValueName = "SOURCE";

        public static MediaPlayerService instance;
        public VkmPlaybackState Status { get { return _state; } }

        private MediaPlayer _player;
        private AudioInfo _currentAudio;
        private VkmPlaybackState _state = VkmPlaybackState.Stoped; //PlaybackState.NoMedia
        
        private WifiManager.WifiLock _wifiLock;

        private MediaSession _mediaSession;
        private MediaController _mediaController;

        public override void OnCreate()
        {
            instance = this;
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
                _player.SetDataSource(_currentAudio.source);
                _player.PrepareAsync();
                _state = VkmPlaybackState.Preparing;
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
            } else if (_state == VkmPlaybackState.Preparing) {
                _player.Reset();
                _state = VkmPlaybackState.Stoped;
            }
        }

        private void Next()
        {
            
        }

        private void Prev()
        {
            
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
                    }
                    else {
                        _mediaController.GetTransportControls().Play();
                    }
                    break;
                case ActionPause:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Pause();
                    }
                    else {
                        _mediaController.GetTransportControls().Pause();
                    }
                    break;
                case ActionStop:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Stop();
                    }
                    else {
                        _mediaController.GetTransportControls().Stop();
                    }
                    break;
                case ActionPrev:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Prev();
                    }
                    else {
                        _mediaController.GetTransportControls().SkipToPrevious();
                    }
                    break;
                case ActionNext:
                    if (Build.VERSION.SdkInt <= BuildVersionCodes.Kitkat) {
                        Next();
                    }
                    else {
                        _mediaController.GetTransportControls().SkipToNext();
                    }
                    break;
               case ActionSetSource:
                   _currentAudio = AudioInfo.UnPack(intent.GetStringExtra(SourceValueName));
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