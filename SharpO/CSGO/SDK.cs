using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static SharpO.WinAPI;

namespace SharpO.CSGO
{
    public delegate IntPtr CreateInterface(string name, int ptr);

    public static class SDK
    {
        public static CClient Client;
        public static EngineClient Engine;

        private static CreateInterface ClientInterface;
        private static CreateInterface EngineInterface;
        private static CreateInterface VGUI2Interface;
        private static CreateInterface VGUISurfaceInterface;
        private static CreateInterface MaterialInterface;
        private static CreateInterface PhysicsInterface;
        private static CreateInterface StdInterface;

        public static void Init()
        {
            ClientInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("client.dll"), "CreateInterface"));
            EngineInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("engine.dll"), "CreateInterface"));
            VGUI2Interface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("vgui2.dll"), "CreateInterface"));
            VGUISurfaceInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("vguimatsurface.dll"), "CreateInterface"));
            MaterialInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("materialsystem.dll"), "CreateInterface"));
            PhysicsInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("vphysics.dll"), "CreateInterface"));
            StdInterface = Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle("vstdlib.dll"), "CreateInterface"));

            Client = new CClient(GetInterfacePtr("VClient", ClientInterface));
            Engine = new EngineClient(GetInterfacePtr("VEngineClient", EngineInterface));
        }

        private static IntPtr GetInterfacePtr(string interfaceName, CreateInterface cInterface)
        {
            string tempInterface = "";
            string interfaceVersion = "0";

            for(int i = 0; i <= 99; i++)
            {
                tempInterface = interfaceName + interfaceVersion + i;
                var funcPtr = cInterface(tempInterface, 0);
                if(funcPtr != IntPtr.Zero)
                {
                    return funcPtr;
                }
                if(i >= 99 && interfaceVersion == "0")
                {
                    interfaceVersion = "00";
                    i = 0;
                }
            }
            return IntPtr.Zero;
        }
    }
}