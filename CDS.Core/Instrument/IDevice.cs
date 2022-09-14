using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface IDevice
    {
        String? SerialNumber { get; }
        Uri? Uri { get; }
        String? Model { get; }       

        DeviceStatus Status { get; }
    }
}
