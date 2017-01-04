using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    interface IRestApiService
    {
        void MakeRequest<T>(string url, string verb, string postData, Action<T> successAction, Action<Exception> errAction);
    }
}
