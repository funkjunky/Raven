#region File description

//------------------------------------------------------------------------------
//GraphSearchTimeSliced.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Graph;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Navigation
{
    ///<summary>
    ///base class for time sliced graph search algorithms
    ///</summary>
    public abstract class GraphSearchTimeSliced
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="searchType"></param>
        protected GraphSearchTimeSliced(SearchTypes searchType, Entity.Bot.BotEntity bot)
        {
            _bot = bot;
            _searchType = searchType;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region SearchResults enum

        ///<summary>
        ///these enums are used as return values from each search update method
        ///</summary>
        public enum SearchResults
        {
            TargetFound,
            TargetNotFound,
            SearchIncomplete
        } ;

        #endregion

        #region SearchTypes enum

        ///<summary>
        ///types of searches
        ///</summary>
        public enum SearchTypes
        {
            AStar,
            Dijkstra
        } ;

        #endregion

        ///<summary>
        ///type of search
        ///</summary>
        public SearchTypes SearchType
        {
            get { return _searchType; }
        }

        public BotEntity Bot
        {
            get { return _bot; }
        }

        ///<summary>
        ///this list contains the edges that comprise the shortest path tree -
        ///a directed subtree of the graph that encapsulates the best paths from 
        ///every node on the SPT to the source node.
        ///</summary>
        public abstract List<NavGraphEdge> ShortestPathTree { get; }

        #region Public methods

        ///<summary>
        ///When called, this method runs the algorithm through one search cycle.
        ///TODO: refactor non-time-sliced searches to replace body of while loop
        ///with CycleOnce
        ///</summary>
        ///<returns>
        ///an enumerated value (TargetFound, TargetNotFound, SearchIncomplete)
        ///indicating the status of the search.
        ///</returns>
        public abstract int CycleOnce();

        ///<summary>
        ///Gets the total cost to the target
        ///</summary>
        ///<returns>the total cost to the target</returns>
        public abstract float GetCostToTarget();

        ///<summary>
        ///Gets a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</summary>
        ///<returns>
        ///a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</returns>
        public abstract List<int> GetPathToTarget();

        ///<summary>
        ///Gets the path as a list of PathEdges
        ///</summary>
        ///<returns>the path as a list of PathEdges</returns>
        public abstract List<PathEdge> GetPathAsPathEdges();

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly SearchTypes _searchType;
        private readonly BotEntity _bot;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}