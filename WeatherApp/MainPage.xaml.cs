using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        // AccuWeather API URL and key
        private string apiUrl = "http://dataservice.accuweather.com/currentconditions/v1/1380799";
        private string apiKey = "CpHhhtXACFLd3NMlTwHEhpYCRRuBv7qC";

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

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
                WeatherDataLabel.Text = data;
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                WeatherDataLabel.Text = $"Error: {ex.Message}";
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
