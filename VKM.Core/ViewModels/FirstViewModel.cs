using System;
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
            Username = "";
            Password = "";
            IsLoading = true;
            _vkmService.Login(null, null, OnLoginSuccess, (e) => IsLoading = false);
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
            set
            { SetProperty(ref _password, value); }
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

        private string _errorText;

        public string ErrorText
        {
            get { return _errorText; }
            private set
            {
                if(_errorText == value) return;
                _errorText = value;
                RaisePropertyChanged(() => ErrorText);
            }
        }

        private bool _showError;

        public bool ShowError
        {
            get { return _showError; }
            private set
            {
                if (_showError == value) return;
                _showError = value;
                RaisePropertyChanged(() => ShowError);
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
            IsLoading = true;
            ShowError = false;
            _vkmService.Login(Username, Password, ()=> ShowViewModel<MainViewModel>(), OnLoginError);
        }

        private void OnLoginError(Exception error)
        {
            var webError = error as WebException;
            if (webError == null) return;
            if (((HttpWebResponse) webError.Response).StatusCode == HttpStatusCode.Unauthorized)
            {
                ErrorText = "Incorrect login or password";
            }
            else
            {
                ErrorText = "Connection error";
            }
            ShowError = true;
            IsLoading = false;
        }

        private void OnLoginSuccess()
        {
            IsLoading = false;
            ShowViewModel<MainViewModel>();
        }
    }
}
