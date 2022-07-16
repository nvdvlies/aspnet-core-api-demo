using Microsoft.Extensions.Logging;

namespace Demo.WebApi.Models;

public class LogMessage
{
    public LogLevel LogLevel { get; set; }
    public int EventId { get; set; }
    public string Message { get; set; }
    public string ExceptionMessage { get; set; }
}
