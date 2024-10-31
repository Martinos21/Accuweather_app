using key;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Pages;

public partial class NewPage1 : ContentPage
{
    private string searchQuery = "";
    private readonly Settings mySettings = new Settings();
    private readonly string apiKey;

    public NewPage1()
    {
        apiKey = mySettings.ApiKey;
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        searchQuery = SearchBarInput.Text;

        string test = await GetCity(searchQuery);


        
        await DisplayAlert("Search Query Saved", $"You entered: {test}", "OK");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async Task FetchWeatherDataAndUpdateUI(string locationKey)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"http://dataservice.accuweather.com/forecasts/v1/daily/5day/{locationKey}";
            string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=true";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();

                JArray jsonArray = JArray.Parse(jsonData);
                JObject weatherData = (JObject)jsonArray[0];

 

            }
            else
            {
                await DisplayAlert("Error", $"Unable to fetch weather data: {response.StatusCode}", "OK");
            }
        }
    }

    private async Task<string> GetCity(string cityName)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"https://dataservice.accuweather.com/locations/v1/cities/search";
            string fullUrl = $"{apiUrl}?apikey={apiKey}&q={cityName}&language=en&details=true";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();

                JArray jsonArray = JArray.Parse(jsonData);
                JObject cityData = (JObject)jsonArray[0];
                string key = (string)cityData["Key"];
                return key;
            }
            else
            {
                await DisplayAlert("Error", $"Unable to fetch weather data: {response.StatusCode}", "OK");
                return null;
            }
        }
    }
}
