#region File description

//------------------------------------------------------------------------------
//FollowPath.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GarageGames.Torque.GUI;
using MapContent;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Goal.Atomic;
using Mindcrafters.RavenX.Map;
using Mindcrafters.RavenX.Memory.NavigationalMemory;
using Mindcrafters.RavenX.Navigation;

    #endregion

    #region Microsoft

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

    #endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Composite
{
    ///<summary>
    ///class for composite goal FollowPath
    ///</summary>
    public class FollowPath : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///Constructor for composite goal FollowPath
        ///</summary>
        ///<param name="bot">Bot entity that owns this goal</param>
        ///<param name="path">path to follow</param>
        public FollowPath(BotEntity bot, List<PathEdge> path)
            : base(bot, GoalTypes.FollowPath)
        {
            _path = path;
            _sourceNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(path[0].Source);
            _destinationNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(
                    path[path.Count - 1].Destination);
            _usedLookup = Bot.CalculateTimeToReachPosition(
                path[path.Count - 1].Destination, out _timeExpected);
            _startTime = Time.TimeNow;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///source node index
        ///</summary>
        public int SourceNodeIndex
        {
            get { return _sourceNodeIndex; }
        }

        ///<summary>
        ///destination node index
        ///</summary>
        public int DestinationNodeIndex
        {
            get { return _destinationNodeIndex; }
        }

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        ///<exception cref="ApplicationException"></exception>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogActivateText + LogStatusText);

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogActivateText + LogStatusText +
                " Path: " + PathToString(_path));

            //get a reference to the next edge
            PathEdge edge = _path[0];

            //remove the edge from the path
            _path.RemoveAt(0);

            //some edges require the bot to use a specific behavior when
            //following them. This switch statement queries the edge behavior
            //flag and adds the appropriate goal(s) to the subgoal list.
            switch (edge.Behavior)
            {
                case EdgeData.BehaviorTypes.Normal:
                    LogUtil.WriteLineIfLogFollowPath(
                        LogPrefixText + LogActivateText + LogStatusText +
                        LogAddSubgoalText + "TraverseEdge " +
                        EdgeToString(edge));
                    AddSubgoal(new TraverseEdge(Bot, edge, (_path.Count == 0)));
                    break;

                case EdgeData.BehaviorTypes.GoesThroughDoor:
                    Door door =
                        (Door) EntityManager.Instance.GetEntityFromId(
                                   edge.DoorId);

                    if (door.Status == Door.DoorStatus.Closed ||
                        door.Status == Door.DoorStatus.Closing)
                    {
                        LogUtil.WriteLineIfLogFollowPath(
                            LogPrefixText + LogActivateText + LogStatusText +
                            LogAddSubgoalText + "NegotiateDoor " +
                            EdgeToString(edge));
                        //add a goal that is able to handle opening the door
                        AddSubgoal(
                            new NegotiateDoor(Bot, edge, (_path.Count == 0)));
                    }
                    else
                    {
                        LogUtil.WriteLineIfLogFollowPath(
                            LogPrefixText + LogActivateText + LogStatusText +
                            LogAddSubgoalText + "TraverseEdge(OpenDoor) " +
                            EdgeToString(edge));
                        AddSubgoal(new TraverseEdge(Bot, edge, (_path.Count == 0)));
                    }
                    break;

                case EdgeData.BehaviorTypes.Jump:
                    //add subgoal to jump along the edge
                    break;

                case EdgeData.BehaviorTypes.Grapple:
                    //add subgoal to grapple along the edge
                    break;

                default:
                    throw new ApplicationException(
                        "FollowPath.Activate: Unrecognized edge type");
            }
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogProcessText + LogStatusText +
                " Before processing" + SubgoalsToString());

            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogProcessText + LogStatusText +
                " After processing" + SubgoalsToString());

            //if there are no subgoals present check to see if the path still
            //has edges remaining. If it does then call activate to grab the
            //next edge.
            if (Status == StatusTypes.Completed)
            {
                if (_path.Count > 0)
                {
                    LogUtil.WriteLineIfLogFollowPath(
                        LogPrefixText + LogProcessText + LogStatusText +
                        " Completed subgoals and Activated to grab next edge");
                    Activate();
                }
            }
            LogUtil.WriteLineIfLogFollowPath(
                LogPrefixText + LogProcessText + LogStatusText);

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            _timeTaken = Time.TimeNow - _startTime;
            if (Status == StatusTypes.Completed)
            {
                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.FollowPath,
                    NavigationalMemory.TravelStatuses.Completed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.FollowPath,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogFollowPath(
                    LogPrefixText + LogTerminateText + LogStatusText +
                    (_usedLookup
                         ?
                             " TimeExpected(usedLookup): "
                         :
                             " TimeExpected(calculated): ") +
                    _timeExpected.ToString("F2") +
                    " TimeTaken: " + _timeTaken.ToString("F2") +
                    " CompletedMarginRunningAvg: " +
                    Bot.NavigationalMemory.Current.CompletedMarginRunningAverage.ToString("F2") +
                    " FailedMarginRunningAvg: " +
                    Bot.NavigationalMemory.Current.FailedMarginRunningAverage.ToString("F2") +
                    " Completed: " + Bot.NavigationalMemory.Current.CompletedCount +
                    " Failed: " + Bot.NavigationalMemory.Current.FailedCount);
            }
            else
            {
                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.FollowPath,
                    NavigationalMemory.TravelStatuses.Failed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.FollowPath,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogFollowPath(
                    LogPrefixText + LogTerminateText + LogStatusText +
                    (_usedLookup
                         ?
                             " TimeExpected(usedLookup): "
                         :
                             " TimeExpected(calculated): ") +
                    _timeExpected.ToString("F2") +
                    " TimeTaken: " + _timeTaken.ToString("F2") +
                    " CompletedMarginRunningAvg: " +
                    Bot.NavigationalMemory.Current.CompletedMarginRunningAverage.ToString("F2") +
                    " FailedMarginRunningAvg: " +
                    Bot.NavigationalMemory.Current.FailedMarginRunningAverage.ToString("F2") +
                    " Completed: " + Bot.NavigationalMemory.Current.CompletedCount +
                    " Failed: " + Bot.NavigationalMemory.Current.FailedCount);
            }
        }

        ///<summary>
        ///
        ///</summary>
        public override void Render()
        {
            //render all the path waypoints remaining on the path list
            foreach (PathEdge curPathEdge in _path)
            {
                DrawUtil.LineWithArrow(curPathEdge.Source, curPathEdge.Destination, Color.Black);

                DrawUtil.CircleFill(curPathEdge.Destination, 3, Color.Red, 20);
                DrawUtil.Circle(curPathEdge.Destination, 3, Color.Black, 20);
            }

            //forward the request to the subgoals
            base.Render();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfFollowPath();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_FOLLOW_PATH is defined.
        ///</summary>
        [Conditional("DEBUG_FOLLOW_PATH")]
        public void DebugRenderIfFollowPath()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //a local copy of the path returned by the path planner
        private readonly int _destinationNodeIndex;
        private readonly List<PathEdge> _path;

        private readonly int _sourceNodeIndex;
        private readonly float _startTime;
        private readonly float _timeExpected;
        private readonly bool _usedLookup;
        private float _timeTaken;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}