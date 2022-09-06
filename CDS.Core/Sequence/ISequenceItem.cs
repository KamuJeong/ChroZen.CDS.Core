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

        bool IsSkipped();

        String? SampleName { get; set; }
        string? SampleID { get; set; }

        IInjection? Injection { get; }
        IMethod? Method { get; set; }
        IProject? Project { get; set;  }
    }
}


