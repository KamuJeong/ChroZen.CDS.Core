using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface ISignalSet
    {
        int Index { get; set; }
        string? Name { get; set; }
        IDevice? Device { get; }
        int Channel { get; }
        string Unit { get; }
        TimeValue? Time { get; set; }
        double Hz { get; set; }
    }
}
