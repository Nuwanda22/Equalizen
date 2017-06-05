using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.Media.Audiofx;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Fragment = Android.Support.V4.App.Fragment;
using Uri = Android.Net.Uri;

namespace Equalizen
{
    public class PlayerFragment : Fragment
    {
        private MediaPlayer player;
        private Uri uri;

        #region Components

        private LinearLayout gainLayout;
        private Button previousButton;
        private Button playButton;
        private Button nextButton;
        private ProgressBar progressBar;

        #endregion

        public PlayerFragment(LocalMusic music)
        {
            // TODO: get uri from file
            uri = music.Uri ?? Uri.Parse(music.FilePath);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.player_fragment, container, false);

            InitializeComponts(view);

            player = MediaPlayer.Create(Activity, uri);
            if(player == null)
            {
                player = MediaPlayer.Create(Activity, Uri.Parse("http://flash.comic.naver.net/bgsound/8336f367-e688-11e5-be49-38eaa78b7a54.mp3"));
            }

            // TODO: synchronize progress bar
            
            // make equalizer by media player
            var equalizer = new Equalizer(0, player.AudioSessionId);
            equalizer.SetEnabled(true);

            // and initialize layout by equalizer
            // TODO: load equalizing data
            InitializeLayoutByEqualizer(gainLayout, equalizer);

            return view;
        }

        private void InitializeComponts(View view)
        {
            gainLayout = view.FindViewById<LinearLayout>(Resource.Id.gain_layout);
            previousButton = view.FindViewById<Button>(Resource.Id.prev_button);
            playButton = view.FindViewById<Button>(Resource.Id.play_button);
            nextButton = view.FindViewById<Button>(Resource.Id.next_button);
            progressBar = view.FindViewById<ProgressBar>(Resource.Id.music_progress_bar);

            previousButton.Click += PlayButton_Click;
            playButton.Click += PlayButton_Click;
            nextButton.Click += PlayButton_Click;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            var tag = button.Tag.ToString();
            
            switch (tag)
            {
                case "prev":
                    // TODO: play previous song
                    break;

                case "play":
                    if (player.IsPlaying)
                    {
                        player.Pause();
                        button.Text = "Play";
                    }
                    else
                    {
                        player.Start();
                        button.Text = "Pause";
                    }
                    break;

                case "next":
                    // TODO: play next song
                    break;
            }
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
                TextView frequencyHeaderTextView = new TextView(Activity);

                frequencyHeaderTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);

                frequencyHeaderTextView.Gravity = GravityFlags.CenterHorizontal;

                string BandFrequency = ConvertTokHz(equalizer.GetCenterFreq(equalizerBandIndex) / 1000);
                frequencyHeaderTextView.SetText(BandFrequency, TextView.BufferType.Normal);

                layout.AddView(frequencyHeaderTextView);
                #endregion

                #region SeekBar
                LinearLayout rowLinearLayout = new LinearLayout(Activity);

                //Initialize lower band level
                TextView lowerBandLevelTextView = new TextView(Activity);

                lowerBandLevelTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

                lowerBandLevelTextView.SetText(lowerEqualizerBandLevel / 100 + "dB", TextView.BufferType.Normal);

                //initialize upper band level
                TextView upperBandLevelTextView = new TextView(Activity);

                upperBandLevelTextView.LayoutParameters = new ViewGroup.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

                upperBandLevelTextView.SetText(upperEqualizerBandLevel / 100 + "dB", TextView.BufferType.Normal);

                //initialize each band level
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    ViewGroup.LayoutParams.WrapContent);

                layoutParams.Weight = 1;

                SeekBar seekBar = new SeekBar(Activity);
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
        
        public override void OnDestroy()
        {
            base.OnDestroy();

            // TODO: save equalizing data
        }
    }
}