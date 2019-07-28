using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Net.Http;
using System;
using System.Net;
using System.IO;
using Android.Graphics;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Plugin.Connectivity;
using Weather.Fragments;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using Android.Content;
using Android.Gms.Location;

namespace Weather
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        Button getWeatherButton;
        TextView placeTextView;
        TextView temperatureTextView;
        TextView weatherDescriptionTextView;
        EditText cityNameEditText;
        ImageView weatherImageView;

        //day 1
        ImageView weatherImageFirst;
        TextView txtTempFirst;
        TextView txtDesFirst;
        TextView txtDayFirst;


        //day 2
        ImageView weatherImageSecond;
        TextView txtTempSecond;
        TextView txtDesSecond;
        TextView txtDaySecond;


        //day 3
        ImageView weatherImageThird;
        TextView txtTempThird;
        TextView txtDesThird;
        TextView txtDayThird;

        ProgressDialogueFragment progressDialogue;






        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            cityNameEditText = (EditText)FindViewById(Resource.Id.cityNameText);
            placeTextView = (TextView)FindViewById(Resource.Id.placeText);
            temperatureTextView = (TextView)FindViewById(Resource.Id.temperatureTextView);
            weatherDescriptionTextView = (TextView)FindViewById(Resource.Id.weatherDescriptionText);
            weatherImageView = (ImageView)FindViewById(Resource.Id.weatherImage);
            getWeatherButton = (Button)FindViewById(Resource.Id.getWeatherButton);
            getWeatherButton.Click += GetWeatherButton_Click;



            //day 1
            weatherImageFirst = (ImageView)FindViewById(Resource.Id.weatherImageFirst);
            txtTempFirst = (TextView)FindViewById(Resource.Id.txtTempFirst);
            txtDesFirst = (TextView)FindViewById(Resource.Id.txtDesFirst);
            txtDayFirst = (TextView)FindViewById(Resource.Id.txtDayFirst);

            //day 2
            weatherImageSecond = (ImageView)FindViewById(Resource.Id.weatherImageSecond);
            txtTempSecond = (TextView)FindViewById(Resource.Id.txtTempSecond);
            txtDesSecond = (TextView)FindViewById(Resource.Id.txtDesSecond);
            txtDaySecond = (TextView)FindViewById(Resource.Id.txtDaySecond);

            //day 3
            weatherImageThird = (ImageView)FindViewById(Resource.Id.weatherImageThird);
            txtTempThird = (TextView)FindViewById(Resource.Id.txtTempThird);
            txtDesThird = (TextView)FindViewById(Resource.Id.txtDesThird);
            txtDayThird = (TextView)FindViewById(Resource.Id.txtDayThird);



        }


        private void GetWeatherButton_Click(object sender, System.EventArgs e)
        {

            string place = cityNameEditText.Text;
            GetWeather(place);
            GetWeatherForcast(place);

        }






        async void GetWeather(string place)
        {
            string apiKey = "bae28df78f81c27a2be7a7e3f1ef3c4e";
            string apiBase = "https://api.openweathermap.org/data/2.5/weather?q=";
            string unit = "metric";

            if (string.IsNullOrEmpty(place))
            {
                Toast.MakeText(this, "please enter a valid city name", ToastLength.Short).Show();
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, "No internet connection", ToastLength.Short).Show();
                return;
            }



            // Asynchronous API call using HttpClient
            string url = apiBase + place + "&appid=" + apiKey + "&units=" + unit;
            var handler = new HttpClientHandler();
            using (var client = new HttpClient(handler))
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await client.GetStringAsync(url);
                    if (!string.IsNullOrEmpty(result))
                    {
                        Console.WriteLine(result);

                        var resultObject = JObject.Parse(result);
                        string weatherDescription = resultObject["weather"][0]["description"].ToString();
                        string icon = resultObject["weather"][0]["icon"].ToString();
                        string temperature = resultObject["main"]["temp"].ToString();
                        string placename = resultObject["name"].ToString();
                        string country = resultObject["sys"]["country"].ToString();
                        weatherDescription = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(weatherDescription);

                        weatherDescriptionTextView.Text = weatherDescription;
                        placeTextView.Text = placename + ", " + country;
                        temperatureTextView.Text = temperature;


                        // Download Image using WebRequest
                        string ImageUrl = "http://openweathermap.org/img/w/" + icon + ".png";
                        System.Net.WebRequest request = default(System.Net.WebRequest);
                        request = WebRequest.Create(ImageUrl);
                        request.Timeout = int.MaxValue;
                        request.Method = "GET";

                        WebResponse response1 = default(WebResponse);
                        response1 = await request.GetResponseAsync();
                        MemoryStream ms = new MemoryStream();
                        response1.GetResponseStream().CopyTo(ms);
                        byte[] imageData = ms.ToArray();

                        Bitmap bitmap = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
                        weatherImageView.SetImageBitmap(bitmap);

                    }


                    else
                        Toast.MakeText(this, "invalid city name", ToastLength.Short).Show();
                }
                else
                    Toast.MakeText(this, "an error occuerd please try again later", ToastLength.Short).Show();
            }

        }


        async void GetWeatherForcast(string place)
        {
            string apiKey = "bae28df78f81c27a2be7a7e3f1ef3c4e";
            string apiBase = "https://api.openweathermap.org/data/2.5/forecast?q=";
            string unit = "metric";

            if (string.IsNullOrEmpty(place))
            {
                Toast.MakeText(this, "please enter a valid city name", ToastLength.Short).Show();
                return;
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                Toast.MakeText(this, "No internet connection", ToastLength.Short).Show();
                return;
            }


            ShowProgressDialogue("Fetching weather...");
            // Asynchronous API call using HttpClient
            string url = apiBase + place + "&appid=" + apiKey + "&units=" + unit;
            var handler = new HttpClientHandler();

            using (var client = new HttpClient(handler))
            {

                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await client.GetStringAsync(url);
                    if (!string.IsNullOrEmpty(result))
                    {

                        Console.WriteLine(result);

                        var resultObject = JObject.Parse(result);
                        string weatherDescription1 = resultObject["list"][5]["weather"][0]["description"].ToString();
                        string weatherDescription2 = resultObject["list"][13]["weather"][0]["description"].ToString();
                        string weatherDescription3 = resultObject["list"][21]["weather"][0]["description"].ToString();

                        string icon1 = resultObject["list"][5]["weather"][0]["icon"].ToString();
                        string icon2 = resultObject["list"][13]["weather"][0]["icon"].ToString();
                        string icon3 = resultObject["list"][21]["weather"][0]["icon"].ToString();


                        string temperature1 = resultObject["list"][5]["main"]["temp"].ToString();
                        string temperature2 = resultObject["list"][13]["main"]["temp"].ToString();
                        string temperature3 = resultObject["list"][21]["main"]["temp"].ToString();

                        string Day1 = resultObject["list"][5]["dt_txt"].ToString().Substring(0, 10);
                        string Day2 = resultObject["list"][13]["dt_txt"].ToString().Substring(0, 10);
                        string Day3 = resultObject["list"][21]["dt_txt"].ToString().Substring(0, 10);
                        weatherDescription1 = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(weatherDescription1);

                        txtDesFirst.Text = weatherDescription1;
                        txtDesSecond.Text = weatherDescription2;
                        txtDesThird.Text = weatherDescription3;


                        txtTempFirst.Text = temperature1;
                        txtTempSecond.Text = temperature2;
                        txtTempThird.Text = temperature3;


                        txtDayFirst.Text = Day1;
                        txtDaySecond.Text = Day2;
                        txtDayThird.Text = Day3;

                        // Download Image using WebRequest
                        string ImageUrl1 = "http://openweathermap.org/img/w/" + icon1 + ".png";
                        System.Net.WebRequest request1 = default(System.Net.WebRequest);
                        request1 = WebRequest.Create(ImageUrl1);
                        request1.Timeout = int.MaxValue;
                        request1.Method = "GET";
                        MemoryStream ms1 = new MemoryStream();
                        (await request1.GetResponseAsync()).GetResponseStream().CopyTo(ms1);
                        byte[] imageData1 = ms1.ToArray();

                        Bitmap bitmap1 = BitmapFactory.DecodeByteArray(imageData1, 0, imageData1.Length);
                        weatherImageFirst.SetImageBitmap(bitmap1);

                        // Download Image using WebRequest
                        string ImageUrl2 = "http://openweathermap.org/img/w/" + icon2 + ".png";
                        System.Net.WebRequest request2 = default(System.Net.WebRequest);
                        request2 = WebRequest.Create(ImageUrl2);
                        request2.Timeout = int.MaxValue;
                        request2.Method = "GET";
                        MemoryStream ms2 = new MemoryStream();
                        (await request2.GetResponseAsync()).GetResponseStream().CopyTo(ms2);
                        byte[] imageData2 = ms2.ToArray();

                        Bitmap bitmap2 = BitmapFactory.DecodeByteArray(imageData2, 0, imageData2.Length);
                        weatherImageSecond.SetImageBitmap(bitmap2);

                        // Download Image using WebRequest
                        string ImageUrl3 = "http://openweathermap.org/img/w/" + icon3 + ".png";
                        System.Net.WebRequest request3 = default(System.Net.WebRequest);
                        request3 = WebRequest.Create(ImageUrl3);
                        request3.Timeout = int.MaxValue;
                        request3.Method = "GET";
                        MemoryStream ms3 = new MemoryStream();
                        (await request3.GetResponseAsync()).GetResponseStream().CopyTo(ms3);
                        byte[] imageData3 = ms3.ToArray();

                        Bitmap bitmap3 = BitmapFactory.DecodeByteArray(imageData3, 0, imageData3.Length);
                        weatherImageThird.SetImageBitmap(bitmap3);
                    }
                    else
                        Toast.MakeText(this, "invalid city name", ToastLength.Short).Show();
                }

                else
                    Toast.MakeText(this, "an error occured please try again", ToastLength.Short).Show();
            }





            ClossProgressDialogue();


        }

        void ShowProgressDialogue(string status)
        {
            progressDialogue = new ProgressDialogueFragment(status);
            var trans = SupportFragmentManager.BeginTransaction();
            progressDialogue.Cancelable = false;
            progressDialogue.Show(trans, "progress");
        }

        void ClossProgressDialogue()
        {
            if (progressDialogue != null)
            {
                progressDialogue.Dismiss();
                progressDialogue = null;
            }
        }

    }

}