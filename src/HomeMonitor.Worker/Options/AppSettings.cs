using System.ComponentModel.DataAnnotations;

namespace HomeMonitor.Worker.Options;

public class AppSettings
{
    public const string ConfigurationSectionName = "AppSettings";
    
    [Required]
    public string Location { get; set; }
}