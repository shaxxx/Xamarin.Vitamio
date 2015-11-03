using Android.OS;
using Android.Views;
using Android.Widget;
using IO.Vov.Vitamio;
using VideoView = IO.Vov.Vitamio.Widget.VideoView;
using Android.Text;

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

namespace IO.Vov.Vitamio.Demo
{

	using OnTimedTextListener = IO.Vov.Vitamio.MediaPlayer.IOnTimedTextListener;

	[global::Android.App.Activity (Label = "VideoViewSubtitle", MainLauncher = false)]
	public class VideoViewSubtitle : Android.App.Activity, OnTimedTextListener
	{

		private string path = "";
		private string subtitle_path = "";
		private VideoView mVideoView;
		private TextView mSubtitleView;
		private long mPosition = 0;
		private int mVideoLayout = 0;

		protected override void OnCreate (Bundle icicle)
		{
			base.OnCreate (icicle);
			if (!LibsChecker.CheckVitamioLibs (this))
				return;
			SetContentView (Resource.Layout.subtitle2);
			mVideoView = FindViewById<VideoView> (Resource.Id.surface_view);
			mSubtitleView = FindViewById<TextView> (Resource.Id.subtitle_view);

			if (path == "") {
				// Tell the user to provide a media file URL/path.
				Toast.MakeText (this, "Please edit VideoViewSubtitle Activity, and set path" + " variable to your media file URL/path", ToastLength.Long).Show ();
				return;
			} else {
				//            
				//			 * Alternatively,for streaming media you can use
				//			 * mVideoView.setVideoURI(Uri.parse(URLstring));
				//			 
				mVideoView.SetVideoPath (path);

				// mVideoView.setMediaController(new MediaController(this));
				mVideoView.RequestFocus ();
				mVideoView.Prepared += (object sender, MediaPlayer.PreparedEventArgs e) => {
					// optional need Vitamio 4.0
					e.P0.SetPlaybackSpeed (1.0f);
					mVideoView.AddTimedTextSource (subtitle_path);
					mVideoView.SetTimedTextShown (true);	
				};

				mVideoView.SetOnTimedTextListener (this);

			}
		}

		protected override void OnPause ()
		{
			mPosition = mVideoView.CurrentPosition;
			mVideoView.StopPlayback ();
			base.OnPause ();
		}

		protected override void OnResume ()
		{
			if (mPosition > 0) {
				mVideoView.SeekTo (mPosition);
				mPosition = 0;
			}
			base.OnResume ();
			mVideoView.Start ();
		}


		[Java.Interop.Export("changeLayout")]
		public virtual void changeLayout (View view)
		{
			mVideoLayout++;
			if (mVideoLayout == 4) {
				mVideoLayout = 0;
			}
			switch (mVideoLayout) {
			case 0:
				mVideoLayout = VideoView.VideoLayoutOrigin;
				view.SetBackgroundResource (Resource.Drawable.mediacontroller_sreen_size_100);
				break;
			case 1:
				mVideoLayout = VideoView.VideoLayoutScale;
				view.SetBackgroundResource (Resource.Drawable.mediacontroller_screen_fit);
				break;
			case 2:
				mVideoLayout = VideoView.VideoLayoutStretch;
				view.SetBackgroundResource (Resource.Drawable.mediacontroller_screen_size);
				break;
			case 3:
				mVideoLayout = VideoView.VideoLayoutZoom;
				view.SetBackgroundResource (Resource.Drawable.mediacontroller_sreen_size_crop);

				break;
			}
			mVideoView.SetVideoLayout (mVideoLayout, 0);
		}

		public void OnTimedText (string text)
		{
			mSubtitleView.Text = text;
		}

		public void OnTimedTextUpdate (byte[] p0, int p1, int p2)
		{
			
		}

	}
}