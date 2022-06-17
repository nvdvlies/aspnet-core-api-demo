using System;
using System.Net;
using Demo.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.WebApi.Controllers
{
    public class LogController : ApiControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
        public ActionResult Post([FromBody] LogMessage[] logMessages)
        {
            if (logMessages != null)
            {
                foreach (var logMessage in logMessages)
                {
                    _logger.Log(
                        logMessage.LogLevel,
                        new EventId(logMessage.EventId),
                        logMessage.ExceptionMessage != null ? new Exception(logMessage.ExceptionMessage) : null,
                        logMessage.Message);
                }
            }

            return Ok();
        }
    }
}