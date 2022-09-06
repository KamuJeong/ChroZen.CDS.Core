using CDS.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public abstract class Device : ModelBase, IDevice, IDisposable
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
                CheckReadyStatus();
            }
        }

        protected virtual void OnTimerTick(TimeSpan ts)     { }
        protected abstract void CheckReadyStatus();

        public Uri? Uri { get; set; }
        public Task<bool> ConnectAsyncWrap()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            Timer(_tokenSource.Token);

            return ConnectAsync();
        }

        public abstract Task<bool> ConnectAsync();

        public void DisconnectWrap()
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
                ChangeStatus(DeviceStatus.None);
            }
        }

        public abstract void Disconnect();

        public abstract bool SetMethod(IMethod? method);
        public abstract void GetMethod(IMethod? method);

        internal Task<bool> SendMethodAsyncWrap()
        {
            ChangeStatus(DeviceStatus.NotReady);
            return SendMethodAsync();
        }
        protected abstract Task<bool> SendMethodAsync();
        
        internal Task<bool> LoadMethodAsyncWrap()
        {
            return LoadMethodAsync();
        }
        internal protected abstract Task<bool> LoadMethodAsync();

        internal protected abstract bool Ready();
        internal protected abstract bool PreRun();
        internal protected abstract bool Run();
        internal protected abstract bool PostRun();
        internal protected abstract bool PostWork();

        internal void StopWrap()
        {
            Stop();
            ChangeStatus(DeviceStatus.NotReady);
        }
        protected abstract void Stop();

        internal void HaltWrap()
        {
            Halt();
            ChangeStatus(DeviceStatus.NotReady);
        }
        protected abstract void Halt();

        internal void ResetWrap()
        {
            Reset();
            ChangeStatus(DeviceStatus.NotReady);
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
