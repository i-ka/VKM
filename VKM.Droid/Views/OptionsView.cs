using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MvvmCross.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace VKM.Droid.Views
{
    [Activity(Label = "Options view",
        Theme = "@style/Theme.AppCompat")]
    class OptionsView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OptionsView);
        }
    }
}