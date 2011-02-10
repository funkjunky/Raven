#region File description

//------------------------------------------------------------------------------
//FzOR.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;

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
    ///class for fuzzy OR
    ///</summary>
    public class FzOR : FuzzyTerm
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="fa"></param>
        public FzOR(FzOR fa)
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
        public FzOR(FuzzyTerm op1, FuzzyTerm op2)
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
        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3)
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
        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4)
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
            return new FzOR(this);
        }

        ///<summary>
        ///the OR operator returns the maximum DOM of the sets it is
        ///operating on
        ///</summary>
        ///<returns></returns>
        public override float GetDOM()
        {
            float largest = Single.MinValue;

            foreach (FuzzyTerm curTerm in Terms)
            {
                if (curTerm.GetDOM() > largest)
                {
                    largest = curTerm.GetDOM();
                }
            }

            return largest;
        }

        ///<summary>
        ///unused
        ///</summary>
        public override void ClearDOM()
        {
            Assert.Fatal(false, "FzOR.ClearDOM: invalid context");
        }

        ///<summary>
        ///unused
        ///</summary>
        ///<param name="val"></param>
        public override void ORwithDOM(float val)
        {
            Assert.Fatal(false, "FzOR.ORwithDOM: invalid context");
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