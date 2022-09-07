using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDS.Core
{
    public class PacketParsingEventArgs : EventArgs
    {
        public PacketParsingEventArgs(byte[] buffer, int length)
        {
            _buffer = buffer;
            _length = length;
        }

        private byte[] _buffer;
        private int _length;

        public ReadOnlySpan<byte> Buffer => new ReadOnlySpan<byte>(_buffer, TotalParsed, _length - TotalParsed);
        public ReadOnlySpan<byte> OriginBuffer => new ReadOnlySpan<byte>(_buffer, 0, _length);
        public int TotalParsed { get; private set; }
        public void Parsed(int length) => TotalParsed += length;
    }
}
