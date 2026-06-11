using System;

namespace Sim.Foundation
{
    public static class SimMath
    {
        public const float Epsilon = 1E-06f;

        ///<summary>
        ///Returns value, limited to the range [min, max]
        ///</summary>
        public static float Clamp(float min, float max, float value)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }

        ///<summary>
        /// Method overload that accepts ints.
        ///</summary>
        public static float Clamp(int min, int max, int value)
            => Clamp((float)min, (float)max, (float)value);

        ///<summary>
        ///Returns value, limited to the range [0, 1]
        ///</summary>
        public static float Clamp01(float value)
        {
            if (value < 0)
                return 0;

            if (value > 1)
                return 1;

            return value;
        }

        ///<summary>
        /// Method overload that accepts ints.
        ///</summary>
        public static float Clamp01(int value)
            => Clamp01((float)value);

        ///<summary>
        ///Linearly interpolates between two values, given t.
        ///</summary>
        public static float Lerp(float a, float b, float t)
            => a + (b - a) * t;

        ///<summary>
        ///Returns a fraction representing how far through a value is between two border values
        ///</summary>
        public static float InverseLerp(float a, float b, float t)
        {
            if (a != b)
            {
                return Clamp01((t - a) / (b - a));
            }

            return 0.0f;
        }

        ///<summary>
        ///Returns true if value is almost zero.
        ///</summary>
        public static bool NearZero(float value)
            => Math.Abs(value) <= Epsilon;

        ///<summary>
        /// Method overload that accepts ints.
        ///</summary>
        public static bool NearZero(int value)
            => NearZero((float)value);

        ///<summary>
        ///Converts an angle, in degrees, to a Vector.
        ///</summary>
        public static SimVec2 DegToVector(float degrees)
        {
            float radians = degrees * (MathF.PI / 180);
            return new SimVec2(MathF.Cos(radians), MathF.Sin(radians));
        }

        ///<summary>
        ///Converts an angle, in radians, to a Vector.
        ///</summary>
        public static SimVec2 RadToVector(float rad)
            => new SimVec2(MathF.Cos(rad), MathF.Sin(rad));
    }
}