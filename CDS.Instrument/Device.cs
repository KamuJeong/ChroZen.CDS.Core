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
        internal abstract Task<bool> SendMethodAsync();
        internal abstract Task<bool> LoadMethodAsync();

        internal abstract bool Ready();
        internal abstract bool PreRun();
        internal abstract bool Run();
        internal abstract bool PostRun();
        internal abstract bool PostWork();

        internal abstract void Stop();
        internal abstract void Halt();
        internal abstract void Reset();
    }
}
