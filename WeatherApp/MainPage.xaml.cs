using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors; // Required for Geolocation
using Newtonsoft.Json.Linq;


namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        private readonly string apiKey = "CpHhhtXACFLd3NMlTwHEhpYCRRuBv7qC"; // Replace with your actual API key

        public MainPage()
        {
            InitializeComponent();
            FetchAndDisplayWeatherData();
        }

        // Method to fetch and display weather data
        private async void FetchAndDisplayWeatherData()
        {
            string q = await GetLocationCoordinates();
            Console.WriteLine(q + "a-------------------------------------------------------------");

            if (q != null)
            {
                // Call a method to fetch the location key based on latitude and longitude
                var (locationKey, localizedName) = await FetchLocationKey(q);


                // Now use the location key to fetch the weather data
                await FetchWeatherDataAndUpdateUI(locationKey);

                // Display the raw JSON response in the label
                //JsonResponseLabel.Text = jsonData;
                LocationLabel.Text = localizedName;
            }
            else
            {
                // Handle case where location is unavailable
                await DisplayAlert("Error", "Unable to fetch location.", "OK");
            }
        }

        // Method to fetch weather data from the API
        private async Task FetchWeatherDataAndUpdateUI(string locationKey)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://dataservice.accuweather.com/currentconditions/v1/{locationKey}";
                string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=true";

                // Make the GET request
                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response
                    string jsonData = await response.Content.ReadAsStringAsync();

                    // Parse the JSON response (the response is an array)
                    JArray jsonArray = JArray.Parse(jsonData);
                    JObject weatherData = (JObject)jsonArray[0];  // Access the first item in the array

                    // Extract required fields
                    double temperature = (double)weatherData["Temperature"]["Metric"]["Value"];
                    int weatherIcon = (int)weatherData["WeatherIcon"];
                    int relativeHumidity = (int)weatherData["RelativeHumidity"];
                    double windSpeed = (double)weatherData["Wind"]["Speed"]["Metric"]["Value"];
                    string windUnit = (string)weatherData["Wind"]["Speed"]["Metric"]["Unit"];
                    int uvIndex = (int)weatherData["UVIndex"];

                    // Assign each value to its respective label
                    TemperatureLabel.Text = $"{temperature}°C";
                    //WeatherIconLabel.Text = $"Weather Icon: {weatherIcon}";
                    WeatherPicture.Source = await PictureDecisionMaker(weatherIcon.ToString());
                    HumidityLabel.Text = $"Humidity: {relativeHumidity}%";
                    WindSpeedLabel.Text = $"Wind Speed: {windSpeed} {windUnit}";
                    UVIndexLabel.Text = $"UV Index: {uvIndex}";
                }
                else
                {
                    // Handle error responses
                    await DisplayAlert("Error", $"Unable to fetch weather data: {response.StatusCode}", "OK");
                }
            }
        }


        // Method to get the current location coordinates (latitude, longitude)
        private async Task<string> GetLocationCoordinates()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location == null)
                {
                    // Handle the case where location is not available
                    await DisplayAlert("Error", "Location is unavailable. Please check location services.", "OK");
                    return null;
                }

                // Combine latitude and longitude into a single string format
                string q = $"{location.Latitude},{location.Longitude}";
                return q;
            }
            catch (Exception ex)
            {
                // Handle any exceptions related to geolocation
                await DisplayAlert("Error", $"An error occurred while retrieving location: {ex.Message}", "OK");
                return null;
            }
        }

        // Method to fetch the location key based on latitude and longitude
        private async Task<(string LocationKey, string LocalizedName)> FetchLocationKey(string coordinates)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://dataservice.accuweather.com/locations/v1/cities/geoposition/search";
                string fullUrl = $"{apiUrl}?apikey={apiKey}&q={coordinates}&language=en";

                // Make the GET request to fetch the location key
                HttpResponseMessage response = await client.GetAsync(fullUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response
                    string jsonData = await response.Content.ReadAsStringAsync();

                    // Parse the JSON data to extract the 'Key' and 'LocalizedName' fields
                    JObject jsonObject = JObject.Parse(jsonData);

                    // Extract the location key
                    string locationKey = (string)jsonObject["Key"];

                    // Extract the localized name (e.g., city name)
                    string localizedName = (string)jsonObject["LocalizedName"];

                    return (locationKey, localizedName);
                }
                else
                {
                    // Handle error responses
                    await DisplayAlert("Error", $"Unable to fetch location key: {response.StatusCode}", "OK");
                    return (null, null);
                }
            }
        }
        /*private string PictureDecisionMaker(string iconNum)
        {
            // Ensure iconNum is a valid number and within range
            if (int.TryParse(iconNum, out int iconNumber) && iconNumber >= 1 && iconNumber <= 44)
            {
                // Construct the image file path
                return $"@Resources/Images/img{iconNumber}.png";  // Make sure the path is correct
            }

            // Return a default image or placeholder in case of an invalid icon number
            return "@Resources/Images/img1.png"; // Default or placeholder image
        }*/
        private async Task<string> PictureDecisionMaker(string numOfPic)
        {
            // Define the base path to the images
            string imagePath = "Resources/Images/";

            // Create a dictionary to map numbers to corresponding image filenames
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

            // Check if the input exists in the dictionary and return the image path
            if (imageDictionary.TryGetValue(numOfPic, out string fileName))
            {
                return fileName;
            }

            // If no matching image is found, return a default or error message
            return "Image not found";
        }
    }
}