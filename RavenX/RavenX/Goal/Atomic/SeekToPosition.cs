#region File description

//------------------------------------------------------------------------------
//SeekToPosition.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Memory.NavigationalMemory;

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
    ///class for the atomic goal SeekToPosition
    ///</summary>
    public class SeekToPosition : Goal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for atomic goal SeekToPosition
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        ///<param name="destination">destination</param>
        public SeekToPosition(BotEntity bot, Vector2 destination)
            : base(bot, GoalTypes.SeekToPosition)
        {
            _destination = destination;
            _timeExpected = 0.0f;

            _sourceNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(
                    bot.Position);
            _destinationNodeIndex =
                bot.PathPlanner.GetClosestNodeToPosition(
                    destination);
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
        public override void Activate()
        {
            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogDestinationText + _destination);
            Status = StatusTypes.Active;
            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogDestinationText + _destination);

            //record the time the bot starts this goal
            _startTime = Time.TimeNow;

            //This value is used to determine if the bot becomes stuck 
            _usedLookup = Bot.CalculateTimeToReachPosition(_destination, out _timeExpected);

            Bot.Steering.Target = _destination;

            Bot.Steering.SeekIsOn = true;

            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogDestinationText + _destination +
                " in " + _timeExpected.ToString("F2") + " secs " +
                (_usedLookup ? "using lookup" : "using calculation"));
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive " + LogDestinationText + _destination);

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                LogDestinationText + _destination +
                " in " + _timeExpected.ToString("F2") + " secs " +
                (_usedLookup ? "using lookup" : "using calculation"));

            //test to see if the bot has become stuck
            if (IsStuck())
            {
                Status = StatusTypes.Failed;

                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.SeekToPosition,
                    NavigationalMemory.TravelStatuses.Failed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.SeekToPosition,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogSeekToPosition(
                    LogPrefixText + LogProcessText + LogStatusText +
                    LogDestinationText + _destination +
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
                //test to see if the bot has reached the waypoint. If so terminate the goal
            else if (Bot.IsAtPosition(
                _destination,
                GameManager.GameManager.Instance.Parameters.WaypointSeekDist))
            {
                Status = StatusTypes.Completed;

                Bot.NavigationalMemory.RecordTimeTaken(
                    NavigationalMemory.TravelModes.SeekToPosition,
                    NavigationalMemory.TravelStatuses.Completed,
                    SourceNodeIndex,
                    DestinationNodeIndex,
                    _timeTaken,
                    _timeExpected);

                Bot.NavigationalMemory.MakeCurrent(
                    NavigationalMemory.TravelModes.SeekToPosition,
                    SourceNodeIndex,
                    DestinationNodeIndex);

                LogUtil.WriteLineIfLogSeekToPosition(
                    LogPrefixText + LogProcessText + LogStatusText +
                    LogDestinationText + _destination +
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
            Bot.Steering.SeekIsOn = false;
            Bot.Steering.ArriveIsOn = false;
            //TODO: this was in original code (what if Failed?)
            //Status = StatusTypes.Completed; 

            Bot.NavigationalMemory.MakeCurrent(
                NavigationalMemory.TravelModes.SeekToPosition,
                SourceNodeIndex,
                DestinationNodeIndex);

            LogUtil.WriteLineIfLogSeekToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                LogDestinationText + _destination +
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
            base.Render();

            switch (Status)
            {
                case StatusTypes.Active:
                    DrawUtil.CircleFill(_destination, 3, Color.Green, 20);
                    DrawUtil.Circle(_destination, 3, Color.Black, 20);
                    break;
                case StatusTypes.Inactive:
                    DrawUtil.CircleFill(_destination, 3, Color.Red, 20);
                    DrawUtil.Circle(_destination, 3, Color.Black, 20);
                    break;
            }
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfSeekToPosition();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_SEEK_TO_POSITION
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_SEEK_TO_POSITION")]
        public void DebugRenderIfSeekToPosition()
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
        ///</returns>
        private bool IsStuck()
        {
            _timeTaken = Time.TimeNow - _startTime;

            return _timeTaken > _timeExpected;
        }

        #endregion

        #region Private, protected, internal fields

        //the position the bot is moving to
        private readonly Vector2 _destination;
        private readonly int _destinationNodeIndex;
        private readonly int _sourceNodeIndex;

        //the approximate time the bot should take to travel the destination

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