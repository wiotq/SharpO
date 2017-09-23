using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpO
{
    public unsafe abstract class Hook
    {
        public IntPtr HookAddress { get; internal set; }
        public IntPtr CallbackAddress { get; internal set; }

        private Delegate Callback;
        private byte[] OldBytes;

        /// <summary>
        /// Initialize hook parameters
        /// </summary>
        /// <param name="hookAddress">Address in memory to be hooked</param>
        /// <param name="callback">Hook callback function</param>
        public Hook(IntPtr hookAddress, Delegate callback)
        {
            this.HookAddress = hookAddress;
            this.Callback = callback;
            this.CallbackAddress = Marshal.GetFunctionPointerForDelegate(callback);
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
            WinAPI.VirtualProtect(HookAddress, asm_bytes.Count, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(asm_bytes.ToArray(), 0, HookAddress, asm_bytes.Count);
            WinAPI.VirtualProtect(HookAddress, asm_bytes.Count, oldProtect, out oldProtect);
        }


        /// <summary>
        /// Replace function pointer to our hook pointer
        /// </summary>
        public void SetPointer()
        {
            var callbackAddress = Marshal.GetFunctionPointerForDelegate(Callback);

            int oldProtect = 0;
            WinAPI.VirtualProtect(HookAddress, IntPtr.Size, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.WriteIntPtr(HookAddress, callbackAddress);
            WinAPI.VirtualProtect(HookAddress, IntPtr.Size, oldProtect, out oldProtect);
        }

        /// <summary>
        /// Hooked function will jump to our function
        /// </summary>
        public void SetJump(IntPtr callbackAdr)
        {
            IntPtr callbackAddress;
            if(callbackAdr == IntPtr.Zero)
            {
                callbackAddress = Marshal.GetFunctionPointerForDelegate(Callback);
            }
            else
            {
                callbackAddress = callbackAdr;
            }

            var asm_bytes = new List<byte>();
            asm_bytes.Add(0xE9);
            asm_bytes.AddRange(BitConverter.GetBytes(callbackAddress.ToInt32() - HookAddress.ToInt32() - 5));

            OldBytes = new byte[asm_bytes.Count];
            Marshal.Copy(HookAddress, OldBytes, 0, asm_bytes.Count);

            int oldProtect = 0;
            WinAPI.VirtualProtect(HookAddress, asm_bytes.Count, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(asm_bytes.ToArray(), 0, HookAddress, asm_bytes.Count);
            WinAPI.VirtualProtect(HookAddress, asm_bytes.Count, oldProtect, out oldProtect);
        }

        /// <summary>
        /// Remove jump hook
        /// </summary>
        public void UnsetJump()
        {
            int oldProtect = 0;
            WinAPI.VirtualProtect(HookAddress, OldBytes.Length, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out oldProtect);
            Marshal.Copy(OldBytes, 0, HookAddress, OldBytes.Length);
            WinAPI.VirtualProtect(HookAddress, OldBytes.Length, oldProtect, out oldProtect);
        }
    }
}