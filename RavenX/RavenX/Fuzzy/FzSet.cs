#region File description

//------------------------------------------------------------------------------
//FzSet.cs
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
    ///class for fuzzy set proxy
    ///</summary>
    public class FzSet : FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="fs"></param>
        public FzSet(FuzzySet fs)
        {
            _set = fs;
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
        ///clone
        ///</summary>
        ///<returns></returns>
        public override FuzzyTerm Clone()
        {
            return new FzSet(_set);
        }

        ///<summary>
        ///retrieves the degree of membership of the term
        ///</summary>
        ///<returns></returns>
        public override float GetDOM()
        {
            return _set.DOM;
        }

        ///<summary>
        ///clears the degree of membership of the term
        ///</summary>
        public override void ClearDOM()
        {
            _set.ClearDOM();
        }

        ///<summary>
        ///method for updating the DOM of a consequent when a rule fires
        ///</summary>
        ///<param name="val"></param>
        public override void ORwithDOM(float val)
        {
            _set.ORwithDOM(val);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly FuzzySet _set;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}