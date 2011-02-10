#region File description

//------------------------------------------------------------------------------
//SparseGraph.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using System.Text;
using GarageGames.Torque.Core;
using MapContent;
using Microsoft.Xna.Framework;

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
    ///class implementing a sparse graph
    ///</summary>
    public class SparseGraph
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="isDigraph"></param>
        public SparseGraph(bool isDigraph)
        {
            NextNodeIndex = 0;
            _isDigraph = isDigraph;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the nodes that comprise this graph
        ///</summary>
        public List<NavGraphNode> Nodes
        {
            get { return _nodes; }
        }

        ///<summary>
        ///a list of adjacency edge lists. (each node index keys into the 
        ///list of edges associated with that node)
        ///</summary>
        public List<LinkedList<NavGraphEdge>> Edges
        {
            get { return _edges; }
        }

        ///<summary>
        ///the next free node index
        ///</summary>
        public int NextFreeNodeIndex
        {
            get { return NextNodeIndex; }
        }

        ///<summary>
        ///the number of active + inactive nodes present in the graph
        ///</summary>
        public int NumNodes
        {
            get { return Nodes.Count; }
        }

        ///<summary>
        ///true if the graph is directed
        ///</summary>
        public bool IsDigraph
        {
            get { return _isDigraph; }
        }

        ///<summary>
        ///true if the graph contains no nodes
        ///</summary>
        public bool IsEmpty
        {
            get { return Nodes.Count == 0; }
        }

        ///<summary>
        ///index of next node to be added
        ///</summary>
        public int NextNodeIndex
        {
            get { return _nextNodeIndex; }
            set { _nextNodeIndex = value; }
        }

        #region Public methods

        ///<summary>
        ///Determines the number of active nodes present in the graph (this
        ///method's performance can be improved greatly by caching the value)
        ///</summary>
        ///<returns>the number of active nodes present in the graph</returns>
        public int NumActiveNodes()
        {
            int numActiveNodes = 0;

            for (int n = 0; n < Nodes.Count; ++n)
            {
                if (!GraphNode.IsInvalidIndex(Nodes[n].Index))
                {
                    ++numActiveNodes;
                }
            }

            return numActiveNodes;
        }

        ///<summary>
        ///Determines the total number of edges present in the graph
        ///</summary>
        ///<returns>the total number of edges present in the graph</returns>
        public int NumEdges()
        {
            int numEdges = 0;

            foreach (LinkedList<NavGraphEdge> edgeList in Edges)
            {
                numEdges += edgeList.Count;
            }

            return numEdges;
        }

        ///<summary>
        ///clears the graph ready for new node insertions
        ///</summary>
        public void Clear()
        {
            NextNodeIndex = 0;
            Nodes.Clear();
            Edges.Clear();
        }

        ///<summary>
        ///Tests if a node with the given index is present in the graph
        ///</summary>
        ///<param name="nd"></param>
        ///<returns>
        ///true if a node with the given index is present in the graph
        ///</returns>
        public bool IsNodePresent(int nd)
        {
            return !GraphNode.IsInvalidIndex(Nodes[nd].Index) && (nd < Nodes.Count);
        }

        ///<summary>
        ///Tests if an edge with the given from/to is present in the graph
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns>
        ///true if an edge with the given from/to is present in the graph
        ///</returns>
        public bool IsEdgePresent(int from, int to)
        {
            if (IsNodePresent(from) && IsNodePresent(to))
            {
                foreach (NavGraphEdge edge in Edges[from])
                {
                    if (edge.To == to) return true;
                }

                return false;
            }
            return false;
        }

        ///<summary>
        ///method for obtaining a reference to a specific node
        ///</summary>
        ///<param name="index"></param>
        ///<returns>the node with the given index</returns>
        public NavGraphNode GetNode(int index)
        {
            Assert.Fatal((index < Nodes.Count) && (index >= 0),
                         "SparseGraph.GetNode: invalid index");
            if(index == GraphNode.INVALID_NODE_INDEX)
                return null;
            return Nodes[index];
        }

        ///<summary>
        ///method for obtaining a reference to a specific edge
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns>the edge between the given node indices</returns>
        public NavGraphEdge GetEdge(int from, int to)
        {
            Assert.Fatal(from < Nodes.Count && from >= 0 &&
                         !GraphNode.IsInvalidIndex(Nodes[from].Index),
                         "SparseGraph.GetEdge: invalid 'from' index");

            Assert.Fatal(to < Nodes.Count && to >= 0 &&
                         !GraphNode.IsInvalidIndex(Nodes[to].Index),
                         "SparseGraph.GetEdge: invalid 'to' index");

            foreach (NavGraphEdge edge in Edges[from])
            {
                if (edge.To == to) return edge;
            }

            Assert.Fatal(false, "SparseGraph.GetEdge: edge does not exist");
            return null;
        }

        ///<summary>
        ///Use this to add an edge to the graph. The method will ensure that the
        ///edge passed as a parameter is valid before adding it to the graph. If
        ///the graph is a digraph then a similar edge connecting the nodes in
        ///the opposite direction will be automatically added.
        ///</summary>
        ///<param name="edge"></param>
        public void AddEdge(NavGraphEdge edge)
        {
            //first make sure the from and to nodes exist within the graph 
            Assert.Fatal((edge.From < NextNodeIndex) &&
                         (edge.To < NextNodeIndex),
                         "SparseGraph.AddEdge: invalid node index");

            //make sure both nodes are active before adding the edge
            if (GraphNode.IsInvalidIndex(Nodes[edge.To].Index) ||
                GraphNode.IsInvalidIndex(Nodes[edge.From].Index))
                return;

            //add the edge, first making sure it is unique
            if (UniqueEdge(edge.From, edge.To))
            {
                Edges[edge.From].AddLast(edge);
            }

            //if the graph is undirected we must add another connection
            //in the opposite direction
            if (IsDigraph)
                return;

            //check to make sure the edge is unique before adding
            if (!UniqueEdge(edge.To, edge.From))
                return;

            NavGraphEdge newEdge = edge;
            newEdge.To = edge.From;
            newEdge.From = edge.To;
            Edges[edge.To].AddLast(newEdge);
        }

        ///<summary>
        ///remove the edge(s) between the given node indices
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        public void RemoveEdge(int from, int to)
        {
            Assert.Fatal((from < Nodes.Count) && (to < Nodes.Count),
                         "SparseGraph.RemoveEdge: invalid node index");

            if (!IsDigraph)
            {
                foreach (NavGraphEdge curEdge in Edges[to])
                {
                    if (curEdge.To != from)
                        continue;

                    Edges[to].Remove(curEdge);
                    break;
                }
            }

            foreach (NavGraphEdge curEdge in Edges[from])
            {
                if (curEdge.To != to)
                    continue;

                Edges[from].Remove(curEdge);
                break;
            }
        }

        ///<summary>
        ///Given a node this method first checks to see if the node has been
        ///added previously but is now inactive. If it is, it is reactivated.
        ///
        ///If the node has not been added previously, it is checked to make
        ///sure its index matches the next node index before being added to
        ///the graph
        ///</summary>
        ///<param name="node"></param>
        ///<returns></returns>
        public int AddNode(NavGraphNode node)
        {
            if (node.Index < Nodes.Count)
            {
                //make sure the client is not trying to add a node with the same
                //Id as a currently active node
                Assert.Fatal(GraphNode.IsInvalidIndex(Nodes[node.Index].Index),
                             "SparseGraph.AddNode: Attempting to add a node with a duplicate Id");

                Nodes[node.Index] = node;

                return NextNodeIndex;
            }

            //make sure the new node has been indexed correctly
            Assert.Fatal(node.Index == NextNodeIndex,
                         "SparseGraph.AddNode: invalid index");

            Nodes.Add(node);
            Edges.Add(new LinkedList<NavGraphEdge>());

            return NextNodeIndex++;
        }

        ///<summary>
        ///iterates through all the edges in the graph and removes any that
        ///point to an invalidated node
        ///</summary>
        public void CullInvalidEdges()
        {
            foreach (LinkedList<NavGraphEdge> curEdgeList in Edges)
            {
                foreach (NavGraphEdge curEdge in curEdgeList)
                {
                    if (GraphNode.IsInvalidIndex(Nodes[curEdge.To].Index) ||
                        GraphNode.IsInvalidIndex(Nodes[curEdge.From].Index))
                    {
                        curEdgeList.Remove(curEdge);
                    }
                }
            }
        }

        ///<summary>
        ///Removes a node from the graph and removes any links to neighboring
        ///nodes
        ///</summary>
        ///<param name="node"></param>
        public void RemoveNode(int node)
        {
            Assert.Fatal(node < Nodes.Count,
                         "SparseGraph.RemoveNode: invalid node index");

            //set this node's index to invalid_node_index
            Nodes[node].Index = GraphNode.INVALID_NODE_INDEX;

            //if the graph is not directed remove all edges leading to this node
            //and then clear the edges leading from the node
            if (!IsDigraph)
            {
                //visit each neighbor and erase any edges leading to this node
                foreach (NavGraphEdge curEdge in Edges[node])
                {
                    foreach (NavGraphEdge curE in Edges[curEdge.To])
                    {
                        if (curE.To != node)
                            continue;

                        Edges[curEdge.To].Remove(curE);
                        break;
                    }
                }

                //finally, clear this node's edges
                Edges[node].Clear();
            }

                //if a digraph, remove the edges the slow way
            else
            {
                CullInvalidEdges();
            }
        }

        ///<summary>
        ///Sets the cost of a specific edge
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="newCost"></param>
        public void SetEdgeCost(int from, int to, float newCost)
        {
            //make sure the nodes given are valid
            Assert.Fatal((from < Nodes.Count) && (to < Nodes.Count),
                         "SparseGraph.SetEdgeCost: invalid index");

            //visit each neighbor and erase any edges leading to this node
            foreach (NavGraphEdge curEdge in Edges[from])
            {
                if (curEdge.To != to)
                    continue;

                curEdge.Cost = newCost;
                break;
            }
        }

        ///<summary>
        ///Test if the edge is not present in the graph. Used when adding edges
        ///to prevent duplication
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns>
        ///true if the edge is not present in the graph. 
        ///</returns>
        public bool UniqueEdge(int from, int to)
        {
            foreach (NavGraphEdge curEdge in Edges[from])
            {
                if (curEdge.To == to)
                {
                    return false;
                }
            }

            return true;
        }

        ///<summary>
        ///load graph from map data
        ///</summary>
        ///<param name="mapData"></param>
        ///<returns>true if successful</returns>
        public bool Load(MapData mapData)
        {
            Clear();

            //get the number of nodes and read them in
            int numNodes = mapData.NodeList.Count;

            for (int n = 0; n < numNodes; ++n)
            {
                NavGraphNode newNode = new NavGraphNode(mapData.NodeList[n]);

                //when editing graphs (with Buckland's Raven map editor), it's
                //possible to end up with a situation where some of the nodes
                //have been invalidated (their id's set to invalid_node_index).
                //Therefore when a node of index invalid_node_index is
                //encountered, it must still be added.
                if (!GraphNode.IsInvalidIndex(newNode.Index))
                {
                    AddNode(newNode);
                }
                else
                {
                    Nodes.Add(newNode);

                    //make sure an edgelist is added for each node
                    Edges.Add(new LinkedList<NavGraphEdge>());

                    ++NextNodeIndex;
                }
            }

            //now add the edges
            int numEdges = mapData.EdgeList.Count;

            for (int e = 0; e < numEdges; ++e)
            {
                NavGraphEdge nextEdge = new NavGraphEdge(mapData.EdgeList[e]);

                Edges[nextEdge.From].AddLast(nextEdge);
            }

            return true;
        }

        ///<summary>
        ///Generate string representation of graph
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Navigation Graph:");
            sb.AppendLine();

            sb.Append("Nodes:");
            foreach (NavGraphNode curNode in Nodes)
            {
                sb.AppendLine();
                sb.Append(curNode.ToString());
            }
            sb.AppendLine();

            sb.Append("Edges:");
            int nodeIndex = 0;
            foreach (LinkedList<NavGraphEdge> edgeList in Edges)
            {
                sb.AppendLine();
                sb.AppendFormat("NodeIndex: {0}", nodeIndex);
                foreach (NavGraphEdge curEdge in edgeList)
                {
                    sb.AppendLine();
                    sb.Append(curEdge.ToString());
                }
                nodeIndex++;
            }
            return sb.ToString();
        }


        /// <summary>
        /// finds the closest node to a certain 2d position
        /// </summary>
        /// <param name="position">position to check against</param>
        /// <returns>closest visible</returns>
        public NavGraphNode FindClosestAccessibleNodeToPosition(Vector2 position)
        {
            float closestDistance = float.MaxValue;
            NavGraphNode closestNode = null;
            foreach (NavGraphNode node in _nodes)
            {
                float distance = Vector2.Distance(node.Position, position) ;
                if(distance < closestDistance)
                { 
                    if(GameManager.GameManager.Instance.IsPathObstructed(node.Position, position, 16))
                    {
                        closestNode = node;
                        closestDistance = distance;
                    }
                }
            }
            return closestNode;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly List<LinkedList<NavGraphEdge>> _edges =
            new List<LinkedList<NavGraphEdge>>();

        private readonly bool _isDigraph;

        private readonly List<NavGraphNode> _nodes =
            new List<NavGraphNode>();

        private int _nextNodeIndex;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}