#region File description

//------------------------------------------------------------------------------
//GraphMinSpanningTree.cs
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
    /// given a graph and a source node you can use this class to calculate
    /// the minimum spanning tree. If no source node is specified then the 
    /// algorithm will calculate a spanning forest starting from node 1 
    ///
    /// It uses a priority first queue implementation of Prims algorithm
    ///</summary>
    public class GraphMinSpanningTree
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="source"></param>
        public GraphMinSpanningTree(SparseGraph graph, int source)
        {
            _graph = graph;

            _spanningTree = new List<NavGraphEdge>(Graph.NumNodes);
            _fringe = new List<NavGraphEdge>(Graph.NumNodes);
            _costToThisNode = new List<float>(Graph.NumNodes);

            for (int i = 0; i < CostToThisNode.Count; i++)
            {
                CostToThisNode[i] = -1;
            }

            if (source < 0)
            {
                for (int nd = 0; nd < Graph.NumNodes; ++nd)
                {
                    if (SpanningTree[nd] == null)
                    {
                        Search(nd);
                    }
                }
            }
            else
            {
                Search(source);
            }
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="graph"></param>
        public GraphMinSpanningTree(SparseGraph graph)
            : this(graph, -1)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the minimum spanning tree
        ///</summary>
        public List<NavGraphEdge> SpanningTree
        {
            get { return _spanningTree; }
        }

        ///<summary>
        ///the graph to be searched
        ///</summary>
        public SparseGraph Graph
        {
            get { return _graph; }
        }

        ///<summary>
        ///the search fringe
        ///</summary>
        public List<NavGraphEdge> Fringe
        {
            get { return _fringe; }
        }

        ///<summary>
        ///cost to this node from source node
        ///</summary>
        public List<float> CostToThisNode
        {
            get { return _costToThisNode; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        private void Search(int source)
        {
            //create a priority queue
            IndexedPriorityQueueLow pq =
                new IndexedPriorityQueueLow(CostToThisNode, Graph.NumNodes);

            //put the source node on the queue
            pq.Insert(source);

            //while the queue is not empty
            while (!pq.Empty())
            {
                //get lowest cost edge from the queue
                int best = pq.Pop();

                //move this edge from the fringe to the spanning tree
                SpanningTree[best] = Fringe[best];

                //now to test the edges attached to this node
                foreach (NavGraphEdge curEdge in Graph.Edges[best])
                {
                    float priority = curEdge.Cost;

                    if (Fringe[curEdge.To] == null)
                    {
                        CostToThisNode[curEdge.To] = priority;

                        pq.Insert(curEdge.To);

                        Fringe[curEdge.To] = curEdge;
                    }
                    else if ((priority < CostToThisNode[curEdge.To]) &&
                             (SpanningTree[curEdge.To] == null))
                    {
                        CostToThisNode[curEdge.To] = priority;

                        pq.ChangePriority(curEdge.To);

                        Fringe[curEdge.To] = curEdge;
                    }
                }
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly List<float> _costToThisNode;
        private readonly List<NavGraphEdge> _fringe;
        private readonly SparseGraph _graph;
        private readonly List<NavGraphEdge> _spanningTree;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}