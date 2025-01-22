using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SimpleWeatherApiDemo.ExternalServices;
using SimpleWeatherApiDemo.Weather;

namespace SimpleWeatherApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KennettWeatherForecastController : ControllerBase
    {
        private WeatherCache _weatherCache;

        public KennettWeatherForecastController(WeatherCache weatherCache)
        {
            _weatherCache = weatherCache;
        }

        //[HttpGet(Name = "GetKennettWeatherForecast")]
        //[ProducesResponseType<WeatherResponse>(200)]
        //[ProducesResponseType<ProblemDetails>(500)]
        //public async Task<IActionResult> GetKennetWeather()
        //{
        //    var apiResult = await _apiClient.GetWeatherForecastAsync($"{32},{68}");

        //    if (apiResult.HasValue)
        //    {
        //        return Ok(apiResult.Value!);
        //    }

        //    return Problem(apiResult.ErrorMessage!, statusCode: 500);
        //}

        [HttpGet(Name = "GetWeatherForcast")]
        [ProducesResponseType<WeatherResponse>(200)]
        [ProducesResponseType<ProblemDetails>(500)]
        [ProducesResponseType<NotFound>(404)]
        public async Task<IActionResult> GetWeatherForcast(string location)
        {
            if (!CoordinateProvider.Instance.IsValid(location))
            {
                return NotFound($"{location} is not a valid location.");
            }

            try
            {
                var weather = await _weatherCache.GetOrAdd(location);
                return Ok(weather);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
    }
}
