using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument.Tests
{
    public class DemoNotReadyDevice : Device
    {
        public DemoNotReadyDevice(ModelBase? parent, string? name) : base(parent, name)
        {
            State = new DemoDeviceState();
        }

        public override DeviceState State { get; init; }

        public override Task<bool> ConnectAsync()
        {
            ChangeStatus(DeviceStatus.NotReady);
            return base.ConnectAsync();
        }

        public override void GetMethod(IMethod? method)
        {
            
        }

        public override bool SetMethod(IMethod? method) => true;

        protected override void Halt() => ChangeStatus(DeviceStatus.NotReady);

        protected override Task<bool> LoadMethodAsync()
        {
            if(new[] { DeviceStatus.NotReady, DeviceStatus.Ready }.Contains(State.Status))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected override Task<bool> SendMethodAsync()
        {
            if (new[] { DeviceStatus.NotReady, DeviceStatus.Ready, DeviceStatus.PreRun }.Contains(State.Status))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }


        protected override bool PostRun() => false;

        protected override bool PostWork() => false;

        protected override bool PreRun() => false;

        protected override bool Ready() => false;

        protected override void Reset() => ChangeStatus(DeviceStatus.NotReady);

        protected override bool Run() => false;


        protected override void Stop() => ChangeStatus(DeviceStatus.NotReady);
    }
}
