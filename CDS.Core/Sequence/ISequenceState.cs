namespace CDS.Core
{
    public enum SequenceStatus
    {
        Reserved,
        Run,
        Pause,
        Stop,
        Error,
        Finished
    }

    public interface ISequenceState
    {
        SequenceStatus Status { get; }

        int Counter { get; }

        IEnumerable<IChromatogram?> Chromatograms { get; }
    }
}