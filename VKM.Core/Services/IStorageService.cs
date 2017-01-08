using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public interface IStorageService
    {
        string OAuthToken { get; set; }
        string RefreshToken { get; set; }
        DateTime TokenExpireTime { get; set; }
    }
}
