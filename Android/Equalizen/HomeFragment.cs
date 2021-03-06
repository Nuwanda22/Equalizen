﻿using System;
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
        public static readonly int PickAudioId = 1000;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.home_fragment, container, false);

            InitializeComponents(view);

            listView.ChoiceMode = ChoiceMode.MultipleModal;
            listView.SetMultiChoiceModeListener(new MultiChoiceModeListener(Activity, listView));

            // loading saved data
            var musics = /*new List<LocalMusic>();*/LoadData();
            adapter = new LocalMusicAdapter(Activity, musics);
            listView.Adapter = adapter;

            return view;

            List<LocalMusic> LoadData()
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
        }

        public override void OnDestroy()
        {
            // saving data
            SaveData();

            base.OnDestroy();
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if ((requestCode == PickAudioId) && (resultCode == (int)Result.Ok) && (data != null))
            {
                var uri = data.Data;

                // TODO: No duplication
                adapter.Add(new LocalMusic(uri));
                adapter.NotifyDataSetChanged();
            }
        }

        #region Methods

        private void ShowFileChooser()
        {
            var intent = new Intent();
            intent.SetType("audio/*");
            intent.SetAction(Intent.ActionGetContent);

            StartActivityForResult(Intent.CreateChooser(intent, "Select Music"), PickAudioId);
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
            //listView.ItemLongClick += ListView_ItemLongClick;
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

        #endregion

        #region Event Handlers

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            // TODO: change to be deletable

            Toast.MakeText(Activity, "Long Pressed", ToastLength.Short).Show();
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var music = e.Parent.GetItemAtPosition(e.Position).Cast<LocalMusic>();

            FragmentManager.BeginTransaction().Add(Resource.Id.fragment, new PlayerFragment(music)).AddToBackStack(null).Commit();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            ShowFileChooser();

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

        #endregion
    }

    class MultiChoiceModeListener : Java.Lang.Object, AbsListView.IMultiChoiceModeListener
    {
        Activity activity;
        ListView listView;

        public MultiChoiceModeListener(Activity activity, ListView listView)
        {
            this.activity = activity;
            this.listView = listView;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.share:
                    Toast.MakeText(activity, "Shared " + listView.CheckedItemCount + " items", ToastLength.Short).Show();
                    mode.Finish();
                    break;
                default:
                    Toast.MakeText(activity, "Clicked " + item.TitleFormatted, ToastLength.Short).Show();
                    break;
            }

            return true;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            activity.MenuInflater.Inflate(Resource.Menu.list_select_menu, menu);
            mode.Title = "편집할 음악을 선택하시오";

            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {

        }

        public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool @checked)
        {
            int checkedCount = listView.CheckedItemCount;

            switch (checkedCount)
            {
                case 0:
                    mode.Subtitle = null;
                    break;
                default:
                    mode.Subtitle = checkedCount + "개 선택됨";
                    break;
            }
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return true;
        }
    }
}