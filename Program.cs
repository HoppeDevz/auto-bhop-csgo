using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using mem;

namespace Hackkk
{
    internal class Program
    {

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vkey);

        static void Main(string[] args)
        {

            Memory memory = new Memory();

            IntPtr moduleBaseAddress = IntPtr.Zero;    

            Process proc = Process.GetProcessesByName("csgo")[0];

            foreach ( ProcessModule module in proc.Modules )
            {

                if (module.ModuleName == "client.dll") moduleBaseAddress = module.BaseAddress;
            }

            if (moduleBaseAddress != IntPtr.Zero)
            {

                const int localPlayerOffset = 0xDB35DC;
                const int forceJumpOffset = 0x5278DDC;
                const int groundFlagOffset = 0x104;

                IntPtr localPlayerAddress = IntPtr.Add(moduleBaseAddress, localPlayerOffset);
                IntPtr forceJumpAddress = IntPtr.Add(moduleBaseAddress, forceJumpOffset);

                IntPtr localPlayerBuffer = memory.readpointer(proc.Handle, localPlayerAddress);

                IntPtr groundAdress = IntPtr.Add(localPlayerBuffer, groundFlagOffset);

                Thread bhopThread = new Thread(bhop);
                bhopThread.Start();

                Console.ReadKey();

                void bhop()
                {

                    while (true)
                    {

                        if (GetAsyncKeyState(Keys.Space) < 0)
                        {

                            IntPtr groundFlagBuffer = memory.readpointer(proc.Handle, groundAdress);

                            int flag = (int)groundFlagBuffer;

                            if (flag == 257 || flag == 263 || flag == 261)
                            {

                                memory.writebytes(proc.Handle, forceJumpAddress, BitConverter.GetBytes(5));
                            } else
                            {
                                memory.writebytes(proc.Handle, forceJumpAddress, BitConverter.GetBytes(4));
                            }
                        }

                        Thread.Sleep(3);
                    }
                }
            }
        }
    }
}
