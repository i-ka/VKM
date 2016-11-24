﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using VKM.Core.Services;


namespace VKM.Core.ViewModels
{
    public class MainViewModel :
        MvxViewModel
    {
        private IPlayerService _playerService;
        public MainViewModel(IVkAudioService service, IPlayerService plService)
        {
            _playerService = plService;
            AudioList = service.GetAudioList();
        }
        private List<Audio> _audioList;
        public List<Audio> AudioList
        {
            get { return _audioList; }
            set
            {
                _audioList = value;
                CurrentAudio = _audioList.First();
                RaisePropertyChanged(() => AudioList);
            }
        }

        private Audio _currentAudio;
        public Audio CurrentAudio
        {
            get { return _currentAudio; }
            private set
            {
                if (_currentAudio == value) {
                    return;
                }
                if (_currentAudio != null) {
                    _currentAudio.IsPlaying = false;
                }
                _currentAudio = value;
                StopPlayer();
                _playerService.SetSource(_currentAudio);
                RaisePropertyChanged(() => CurrentAudio);
            }
        }

        private MvxCommand _optionsButtonCommand;
        public MvxCommand OptionsButtonCommand
        {
            get
            {
                if (_optionsButtonCommand == null) {
                    _optionsButtonCommand = new MvxCommand(OnOptionsButtonClicked);
                }
                return _optionsButtonCommand;
            }
        }
        public MvxCommand<Audio> SelectAudio
        {
            get {
                return new MvxCommand<Audio>((audio) => {
                    if (CurrentAudio == audio) {
                        if (CurrentAudio.IsPlaying) {
                            PausePlayer();
                        } else {
                            StartPlayer();
                        }
                        CurrentAudio.IsPlaying = !CurrentAudio.IsPlaying;
                        return;
                    }
                    CurrentAudio = audio;
                    CurrentAudio.IsPlaying = true;
                    StartPlayer();
                });
            }
        }

        public MvxCommand PlayCommand
        {
            get {
                return new MvxCommand(() => {
                    if (CurrentAudio == null) {
                        CurrentAudio = _audioList[0];
                    }
                    CurrentAudio.IsPlaying = true;
                    StartPlayer();
                });
            }
        }
        public MvxCommand PauseCommand
        {
            get {
                return new MvxCommand(() => {
                    CurrentAudio.IsPlaying = false;
                    PausePlayer();
                });
            }
        }
        public MvxCommand NextCommand
        {
            get {
                return new MvxCommand(() => {
                    VkmPlaybackState prState = _playerService.Status;
                    int newidx = _audioList.IndexOf(CurrentAudio) + 1;
                    if (newidx >= _audioList.Count) {
                        newidx = 0;
                    }
                    CurrentAudio = _audioList[newidx];
                    if (prState == VkmPlaybackState.Playing || prState == VkmPlaybackState.Preparing) {
                        StartPlayer();
                        CurrentAudio.IsPlaying = true;
                    }
                });
            }
        }
        public MvxCommand PrevCommand
        {
            get {
                return new MvxCommand(() => {
                    VkmPlaybackState prState = _playerService.Status;
                    int newidx = _audioList.IndexOf(CurrentAudio) - 1;
                    if (newidx < 0) {
                        newidx = _audioList.Count - 1;
                    }
                    CurrentAudio = _audioList[newidx];
                    if (prState == VkmPlaybackState.Playing || prState == VkmPlaybackState.Preparing) {
                        StartPlayer();
                        CurrentAudio.IsPlaying = true;
                    }
                });
            }
        }

        private void StopPlayer()
        {
            if (_playerService.Status != VkmPlaybackState.NoMedia) {
                _playerService.Stop();
            }
        }

        private void StartPlayer()
        {
            _playerService.Start();
        }

        private void PausePlayer()
        {
            if (_playerService.Status != VkmPlaybackState.NoMedia) {
                _playerService.Pause();
            }
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
