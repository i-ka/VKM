using System;

namespace VKM.Core.Services
{
    internal interface IRestApiService
    {
        void MakeRequest<T>(string url, string verb, string postData, Action<T> successAction,
            Action<Exception> errAction);
    }
}