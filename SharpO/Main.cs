using System;
using System.Runtime.InteropServices;
using System.Threading;

using SharpO.CSGO;
using SharpO.CSGO.Valve;

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
            SDK.Engine.ClientCmd_Unrestricted("echo ...", 0);
            SDK.Engine.ClientCmd_Unrestricted("echo [C#]", 0);
            SDK.Engine.ClientCmd_Unrestricted("echo ...", 0);

            while(true)
            {
                Console.ReadKey();


            }
        }
    }
}