using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpO.CSGO
{
    public abstract class InterfaceBase
    {
        public IntPtr BaseAdr;

        public InterfaceBase(IntPtr baseAdr)
        {
            this.BaseAdr = baseAdr;
        }

        internal T GetInterfaceFunction<T>(int index)
        {
            return Memory.GetFunction<T>(Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + index * 4));
        }

        internal IntPtr GetInterfaceFunctionAddress<T>(int index)
        {
            return Memory.ReadPointer(Memory.ReadPointer(BaseAdr) + index * 4);
        }
    }
}