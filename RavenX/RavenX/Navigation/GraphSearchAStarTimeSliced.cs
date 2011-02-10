#region File description

//------------------------------------------------------------------------------
//GraphSearchAStarTimeSliced.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Mindcrafters.Library.Utility;
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
    ///an A* class where search is completed over multiple update-steps
    ///</summary>
    public sealed class GraphSearchAStarTimeSliced : GraphSearchTimeSliced
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="source"></param>
        ///<param name="target"></param>
        ///<param name="bot">Bot Requesting the path</param>
        public GraphSearchAStarTimeSliced(
            SparseGraph graph,
            int source,
            int target,
            BotEntity bot)
            : base(SearchTypes.AStar, bot)
        {
            _graph = graph;
            _shortestPathTree = new List<NavGraphEdge>(graph.NumNodes);
            _searchFrontier = new List<NavGraphEdge>(graph.NumNodes);
            _gCosts = new List<float>(graph.NumNodes);
            _fCosts = new List<float>(graph.NumNodes);
            for (int i = 0; i < graph.NumNodes; i++)
            {
                ShortestPathTree.Add(null);
                SearchFrontier.Add(null);
                GCosts.Add(0);
                FCosts.Add(0);
            }
            _source = source;
            _target = target;

            //create the priority queue         
            _pq = new IndexedPriorityQueueLow(FCosts, Graph.NumNodes);

            //put the source node on the queue
            PQ.Insert(source);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///this list contains the edges that comprise the shortest path tree -
        ///a directed subtree of the graph that encapsulates the best paths from 
        ///every node on the SPT to the source node.
        ///</summary>
        public override List<NavGraphEdge> ShortestPathTree
        {
            get { return _shortestPathTree; }
        }

        ///<summary>
        ///the graph to be searched
        ///</summary>
        public SparseGraph Graph
        {
            get { return _graph; }
        }

        ///<summary>
        ///indexed by node. Contains the 'real' accumulative cost to that node
        ///</summary>
        public List<float> GCosts
        {
            get { return _gCosts; }
        }

        ///<summary>
        ///indexed by node. Contains the cost from adding GCosts[n] to the
        ///heuristic cost from n to the target node. This is the list the
        ///priority queue indexes into.
        ///</summary>
        public List<float> FCosts
        {
            get { return _fCosts; }
        }

        ///<summary>
        ///this is an indexed (by node) list of 'parent' edges leading to nodes 
        ///connected to the SPT but that have not been added to the SPT yet.
        ///This is a little like the stack or queue used in BST and DST searches.
        ///</summary>
        public List<NavGraphEdge> SearchFrontier
        {
            get { return _searchFrontier; }
        }

        ///<summary>
        ///the source node index
        ///</summary>
        public int Source
        {
            get { return _source; }
        }

        ///<summary>
        ///the source node index
        ///</summary>
        public int Target
        {
            get { return _target; }
        }

        ///<summary>
        ///create an indexed priority queue of nodes. The nodes with the lowest
        ///overall F cost (G+H) are positioned at the front.
        ///</summary>
        public IndexedPriorityQueueLow PQ
        {
            get { return _pq; }
        }

        #region Public methods

        ///<summary>
        ///When called, this method runs the algorithm through one search cycle.
        ///</summary>
        ///<returns>
        ///an enumerated value (TargetFound, TargetNotFound, SearchIncomplete)
        ///indicating the status of the search.
        ///</returns>
        public override int CycleOnce()
        {
            //if the PQ is empty the target has not been found
            if (PQ.Empty())
            {
                return (int) SearchResults.TargetNotFound;
            }

            //get lowest cost node from the queue
            int nextClosestNode = PQ.Pop();

            //move this node from the frontier to the spanning tree
            ShortestPathTree[nextClosestNode] =
                SearchFrontier[nextClosestNode];

            //if the target has been found exit
            if (nextClosestNode == Target)
            {
                return (int) SearchResults.TargetFound;
            }

            //now to test all the edges attached to this node
            foreach (NavGraphEdge curEdge in Graph.Edges[nextClosestNode])
            {
                //calculate (H) the heuristic cost from this node to the target                       
                float hCost =
                    HeuristicEuclid.Calculate(Graph, Target, curEdge.To);

                //calculate (G) the 'real' cost to this node from the source 
                float gCost = GCosts[nextClosestNode] + curEdge.Cost;

                //if the node has not been added to the frontier, add it and
                //update the G and F costs
                if (SearchFrontier[curEdge.To] == null)
                {
                    FCosts[curEdge.To] = gCost + hCost;
                    GCosts[curEdge.To] = gCost;

                    PQ.Insert(curEdge.To);

                    SearchFrontier[curEdge.To] = curEdge;
                }
                    //if this node is already on the frontier but the cost to get
                    //here is cheaper than has been found previously, update the
                    //node costs and frontier accordingly.
                else if ((gCost < GCosts[curEdge.To]) &&
                         (ShortestPathTree[curEdge.To] == null))
                {
                    FCosts[curEdge.To] = gCost + hCost;
                    GCosts[curEdge.To] = gCost;

                    PQ.ChangePriority(curEdge.To);

                    SearchFrontier[curEdge.To] = curEdge;
                }
            }

            //there are still nodes to explore
            return (int) SearchResults.SearchIncomplete;
        }

        ///<summary>
        ///Gets the total cost to the target
        ///</summary>
        ///<returns>the total cost to the target</returns>
        public override float GetCostToTarget()
        {
            return GCosts[Target];
        }

        ///<summary>
        ///Gets a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</summary>
        ///<returns>
        ///a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</returns>
        public override List<int> GetPathToTarget()
        {
            List<int> path = new List<int>();

            //just return an empty path if no target or no path found
            if (Target < 0) return path;

            int nd = Target;

            path.Add(nd);

            while ((nd != Source) && (ShortestPathTree[nd] != null))
            {
                nd = ShortestPathTree[nd].From;

                path.Insert(0, nd);
            }

            return path;
        }

        ///<summary>
        ///Gets the path as a list of PathEdges
        ///</summary>
        ///<returns>the path as a list of PathEdges</returns>
        public override List<PathEdge> GetPathAsPathEdges()
        {
            List<PathEdge> path = new List<PathEdge>();

            //just return an empty path if no target or no path found
            if (Target < 0) return path;

            int nd = Target;

            while ((nd != Source) && (ShortestPathTree[nd] != null))
            {
                path.Insert(
                    0,
                    new PathEdge(
                        Graph.GetNode(
                            ShortestPathTree[nd].From).Position,
                        Graph.GetNode(ShortestPathTree[nd].To).Position,
                        ShortestPathTree[nd].BehaviorType,
                        ShortestPathTree[nd].IntersectingEntityId));

                nd = ShortestPathTree[nd].From;
            }

            return path;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly List<float> _fCosts;
        private readonly List<float> _gCosts;
        private readonly SparseGraph _graph;
        private readonly IndexedPriorityQueueLow _pq;
        private readonly List<NavGraphEdge> _searchFrontier;
        private readonly List<NavGraphEdge> _shortestPathTree;
        private readonly int _source;
        private readonly int _target;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}