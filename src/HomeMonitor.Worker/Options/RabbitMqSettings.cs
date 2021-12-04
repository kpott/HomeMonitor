using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Worker.Options;

public class RabbitMqSettings
{
    public const string ConfigurationSectionName = "RabbitMq";

    [Required] public Uri? Uri { get; set; }
    [Required] public string? Username { get; set; }
    [Required] public string? Password { get; set; }
}