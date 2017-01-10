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
            GetMyPlayList();
        }
        private List<Audio> _audioList;
        public List<Audio> AudioList
        {
            get { return _audioList; }
            set
            {
                _audioList = value;
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

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            private set
            {
                if (_isLoading == value) return;
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        private bool _showMessage;
        public bool ShowMessage
        {
            get { return _showMessage; }
            private set
            {
                _showMessage = value;
                RaisePropertyChanged(() => ShowMessage);
            }
        }

        private bool _showPlayer = false;

        public bool ShowPlayer
        {
            get { return _showPlayer; }
            private set
            {
                _showPlayer = value;
                RaisePropertyChanged(() => ShowPlayer);
            }
        }

        private string _message;

        public string Message
        {
            get { return _message;}
            private set
            {
                _message = value;
                RaisePropertyChanged(() => Message);
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
        public MvxCommand PlayPauseCommand => new MvxCommand(() =>
        {
            if (CurrentAudio != null && CurrentAudio.IsPlaying)
            {
                PauseCommand.Execute();
            } else
            {
                PlayCommand.Execute();
            }
        });
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

        public MvxCommand<int> SeekCommand => new MvxCommand<int>((pos)=>{
            _playerService.Seek(pos);
            });

        private void StopPlayer()
        {
            if (_playerService.Status != VkmPlaybackState.NoMedia) {
                ShowPlayer = false;
                _playerService.Stop();
            }
        }

        private void StartPlayer()
        {
            ShowPlayer = true;
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

        public MvxCommand GetMyAudioCommand => new MvxCommand(GetMyPlayList);

        public MvxCommand<string> SearchCommand
        {
            get { return new MvxCommand<string>((term) =>
            {
                ShowMessage = false;
                IsLoading = true;
                _vkAudioService.Search(term, OnLoadingSuccess, OnLoadingError);
            });}
        }

        private void OnLoadingSuccess(List<Audio> resultAudios)
        {
            AudioList = resultAudios;
            IsLoading = false;
            if (AudioList.Count == 0)
            {
                ShowMessage = true;
                Message = "Nothing found";
            }
        }

        private void OnLoadingError(Exception error)
        {
            IsLoading = false;
            ShowMessage = true;
            Message = "Network error";
        }

        private void GetMyPlayList()
        {
            ShowMessage = false;
            IsLoading = true;
            _vkAudioService.GetMyPlaylist(OnLoadingSuccess, OnLoadingError);
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
