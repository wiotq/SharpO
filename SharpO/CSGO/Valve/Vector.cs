using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpO.CSGO.Valve
{
    public struct Vector
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector(float x = 0, float y = 0, float z = 0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z);
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z);
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public void Normalize()
        {
            while(Y > 180)
            {
                Y -= 360;
            }
            while(Y < -180)
            {
                Y += 360;
            }

            while(X > 89)
            {
                X -= 180;
            }

            while(X < -89)
            {
                X += 180;
            }
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z:{Z}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}