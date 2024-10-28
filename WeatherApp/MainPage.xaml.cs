using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors; 
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using key;


namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        //private readonly string apiKey = "CpHhhtXACFLd3NMlTwHEhpYCRRuBv7qC"; 
        private readonly Settings mySettings = new Settings();
        private readonly string apiKey;


        public MainPage()
        {
            apiKey = mySettings.ApiKey;
            InitializeComponent();
            FetchAndDisplayWeatherData();
        }

        private async void FetchAndDisplayWeatherData()
        {
            string q = await GetLocationCoordinates();

            if (q != null)
            {

                var (locationKey, localizedName) = await FetchLocationKey(q);

                await FetchWeatherDataAndUpdateUI(locationKey);

                LocationLabel.Text = localizedName;
            }
            else
            {
                await DisplayAlert("Error", "Unable to fetch location.", "OK");
            }
        }

        private async Task FetchWeatherDataAndUpdateUI(string locationKey)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://dataservice.accuweather.com/currentconditions/v1/{locationKey}";
                string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=true";

                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();

                    JArray jsonArray = JArray.Parse(jsonData);
                    JObject weatherData = (JObject)jsonArray[0];  

                    double temperature = (double)weatherData["Temperature"]["Metric"]["Value"];
                    int weatherIcon = (int)weatherData["WeatherIcon"];
                    int relativeHumidity = (int)weatherData["RelativeHumidity"];
                    double windSpeed = (double)weatherData["Wind"]["Speed"]["Metric"]["Value"];
                    string windUnit = (string)weatherData["Wind"]["Speed"]["Metric"]["Unit"];
                    string windDirection = (string)weatherData["Wind"]["Direction"]["Localized"];
                    int minTemp = (int)weatherData["TemperatureSummary"]["Past24HourRange"]["Minimum"]["Metric"]["Value"];
                    int maxTemp = (int)weatherData["TemperatureSummary"]["Past24HourRange"]["Maximum"]["Metric"]["Value"];
                    int uvIndex = (int)weatherData["UVIndex"];
                    int dewPoint = (int)weatherData["DewPoint"]["Metric"]["Value"];
                    int visibility = (int)weatherData["Visibility"]["Metric"]["Value"];
                    int pressure = (int)weatherData["Pressure"]["Metric"]["Value"];


                    TemperatureLabel.Text = $"{temperature}°C";
                    WeatherPicture.Source = PictureDecisionMaker(weatherIcon.ToString());
                    HumidityLabel.Text = $"{relativeHumidity}%";
                    DewPointLabel.Text = $"DewPoint is at {dewPoint}°C";
                    WindSpeedLabel.Text = $"Speed: {windSpeed} {windUnit}";
                    WindDirectionLabel.Text =$"Direction: {windDirection}";
                    MinTempLabel.Text = $"Min: {minTemp}°C";
                    MaxTempLabel.Text = $"Max: {maxTemp}°C";
                    UVIndexLabel.Text = $"{uvIndex}";
                    VisibilityLabel.Text = $"Visibility is: {visibility} km";
                    PressureLabel.Text = $"Pressure is: {pressure} hPa";

                }
                else
                {
                    await DisplayAlert("Error", $"Unable to fetch weather data: {response.StatusCode}", "OK");
                }
            }
        }

        private async Task<string> GetLocationCoordinates()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location == null)
                {
                    await DisplayAlert("Error", "Location is unavailable. Please check location services.", "OK");
                    return null;
                }

                string q = $"{location.Latitude},{location.Longitude}";
                return q;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while retrieving location: {ex.Message}", "OK");
                return null;
            }
        }

        private async Task<(string LocationKey, string LocalizedName)> FetchLocationKey(string coordinates)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://dataservice.accuweather.com/locations/v1/cities/geoposition/search";
                string fullUrl = $"{apiUrl}?apikey={apiKey}&q={coordinates}&language=en";

                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync();

                    JObject jsonObject = JObject.Parse(jsonData);

                    string locationKey = (string)jsonObject["Key"];

                    string localizedName = (string)jsonObject["LocalizedName"];

                    return (locationKey, localizedName);
                }
                else
                {
                    await DisplayAlert("Error", $"Unable to fetch location key: {response.StatusCode}", "OK");
                    return (null, null);
                }
            }
        }

        private string PictureDecisionMaker(string numOfPic)
        {
            //string imagePath = "Resources/Images/";

            var imageDictionary = new Dictionary<string, string>
            {
                { "1", "img1.png" },
                { "2", "img2.png" },
                { "3", "img3.png" },
                { "6", "img6.png" },
                { "7", "img7.png" },
                { "11", "img11.png" },
                { "12", "img12.png" },
                { "13", "img13.png" },
                { "14", "img14.png" },
                { "15", "img15.png" },
                { "16", "img16.png" },
                { "17", "img17.png" },
                { "18", "img18.png" },
                { "19", "img19.png" },
                { "21", "img21.png" },
                { "22", "img22.png" },
                { "23", "img23.png" },
                { "25", "img25.png" },
                { "26", "img26.png" },
                { "30", "img30.png" },
                { "31", "img31.png" },
                { "32", "img32.png" },
                { "33", "img33.png" },
                { "34", "img34.png" },
                { "35", "img35.png" },
                { "36", "img36.png" },
                { "37", "img37.png" },
                { "38", "img38.png" },
                { "39", "img39.png" },
                { "40", "img40.png" },
                { "41", "img41.png" },
                { "42", "img42.png" },
                { "43", "img43.png" },
                { "44", "img44.png" }
            };

            if (imageDictionary.TryGetValue(numOfPic, out string fileName))
            {
                return fileName;
            }

            return "Image not found";
        }
    }
}