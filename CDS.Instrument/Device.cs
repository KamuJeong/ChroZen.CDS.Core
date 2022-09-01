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

        public abstract DeviceState State { get; init; }
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

        public abstract bool SetMethod(IMethod? method);
        public abstract void GetMethod(IMethod? method);
        internal protected abstract Task<bool> SendMethodAsync();
        internal protected abstract Task<bool> LoadMethodAsync();

        internal protected abstract bool Ready();
        internal protected abstract bool PreRun();
        internal protected abstract bool Run();
        internal protected abstract bool PostRun();
        internal protected abstract bool PostWork();

        internal protected abstract void Stop();
        internal protected abstract void Halt();
        internal protected abstract void Reset();
    }
}
