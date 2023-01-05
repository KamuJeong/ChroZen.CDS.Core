using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public struct SignalPoint
    {
        public DateTime Time;
        public double Value;
        public bool Acquire;
    }

    public class SignalUpdatedArgs : EventArgs
    {
        public SignalSet Source { get; init; }
        public IEnumerable<SignalPoint> Points { get; init; }

        public SignalUpdatedArgs(SignalSet source, IEnumerable<SignalPoint> signalPoints)
        {
            Source = source;
            Points = signalPoints;
        }
    }


    public class SignalSet : ModelBase, ISignalSet
    {
        public SignalSet(ModelBase? parent, string? channel, string? name) : base(parent, name)
        {
            Channel = channel;
            if (Device != null && Device.CreateReferInstance("Channels") is DeviceChannel[] channels)
            {
                var ch = channels.FirstOrDefault(c => string.Equals(channel, c.Name));
                Unit = ch?.Unit;
                Type = ch?.Type ?? DeviceChannelType.None;
                Hz = ch?.AvailableHz?[0] ?? 1.0;
            }
        }
        public bool Use { get; set; } = true;
        public int Index { get; set; }
        public IDevice? Device => Parent as IDevice;
        public string? Channel { get; }
        public DeviceChannelType Type { get; }
        public string? Unit { get; init; }
        public TimeSpan Time { get; set; }
        public double Hz { get; set; }

        private List<(double value, bool acquire)> Data = new();
        private LinkedList<SignalPoint> WindowData = new();

        public void WriteData(double value, bool acquire = false)
        {
            Data.Add((value, acquire));
            PopulateWindow();
        }

        public void WriteData(double[] value, bool acquire = false)
        {
            foreach (var v in value)
                Data.Add((v, acquire));

            PopulateWindow();
        }

        private void PopulateWindow()
        {
            if (Parent is Device dev && dev.Parent is Instrument inst)
            {
                if (inst.State.Status == InstrumentStatus.Run && Use)
                {
                    int count = WindowData.Where(d => (d.Time >= inst.State.LastRunTime) && d.Acquire).Count();
                    foreach (var v in Data.Where(d => d.acquire))
                    {
                        WindowData.AddLast(new SignalPoint
                        {
                            Time = inst.State.LastRunTime + TimeSpan.FromSeconds(count++ * Hz),
                            Value = v.value,
                            Acquire = v.acquire
                        });
                    }
                }
                else
                {
                    var elapse = DateTime.Now - (WindowData.Last?.Value.Time ?? DateTime.Now);
                    foreach (var (i, v) in Data.Select((d, i) => (Data.Count - i - 1, d)))
                    {
                        WindowData.AddLast(new SignalPoint
                        {
                            Time = DateTime.Now - (elapse * i) / Data.Count,
                            Value = v.value,
                            Acquire = v.acquire
                        });
                    }
                }
                Data.Clear();

                var beginWindow = (WindowData.Last?.Value.Time ?? DateTime.Now) - inst.SignalWindow;
                while (WindowData.Count > 0 && WindowData.First!.Value.Time < beginWindow)
                    WindowData.RemoveFirst();
            }
            else
            {
                Data.Clear();
                WindowData.Clear();
            }

            SignalUpdated?.Invoke(this, new SignalUpdatedArgs(this, WindowData));
        }

        public IEnumerable<SignalPoint> Points => WindowData;

        public event EventHandler<SignalUpdatedArgs>? SignalUpdated;
    }
}
