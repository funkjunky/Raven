#region File description

//------------------------------------------------------------------------------
//MoveToPosition.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
using GarageGames.Torque.GUI;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Goal.Atomic;
using Mindcrafters.RavenX.Messaging;

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
    ///class for the composite goal MoveToPosition
    ///</summary>
    public class MoveToPosition : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal MoveToPosition
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        ///<param name="destination">destination</param>
        public MoveToPosition(BotEntity bot, Vector2 destination)
            : base(bot, GoalTypes.MoveToPosition)
        {
            _destination = destination;
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogDestinationText + _destination);

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogDestinationText + _destination);

            //make sure the subgoal list is clear.
            RemoveAllSubgoals();

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogActivateText + LogStatusText +
                LogRequestPathToPositionText + _destination);

            //requests a path to the target position from the path planner.
            //Because, for demonstration purposes, the Raven path planner uses
            //time-slicing when processing the path requests the bot may have
            //to wait a few update cycles before a path is calculated. 
            //Consequently, for appearances sake, it just seeks directly to the
            //target position whilst it's awaiting notification that the path 
            //planning request has succeeded/failed
            if (!Bot.PathPlanner.RequestPathToPosition(_destination,Bot))
                return;

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogActivateText + LogStatusText + LogAddSubgoalText +
                LogSeekToPositionText + _destination);
            AddSubgoal(new SeekToPosition(Bot, _destination));
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive " +
                LogDestinationText + _destination);

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                LogDestinationText + _destination +
                " Before processing" + SubgoalsToString());

            //process the subgoals
            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                LogDestinationText + _destination +
                " After processing" + SubgoalsToString());

            //if any of the subgoals have failed then this goal re-plans
            ReactivateIfFailed();

            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogProcessText + LogStatusText +
                LogDestinationText + _destination);

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogMoveToPosition(
                LogPrefixText + LogTerminateText + LogStatusText +
                LogDestinationText + _destination);
        }

        ///<summary>
        ///handle messages
        ///</summary>
        ///<param name="msg"></param>
        ///<returns></returns>
        public override bool HandleMessage(Telegram msg)
        {
            //first, pass the message down the goal hierarchy
            bool isHandled = ForwardMessageToFrontMostSubgoal(msg);

            if (isHandled)
            {
                LogUtil.WriteLineIfLogMoveToPosition(
                    LogPrefixText + LogHandleMessageText + LogStatusText +
                    LogDestinationText + _destination +
                    " Message handled by subgoal");
            }

            //if the msg was not handled, test to see if this goal can handle it
            if (!isHandled)
            {
                switch (msg.Msg)
                {
                    case MessageTypes.PathReady:
                        LogUtil.WriteLineIfLogMoveToPosition(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogDestinationText + _destination +
                            " Msg: path ready");
                        //clear any existing goals
                        RemoveAllSubgoals();

                        LogUtil.WriteLineIfLogMoveToPosition(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogAddSubgoalText + "FollowPath: " + PathToString());
                        AddSubgoal(
                            new FollowPath(Bot, Bot.PathPlanner.GetPath()));
                        return true; //msg handled

                    case MessageTypes.NoPathAvailable:
                        Status = StatusTypes.Failed;
                        LogUtil.WriteLineIfLogMoveToPosition(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogDestinationText + _destination +
                            " Msg: no path available");
                        return true; //msg handled

                    default:
                        return false;
                }
            }

            //handled by subgoals
            return true;
        }

        ///<summary>
        ///
        ///</summary>
        public override void Render()
        {
            //forward the request to the subgoals
            base.Render();

            DrawUtil.CircleFill(_destination, 6, Color.Blue, 20);
            DrawUtil.Circle(_destination, 6, Color.Black, 20);
            DrawUtil.CircleFill(_destination, 4, Color.Red, 20);
            DrawUtil.CircleFill(_destination, 2, Color.Yellow, 20);
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfMoveToPosition();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_MOVE_TO_POSITION
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_MOVE_TO_POSITION")]
        public void DebugRenderIfMoveToPosition()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //the position the bot wants to reach
        private readonly Vector2 _destination;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}