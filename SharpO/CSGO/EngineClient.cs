using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpO.CSGO.Valve;

namespace SharpO.CSGO
{
    public class EngineClient
    {
        IntPtr BaseAdr = IntPtr.Zero;

        public delegate int GetLocalPlayerDlg();
        public delegate void ClientCmd_UnrestrictedDlg(string text, int flags);
        public delegate void GetViewAnglesDlg(out Vector vector);
        public delegate void SetViewAngleDlg(Vector vector);

        public ClientCmd_UnrestrictedDlg ClientCmd_Unrestricted;
        public GetLocalPlayerDlg GetLocalPlayer;
        public GetViewAnglesDlg GetViewAngles;
        public SetViewAngleDlg SetViewAngles;

        public EngineClient(IntPtr baseAdr)
        {
            this.BaseAdr = baseAdr;

            GetLocalPlayer = Memory.GetFunction<GetLocalPlayerDlg>(Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + 12 * 4));
            ClientCmd_Unrestricted = Memory.GetFunction<ClientCmd_UnrestrictedDlg>(Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + 114 * 4));
            GetViewAngles = Memory.GetFunction<GetViewAnglesDlg>(Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + 18 * 4));
            SetViewAngles = Memory.GetFunction<SetViewAngleDlg>(Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + 19 * 4));
        }
    }
}