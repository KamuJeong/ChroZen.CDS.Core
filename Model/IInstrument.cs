namespace CDS.Core.Model
{
    public interface IInstrument
    {
        IInstrumentState State { get; }
        IEnumerable<IDevice> Devices { get; }
        IDictionary<int, ISignalSet> Signals { get; }

        bool Ready();
        bool Run();
        bool Stop();
        bool Reset();
    }
}