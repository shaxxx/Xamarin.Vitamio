using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Views;

namespace IO.Vov.Vitamio.Demo
{
	[Activity (Label = "VitamioDemo", MainLauncher = true, Icon = "@drawable/ic_launcher")]
	public class VitamioListActivity : ListActivity
	{
		List<IDictionary<string, object>> data;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (!global::IO.Vov.Vitamio.LibsChecker.CheckVitamioLibs (this))
				return;
			this.ListAdapter = new SimpleListAdapter (this, GetData ());
		}

		protected internal virtual IList<IDictionary<string,object>> GetData ()
		{
			data = new List<IDictionary<string, object>> ();
			AddItem (data, "MediaPlayer", new Intent (this, typeof(MediaPlayerDemo)));
			AddItem (data, "VideoView", new Intent (this, typeof(VideoViewDemo)));
			AddItem (data, "MediaMetadata", new Intent (this, typeof(MediaMetadataRetrieverDemo)));
			AddItem (data, "VideoSubtitle", new Intent (this, typeof(VideoSubtitleList)));
			AddItem (data, "VideoViewBuffer", new Intent (this, typeof(VideoViewBuffer)));
			return data;
		}

		protected internal virtual void AddItem (IList<IDictionary<string,object>> dataList, string name, Intent intent)
		{
			var temp = new Dictionary<string, object> ();
			temp.Add ("title", name);
			temp.Add ("intent", intent);
			dataList.Add (temp);
		}


		protected override void OnListItemClick (ListView l, View v, int position, long id)
		{
			var map = data [position];
			Intent intent = (Intent)map ["intent"];
			StartActivity (intent);
		}
	}
}


