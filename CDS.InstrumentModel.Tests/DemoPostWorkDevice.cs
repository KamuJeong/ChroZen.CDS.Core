using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel.Tests
{
    public class DemoPostWorkDevice : DemoNotReadyDevice
    {
        public DemoPostWorkDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        protected override bool PostWork()
        {
            ChangeStatus(DeviceStatus.PostWork);
            return true;
        }

        public void StopPostWork() => Stop();
    }
}
