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
        
        //private string apiUrl = $"https://dataservice.accuweather.com/currentconditions/v1/{locationKey}";
        private string apiKey = "CpHhhtXACFLd3NMlTwHEhpYCRRuBv7qC";

        // Accuweather API for current location data
        private string apiUrlLocation = "http://dataservice.accuweather.com/locations/v1/cities/geoposition/search";

        private string q;
        
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            string LongLat = await GetKey();
            System.Diagnostics.Debug.WriteLine("---------------------------"+LongLat);


            
            await FetchWeatherDataOnLoad(LongLat);
        }

        // Method to handle weather data fetching on page load
        private async Task FetchWeatherDataOnLoad(string q)
        {
            try
            {
                // Fetch weather data asynchronously
                string locationKey = await FetchLocationData(q);
                System.Diagnostics.Debug.WriteLine("dadddddddddddddddddddddddddddddddd" + locationKey);
                JObject key = JObject.Parse(locationKey);
                string location = (string)key["Key"];
                //System.Diagnostics.Debug.WriteLine("dadddddddddddddddddddddddddddddddd" + location);
                string data = await FetchWeatherData(location);
                System.Diagnostics.Debug.WriteLine("adadaadadadadadada"+data);


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
        private async Task<string> FetchWeatherData(string locationKey)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://dataservice.accuweather.com/currentconditions/v1/{locationKey}";
                // Build the complete URL with the API key, language, and details parameters
                string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=true";

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(fullUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and return the raw JSON data
                    string jsonData = await response.Content.ReadAsStringAsync();
                    //System.Diagnostics.Debug.WriteLine("data jsou--------------------"+jsonData);
                    return jsonData;
                }
                else
                {
                    // Handle error responses
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        private async Task<string> FetchLocationData(string q)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string fullUrl = $"{apiUrlLocation}?apikey={apiKey}&q={q}&language=en&details=true";

                HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonData = await response.Content.ReadAsStringAsync ();
                    //JArray data = JArray.Parse (jsonData);
                    //string locationKey = (string)data[0]["Key"];
                    //System.Diagnostics.Debug.WriteLine("Location key jeeeeeeee------"+locationKey);
                    return jsonData;
                }
                else
                {
                    return $"Error: {response.StatusCode}";
                }
            }
        }

        private async Task<string> GetKey()
        {
            Location location = await Geolocation.Default.GetLastKnownLocationAsync();

            q = location.Latitude.ToString() + "," + location.Longitude.ToString();

            return q;
        }
    }
}
