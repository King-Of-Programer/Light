using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace FlashlightTool.Droid
{
    [Activity(Label = "FlashlightTool", Icon = "@mipmap/icon", Theme = "@style/MainTheme.NoDisplay", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        bool isFlashLight =false;
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            checkFlashLightState();
            if (PermissionStatus.Granted == await isCameraPermission())
            {
                if (isFlashLight)
                {
                    await Xamarin.Essentials.Flashlight.TurnOffAsync();
                    isFlashLight = false;
                    App.Current.Properties["flashState"] = isFlashLight;
                }
                else
                {
                    isFlashLight = true;
                    App.Current.Properties["flashState"] = isFlashLight;
                    await Xamarin.Essentials.Flashlight.TurnOnAsync();
                }
            }
            else
            {
                var status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            Finish();
        }
        public void checkFlashLightState()
        {
            object flashState = false;
            App.Current.Properties.TryGetValue("flashState", out flashState);
            if (flashState != null)
            {
                isFlashLight = (bool)flashState;
            }
            else
            {
                App.Current.Properties.Add("flashState", isFlashLight);
            }
        }
        async Task<PermissionStatus> isCameraPermission()
        {
            return await Permissions.CheckStatusAsync<Permissions.Camera>();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}