using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface ISignalSet
    {
        bool Use { get; set; }
        int Index { get; }
        string? Name { get; }
        IDevice? Device { get; }
        string? Channel { get; }
        string? Unit { get; }
        TimeSpan Time { get; set; }
        double Hz { get; set; }
    }
}
