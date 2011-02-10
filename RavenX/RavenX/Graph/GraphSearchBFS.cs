#region File description

//------------------------------------------------------------------------------
//GraphSearchBFS.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

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
    ///breadth first search class
    ///</summary>
    public class GraphSearchBFS
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
        public GraphSearchBFS(SparseGraph graph, int source, int target)
        {
            _graph = graph;
            _source = source;
            _target = target;
            _isFound = false;
            _visited = new List<int>(Graph.NumNodes);
            for (int i = 0; i < Visited.Count; i++)
            {
                Visited[i] = (int) SearchStatus.Unvisited;
            }
            _route = new List<int>(Graph.NumNodes);
            for (int i = 0; i < Route.Count; i++)
            {
                Route[i] = (int) SearchStatus.NoParentAssigned;
            }
            _isFound = Search();
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="source"></param>
        public GraphSearchBFS(SparseGraph graph, int source)
            : this(graph, source, GraphNode.INVALID_NODE_INDEX)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///a list containing of all the edges the search has examined
        ///</summary>
        public List<NavGraphEdge> SearchTree
        {
            get { return SpanningTree; }
        }

        ///<summary>
        ///true if the target node has been located
        ///</summary>
        public bool IsFound
        {
            get { return _isFound; }
        }

        ///<summary>
        ///the graph to be searched
        ///</summary>
        public SparseGraph Graph
        {
            get { return _graph; }
        }

        ///<summary>
        ///this records the indexes of all the nodes that are visited as the
        //search progresses
        ///</summary>
        public List<int> Visited
        {
            get { return _visited; }
        }

        ///<summary>
        ///this holds the route taken to the target. Given a node index, the
        ///value at that index is the node's parent. i.e., if the path to the
        ///target is 3-8-27, Route[8] will hold 3 and Route[27] will hold 8.
        ///</summary>
        public List<int> Route
        {
            get { return _route; }
        }

        ///<summary>
        ///As the search progresses, this will hold all the edges the algorithm
        ///has examined. THIS IS NOT NECESSARY FOR THE SEARCH, IT IS HERE PURELY
        ///TO PROVIDE THE USER WITH SOME VISUAL FEEDBACK
        ///</summary>
        public List<NavGraphEdge> SpanningTree
        {
            get { return _spanningTree; }
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
        ///Gets a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</summary>
        ///<returns>
        ///a list of node indexes that comprise the shortest path from the
        ///source to the target
        ///</returns>
        public List<int> GetPathToTarget()
        {
            List<int> path = new List<int>();

            //just return an empty path if no path to target found or if
            //no target has been specified
            if (!IsFound || Target < 0)
                return path;

            int nd = Target;

            path.Insert(0, nd); //TODO: push_front?

            while (nd != Source)
            {
                nd = Route[nd];

                path.Insert(0, nd);
            }

            return path;
        }

        #endregion

        #region Private, protected, internal methods

        private bool Search()
        {
            //create a queue of edges
            Queue<NavGraphEdge> queue = new Queue<NavGraphEdge>();

            //create a dummy edge and put on the queue
            NavGraphEdge dummy = new NavGraphEdge(Source, Source, 0);

            queue.Enqueue(dummy);

            //mark the source node as visited
            Visited[Source] = (int) SearchStatus.Visited;

            //while there are edges in the queue keep searching
            while (queue.Count > 0)
            {
                //grab the next edge
                NavGraphEdge next = queue.Peek();

                //remove the edge from the queue
                queue.Dequeue();

                //make a note of the parent of the node this edge points to
                Route[next.To] = next.From;

                //put it on the tree.
                //(making sure the dummy edge is not placed on the tree)
                if (next != dummy)
                {
                    SpanningTree.Add(next);
                }

                //if the target has been found the method can return success
                if (next.To == Target)
                {
                    return true;
                }

                //push the edges leading from the node at the end of this edge 
                //onto the queue
                foreach (NavGraphEdge curEdge in Graph.Edges[next.To])
                {
                    if (Visited[curEdge.To] != (int) SearchStatus.Unvisited)
                        continue;

                    //if the node hasn't already been visited we can push
                    //the edge onto the queue
                    queue.Enqueue(curEdge);

                    //and mark it visited
                    Visited[curEdge.To] = (int) SearchStatus.Visited;
                }
            }

            //no path to target
            return false;
        }

        ///<summary>
        ///to aid readability
        ///</summary>
        private enum SearchStatus
        {
            Visited,
            Unvisited,
            NoParentAssigned
        } ;

        #endregion

        #region Private, protected, internal fields

        private readonly SparseGraph _graph;
        private readonly bool _isFound;
        private readonly List<int> _route;

        private readonly int _source;

        private readonly List<NavGraphEdge> _spanningTree =
            new List<NavGraphEdge>();

        private readonly int _target;
        private readonly List<int> _visited;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}