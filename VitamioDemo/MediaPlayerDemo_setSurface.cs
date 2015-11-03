using System;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;

using IO.Vov.Vitamio;
using OnBufferingUpdateListener = IO.Vov.Vitamio.MediaPlayer.IOnBufferingUpdateListener;
using OnCompletionListener = IO.Vov.Vitamio.MediaPlayer.IOnCompletionListener;
using OnPreparedListener = IO.Vov.Vitamio.MediaPlayer.IOnPreparedListener;
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
	[global::Android.App.Activity (Label = "MediaPlayerDemo_setSurface", MainLauncher = false)]
	public class MediaPlayerDemo_setSurface : Android.App.Activity, OnBufferingUpdateListener, OnCompletionListener, OnPreparedListener, TextureView.ISurfaceTextureListener
	{

		private const string TAG = "MediaPlayerDemo";
		private int mVideoWidth;
		private int mVideoHeight;
		private MediaPlayer mMediaPlayer;
		private TextureView mTextureView;
		private string path;
		private Surface surf;

		private bool mIsVideoSizeKnown = false;
		private bool mIsVideoReadyToBePlayed = false;

		//  *
		//   * 
		//   * Called when the activity is first created.
		//   
		protected override void OnCreate(Bundle icicle) 
		{
			base.OnCreate(icicle);
			if (!LibsChecker.CheckVitamioLibs(this))
				return;
			SetContentView(Resource.Layout.mediaplayer_3);
			mTextureView =  FindViewById<TextureView>(Resource.Id.surface);
			mTextureView.SurfaceTextureListener = this;

		}

		private void PlayVideo(SurfaceTexture surfaceTexture)
		{
			DoCleanUp();
			try
			{

				path = "";
				if (path == "")
				{
					// Tell the user to provide a media file URL.
					Toast.MakeText(this, "Please edit MediaPlayerDemo_setSurface Activity, " + "and set the path variable to your media file path." + " Your media file must be stored on sdcard.", ToastLength.Long).Show();
					return;
				}
				// Create a new media player and set the listeners
				mMediaPlayer = new MediaPlayer(this, true);
				mMediaPlayer.SetDataSource(path);
				if (surf == null)
				{
					surf = new Surface (surfaceTexture);
				}
				mMediaPlayer.SetSurface(surf);
				mMediaPlayer.PrepareAsync();
				mMediaPlayer.SetOnBufferingUpdateListener(this);
				mMediaPlayer.SetOnCompletionListener(this);
				mMediaPlayer.SetOnPreparedListener(this);
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

		public virtual void OnPrepared(MediaPlayer mediaplayer)
		{
			Log.Debug(TAG, "onPrepared called");
			mIsVideoReadyToBePlayed = true;
			if (mIsVideoReadyToBePlayed)
			{
				StartVideoPlayback();
			}
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

		private void StartVideoPlayback()
		{
			Log.Verbose(TAG, "startVideoPlayback");
			AdjustAspectRatio(mMediaPlayer.VideoWidth, mMediaPlayer.VideoHeight);
			mMediaPlayer.Start();
		}

		//  *
		//   * Sets the TextureView transform to preserve the aspect ratio of the video.
		//   
		private void AdjustAspectRatio(int videoWidth, int videoHeight)
		{
			int viewWidth = mTextureView.Width;
			int viewHeight = mTextureView.Height;
			double aspectRatio = (double) videoHeight / videoWidth;

			int newWidth, newHeight;
			if (viewHeight > (int)(viewWidth * aspectRatio))
			{
				// limited by narrow width; restrict height
				newWidth = viewWidth;
				newHeight = (int)(viewWidth * aspectRatio);
			}
			else
			{
				// limited by short height; restrict width
				newWidth = (int)(viewHeight / aspectRatio);
				newHeight = viewHeight;
			}
			int xoff = (viewWidth - newWidth) / 2;
			int yoff = (viewHeight - newHeight) / 2;
			Log.Verbose(TAG, "video=" + videoWidth + "x" + videoHeight + " view=" + viewWidth + "x" + viewHeight + " newView=" + newWidth + "x" + newHeight + " off=" + xoff + "," + yoff);

			Matrix txform = new Matrix();
			mTextureView.GetTransform(txform);
			txform.SetScale((float) newWidth / viewWidth, (float) newHeight / viewHeight);
			//txform.postRotate(10);          // just for fun
			txform.PostTranslate(xoff, yoff);
			mTextureView.SetTransform(txform);
		}

		public void OnSurfaceTextureAvailable (SurfaceTexture surface, int width, int height)
		{
			PlayVideo(surface);
		}
		public bool OnSurfaceTextureDestroyed (SurfaceTexture surface)
		{
			return false;
		}
		public void OnSurfaceTextureSizeChanged (SurfaceTexture surface, int width, int height)
		{
			
		}
		public void OnSurfaceTextureUpdated (SurfaceTexture surface)
		{
			
		}
	}
}