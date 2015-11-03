using Android.Widget;
using Android.OS;
using Android.Content;

namespace IO.Vov.Vitamio.Demo
{
	[Android.App.Activity (Label = "MediaPlayerDemo", MainLauncher = false)]
	public class MediaPlayerDemo : Android.App.Activity
	{
		private Button mlocalvideo;
		private Button mlocalvideoSurface;
		private Button mstreamvideo;
		private Button mlocalaudio;
		private Button mresourcesaudio;
		private const string MEDIA = "media";
		private const int LOCAL_AUDIO = 1;
		private const int STREAM_AUDIO = 2;
		private const int RESOURCES_AUDIO = 3;
		private const int LOCAL_VIDEO = 4;
		private const int STREAM_VIDEO = 5;
		private const int RESOURCES_VIDEO = 6;
		private const int LOCAL_VIDEO_SURFACE = 7;

		protected override void OnCreate (Bundle icicle)
		{
			base.OnCreate (icicle);
			SetContentView (Resource.Layout.mediaplayer_1);
			mlocalaudio = (Button)FindViewById (Resource.Id.localaudio);
			//mlocalaudio.SetOnClickListener(mLocalAudioListener);
			mlocalaudio.Click += (sender, e) => {
				Intent intent = new Intent (Application, typeof(MediaPlayerDemo_Audio));
				intent.PutExtra (MEDIA, LOCAL_AUDIO);
				StartActivity (intent);
			};
//			mresourcesaudio = (Button)FindViewById (Resource.Id.resourcesaudio);
//			mresourcesaudio.Click += (sender, e) => {
//				Intent intent = new Intent (Application, typeof(MediaPlayerDemo_Audio));
//				intent.PutExtra (MEDIA, RESOURCES_AUDIO);
//				StartActivity (intent);
//			};

			mlocalvideo = (Button)FindViewById (Resource.Id.localvideo);
//			mlocalvideo.SetOnClickListener(mLocalVideoListener);
			mlocalvideo.Click += (object sender, System.EventArgs e) => {
				Intent intent = new Intent (this, typeof(MediaPlayerDemo_Video));
				intent.PutExtra (MEDIA, LOCAL_VIDEO);
				StartActivity (intent);
			};
			mlocalvideoSurface = (Button)FindViewById (Resource.Id.localvideo_setsurface);
			mlocalvideoSurface.Click += (object sender, System.EventArgs e) => {
				Intent intent = new Intent (this, typeof(MediaPlayerDemo_setSurface));
				intent.PutExtra (MEDIA, LOCAL_VIDEO_SURFACE);
				StartActivity (intent);
			};
			mstreamvideo = (Button)FindViewById (Resource.Id.streamvideo);
			mstreamvideo.Click += (object sender, System.EventArgs e) => {
				Intent intent = new Intent (this, typeof(MediaPlayerDemo_Video));
				intent.PutExtra (MEDIA, STREAM_VIDEO);
				StartActivity (intent);
			};
		}
	}
}