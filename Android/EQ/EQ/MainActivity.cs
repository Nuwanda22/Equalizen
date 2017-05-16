using System;

using Android.App;
using Android.Media;
using Android.Net;
using Android.OS;
using Android.Widget;
using Android.Content;

using Android.Provider;
using Android.Support.V7.App;

namespace Equalizen
{
    [Activity(Label = "Equalizen", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainActivity : AppCompatActivity
    {
        #region Components

        Button AddButton;
        Button PlayButton;
        LinearLayout MusicList;

        #endregion

        MediaPlayer mediaPlayer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            #region AppCompat
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = "Equalizen";
            Window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
            #endregion
            #region InitializeComponet

            AddButton = FindViewById<Button>(Resource.Id.AddButton);
            PlayButton = FindViewById<Button>(Resource.Id.PlayButton);
            MusicList = FindViewById<LinearLayout>(Resource.Id.MusicList);

            #endregion

            AddButton.Click += AddButton_Click;
            PlayButton.Click += PlayButton_Click;
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

                string path = GetRealPathFromURI(this, uri);

                var button = new Button(this) { Text = uri.Path };
                button.Click += (sender, e) =>
                {
                    if (mediaPlayer != null)
                    {
                        mediaPlayer.Stop();
                    }

                    mediaPlayer = MediaPlayer.Create(this, uri);
                    mediaPlayer.Start();
                };

                MusicList.AddView(button);
            }
        }

        public string GetRealPathFromURI(Context context, Android.Net.Uri contentUri)
        {
            string[] proj = { MediaStore.Audio.AudioColumns.Data };
            var cursor = context.ContentResolver.Query(contentUri, proj, null, null, null);

            string path = null;

            if (cursor != null)
            {
                int count = cursor.Count;
                if (count > 0)
                {
                    while (cursor.MoveToNext())
                    {
                        path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Audio.AudioColumns.Data));
                    }
                }
            }
            cursor.Close();
            return path;
        }
    }
}

