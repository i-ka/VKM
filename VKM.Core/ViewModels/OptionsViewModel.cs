using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using VKM.Core.Services;

namespace VKM.Core.ViewModels
{
    internal class OptionsViewModel :
        MvxViewModel
    {
        private List<AudioSorting> _list = new List<AudioSorting>
        {
            AudioSorting.Date,
            AudioSorting.Duration,
            AudioSorting.None
        };

        private readonly IPlayerService _player;
        private readonly IStorageService _srtorage;

        public OptionsViewModel(IStorageService storage, IPlayerService player)
        {
            _srtorage = storage;
            _player = player;
        }

        public List<AudioSorting> List
        {
            get { return _list; }
            set
            {
                _list = value;
                RaisePropertyChanged(() => List);
            }
        }

        public bool FiterActive
        {
            get { return _srtorage.FiltersActive; }
            set
            {
                _srtorage.FiltersActive = value;
                RaisePropertyChanged(() => FiterActive);
            }
        }

        public string FilterString
        {
            get { return _srtorage.FilterString; }
            set
            {
                _srtorage.FilterString = value;
                RaisePropertyChanged(() => FilterString);
            }
        }

        public AudioSorting CurrentAudioSorting
        {
            get { return _srtorage.AudioSorting; }
            set
            {
                _srtorage.AudioSorting = value;
                RaisePropertyChanged(() => CurrentAudioSorting);
            }
        }

        public MvxCommand LogOutCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    _srtorage.Clear();
                    _player.Stop();
                    ShowViewModel<FirstViewModel>();
                });
            }
        }
    }
}