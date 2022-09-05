namespace CDS.Core
{
    public interface ISequence
    {
        IEnumerable<ISequenceItem> Items { get; }
        ISequenceItem? GetNext();
        ISequenceItem? GetCurrent();

        bool Ready();
        bool Run();
        bool Pause();
        bool Resume();
    }
}