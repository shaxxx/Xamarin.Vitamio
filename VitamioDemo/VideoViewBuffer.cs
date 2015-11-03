using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using IO.Vov.Vitamio;

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
	using OnBufferingUpdateListener = IO.Vov.Vitamio.MediaPlayer.IOnBufferingUpdateListener;
	using OnInfoListener = IO.Vov.Vitamio.MediaPlayer.IOnInfoListener;
	using MediaController = IO.Vov.Vitamio.Widget.MediaController;
	using VideoView = IO.Vov.Vitamio.Widget.VideoView;

	[global::Android.App.Activity (Label = "VideoViewBuffer", MainLauncher = false)]
	public class VideoViewBuffer : Android.App.Activity, IO.Vov.Vitamio.MediaPlayer.IOnInfoListener, IO.Vov.Vitamio.MediaPlayer.IOnBufferingUpdateListener
	{

		//  *
		//   * TODO: Set the path variable to a streaming video URL or a local media file
		//   * path.
		//
		private string path = "http://pl.youku.com/playlist/m3u8?ts=1394676342&keyframe=0&vid=XNjU4MTc0Mjky&type=mp4";
		private Android.Net.Uri uri;
		private VideoView mVideoView;
		private ProgressBar pb;
		private TextView downloadRateView, loadRateView;

		protected override void OnCreate (Bundle icicle)
		{
			//base.OnCreate(icicle);
			if (!LibsChecker.CheckVitamioLibs (this))
				return;
			SetContentView (Resource.Layout.videobuffer);
			mVideoView = FindViewById<VideoView> (Resource.Id.buffer);
			pb = FindViewById<ProgressBar> (Resource.Id.probar);

			downloadRateView = FindViewById<TextView> (Resource.Id.download_rate);
			loadRateView = FindViewById<TextView> (Resource.Id.load_rate);
			if (path == "") {
				// Tell the user to provide a media file URL/path.
				Toast.MakeText (this, "Please edit VideoBuffer Activity, and set path" + " variable to your media file URL/path", ToastLength.Long).Show ();
				return;
			} else {
				//      
				//       * Alternatively,for streaming media you can use
				//       * mVideoView.setVideoURI(Uri.parse(URLstring));
				//       
				uri = Android.Net.Uri.Parse (path);
				mVideoView.SetVideoURI (uri);
				mVideoView.SetMediaController (new MediaController (this));
				mVideoView.RequestFocus ();
				mVideoView.SetOnInfoListener (this);
				mVideoView.SetOnBufferingUpdateListener (this);
				mVideoView.Prepared += (object sender, MediaPlayer.PreparedEventArgs e) => {
					e.P0.SetPlaybackSpeed(1.0f);
				};
			}
		}

		public bool OnInfo (MediaPlayer mp, int what, int extra)
		{
			switch (what) {
			case MediaPlayer.MediaInfoBufferingStart:
				if (mVideoView.IsPlaying) {
					mVideoView.Pause ();
					pb.Visibility = ViewStates.Visible;
					downloadRateView.Text = "";
					loadRateView.Text = "";
					downloadRateView.Visibility = ViewStates.Visible;
					loadRateView.Visibility = ViewStates.Visible;

				}
				break;
			case MediaPlayer.MediaInfoBufferingEnd:
				mVideoView.Start ();
				pb.Visibility = ViewStates.Gone;
				downloadRateView.Visibility = ViewStates.Gone;
				loadRateView.Visibility = ViewStates.Gone;
				break;
			case MediaPlayer.MediaInfoDownloadRateChanged:
				downloadRateView.Text = "" + extra + "kb/s" + "  ";
				break;
			}
			return true;
		}

		public void OnBufferingUpdate (MediaPlayer mp, int percent)
		{
			loadRateView.Text = percent + "%";
		}

	}
}