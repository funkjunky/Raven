#region File description

//------------------------------------------------------------------------------
//HeuristicDijkstra.cs
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
    ///you can use this class to turn the A* algorithm into Dijkstra's search.
    ///this is because Dijkstra's is equivalent to an A* search using a
    ///heuristic value that is always equal to zero.
    ///</summary>
    public class HeuristicDijkstra
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///heuristic estimate always 0
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="nd1"></param>
        ///<param name="nd2"></param>
        ///<returns></returns>
        public static float Calculate(SparseGraph graph, int nd1, int nd2)
        {
            return 0;
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