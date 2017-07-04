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

using Newtonsoft.Json;

using Uri = Android.Net.Uri;

namespace Equalizen
{

    public class LocalMusic
    {
        private Uri uri;

        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public TimeSpan Duration { get; set; }
        public string UriInfo { get; set; }
        [JsonIgnore]
        public Uri Uri
        {
            get
            {
                return uri ?? Uri.Parse(UriInfo ?? FilePath);
            }

            set
            {
                uri = value;
                UriInfo = uri.ToString();
            }
        }

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
