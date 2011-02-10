#region File description

//------------------------------------------------------------------------------
//FuzzySet.cs
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

namespace Mindcrafters.RavenX.Fuzzy
{
    ///<summary>
    ///class to define an interface for a fuzzy set
    ///</summary>
    public abstract class FuzzySet
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="representativeValue"></param>
        protected FuzzySet(float representativeValue)
        {
            _dom = 0.0f;
            _representativeValue = representativeValue;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///this is the maximum of the set's membership function. For instance, 
        ///if the set is triangular then this will be the peak point of the 
        ///triangular. If the set has a plateau then this value will be the
        ///mid-point of the plateau. This value is set in the constructor to
        ///avoid run-time calculation of mid-point values.
        ///</summary>
        public float RepresentativeValue
        {
            get { return _representativeValue; }
        }

        ///<summary>
        ///this will hold the degree of membership of a given value in this set
        ///</summary>
        public float DOM
        {
            get { return _dom; }
            set
            {
                Assert.Fatal((value <= 1) && (value >= 0),
                             "FuzzySet.SetDOM: invalid value");
                _dom = value;
            }
        }

        #region Public methods

        ///<summary>
        ///returns the degree of membership in this set of the given value.
        ///NOTE, this does not set <see cref="_dom"/> to the degree of
        ///membership of the value passed as the parameter. This is because the
        ///centroid defuzzification method also uses this method to determine
        ///the DOMs of the values it uses as its sample points.
        ///</summary>
        ///<param name="val">given value</param>
        ///<returns>
        ///the degree of membership in this set of the given value.
        ///</returns>
        public abstract float CalculateDOM(float val);

        ///<summary>
        ///if this fuzzy set is part of a consequent FLV, and it is fired by a
        ///rule then this method sets the DOM (in this context, the DOM
        ///represents a confidence level) to the maximum of the parameter value
        ///or the set's existing <see cref="_dom"/> value.
        ///</summary>
        ///<param name="val"></param>
        public void ORwithDOM(float val)
        {
            if (val > _dom)
            {
                _dom = val;
            }
        }

        ///<summary>
        ///Clear the degree of membership
        ///</summary>
        public void ClearDOM()
        {
            _dom = 0.0f;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly float _representativeValue;
        private float _dom;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}