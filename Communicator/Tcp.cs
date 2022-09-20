using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communicator
{
    public class Tcp : ICommunicator
    {
        private Task? watchmanTask;

        private TcpClient? tcpClient;

        private NetworkStream? NetworkStream;

        private int receivedPos = 0;
        private Byte[] receivedBuffer = new byte[1024];

        public async Task ConnectAsync(Uri uri, CancellationToken token)
        {
            if (!IsConnected)
            {
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(uri.Host, uri.Port, token);
                NetworkStream = tcpClient.GetStream();

                watchmanTask = Task.Factory.StartNew(c => WatchMan(c), SynchronizationContext.Current, TaskCreationOptions.LongRunning);
            }
        }

        private void WatchMan(object context)
        {
            SynchronizationContext? synchronizationContext = context as SynchronizationContext;

            int pos = 0;
            byte[] buffer = new byte[4096];

            try
            {
                while (NetworkStream != null)
                {
                    int rlen = NetworkStream.Read(buffer, pos, Math.Min(4096, buffer.Length - pos));
                    if (rlen <= 0)
                    {
                        if (synchronizationContext != null)
                            synchronizationContext.Post(new SendOrPostCallback(o => OnReceived(o as byte[])), null);
                        else
                            OnReceived(null);
                        break;
                    }

                    pos += rlen;
                    if (synchronizationContext != null)
                        synchronizationContext.Post(new SendOrPostCallback(o => OnReceived(o as byte[])), buffer.Take(pos).ToArray());
                    else
                        OnReceived(buffer.Take(pos).ToArray());
                    pos = 0;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void OnReceived(byte[]? buffer)
        {
            if (buffer != null)
            {
                while (buffer.Length + receivedPos > receivedBuffer.Length)
                {
                    var buf = new byte[receivedBuffer.Length * 2];
                    Array.Copy(receivedBuffer, buf, receivedPos);
                    receivedBuffer = buf;
                }

                Array.Copy(buffer, 0, receivedBuffer, receivedPos, buffer.Length);
                receivedPos += buffer.Length;

                try
                {
                    while (receivedPos > 0)
                    {
                        var ParsingArgs = new PacketParsingEventArgs(receivedBuffer, receivedPos);

                        PacketParsing?.Invoke(this, ParsingArgs);

                        if (ParsingArgs.TotalParsed == 0)
                            break;

                        if (ParsingArgs.TotalParsed < 0 || ParsingArgs.TotalParsed > receivedPos)
                        {
                            receivedPos = 0;
                            break;
                        }

                        receivedPos -= ParsingArgs.TotalParsed;
                        if (receivedPos > 0)
                            Array.Copy(receivedBuffer, ParsingArgs.TotalParsed, receivedBuffer, 0, receivedPos);
                    }
                }
                catch
                {
                    throw new InvalidOperationException("pasing error");
                }

                if (receivedPos == 0 && receivedBuffer.Length > 1024)
                    receivedBuffer = new byte[1024];
            }
            else
            {
                Close();
            }
        }

        public event EventHandler<PacketParsingEventArgs>? PacketParsing;

        public bool IsConnected => tcpClient != null && tcpClient.Connected;

        public void Close()
        {
            try
            {
                NetworkStream?.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            try
            {
                tcpClient?.Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            finally
            {
                tcpClient = null;
                NetworkStream = null;
            }

            watchmanTask?.Wait();
        }


        public int Send(byte[] data)
        {
            if (IsConnected)
            {
                NetworkStream?.Write(data, 0, data.Length);
                return data.Length;
            }
            return 0;
        }
    }
}
