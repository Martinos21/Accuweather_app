
using key;
using Newtonsoft.Json.Linq;

namespace WeatherApp.Pages;

public partial class NewPage1 : ContentPage
{
    private string searchQuery;
    private readonly Settings mySettings = new Settings();
    private readonly string apiKey;

    public NewPage1()
	{
        apiKey = mySettings.ApiKey;
        InitializeComponent();
	}

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // Retrieve the text from the SearchBar and save it to the variable
        searchQuery = SearchBarInput.Text;

        // Optional: Display a message or use the searchQuery variable for further logic
        DisplayAlert("Search Query Saved", $"You entered: {searchQuery}", "OK");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        
    }

    private async Task GetCity(string cityName)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"http://dataservice.accuweather.com/locations/v1/cities/search";
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


}