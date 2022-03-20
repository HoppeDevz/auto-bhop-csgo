using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mem
{

    public class Memory
    {

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(IntPtr proc, IntPtr address, byte[] buffer, int size, ref int read);

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(IntPtr proc, IntPtr address, byte[] buffer, int size, ref int written);

        int read, write;
        byte[] ptr = new byte[8];
        byte[] buffer = new byte[1024];

        public IntPtr readpointer(IntPtr proc, IntPtr address)
        {

            ReadProcessMemory(proc, address, buffer, 4, ref read);

            return (IntPtr)BitConverter.ToInt32(buffer, 0);
        }


        public byte[] readBytes(IntPtr proc, IntPtr address, int size)
        {

            ReadProcessMemory(proc, address, buffer, size, ref read);

            return buffer;
        }


        public void writebytes(IntPtr proc, IntPtr address, byte[] newbytes)
        {

            WriteProcessMemory(proc, address, newbytes, newbytes.Length, ref write);
        }

    }
}