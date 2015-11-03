using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

//
// * Copyright (C) 2013 yixia.com
// *
// * Licensed under the Apache License, Version 2.0 (the "License");
// * you may not use this file except in compliance with the License.
// * You may obtain a copy of the License at
// *
// *      http://www.apache.org/licenses/LICENSE-2.0
// *
// * Unless required by applicable law or agreed to in writing, software
// * distributed under the License is distributed on an "AS IS" BASIS,
// * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// * See the License for the specific language governing permissions and
// * limitations under the License.
// 
using System.Collections.Generic;


namespace IO.Vov.Vitamio.Demo
{

	[global::Android.App.Activity (Label = "VideoSubtitleList", MainLauncher = false)]
	public class VideoSubtitleList : Android.App.ListActivity
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
			AddItem (data, "MediaPlayerSubtitle", new Intent (this, typeof(MediaPlayerSubtitle)));
			AddItem (data, "VideoViewSubtitle", new Intent (this, typeof(VideoViewSubtitle)));
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