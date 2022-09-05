using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface ISequenceItem
    {
        ISequenceState State { get; }
        void SetRunStatus();
        void SetErrorStatus();
        void Reset();

        ISample? Sample { get; }
        IInjection? Injection { get; }
        IMethod? Method { get; set; }
        IProject? Project { get; set;  }
    }
}


