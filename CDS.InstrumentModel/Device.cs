using CDS.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public abstract class Device : ModelBase, IDevice, IDisposable
    {
        public Device(ModelBase? parent, string? name) : base(parent, name)
        {

        }

        public int TickInterval { get; set; } = 100; 
        public event EventHandler<TimeSpan>? TimerTick;

        private DateTime _launchTime;
        private CancellationTokenSource? _tokenSource;
        private bool disposedValue;

        private async void Timer(CancellationToken token)
        {
            _launchTime = DateTime.Now;

            while(!token.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TickInterval, token);
                }
                catch (TaskCanceledException) 
                { 
                    break; 
                }

                OnTimerTick(DateTime.Now - _launchTime);
                TimerTick?.Invoke(this, DateTime.Now - _launchTime);
                if (new[] {DeviceStatus.NotReady, DeviceStatus.Ready}.Contains(Status))
                {
                    Status = CheckReadyStatus() ? DeviceStatus.Ready : DeviceStatus.NotReady;
                }
            }
        }

        protected virtual void OnTimerTick(TimeSpan ts)     { }
        protected abstract bool CheckReadyStatus();

        public Uri? Uri { get; set; }

        public string? SerialNumber { get; set; }

        public string? Model { get; set; }

        private DeviceStatus _status = DeviceStatus.None;
        public DeviceStatus Status 
        {
            get => _status; 
            protected set
            {
                if (_status != value)
                {
                    _status = value;
                    (Parent as Instrument)?.InvokeStatusChangedEvent(this);
                }
            }
        }

        internal Task<bool> ConnectAsyncWrap()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            Timer(_tokenSource.Token);

            return ConnectAsync(_tokenSource.Token);
        }

        public abstract Task<bool> ConnectAsync(CancellationToken token);

        internal void DisconnectWrap()
        {
            Disconnect();

            try
            {
                _tokenSource?.Cancel();
            }
            catch
            {

            }
            finally
            {
                Status = DeviceStatus.None;
            }
        }

        public abstract void Disconnect();

        public abstract void GetMethod(IMethod? method);

        internal bool SendMethodWrap(IMethod? method)
        {
            Status = DeviceStatus.NotReady;
            return SendMethod(method);
        }
        protected abstract bool SendMethod(IMethod? method);
        
        internal Task<bool> LoadMethodAsyncWrap(IMethod? method)
        {
            return LoadMethodAsync(method);
        }
        protected abstract Task<bool> LoadMethodAsync(IMethod? method);

        public abstract TimeSpan RunTime { get; }

        internal protected abstract bool Ready();
        internal protected abstract bool PreRun();
        internal protected abstract bool Run();
        internal protected abstract bool PostRun();
        internal protected abstract bool PostWork();

        internal void StopWrap()
        {
            Stop();
            Status = DeviceStatus.NotReady;
        }
        protected abstract void Stop();

        internal void HaltWrap()
        {
            Halt();
            Status = DeviceStatus.NotReady;
        }
        protected abstract void Halt();

        internal void ResetWrap()
        {
            Reset();
            Status = DeviceStatus.NotReady;
        }
        protected abstract void Reset();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                DisconnectWrap();

                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Device()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
