using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public interface IVkAudioService
    {
        void Search(string searchTerm, Action<List<Audio>> succesAction, Action<Exception> errorAction);
        void Login(string login, string password, Action authSuccessAction, Action<Exception> authFailureAction);
        void GetMyPlaylist(Action<List<Audio>> succesAction, Action<Exception> errorAction);
        void MakeRequest<T>(string url, string verb, string postData, Action<T> successAction, Action<Exception> errAction);
    }
}
