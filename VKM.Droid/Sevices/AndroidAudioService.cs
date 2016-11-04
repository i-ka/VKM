using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using VKM.Core.Services;


namespace VKM.Droid.Sevices
{
    class AndroidAudioService : IPlayerService
    {
        private MediaPlayer _player = new MediaPlayer();
        private PlaybackState _state = PlaybackState.Stoped;
        private string _source = "https://cs3-2v4.vk-cdn.net/p2/f6ace8e9a9e4c5.mp3?extra=8z7yFcy_6Kl2Sax6Q_5sJSNU9upWa3N5qWRyl-P8C6e3vLfJdnDBM_NDH_UTJjG7QNIlN5NKCTe9o_L0442w9JhzS4frRgWR3bBVgM5T3y3sknDryGWUypq1q9aT0mx6HonFu_CWox4j";
        public string Source
        {
            get { return _source; }
            set {
                Stop();
                _source = value;
                if (_source == "") {
                    _state = PlaybackState.NoMedia;
                }
            }
        }

        public void Pause()
        {
            if (_state == PlaybackState.Playing) {
                _player.Pause();
                _state = PlaybackState.Paused;
            }
        }

        public async Task Play()
        {
            if (_state == PlaybackState.Stoped || _state == PlaybackState.Paused) {
                _player.SetAudioStreamType(Stream.Music);
                _player.SetDataSource(_source);
                await Task.Factory.StartNew(() => _player.Prepare());
                _player.Start();
                _state = PlaybackState.Playing;
            }
        }

        public void Seek(int msPos)
        {
            if (_state == PlaybackState.Paused || _state == PlaybackState.Playing) {
                _player.SeekTo(msPos);
            }
        }

        public void Stop()
        {
            if (_state == PlaybackState.Paused || _state == PlaybackState.Playing) {
                _player.Stop();
                _player.Reset();
                _state = PlaybackState.Stoped;
            }
        }
    }
}