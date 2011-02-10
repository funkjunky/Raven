#region File description

//------------------------------------------------------------------------------
//GraphSearchDijkstrasTimeSliced.cs
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
    ///Dijkstra's algorithm class modified to spread a search over multiple
    ///update-steps
    ///</summary>
    public sealed class GraphSearchDijkstrasTimeSliced : GraphSearchTimeSliced
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
        ///<param name="bot">The bot requesting the search</param>
        public GraphSearchDijkstrasTimeSliced
            (SparseGraph graph,
             int source,
             int target,
             BotEntity bot)
            : base(SearchTypes.Dijkstra, bot)
        {
            _graph = graph;
            _shortestPathTree = new List<NavGraphEdge>(graph.NumNodes);
            _searchFrontier = new List<NavGraphEdge>(graph.NumNodes);
            _costToThisNode = new List<float>(graph.NumNodes);
            for (int i = 0; i < graph.NumNodes; i++)
            {
                ShortestPathTree.Add(null);
                SearchFrontier.Add(null);
                CostToThisNode.Add(0);
            }
            _source = source;
            Target = target;

            //create the PQ         
            PQ = new IndexedPriorityQueueLow(CostToThisNode, Graph.NumNodes);

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
            set { _target = value; }
        }

        ///<summary>
        ///cost to this node from source node
        ///</summary>
        public List<float> CostToThisNode
        {
            get { return _costToThisNode; }
        }

        ///<summary>
        ///create an indexed priority queue of nodes. The nodes with the lowest
        ///cost are positioned at the front.
        public IndexedPriorityQueueLow PQ
        {
            get { return _pq; }
            set { _pq = value; }
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
            ShortestPathTree[nextClosestNode] = SearchFrontier[nextClosestNode];

            //if the target has been found exit
            if (FindActiveTrigger.IsSatisfied(Graph, Target, nextClosestNode,Bot))
            {
                //save the node index that has satisfied the condition. This is
                //so we can work backwards from the index to extract the path
                //from the shortest path tree.
                Target = nextClosestNode;

                return (int) SearchResults.TargetFound;
            }

            //now to test all the edges attached to this node
            foreach (NavGraphEdge curEdge in Graph.Edges[nextClosestNode])
            {
                //the total cost to the node this edge points to is the cost to
                //the /current node plus the cost of the edge connecting them.
                float newCost = CostToThisNode[nextClosestNode] + curEdge.Cost;

                //if this edge has never been on the frontier save the cost to
                //get to the node it points to, then add the edge to the
                //frontier and the destination node to the PQ.
                if (SearchFrontier[curEdge.To] == null)
                {
                    CostToThisNode[curEdge.To] = newCost;

                    PQ.Insert(curEdge.To);

                    SearchFrontier[curEdge.To] = curEdge;
                }
                    //else test to see if the cost to reach the destination node via
                    //the current node is cheaper than the cheapest cost found so
                    //far. If this path is cheaper, we assign the new cost to the
                    //destination node, update its entry in the PQ to reflect the
                    //change and add the edge to the frontier
                else if ((newCost < CostToThisNode[curEdge.To]) &&
                         (ShortestPathTree[curEdge.To] == null))
                {
                    CostToThisNode[curEdge.To] = newCost;

                    //because the cost is less than it was previously, the PQ
                    //must be re-sorted to account for this.
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
            return CostToThisNode[Target];
        }

        ///<summary>
        ///Gets the total cost to the given node
        ///</summary>
        ///<param name="nd"></param>
        ///<returns>the total cost to the given node</returns>
        public float GetCostToNode(int nd)
        {
            return CostToThisNode[nd];
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
                path.Insert(0, new PathEdge(
                                   Graph.GetNode(ShortestPathTree[nd].From).Position,
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

        private readonly List<float> _costToThisNode;
        private readonly SparseGraph _graph;
        private readonly List<NavGraphEdge> _searchFrontier;
        private readonly List<NavGraphEdge> _shortestPathTree;
        private readonly int _source;
        private IndexedPriorityQueueLow _pq;
        private int _target;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}