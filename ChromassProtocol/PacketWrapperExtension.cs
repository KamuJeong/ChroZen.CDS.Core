using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ChromassProtocol
{
    public static class PacketWrapperExtension
    {
        public static byte[] ToBytes(this Header head)
        {
            byte[] arr = new byte[24];
            IntPtr ptrHead = Marshal.AllocHGlobal(24);

            Marshal.StructureToPtr(head, ptrHead, false);
            Marshal.Copy(ptrHead, arr, 0, 24);
            Marshal.FreeHGlobal(ptrHead);

            return arr;
        }

        public static byte[] ToBytes<T>(this PacketWrapper<T> wrapper, ref Header head) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size + 24];
            IntPtr ptrHead = Marshal.AllocHGlobal(24);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(head, ptrHead, false);
            Marshal.StructureToPtr(wrapper.Packet, ptr, false);

            Marshal.Copy(ptrHead, arr, 0, 24);
            Marshal.Copy(ptr, arr, 24, size);

            Marshal.FreeHGlobal(ptrHead);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public static byte[] ToBytes<T>(this PacketWrapper<T> wrapper) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(wrapper.Packet, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }


        public static T ConvertTo<T>(this byte[] arr, int offset = 0) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));

            if (size > arr.Length - offset)
            {
                throw new ArgumentException(nameof(arr));
            }

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, offset, ptr, size);
            T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);

            return obj;
        }

        public static void Assemble<T>(this PacketWrapper<T> wrapper, object caller, ReadOnlySpan<byte> slot, int index, int offset) where T : struct
        {
            byte[] assemble = wrapper.Binary;

            if (assemble.Length < offset + slot.Length)
                throw new ArgumentOutOfRangeException();

            var dest = new Span<byte>(assemble, offset, slot.Length);
            slot.Slice(offset, slot.Length).CopyTo(dest);

            wrapper.Update(caller, assemble, index);
        }

        public static byte[] RequestPacket<T>(this PacketWrapper<T> wrapper, int index = 0, uint id = 0) where T : struct
        {
            return new Header
            {
                Length = 24,
                Id = id,
                Code = wrapper.Code,
                Index = index,
                SlotOffset = 0,
                SlotSize = wrapper.Binary.Length
            }.ToBytes();
        }

        public static byte[] SendPacket<T>(this PacketWrapper<T> wrapper, int index = 0, uint id = 0, int offset = 0, int size = -1) where T : struct
        {
            var arr = wrapper.Binary;
            size = size < 0 ? arr.Length : size;
            Header header = new Header
            {
                Length = 24 + size,
                Id = id,
                Code = wrapper.Code,
                Index = index,
                SlotOffset = offset,
                SlotSize = size,
            };
            return wrapper.ToBytes(ref header);
        }

        public static byte[] SendOkPacket<T>(this PacketWrapper<T> wrapper, int index = 0, int id = 0) where T : struct
        {
            return SendPacket<T>(wrapper, index, size: 0);
        }

    }
}
