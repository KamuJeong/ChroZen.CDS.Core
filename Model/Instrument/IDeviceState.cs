using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core.Model
{
    public enum DeviceStatus
    {
        None,
        NotReady,
        Ready,
        PreRun,
        Run,
        PostRun,
        PostWork,
        Error,
    }

    public interface IDeviceState
    {
        DeviceStatus Status { get; set; }
        object Inform { get; set; }
        object Configuration { get; set; }
        object Method { get; set; }
        object Monitor { get; set; }
    }
}
