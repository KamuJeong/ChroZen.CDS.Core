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

        public int Index { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDevice Device { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public int Channel { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public string Unit { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }
        public TimeValue Time { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Hz { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
