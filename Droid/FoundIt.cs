
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace Droid
{
	[Activity (Label = "Congrats!")]			
	public class FoundIt : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Found);
			TextView text = FindViewById<TextView>(Resource.Id.textView4);
			int steps = Intent.GetIntExtra("steps", -1);
			text.Text = steps.ToString ();

//			var sr = Assets.Open ("read_asset.txt");
//			sr.Write (steps.ToString ());
			var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
			var filePath = Path.Combine (documentsPath, "laseraugen.txt");
			DateTime now = DateTime.Now.ToLocalTime();
			System.IO.File.WriteAllText (filePath, "It took you : " + steps.ToString() + " Steps. " + now.ToString());

			Button button = FindViewById<Button>(Resource.Id.button);
			button.Click += (sender, e) => {
				var activity2 = new Intent (this, typeof(MainActivity));
				StartActivity (activity2);
			};
		}
	}
}

