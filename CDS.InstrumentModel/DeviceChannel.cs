using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public sealed class DeviceChannel
    {
        public string? Unit { get; init; }
        public double[]? AvailableHz { get; init; }
    }
}
