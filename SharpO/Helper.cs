using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace SharpO
{
    public static class Helper
    {
        [DllImport("user32.dll")]
        static extern bool GetAsyncKeyState(int vKey);

        static bool[] KeyStates = new bool[256];

        public static bool KeyPressed(int i)
        {
            if(GetAsyncKeyState(i) != KeyStates[i])
                return KeyStates[i] = !KeyStates[i];
            else
                return false;
        }

        public static bool KeyDown(int i)
        {
            return GetAsyncKeyState(i);
        }

        public static int[] SignatureToPattern(string signature)
        {
            string[] split = signature.Split(' ');
            List<int> shit = new List<int>();

            foreach(string s in split)
            {
                if(s.Contains("?"))
                {
                    shit.Add(-1);
                    continue;
                }
                shit.Add(Convert.ToInt32(s, 16));
            }

            return shit.ToArray();
        }
    }
}