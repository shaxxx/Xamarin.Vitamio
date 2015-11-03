using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Views;

namespace IO.Vov.Vitamio.Demo
{

	public class SimpleListAdapter : BaseAdapter<IDictionary<string,object>>
	{
		IList<IDictionary<string,object>> _data;
		Android.App.Activity context;

		public SimpleListAdapter (Android.App.Activity context, IList<IDictionary<string,object>> data) : base ()
		{
			this.context = context;
			this._data = data;
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override IDictionary<string,object> this [int position] {  
			get { return _data [position]; }
		}

		public override int Count {
			get { return _data.Count; }
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null) // otherwise create a new one
				view = context.LayoutInflater.Inflate (Android.Resource.Layout.SimpleListItem1, null);
			view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = _data[position]["title"].ToString();
			return view;
		}
	}

}
