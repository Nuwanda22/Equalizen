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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change main_compat_menu
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_edit:
                    // TODO: open the edit mode
                    
                    break;

                case Resource.Id.action_remove_all:
                    // TODO: clear list and delete saved data

                    break;
            }
            return base.OnOptionsItemSelected(item);
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

