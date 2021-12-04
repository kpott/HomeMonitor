using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Worker.Options;

public class MonitorSettings
{
    public const string ConfigurationSectionName = "Monitor";

    [Required] public string Location { get; set; } = "Location Not Configured";
    [Required] public int DelayBetweenMeasurementsMs { get; set; } = 300000;
    [Required] public string SensorType { get; set; } = "bme280";
}