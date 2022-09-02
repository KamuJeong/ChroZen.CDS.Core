﻿using CDS.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument
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

                TimerTick?.Invoke(this, DateTime.Now - _launchTime);
            }
        }

        public Uri? Uri { get; set; }
        public virtual Task<bool> ConnectAsync()
        {
            _tokenSource?.Cancel();
            _tokenSource = new CancellationTokenSource();
            Timer(_tokenSource.Token);

            return Task.FromResult(true);
        }

        public virtual void Disconnect()
        {
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
                Disconnect();

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
