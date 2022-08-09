using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core.Model
{
    public interface IDevice
    {
        string Name { get; set; }
        string Model { get; set; }
        IDeviceState State { get; }
    }
}
