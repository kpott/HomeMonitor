using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Worker.Options;

public class RabbitMQ
{
    public const string ConfigurationSectionName = "RabbitMQ";
    
    [Required]
    public Uri Uri { get; set; }
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}