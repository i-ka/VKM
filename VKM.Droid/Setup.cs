using Android.Content;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using VKM.Droid.Services;
using VKM.Core.Services;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Support.V7.AppCompat.Target;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;

namespace VKM.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
            Mvx.RegisterSingleton<IPlayerService>(() => new DroidPlayerService());
            Mvx.RegisterSingleton<IStorageService>(() => new StorageService());
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            //MvxAppCompatSetupHelper.FillTargetFactories(registry);
            base.FillTargetFactories(registry);
        }
    }
}
