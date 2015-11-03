using IO.Vov.Vitamio;
using Android.OS;
using Android.Graphics;
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
using Java.Lang;
using Java.IO;

namespace IO.Vov.Vitamio.Demo
{


	using LibsChecker = IO.Vov.Vitamio.LibsChecker;
	using MediaPlayer = IO.Vov.Vitamio.MediaPlayer;
	using OnPreparedListener = IO.Vov.Vitamio.MediaPlayer.IOnPreparedListener;
	using OnTimedTextListener = IO.Vov.Vitamio.MediaPlayer.IOnTimedTextListener;

	[global::Android.App.Activity (Label = "MediaPlayerSubtitle", MainLauncher = false)]
	public class MediaPlayerSubtitle : Android.App.Activity, ISurfaceHolderCallback, OnPreparedListener, OnTimedTextListener
	{

		internal SurfaceView splayer;
		internal ISurfaceHolder sholder;
		internal TextView tv;
		private MediaPlayer mediaPlayer;
		private string path = "";

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (!LibsChecker.CheckVitamioLibs (this))
				return;
			SetContentView (Resource.Layout.subtitle1);
			tv = (TextView)FindViewById (Resource.Id.sub1);
			splayer = (SurfaceView)FindViewById (Resource.Id.surface);
			sholder = splayer.Holder;
			sholder.SetFormat (Format.Rgba8888);
			sholder.AddCallback (this);
		}

		private void PlayVideo ()
		{
			try {
				if (path == "") {
					// Tell the user to provide an audio file URL.
					Toast.MakeText (this, "Please edit MediaPlayer Activity, " + "and set the path variable to your media file path." + " Your media file must be stored on sdcard.", ToastLength.Long).Show ();
					return;
				}
				mediaPlayer = new MediaPlayer (this);
				mediaPlayer.SetDataSource (path);
				mediaPlayer.SetDisplay (sholder);
				mediaPlayer.PrepareAsync ();
				mediaPlayer.SetOnPreparedListener (this);

				mediaPlayer.SetOnTimedTextListener (this);

				// TODO Auto-generated catch block
			} catch (IllegalArgumentException e) {
				// TODO Auto-generated catch block
				e.PrintStackTrace ();
			} catch (IllegalStateException e) {
				// TODO Auto-generated catch block
				e.PrintStackTrace ();
			} catch (SecurityException e) {
				// TODO Auto-generated catch block
				e.PrintStackTrace ();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.PrintStackTrace ();
			}
		}

		private void StartVPback ()
		{

			mediaPlayer.Start ();
		}

		protected override void OnPause ()
		{
			// TODO Auto-generated method stub
			base.OnPause ();
			RelaMediaPlay ();
		}

		private void RelaMediaPlay ()
		{
			// TODO Auto-generated method stub
			if (mediaPlayer != null) {
				mediaPlayer.Release ();
				mediaPlayer = null;
			}

		}

		protected override void OnDestroy ()
		{
			// TODO Auto-generated method stub
			base.OnDestroy ();
			RelaMediaPlay ();
		
		}

		public void SurfaceChanged (ISurfaceHolder holder, Format format, int width, int height)
		{
			// TODO Auto-generated method stub	
		}

		public void SurfaceCreated (ISurfaceHolder holder)
		{
			PlayVideo ();
		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{
			// TODO Auto-generated method stub	
		}


		public void OnPrepared (MediaPlayer p0)
		{
			// TODO Auto-generated method stub
			StartVPback ();
			mediaPlayer.AddTimedTextSource (Environment.ExternalStorageDirectory + "/12.srt");
			mediaPlayer.SetTimedTextShown (true);
		}


		public void OnTimedText (string p0)
		{
			// TODO Auto-generated method stub
			tv.Text = p0;
		}

		public void OnTimedTextUpdate (byte[] p0, int p1, int p2)
		{
			// TODO Auto-generated method stub
		}
	}
}