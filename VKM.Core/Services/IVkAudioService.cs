using System;
using System.Collections.Generic;

namespace VKM.Core.Services
{
    public interface IVkAudioService
    {
        void Search(string searchTerm, Action<List<Audio>> succesAction, Action<Exception> errorAction);

        void Login(string login = null, string password = null, Action authSuccessAction = null,
            Action<Exception> authFailureAction = null);

        void GetMyPlaylist(Action<List<Audio>> succesAction, Action<Exception> errorAction);
    }
}