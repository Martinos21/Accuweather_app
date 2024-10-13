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
                string iconNum = (string)weatherdata[0]["WeatherIcon"];
                string pic = await PictureDecisionMaker(iconNum);
                //System.Diagnostics.Debug.WriteLine(iconNum);
                string locatioName = link.Split('/')[5];
                

                LocationName.Text = locatioName[0].ToString().ToUpper()+locatioName.Substring(1);
                Temperature.Text = temp.ToString()+ "°C";
                WeatherPicture.Source = pic;
            }
            catch (Exception ex)
            {
                // Display error message if something goes wrong
                Temperature.Text = $"Error: {ex.Message}";
            }
        }

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
