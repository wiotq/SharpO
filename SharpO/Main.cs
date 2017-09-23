using System;
using System.Runtime.InteropServices;
using System.Threading;

using SharpO.CSGO;
using SharpO.CSGO.Valve;
using System.Collections.Generic;

namespace SharpO
{
    // Should NOT be obfuscated
    public static class Main
    {
        /// <summary>
        /// Actually never called, need for injector to see exports
        /// </summary>
        public static void DllMain(IntPtr dllInstance, int reason, IntPtr reserved) { }

        /// <summary>
        /// Fake entry point, must be called after injection
        /// </summary>
        [DllExport("Init")]
        public static void Init()
        {
            DebugHelper.ShowConsoleWindow();

            SDK.Init();

            SDK.Engine.ClientCmd_Unrestricted("clear", 0);
            SDK.Engine.ClientCmd_Unrestricted("echo [C#]", 0);

            EngineHook.Init();

            while(true)
            {
                Console.ReadKey();

            }
        }


    }

    public static class EngineHook
    {
        static Hook HookPT;

        public delegate void PaintTraverseDlg(IntPtr panelAdr, int vguiPanel, int forceRepaint, int allowForce);
        public delegate void Test();
        static Test test;

        public static void Init()
        {
            HookPT = new Hook(SDK.Panel.GetInterfaceFunctionAddress<PaintTraverseDlg>(41), (PaintTraverseDlg)PaintTraverseHooked);
            Console.WriteLine(HookPT.HookAddress.ToString("X"));

            List<byte> asmBytes = new List<byte>();
            asmBytes.AddRange(new byte[] {
                0xB8, 0x01, 0x00, 0x00, 0x00, // mov eax, 1
                0xC3 // ret
            });

            IntPtr taskPtr = Marshal.AllocCoTaskMem(asmBytes.Count);
            WinAPI.VirtualProtect(taskPtr, asmBytes.Count, (int)WinAPI.Protection.PAGE_EXECUTE_READWRITE, out int x);
            Memory.WriteBytes(taskPtr, asmBytes.ToArray());
            test = Memory.GetFunction<Test>(taskPtr);

            Console.WriteLine(taskPtr.ToString("X"));

            Console.ReadLine();

            HookPT.SetCall();
        }

        static unsafe void PaintTraverseHooked(IntPtr panelAdr, int vguiPanel, int forceRepaint, int allowForce)
        {
            test();

            //HookPT.UnsetJump();
            //SDK.Panel.PaintTraverse(panelAdr, vguiPanel, forceRepaint, allowForce);
            //HookPT.SetJump();
        }
    }
}