using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

using Fragment = Android.Support.V4.App.Fragment;
using FloatingActionButton = Clans.Fab.FloatingActionButton;
using FloatingActionMenu = Clans.Fab.FloatingActionMenu;

namespace Equalizen
{
    public class HomeFragment : Fragment
    {
        #region Components

        private FloatingActionButton addAllButton;
        private FloatingActionButton selectButton;
        private FloatingActionMenu menu;
        private ListView listView;

        #endregion

        private LocalMusicAdapter adapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            InitializeComponents(view);

            // loading saved data
            var musics = LoadData();
            adapter = new LocalMusicAdapter(Activity, musics);
            listView.Adapter = adapter;

            return view;
        }
        
        private void InitializeComponents(View view)
        {
            menu = view.FindViewById<FloatingActionMenu>(Resource.Id.fabMenu);
            addAllButton = view.FindViewById<FloatingActionButton>(Resource.Id.all_all_button);
            selectButton = view.FindViewById<FloatingActionButton>(Resource.Id.select_button);
            listView = view.FindViewById<ListView>(Resource.Id.list);

            addAllButton.Click += AddAllButton_Click;
            selectButton.Click += SelectButton_Click;
            listView.ItemClick += ListView_ItemClick;
            listView.ItemLongClick += ListView_ItemLongClick;
        }

        private List<LocalMusic> LoadData()
        {
            var data = new List<LocalMusic>();

            var pref = PreferenceManager.GetDefaultSharedPreferences(Activity);
            var json = pref.GetString("musics", null);

            if (json != null)
            {
                data = JsonConvert.DeserializeObject<List<LocalMusic>>(json);
            }
            else
            {
                data = new List<LocalMusic>();
            }

            return data;
        }

        private void SaveData()
        {
            var pref = PreferenceManager.GetDefaultSharedPreferences(Activity);
            var editor = pref.Edit();

            var musics = new List<LocalMusic>();
            for (int i = 0; i < adapter.Count; i++)
            {
                musics.Add(adapter.GetItem(i));
            }

            var json = JsonConvert.SerializeObject(musics);
            editor.PutString("musics", json);

            editor.Apply();
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // TODO: change to be deletable

            Toast.MakeText(Activity, "Long Pressed", ToastLength.Short).Show();
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var music = e.Parent.GetItemAtPosition(e.Position).Cast<LocalMusic>();

            // TODO: show player fragment
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            // TODO: using file chooser

            //adapter.Add(new LocalMusic { Artist = "Red Velvet", Title = "Rookie" });
            adapter.NotifyDataSetChanged();

            menu.Close(true);
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

        public override void OnDestroy()
        {
            // saving data
            SaveData();

            base.OnDestroy();
        }
    }
}