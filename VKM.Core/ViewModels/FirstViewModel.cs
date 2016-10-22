using MvvmCross.Core.ViewModels;

namespace VKM.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
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

    }
}
