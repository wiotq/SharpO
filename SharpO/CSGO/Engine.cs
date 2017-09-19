using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpO.CSGO.Valve;

namespace SharpO.CSGO
{
    public class Engine: InterfaceBase
    {
        public delegate int GetLocalPlayerDlg();
        public delegate void ClientCmd_UnrestrictedDlg(string text, int flags);
        public delegate void GetViewAnglesDlg(out Vector vector);
        public delegate void SetViewAngleDlg(Vector vector);

        public ClientCmd_UnrestrictedDlg ClientCmd_Unrestricted;
        public GetLocalPlayerDlg GetLocalPlayer;
        public GetViewAnglesDlg GetViewAngles;
        public SetViewAngleDlg SetViewAngles;

        public Engine(IntPtr baseAdr) : base(baseAdr)
        {
            GetLocalPlayer = GetInterfaceFunction<GetLocalPlayerDlg>(12);
            ClientCmd_Unrestricted = GetInterfaceFunction<ClientCmd_UnrestrictedDlg>(114);
            GetViewAngles = GetInterfaceFunction<GetViewAnglesDlg>(18);
            SetViewAngles = GetInterfaceFunction<SetViewAngleDlg>(19);
        }
    }
}