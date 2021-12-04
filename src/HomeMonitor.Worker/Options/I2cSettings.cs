using Iot.Device.Si7021;

namespace HomeMonitor.Worker.Options;

public class I2CSettings
{
    public const string ConfigurationSectionName = "I2C";

    public int BusId { get; set; } = 1;
    public int DeviceAddress { get; set; } = Si7021.DefaultI2cAddress;
}