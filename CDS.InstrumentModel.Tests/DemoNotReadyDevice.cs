using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel.Tests
{
    public class DemoNotReadyDevice : Device
    {
        public override TimeSpan RunTime => TimeSpan.Zero;

        public DemoNotReadyDevice(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        public override Task<bool> ConnectAsync(CancellationToken token)
        {
            Status = DeviceStatus.NotReady;
            return Task.FromResult(true);
        }

        public override void Disconnect()
        {
        }

        public override void GetMethod(IMethod? method)
        {
            
        }

        protected override void Halt() => Status = DeviceStatus.NotReady;

        protected override Task<bool> LoadMethodAsync(IMethod? method)
        {
            if(new[] { DeviceStatus.NotReady, DeviceStatus.Ready }.Contains(Status))
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        protected override bool SendMethod(IMethod? method)
        {
            if (new[] { DeviceStatus.NotReady, DeviceStatus.Ready, DeviceStatus.PreRun }.Contains(Status))
            {
                return true;
            }
            return false;
        }


        protected override bool PostRun() => false;

        protected override bool PostWork() => false;

        protected override bool PreRun() => false;

        protected override bool Ready() => false;

        protected override void Reset() => Status = DeviceStatus.NotReady;

        protected override bool Run() => false;


        protected override void Stop() => Status = DeviceStatus.NotReady;

        protected override bool CheckReadyStatus() => false;
    }
}
