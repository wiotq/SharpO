using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Injector
{
    public class Injector
    {
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);

        public static void Main()
        {
            Process proc = Process.GetProcessesByName("csgo").FirstOrDefault();
            while(proc == null)
            {
                proc = Process.GetProcessesByName("csgo").FirstOrDefault();
                Thread.Sleep(1000);
            }

            var sharpo = @"T:\C#\!Projects\SharpO\SharpO\bin\Debug\SharpO.dll";

            var injectedLibPtr = proc.LoadLibrary(sharpo);
            var lib = LoadLibrary(sharpo);
            var ptr = GetProcAddress(lib, "Init");

            var diff = IntPtr.Subtract(ptr, (int)lib);

            proc.CallAsync(IntPtr.Add(injectedLibPtr, (int)diff), IntPtr.Zero);
        }
    }
}