using Android.App;
using Android.OS;
using MvvmCross.Droid.Views;

namespace VKM.Droid.Views
{
    [Activity(Label = "View for FirstViewModel",
        Theme = "@android:style/Theme.Material.NoActionBar")]
    public class FirstView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
        }
    }
}
