#region File description

//------------------------------------------------------------------------------
//HeuristicEuclid.cs
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

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///the Euclidean heuristic (straight-line distance)
    ///</summary>
    public class HeuristicEuclid
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
            return (graph.GetNode(nd1).Position - graph.GetNode(nd2).Position).Length();
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