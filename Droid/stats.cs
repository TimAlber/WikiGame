
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
using System.Reflection;
using System.IO;

namespace Droid
{
	[Activity (Label = "STATS")]			
	public class stats : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.stats);
			TextView text = FindViewById<TextView>(Resource.Id.textView3);
			Button butn = FindViewById<Button>(Resource.Id.button);
			// Create your application here
			var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.MyDocuments);
			var filePath = Path.Combine (documentsPath, "laseraugen.txt");
			if (System.IO.File.Exists(filePath)){
				string content = System.IO.File.ReadAllText (filePath);
				text.Text = content;
			} else {
				text.Text = "Keine Stats gefunden.";
			}
			butn.Click += (sender, e) => {
				var activity2 = new Intent (this, typeof(MainActivity));
				StartActivity (activity2);
			};
			}
	}
}