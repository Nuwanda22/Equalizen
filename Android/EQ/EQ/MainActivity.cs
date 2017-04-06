using System;
using Android.App;
using Android.Media;
using Android.Media.Audiofx;
using Android.Net;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace EQ
{
    [Activity(Label = "Equalizen", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        LinearLayout mLinearLayout;

        MediaPlayer mMediaPlayer;
        Equalizer mEqualizer;

        int lowerEqualizerBandLevel;
        int upperEqualizerBandLevel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.btn_start);
            button.Click += Button_Click;

            mLinearLayout = (LinearLayout)FindViewById(Resource.Id.GainLayout);

            //Test Source
            mMediaPlayer = MediaPlayer.Create(this, Android.Net.Uri.Parse("http://flash.comic.naver.net/bgsound/8336f367-e688-11e5-be49-38eaa78b7a54.mp3"));
            mEqualizer = new Equalizer(0, mMediaPlayer.AudioSessionId);
            mEqualizer.SetEnabled(true);

            int frequencyBandCount = mEqualizer.NumberOfBands;

            lowerEqualizerBandLevel = mEqualizer.GetBandLevelRange()[0];
            upperEqualizerBandLevel = mEqualizer.GetBandLevelRange()[1];

            for (int i = 0; i < frequencyBandCount; i++)
            {
                TextView frequencyHeaderTextView = new TextView(this);

                frequencyHeaderTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);

                frequencyHeaderTextView.Gravity = GravityFlags.CenterHorizontal;

                string BandFrequency = ConvertTokHz(mEqualizer.GetCenterFreq((short)i) / 1000);
                frequencyHeaderTextView.SetText(BandFrequency, TextView.BufferType.Normal);

                mLinearLayout.AddView(frequencyHeaderTextView);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var finder = new LocalMusicFinder();
            var list = finder.FindMusic(ContentResolver);
        }

        private string ConvertTokHz(int Freq)
        {
            if(Freq <= 1000)
            {
                return Freq + " Hz";
            }
            else
            {
                return (double)Freq / 1000 + " kHz";
            }
        }
    }
}

