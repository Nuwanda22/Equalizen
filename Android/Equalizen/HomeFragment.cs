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

namespace Equalizen
{
    public class HomeFragment : Fragment
    {
        private FloatingActionButton addAllButton;
        private FloatingActionButton selectButton;
        private ListView listView;
        private ArrayAdapter adapter;

        List<Tuple<string, string>> items = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("KNOCK KNOCK", "TWICE"),
            new Tuple<string, string>("TT", "TWICE"),
            new Tuple<string, string>("Very Very Very", "I.O.I")
        };
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            addAllButton = view.FindViewById<FloatingActionButton>(Resource.Id.all_all_button);
            selectButton = view.FindViewById<FloatingActionButton>(Resource.Id.select_button);
            listView = view.FindViewById<ListView>(Resource.Id.list);

            adapter = new SimpleListItem2Adapter(Activity, items);

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
            items.Add(new Tuple<string, string>("Rookie", "Red Velvet"));
            adapter.NotifyDataSetChanged();

            Toast.MakeText(Activity, "Select", ToastLength.Short).Show();
        }

        private void AddAllButton_Click(object sender, EventArgs e)
        {
            Toast.MakeText(Activity, "Add all", ToastLength.Short).Show();
        }
    }

    public class SimpleListItem2Adapter : ArrayAdapter<Tuple<string, string>>
    {
        Activity context;
        public SimpleListItem2Adapter(Activity context, IList<Tuple<string, string>> objects)
            : base(context, Android.Resource.Id.Text1, objects)
        {
            this.context = context;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            var item = GetItem(position);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Item1;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.Item2;

            return view;
        }
    }
}