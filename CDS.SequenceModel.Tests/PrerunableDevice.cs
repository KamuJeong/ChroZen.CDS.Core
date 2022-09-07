using CDS.Core;
using CDS.InstrumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.SequenceModel.Tests
{
    internal class PrerunableDevice : Device
    {
        public PrerunableDevice(ModelBase? parent, string? name) : base(parent, name)
        {
            State = new DeviceState();
        }

        public override DeviceState State { get; init; }

        public override Task<bool> ConnectAsync(CancellationToken token)
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

            if(Parent is Instrument instrument && State.Status == DeviceStatus.PreRun)
            {
                instrument.Run();
            }
        }


        public override void GetMethod(IMethod? method)
        {
        }

        public override bool SetMethod(IMethod? method) => true;

        protected override void CheckReadyStatus()
        {
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

        protected override bool PreRun()
        {
            ChangeStatus(DeviceStatus.PreRun);
            return true;
        }

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
            ChangeStatus(DeviceStatus.NotReady);
            return false;
        }

        protected override bool SendMethod() => true;

        protected override void Stop()
        {
        }
    }
}
