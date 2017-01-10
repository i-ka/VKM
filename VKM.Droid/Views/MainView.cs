using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
    [Activity(Label = "Мой плейлист",
        Theme = "@style/Theme.AppCompat")]
    class MainView : MvxAppCompatActivity<MainViewModel>, SearchView.IOnQueryTextListener, SeekBar.IOnSeekBarChangeListener

    {
        private MediaPlayerService _player;

        private SeekBar _seekBar;
        private bool _seekPressed = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            MediaPlayerService.OnInstanceCreated += SetupPlayer;
            _seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
            _seekBar.SetOnSeekBarChangeListener(this);
            if (MediaPlayerService.instance != null)
            {
                SetupPlayer(MediaPlayerService.instance);
            }
        }

        void SetupPlayer(MediaPlayerService instance)
        {
            _player = instance;
            _player.OnNext += () => ViewModel?.NextCommand.Execute();
            _player.OnPrev += () => ViewModel?.PrevCommand.Execute();
            _player.OnError += OnPlayerError;
            _player.PlaybackStateChanged += (state) => ViewModel?.PlayerStateChanged(state);
            _player.PlaybackPositionChanged += OnPlayerPositionChanged;
            _player.DurationChanged += OnDurationChanged;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionButtons, menu);
            var search = (SearchView) menu.FindItem(Resource.Id.search).ActionView;
            search.SetOnQueryTextListener(this);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId) {
                case Resource.Id.settings:
                    ViewModel?.OptionsButtonCommand.Execute();
                    return true;
                case Resource.Id.home:
                    ViewModel?.GetMyAudioCommand.Execute();
                    Title = "Мой плейлист";
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void OnPlayerError()
        {
            Console.WriteLine("Error");
            ViewModel?.NextCommand.Execute();
        }

        void OnPlayerPositionChanged(long pos)
        {
            Console.WriteLine("Current position: " + pos.ToString());
            _seekBar.Progress = (int) pos;
        }

        void OnDurationChanged(long duration)
        {
            Console.WriteLine("Current playback duration is:" + duration.ToString());
            if (!_seekPressed)
            {
                _seekBar.Max = (int)duration;
            }
        }

        public bool OnQueryTextChange(string p0)
        {
            return true;
        }

        public bool OnQueryTextSubmit(string p0)
        {
            ViewModel?.SearchCommand.Execute(p0);
            Title = "Поиск";
            return true;
        }

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            //throw new NotImplementedException();
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            _seekPressed = true;
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            ViewModel.SeekCommand.Execute(seekBar.Progress);
            _seekPressed = false;
        }
    }
}