using Microsoft.AspNetCore.Http;
using SimpleWeatherApiDemo.ExternalServices;

namespace SimpleWeatherApiDemo.Weather;

public class WeatherCache
{
    private readonly WeatherDotGovClient _apiClient;
    private Dictionary<string, WeatherCacheItem> _weatherDictionary;
    private object _lockObj = new object();

    public WeatherCache(WeatherDotGovClient apiClient)
    {
        _weatherDictionary = new Dictionary<string, WeatherCacheItem>();
        _apiClient = apiClient;
    }

    public async Task<WeatherResponse> GetOrAdd(string location)
    {
        if (!_weatherDictionary.TryGetValue(location, out var weatherItem) || weatherItem.Generated < DateTime.Now.AddMinutes(-15))
        {
            var coordinates = CoordinateProvider.Instance.GetCoordinates(location);
            var response = await _apiClient.GetWeatherForecastAsync(coordinates);

            if (!response.HasValue)
            {
                throw new Exception(response.ErrorMessage!);
            }

            lock (_lockObj)
            {
                weatherItem = new WeatherCacheItem
                {
                    Weather = response.Value,
                    Generated = DateTime.Now
                };
                _weatherDictionary[location] = weatherItem;
            }
        }
        return weatherItem.Weather;
    }
}

public class WeatherCacheItem
{
    public WeatherResponse Weather { get; set; }
    public DateTime Generated { get; set; }
}