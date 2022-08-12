namespace CDS.Core
{
    public interface IInstrument
    {
        IInstrumentState State { get; }

        IEnumerable<IDevice> Devices { get; }
        IEnumerable<ISignalSet> Signals { get; }

        void Ready();
        void PreRun();
        void Run();
        void Stop();
        void Halt();
    }
}