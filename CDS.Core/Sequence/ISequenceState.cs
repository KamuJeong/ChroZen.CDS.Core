namespace CDS.Core
{
    public enum SequenceStatus
    {
        Reserved,
        Run,
        Pause,
        Stop,
        Error,
        Final
    }

    public interface ISequenceState
    {
        SequenceStatus Status { get; set; }
        int Counter { get; set; }
        IEnumerable<IChromatogram> Chromatograms { get; init; }
    }
}