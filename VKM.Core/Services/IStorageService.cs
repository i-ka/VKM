using System;

namespace VKM.Core.Services
{
    public enum AudioSorting
    {
        Date,
        Duration,
        None
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