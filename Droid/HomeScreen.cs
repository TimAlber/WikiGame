using System;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Threading.Tasks;
using System.Net.Http;
using Android.Webkit;

namespace Droid
{
	[Activity (Label = "Get to /wiki/Philosophy")]			
	public class HomeScreen : Activity
	{
		List<string> dogs = new List<string> ();
		List<string> dogs2 = new List<string> ();
		WebView web_view;
		int steps;
		protected override async void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.home);

//			string html = Intent.GetStringExtra ("raw html") ?? "Data not available";
			string name = Intent.GetStringExtra ("name") ?? "Data not available";
			string url = Intent.GetStringExtra ("url") ?? "Data not available";
			steps = Intent.GetIntExtra("steps", 1);


			web_view = FindViewById<WebView> (Resource.Id.webView1);
			web_view.Settings.JavaScriptEnabled = false;
			web_view.LoadUrl (url);
			web_view.SetWebViewClient (new HelloWebViewClient ());
			web_view.Settings.Dispose ();

			if (name == "/wiki/Philosophy") {
				var activity2 = new Intent (this, typeof(FoundIt));
				activity2.PutExtra ("steps", steps);
				StartActivity (activity2);
			};

			this.Title = ("#" + steps.ToString() + " | You're at " + name);

			Task<string> sizeTask = DownloadHomepageAsync (url);
			string html = await sizeTask;

			foreach (LinkItem i in LinkFinder.Find(html)) {
				string[] url1 = i.ToString().Split ('§');
				if (i.ToString().Contains ("/wiki/File:") || i.ToString().Contains ("/wiki/Wikipedia:") || i.ToString().Contains ("/wiki/Portal:")) {

				} else {
					dogs.Add (url1 [1]);
					dogs2.Add (url1 [0]);
				}
			}
			ListView Lview = FindViewById<ListView>(Resource.Id.listView1);
			Lview.ItemClick += OnListItemClick;
			Lview.Adapter = new ArrayAdapter (this, Android.Resource.Layout.SimpleListItem1, dogs);
			//ListAdapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, dogs);


		}

		void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e) {
//			var t = dogs.ElementAt(position);
//			Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
		
//			Task<string> sizeTask = DownloadHomepageAsync (position);
//			string html = await sizeTask;

			steps = Intent.GetIntExtra("steps", -1);

			var activity2 = new Intent (this, typeof(HomeScreen));
			activity2.PutExtra ("steps", steps+1);
			activity2.PutExtra ("name", dogs2[e.Position]);
			activity2.PutExtra ("url", "https://en.wikipedia.org/wiki/" + dogs.ElementAt(e.Position));
			StartActivity (activity2);
		
		}
		public async Task<string> DownloadHomepageAsync (string url)
		{
			var httpClient = new HttpClient ();
			Android.Widget.Toast.MakeText (this, url, Android.Widget.ToastLength.Short).Show();
			Task<string> contentsTask = httpClient.GetStringAsync (url);
			string contents = await contentsTask;
			return contents;
		}
	}

	public class HelloWebViewClient : WebViewClient
	{
		public override bool ShouldOverrideUrlLoading (WebView view, string url)
		{
			view.LoadUrl (url);
			return true;
		}
	}

	public struct LinkItem
	{
		public string Href;
		public string Text;

		public override string ToString ()
		{
			return Href + "§" + Text;
		}
	}

	static class LinkFinder
	{
		public static List<LinkItem> Find (string file)
		{

			int index1 = file.IndexOf("<div id=\"mw-content-text\"");
			int index2 = file.IndexOf("<div class=\"printfooter\">");
			file = file.Remove(0,index1);
			file = file.Remove(index2);

			List<LinkItem> list = new List<LinkItem> ();

			// 1.
			// Find all matches in file.
//			MatchCollection m1 = Regex.Matches (file, @"(<a.*?>.*?</a>)",
//				                     RegexOptions.Singleline);
			MatchCollection m1 = Regex.Matches (file, @"(<a href=""/wiki/.*?</a>)",
				RegexOptions.Singleline);

			// 2.
			// Loop over each match.
			foreach (Match m in m1) {
				string value = m.Groups [1].Value;
				LinkItem i = new LinkItem ();

				// 3.
				// Get href attribute.
				Match m2 = Regex.Match (value, @"href=\""(.*?)\""",
					           RegexOptions.Singleline);
				if (m2.Success) {
					i.Href = m2.Groups [1].Value;
				}

				// 4.
				// Remove inner tags from text.
				string t = Regex.Replace (value, @"\s*<.*?>\s*", "",
					           RegexOptions.Singleline);
				i.Text = t;

				list.Add (i);
			}
			return list;
		}
	}
}

