namespace CDS.Core
{
    public interface IInstrument
    {
        IInstrumentState State { get; }

        event EventHandler<InstrumentStatus>? StatusChanged;

        IEnumerable<IDevice> Devices { get; }
        IEnumerable<ISignalSet> Signals { get; }

        void Ready();
        void PreRun();
        void Run();
        void Stop();
        void Halt();

        void StopAfterThisRun(bool stop);
        void HaltAfterThisRun(bool halt);
        void HaltAfterSequenceRun(bool halt); 

        void SetMethod(IMethod? method);
        void SetProject(IProject? project);
        void SetSequenceItem(ISequenceItem? sequenceItem);
    }
}