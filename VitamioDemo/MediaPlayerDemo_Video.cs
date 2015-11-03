using Android.App;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using IO.Vov.Vitamio;
using IO.Vov.Vitamio.Widget;

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
using Java.Lang;

namespace IO.Vov.Vitamio.Demo
{

	using OnBufferingUpdateListener = IO.Vov.Vitamio.MediaPlayer.IOnBufferingUpdateListener;
	using OnCompletionListener = IO.Vov.Vitamio.MediaPlayer.IOnCompletionListener;
	using OnPreparedListener = IO.Vov.Vitamio.MediaPlayer.IOnPreparedListener;
	using OnVideoSizeChangedListener = IO.Vov.Vitamio.MediaPlayer.IOnVideoSizeChangedListener;

	[global::Android.App.Activity (Label = "MediaPlayerDemo_Video", MainLauncher = false)]
	public class MediaPlayerDemo_Video : Android.App.Activity, OnBufferingUpdateListener, OnCompletionListener, OnPreparedListener, MediaPlayer.IOnVideoSizeChangedListener, ISurfaceHolderCallback
	{

		private const string TAG = "MediaPlayerDemo";
		private int mVideoWidth;
		private int mVideoHeight;
		private MediaPlayer mMediaPlayer;
		private SurfaceView mPreview;
		private ISurfaceHolder holder;
		private string path;
		private Bundle extras;
		private const string MEDIA = "media";
		private const int LOCAL_AUDIO = 1;
		private const int STREAM_AUDIO = 2;
		private const int RESOURCES_AUDIO = 3;
		private const int LOCAL_VIDEO = 4;
		private const int STREAM_VIDEO = 5;
		private bool mIsVideoSizeKnown = false;
		private bool mIsVideoReadyToBePlayed = false;

		//    *
		//	 * 
		//	 * Called when the activity is first created.
		//	 
		protected override void OnCreate(Bundle icicle)
		{
			base.OnCreate(icicle);
			if (!LibsChecker.CheckVitamioLibs(this))
				return;
			SetContentView(Resource.Layout.mediaplayer_2);
			mPreview =  FindViewById<SurfaceView>(Resource.Id.surface);
			holder = mPreview.Holder;
			holder.AddCallback(this);
			holder.SetFormat(Format.Rgba8888);
			extras = Intent.Extras;

		}

		private void PlayVideo(int Media)
		{
			DoCleanUp();
			try
			{

				switch (Media)
				{
				case LOCAL_VIDEO:
					//                
					//				 * TODO: Set the path variable to a local media file path.
					//				 
					path = "";
					if (path == "")
					{
						// Tell the user to provide a media file URL.
						Toast.MakeText(this, "Please edit MediaPlayerDemo_Video Activity, " + "and set the path variable to your media file path." + " Your media file must be stored on sdcard.", ToastLength.Long).Show();
						return;
					}
					break;
				case STREAM_VIDEO:
					//                
					//				 * TODO: Set path variable to progressive streamable mp4 or
					//				 * 3gpp format URL. Http protocol should be used.
					//				 * Mediaplayer can only play "progressive streamable
					//				 * contents" which basically means: 1. the movie atom has to
					//				 * precede all the media data atoms. 2. The clip has to be
					//				 * reasonably interleaved.
					//				 * 
					//				 
					path = "";
					if (path == "")
					{
						// Tell the user to provide a media file URL.
						Toast.MakeText(this, "Please edit MediaPlayerDemo_Video Activity," + " and set the path variable to your media file URL.", ToastLength.Long).Show();
						return;
					}

					break;

				}

				// Create a new media player and set the listeners
				mMediaPlayer = new MediaPlayer(this);
				mMediaPlayer.SetDataSource(path);
				mMediaPlayer.SetDisplay(holder);
				mMediaPlayer.PrepareAsync();
				mMediaPlayer.SetOnBufferingUpdateListener(this);
				mMediaPlayer.SetOnCompletionListener(this);
				mMediaPlayer.SetOnPreparedListener(this);
				mMediaPlayer.SetOnVideoSizeChangedListener(this);
				VolumeControlStream = Stream.Music;

			}
			catch (Exception e)
			{
				Log.Error(TAG, "error: " + e.Message, e);
			}
		}

		public virtual void OnBufferingUpdate(MediaPlayer arg0, int percent)
		{
			// Log.d(TAG, "onBufferingUpdate percent:" + percent);

		}

		public virtual void OnCompletion(MediaPlayer arg0)
		{
			Log.Debug(TAG, "onCompletion called");
		}

		public virtual void OnVideoSizeChanged(MediaPlayer mp, int width, int height)
		{
			Log.Verbose(TAG, "onVideoSizeChanged called");
			if (width == 0 || height == 0)
			{
				Log.Error(TAG, "invalid video width(" + width + ") or height(" + height + ")");
				return;
			}
			mIsVideoSizeKnown = true;
			mVideoWidth = width;
			mVideoHeight = height;
			if (mIsVideoReadyToBePlayed && mIsVideoSizeKnown)
			{
				startVideoPlayback();
			}
		}

		public virtual void OnPrepared(MediaPlayer mediaplayer)
		{
			Log.Debug(TAG, "onPrepared called");
			mIsVideoReadyToBePlayed = true;
			if (mIsVideoReadyToBePlayed && mIsVideoSizeKnown)
			{
				startVideoPlayback();
			}
		}

		public virtual void SurfaceChanged(ISurfaceHolder surfaceholder, int i, int j, int k)
		{
			Log.Debug(TAG, "surfaceChanged called");

		}

		public virtual void SurfaceDestroyed(ISurfaceHolder surfaceholder)
		{
			Log.Debug(TAG, "surfaceDestroyed called");
		}

		public virtual void SurfaceCreated(ISurfaceHolder holder)
		{
			Log.Debug(TAG, "surfaceCreated called");
			PlayVideo(extras.GetInt(MEDIA));

		}

		protected override void OnPause()
		{
			base.OnPause();
			ReleaseMediaPlayer();
			DoCleanUp();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ReleaseMediaPlayer();
			DoCleanUp();
		}

		private void ReleaseMediaPlayer()
		{
			if (mMediaPlayer != null)
			{
				mMediaPlayer.Release();
				mMediaPlayer = null;
			}
		}

		private void DoCleanUp()
		{
			mVideoWidth = 0;
			mVideoHeight = 0;
			mIsVideoReadyToBePlayed = false;
			mIsVideoSizeKnown = false;
		}

		private void startVideoPlayback()
		{
			Log.Verbose(TAG, "startVideoPlayback");
			holder.SetFixedSize(mVideoWidth, mVideoHeight);
			mMediaPlayer.Start();
		}

		public void SurfaceChanged (ISurfaceHolder holder, Format format, int width, int height)
		{
			Log.Debug(TAG, "surfaceChanged called");
		}
	}
}