using System;

using Android.App;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Widget;
using Android.Content;

using Android.Support.V7.App;

namespace Equalizen
{
    [Activity(Label = "Equalizen", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        MediaPlayer mediaPlayer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main_activity);

            #region AppCompat

            // Set Toolber
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Equalizen";

            // Set Status Bar
            Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            
            #endregion

            if (bundle == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragment, new HomeFragment()).Commit();
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(PlayerActivity));
        }

        public static readonly int PickImageId = 1000;
        private void AddButton_Click(object sender, EventArgs e)
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

