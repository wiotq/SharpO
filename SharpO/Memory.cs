using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpO
{
    public static class Memory
    {
        #region Reading
        public static int ReadInt(IntPtr address)
        {
            unsafe
            {
                return *(int*)address;
            }
        }

        public static float ReadFloat(IntPtr address)
        {
            unsafe
            {
                return *(float*)address;
            }
        }

        public static IntPtr ReadPointer(IntPtr address)
        {
            return Marshal.ReadIntPtr(address);
        }

        public static IntPtr ReadPointer(int address)
        {
            return Marshal.ReadIntPtr((IntPtr)address);
        }

        public static byte[] ReadBytes(IntPtr address, int length)
        {
            byte[] buff = new byte[length];
            Marshal.Copy(address, buff, 0, length);
            return buff;
        }
        #endregion Reading

        #region Writing
        public static void WriteFloat(IntPtr address, float value)
        {
            unsafe
            {
                *(float*)address = value;
            }
            //var buffer = BitConverter.GetBytes(value);
            //Marshal.Copy(buffer, 0, address, buffer.Length);
        }

        public static void WriteInt(IntPtr address, int value)
        {
            unsafe
            {
                *(int*)address = value;
            }
            //var buffer = BitConverter.GetBytes(value);
            //Marshal.Copy(buffer, 0, address, buffer.Length);
        }

        public static void WriteByte(IntPtr address, byte value)
        {
            unsafe
            {
                *(byte*)address = value;
            }
        }

        public static void WriteBytes(IntPtr address, byte[] bytes)
        {
            Marshal.Copy(bytes, 0, address, bytes.Length);
        }

        public static void WriteNops(IntPtr address, int count)
        {
            unsafe
            {
                for(int i = 0; i < count; i++)
                {
                    *(byte*)address = 0x90;
                }
            }
        }
        #endregion Writing

        public static T GetFunction<T>(IntPtr address)
        {
            return Marshal.GetDelegateForFunctionPointer<T>(address);
        }

        #region Aobscan


        /// <summary>
        /// Very fast aobscan
        /// </summary>
        /// <param name="start">Start address</param>
        /// <param name="length">Length</param>
        /// <param name="signature">Signature in CE format (80 3A ?? D9)</param>
        /// <returns>Address of signature</returns>
        public static IntPtr Aobscan(int start, int length, string signature)
        {
            return Aobscan((IntPtr)start, length, signature);
        }

        /// <summary>
        /// Very fast aobscan
        /// </summary>
        /// <param name="start">Start address</param>
        /// <param name="length">Length</param>
        /// <param name="signature">Signature in CE format (80 3A ?? D9)</param>
        /// <returns>Address of signature</returns>
        public static IntPtr Aobscan(IntPtr start, int length, string signature)
        {
            int[] pattern = Helper.SignatureToPattern(signature);

            unsafe
            {
                for(int i = start.ToInt32(); i < start.ToInt32() + length; i++)
                {
                    byte b = *(byte*)i;
                    if(b == pattern[0])
                    {
                        for(int j = 0; j < pattern.Length; j++)
                        {
                            b = *(byte*)(i + j);
                            if(b == pattern[j] || pattern[j] == -1)
                            {
                                if(j == pattern.Length - 1)
                                {
                                    return (IntPtr)i;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }
        #endregion Aobscan
    }
}