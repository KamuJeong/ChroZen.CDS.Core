using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel.Tests
{
    public class DemoRunDevice : DemoReadyDevice
    {
        public DemoRunDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        protected override bool PostRun() => false;

        protected override bool PostWork() => false;

        protected override bool PreRun() => false;

        protected override bool Ready()
        {
            Status = DeviceStatus.Ready;
            return true;
        }

        protected override bool Run()
        {
            Status = DeviceStatus.Run;
            return true;
        }

        public virtual void StopRun() => Stop();
    }
}
