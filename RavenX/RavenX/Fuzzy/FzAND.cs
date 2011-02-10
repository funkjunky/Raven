#region File description

//------------------------------------------------------------------------------
//FzAND.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;

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
    ///a fuzzy AND operator class
    ///</summary>
    public class FzAND : FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="fa"></param>
        public FzAND(FzAND fa)
        {
            foreach (FuzzyTerm curTerm in fa.Terms)
            {
                Terms.Add(curTerm.Clone());
            }
        }

        ///<summary>
        ///constructor using two terms
        ///</summary>
        ///<param name="op1"></param>
        ///<param name="op2"></param>
        public FzAND(FuzzyTerm op1, FuzzyTerm op2)
        {
            Terms.Add(op1.Clone());
            Terms.Add(op2.Clone());
        }

        ///<summary>
        ///constructor using three terms
        ///</summary>
        ///<param name="op1"></param>
        ///<param name="op2"></param>
        ///<param name="op3"></param>
        public FzAND(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3)
        {
            Terms.Add(op1.Clone());
            Terms.Add(op2.Clone());
            Terms.Add(op3.Clone());
        }

        ///<summary>
        ///constructor using four terms
        ///</summary>
        ///<param name="op1"></param>
        ///<param name="op2"></param>
        ///<param name="op3"></param>
        ///<param name="op4"></param>
        public FzAND(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4)
        {
            Terms.Add(op1.Clone());
            Terms.Add(op2.Clone());
            Terms.Add(op3.Clone());
            Terms.Add(op4.Clone());
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///an instance of this class may AND together up to 4 terms
        ///</summary>
        public List<FuzzyTerm> Terms
        {
            get { return _terms; }
        }

        #region Public methods

        ///<summary>
        ///clone
        ///</summary>
        ///<returns></returns>
        public override FuzzyTerm Clone()
        {
            return new FzAND(this);
        }

        ///<summary>
        ///the AND operator returns the minimum DOM of the sets it is
        ///operating on
        ///</summary>
        ///<returns></returns>
        public override float GetDOM()
        {
            float smallest = Single.MaxValue;

            foreach (FuzzyTerm curTerm in Terms)
            {
                if (curTerm.GetDOM() < smallest)
                {
                    smallest = curTerm.GetDOM();
                }
            }

            return smallest;
        }

        ///<summary>
        ///method for updating the DOM of a consequent when a rule fires
        ///</summary>
        ///<param name="val"></param>
        public override void ORwithDOM(float val)
        {
            foreach (FuzzyTerm curTerm in Terms)
            {
                curTerm.ORwithDOM(val);
            }
        }

        ///<summary>
        ///clears the degree of membership of the term
        ///</summary>
        public override void ClearDOM()
        {
            foreach (FuzzyTerm curTerm in Terms)
            {
                curTerm.ClearDOM();
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly List<FuzzyTerm> _terms = new List<FuzzyTerm>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}