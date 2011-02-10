#region File description

//------------------------------------------------------------------------------
//FuzzyRule.cs
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
    ///a Fuzzy Rule class
    ///</summary>
    public class FuzzyRule
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="antecedent"></param>
        ///<param name="consequent"></param>
        public FuzzyRule(FuzzyTerm antecedent, FuzzyTerm consequent)
        {
            _antecedent = antecedent.Clone();
            _consequent = consequent.Clone();
        }

        ///<summary>
        ///antecedent (usually a composite of several fuzzy sets and operators)
        ///</summary>
        public FuzzyTerm Antecedent
        {
            get { return _antecedent; }
        }

        ///<summary>
        ///consequent (usually a single fuzzy set, but can be several ANDed
        ///together)
        ///</summary>
        public FuzzyTerm Consequent
        {
            get { return _consequent; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///Clear the degree of membership of the consequence
        ///</summary>
        public void SetConfidenceOfConsequentToZero()
        {
            Consequent.ClearDOM();
        }

        ///<summary>
        ///this method updates the DOM (the confidence) of the consequent term
        ///with the DOM of the antecedent term. 
        ///</summary>
        public void Calculate()
        {
            Consequent.ORwithDOM(Antecedent.GetDOM());
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly FuzzyTerm _antecedent;
        private readonly FuzzyTerm _consequent;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}