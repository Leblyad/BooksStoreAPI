using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BooksStore
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherForecast : ControllerBase
    {
        private ILoggerManager _logger;

        public WeatherForecast(ILoggerManager logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Here is info message from our values conroller.");
            _logger.LogDebug("Here is debug message from our values conroller.");
            _logger.LogWarn("Here is warn message from our values conroller.");
            _logger.LogError("Here is erro message from our values conroller.");

            return new string[] { "value1", "value2" };
        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
