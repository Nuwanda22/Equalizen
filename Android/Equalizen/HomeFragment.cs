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
        private FloatingActionButton addAllButton;
        private FloatingActionButton selectButton;
        private FloatingActionMenu menu;
        private ListView listView;
        private LocalMusicAdapter adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            menu = view.FindViewById<FloatingActionMenu>(Resource.Id.fabMenu);
            addAllButton = view.FindViewById<FloatingActionButton>(Resource.Id.all_all_button);
            selectButton = view.FindViewById<FloatingActionButton>(Resource.Id.select_button);
            listView = view.FindViewById<ListView>(Resource.Id.list);

            adapter = new LocalMusicAdapter(Activity, new List<LocalMusic>());

            listView.Adapter = adapter;
            
            addAllButton.Click += AddAllButton_Click;
            selectButton.Click += SelectButton_Click;
            listView.ItemClick += ListView_ItemClick;

            return view;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var text = e.Parent.GetItemAtPosition(e.Position).ToString();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            adapter.Add(new LocalMusic { Artist = "Red Velvet", Title = "Rookie" });
            adapter.NotifyDataSetChanged();

            Toast.MakeText(Activity, "Select", ToastLength.Short).Show();
        }

        private void AddAllButton_Click(object sender, EventArgs e)
        {
            var finder = new LocalMusicFinder();

            var result = finder.FindMusics(Activity.ContentResolver);
            
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("확인");
            builder.SetMessage($"총 {result.Count()}개의 음악이 추가됩니다. 추가하시겠습니까?");
            builder.SetPositiveButton("확인", (s, events) =>
            {
                // TODO: No duplication
                adapter.AddAll(result.ToArray());
                adapter.NotifyDataSetChanged();
                menu.Close(true);
            });
            builder.SetNegativeButton("취소", (s, events) => 
            {
                menu.Close(true);
            });

            var dialog = builder.Create();
            dialog.Show();
        }
    }
}