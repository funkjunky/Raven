#region File description
//------------------------------------------------------------------------------
//RandomUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft
#endregion

#region GarageGames

using GarageGames.Torque.Util;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class RandomUtil

    ///<summary>
    ///class for random generator utilities
    ///</summary>
    public class RandomUtil
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///Generates a random integer between <paramref name="inclusiveMin"/>
        ///and <paramref name="exclusiveMax"/>-1 inclusive
        ///</summary>
        ///<param name="inclusiveMin"></param>
        ///<param name="exclusiveMax"></param>
        ///<returns>
        ///a random integer between <paramref name="inclusiveMin"/>
        ///and <paramref name="exclusiveMax"/>-1 inclusive
        ///</returns>
        public static int RandomInt(int inclusiveMin, int exclusiveMax)
        {
            return TorqueUtil.GetFastRandomInt(inclusiveMin, exclusiveMax);
        }

        ///<summary>
        ///Generates a random integer between <paramref name="inclusiveMin"/>
        ///and <paramref name="inclusiveMax"/> inclusive
        ///</summary>
        ///<param name="inclusiveMin"></param>
        ///<param name="inclusiveMax"></param>
        ///<returns>
        ///a random integer between <paramref name="inclusiveMin"/>
        ///and <paramref name="inclusiveMax"/> inclusive</returns>
        public static int RandomIntInRange(int inclusiveMin, int inclusiveMax)
        {
            return TorqueUtil.GetFastRandomInt(inclusiveMin, inclusiveMax + 1);
        }

        ///<summary>
        ///Generates a random float between 0 and 1 inclusive
        ///</summary>
        ///<returns>a random float between 0 and 1 inclusive</returns>
        public static float RandomFloat()
        {
            return TorqueUtil.GetFastRandomFloat();
        }

        ///<summary>
        ///Generates a random float between <paramref name="inclusiveMin"/>
        ///and <paramref name="inclusiveMax"/> inclusive
        ///</summary>
        ///<param name="inclusiveMin"></param>
        ///<param name="inclusiveMax"></param>
        ///<returns>
        ///a random float between <paramref name="inclusiveMin"/>
        ///and <paramref name="inclusiveMax"/> inclusive</returns>
        public static float RandomFloatInRange(
            float inclusiveMin, float inclusiveMax)
        {
            return TorqueUtil.GetFastRandomFloat(inclusiveMin, inclusiveMax);
        }

        ///<summary>
        ///Generates a random bool
        ///</summary>
        ///<returns>a random bool</returns>
        public static bool RandomBool()
        {
            return RandomIntInRange(0, 1) == 1;
        }

        ///<summary>
        ///Generates a random float in the range -1 < n < 1
        ///TODO: need more meaningful name for this method
        ///</summary>
        ///<returns>a random float in the range -1 < n < 1</returns>
        public static float RandomClamped()
        {
            return RandomFloat() - RandomFloat();
        }

        ///<summary>
        ///Generates a random number with a normal (0,1) distribution.
        ///<remarks>
        ///See method at http://www.taygeta.com/random/gaussian.html
        ///</remarks>
        ///</summary>
        ///<returns>a random number with a normal (0,1) distribution.</returns>
        public static float RandomGaussian()
        {
            return RandomGaussian(0, 1);
        }

        //returns a random number with a normal distribution. 
        ///<summary>
        ///Generates a random number with a normal
        ///(<paramref name="mean"/>,<paramref name="standardDeviation"/>)
        ///distribution.
        ///<remarks>
        ///See method at http://www.taygeta.com/random/gaussian.html
        ///</remarks>
        ///</summary>
        ///<param name="mean"></param>
        ///<param name="standardDeviation"></param>
        ///<returns>a random number with a normal distribution.</returns>
        public static float RandomGaussian(float mean, float standardDeviation)
        {
            float y1;

            if (_randomGaussianUseLast) //use value from previous call
            {
                y1 = _randomGaussianUseLastValue;
                _randomGaussianUseLast = false;
            }
            else
            {
                float x1;
                float x2;
                float w;
                do
                {
                    x1 = 2.0f * RandomFloat() - 1.0f;
                    x2 = 2.0f * RandomFloat() - 1.0f;
                    w = x1 * x1 + x2 * x2;
                }
                while (w >= 1.0f);

                w = (float)System.Math.Sqrt((-2.0 * System.Math.Log(w)) / w);
                y1 = x1 * w;
                _randomGaussianUseLastValue = x2 * w;
                _randomGaussianUseLast = true;
            }

            return mean + y1 * standardDeviation;
        }
        private static bool _randomGaussianUseLast;
        private static float _randomGaussianUseLastValue;

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }

    #endregion
}

