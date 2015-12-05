using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Android.Graphics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Droid
{
	[Activity (Label = "The Wikipedia Game", MainLauncher = true)]
	public class MainActivity : Activity
	{
		int steps = 0;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);
			Button button = FindViewById<Button>(Resource.Id.button);
			Button button2 = FindViewById<Button>(Resource.Id.button2);

			button.Click += (sender, e) => {

//				Task<string> sizeTask = DownloadHomepageAsync ();
//				string html = await sizeTask;

				var activity2 = new Intent (this, typeof(HomeScreen));
//				activity2.PutExtra ("raw html", html);
				activity2.PutExtra ("url", "https://en.wikipedia.org/wiki/Special:Random");
				activity2.PutExtra ("steps", steps);
				StartActivity (activity2);

			};
			button2.Click += (sender, e) => {
				var activity2 = new Intent (this, typeof(stats));
				StartActivity (activity2);
			};
		}
//		public async Task<string> DownloadHomepageAsync ()
//		{
//			var httpClient = new HttpClient ();
//			Android.Widget.Toast.MakeText (this, "https://en.wikipedia.org/wiki/Special:Random", Android.Widget.ToastLength.Long).Show();
//			Task<string> contentsTask = httpClient.GetStringAsync ("https://en.wikipedia.org/wiki/Special:Random");
////			Task<string> contentsTask = httpClient.GetStringAsync ("https://en.wikipedia.org/wiki/Philosophy_of_science");
//			string contents = await contentsTask;
//			return contents;
//		}
	}
}