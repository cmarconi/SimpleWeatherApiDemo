using Microsoft.AspNetCore.Mvc;
using SimpleWeatherApiDemo.ExternalServices;

namespace SimpleWeatherApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KennettWeatherForecastController : ControllerBase
    {
        private readonly WeatherDotGovClient _apiClient;

        public KennettWeatherForecastController(WeatherDotGovClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet(Name = "GetKennettWeatherForecast")]
        [ProducesResponseType<WeatherResponse>(200)]
        [ProducesResponseType<ProblemDetails>(500)]
        public async Task<IActionResult> GetKennetWeather()
        {
            var apiResult = await _apiClient.GetWeatherForecastAsync($"{32},{68}");

            if (apiResult.HasValue)
            {
                return Ok(apiResult.Value!);
            }

            return Problem(apiResult.ErrorMessage!, statusCode: 500);
        }
    }
}
