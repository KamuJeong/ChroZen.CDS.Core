using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core.Model
{
    public enum InstrumentStatus
    {
        None,
        NotReady,
        Ready,
        PreRun,
        Run,
        PostRun,
        PostWork,
        Error,
    }

    public interface IInstrumentState
    {
        InstrumentStatus Status { get; set; }
        IMethod Method { get; set; }
        IProject? Project { get; set; }
        IChromatogram? Chromatogram { get; set; }
    }
}
