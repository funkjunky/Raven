#region File description

//------------------------------------------------------------------------------
//HeuristicNoisyEuclid.cs
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

using GarageGames.Torque.Util;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///this uses the Euclidean distance but adds in an amount of noise to the 
    ///result. You can use this heuristic to provide imperfect paths. This can
    ///be handy if you find that you frequently have lots of agents all
    ///following each other in single file to get from one place to another
    ///</summary>
    public class HeuristicNoisyEuclid
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///calculate the straight line distance from node nd1 to node nd2
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="nd1"></param>
        ///<param name="nd2"></param>
        ///<returns></returns>
        public static float Calculate(SparseGraph graph, int nd1, int nd2)
        {
            float result = TorqueUtil.GetFastRandomFloat(0.9f, 1.1f)*
                           (graph.GetNode(nd1).Position -
                            graph.GetNode(nd2).Position).Length();
            return result;
        }

        #endregion

        #region Constructors

        #endregion

        #region Public methods

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