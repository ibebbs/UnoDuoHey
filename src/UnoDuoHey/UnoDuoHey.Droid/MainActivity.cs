using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Views;
using Microsoft.Device.Display;

namespace UnoDuoHey.Droid
{
    [Activity(
            MainLauncher = true,
            ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize,
            WindowSoftInputMode = SoftInput.AdjustPan | SoftInput.StateHidden
        )]
    public class MainActivity : Windows.UI.Xaml.ApplicationActivity
    {
        public MainActivity()
        {
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            var deviceHelper = new DeviceHelper(this);
            deviceHelper.Initialize();

            UnoDuoHey.DeviceHelper.Instance = deviceHelper;
        }
    }
}

