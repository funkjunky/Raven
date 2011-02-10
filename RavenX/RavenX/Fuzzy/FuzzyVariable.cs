#region File description

//------------------------------------------------------------------------------
//FuzzyVariable.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using Mindcrafters.Library.Math;

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
    ///class to implement a fuzzy variable
    ///</summary>
    public class FuzzyVariable
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public FuzzyVariable()
        {
            MinRange = 0.0f;
            MaxRange = 0.0f;
        }

        ///<summary>
        ///member sets of the variable
        ///</summary>
        public Dictionary<string, FuzzySet> MemberSets
        {
            get { return _memberSets; }
        }

        ///<summary>
        /////the minimum value of the range of this variable
        ///</summary>
        public float MinRange
        {
            get { return _minRange; }
            set { _minRange = value; }
        }

        ///<summary>
        /////the maximum value of the range of this variable
        ///</summary>
        public float MaxRange
        {
            get { return _maxRange; }
            set { _maxRange = value; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///takes a crisp value and calculates its degree of membership for each
        ///set in the variable.
        ///</summary>
        ///<param name="val"></param>
        public void Fuzzify(float val)
        {
            //make sure the value is within the bounds of this variable
            Assert.Fatal((val >= MinRange) && (val <= MaxRange),
                         "FuzzyVariable.Fuzzify>: value out of range");

            //for each set in the flv calculate the DOM for the given value
            foreach (KeyValuePair<string, FuzzySet> kvp in MemberSets)
            {
                kvp.Value.DOM = kvp.Value.CalculateDOM(val);
            }
        }

        ///<summary>
        ///defuzzifies the value by averaging the maxima of the sets that have
        ///fired
        ///</summary>
        ///<returns>sum (maxima * DOM) / sum (DOMs)</returns>
        public float DeFuzzifyMaxAv()
        {
            float bottom = 0.0f;
            float top = 0.0f;

            foreach (KeyValuePair<string, FuzzySet> kvp in MemberSets)
            {
                bottom += kvp.Value.DOM;

                top += kvp.Value.RepresentativeValue*kvp.Value.DOM;
            }

            //make sure bottom is not equal to zero
            if (Epsilon.IsEqual(0, bottom)) return 0.0f;

            return top/bottom;
        }

        ///<summary>
        ///defuzzify the variable using the centroid method
        ///</summary>
        ///<param name="numSamples"></param>
        ///<returns></returns>
        public float DeFuzzifyCentroid(int numSamples)
        {
            //calculate the step size
            float stepSize = (MaxRange - MinRange)/numSamples;

            float totalArea = 0.0f;
            float sumOfMoments = 0.0f;

            //step through the range of this variable in increments equal to
            //stepSize adding up the contribution (lower of CalculateDOM or
            //the actual DOM of this variable's fuzzified value) for each
            //subset. This gives an approximation of the total area of the
            //fuzzy manifold. (This is similar to how the area under a curve
            //is calculated using calculus... the heights of lots of 'slices'
            //are summed to give the total area.)
            //
            //In addition the moment of each slice is calculated and summed.
            //Dividing the total area by the sum of the moments gives the
            //centroid. (Just like calculating the center of mass of an object)
            for (int samp = 1; samp <= numSamples; ++samp)
            {
                //for each set get the contribution to the area. This is the
                //lower of the value returned from CalculateDOM or the actual
                //DOM of the fuzzified value itself   
                foreach (KeyValuePair<string, FuzzySet> kvp in MemberSets)
                {
                    float contribution =
                        Math.Min(
                            kvp.Value.CalculateDOM(MinRange + samp*stepSize),
                            kvp.Value.DOM);

                    totalArea += contribution;

                    sumOfMoments +=
                        (MinRange + samp*stepSize)*contribution;
                }
            }

            //make sure total area is not equal to zero
            if (Epsilon.IsEqual(0, totalArea))
                return 0.0f;

            return (sumOfMoments/totalArea);
        }

        ///<summary>
        ///adds a triangular shaped fuzzy set to the variable
        ///</summary>
        ///<param name="name"></param>
        ///<param name="minBound"></param>
        ///<param name="peak"></param>
        ///<param name="maxBound"></param>
        ///<returns></returns>
        public FzSet AddTriangularSet(
            string name,
            float minBound,
            float peak,
            float maxBound)
        {
            MemberSets[name] =
                new FuzzySetTriangle(peak, peak - minBound, maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(MemberSets[name]);
        }

        ///<summary>
        ///adds a left shoulder type set
        ///</summary>
        ///<param name="name"></param>
        ///<param name="minBound"></param>
        ///<param name="peak"></param>
        ///<param name="maxBound"></param>
        ///<returns></returns>
        public FzSet AddLeftShoulderSet(
            string name,
            float minBound,
            float peak,
            float maxBound)
        {
            MemberSets[name] =
                new FuzzySetLeftShoulder(
                    peak,
                    peak - minBound,
                    maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(MemberSets[name]);
        }

        ///<summary>
        ///adds a left shoulder type set
        ///</summary>
        ///<param name="name"></param>
        ///<param name="minBound"></param>
        ///<param name="peak"></param>
        ///<param name="maxBound"></param>
        ///<returns></returns>
        public FzSet AddRightShoulderSet(
            string name,
            float minBound,
            float peak,
            float maxBound)
        {
            MemberSets[name] =
                new FuzzySetRightShoulder(
                    peak,
                    peak - minBound,
                    maxBound - peak);

            //adjust range if necessary
            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(MemberSets[name]);
        }

        ///<summary>
        ///adds a singleton to the variable
        ///</summary>
        ///<param name="name"></param>
        ///<param name="minBound"></param>
        ///<param name="peak"></param>
        ///<param name="maxBound"></param>
        ///<returns></returns>
        public FzSet AddSingletonSet(
            string name,
            float minBound,
            float peak,
            float maxBound)
        {
            MemberSets[name] =
                new FuzzySetSingleton(
                    peak,
                    peak - minBound,
                    maxBound - peak);

            AdjustRangeToFit(minBound, maxBound);

            return new FzSet(MemberSets[name]);
        }

        #endregion

        #region Private, protected, internal methods

        //---------------------------- AdjustRangeToFit -------------------------------
        //
        // 
        //-----------------------------------------------------------------------------
        ///<summary>
        ///this method is called with the upper and lower bound of a set each
        ///time a new set is added to adjust the upper and lower range values
        ///accordingly
        ///</summary>
        ///<param name="minBound"></param>
        ///<param name="maxBound"></param>
        private void AdjustRangeToFit(float minBound, float maxBound)
        {
            if (minBound < MinRange) MinRange = minBound;
            if (maxBound > MaxRange) MaxRange = maxBound;
        }

        #endregion

        #region Private, protected, internal fields

        private readonly Dictionary<string, FuzzySet> _memberSets =
            new Dictionary<string, FuzzySet>();

        private float _maxRange;
        private float _minRange;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}