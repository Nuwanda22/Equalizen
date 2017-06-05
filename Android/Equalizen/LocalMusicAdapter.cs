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
    public class LocalMusicAdapter : ArrayAdapter<LocalMusic>
    {
        Activity context;

        public LocalMusicAdapter(Activity context, IList<LocalMusic> objects)
            : base(context, Android.Resource.Id.Text1, objects)
        {
            this.context = context;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            var item = GetItem(position);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = item.Title;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = item.Artist;

            return view;
        }
    }
}