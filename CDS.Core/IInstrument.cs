namespace CDS.Core
{
    public interface IInstrument
    {
        IInstrumentState State { get; init; }
        IEnumerable<IDevice> Devices { get; }
        IEnumerable<ISignalSet> Signals { get; }

        Task<bool> ConnectAsync();
        void Disconnect();

        Task<bool> LoadMethodAsync();
        Task<bool> SendMethodAsync(IMethod method);

        bool Ready();
        bool Run();
        bool Stop();
        bool Halt();
        bool Reset();
    }
}