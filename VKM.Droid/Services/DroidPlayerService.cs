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
        public void Goto(int idx)
        {
            throw new NotImplementedException();
        }

        public void Next()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionNext);
            Application.Context.StartService(intent);
        }

        public void Pause()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionPause);
            Application.Context.StartService(intent);
        }

        public void Prev()
        {
            var intent = new Intent(Application.Context, typeof(MediaPlayerService));
            intent.SetAction(MediaPlayerService.ActionPrev);
            Application.Context.StartService(intent);
        }

        public void SetPlayList(List<Audio> playList)
        {
            var intent = new Intent(MediaPlayerService.ActionPause);
            intent.PutStringArrayListExtra("PLAY_LIST", playList.Select(x => x.Source).ToList());
            Application.Context.StartService(intent);
        }

        public void SetSource(string newSource)
        {
            throw new NotImplementedException();
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