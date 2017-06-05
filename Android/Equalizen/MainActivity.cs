using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;

using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Equalizen
{
    [Activity(Label = "Equalizen", MainLauncher = true, Theme = "@style/MyTheme")]
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

            // TODO: add ... button

            // Set Status Bar
            Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            
            #endregion

            if (bundle == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragment, new HomeFragment()).Commit();
            }

            SupportFragmentManager.BackStackChanged += SupportFragmentManager_BackStackChanged;

            toolbar.NavigationClick += (s, ev) =>
            {
                OnBackPressed();
            };
        }

        private void SupportFragmentManager_BackStackChanged(object sender, EventArgs e)
        {
            if (SupportFragmentManager.BackStackEntryCount > 0)
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }
            else
            {
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            }
        }
    }
}

