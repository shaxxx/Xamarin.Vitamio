using Android.OS;
using Android.Views;
using Android.Widget;
using IO.Vov.Vitamio;
using IO.Vov.Vitamio.Widget;
using Android.Text;
using VideoView = IO.Vov.Vitamio.Widget.VideoView;

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

	[global::Android.App.Activity (Label = "VideoViewDemo", MainLauncher = false)]
	public class VideoViewDemo : Android.App.Activity
	{

		//    *
		//	 * TODO: Set the path variable to a streaming video URL or a local media file
		//	 * path.
		//	 
		//private string path = "/storage/sdcard1/DCIM/Camera/VID_20150830_190118.mp4";
		private string path = "/storage/sdcard/DCIM/test.3gp";
		private VideoView mVideoView;
		private EditText mEditText;

		protected override void OnCreate(Bundle icicle)
		{
			base.OnCreate(icicle);
			if (!LibsChecker.CheckVitamioLibs(this))
				return;
			SetContentView(Resource.Layout.videoview);
			mEditText = FindViewById<EditText>(Resource.Id.url);
			mVideoView = (VideoView) FindViewById<VideoView>(Resource.Id.surface_view);
			if (path == "")
			{
				// Tell the user to provide a media file URL/path.
				Toast.MakeText(this, "Please edit VideoViewDemo Activity, and set path" + " variable to your media file URL/path", ToastLength.Long).Show();
				return;
			}
			else
			{
				//            
				//			 * Alternatively,for streaming media you can use
				//			 * mVideoView.setVideoURI(Uri.parse(URLstring));
				//			 
				mVideoView.SetVideoPath(path);
				mVideoView.SetMediaController(new IO.Vov.Vitamio.Widget.MediaController(this));
				mVideoView.RequestFocus();
				mVideoView.Prepared += (object sender, MediaPlayer.PreparedEventArgs e) => e.P0.SetPlaybackSpeed (1.0f);
			}

		}

		[Java.Interop.Export("startPlay")]
		public virtual void startPlay(View view)
		{
			string url = mEditText.Text.ToString();
			path = url;
			if (!TextUtils.IsEmpty(url))
			{
				mVideoView.SetVideoPath(url);
			}
		}

		[Java.Interop.Export("openVideo")]
		public virtual void openVideo(View View)
		{
			mVideoView.SetVideoPath(path);
		}

	}
}