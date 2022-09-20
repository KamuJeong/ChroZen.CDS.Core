namespace Communicator
{ 
    public interface ICommunicator
    {
        bool IsConnected { get; }

        void Close();
        Task ConnectAsync(Uri uri, CancellationToken token);

        event EventHandler<PacketParsingEventArgs>? PacketParsing;

        int Send(byte[] data);
    }
}