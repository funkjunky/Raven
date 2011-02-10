#region File description

//------------------------------------------------------------------------------
//FuzzySetTriangle.cs
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

#endregion

#region Mindcrafters

#endregion

using Mindcrafters.Library.Math;

#endregion

namespace Mindcrafters.RavenX.Fuzzy
{
    ///<summary>
    ///class for triangular shaped fuzzy set
    ///</summary>
    public class FuzzySetTriangle : FuzzySet
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="mid"></param>
        ///<param name="left"></param>
        ///<param name="right"></param>
        public FuzzySetTriangle(float mid, float left, float right)
            : base(mid)
        {
            PeakPoint = mid;
            LeftOffset = left;
            RightOffset = right;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///peak point
        ///</summary>
        public float PeakPoint
        {
            get { return _peakPoint; }
            set { _peakPoint = value; }
        }

        ///<summary>
        ///left offset
        ///</summary>
        public float LeftOffset
        {
            get { return _leftOffset; }
            set { _leftOffset = value; }
        }

        ///<summary>
        ///right offset
        ///</summary>
        public float RightOffset
        {
            get { return _rightOffset; }
            set { _rightOffset = value; }
        }

        #region Public methods

        ///<summary>
        ///returns the degree of membership in this set of the given value.
        ///NOTE, this does not set <see cref="FuzzySet._dom"/> to the degree of
        ///membership of the value passed as the parameter. This is because the
        ///centroid defuzzification method also uses this method to determine
        ///the DOMs of the values it uses as its sample points.
        ///</summary>
        ///<param name="val">given value</param>
        ///<returns>
        ///the degree of membership in this set of the given value.
        ///</returns>
        public override float CalculateDOM(float val)
        {
            //test for the case where the triangle's left or right offsets are
            //zero (to prevent divide by zero errors below)
            if ((Epsilon.IsEqual(RightOffset, 0.0f) &&
                 (Epsilon.IsEqual(PeakPoint, val))) ||
                (Epsilon.IsEqual(LeftOffset, 0.0f) &&
                 (Epsilon.IsEqual(PeakPoint, val))))
            {
                return 1.0f;
            }

            //find DOM if left of center
            if ((val <= PeakPoint) && (val >= (PeakPoint - LeftOffset)))
            {
                float grad = 1.0f/LeftOffset;
                return grad*(val - (PeakPoint - LeftOffset));
            }

            //find DOM if right of center
            if ((val > PeakPoint) && (val < (PeakPoint + RightOffset)))
            {
                float grad = 1.0f/-RightOffset;
                return grad*(val - PeakPoint) + 1.0f;
            }
            //out of range of this FLV, return zero
            return 0.0f;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //the values that define the shape of this FLV
        private float _leftOffset;
        private float _peakPoint;
        private float _rightOffset;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}