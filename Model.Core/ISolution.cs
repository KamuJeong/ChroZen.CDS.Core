using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core.Model
{
    public interface ISolution
    {
        ISequence Sequence { get; }
        IInstrument Instrument { get; }
        IEnumerable<ISample> Samples { get; }
        IEnumerable<IMethod> Methods { get; }
        IEnumerable<IProject> Projects { get; }
        IReport Report { get; }
    }
}
