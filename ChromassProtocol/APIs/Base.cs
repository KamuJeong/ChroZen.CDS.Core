using ChromassProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromassProtocols.APIs
{
    public class Base<TPacket> : UnionActionsAfter100ms where TPacket : struct
    {
        public PacketWrapper<TPacket> Wrapper { get; init; }

        public Base(PacketWrapper<TPacket> wrapper, Action? action) : base(action)
        {
            Wrapper = wrapper;
        }

        public virtual void Assign(Base<TPacket> src)
        {
            if (src != this && src.Wrapper != Wrapper)
            {
                Wrapper.Packet = src.Wrapper.Packet;
                CallAction();
            }
        }
    }
}
