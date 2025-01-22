using SimpleWeatherApiDemo.ExternalServices;

namespace SimpleWeatherApiDemo.Weather;

public class CoordinateProvider
{
    private Dictionary<string, string> _locationCoordinates;

    private static CoordinateProvider _instance;
    public static CoordinateProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CoordinateProvider();
            }
            return _instance;
        }
    }

    public CoordinateProvider()
    {
        _locationCoordinates = new Dictionary<string, string>
        {
            { "Chicago", "76,73" },
            { "New York", "33,35" },
            { "Miami", "110,50" },
            { "Seattle", "125,68" },
            { "Houston", "65,97" },
            { "Denver", "63,64" }
        };
    }

    public bool IsValid(string location)
    {
        return _locationCoordinates.ContainsKey(location);
    }

    public string GetCoordinates(string location)
    {
        return _locationCoordinates[location];
    }
}