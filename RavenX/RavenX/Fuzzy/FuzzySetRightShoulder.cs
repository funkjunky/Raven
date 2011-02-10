#region File description

//------------------------------------------------------------------------------
//FuzzySetRightShoulder.cs
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
    ///class for right shoulder type fuzzy set
    ///</summary>
    public class FuzzySetRightShoulder : FuzzySet
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="peak"></param>
        ///<param name="left"></param>
        ///<param name="right"></param>
        public FuzzySetRightShoulder(float peak, float left, float right)
            : base(((peak + right) + peak)/2)
        {
            _peakPoint = peak;
            _leftOffset = left;
            _rightOffset = right;
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
        }

        ///<summary>
        ///left offset
        ///</summary>
        public float LeftOffset
        {
            get { return _leftOffset; }
        }

        ///<summary>
        ///right offset
        ///</summary>
        public float RightOffset
        {
            get { return _rightOffset; }
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
            //test for the case where the left or right offsets are zero
            //(to prevent divide by zero errors below)
            if ((Epsilon.IsEqual(RightOffset, 0.0f) &&
                 (Epsilon.IsEqual(PeakPoint, val))) ||
                (Epsilon.IsEqual(LeftOffset, 0.0f) &&
                 (Epsilon.IsEqual(PeakPoint, val))))
            {
                return 1.0f;
            }

            //find DOM if left of center
            if ((val <= PeakPoint) && (val > (PeakPoint - LeftOffset)))
            {
                float grad = 1.0f/LeftOffset;
                return grad*(val - (PeakPoint - LeftOffset));
            }

            //find DOM if right of center and less than center + right offset
            if ((val > PeakPoint) && (val <= PeakPoint + RightOffset))
            {
                return 1.0f;
            }

            //out of range of this FLV, return zero
            return 0f;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //the values that define the shape of this FLV
        private readonly float _leftOffset;
        private readonly float _peakPoint;
        private readonly float _rightOffset;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}