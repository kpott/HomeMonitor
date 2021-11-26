namespace HomeMonitor.Worker.Options;

public class RabbitMqSettings
{
    public const string ConfigurationSectionName = "RabbitMq";

    public Uri Uri { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}