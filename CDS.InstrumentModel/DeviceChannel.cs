using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public enum DeviceChannelType
    {
        None,
        Chromatogram,
        Pressure,
        Flow,
        Temperature,
        Spectra,
    }

    public sealed class DeviceChannel
    {
        public string? Name { get; init; }
        public string? Unit { get; init; }
        public DeviceChannelType Type { get; init; }
        public double[]? AvailableHz { get; init; }
    }
}
