using System.Net;
using MvvmCross.Core.ViewModels;
using VKM.Core.Services;
using MvvmCross.Platform;

namespace VKM.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private IVkAudioService _vkmService;

        public FirstViewModel(IVkAudioService vkAudioService)
        {
            _vkmService = vkAudioService;
            _vkmService.Login(null, null, () => ShowViewModel<MainViewModel>(), e =>
            {
                var webError = e as WebException;
                if (webError == null) return;
                if (((HttpWebResponse) webError.Response).StatusCode == HttpStatusCode.Unauthorized)
                {
                    Mvx.Error("Incorrect login or password");
                }
            });
        }

        private string _username;
        public string Username
        { 
            get { return _username; }
            set { SetProperty (ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value);
}
        }

        private MvxCommand _loginButtonCommand;
        public MvxCommand LoginButtonCommand
        {
            get
            {
                if (_loginButtonCommand == null) {
                    _loginButtonCommand = new MvxCommand(OnLoginButtonPressed);
                }
                return _loginButtonCommand;
            }
        }
        private void OnLoginButtonPressed()
        {
            _vkmService.Login(Username, Password, ()=> ShowViewModel<MainViewModel>(), e =>
            {
                var webError = e as WebException;
                if (webError == null) return;
                if (((HttpWebResponse)webError.Response).StatusCode == HttpStatusCode.Unauthorized) {
                    Mvx.Error("Incorrect login or password");
                }
            });
            //ShowViewModel<MainViewModel>();
        }

    }
}
