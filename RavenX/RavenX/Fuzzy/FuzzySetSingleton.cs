#region File description

//------------------------------------------------------------------------------
//FuzzySetSingleton.cs
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

#endregion

namespace Mindcrafters.RavenX.Fuzzy
{
    ///<summary>
    ///class for singleton fuzzy set
    ///</summary>
    public class FuzzySetSingleton : FuzzySet
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
        public FuzzySetSingleton(float mid, float left, float right)
            : base(mid)
        {
            _midPoint = mid;
            _leftOffset = left;
            _rightOffset = right;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///mid-point
        ///</summary>
        public float MidPoint
        {
            get { return _midPoint; }
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
            if ((val >= MidPoint - LeftOffset) &&
                (val <= MidPoint + RightOffset))
            {
                return 1.0f;
            }
            //out of range of this FLV, return zero
            return 0.0f;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //the values that define the shape of this FLV
        private readonly float _leftOffset;
        private readonly float _midPoint;
        private readonly float _rightOffset;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}