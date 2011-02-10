#region File description

//------------------------------------------------------------------------------
//FuzzyModule.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

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
    ///this class describes a fuzzy module: a collection of fuzzy variables
    ///and the rules that operate on them.
    ///</summary>
    public class FuzzyModule
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region CrossSections enum

        ///<summary>
        ///when calculating the centroid of the fuzzy manifold this value is
        ///used to determine how many cross-sections should be sampled
        ///</summary>
        public enum CrossSections
        {
            NumSamples = 15
        } ;

        #endregion

        #region DefuzzifyMethod enum

        ///<summary>
        ///you must pass one of these values to the <see cref="DeFuzzify"/>
        ///method. This module only supports the MaxAv and Centroid methods.
        ///</summary>
        public enum DefuzzifyMethod
        {
            MaxAv,
            Centroid
        } ;

        #endregion

        ///<summary>
        ///a map of all the fuzzy variables this module uses
        ///</summary>
        public Dictionary<string, FuzzyVariable> Variables
        {
            get { return _variables; }
        }

        ///<summary>
        ///a list containing all the fuzzy rules
        ///</summary>
        public List<FuzzyRule> Rules
        {
            get { return _rules; }
        }

        #region Public methods

        ///<summary>
        ///this method calls the Fuzzify method of the variable with the same
        ///name as the key
        ///</summary>
        ///<param name="nameOfFLV"></param>
        ///<param name="val"></param>
        public void Fuzzify(string nameOfFLV, float val)
        {
            //first make sure the key exists
            Assert.Fatal(Variables.ContainsKey(nameOfFLV),
                         "FuzzyModule.Fuzzify>: key not found");

            Variables[nameOfFLV].Fuzzify(val);
        }

        ///<summary>
        ///given a fuzzy variable and a defuzzification method this returns a 
        ///crisp value
        ///</summary>
        ///<param name="nameOfFLV"></param>
        ///<param name="method"></param>
        ///<returns></returns>
        public float DeFuzzify(string nameOfFLV, DefuzzifyMethod method)
        {
            //first make sure the key exists
            Assert.Fatal(Variables.ContainsKey(nameOfFLV),
                         "FuzzyModule.DeFuzzify: key not found");

            //clear the DOMs of all the consequents of all the rules
            SetConfidencesOfConsequentsToZero();

            //process the rules
            foreach (FuzzyRule curRule in Rules)
            {
                curRule.Calculate();
            }

            //now defuzzify the resultant conclusion using the specified method
            switch (method)
            {
                case DefuzzifyMethod.Centroid:
                    return Variables[nameOfFLV].DeFuzzifyCentroid(
                        (int) CrossSections.NumSamples);

                case DefuzzifyMethod.MaxAv:
                    return Variables[nameOfFLV].DeFuzzifyMaxAv();
            }

            return 0;
        }

        ///<summary>
        ///add a rule
        ///</summary>
        ///<param name="antecedent"></param>
        ///<param name="consequence"></param>
        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence)
        {
            Rules.Add(new FuzzyRule(antecedent, consequence));
        }

        ///<summary>
        ///creates a new fuzzy variable and returns a reference to it
        ///</summary>
        ///<param name="varName"></param>
        ///<returns></returns>
        public FuzzyVariable CreateFLV(string varName)
        {
            Variables[varName] = new FuzzyVariable();

            return Variables[varName];
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///zeros the DOMs of the consequents of each rule
        ///</summary>
        private void SetConfidencesOfConsequentsToZero()
        {
            foreach (FuzzyRule curRule in Rules)
            {
                curRule.SetConfidenceOfConsequentToZero();
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly List<FuzzyRule> _rules = new List<FuzzyRule>();

        private readonly Dictionary<string, FuzzyVariable> _variables =
            new Dictionary<string, FuzzyVariable>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}