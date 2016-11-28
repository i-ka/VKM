using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKM.Core.ViewModels;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;

using VKM.Core.ViewModels;
using VKM.Droid.Services;

namespace VKM.Droid.Views
{
    [Activity(Label = "Main view",
        Theme = "@android:style/Theme.Holo")]
    class MainView : MvxActivity
    {
        MediaPlayerService player;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            MediaPlayerService.OnInstanceCreated += SetupPlayer;
        }

        void SetupPlayer(MediaPlayerService instance)
        {
            player = instance;
            player.OnCompletion += () => (ViewModel as MainViewModel).NextCommand.Execute();
            player.PlaybackPositionChanged += OnPlayerPositionChanged;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionButtons, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
            case Resource.Id.settings:
                (ViewModel as MainViewModel).OptionsButtonCommand.Execute(null);
                return true;
            default:
                return base.OnOptionsItemSelected(item);
            }
        }

        void OnPlayerPositionChanged(long pos)
        {
            Console.WriteLine("Current position: " + pos.ToString());
        }
    }
}