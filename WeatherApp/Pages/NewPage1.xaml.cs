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

        string location = await GetCity(searchQuery);

        await FetchWeatherDataAndUpdateUI(location);

        LocationLabel.Text = searchQuery;
        
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }

    private async Task FetchWeatherDataAndUpdateUI(string locationKey)
    {
        using (HttpClient client = new HttpClient())
        {
            string apiUrl = $"https://dataservice.accuweather.com/forecasts/v1/daily/5day/{locationKey}";
            string fullUrl = $"{apiUrl}?apikey={apiKey}&language=en&details=false&metric=true";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();

                //JArray jsonArray = JArray.Parse(jsonData);
                //JObject weatherData = (JObject)jsonArray[0];

                JObject weatherData = JObject.Parse(jsonData);
                JArray dailyForecasts = (JArray)weatherData["DailyForecasts"];

                string date1 = DateTime.Parse((string)dailyForecasts[0]["Date"]).ToString("dd.MM");
                string date2 = DateTime.Parse((string)dailyForecasts[1]["Date"]).ToString("dd.MM");
                string date3 = DateTime.Parse((string)dailyForecasts[2]["Date"]).ToString("dd.MM");
                string date4 = DateTime.Parse((string)dailyForecasts[3]["Date"]).ToString("dd.MM");
                string date5 = DateTime.Parse((string)dailyForecasts[4]["Date"]).ToString("dd.MM");

                string min1 = (string)dailyForecasts[0]["Temperature"]["Minimum"]["Value"];
                string max1 = (string)dailyForecasts[0]["Temperature"]["Maximum"]["Value"];
                string min2 = (string)dailyForecasts[1]["Temperature"]["Minimum"]["Value"];
                string max2 = (string)dailyForecasts[1]["Temperature"]["Maximum"]["Value"];
                string min3 = (string)dailyForecasts[2]["Temperature"]["Minimum"]["Value"];
                string max3 = (string)dailyForecasts[2]["Temperature"]["Maximum"]["Value"];
                string min4 = (string)dailyForecasts[3]["Temperature"]["Minimum"]["Value"];
                string max4 = (string)dailyForecasts[3]["Temperature"]["Maximum"]["Value"];
                string min5 = (string)dailyForecasts[4]["Temperature"]["Minimum"]["Value"];
                string max5 = (string)dailyForecasts[4]["Temperature"]["Maximum"]["Value"];

                string icon1 = (string)dailyForecasts[0]["Day"]["Icon"];
                string icon2 = (string)dailyForecasts[1]["Day"]["Icon"];
                string icon3 = (string)dailyForecasts[2]["Day"]["Icon"];
                string icon4 = (string)dailyForecasts[3]["Day"]["Icon"];
                string icon5 = (string)dailyForecasts[4]["Day"]["Icon"];

                DayImageFirst.Source = PictureDecisionMaker(icon1);
                DayImageSecond.Source = PictureDecisionMaker(icon2);
                DayImageThird.Source = PictureDecisionMaker(icon3);
                DayImageFourth.Source = PictureDecisionMaker(icon4);
                DayImageFifth.Source = PictureDecisionMaker(icon5);

                string final1 = $"{date1} Min: {min1} Max: {max1} ";
                string final2 = $"{date2} Min: {min2} Max: {max2} ";
                string final3 = $"{date3} Min: {min3} Max: {max3} ";
                string final4 = $"{date4} Min: {min4} Max: {max4} ";
                string final5 = $"{date5} Min: {min5} Max: {max5} ";

                DayLabelFirst.Text = final1;
                DayLabelSecond.Text = final2;
                DayLabelThird.Text = final3;
                DayLabelFourth.Text = final4;
                DayLabelFifth.Text = final5;

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

    private string PictureDecisionMaker(string numOfPic)
    {
        //string imagePath = "Resources/Images/";

        var imageDictionary = new Dictionary<string, string>
            {
                { "1", "img1.png" },
                { "2", "img2.png" },
                { "3", "img3.png" },
                { "4", "img4.png" },
                { "6", "img6.png" },
                { "7", "img7.png" },
                { "8", "img8.png" },
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
