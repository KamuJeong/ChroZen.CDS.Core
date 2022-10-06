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
        public SignalSet(ModelBase? parent, string? name) : base(parent, name)
        {
        }
        public bool Use { get; set; } = true;
        public int Index { get; set; }
        public IDevice? Device { get; set; }
        public int Channel { get; set; }
        public string? Unit { get; set; } 
        public TimeSpan Time { get; set; }
        public double Hz { get; set; }
    }
}
