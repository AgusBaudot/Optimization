using System;
    
namespace Sim.Foundation
{
    public struct SimVec2 : IEquatable<SimVec2>
    {
        public readonly float x;
        public readonly float y;
        public readonly float magnitude => MathF.Sqrt(x * x + y * y);
        public readonly SimVec2 normalized => Normalize();

        public static SimVec2 one => new(1, 1);
        public static SimVec2 zero => new(0, 0);
        public static SimVec2 right => new(1, 0);
        public static SimVec2 left => new(-1, 0);
        public static SimVec2 up => new(0, 1);
        public static SimVec2 down => new(0, -1);

        public SimVec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public SimVec2(float value)
        {
            x = value;
            y = value;
        }

        public SimVec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public SimVec2(int value)
        {
            x = value;
            y = value;
        }

        public static SimVec2 operator +(SimVec2 v1, SimVec2 v2)
        {
            return new SimVec2(v1.x + v2.x, v1.y + v2.y);
        }

        public static SimVec2 operator -(SimVec2 v1, SimVec2 v2)
        {
            return new SimVec2(v1.x - v2.x, v1.y - v2.y);
        }

        public static SimVec2 operator *(SimVec2 v1, float scalar)
        {
            return new SimVec2(v1.x * scalar, v1.y * scalar);
        }

        public static SimVec2 operator /(SimVec2 v1, float scalar)
        {
            return new SimVec2(v1.x / scalar, v1.y / scalar);
        }

        public float Dot(SimVec2 other)
        {
            return x * other.x + y * other.y;
        }

        public SimVec2 Normalize()
        {
            var m = magnitude; //Compute and store magnitude once, use it several times in this method.
            if (m <= SimMath.Epsilon)
                return zero;

            return new SimVec2(x / m, y / m);
        }

        public static float SquaredDistance(SimVec2 v1, SimVec2 v2)
        {
            return (v2.x - v1.x) * (v2.x - v1.x) + (v2.y - v1.y) * (v2.y - v1.y);
        }

        public bool Equals(SimVec2 other)
        {
            return Math.Abs(x - other.x) < SimMath.Epsilon &&
                   Math.Abs(y - other.y) < SimMath.Epsilon;
        }
    }
}