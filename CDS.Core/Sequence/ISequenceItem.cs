using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface ISequenceItem
    {
        ISequenceState State { get; init; }
        ISample Sample { get; init; }
        IInjection Injection { get; init; }
        IMethod? Method { get; set; }
        IProject? Project { get; set;  }
    }
}


