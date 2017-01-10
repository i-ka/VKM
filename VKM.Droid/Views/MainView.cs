using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using VKM.Core.ViewModels;
using VKM.Droid.Services;
using SearchView = Android.Support.V7.Widget.SearchView;

namespace VKM.Droid.Views
{
    [Activity(Label = "Мой плейлист",
        Theme = "@style/Theme.AppCompat")]
    internal class MainView : MvxAppCompatActivity<MainViewModel>, SearchView.IOnQueryTextListener,
        SeekBar.IOnSeekBarChangeListener

    {
        private MediaPlayerService _player;

        private SeekBar _seekBar;
        private bool _seekPressed;

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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
            MediaPlayerService.OnInstanceCreated += SetupPlayer;
            _seekBar = FindViewById<SeekBar>(Resource.Id.seekBar);
            _seekBar.SetOnSeekBarChangeListener(this);
            if (MediaPlayerService.instance != null)
                SetupPlayer(MediaPlayerService.instance);
        }

        private void SetupPlayer(MediaPlayerService instance)
        {
            _player = instance;
            _player.OnNext += () => ViewModel?.NextCommand.Execute();
            _player.OnPrev += () => ViewModel?.PrevCommand.Execute();
            _player.OnError += OnPlayerError;
            _player.PlaybackStateChanged += state => ViewModel?.PlayerStateChanged(state);
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
            switch (item.ItemId)
            {
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

        private void OnPlayerPositionChanged(long pos)
        {
            Console.WriteLine("Current position: " + pos);
            _seekBar.Progress = (int) pos;
        }

        private void OnDurationChanged(long duration)
        {
            Console.WriteLine("Current playback duration is:" + duration);
            if (!_seekPressed)
                _seekBar.Max = (int) duration;
        }
    }
}