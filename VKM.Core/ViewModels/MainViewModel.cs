using System;
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
        private readonly IPlayerService _playerService;
        private readonly IVkAudioService _vkAudioService;
        public MainViewModel(IVkAudioService vkAudioService, IPlayerService playerService)
        {
            _playerService = playerService;
            _vkAudioService = vkAudioService;
            _vkAudioService.GetMyPlaylist((list) => AudioList = list, null);
        }
        private List<Audio> _audioList;
        public List<Audio> AudioList
        {
            get { return _audioList; }
            set
            {
                _audioList = value;
                //CurrentAudio = _audioList.First();
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
        public MvxCommand OptionsButtonCommand => _optionsButtonCommand ?? (_optionsButtonCommand = new MvxCommand(OnOptionsButtonClicked));

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
                        return;
                    }
                    CurrentAudio = audio;
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
                    StartPlayer();
                });
            }
        }
        public MvxCommand PauseCommand => new MvxCommand(PausePlayer);

        public MvxCommand NextCommand
        {
            get {
                return new MvxCommand(() => {
                    var prState = _playerService.Status;
                    var newidx = _audioList.IndexOf(CurrentAudio) + 1;
                    if (newidx >= _audioList.Count) {
                        newidx = 0;
                    }
                    CurrentAudio = _audioList[newidx];
                    if (prState == VkmPlaybackState.Playing || prState == VkmPlaybackState.Preparing) {
                        StartPlayer();
                    }
                });
            }
        }
        public MvxCommand PrevCommand
        {
            get {
                return new MvxCommand(() => {
                    var prState = _playerService.Status;
                    var newidx = _audioList.IndexOf(CurrentAudio) - 1;
                    if (newidx < 0) {
                        newidx = _audioList.Count - 1;
                    }
                    CurrentAudio = _audioList[newidx];
                    if (prState == VkmPlaybackState.Playing || prState == VkmPlaybackState.Preparing) {
                        StartPlayer();
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

        public void PlayerStateChanged(VkmPlaybackState state)
        {
            switch (state) {
                case VkmPlaybackState.Playing:
                    CurrentAudio.IsPlaying = true;
                    break;
                case VkmPlaybackState.Paused:
                case VkmPlaybackState.Stoped:
                case VkmPlaybackState.Preparing:
                    CurrentAudio.IsPlaying = false;
                    break;
            }
        }

        public MvxCommand<string> SearchCommand
        {
            get { return new MvxCommand<string>((term) =>
            {
                _vkAudioService.Search(term, (result)=>
                {
                    AudioList = result;
                }, null);
            });}
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
