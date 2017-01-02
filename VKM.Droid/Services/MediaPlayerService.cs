using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
    public class MediaPlayerService : Service, AudioManager.IOnAudioFocusChangeListener
    {
        public const string ActionPlay = "com.vkm.player.action.play";
        public const string ActionPause = "com.vkm.player.action.pause";
        public const string ActionStop = "com.vkm.player.action.stop";
        public const string ActionNext = "com.vkm.player.action.next";
        public const string ActionPrev = "com.vkm.player.action.prev";
        public const string ActionSetSource = "com.vkm.player.action.setsource";

        public const string SourceValueName = "SOURCE";

        public static MediaPlayerService instance;

        private MediaPlayer _player;
        private AudioInfo _currentAudio;
        
        private WifiManager.WifiLock _wifiLock;

        private MediaSession _mediaSession;
        private MediaController _mediaController;

        private Thread _playbackPosThread;

        public delegate void OnInstanceListener(MediaPlayerService instance);
        public delegate void PlayerEventListener();
        public delegate void PlayerPlaybackStateListner(VkmPlaybackState state);
        public delegate void PlaybackPositionListener(long currentPos);

        public static event OnInstanceListener OnInstanceCreated;
        public event PlayerEventListener OnNext;
        public event PlayerEventListener OnPrev;
        public event PlayerEventListener OnError;

        public event PlayerPlaybackStateListner PlaybackStateChanged;
        private VkmPlaybackState _state = VkmPlaybackState.Stoped; //PlaybackState.NoMedia
        public VkmPlaybackState State {
            get { return _state; }
            private set
            {
                if (_state == value) {
                    return;
                }
                _state = value;
                PlaybackStateChanged(_state);
            }
        }


        public event PlaybackPositionListener DurationChanged;
        private long _duration;
        public long Duration
        {
            get { return _duration; }
            set
            {
                if (_duration == value) return;
                _duration = value;
                DurationChanged(_duration);
            }
        }

        public event PlaybackPositionListener PlaybackPositionChanged;
        private long _currPos;
        public long CurrentPosition {
            get
            {
                return _currPos;
            }
            set {
                _currPos = value;
                PlaybackPositionChanged(_currPos);
            }
        }

        public override void OnCreate()
        {
            instance = this;
            OnInstanceCreated(this);
            _player = new MediaPlayer();
            _player.Prepared += OnPlayerPrepared;
            _player.Completion += (sender, e) => Next();
            _player.Error += (sender, e) => {
                Stop();
                OnError();
            };
            //int version = (int) Build.VERSION.SdkInt;
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
            if (State == VkmPlaybackState.Playing) {
                _player.Pause();
                State = VkmPlaybackState.Paused;
            }
        }

        private void Play()
        {
            if (State == VkmPlaybackState.Stoped) {
                _player.SetAudioStreamType(Stream.Music);
                _player.SetDataSource(_currentAudio.source);
                _player.PrepareAsync();
                State = VkmPlaybackState.Preparing;
            } else if (State == VkmPlaybackState.Paused) {
                _player.Start();
                State = VkmPlaybackState.Playing;
            }
        }

        private void OnPlayerPrepared(object sender, EventArgs e)
        {
            SetupPlaybackPosThread();
            _player.Start();
            _player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);
            AquireWifiLock();
            Duration = _player.Duration;
            (GetSystemService(AudioService) as AudioManager).RequestAudioFocus(this, Stream.Music, AudioFocus.Gain);
            State = VkmPlaybackState.Playing;
        }

        private void Seek(long msPos)
        {
            if (State == VkmPlaybackState.Paused || State == VkmPlaybackState.Playing) {
                _player.SeekTo((int)msPos);
            }
        }

        private void SetupPlaybackPosThread()
        {
            if (_playbackPosThread == null)
            {
                _playbackPosThread = new Thread(CheckPlaybackPos);
            }
            if (_playbackPosThread.ThreadState == ThreadState.Stopped ||
                _playbackPosThread.ThreadState == ThreadState.Unstarted)
            {
                _playbackPosThread.Start();
            }
        }

        private void CheckPlaybackPos()
        {
            Console.WriteLine("Stert checking playback position");
            while (true)
            {
                if (State != VkmPlaybackState.Playing) continue;
                CurrentPosition = _player.CurrentPosition;
                Thread.Sleep(100);
            }
        }

        private void Stop()
        {
            if (State == VkmPlaybackState.Paused || State == VkmPlaybackState.Playing) {
                _player.Stop();
                _player.Reset();
                ReleaseWifiLock();
                State = VkmPlaybackState.Stoped;
            } else if (State == VkmPlaybackState.Preparing) {
                _player.Reset();
                State = VkmPlaybackState.Stoped;
            }
        }

        private void Next()
        {
            OnNext();
        }

        private void Prev()
        {
            OnPrev();
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

        public void OnAudioFocusChange([GeneratedEnum] AudioFocus focusChange)
        {
            switch (focusChange) {
                case AudioFocus.Gain:
                    _player.SetVolume(1.0f, 1.0f);
                    break;
                case AudioFocus.Loss:
                    Stop();
                    break;
                case AudioFocus.LossTransient:
                    Pause();
                    break;
                case AudioFocus.LossTransientCanDuck:
                    _player.SetVolume(0.1f, 0.1f);
                    break;
            }
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