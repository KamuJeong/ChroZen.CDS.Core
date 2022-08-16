using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
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
        InstrumentStatus Status { get; }
        bool IsSingleShot { get; }
        bool StopReserved { get; }
        bool HaltReserved { get; }
        bool HaltAfterLastSequence { get; }

        IMethod? Method { get; set; }
        IProject? Project { get; set; }
        IChromatogram? Chromatogram { get; set; }

        TimeSpan ElapsedTime { get; }
    }
}
