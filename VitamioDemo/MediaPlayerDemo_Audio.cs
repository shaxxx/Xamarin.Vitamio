using Android.Widget;
using Android.OS;
using Android.Content;
using Java.Lang;
using Android.Util;
using Android.Content.Res;
using Java.IO;

namespace IO.Vov.Vitamio.Demo
{
	[Android.App.Activity (Label = "MediaPlayerDemo_Audio", MainLauncher = false)]
	public class MediaPlayerDemo_Audio : Android.App.Activity
	{

		private const string TAG = "MediaPlayerDemo";
		private MediaPlayer mMediaPlayer;
		private const string MEDIA = "media";
		private const int LOCAL_AUDIO = 1;
		private const int STREAM_AUDIO = 2;
		private const int RESOURCES_AUDIO = 3;
		private const int LOCAL_VIDEO = 4;
		private const int STREAM_VIDEO = 5;
		private string path;

		private TextView tx;

		protected override void OnCreate(Bundle icicle)
		{
			base.OnCreate(icicle);
			if (!global::IO.Vov.Vitamio.LibsChecker.CheckVitamioLibs(this))
				return;
			tx = new TextView(this);
			SetContentView(tx);
			Bundle extras = Intent.Extras;
			playAudio(extras.GetInt(MEDIA));
		}

		private void playAudio(int media)
		{
			try
			{
				switch (media)
				{
				case LOCAL_AUDIO:
					//                *
					//				 * TODO: Set the path variable to a local audio file path.
					//				 
					path = "/storage/sdcard/test_cbr.mp3";
					if (path == "")
					{
						// Tell the user to provide an audio file URL.
						Toast.MakeText(this, "Please edit MediaPlayer_Audio Activity, " + "and set the path variable to your audio file path." + " Your audio file must be stored on sdcard.", ToastLength.Long).Show();
						return;
					}
					mMediaPlayer = new MediaPlayer(this);
					mMediaPlayer.SetDataSource(path);
					mMediaPlayer.Prepare();
					mMediaPlayer.Start();
					break;
				case RESOURCES_AUDIO:
					//                *
					//				 * TODO: Upload a audio file to res/raw folder and provide its resid in
					//				 * MediaPlayer.create() method.
					//				 
					//Bug need fixed
					mMediaPlayer = createMediaPlayer(this, Resource.Raw.test_cbr);
					mMediaPlayer.Start();

					break;
				}
				tx.Text = "Playing audio...";

			}
			catch (Exception e)
			{
				Log.Error(TAG, "error: " + e.Message, e);
			}

		}

		public virtual MediaPlayer createMediaPlayer(Context context, int resid)
		{
			try
			{
				AssetFileDescriptor afd = context.Resources.OpenRawResourceFd(resid);
				MediaPlayer mp = new MediaPlayer(context);
				mp.SetDataSource(afd.FileDescriptor);
				afd.Close();
				mp.Prepare();
				return mp;
			}
			catch (IOException ex)
			{
				Log.Debug(TAG, "create failed:", ex);
				// fall through
			}
			catch (IllegalArgumentException ex)
			{
				Log.Debug(TAG, "create failed:", ex);
				// fall through
			}
			catch (SecurityException ex)
			{
				Log.Debug(TAG, "create failed:", ex);
				// fall through
			}
			return null;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (mMediaPlayer != null)
			{
				mMediaPlayer.Release();
				mMediaPlayer = null;
			}

		}
	}
}