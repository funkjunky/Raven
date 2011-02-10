#region File description

//------------------------------------------------------------------------------
//Goal.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Messaging;
using Mindcrafters.RavenX.Navigation;
using Mindcrafters.Tx2D.GameAI;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal
{
    ///<summary>
    ///Base goal class
    ///</summary>
    public abstract class Goal
    {
        #region Static methods, fields, constructors

        protected const string LogAddSubgoalText = " AddSubgoal: ";
        protected const string LogDestinationText = " Destination: ";
        protected const string LogRequestPathToPositionText = " RequestPathToPosition: ";
        protected const string LogSeekToPositionText = " SeekToPosition: ";

        private static readonly string _logActivateText =
            String.Format("{0,-23}] ", "Activate");

        private static readonly string _logHandleMessageText =
            String.Format("{0,-23}] ", "HandleMessage");

        private static readonly string _logProcessText =
            String.Format("{0,-23}] ", "Process");

        private static readonly string _logTerminateText =
            String.Format("{0,-23}] ", "Terminate");

        ///<summary>
        ///log text for <see cref="Activate"/> method
        ///</summary>
        protected static string LogActivateText
        {
            get { return _logActivateText; }
        }

        ///<summary>
        ///log text for <see cref="HandleMessage"/> method
        ///</summary>
        protected static string LogHandleMessageText
        {
            get { return _logHandleMessageText; }
        }

        ///<summary>
        ///log text for <see cref="Process"/> method
        ///</summary>
        protected static string LogProcessText
        {
            get { return _logProcessText; }
        }

        ///<summary>
        ///log text for <see cref="Terminate"/> method
        ///</summary>
        protected static string LogTerminateText
        {
            get { return _logTerminateText; }
        }

        ///<summary>
        ///Generate a string representation of a path edge
        ///TODO: this could probably be moved to PathEdge
        ///</summary>
        ///<returns></returns>
        protected static string EdgeToString(PathEdge edge)
        {
            StringBuilder edgeText = new StringBuilder();
            edgeText.AppendFormat("{0}--{1}-->{2}",
                                  Vector2Util.ToString(edge.Source),
                                  edge.Behavior,
                                  Vector2Util.ToString(edge.Destination));
            return edgeText.ToString();
        }

        ///<summary>
        ///Convert an item type to the corresponding goal type
        ///</summary>
        ///<param name="itemType"></param>
        ///<returns></returns>
        ///<exception cref="ApplicationException"></exception>
        public static GoalTypes ItemTypeToGoalType(ItemTypes itemType)
        {
            switch (itemType)
            {
                case ItemTypes.Health:
                    return GoalTypes.GetHealth;

                case ItemTypes.Railgun:
                    return GoalTypes.GetRailgun;

                case ItemTypes.RocketLauncher:
                    return GoalTypes.GetRocketLauncher;

                case ItemTypes.Shotgun:
                    return GoalTypes.GetShotgun;

                case ItemTypes.Flag:
                    return GoalTypes.CaptureFlag;

                default:
                    throw new ApplicationException(
                        "Goal.ItemToGoalType: cannot determine item type");
            }
        }

        #endregion

        #region Constructors

        ///<summary>
        ///Base goal constructor
        ///</summary>
        ///<param name="bot"></param>
        ///<param name="goalType"></param>
        protected Goal(BotEntity bot, GoalTypes goalType)
        {
            _bot = bot;
            _goalType = goalType;
            _status = StatusTypes.Inactive;
            _logPrefixText = String.Format("[{0,-8}] [{1,17}.", Bot.Name, goalType);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region GoalTypes enum

        ///<summary>
        ///Goal types
        ///</summary>
        public enum GoalTypes
        {
            Think,
            Explore,
            [Description("Arrive at position")] ArriveAtPosition,
            [Description("Seek to position")] SeekToPosition,
            [Description("Follow path")] FollowPath,
            [Description("Traverse edge")] TraverseEdge,
            [Description("Move to position")] MoveToPosition,
            [Description("Get health")] GetHealth,
            [Description("Get shotgun")] GetShotgun,
            [Description("Get railgun")] GetRailgun,
            [Description("Get rocket launcher")] GetRocketLauncher,
            Wander,
            [Description("Negotiate door")] NegotiateDoor,
            [Description("Attack target")]
            AttackTarget,
            //[Description("Follow Formation")]
            //FollowFormation,
            //[Description("Lead Formation")]
            //LeadFormation,
            [Description("Hunt target")] HuntTarget,
            Strafe,
            [Description("Adjust range")] AdjustRange,
            [Description("Evade bot")] EvadeBot,
            [Description("Pursue bot")] PursueBot,
            [Description("Capture Flag")] CaptureFlag

        }

        #endregion

        #region StatusTypes enum

        ///<summary>
        ///Status types
        ///</summary>
        public enum StatusTypes
        {
            Active,
            Inactive,
            Completed,
            Failed
        }

        #endregion

        ///<summary>
        ///Goal is completed
        ///</summary>
        public bool IsComplete
        {
            get { return _status == StatusTypes.Completed; }
        }

        ///<summary>
        ///Goal is active
        ///</summary>
        public bool IsActive
        {
            get { return _status == StatusTypes.Active; }
        }

        ///<summary>
        ///Goal is inactive
        ///</summary>
        public bool IsInactive
        {
            get { return _status == StatusTypes.Inactive; }
        }

        ///<summary>
        ///Goal has failed
        ///</summary>
        public bool HasFailed
        {
            get { return _status == StatusTypes.Failed; }
        }

        ///<summary>
        ///Bot entity that owns this goal
        ///</summary>
        public BotEntity Bot
        {
            get { return _bot; }
            set { _bot = value; }
        }

        ///<summary>
        ///Goal type of this goal
        ///</summary>
        public GoalTypes GoalType
        {
            get { return _goalType; }
            set { _goalType = value; }
        }

        ///<summary>
        ///Status of this goal
        ///</summary>
        public StatusTypes Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #region Public methods

        ///<summary>
        ///Activate this goal
        ///</summary>
        public abstract void Activate();

        ///<summary>
        ///Process this goal
        ///</summary>
        ///<returns></returns>
        public abstract StatusTypes Process();

        ///<summary>
        ///Terminate this goal
        ///</summary>
        public abstract void Terminate();

        ///<summary>
        ///goals can handle messages. Many don't though, so this defines a
        ///default behavior
        ///</summary>
        ///<param name="msg"></param>
        ///<returns></returns>
        public virtual bool HandleMessage(Telegram msg)
        {
            return false;
        }

        ///<summary>
        ///a Goal is atomic and cannot aggregate subgoals yet we must implement
        ///this method to provide the uniform interface required for the goal
        ///hierarchy.
        ///</summary>
        ///<param name="goal"></param>
        ///<exception cref="ApplicationException"></exception>
        public virtual void AddSubgoal(Goal goal)
        {
            throw new ApplicationException("Cannot add goals to atomic goals");
        }

        ///<summary>
        ///used to render any goal specific information
        ///</summary>
        public virtual void Render()
        {
            DebugRender();
        }

        ///<summary>
        ///this is used to draw the name of the goal at the specific position.
        ///used for debugging
        ///</summary>
        ///<param name="pos"></param>
        public virtual void RenderAtPos(ref Vector2 pos)
        {
            pos.Y += 15;
            Color textColor = Color.Black;

            if (IsComplete)
                textColor = Color.Green;
            if (IsInactive)
                textColor = Color.Black;
            if (HasFailed)
                textColor = Color.Red;
            if (IsActive)
                textColor = Color.Blue;

            TextUtil.DrawText(
                pos,
                textColor,
                EnumUtil.GetDescription(GoalType));
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///common text prefix for log: "[BOT_$objectId$] [$goalType$."
        ///</summary>
        protected string LogPrefixText
        {
            get { return _logPrefixText; }
        }

        ///<summary>
        ///Status text for log
        ///</summary>
        protected string LogStatusText
        {
            get { return String.Format("[{0,-9}]", Status); }
        }

        ///<summary>
        ///if status is failed this method sets it to inactive so that the goal
        ///will be reactivated (replanned) on the next update-step.
        ///</summary>
        protected void ReactivateIfFailed()
        {
            if (HasFailed)
            {
                Status = StatusTypes.Inactive;
            }
        }

        ///<summary>
        ///reactivate in active goal
        ///</summary>
        protected void ActivateIfInactive()
        {
            if (IsInactive) Activate();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        [Conditional("DEBUG")]
        public virtual void DebugRender()
        {
            DebugRenderIfGoal();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_GOAL is defined.
        ///</summary>
        [Conditional("DEBUG_GOAL")]
        public virtual void DebugRenderIfGoal()
        {
        }

        #endregion

        #region Private, protected, internal fields

        private readonly string _logPrefixText;
        private BotEntity _bot;
        private GoalTypes _goalType;
        private StatusTypes _status;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}
