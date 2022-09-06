using CDS.Core;
using CDS.InstrumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.SequenceModel.Tests
{
    internal class RunableDevice : Device
    {
        public RunableDevice(ModelBase? parent, string? name) : base(parent, name)
        {
            State = new DeviceState();
        }

        public override DeviceState State { get; init; }

        public override Task<bool> ConnectAsync()
        {
            ChangeStatus(DeviceStatus.NotReady);
            return Task.FromResult(true);
        }

        public override void Disconnect()
        {
            
        }

        protected override void OnTimerTick(TimeSpan ts)
        {
            base.OnTimerTick(ts);

            if(Parent is Instrument instrument && State.Status == DeviceStatus.Run)
            {
                if(instrument.State.ElapsedTime.TotalSeconds >= 0.2)
                {
                    Ready();
                }
            }
        }

        public override void GetMethod(IMethod? method)
        {
            throw new NotImplementedException();
        }

        public override bool SetMethod(IMethod? method) => true;

        protected override void CheckReadyStatus()
        {
            // nothing to do
        }

        protected override void Halt()
        {

        }

        protected override Task<bool> LoadMethodAsync()
        {
            throw new NotImplementedException();
        }

        protected override bool PostRun() => false;

        protected override bool PostWork() => false;

        protected override bool PreRun() => false;

        protected override bool Ready()
        {
            ChangeStatus(DeviceStatus.Ready);
            return true;
        }

        protected override void Reset()
        {
        }

        protected override bool Run()
        {
            ChangeStatus(DeviceStatus.Run);
            return true;
        }

        protected override Task<bool> SendMethodAsync() => Task.FromResult(true);

        protected override void Stop()
        {
        }
    }
}
