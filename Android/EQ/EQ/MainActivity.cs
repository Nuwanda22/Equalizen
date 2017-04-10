using System;
using Android.App;
using Android.Media;
using Android.Media.Audiofx;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

namespace EQ
{
    [Activity(Label = "Equalizen", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
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
    }
}

