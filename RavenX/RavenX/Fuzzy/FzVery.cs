#region File description

//------------------------------------------------------------------------------
//FzVery.cs
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
    ///class for Very fuzzy set
    ///</summary>
    public class FzVery : FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="ft"></param>
        public FzVery(FzSet ft)
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
        ///clone
        ///</summary>
        ///<returns></returns>
        public override FuzzyTerm Clone()
        {
            return new FzVery(this);
        }

        ///<summary>
        ///retrieves the degree of membership of the term
        ///</summary>
        ///<returns></returns>
        public override float GetDOM()
        {
            return Set.DOM*Set.DOM;
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
            Set.ORwithDOM(val*val);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///copy
        ///</summary>
        ///<param name="src"></param>
        private FzVery(FzVery src)
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