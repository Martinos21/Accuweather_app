using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.Maui.Devices.Sensors;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        // AccuWeather API URL and key
        private string apiUrl = "https://dataservice.accuweather.com/currentconditions/v1/1380799";
        private string apiKey = "CpHhhtXACFLd3NMlTwHEhpYCRRuBv7qC";

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            System.Diagnostics.Debug.WriteLine("----------------------"+location.Longitude+"----------------"+location.Latitude+"------------"+location.Altitude);


            // Call the FetchWeatherData function when the page is about to appear
            await FetchWeatherDataOnLoad();
        }

        // Method to handle weather data fetching on page load
        private async Task FetchWeatherDataOnLoad()
        {
            try
            {
                // Fetch weather data asynchronously
                string data = await FetchWeatherData();

                // Display the raw JSON data in the label
                JArray weatherdata = JArray.Parse(data);
                double temp = (double)weatherdata[0]["Temperature"]["Metric"]["Value"];
                string link = (string)weatherdata[0]["Link"];
                string locatioName = link.Split('/')[5];
                

                LocationName.Text = locatioName[0].ToString().ToUpper()+locatioName.Substring(1);
                Temperature.Text = temp.ToString()+ "°C";
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                Temperature.Text = $"Error: {ex.Message}";
            }
        }

        // Function to fetch weather data from API
        private async Task<string> FetchWeatherData()
        {
            using (HttpClient client = new HttpClient())
            {
                // Build the complete URL with the API key, language, and details parameters
                string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=true";

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(fullUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and return the raw JSON data
                    string jsonData = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(jsonData);
                    return jsonData;
                }
                else
                {
                    // Handle error responses
                    return $"Error: {response.StatusCode}";
                }
            }
        }




    }
}
