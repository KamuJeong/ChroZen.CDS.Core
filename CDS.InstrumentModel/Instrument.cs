using CDS.Core;

namespace CDS.InstrumentModel
{
    public class Instrument : ModelBase, IInstrument
    {
        public Instrument(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        public InstrumentState State { get; } = new InstrumentState();
        IInstrumentState IInstrument.State => State;

        public event EventHandler<InstrumentStatus>? StatusChanged;
        internal void InvokeStatusChangedEvent(object sender, InstrumentStatus oldStatus = InstrumentStatus.None)
        {
            StatusChanged?.Invoke(sender, oldStatus);

            if (sender is Device device)
            {
                if (device.Status == DeviceStatus.Error)
                {
                    foreach (var d in Devices)
                        d.HaltWrap();

                    ChangeStatus(InstrumentStatus.Error);
                }
                else if (State.Status == InstrumentStatus.Ready && device.Status == DeviceStatus.NotReady)
                {
                    ChangeStatus(InstrumentStatus.NotReady);
                }
                else if (State.Status == InstrumentStatus.NotReady && device.Status == DeviceStatus.Ready)
                {
                    if (Devices.All(d => d.Status == DeviceStatus.Ready))
                    {
                        ChangeStatus(InstrumentStatus.Ready);
                    }
                }
                else if (State.Status == InstrumentStatus.Run)
                {
                    if (Devices.All(d => d.Status != DeviceStatus.Run))
                    {
                        PostRun();
                    }
                }
                else if (State.Status == InstrumentStatus.PostRun)
                {
                    if (Devices.All(d => d.Status != DeviceStatus.PostRun))
                    {
                        PostWork();
                    }
                }
                else if (State.Status == InstrumentStatus.PostWork)
                {
                    if (Devices.All(d => d.Status != DeviceStatus.PostWork))
                    {
                        NextInjectOrFinish();
                    }
                }
            }
        }

        private void ChangeStatus(InstrumentStatus status)
        {
            if (status != State.Status)
            {
                (State.Status, status) = (status, State.Status);
                InvokeStatusChangedEvent(this, status);

                if (State.Status == InstrumentStatus.NotReady)
                {
                    State.SequenceItem = null;

                    State.IsSingleShot = true;
                    State.StopReserved = false;
                    State.HaltReserved = false;
                    State.HaltAfterSequenceRun = false;
                }
            }
        }

        public IEnumerable<Device> Devices => FindChildren<Device>(null);
        IEnumerable<IDevice> IInstrument.Devices => Devices;

        public IEnumerable<SignalSet> Signals => FindChildren<SignalSet>(null);
        IEnumerable<ISignalSet> IInstrument.Signals => Signals;

        public async Task<bool> ConnectAsync()
        {
            ChangeStatus(InstrumentStatus.NotReady);

            return (await Task.WhenAll(Devices.Select(d => d.ConnectAsyncWrap()))).All(r => r);
        }

        public void Disconnect()
        {
            ChangeStatus(InstrumentStatus.None);

            foreach (var d in Devices)
                d.DisconnectWrap();
        }

        public void Halt()
        {
            ChangeStatus(InstrumentStatus.NotReady);

            foreach (var d in Devices)
                d.HaltWrap();
        }

        public void Ready()
        {
            if (State.Status == InstrumentStatus.NotReady)
            {
                SendMethod();

                foreach (var d in Devices)
                    d.Ready();
            }
        }

        public void Reset()
        {
            if (State.Status == InstrumentStatus.Error)
            {
                ChangeStatus(InstrumentStatus.NotReady);

                foreach (var d in Devices)
                    d.ResetWrap();
            }
        }

        public void PreRun()
        {
            if (State.Status == InstrumentStatus.PreRun && (State.SequenceItem?.IsSkipped() ?? false))
            {
                State.Status = InstrumentStatus.Ready;
            }

            if (new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready, InstrumentStatus.Pause, InstrumentStatus.PostWork }.Contains(State.Status))
            {
                // In the StatusChanged event handler of Sequence
                // if there are no more items, Invoke Stop() or Halt()
                ChangeStatus(InstrumentStatus.PreRun);

                if (State.Status == InstrumentStatus.PreRun)
                {
                    if (State.SequenceItem?.State.Status == SequenceStatus.Pause)
                    {
                        ChangeStatus(InstrumentStatus.Pause);
                    }
                    else
                    {
                        State.IsSingleShot = false;

                        SendMethod();

                        foreach (var d in Devices)
                            d.PreRun();
                    }
                }
            }
        }

        private void SendMethod()
        {
            foreach (var d in Devices)
            {
                if (d.SetMethod(State.Method))
                {
                    d.SendMethodWrap();
                }
            }
        }

        public void Run()
        {
            if (new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready, InstrumentStatus.PreRun }.Contains(State.Status))
            {
                PrepareAcquisition();

                ChangeStatus(InstrumentStatus.Run);

                foreach (var d in Devices)
                    d.Run();

                PostRun();
            }
        }

        private void PrepareAcquisition()
        {
            State._lastRunTime = DateTime.Now;
            //            throw new NotImplementedException();
        }

        public void PostRun()
        {
            if (State.Status == InstrumentStatus.Run && !Devices.Any(d => d.Status == DeviceStatus.Run))
            {
                FinishAcquisition();

                ChangeStatus(InstrumentStatus.PostRun);

                foreach (var d in Devices)
                    d.PostRun();

                PostWork();
            }
        }

        private void FinishAcquisition()
        {
            //            throw new NotImplementedException();
        }

        public void PostWork()
        {
            if (State.Status == InstrumentStatus.PostRun && !Devices.Any(d => d.Status == DeviceStatus.PostRun))
            {
                ChangeStatus(InstrumentStatus.PostWork);

                foreach (var d in Devices)
                    d.PostWork();

                NextInjectOrFinish();
            }
        }

        private void NextInjectOrFinish()
        {
            if (State.Status == InstrumentStatus.PostWork && !Devices.Any(d => d.Status == DeviceStatus.PostWork))
            {
                if (State.HaltReserved)
                {
                    Halt();
                }
                else if (State.IsSingleShot || State.StopReserved)
                {
                    Stop();
                }
                else
                {
                    PreRun();
                }
            }
        }

        public void Stop()
        {
            if (new[] { InstrumentStatus.PreRun, InstrumentStatus.Pause, InstrumentStatus.Run, InstrumentStatus.PostRun, InstrumentStatus.PostWork }.Contains(State.Status))
            {
                if (State.Status == InstrumentStatus.Run)
                    FinishAcquisition();

                foreach (var d in Devices)
                    d.StopWrap();

                ChangeStatus(InstrumentStatus.NotReady);
            }
        }

        public void StopAfterThisRun(bool stop)
        {
            State.StopReserved = stop;
        }

        public void HaltAfterThisRun(bool halt)
        {
            State.HaltReserved = halt;
        }

        public void HaltAfterSequenceRun(bool halt)
        {
            State.HaltAfterSequenceRun = halt;
        }

        public void SetMethod(IMethod? method)
        {
            if (new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready, InstrumentStatus.PreRun }.Contains(State.Status))
            {
                State.Method = method?.Clone() as IMethod;
            }
        }

        public void SetProject(IProject? project)
        {
            if (new[] { InstrumentStatus.NotReady, InstrumentStatus.Ready, InstrumentStatus.PreRun }.Contains(State.Status))
            {
                State.Project = project;
            }
        }

        public void SetSequenceItem(ISequenceItem? sequenceItem)
        {
            State.SequenceItem = sequenceItem;

            SetProject(sequenceItem?.Project);
            SetMethod(sequenceItem?.Method);
        }

        public TimeSpan TotalRunTime
            => new TimeSpan(
                Math.Max(Devices.Select(d => d.RunTime.Ticks).DefaultIfEmpty().Max(),
                         Signals.Select(s => s.Use ? s.Time.Ticks : 0).DefaultIfEmpty().Max())
                );

        public TimeSpan GetRunTime(IDevice device)
            => new TimeSpan(
                Math.Max(device.RunTime.Ticks,
                         Signals.Where(s => s.Use && s.Device == device)
                                .Select(s => s.Time.Ticks).DefaultIfEmpty().Max())
                );
    }
}