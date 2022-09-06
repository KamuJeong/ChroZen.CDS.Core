using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel.Tests
{
    public class DemoPreRunDevice : DemoReadyDevice
    {
        public DemoPreRunDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        protected override bool PostRun() => false;

        protected override bool PostWork() => false;

        protected override bool PreRun()
        {
            ChangeStatus(DeviceStatus.PreRun);
            return true;
        }

        protected override bool Run() => false;
    }
}
