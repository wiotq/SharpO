using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpO.CSGO
{
    public class Client
    {
        IntPtr BaseAdr = IntPtr.Zero;

        public Client(IntPtr baseAdr)
        {
            this.BaseAdr = baseAdr;
        }
    }
}
