using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _90sWeatherApp
{
    public partial class MainPage : ContentPage
    {
        private const string ApiKey = "tIBQGz0Mhc2fYTLtx55c6jWCFROk2ZBu";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            var city = CityEntry.Text;
            if (!string.IsNullOrEmpty(city))
            {
                var (weatherTemp,weatherType, weatherTime) = await GetWeatherAsync(city);
                WeatherLabel.Text = "It is currently " + weatherTemp.ToString() + " °F and " + weatherType ?? "Unable to get weather data";
            }
        }

        private async Task<(double weatherTemp, String weatherType, DateTime currentTime)> GetWeatherAsync(string city)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Example API request URL
                    var url = $"https://api.tomorrow.io/v4/timelines?location={city}&fields=temperature,weatherCode&timeSteps=current&apikey={ApiKey}";

                    var response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JObject.Parse(json);

                        // Extract current temperature in Celsius from the response
                        var currentTemperatureCelsius = Convert.ToDouble(data["data"]["timelines"][0]["intervals"][0]["values"]["temperature"].ToString());

                        // Convert Celsius to Fahrenheit
                        var weatherTemp = Math.Round((currentTemperatureCelsius * 9 / 5) + 32);

                        // Extract weather type code and translate to human-readable description
                        var weatherCode = data["data"]["timelines"][0]["intervals"][0]["values"]["weatherCode"].ToString();
                        var weatherType = GetWeatherTypeFromCode(weatherCode);

                        // Extract current timestamp (UTC)
                        var weatherTime = DateTime.UtcNow;

                        // Build the weather response message
                        var responseMessage = (weatherTemp,weatherType,weatherTime);
                        return responseMessage;
                    }
                    else
                    {
                        // Handle HTTP errors
                        throw new HttpRequestException($"HTTP Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        private string GetWeatherTypeFromCode(string weatherCode)
        {
            // Map weather code to descriptive weather type (example mapping)
            switch (weatherCode)
            {
                case "1000":
                    return "Clear, Sunny";
                case "1100":
                    return "Mostly Clear";
                case "1101":
                    return "Partly Cloudy";
                case "1102":
                    return "Mostly Cloudy";
                case "1001":
                    return "Cloudy";
                case "2000":
                    return "fog";
                default:
                    return "Unknown";
            }
        }
    }
}