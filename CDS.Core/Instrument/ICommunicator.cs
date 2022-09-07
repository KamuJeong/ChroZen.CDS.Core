using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public interface ICommunicator
    {
        bool IsConnected { get; }

        void Close();
        Task? ConnectAsync(Uri uri, CancellationToken token);

        event EventHandler<PacketParsingEventArgs>? PacketParsing;

        int Send(byte[] data);
    }
}
