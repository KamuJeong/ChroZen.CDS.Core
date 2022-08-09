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
        IDeviceState State { get; init; }

        Uri Uri { get; set; }

        Task<bool> ConnectAsync();
        void Disconnect();

        Task<object?> GetMethodAsync();
    }
}
