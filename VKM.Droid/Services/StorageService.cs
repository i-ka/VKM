using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using VKM.Core.Services;

namespace VKM.Droid.Services
{
    class StorageService : IStorageService
    {
        private ISharedPreferences _preferences;

        public StorageService()
        {
            _preferences = Application.Context.GetSharedPreferences(Application.Context.PackageName + ".Storage",
                FileCreationMode.Private);
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
            get { return (AudioSorting)Enum.Parse(typeof(AudioSorting), GetValue("VkmSorting")); }
            set
            {
                AddValue("VkmSorting", value.ToString());
            }
        }

        public bool FiltersActive
        {
            get { return GetValue("VkmFiltersActive") == "true"; }
            set
            {
                AddValue("VkmFiltersActive", value.ToString());
            }
        }

        public string FilterString
        {
            get { return GetValue("VkmFilterString"); }
            set
            {
                AddValue("VkmFilterString", value);
            }
        }

        public void Clear()
        {
            _preferences.Edit().Clear().Commit();
        }
    }
}