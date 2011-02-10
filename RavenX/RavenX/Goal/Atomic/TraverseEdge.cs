#region File description

//------------------------------------------------------------------------------
//TraverseEdge.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
using GarageGames.Torque.GUI;
using MapContent;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
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

namespace Mindcrafters.RavenX.Goal.Atomic
{
    ///<summary>
    ///class for the atomic goal TraverseEdge
    ///</summary>
    public class TraverseEdge : Goal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for atomic goal TraverseEdge
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        ///<param name="edgeToFollow">edge to follow</param>
        ///<param name="lastEdgeInPath">true if last edge in path</param>
        public TraverseEdge(
            BotEntity bot,
            PathEdge edgeToFollow,
            bool lastEdgeInPath)
            : base(bot, GoalTypes.TraverseEdge)
        {
            _edgeToFollow = edgeToFollow;
            _timeExpected = 0.0f;
            _lastEdgeInPath = lastEdgeInPath;

            _sourceNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(
                    edgeToFollow.Source);
            _destinationNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(
                    edgeToFollow.Destination);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the edge to traverse
        ///</summary>
        public PathEdge EdgeToFollow
        {
            get { return _edgeToFollow; }
        }

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
        public override void Activate()
        {
            LogUtil.WriteLineIfLogTraverseEdge(
                LogPrefixText + LogActivateText + LogStatusText +
                " " + EdgeToString(EdgeToFollow));

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogTraverseEdge(
                LogPrefixText + LogActivateText + LogStatusText +
                " " + EdgeToString(EdgeToFollow));

            //the edge behavior flag may specify a type of movement that
            //necessitates a change in the bot's max possible speed as it
            //follows this edge
            switch (EdgeToFollow.Behavior)
            {
                case EdgeData.BehaviorTypes.Swim:
                    {
                        Bot.MaxSpeed = Bot.MaxSwimmingSpeed;
                    }

                    break;

                case EdgeData.BehaviorTypes.Crawl:
                    {
                        Bot.MaxSpeed = Bot.MaxCrawlingSpeed;
                    }

                    break;
            }

            //record the time the bot starts this goal
            _startTime = Time.TimeNow;

            //calculate the expected time required to reach the this waypoint. This value
            //is used to determine if the bot becomes stuck 
            _usedLookup = Bot.CalculateTimeToReachPosition(EdgeToFollow.Destination, out _timeExpected);

            //set the steering target
            Bot.Steering.Target = EdgeToFollow.Destination;

            //Set the appropriate steering behavior. If this is the last edge in the path
            //the bot should arrive at the position it points to, else it should seek
            if (_lastEdgeInPath)
            {
                LogUtil.WriteLineIfLogTraverseEdge(
                    LogPrefixText + LogActivateText + LogStatusText +
                    " " + EdgeToString(EdgeToFollow) + " Arrive at position in " +
                    _timeExpected.ToString("F2") + " secs");
                Bot.Steering.ArriveIsOn = true;
            }
            else
            {
                LogUtil.WriteLineIfLogTraverseEdge(
                    LogPrefixText + LogActivateText + LogStatusText +
                    " " + EdgeToString(EdgeToFollow) + " Seek to position in " +
                    _timeExpected.ToString("F2") + " secs");
                Bot.Steering.SeekIsOn = true;
            }
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogTraverseEdge(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive " + EdgeToString(EdgeToFollow));
            //if status is inactive, call Activate()
            ActivateIfInactive();
            LogUtil.WriteLineIfLogTraverseEdge(
                LogPrefixText + LogProcessText + LogStatusText +
                " " + EdgeToString(EdgeToFollow) +
                " in " + _timeExpected.ToString("F2") + " secs");

            //if the bot has become stuck return failure
            if (IsStuck())
            {
                Status = StatusTypes.Failed;

                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.TraverseEdge,
                    NavigationalMemory.TravelStatuses.Failed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.TraverseEdge,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogTraverseEdge(
                    LogPrefixText + LogProcessText + LogStatusText +
                    " " + EdgeToString(EdgeToFollow) +
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
                //if the bot has reached the end of the edge return completed
            else if (Bot.IsAtPosition(
                EdgeToFollow.Destination,
                GameManager.GameManager.Instance.Parameters.WaypointSeekDist))
            {
                Status = StatusTypes.Completed;

                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.TraverseEdge,
                    NavigationalMemory.TravelStatuses.Completed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.TraverseEdge,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogTraverseEdge(
                    LogPrefixText + LogProcessText + LogStatusText +
                    " " + EdgeToString(EdgeToFollow) +
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

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            //turn off steering behaviors.
            Bot.Steering.SeekIsOn = false;
            Bot.Steering.ArriveIsOn = false;
            //return max speed back to normal
            Bot.MaxSpeed = Bot.MaxNormalSpeed;

            Bot.NavigationalMemory.MakeCurrent(
                NavigationalMemory.TravelModes.TraverseEdge,
                SourceNodeIndex,
                DestinationNodeIndex);

            LogUtil.WriteLineIfLogTraverseEdge(
                LogPrefixText + LogProcessText + LogStatusText +
                " " + EdgeToString(EdgeToFollow) +
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

        ///<summary>
        ///
        ///</summary>
        public override void Render()
        {
            if (Status != StatusTypes.Active)
                return;

            base.Render();

            DrawUtil.LineWithArrow(
                Bot.Position,
                EdgeToFollow.Destination,
                Color.Blue);
            DrawUtil.CircleFill(
                EdgeToFollow.Destination,
                3,
                Color.Green,
                20);
            DrawUtil.Circle(
                EdgeToFollow.Destination,
                3,
                Color.Black,
                20);
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfTraverseEdge();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_TRAVERSE_EDGE is defined.
        ///</summary>
        [Conditional("DEBUG_TRAVERSE_EDGE")]
        public void DebugRenderIfTraverseEdge()
        {
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        /// 
        ///</summary>
        ///<returns>
        ///true if the bot has taken longer than expected to reach the currently
        ///active waypoint
        /// </returns>
        private bool IsStuck()
        {
            _timeTaken = Time.TimeNow - _startTime;

            return _timeTaken > _timeExpected;
        }

        #endregion

        #region Private, protected, internal fields

        //the edge the bot will follow
        private readonly int _destinationNodeIndex;
        private readonly PathEdge _edgeToFollow;

        //true if _edgeToFollow is the last in the path.
        private readonly bool _lastEdgeInPath;
        private readonly int _sourceNodeIndex;

        //the estimated time the bot should take to traverse the edge

        //this records the time this goal was activated
        private float _startTime;
        private float _timeExpected;

        //this records the time taken to reach the destination
        private float _timeTaken;

        //this records whether expected time was looked up
        private bool _usedLookup;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}