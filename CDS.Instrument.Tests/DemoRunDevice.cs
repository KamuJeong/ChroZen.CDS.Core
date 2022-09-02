using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument.Tests
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
            ChangeStatus(DeviceStatus.Ready);
            return true;
        }

        protected override bool Run()
        {
            ChangeStatus(DeviceStatus.Run);
            return true;
        }

        public virtual void StopRun() => Stop();
    }
}
