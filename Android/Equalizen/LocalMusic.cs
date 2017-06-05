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

using Uri = Android.Net.Uri;

namespace Equalizen
{

    public class LocalMusic
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public TimeSpan Duration { get; set; }
        public Uri Uri { get; set; }

        public LocalMusic() { }

        public LocalMusic(Uri uri)
        {
            Uri = uri;

            // TODO: Get real information 
            Artist = uri.Path;
            Title = uri.Path;
            FilePath = uri.Path;
        }
    }
}