using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChromassProtocol
{
    public class PacketUpdatedEventArgs<T> : EventArgs where T : struct
    {
        public PacketUpdatedEventArgs(PacketWrapper<T> wrap, int index=0)
        {
            Wrapper = wrap;
            Index = index;
        }

        public PacketWrapper<T> Wrapper { get; }
        public int Index { get; }
    }

    public abstract class PacketWrapper<TPacket>  where TPacket : struct 
    {
        public abstract uint Code { get; }

        public TPacket Packet;

        private TaskCompletionSource? taskCompletionSource;

        public event EventHandler<PacketUpdatedEventArgs<TPacket>>? Updated;

        public byte[] Binary => this.ToBytes<TPacket>();

        public PacketWrapper()
        {
            int size = Marshal.SizeOf(typeof(TPacket));
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(new byte[size], 0, ptr, size);
#pragma warning disable CS8605 // Unboxing a possibly null value.
            Packet = (TPacket)Marshal.PtrToStructure(ptr, typeof(TPacket));
#pragma warning restore CS8605 // Unboxing a possibly null value.
            Marshal.FreeHGlobal(ptr);
        }

        internal void Update(object src, byte[] buf, int index=0)
        {
            Packet = buf.ConvertTo<TPacket>();
            Updated?.Invoke(src, new PacketUpdatedEventArgs<TPacket>(this, index));

            taskCompletionSource?.SetResult();
        }

        public async Task<bool> WaitAnUpdateFor(int mSecond)
        {
            if (mSecond > 0)
            {
                taskCompletionSource = new TaskCompletionSource();
                await await Task.WhenAny(Task.Delay(mSecond), taskCompletionSource.Task);
                return taskCompletionSource.Task.IsCompleted;
            }
            else
            {
                return true;
            }
        }
    }
}
