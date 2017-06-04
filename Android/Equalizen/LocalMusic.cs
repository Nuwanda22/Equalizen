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

namespace Equalizen
{

    public class LocalMusic
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public TimeSpan Duration { get; set; }
    }
}