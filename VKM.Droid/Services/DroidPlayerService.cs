using Android.App;
using Android.Content;
using VKM.Core.Services;

namespace VKM.Droid.Services
{
    internal class DroidPlayerService : IPlayerService
    {
        public VkmPlaybackState Status
        {
            get
            {
                if (MediaPlayerService.instance != null) return MediaPlayerService.instance.State;
                return VkmPlaybackState.NoMedia;
            }
        }

        public void Pause()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionPause);
            Application.Context.StartService(intent);
        }

        public void SetSource(Audio audio)
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionSetSource);
            intent.PutExtra(MediaPlayerService.SourceValueName, audio.AudioInfo.Pack());
            Application.Context.StartService(intent);
        }

        public void Start()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionPlay);
            Application.Context.StartService(intent);
        }

        public void Stop()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionStop);
            Application.Context.StartService(intent);
        }

        public void Seek(long pos)
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionSeek);
            intent.PutExtra(MediaPlayerService.SeekPosValueName, (int) pos);
            Application.Context.StartService(intent);
        }
    }
}