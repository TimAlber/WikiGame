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

namespace Droid
{
	[Activity (Label = "Get to /wiki/Philosophy")]			
	public class HomeScreen : ListActivity
	{
		List<string> dogs = new List<string> ();
		List<string> dogs2 = new List<string> ();

		protected override async void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

//			string html = Intent.GetStringExtra ("raw html") ?? "Data not available";
			string name = Intent.GetStringExtra ("name") ?? "Data not available";
			string url = Intent.GetStringExtra ("url") ?? "Data not available";
			int steps = Intent.GetIntExtra("steps", 1);

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
			ListAdapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, dogs);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
//			var t = dogs.ElementAt(position);
//			Android.Widget.Toast.MakeText(this, t, Android.Widget.ToastLength.Short).Show();
		
//			Task<string> sizeTask = DownloadHomepageAsync (position);
//			string html = await sizeTask;

			int steps = Intent.GetIntExtra("steps", -1);

			var activity2 = new Intent (this, typeof(HomeScreen));
			activity2.PutExtra ("steps", steps+1);
			activity2.PutExtra ("name", dogs2[position]);
			activity2.PutExtra ("url", "https://en.wikipedia.org/wiki/" + dogs.ElementAt(position));
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

