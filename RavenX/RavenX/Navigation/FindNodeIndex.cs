#region File description

//------------------------------------------------------------------------------
//FindNodeIndex.cs
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

using Mindcrafters.RavenX.Graph;

namespace Mindcrafters.RavenX.Navigation
{
    ///<summary>
    ///class for implementing test for successful search for a target node
    ///</summary>
    public class FindNodeIndex
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Tests if the current node is the target node. 
        ///Used for search termination.
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="target"></param>
        ///<param name="currentNodeIdx"></param>
        ///<returns></returns>
        public static bool IsSatisfied(
            SparseGraph graph,
            int target,
            int currentNodeIdx)
        {
            return currentNodeIdx == target;
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