using Android.OS;
using Android.Widget;
using Java.Lang;
using Java.IO;

namespace IO.Vov.Vitamio.Demo
{
	[global::Android.App.Activity (Label = "MediaMetadataRetrieverDemo", MainLauncher = false)]
	public class MediaMetadataRetrieverDemo : Android.App.Activity
	{

		private string path = "";
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			var retriever = new IO.Vov.Vitamio.MediaMetadataRetriever(this);
			try
			{
				path = "";
				if (path == "")
				{
					// Tell the user to provide an audio file URL.
					Toast.MakeText(this, "Please edit MediaMetadataRetrieverDemo Activity, " + "and set the path variable to your audio file path." + " Your audio file must be stored on sdcard.", ToastLength.Short).Show();
					return;
				}
				retriever.SetDataSource ( path);
			}
			catch (IllegalArgumentException e)
			{
				e.PrintStackTrace();
			}
			catch (IllegalStateException e)
			{
				e.PrintStackTrace();
			}
			catch (IOException e)
			{
				e.PrintStackTrace();
			}
			long durationMs = Long.ParseLong(retriever.ExtractMetadata(MediaMetadataRetriever.MetadataKeyDuration));
			string artist = retriever.ExtractMetadata(MediaMetadataRetriever.MetadataKeyArtist);
			string title = retriever.ExtractMetadata(MediaMetadataRetriever.MetadataKeyTitle);

			SetContentView(Resource.Layout.media_metadata);
			TextView textView = FindViewById<TextView>(Resource.Id.textView);
			textView.Text = durationMs + "" + artist + title;

		}
	}
}