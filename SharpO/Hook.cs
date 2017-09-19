using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpO
{
    public class Hook
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        public enum Protection
        {
            PAGE_NOACCESS = 0x01,
            PAGE_READONLY = 0x02,
            PAGE_READWRITE = 0x04,
            PAGE_WRITECOPY = 0x08,
            PAGE_EXECUTE = 0x10,
            PAGE_EXECUTE_READ = 0x20,
            PAGE_EXECUTE_READWRITE = 0x40,
            PAGE_EXECUTE_WRITECOPY = 0x80,
            PAGE_GUARD = 0x100,
            PAGE_NOCACHE = 0x200,
            PAGE_WRITECOMBINE = 0x400
        }

        private IntPtr HookAddress;
        private Delegate Callback;
        private byte[] OldBytes;

        /// <summary>
        /// Initialize hook parameters
        /// </summary>
        /// <param name="hookAddress">Address in memory to be hooked</param>
        /// <param name="callback">Hook callback function</param>
        public Hook(IntPtr hookAddress, Delegate callback)
        {
            HookAddress = hookAddress;
            Callback = callback;
        }

        /// <summary>
        /// Change call address to hooked address
        /// </summary>
        public void SetCall()
        {
            var callbackAddress = Marshal.GetFunctionPointerForDelegate(Callback);

            byte firstByte = Marshal.ReadByte(HookAddress);

            var asm_bytes = new List<byte>();
            asm_bytes.Add(0xE8);
            asm_bytes.AddRange(BitConverter.GetBytes(callbackAddress.ToInt32() - HookAddress.ToInt32() - 5));

            if(firstByte == 0xFF)
            {
                asm_bytes.Add(0x90);
            }

            int oldProtect = 0;
            VirtualProtect(HookAddress, asm_bytes.Count, (int)Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(asm_bytes.ToArray(), 0, HookAddress, asm_bytes.Count);
            VirtualProtect(HookAddress, asm_bytes.Count, oldProtect, out oldProtect);
        }


        /// <summary>
        /// Replace function pointer to our hook pointer
        /// </summary>
        public void SetPointer()
        {
            var callbackAddress = Marshal.GetFunctionPointerForDelegate(Callback);

            int oldProtect = 0;
            VirtualProtect(HookAddress, IntPtr.Size, (int)Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.WriteIntPtr(HookAddress, callbackAddress);
            VirtualProtect(HookAddress, IntPtr.Size, oldProtect, out oldProtect);
        }

        /// <summary>
        /// Hooked function will jump to our function
        /// </summary>
        public void SetJump()
        {
            var callbackAddress = Marshal.GetFunctionPointerForDelegate(Callback);

            var asm_bytes = new List<byte>();
            asm_bytes.Add(0xE9);
            asm_bytes.AddRange(BitConverter.GetBytes(callbackAddress.ToInt32() - HookAddress.ToInt32() - 5));

            OldBytes = new byte[asm_bytes.Count];
            Marshal.Copy(HookAddress, OldBytes, 0, asm_bytes.Count);

            int oldProtect = 0;
            VirtualProtect(HookAddress, asm_bytes.Count, (int)Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(asm_bytes.ToArray(), 0, HookAddress, asm_bytes.Count);
            VirtualProtect(HookAddress, asm_bytes.Count, oldProtect, out oldProtect);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnsetJump()
        {
            int oldProtect = 0;
            VirtualProtect(HookAddress, OldBytes.Length, (int)Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(OldBytes, 0, HookAddress, OldBytes.Length);
            VirtualProtect(HookAddress, OldBytes.Length, oldProtect, out oldProtect);
        }
    }
}