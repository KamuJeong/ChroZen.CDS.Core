using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument
{
    public abstract class Device : ModelBase, IDevice
    {
        public Device(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        public abstract IDeviceState State { get; init; }
        protected void ChangeStatus(DeviceStatus status)
        {
            if(State.Status != status)
            {
                State.Status = status;
                (Parent as Instrument)?.InvokeStatusChangedEvent(this);
            }
        }

        public Uri? Uri { get; set; }
        public abstract Task<bool> ConnectAsync();
        public abstract void Disconnect();

        public abstract Task<object?> GetMethodAsync();

        public abstract bool Ready();
        public abstract bool PreRun();
        public abstract bool Run();
        public abstract bool PostRun();
        public abstract bool PostWork();

        public abstract void Stop();
        public abstract void Halt();
        public abstract void Reset();
    }
}
