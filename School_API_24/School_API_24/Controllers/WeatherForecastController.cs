﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace School_API_24.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private ILoggerManager _logger;

        public WeatherForecastController(IRepositoryManager repository)
        {
            _repository = repository;
        }

        /*[HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //inject methods here
           // _repository.Organization.AnyMethodFromOrganizationRepository();
            // _repository.User.AnyMethodFromUserRepository();

            return new string[] { "value1", "value2"};
        }*/


        public WeatherForecastController(ILoggerManager logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogInfo("Here is info message from our values controller.");
            _logger.LogDebug("Here is debug message from our values controller.");
            _logger.LogWarn("Here is warn message from our values controller.");
            _logger.LogError("Here is an error message from our values controller.");

            return new string[] { "value1", "value2" };

        }
    }
}
