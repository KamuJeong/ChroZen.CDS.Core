using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
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

    public class DeviceState
    {
        public DeviceStatus Status { get; internal set; }
    }
}
