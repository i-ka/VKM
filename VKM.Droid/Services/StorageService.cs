using System;
using System.Globalization;
using Android.App;
using Android.Content;
using VKM.Core.Services;

namespace VKM.Droid.Services
{
    internal class StorageService : IStorageService
    {
        private readonly ISharedPreferences _preferences;

        public StorageService()
        {
            _preferences = Application.Context.GetSharedPreferences(Application.Context.PackageName + ".Storage",
                FileCreationMode.Private);
        }

        public string OAuthToken
        {
            get { return GetValue("VkmOAuthToken"); }
            set { AddValue("VkmOAuthToken", value); }
        }

        public string RefreshToken
        {
            get { return GetValue("VkmRefreshToken"); }
            set { AddValue("VkmRefreshToken", value); }
        }

        public DateTime TokenExpireTime
        {
            get
            {
                var result = GetValue("VkmTokenExpireTime");
                return result != null ? DateTime.Parse(result) : new DateTime();
            }
            set { AddValue("VkmTokenExpireTime", value.ToString(CultureInfo.InvariantCulture)); }
        }

        public AudioSorting AudioSorting
        {
            get
            {
                var sortType = GetValue("VkmSorting");
                if (sortType != null)
                    return (AudioSorting) Enum.Parse(typeof(AudioSorting), GetValue("VkmSorting"));
                return AudioSorting.None;
            }
            set { AddValue("VkmSorting", value.ToString()); }
        }

        public bool FiltersActive
        {
            get { return GetValue("VkmFiltersActive") == "True"; }
            set { AddValue("VkmFiltersActive", value.ToString()); }
        }

        public string FilterString
        {
            get { return GetValue("VkmFilterString"); }
            set { AddValue("VkmFilterString", value); }
        }

        public void Clear()
        {
            _preferences.Edit().Clear().Commit();
        }

        private void AddValue(string key, string value)
        {
            var editor = _preferences.Edit();
            editor.PutString(key, value);
            editor.Commit();
        }

        private string GetValue(string key)
        {
            try
            {
                return _preferences.GetString(key, null);
            }
            catch
            {
                return null;
            }
        }

        private void Remove(string key)
        {
            if (!_preferences.Contains(key)) return;
            var editor = _preferences.Edit();
            editor.Remove(key);
            editor.Commit();
        }
    }
}