using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Instrument
{
    public class SignalSet : ModelBase, ISignalSet
    {
        public SignalSet(ModelBase? parent, string? name) : base(parent, name)
        {
        }

        public int Index { get; set; }
        public IDevice? Device { get; set; }
        public int Channel { get; set; }
        public string? Unit { get; set; }
        public TimeValue? Time { get; set; }
        public double Hz { get; set; }
    }
}
