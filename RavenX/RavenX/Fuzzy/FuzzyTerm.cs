#region File description

//------------------------------------------------------------------------------
//FuzzyTerm.cs
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
    ///abstract class to provide an interface for classes able to be
    ///used as terms in a fuzzy if-then rule base.
    ///</summary>
    public abstract class FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        ///<summary>
        ///clone
        ///</summary>
        ///<returns></returns>
        public abstract FuzzyTerm Clone();

        ///<summary>
        ///retrieves the degree of membership of the term
        ///</summary>
        ///<returns></returns>
        public abstract float GetDOM();

        ///<summary>
        ///clears the degree of membership of the term
        ///</summary>
        public abstract void ClearDOM();

        ///<summary>
        ///method for updating the DOM of a consequent when a rule fires
        ///</summary>
        ///<param name="val"></param>
        public abstract void ORwithDOM(float val);

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}