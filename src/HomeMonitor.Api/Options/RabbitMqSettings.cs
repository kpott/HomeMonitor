using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Api.Options;

public class RabbitMqSettings
{
    public const string ConfigurationSectionName = "RabbitMq";

    [Required] public Uri? Uri { get; set; }
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}