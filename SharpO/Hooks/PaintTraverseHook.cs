using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Binarysharp.Assemblers.Fasm;

namespace SharpO.Hooks
{
    public unsafe class PaintTraverseHook: Hook
    {
        public PaintTraverseHook(IntPtr hookAddress, Delegate callback) : base(hookAddress, callback)
        {
            HookAddress += 24; // Offset to the end of function
        }

        public void Hook()
        {
            IntPtr taskPtr = Marshal.AllocCoTaskMem(1024);

            //
            var hookedFuncBytes = FasmNet.Assemble(new[] {
                "use32",
                "pushad",
                $"call {this.CallbackAddress - (int)taskPtr}",
                "popad",
                "call dword [edx+12]",
                "pop ebp",
                "ret 12"
            });

            WinAPI.VirtualProtect(taskPtr, hookedFuncBytes.Length, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out int x);
            Memory.WriteBytes(taskPtr, hookedFuncBytes);
            SetJump(taskPtr);
        }
    }
}