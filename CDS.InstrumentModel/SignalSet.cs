using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.InstrumentModel
{
    public class SignalSet : ModelBase, ISignalSet
    {
        public SignalSet(ModelBase? parent, int channel, string? name) : base(parent, name)
        {
            Channel = channel;
        }
        public bool Use { get; set; } = true;
        public int Index { get; set; }
        public IDevice? Device => Parent as IDevice;
        public int Channel { get; }
        public string? Unit { get; set; } 
        public TimeSpan Time { get; set; }
        public double Hz { get; set; } = 1.0;

        private List<(DateTime dt, double value, bool acquire)> Data = new();

        public void WriteData(double value, bool acquire = false)
        {
            Data.Add((DateTime.Now, value, acquire));
        }

        internal IReadOnlyList<(DateTime dt, double value, bool acquire)> GetData()
        {
            IReadOnlyList<(DateTime dt, double value, bool acquire)> data = Data;
            Data = new();
            return data;
        }
    }
}
