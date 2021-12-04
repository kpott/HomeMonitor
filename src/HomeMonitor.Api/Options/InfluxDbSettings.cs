using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Api.Options;

public class InfluxDbSettings
{
    public const string ConfigurationSectionName = "InfluxDb";

    [Required] public Uri? Uri { get; set; }
    [Required] public string? Token { get; set; }
}