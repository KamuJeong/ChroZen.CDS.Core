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
        IDevice Device { get; init; }
        int Channel { get; init; }
        string Unit { get; init; }
        TimeValue Time { get; set; }
        double Hz { get; set; }
    }
}
