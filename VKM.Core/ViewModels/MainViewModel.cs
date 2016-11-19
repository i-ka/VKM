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
        public MainViewModel(IVkAudioService service)
        {
            AudioList = service.GetAudioList();
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

        public void SetPlayerService(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
