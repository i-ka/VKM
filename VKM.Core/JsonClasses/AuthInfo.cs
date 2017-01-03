using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace VKM.Core.JsonClasses
{
    public class AuthInfo
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string error { get; set; }
    }
}
