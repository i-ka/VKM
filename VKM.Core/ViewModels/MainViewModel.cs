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
                _playerService.SetPlayList(_audioList);
                RaisePropertyChanged(() => AudioList);
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
            get { return new MvxCommand<Audio>((audio) => _playerService.Goto(_audioList.IndexOf(audio))); }
        }

        public MvxCommand PlayCommand
        {
            get { return new MvxCommand(() => _playerService.Start()); }
        }
        public MvxCommand PauseCommand
        {
            get { return new MvxCommand(() => _playerService.Pause()); }
        }
        public MvxCommand NextCommand
        {
            get { return new MvxCommand(() => _playerService.Next()); }
        }
        public MvxCommand PrevCommand
        {
            get { return new MvxCommand(() => _playerService.Prev()); }
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
