using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKM.Core.Services
{
    public enum AudioSorting
    {
        Date, Duration
    }
    public interface IStorageService
    {
        string OAuthToken { get; set; }
        string RefreshToken { get; set; }
        DateTime TokenExpireTime { get; set; }
        AudioSorting AudioSorting { get; set; }
        bool FiltersActive { get; set; }
        string FilterString { get; set; }
        void Clear();
    }
}
