using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using VKM.Core.Services;


namespace VKM.Core.ViewModels
{
    class MainViewModel :
        MvxViewModel
    {
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

        private void OnOptionsButtonClicked()
        {
            ShowViewModel<OptionsViewModel>();
        }
    }
}
