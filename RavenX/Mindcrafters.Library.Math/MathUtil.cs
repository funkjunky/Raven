#region File description
//------------------------------------------------------------------------------
//MathUtil.cs
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

using GarageGames.Torque.Core;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class MathUtil

    ///<summary>
    ///class for some useful math utilities
    ///</summary>
    public class MathUtil
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///clamps the first argument between the second two
        ///</summary>
        ///<param name="arg"></param>
        ///<param name="minVal"></param>
        ///<param name="maxVal"></param>
        public static void Clamp(ref float arg, float minVal, float maxVal)
        {
            Assert.Fatal((minVal < maxVal),
                "MathUtil.Clamp: maxVal < minVal!");

            if (arg < minVal)
            {
                arg = minVal;
            }

            if (arg > maxVal)
            {
                arg = maxVal;
            }
        }

        ///<summary>
        ///clamps the first argument between the second two
        ///</summary>
        ///<param name="arg"></param>
        ///<param name="minVal"></param>
        ///<param name="maxVal"></param>
        public static void Clamp(ref int arg, int minVal, int maxVal)
        {
            Assert.Fatal((minVal < maxVal),
                "MathUtil.Clamp: maxVal < minVal!");

            if (arg < minVal)
            {
                arg = minVal;
            }

            if (arg > maxVal)
            {
                arg = maxVal;
            }
        }

        ///<summary>
        ///clamps the first argument between the second two
        ///</summary>
        ///<param name="arg"></param>
        ///<param name="minVal"></param>
        ///<param name="maxVal"></param>
        ///<returns>clamped value</returns>
        public static float Clamp(float arg, float minVal, float maxVal)
        {
            Assert.Fatal((minVal < maxVal),
                "MathUtil.Clamp: maxVal < minVal!");

            if (arg < minVal)
            {
                return minVal;
            }

            return arg > maxVal ? maxVal : arg;
        }

        ///<summary>
        ///clamps the first argument between the second two
        ///</summary>
        ///<param name="arg"></param>
        ///<param name="minVal"></param>
        ///<param name="maxVal"></param>
        ///<returns>clamped value</returns>
        public static int Clamp(int arg, int minVal, int maxVal)
        {
            Assert.Fatal((minVal < maxVal),
                "MathUtil.Clamp: maxVal < minVal!");

            if (arg < minVal)
            {
                return minVal;
            }

            return arg > maxVal ? maxVal : arg;
        }

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
