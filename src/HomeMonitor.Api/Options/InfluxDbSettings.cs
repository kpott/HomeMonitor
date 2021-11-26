namespace HomeMonitor.Api.Options;

public class InfluxDbSettings
{
    public const string ConfigurationSectionName = "InfluxDb";

    public Uri Uri { get; set; }
    public string Token { get; set; }
}