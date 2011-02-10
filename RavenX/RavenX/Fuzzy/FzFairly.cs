#region File description

//------------------------------------------------------------------------------
//FzFairly.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;

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
    ///class for implementing Fairly fuzzy term
    ///</summary>
    public class FzFairly : FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="ft"></param>
        public FzFairly(FzSet ft)
        {
            _set = ft.Set;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the fuzzy set
        ///</summary>
        public FuzzySet Set
        {
            get { return _set; }
        }

        #region Public methods

        ///<summary>
        ///retrieves the degree of membership of the term
        ///</summary>
        ///<returns></returns>
        public override float GetDOM()
        {
            return (float) Math.Sqrt(Set.DOM);
        }

        ///<summary>
        ///clone
        ///</summary>
        ///<returns></returns>
        public override FuzzyTerm Clone()
        {
            return new FzFairly(this);
        }

        ///<summary>
        ///clears the degree of membership of the term
        ///</summary>
        public override void ClearDOM()
        {
            Set.ClearDOM();
        }

        ///<summary>
        ///method for updating the DOM of a consequent when a rule fires
        ///</summary>
        ///<param name="val"></param>
        public override void ORwithDOM(float val)
        {
            Set.ORwithDOM((float) Math.Sqrt(val));
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///copy
        ///</summary>
        ///<param name="src"></param>
        private FzFairly(FzFairly src)
        {
            _set = src.Set;
        }

        #endregion

        #region Private, protected, internal fields

        private readonly FuzzySet _set;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}