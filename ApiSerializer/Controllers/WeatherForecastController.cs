using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSerializer.Controllers
{
    [ApiController]
    [Route("W")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("A")]
        public async Task<IActionResult> Ambiente()
        {
            Dictionary<string, object> settings = _configuration.GetSection("Conf").Get<Dictionary<string, object>>();

            _logger.LogInformation(settings["Nombre"].ToString());

            return new JsonResult(settings);
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Stopwatch tiempo = new Stopwatch();
            tiempo.Start();
            var rng = new Random();

            var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            tiempo.Stop();



            _logger.LogInformation($"Tiempo de respuesta para {Request.Headers["Accept"]} es {tiempo.ElapsedMilliseconds}");
            _logger.LogInformation($"Estoy en el ambiente <<< {_configuration.GetValue<string>("Conf:Nombre")} >>>");
            return res;
        }
    }
}
