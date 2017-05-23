using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Equalizen
{
    [Activity(Label = "Equalizen", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_activity);

            #region AppCompat

            // Set Toolber
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Equalizen";

            // Set Status Bar
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            
            #endregion

            if (bundle == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragment, new HomeFragment()).Commit();
            }
        }

        public static readonly int PickImageId = 1000;

        private void ShowFileChooser()
        {
            Intent = new Intent();
            Intent.SetType("audio/*");
            Intent.SetAction(Intent.ActionGetContent);

            StartActivityForResult(Intent.CreateChooser(Intent, "Select Music"), PickImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                var uri = data.Data;
            }
        }
    }
}

