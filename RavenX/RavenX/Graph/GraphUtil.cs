#region File description

//------------------------------------------------------------------------------
//GraphUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Tx2D.GameAI;

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
    ///class for some handy graph functions
    ///</summary>
    public class GraphUtil
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Tests if x,y is a valid position in the map
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="numCellsX"></param>
        ///<param name="numCellsY"></param>
        ///<returns>true if x,y is a valid position in the map</returns>
        public static bool ValidNeighbor(
            int x,
            int y,
            int numCellsX,
            int numCellsY)
        {
            return !((x < 0) || (x >= numCellsX) || (y < 0) || (y >= numCellsY));
        }

        ///<summary>
        ///use to add the eight neighboring edges of a graph node that 
        ///is positioned in a grid layout
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="row"></param>
        ///<param name="col"></param>
        ///<param name="numCellsX"></param>
        ///<param name="numCellsY"></param>
        public static void AddAllNeighborsToGridNode(
            SparseGraph graph,
            int row,
            int col,
            int numCellsX,
            int numCellsY)
        {
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    int nodeX = col + j;
                    int nodeY = row + i;

                    //skip if equal to this node
                    if ((i == 0) && (j == 0)) continue;

                    //check to see if this is a valid neighbor
                    if (!ValidNeighbor(nodeX, nodeY, numCellsX, numCellsY))
                        continue;

                    //calculate the distance to this node
                    Vector2 posNode =
                        graph.GetNode(row*numCellsX + col).Position;
                    Vector2 posNeighbour =
                        graph.GetNode(nodeY*numCellsX + nodeX).Position;

                    float dist = (posNode - posNeighbour).Length();

                    //this neighbor is okay so it can be added
                    NavGraphEdge newEdge =
                        new NavGraphEdge(
                            row*numCellsX + col,
                            nodeY*numCellsX + nodeX,
                            dist);
                    graph.AddEdge(newEdge);

                    //if graph is not a digraph, an edge needs to be added going
                    //in the other direction
                    if (graph.IsDigraph)
                        continue;

                    NavGraphEdge newReverseEdge =
                        new NavGraphEdge(
                            nodeY*numCellsX + nodeX,
                            row*numCellsX + col,
                            dist);
                    graph.AddEdge(newReverseEdge);
                }
            }
        }

        ///<summary>
        ///creates a graph based on a grid layout. This function requires the 
        ///dimensions of the environment and the number of cells required
        ///horizontally and vertically 
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="cySize"></param>
        ///<param name="cxSize"></param>
        ///<param name="numCellsY"></param>
        ///<param name="numCellsX"></param>
        public static void CreateGrid(
            SparseGraph graph,
            int cySize,
            int cxSize,
            int numCellsY,
            int numCellsX)
        {
            //need some temporaries to help calculate each node center
            float cellWidth = (float) cySize/numCellsX;
            float cellHeight = (float) cxSize/numCellsY;

            float midX = cellWidth/2;
            float midY = cellHeight/2;


            //first create all the nodes
            for (int row = 0; row < numCellsY; ++row)
            {
                for (int col = 0; col < numCellsX; ++col)
                {
                    graph.AddNode(
                        new NavGraphNode(
                            graph.NextFreeNodeIndex,
                            new Vector2(
                                midX + (col*cellWidth),
                                midY + (row*cellHeight))));
                }
            }
            //now to calculate the edges. (A position in a 2D array [x][y] is
            //the same as [y*NumCellsX + x] in a 1d array). Each cell has up to
            //eight neigbors.
            for (int row = 0; row < numCellsY; ++row)
            {
                for (int col = 0; col < numCellsX; ++col)
                {
                    AddAllNeighborsToGridNode(
                        graph,
                        row,
                        col,
                        numCellsX,
                        numCellsY);
                }
            }
        }

        ///<summary>
        ///draws the given graph
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="color"></param>
        ///<param name="drawNodeIds"></param>
        public static void Draw(
            SparseGraph graph, Color color, bool drawNodeIds)
        {
            //just return if the graph has no nodes
            if (graph.NumNodes == 0) return;

            //draw the nodes 
            foreach (NavGraphNode curNode in graph.Nodes)
            {
                if (GraphNode.IsInvalidIndex(curNode.Index))
                    continue;

                DrawUtil.Circle(curNode.Position, 2, color, 20);

                if (drawNodeIds)
                {
                    TextUtil.DrawText(
                        @"data\fonts\Arial6", //TODO: should be a parameter
                        new Vector2(
                            curNode.Position.X + 5,
                            curNode.Position.Y - 5),
                        new Color(200, 200, 200),
                        curNode.Index.ToString());
                }

                foreach (NavGraphEdge curEdge in graph.Edges[curNode.Index])
                {
                    DrawUtil.Line(
                        curNode.Position,
                        graph.GetNode(curEdge.To).Position,
                        color);
                }
            }
        }

        ///<summary>
        ///draws the given graph
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="color"></param>
        public static void Draw(SparseGraph graph, Color color)
        {
            Draw(graph, color, false);
        }

        ///<summary>
        ///Given a cost value and an index to a valid node this function
        ///examines all a node's edges, calculates their length, and
        ///multiplies the value with the weight. Useful for setting terrain
        ///costs.
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="node"></param>
        ///<param name="weight"></param>
        public static void WeightNavGraphNodeEdges(
            SparseGraph graph,
            int node,
            float weight)
        {
            //make sure the node is present
            Assert.Fatal(node < graph.NumNodes,
                         "GraphUtil.WeightNavGraphNodeEdges: node index out of range");

            //set the cost for each edge
            foreach (NavGraphEdge curEdge in graph.Edges[node])
            {
                //calculate the distance between nodes
                float dist = (graph.GetNode(curEdge.From).Position -
                              graph.GetNode(curEdge.To).Position).Length();

                //set the cost of this edge
                graph.SetEdgeCost(curEdge.From, curEdge.To, dist*weight);

                //if not a digraph, set the cost of the parallel edge to be
                //the same
                if (!graph.IsDigraph)
                {
                    graph.SetEdgeCost(curEdge.To, curEdge.From, dist*weight);
                }
            }
        }

        ///<summary>
        ///creates a lookup table encoding the shortest path info between each
        ///node in a graph to every other
        ///</summary>
        ///<param name="graph"></param>
        ///<returns></returns>
        public static List<List<int>> CreateAllPairsTable(SparseGraph graph)
        {
            const int noPath = -1;

            List<int> row = new List<int>(graph.NumNodes);
            for (int i = 0; i < row.Count; i++)
            {
                row[i] = noPath;
            }

            List<List<int>> shortestPaths =
                new List<List<int>>(graph.NumNodes);
            for (int i = 0; i < shortestPaths.Count; i++)
            {
                shortestPaths[i] = new List<int>(row);
            }

            for (int source = 0; source < graph.NumNodes; ++source)
            {
                //calculate the SPT for this node
                GraphSearchDijkstra search =
                    new GraphSearchDijkstra(graph, source);

                List<NavGraphEdge> spt = search.SpanningTree;

                //now we have the SPT it's easy to work backwards through it to
                //find the shortest paths from each node to this source node
                for (int target = 0; target < graph.NumNodes; ++target)
                {
                    //if the source node is the same as the target just set to
                    //target
                    if (source == target)
                    {
                        shortestPaths[source][target] = target;
                    }

                    else
                    {
                        int nd = target;
                        while ((nd != source) && (spt[nd] != null))
                        {
                            shortestPaths[spt[nd].From][target] = nd;
                            nd = spt[nd].From;
                        }
                    }
                }
            }

            return shortestPaths;
        }

        ///<summary>
        ///creates a lookup table of the cost associated from traveling from one
        ///node to every other
        ///</summary>
        ///<param name="graph"></param>
        ///<returns></returns>
        public static List<List<float>> CreateAllPairsCostsTable(
            SparseGraph graph)
        {
            //create a two dimensional vector
            List<List<float>> pathCosts =
                new List<List<float>>(graph.NumNodes);
            for (int i = 0; i < graph.NumNodes; i++)
            {
                pathCosts.Add(new List<float>(graph.NumNodes));
                for (int j = 0; j < graph.NumNodes; j++)
                {
                    pathCosts[i].Add(0);
                }
            }

            for (int source = 0; source < graph.NumNodes; ++source)
            {
                //do the search
                GraphSearchDijkstra search =
                    new GraphSearchDijkstra(graph, source);

                //iterate through every node in the graph and grab the cost to
                //travel to that node
                for (int target = 0; target < graph.NumNodes; ++target)
                {
                    if (source != target)
                    {
                        pathCosts[source][target] =
                            search.GetCostToNode(target);
                    }
                }
            }

            return pathCosts;
        }

        //---------------------- CalculateAverageGraphEdgeLength ----------------------
        //
        //
        //------------------------------------------------------------------------------
        ///<summary>
        ///determines the average length of the edges in a navgraph (using the 
        ///distance between the source and target node positions (not the cost
        ///of the edge as represented in the graph, which may account for all
        ///sorts of other factors such as terrain type, gradients etc)
        ///</summary>
        ///<param name="graph"></param>
        ///<returns></returns>
        public static float CalculateAverageGraphEdgeLength(SparseGraph graph)
        {
            float totalLength = 0;
            int numEdgesCounted = 0;

            foreach (NavGraphNode curNode in graph.Nodes)
            {
                if (GraphNode.IsInvalidIndex(curNode.Index))
                    continue;

                foreach (NavGraphEdge curEdge in graph.Edges[curNode.Index])
                {
                    //increment edge counter
                    ++numEdgesCounted;

                    //add length of edge to total length
                    totalLength +=
                        (graph.GetNode(curEdge.From).Position -
                         graph.GetNode(curEdge.To).Position).Length();
                }
            }

            return totalLength/numEdgesCounted;
        }

        ///<summary>
        ///Get the cost of the costliest edge in the graph
        ///</summary>
        ///<param name="graph"></param>
        ///<returns>the cost of the costliest edge in the graph</returns>
        public static float GetCostliestGraphEdge(SparseGraph graph)
        {
            float greatest = Single.MinValue;

            foreach (NavGraphNode curNode in graph.Nodes)
            {
                if (GraphNode.IsInvalidIndex(curNode.Index))
                    continue;

                foreach (NavGraphEdge curEdge in graph.Edges[curNode.Index])
                {
                    if (curEdge.Cost > greatest)
                    {
                        greatest = curEdge.Cost;
                    }
                }
            }

            return greatest;
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