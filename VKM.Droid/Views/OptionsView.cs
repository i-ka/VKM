using Android.App;
using Android.OS;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace VKM.Droid.Views
{
    [Activity(Label = "Options",
        Theme = "@style/Theme.AppCompat")]
    internal class OptionsView : MvxAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.OptionsView);
        }
    }
}