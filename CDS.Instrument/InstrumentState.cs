using CDS.Core;

namespace CDS.Instrument
{
    public class InstrumentState : IInstrumentState
    {
        public InstrumentStatus Status { get; internal set; } 
 
        public IMethod? Method { get; set; }
        public IProject? Project { get; set; }
        public IChromatogram? Chromatogram { get; set; }

        public TimeSpan ElapsedTime => TimeSpan.Zero;
        public bool IsSingleShot { get; internal set; }
        public bool StopReserved { get; set; }
        public bool HaltReserved { get; set; }
        public bool HaltAfterLastSequence { get; set; }
    }
}