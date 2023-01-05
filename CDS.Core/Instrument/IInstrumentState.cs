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
        Pause,
        Run,
        PostRun,
        PostWork,
        Error,
    }

    public class InstrumentStatusChangedArgs : EventArgs
    {
        public InstrumentStatus OldStatus { get; }

        public InstrumentStatusChangedArgs(InstrumentStatus oldStatus)
        {
            OldStatus = oldStatus;
        }
    }

    public interface IInstrumentState
    {
        InstrumentStatus Status { get; }
        bool IsSingleShot { get; }
        bool StopReserved { get; }
        bool HaltReserved { get; }
        bool HaltAfterSequenceRun { get; }

        public ISequenceItem? SequenceItem { get; }
        IMethod? Method { get; }
        IProject? Project { get; }
        IChromatogram? Chromatogram { get; }

        DateTime LastRunTime { get; }
        TimeSpan ElapsedTime { get; }
    }
}
