using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument.Tests
{
    public class DemoPostRunDevice : DemoRunDevice
    {
        public DemoPostRunDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }
        public override void StopRun()
        {
            ChangeStatus(DeviceStatus.PostRun);
        }

        protected override bool PostRun()
        {
            // Do postrun
            return true;
        }

        public virtual void StopPostRun() => Stop();

        protected override bool PostWork() => false;

        protected override bool PreRun() => false;
    }
}
