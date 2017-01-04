using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace VKM.Core.Services
{
    class RestApiService :IRestApiService
    {
        private readonly IMvxJsonConverter _jsonConverter;
        public RestApiService(IMvxJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
        }

        public void MakeRequest<T>(string url, string verb, string postData, Action<T> successAction,
            Action<Exception> errAction)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = verb;
            request.Accept = "application/json";
            if (postData != "" && verb == "POST") {
                //Mvx.Warning(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.BeginGetRequestStream((tocken) =>
                {
                    using (var reqStr = request.EndGetRequestStream(tocken)) {
                        var writer = new StreamWriter(reqStr);
                        writer.Write(postData, 0, postData.Length);
                        writer.Flush();
                        BeginRequest(request, successAction, errAction);
                    }
                }, null);
                return;
            }
            BeginRequest(request, successAction, errAction);
        }

        private void BeginRequest<T>(HttpWebRequest request, Action<T> successAction, Action<Exception> errAction)
        {
            MakeRequest(request,
                response =>
                {
                    T retVal;
                    try {
                        retVal = Desirealize<T>(response);
                    }
                    catch (Exception ex) {
                        errAction(ex);
                        return;
                    }
                    successAction?.Invoke(retVal);
                },
                error => errAction?.Invoke(error));
        }

        private void MakeRequest(HttpWebRequest request, Action<string> successAction, Action<Exception> errAction)
        {
            request.BeginGetResponse(token =>
            {
                try {
                    using (var response = request.EndGetResponse(token)) {
                        using (var stream = response.GetResponseStream()) {
                            var reader = new StreamReader(stream);
                            successAction(reader.ReadToEnd());
                        }
                    }
                }
                catch (Exception e) {
                    Mvx.Error("Error: '{0}' when making {1} request to {2}", e.Message, request.Method, request.RequestUri);
                    errAction(e);
                }
            }, null);
        }

        private T Desirealize<T>(string response)
        {
            var result = _jsonConverter.DeserializeObject<T>(response);
            return result;
        }
    }
}
