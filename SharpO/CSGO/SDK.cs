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
        public static Client Client;
        public static Engine Engine;

        private static CreateInterface ClientInterface;
        private static CreateInterface EngineInterface;
        private static CreateInterface VGUI2Interface;
        private static CreateInterface VGUISurfaceInterface;
        private static CreateInterface MaterialInterface;
        private static CreateInterface PhysicsInterface;
        private static CreateInterface StdInterface;

        /// <summary>
        /// Find all interface pointers
        /// </summary>
        public static void Init()
        {
            ClientInterface = GetCreateInterfaceFunction("client.dll");
            EngineInterface = GetCreateInterfaceFunction("engine.dll");
            VGUI2Interface = GetCreateInterfaceFunction("vgui2.dll");
            VGUISurfaceInterface = GetCreateInterfaceFunction("vguimatsurface.dll");
            MaterialInterface = GetCreateInterfaceFunction("materialsystem.dll");
            PhysicsInterface = GetCreateInterfaceFunction("vphysics.dll");
            StdInterface = GetCreateInterfaceFunction("vstdlib.dll");

            Client = new Client(GetInterfacePtr("VClient", ClientInterface));
            Engine = new Engine(GetInterfacePtr("VEngineClient", EngineInterface));
        }

        /// <summary>
        /// Get CreateInterface() function from specified module (dll)
        /// </summary>
        /// <param name="moduleName">Module (dll) name</param>
        /// <returns>Function</returns>
        private static CreateInterface GetCreateInterfaceFunction(string moduleName)
        {
            return Memory.GetFunction<CreateInterface>(GetProcAddress(GetModuleHandle(moduleName), "CreateInterface"));
        }

        /// <summary>
        /// Find interface pointer
        /// </summary>
        /// <param name="interfaceName"></param>
        /// <param name="cInterface"></param>
        /// <returns></returns>
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