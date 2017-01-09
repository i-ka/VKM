using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using VKM.Core.Services;

namespace VKM.Core.ViewModels
{
    class OptionsViewModel : 
        MvxViewModel
    {
        private IStorageService _srtorage;
        private IPlayerService _player;

        public OptionsViewModel(IStorageService storage, IPlayerService player)
        {
            _srtorage = storage;
            _player = player;
        }
        private List<AudioSorting> _list = new List<AudioSorting>()
            {
                AudioSorting.Date,
                AudioSorting.Duration,
                AudioSorting.None
            };

        public List<AudioSorting> List
        {
            get { return _list; }
            set { _list = value; RaisePropertyChanged(() => List); }
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
