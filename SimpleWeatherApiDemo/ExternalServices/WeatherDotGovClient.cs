using System.ComponentModel.DataAnnotations;

namespace SimpleWeatherApiDemo.ExternalServices;

public class WeatherDotGovClient
{
    private readonly HttpClient _httpClient;

    public WeatherDotGovClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Option<WeatherResponse>> GetWeatherForecastAsync(string gridPoints)
    {
        var response = await _httpClient.GetAsync($"/gridpoints/PHI/{gridPoints}/forecast");
        
        if (response.IsSuccessStatusCode == false)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>();
            return $"Failed to get weather forecast: {problemDetails?.Title ?? "Unknown error"}";
        }

        var deserialized = await response.Content.ReadFromJsonAsync<WeatherResponse>();
        if (deserialized != null) return deserialized;

        return "Failed to deserialize weather forecast.";
    }
}


public class WeatherDotGovOptions
{
    public static string ConfigurationSectionName = "WeatherDotGov";

    [Required]
    public string BaseUrl { get; set; }
    
    [Required]
    public string UserAgent { get; set; }
}

public class Option<TResult>
{
    public TResult? Value { get; }
    public string? ErrorMessage { get; }
    public bool HasValue { get; }

    public Option(TResult value)
    {
        Value = value;
        HasValue = true;
    }

    public Option(string errorMessage)
    {
        ErrorMessage = errorMessage;
        HasValue = false;
    }

    public static implicit operator Option<TResult>(TResult value) => new(value);
    public static implicit operator Option<TResult>(string errorMessage) => new(errorMessage);
}

public class WeatherResponse
{
    public string Geometry { get; set; }
    public string Units { get; set; }
    public string ForecastGenerator { get; set; }
    public DateTime GeneratedAt { get; set; }
    public DateTime UpdateTime { get; set; }
    public string ValidTimes { get; set; }
    public Elevation Elevation { get; set; }
    public Period[] Periods { get; set; }
}

public class Context
{
    public string @version { get; set; }
    public string wx { get; set; }
    public string geo { get; set; }
    public string unit { get; set; }
    public string @vocab { get; set; }
}

public class Elevation
{
    public string UnitCode { get; set; }
    public double Value { get; set; }
}

public class Period
{
    public int Number { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDaytime { get; set; }
    public int Temperature { get; set; }
    public string TemperatureUnit { get; set; }
    public string TemperatureTrend { get; set; }
    public ProbabilityOfPrecipitation ProbabilityOfPrecipitation { get; set; }
    public string WindSpeed { get; set; }
    public string WindDirection { get; set; }
    public string Icon { get; set; }
    public string ShortForecast { get; set; }
    public string DetailedForecast { get; set; }
}

public class ProbabilityOfPrecipitation
{
    public string UnitCode { get; set; }
    public object Value { get; set; }
}