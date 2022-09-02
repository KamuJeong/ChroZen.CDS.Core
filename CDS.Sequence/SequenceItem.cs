using CDS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Sequence
{
    public class SequenceItem : ISequenceItem
    {
        public ISequenceState State { get;  } = new SequenceState();

        public ISample? Sample { get; set; }

        public IInjection? Injection { get; set; }

        public IMethod? Method { get; set; }

        public IProject? Project { get; set; }
    }
}
