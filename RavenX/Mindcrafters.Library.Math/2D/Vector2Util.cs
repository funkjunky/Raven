#region File description
//------------------------------------------------------------------------------
//Vector2Util.cs
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
#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class Vector2Util

    ///<summary>
    ///class for some useful Vector2 utilities
    ///</summary>
    public class Vector2Util
    {
        //======================================================================
        #region Static methods, fields, constructors

        //public enum Direction {clockwise = 1, anticlockwise = -1};

        ///<summary>
        ///Tests direction of v2 relative to v1
        ///<remarks>
        ///Y axis points down, X axis points right
        ///</remarks>
        ///</summary>
        ///<param name="v1"></param>
        ///<param name="v2"></param>
        ///<returns>
        ///1 if v2 is clockwise from v1. 
        ///-1 if v2 is anticlockwise from v1.
        ///</returns>
        public static int Sign(Vector2 v1, Vector2 v2)
        {
            if (v1.Y * v2.X > v1.X * v2.Y)
            {
                return -1; //anticlockwise;
            }
            return 1;  //clockwise;
        }

        ///<summary>
        ///Determines a vector perpendicular to v
        ///</summary>
        ///<param name="v"></param>
        ///<returns>a vector perpendicular to v</returns>
        public static Vector2 Perp(Vector2 v)
        {
          return new Vector2(-v.Y, v.X);
        }

        ///<summary>
        ///truncates a vector so that its length does not
        ///exceed <paramref name="maxLength"/>
        ///</summary>
        ///<param name="v"></param>
        ///<param name="maxLength"></param>
        public static void Truncate(ref Vector2 v, float maxLength)
        {
            if (v.Length() <= maxLength) 
                return;

            v.Normalize();
            v *= maxLength;
        }

        ///<summary>
        ///truncates a vector so that its length does not
        ///exceed <paramref name="maxLength"/>
        ///</summary>
        ///<param name="v"></param>
        ///<param name="maxLength"></param>
        ///<returns>the truncated vector</returns>
        public static Vector2 Truncate(Vector2 v, float maxLength)
        {
            if (v.Length() > maxLength)
            {
                v.Normalize();
                v *= maxLength;
            }
            return v;
        }

        ///<summary>
        ///Tests if the target position is in the field of view of the entity
        ///positioned at <paramref name="positionFirst"/>
        ///facing in <paramref name="facingFirst"/>
        ///</summary>
        ///<param name="positionFirst"></param>
        ///<param name="facingFirst"></param>
        ///<param name="positionSecond"></param>
        ///<param name="fov"></param>
        ///<returns>
        ///true if the target position is in the field of view of the entity
        ///positioned at <paramref name="positionFirst"/>
        ///facing in <paramref name="facingFirst"/>
        ///</returns>
        public static bool IsSecondInFOVOfFirst(
            Vector2 positionFirst,
            Vector2 facingFirst,
            Vector2 positionSecond,
            float fov)
        {
          Vector2 toTarget = Vector2.Normalize(positionSecond - positionFirst);

          return Vector2.Dot(facingFirst, toTarget) >= System.Math.Cos(fov/2.0);
        }

        ///<summary>
        ///Convert vector to string representation applying format "F2" to
        ///the X and Y co-ordinates
        ///</summary>
        ///<param name="v">vector</param>
        ///<returns>string representation of v</returns>
        public static string ToString(Vector2 v)
        {
            return ToString(v, "F2", "F2");
        }

        ///<summary>
        ///Convert vector to string representation applying
        ///<paramref name="formatXandY"/> to the X and Y co-ordinates
        ///</summary>
        ///<param name="v">vector</param>
        ///<param name="formatXandY">
        ///format to apply to both co-ordinates
        /// </param>
        ///<returns>string representation of v</returns>
        public static string ToString(Vector2 v, string formatXandY)
        {
            return ToString(v, formatXandY, formatXandY);
        }

        ///<summary>
        ///Convert vector to string representation applying
        ///<paramref name="formatX"/> to the X co-ordinate and
        ///<paramref name="formatY"/> to the Y co-ordinate
        ///</summary>
        ///<param name="v">vector</param>
        ///<param name="formatX">format to apply to the X co-ordinate</param>
        ///<param name="formatY">format to apply to the Y co-ordinate</param>
        ///<returns>string representation of v</returns>
        public static string ToString(Vector2 v, string formatX, string formatY)
        {
            return "{X:" + v.X.ToString(formatX) + 
                " Y:" + v.Y.ToString(formatY) + "}";
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
