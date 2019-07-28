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

namespace Weather
{
    [Activity(Label = "CityName", Theme = "@style/AppTheme", MainLauncher = true)]
    public class CityName : Activity
    {
        Button btnNext;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.CityName);

            btnNext = FindViewById<Button>(Resource.Id.btnNext);
            btnNext.Click += BtnNext_Click;
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            Intent i = new Intent(this, typeof(MainActivity));
            StartActivity(i);
        OverridePendingTransition(Resource.Animation.fade_in, Resource.Animation.fade_out);
    }
}
}