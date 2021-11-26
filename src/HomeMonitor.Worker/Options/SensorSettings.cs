using Iot.Device.Si7021;

namespace HomeMonitor.Worker.Options;

public class SensorSettings
{
    public const string ConfigurationSectionName = "Sensor";

    public string Location { get; set; }

    public int DelayBetweenMeasurementsMs { get; set; } = 300000;
    public int I2CBusId { get; set; } = 1;

    public int I2CDeviceAddress { get; set; } = Si7021.DefaultI2cAddress;
}