using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKM.Core.ViewModels;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using Android.Support.V7.Widget;

using VKM.Droid.Services;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace VKM.Droid.Views
{
    [Activity(Label = "Main view",
        Theme = "@style/AppStyle")]
    class MainView : MvxAppCompatActivity<MainViewModel>, SearchView.IOnQueryTextListener

    {
        private MediaPlayerService _player;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            MediaPlayerService.OnInstanceCreated += SetupPlayer;
        }

        void SetupPlayer(MediaPlayerService instance)
        {
            _player = instance;
            _player.OnNext += () => (ViewModel as MainViewModel)?.NextCommand.Execute();
            _player.OnPrev += () => (ViewModel as MainViewModel)?.PrevCommand.Execute();
            _player.OnError += OnPlayerError;
            _player.PlaybackStateChanged += (state) => (ViewModel as MainViewModel)?.PlayerStateChanged(state);
            _player.PlaybackPositionChanged += OnPlayerPositionChanged;
            _player.DurationChanged += OnDurationChanged;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionButtons, menu);
            var search = (Android.Support.V7.Widget.SearchView) menu.FindItem(Resource.Id.search).ActionView;
            search.SetOnQueryTextListener((SearchView.IOnQueryTextListener)this);
            return true;
            //return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {
                case Resource.Id.settings:
                    (ViewModel as MainViewModel)?.OptionsButtonCommand.Execute();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void OnPlayerError()
        {
            Console.WriteLine("Error");
            (ViewModel as MainViewModel)?.NextCommand.Execute();
        }

        void OnPlayerPositionChanged(long pos)
        {
            Console.WriteLine("Current position: " + pos.ToString());
        }

        void OnDurationChanged(long duration)
        {
            Console.WriteLine("Current playback duration is:" + duration.ToString());
        }

        public bool OnQueryTextChange(string p0)
        {
            return true;
        }

        public bool OnQueryTextSubmit(string p0)
        {
            ViewModel.SearchCommand.Execute(p0);
            
            return true;
        }
    }
}