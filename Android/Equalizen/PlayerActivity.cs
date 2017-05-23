using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media.Audiofx;
using Android.Media;

namespace Equalizen
{
    [Activity(Label = "PlayerActivity")]
    public class PlayerActivity : Activity
    {
        #region View Fields
        LinearLayout gainLayout;
        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Player);

            #region InitializeComponent

            gainLayout = (LinearLayout)FindViewById(Resource.Id.GainLayout);

            #endregion

            // TODO:
            // Find local music file
            var finder = new LocalMusicFinder();
            var list = finder.FindMusic(ContentResolver);

            // create media player and play first song in local music
            // if there is no music file on the device, play it on the internet
            var mediaPlayer = MediaPlayer.Create(this, Android.Net.Uri.Parse(/*list[0].Path ?? */"http://flash.comic.naver.net/bgsound/8336f367-e688-11e5-be49-38eaa78b7a54.mp3"));
            //mediaPlayer.Start();

            // make equalizer by media player
            var equalizer = new Equalizer(0, mediaPlayer.AudioSessionId);
            equalizer.SetEnabled(true);

            // and initialize layout by equalizer
            InitializeLayoutByEqualizer(gainLayout, equalizer);
        }

        private void InitializeLayoutByEqualizer(ViewGroup layout, Equalizer equalizer)
        {
            int frequencyBandCount = equalizer.NumberOfBands;

            int lowerEqualizerBandLevel = equalizer.GetBandLevelRange()[0];
            int upperEqualizerBandLevel = equalizer.GetBandLevelRange()[1];
            short equalizerBandIndex;

            for (int i = 0; i < frequencyBandCount; i++)
            {
                equalizerBandIndex = (short)i;

                #region FrequecyBands
                TextView frequencyHeaderTextView = new TextView(this);

                frequencyHeaderTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);

                frequencyHeaderTextView.Gravity = GravityFlags.CenterHorizontal;

                string BandFrequency = ConvertTokHz(equalizer.GetCenterFreq(equalizerBandIndex) / 1000);
                frequencyHeaderTextView.SetText(BandFrequency, TextView.BufferType.Normal);

                layout.AddView(frequencyHeaderTextView);
                #endregion

                #region SeekBar
                LinearLayout rowLinearLayout = new LinearLayout(this);

                //Initialize lower band level
                TextView lowerBandLevelTextView = new TextView(this);

                lowerBandLevelTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

                lowerBandLevelTextView.SetText(lowerEqualizerBandLevel / 100 + "dB", TextView.BufferType.Normal);

                //initialize upper band level
                TextView upperBandLevelTextView = new TextView(this);

                upperBandLevelTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

                upperBandLevelTextView.SetText(upperEqualizerBandLevel / 100 + "dB", TextView.BufferType.Normal);

                //initialize each band level
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);

                layoutParams.Weight = 1;

                SeekBar seekBar = new SeekBar(this);
                seekBar.Id = equalizerBandIndex;
                seekBar.LayoutParameters = layoutParams;

                seekBar.Max = upperEqualizerBandLevel - lowerEqualizerBandLevel;
                seekBar.Progress = equalizer.GetBandLevel(equalizerBandIndex);

                seekBar.ProgressChanged += (s, e) =>
                {
                    equalizer.SetBandLevel(equalizerBandIndex, (short)(e.Progress + lowerEqualizerBandLevel));
                };

                rowLinearLayout.AddView(lowerBandLevelTextView);
                rowLinearLayout.AddView(seekBar);
                rowLinearLayout.AddView(upperBandLevelTextView);

                layout.AddView(rowLinearLayout);
                #endregion
            }
        }
        private string ConvertTokHz(int Freq)
        {
            if (Freq <= 1000)
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