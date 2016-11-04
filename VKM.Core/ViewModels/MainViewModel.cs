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
        public MainViewModel(IVkAudioService vkAudioService)
        {
            AudioList = vkAudioService.GetAudioList();
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
        public MvxCommand PlayButtonCommand
        {
            get { return new MvxCommand(() => _playerService.Play()); }
        }

        public MvxCommand SetSourceCommand
        {
            get { return new MvxCommand(() => _playerService.Source = "https://cs3-3v4.vk-cdn.net/p14/ff3562cc4d0293.mp3?extra=oUmFJBJ8YOq2zHJSjE70RFMeVi6hoMZZgZ9a6vUDeU4nLfE0yILwJZr5CwXRNXjBLFEtgdRMEnX7QackInZ5KQLBFOY5D5Cx6JmTkvzpDnHW37rEB6KbXcNL_dn4mC1VkWXJCSqb1xBW"); }
        }

        private IPlayerService _playerService;
        public IPlayerService Payer
        {
            set { _playerService = value; }
        }

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
