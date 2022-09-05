using CDS.Core;

namespace CDS.Instrument
{
    public class InstrumentState : IInstrumentState
    {
        public InstrumentStatus Status { get; internal set; } 
 
        public ISequenceItem? SequenceItem { get; set; }
        public IMethod? Method { get; set; }
        public IProject? Project { get; set; }
        public IChromatogram? Chromatogram { get; set; }

        internal DateTime _lastRunTime = DateTime.Today;
        public TimeSpan ElapsedTime => DateTime.Now - _lastRunTime;
        public bool IsSingleShot { get; internal set; }
        public bool StopReserved { get; set; }
        public bool HaltReserved { get; set; }
        public bool HaltAfterSequenceRun { get; set; }
    }
}