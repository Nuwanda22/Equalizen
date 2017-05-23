using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Fragment = Android.Support.V4.App.Fragment;
using FloatingActionButton = Clans.Fab.FloatingActionButton;
using FloatingActionMenu = Clans.Fab.FloatingActionMenu;

namespace Equalizen
{
    public class HomeFragment : Fragment
    {
        private FloatingActionButton AddButton;
        private FloatingActionButton DeleteButton;
        private FloatingActionMenu Menu;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.home_fragment, container, false);
        }
    }
}