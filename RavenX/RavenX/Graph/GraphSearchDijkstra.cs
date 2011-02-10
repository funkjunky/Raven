#region File description

//------------------------------------------------------------------------------
//GraphSearchDijkstra.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Mindcrafters.Library.Utility;

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
    /// Given a graph, source and optional target this class solves for
    /// single source shortest paths (without a target being specified) or 
    /// shortest path from source to target.
    ///
    /// The algorithm used is a priority queue implementation of Dijkstra's.
    /// note how similar this is to the algorithm used in Graph_MinSpanningTree.
    /// The main difference is in the calculation of the priority in the line:
    /// 
    /// float newCost = _costToThisNode[best] + curEdge.Cost;
    ///</summary>
    public class GraphSearchDijkstra
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
        public GraphSearchDijkstra(SparseGraph graph, int source, int target)
        {
            _graph = graph;
            _source = source;
            _target = target;
            _shortestPathTree =
                new List<NavGraphEdge>(Graph.NumNodes);
            for (int i = 0; i < Graph.NumNodes; i++)
                ShortestPathTree.Add(null);
            _searchFrontier =
                new List<NavGraphEdge>(Graph.NumNodes);
            for (int i = 0; i < Graph.NumNodes; i++)
                SearchFrontier.Add(null);
            _costToThisNode =
                new List<float>(Graph.NumNodes);
            for (int i = 0; i < Graph.NumNodes; i++)
                CostToThisNode.Add(0);
            Search();
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="source"></param>
        public GraphSearchDijkstra(SparseGraph graph, int source)
            : this(graph, source, -1)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the list of edges that defines the SPT. If a target was given in the
        ///constructor then this will be an SPT comprising of all the nodes
        ///examined before the target was found, else it will contain all the
        ///nodes in the graph.
        ///</summary>
        public List<NavGraphEdge> SpanningTree
        {
            get { return ShortestPathTree; }
        }

        ///<summary>
        ///the graph to be searched
        ///</summary>
        public SparseGraph Graph
        {
            get { return _graph; }
        }

        ///<summary>
        ///this list contains the edges that comprise the shortest path tree -
        ///a directed subtree of the graph that encapsulates the best paths from 
        ///every node on the SPT to the source node.
        ///</summary>
        public List<NavGraphEdge> ShortestPathTree
        {
            get { return _shortestPathTree; }
        }

        ///<summary>
        ///Gets a list of costs to this node
        ///</summary>
        ///<returns>
        ///a list of costs to this node
        ///</returns>
        public List<float> CostToThisNode
        {
            get { return _costToThisNode; }
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
        ///the target node index
        ///</summary>
        public int Target
        {
            get { return _target; }
        }

        #region Public methods

        ///<summary>
        ///Gets the total cost to the target
        ///</summary>
        ///<returns>the total cost to the target</returns>
        public float GetCostToTarget()
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
        ///Gets shortest path to target
        ///</summary>
        ///<returns></returns>
        public List<int> GetPathToTarget()
        {
            List<int> path = new List<int>();

            //just return an empty path if no target or no path found
            if (Target < 0) return path;

            int nd = Target;

            //TODO: maybe replace Insert(0, with Add( and reverse path before returning??
            path.Insert(0, nd);

            while ((nd != Source) && (ShortestPathTree[nd] != null))
            {
                nd = ShortestPathTree[nd].From;

                path.Insert(0, nd);
            }

            return path;
        }

        #endregion

        #region Private, protected, internal methods

        private void Search()
        {
            //create an indexed priority queue that sorts smallest to largest
            //(front to back). Note that the maximum number of elements the
            //priority queue may contain is N. This is because no node can be
            //represented on the queue more than once.
            IndexedPriorityQueueLow pq =
                new IndexedPriorityQueueLow(CostToThisNode, Graph.NumNodes);

            //put the source node on the queue
            pq.Insert(Source);

            //while the queue is not empty
            while (!pq.Empty())
            {
                //get lowest cost node from the queue. Don't forget, the return
                //value /is a *node index*, not the node itself. This node is
                //the node not already on the SPT that is the closest to the
                //source node
                int nextClosestNode = pq.Pop();

                //move this edge from the frontier to the shortest path tree
                ShortestPathTree[nextClosestNode] =
                    SearchFrontier[nextClosestNode];

                //if the target has been found exit
                if (nextClosestNode == Target) return;

                //now to relax the edges.
                foreach (NavGraphEdge curEdge in Graph.Edges[nextClosestNode])
                {
                    //the total cost to the node this edge points to is the cost
                    //to the current node plus the cost of the edge connecting
                    //them.
                    float newCost =
                        CostToThisNode[nextClosestNode] + curEdge.Cost;

                    //if this edge has never been on the frontier make a note
                    //of the cost to get to the node it points to, then add the
                    //edge to the frontier and the destination node to the PQ.
                    if (SearchFrontier[curEdge.To] == null)
                    {
                        CostToThisNode[curEdge.To] = newCost;
                        pq.Insert(curEdge.To);
                        SearchFrontier[curEdge.To] = curEdge;
                    }

                        //else test to see if the cost to reach the destination node
                        //via the current node is cheaper than the cheapest cost
                        //found so far. If this path is cheaper, we assign the new
                        //cost to the destination node, update its entry in the PQ
                        //to reflect the change and add the edge to the frontier
                    else if ((newCost < CostToThisNode[curEdge.To]) &&
                             (ShortestPathTree[curEdge.To] == null))
                    {
                        CostToThisNode[curEdge.To] = newCost;

                        //because the cost is less than it was previously,
                        //the PQ must be re-sorted to account for this.
                        pq.ChangePriority(curEdge.To);
                        SearchFrontier[curEdge.To] = curEdge;
                    }
                }
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly List<float> _costToThisNode;
        private readonly SparseGraph _graph;
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