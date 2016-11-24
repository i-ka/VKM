using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VKM.Core.Services;

namespace VKM.Droid.Services
{
    class DroidPlayerService : IPlayerService

    {
        public VkmPlaybackState Status
        {
            get
            {
                if (MediaPlayerService.instance != null) {
                    return MediaPlayerService.instance.Status;
                }
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
    }
}