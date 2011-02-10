#region File description
//------------------------------------------------------------------------------
//Epsilon.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.MathUtil;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class Epsilon

    ///<summary>
    ///class for handling small things
    ///</summary>
    public class Epsilon
    {
        //======================================================================
        #region Static methods, fields, constructors

        private const float DEFAULT_EPSILON = 0.000001f;

        private static EpsilonTester _epsilonTester =
            new EpsilonTester(DEFAULT_EPSILON);

        ///<summary>
        ///Epsilon value used in comparisons.
        ///</summary>
        public float Value
        {
            get { return _epsilonTester.Value; }
            set { _epsilonTester.Value = value; }
        }

        ///<summary>
        ///Tests if <paramref name="a"/> and <paramref name="b"/>
        ///are nearly equal
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if <paramref name="a"/> and <paramref name="b"/>
        ///are nearly equal
        ///</returns>
        public static bool IsEqual(float a, float b)
        {
            return IsZero(a - b);
        }

        ///<summary>
        ///Tests if vectors <paramref name="v1"/> and <paramref name="v2"/>
        ///are nearly equal
        ///</summary>
        ///<param name="v1"></param>
        ///<param name="v2"></param>
        ///<returns>
        ///true if vectors <paramref name="v1"/> and <paramref name="v2"/>
        ///are nearly equal
        ///</returns>
        public static bool IsEqual(Vector2 v1, Vector2 v2)
        {
            return IsZero(v1 - v2);
        }

        ///<summary>
        ///Tests if <paramref name="a"/> and <paramref name="b"/>
        ///are not nearly equal
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if <paramref name="a"/> and <paramref name="b"/>
        ///are not nearly equal
        ///</returns>
        public static bool IsNotEqual(float a, float b)
        {
            return IsNotZero(a - b);
        }

        ///<summary>
        ///Tests if <paramref name="f"/> is not nearly zero.
        ///</summary>
        ///<param name="f">Float to test.</param>
        ///<returns>true if <paramref name="f"/> is not nearly zero.</returns>
        public static bool IsNotZero(float f)
        {
            return _epsilonTester.FloatIsNotZero(f);
        }

        ///<summary>
        ///Tests if <paramref name="f"/> is nearly zero.
        ///</summary>
        ///<param name="f">Float to test.</param>
        ///<returns>true if <paramref name="f"/> is nearly zero.</returns>
        public static bool IsZero(float f)
        {
            return _epsilonTester.FloatIsZero(f);
        }

        ///<summary>
        ///Tests if <paramref name="v"/> is nearly zero.
        ///</summary>
        ///<param name="v">Vector to test.</param>
        ///<returns>true if <paramref name="v"/> is nearly zero.</returns>
        public static bool IsZero(Vector2 v)
        {
            return _epsilonTester.VectorIsZero(v);
        }

        ///<summary>
        ///Tests if <paramref name="v"/> is nearly zero.
        ///</summary>
        ///<param name="v">Vector to test.</param>
        ///<returns>true <paramref name="v"/> is nearly zero.</returns>
        public static bool IsZero(Vector3 v)
        {
            return _epsilonTester.VectorIsZero(v);
        }

        ///<summary>
        ///Tests if <paramref name="v"/> is nearly zero.
        ///</summary>
        ///<param name="v">Vector to test.</param>
        ///<returns>true if <paramref name="v"/> is nearly zero.</returns>
        public static bool IsZero(Vector4 v)
        {
            return _epsilonTester.VectorIsZero(v);
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
