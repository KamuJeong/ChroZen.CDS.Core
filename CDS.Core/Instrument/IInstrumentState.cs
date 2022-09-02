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
        bool HaltAfterSequenceRun { get; }

        IMethod? Method { get; }
        IProject? Project { get; }
        IChromatogram? Chromatogram { get; }

        TimeSpan ElapsedTime { get; }
    }
}
