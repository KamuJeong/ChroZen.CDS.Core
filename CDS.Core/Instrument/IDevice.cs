﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface IDevice
    {
        string? Name { get; }

        string? SerialNumber { get; }
        Uri? Uri { get; }
        string? Model { get; }       

        DeviceStatus Status { get; }

        TimeSpan RunTime { get; }
    }
}
