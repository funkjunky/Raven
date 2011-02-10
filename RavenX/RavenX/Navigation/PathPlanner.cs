#region File description

//------------------------------------------------------------------------------
//PathPlanner.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Graph;
using Mindcrafters.RavenX.Messaging;

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
    ///class to handle the creation of paths through a navigation graph
    ///</summary>
    public class PathPlanner
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        public PathPlanner(BotEntity owner)
        {
            _owner = owner;
            _navGraph = GameManager.GameManager.Instance.Map.NavGraph;
            CurrentSearch = null;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        private const int NO_CLOSEST_NODE_FOUND = -1;

        ///<summary>
        ///position the bot wishes to plan a path to reach
        ///</summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        ///<summary>
        ///the owner of this instance
        ///</summary>
        public BotEntity Owner
        {
            get { return _owner; }
        }

        ///<summary>
        ///the navigation graph
        ///</summary>
        public SparseGraph NavGraph
        {
            get { return _navGraph; }
        }

        ///<summary>
        ///an instance of the current graph search algorithm.
        ///</summary>
        public GraphSearchTimeSliced CurrentSearch
        {
            get { return _currentSearch; }
            set { _currentSearch = value; }
        }

        #region Public methods

        ///<summary>
        ///called by the path manager when a search has been terminated to free
        ///up the memory used when an instance of the search was created
        ///</summary>
        public void GetReadyForNewSearch()
        {
            //unregister any existing search with the path manager
            GameManager.GameManager.Instance.PathManager.UnRegister(this);

            //clean up memory used by any existing search  
            CurrentSearch = null;
        }

        ///<summary>
        ///Gets the cost to travel from the bot's current position to a specific
        ///graph node. This method makes use of the pre-calculated lookup table
        ///</summary>
        ///<param name="nodeIdx"></param>
        ///<returns>
        ///the cost to travel from the bot's current position to a specific
        ///graph node.
        ///</returns>
        public float GetCostToNode(int nodeIdx)
        {
            //find the closest visible node to the bots position
            int nd = GetClosestNodeToPosition(Owner.Position);

            //add the cost to this node
            float cost =
                (Owner.Position - NavGraph.GetNode(nd).Position).Length();

            //add the cost to the target node and return
            return cost +
                   GameManager.GameManager.Instance.Map.CalculateCostBetweenNodes(nd, nodeIdx);
        }

        ///<summary>
        ///Gets the cost to the closest instance of the giver type. This method
        ///makes use of the pre-calculated lookup table.
        ///Returns NO_CLOSEST_NODE_FOUND if no active trigger found
        ///</summary>
        ///<param name="giverType"></param>
        ///<returns>
        ///the cost to the closest instance of the giver type.
        ///</returns>
        public float GetCostToClosestItem(ItemTypes giverType)
        {
            //find the closest visible node to the bots position
            int nd = GetClosestNodeToPosition(Owner.Position);

            //if no closest node found return failure
            if (GraphNode.IsInvalidIndex(nd))
                return NO_CLOSEST_NODE_FOUND;

            float closestSoFar = Single.MaxValue;

            //iterate through all the triggers to find the closest *active*
            //trigger of type giverType
            foreach (Trigger.Trigger trigger in Owner.FoundTriggers.List)
            {
                if (!trigger.IsActive ||
                    trigger.EntityType != Entity.Entity.ItemTypeToEntityType(giverType))
                    continue;
                float cost =
                    GameManager.GameManager.Instance.Map.CalculateCostBetweenNodes(
                        nd,
                        trigger.NodeIndex);

                if (cost < closestSoFar)
                {
                    closestSoFar = cost;
                }
            }

            //return a negative value if no active trigger of the type found
            return Epsilon.IsEqual(closestSoFar, Single.MaxValue)
                       ?
                           NO_CLOSEST_NODE_FOUND
                       : closestSoFar;
        }

        ///<summary>
        ///called by an agent after it has been notified that a search has
        ///terminated successfully. The method extracts the path from
        ///<see cref="_currentSearch"/>, adds additional edges appropriate to
        ///the search type and returns it as a list of PathEdges.
        ///</summary>
        ///<returns>a list of PathEdges</returns>
        public List<PathEdge> GetPath()
        {
            Assert.Fatal(CurrentSearch != null,
                         "PathPlanner.GetPathAsNodes: no current search");

            List<PathEdge> path = CurrentSearch.GetPathAsPathEdges();

            int closest = GetClosestNodeToPosition(Owner.Position);

            //temp: to allow debug stepping through 
            if (closest == NO_CLOSEST_NODE_FOUND) //how can this happen?
            {
                //TODO: this happens if bot is too close to wall
                //maybe a feeler bug???
                closest = GetClosestNodeToPosition(Owner.Position);
            }

            path.Insert(0, new PathEdge(Owner.Position,
                                        GetNodePosition(closest),
                                        EdgeData.BehaviorTypes.Normal));

            //if the bot requested a path to a location then an edge leading to
            //the destination must be added
            if (CurrentSearch.GetType() == typeof (GraphSearchAStarTimeSliced))
            {
                path.Add(new PathEdge(path[path.Count - 1].Destination,
                                      Destination,
                                      EdgeData.BehaviorTypes.Normal));
            }

            //smooth paths if required
            if (GameManager.GameManager.Instance.Options.SmoothPathsQuick)
            {
                SmoothPathEdgesQuick(path);
            }

            if (GameManager.GameManager.Instance.Options.SmoothPathsPrecise)
            {
                SmoothPathEdgesPrecise(path);
            }

            return path;
        }

        ///<summary>
        ///smooths a path by removing extraneous edges.
        ///</summary>
        ///<param name="path"></param>
        public void SmoothPathEdgesQuick(List<PathEdge> path)
        {
            int e1 = 0;
            int e2 = e1 + 1; //e2 points to the edge following e1.

            //while e2 is not the last edge in the path, step through the edges
            //checking to see if the agent can move without obstruction from the
            //source node of e1 to the destination node of e2. If the agent can
            //move between those positions then the two edges are replaced with
            //a single edge.
            while (e2 < path.Count)
            {
                //check for obstruction, adjust and remove the edges accordingly
                if ((path[e2].Behavior == EdgeData.BehaviorTypes.Normal) &&
                    Owner.CanWalkBetween(path[e1].Source, path[e2].Destination))
                {
                    path[e1].Destination = path[e2].Destination;
                    path.RemoveAt(e2);
                }
                else
                {
                    e1 = e2;
                    ++e2;
                }
            }
        }

        ///<summary>
        ///smooths a path by removing extraneous edges.
        ///</summary>
        ///<param name="path"></param>
        public void SmoothPathEdgesPrecise(List<PathEdge> path)
        {
            //create a couple of iterators
            int e1 = 0;

            while (e1 < path.Count)
            {
                //point e2 to the edge immediately following e1
                int e2 = e1 + 1;

                //while e2 is not the last edge in the path, step through the
                //edges checking to see if the agent can move without
                //obstruction from the source node of e1 to the destination node
                //of e2. If the agent can move between those positions then the
                //any edges between e1 and e2 are replaced with a single edge.
                while (e2 < path.Count)
                {
                    //check for obstruction, adjust and remove the edges
                    //accordingly
                    if ((path[e2].Behavior == EdgeData.BehaviorTypes.Normal) &&
                        Owner.CanWalkBetween(
                            path[e1].Source,
                            path[e2].Destination))
                    {
                        path[e1].Destination = path[e2].Destination;
                        path.RemoveRange(e1 + 1, e2 - e1);
                        e2 = e1 + 1;
                    }
                    else
                    {
                        ++e2;
                    }
                }
                ++e1;
            }
        }

        ///<summary>
        ///the path manager calls this to iterate once though the search cycle
        ///of the currently assigned search algorithm.
        ///</summary>
        ///<returns></returns>
        public int CycleOnce()
        {
            Assert.Fatal(CurrentSearch != null,
                         "PathPlanner.CycleOnce: No search object instantiated");

            int result = CurrentSearch.CycleOnce();

            //let the bot know of the failure to find a path
            if (result == (int) GraphSearchTimeSliced.SearchResults.TargetNotFound)
            {
                MessageDispatcher.Instance.DispatchMsg(
                    MessageDispatcher.SEND_MSG_IMMEDIATELY,
                    MessageDispatcher.SENDER_ID_IRRELEVANT,
                    Owner.ObjectId,
                    MessageTypes.NoPathAvailable,
                    MessageDispatcher.NO_ADDITIONAL_INFO);
            }
            else if (result == (int) GraphSearchTimeSliced.SearchResults.TargetFound)
            {
                //if the search was for an item type then the final node in the
                //path will represent a giver trigger. Consequently, it's worth
                //passing the reference to the trigger in the extra info field
                //of the message. (The reference will be null if no trigger)
                List<int> pathToTarget = CurrentSearch.GetPathToTarget();
                Trigger.Trigger trigger =
                    NavGraph.GetNode(
                        pathToTarget[pathToTarget.Count - 1]).ExtraInfo
                    as Trigger.Trigger;

                MessageDispatcher.Instance.DispatchMsg(
                    MessageDispatcher.SEND_MSG_IMMEDIATELY,
                    MessageDispatcher.SENDER_ID_IRRELEVANT,
                    Owner.ObjectId,
                    MessageTypes.PathReady,
                    trigger);
            }

            return result;
        }

        ///<summary>
        ///Gets the index of the closest visible graph node to the given
        ///position
        ///</summary>
        ///<param name="pos"></param>
        ///<returns>
        ///the index of the closest visible graph node to the given position
        ///</returns>
        public int GetClosestNodeToPosition(Vector2 pos)
        {
            float closestSoFar = Single.MaxValue;
            int closestNode = NO_CLOSEST_NODE_FOUND;

            //when the cell space is queried this the the range searched for
            //neighboring graph nodes. This value is inversely proportional to
            //the density of a navigation graph (less dense = bigger values)
            float range = GameManager.GameManager.Instance.Map.CellSpaceNeighborhoodRange;

            //calculate the graph nodes that are neighboring this position
            GameManager.GameManager.Instance.Map.CellSpace.CalculateNeighbors(pos, range);

            //iterate through the neighbors and sum up all the position vectors
            foreach (NavGraphNode curNode in
                GameManager.GameManager.Instance.Map.CellSpace.Neighbors)
            {
                //end of Neighbors list is marked with null
                if (curNode == null)
                    break;

                //if the path between this node and pos is unobstructed
                //calculate the distance
                if (!Owner.CanWalkBetween(pos, curNode.Position))
                    continue;

                float dist = Vector2.DistanceSquared(pos, curNode.Position);

                //keep a record of the closest so far
                if (dist >= closestSoFar)
                    continue;

                closestSoFar = dist;
                closestNode = curNode.Index;
            }

            return closestNode;
        }

        ///<summary>
        ///Given a target, this method first determines if nodes can be reached
        ///from the bot's current position and the target position. If either
        ///end point is unreachable the method returns false. 
        ///
        ///If nodes are reachable from both positions then an instance of the
        ///time-sliced A* search is created and registered with the search
        ///manager.
        ///</summary>
        ///<param name="destination"></param>
        ///<returns>false if request unsuccessful</returns>
        public bool RequestPathToPosition(Vector2 destination, BotEntity bot)
        {
            LogUtil.WriteLineIfLogNavigation(
                String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                String.Format("{0,-23}] ", "RequestPathToPosition") +
                String.Format("[{0,-9}]", " ") + //skip status field
                " At position: " + Vector2Util.ToString(Owner.Position) +
                " Destination: " + Vector2Util.ToString(destination));

            GetReadyForNewSearch();

            //save the destination position.
            Destination = destination;

            //if the destination is walkable from the bot's position a path does
            //not need to be calculated, the bot can go straight to the position
            //by ARRIVING at the current waypoint
            if (Owner.CanWalkTo(destination))
            {
                LogUtil.WriteLineIfLogNavigation(
                    String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                    String.Format("{0,-23}] ", "RequestPathToPosition") +
                    String.Format("[{0,-9}]", " ") + //skip status field
                    " At position: " + Vector2Util.ToString(Owner.Position) +
                    " Destination: " + Vector2Util.ToString(destination) +
                    " The way is clear. Start walking.");
                return true;
            }

            //find the closest visible node to the bots position
            int closestNodeToBot = GetClosestNodeToPosition(Owner.Position);

            //remove the destination node from the list and return false if no
            //visible node found. This will occur if the navgraph is badly
            //designed or if the bot has managed to get itself *inside* the
            //geometry (surrounded by walls), or an obstacle.
            if (closestNodeToBot == NO_CLOSEST_NODE_FOUND)
            {
                LogUtil.WriteLineIfLogNavigation(
                    String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                    String.Format("{0,-23}] ", "RequestPathToPosition") +
                    String.Format("[{0,-9}]", " ") + //skip status field
                    " At position: " + Vector2Util.ToString(Owner.Position) +
                    " Destination: " + Vector2Util.ToString(destination) +
                    " No closest node to position found." +
                    " Has bot penetrated a wall?");
                return false;
            }

            LogUtil.WriteLineIfLogNavigation(
                String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                String.Format("{0,-23}] ", "RequestPathToPosition") +
                String.Format("[{0,-9}]", " ") + //skip status field
                " At position: " + Vector2Util.ToString(Owner.Position) +
                " Destination: " + Vector2Util.ToString(destination) +
                " Closest node to position is: " + closestNodeToBot);

            //find the closest visible node to the target position
            int closestNodeToDestination = GetClosestNodeToPosition(destination);

            //return false if there is a problem locating a visible node from
            //the destination. This sort of thing occurs much more frequently
            //than the above. For example, if the user clicks inside an area
            //bounded by walls or inside an object.
            if (closestNodeToDestination == NO_CLOSEST_NODE_FOUND)
            {
                LogUtil.WriteLineIfLogNavigation(
                    String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                    String.Format("{0,-23}] ", "RequestPathToPosition") +
                    String.Format("[{0,-9}]", " ") + //skip status field
                    " At position: " + Vector2Util.ToString(Owner.Position) +
                    " Destination: " + Vector2Util.ToString(destination) +
                    " No closest node to destination found." +
                    " Is destination inside an obstacle or surrounded by walls?");
                return false;
            }

            LogUtil.WriteLineIfLogNavigation(
                String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                String.Format("{0,-23}] ", "RequestPathToPosition") +
                String.Format("[{0,-9}]", " ") + //skip status field
                " At position: " + Vector2Util.ToString(Owner.Position) +
                " Destination: " + Vector2Util.ToString(destination) +
                " Closest node to destination is: " + closestNodeToDestination);

            //create an instance of a the distributed A* search class
            CurrentSearch =
                new GraphSearchAStarTimeSliced(
                    NavGraph,
                    closestNodeToBot,
                    closestNodeToDestination,
                    bot);

            //and register the search with the path manager
            GameManager.GameManager.Instance.PathManager.Register(this);

            return true;
        }

        ///<summary>
        ///Given an item type, this method determines the closest reachable
        ///graph node to the bot's position and then creates a instance of the
        ///time-sliced Dijkstra's algorithm, which it registers with the search
        ///manager
        ///</summary>
        ///<param name="itemType"></param>
        ///<returns></returns>
        public bool RequestPathToItem(ItemTypes itemType, BotEntity bot)
        {
            LogUtil.WriteLineIfLogNavigation(
                String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                String.Format("{0,-23}] ", "RequestPathToItem") +
                String.Format("[{0,-9}]", " ") + //skip status field
                " At position: " + Vector2Util.ToString(Owner.Position) +
                " Item type: " + EnumUtil.GetDescription(itemType));

            //clear the waypoint list and delete any active search
            GetReadyForNewSearch();

            //find the closest visible node to the bots position
            int closestNodeToBot = GetClosestNodeToPosition(Owner.Position);

            //remove the destination node from the list and return false if no
            //visible node found. This will occur if the navgraph is badly
            //designed or if the bot has managed to get itself *inside* the
            //geometry (surrounded by walls), or an obstacle
            if (closestNodeToBot == NO_CLOSEST_NODE_FOUND)
            {
                LogUtil.WriteLineIfLogNavigation(
                    String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                    String.Format("{0,-23}] ", "RequestPathToItem") +
                    String.Format("[{0,-9}]", " ") + //skip status field
                    " At position: " + Vector2Util.ToString(Owner.Position) +
                    " Item type: " + EnumUtil.GetDescription(itemType) +
                    " No closest node to position found." +
                    " Has bot penetrated a wall?");
                return false;
            }

            LogUtil.WriteLineIfLogNavigation(
                String.Format("[{0,-8}] [{1,17}.", Owner.Name, "PathPlanner") +
                String.Format("{0,-23}] ", "RequestPathToItem") +
                String.Format("[{0,-9}]", " ") + //skip status field
                " At position: " + Vector2Util.ToString(Owner.Position) +
                " Item type: " + EnumUtil.GetDescription(itemType) +
                " Closest node to position is: " + closestNodeToBot);

            //create an instance of the search algorithm
            CurrentSearch =
                new GraphSearchDijkstrasTimeSliced(
                    NavGraph,
                    closestNodeToBot,
                    (int) itemType,
                    bot); //TODO: target is used as itemType and nodeIndex ... fix

            //register the search with the path manager
            GameManager.GameManager.Instance.PathManager.Register(this);

            return true;
        }

        ///<summary>
        ///used to retrieve the position of a graph node from its index.
        ///</summary>
        ///<param name="idx"></param>
        ///<returns>the position of a graph node from its index.</returns>
        public Vector2 GetNodePosition(int idx)
        {
            if (GraphNode.IsInvalidIndex(idx))
                return Vector2.Zero;
            return NavGraph.GetNode(idx).Position;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly SparseGraph _navGraph;
        private readonly BotEntity _owner;
        private GraphSearchTimeSliced _currentSearch;
        private Vector2 _destination;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}