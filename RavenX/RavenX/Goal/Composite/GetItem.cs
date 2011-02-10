#region File description

//------------------------------------------------------------------------------
//GetItem.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Diagnostics;
using MapContent;
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
    ///class for the composite goal GetItem
    ///</summary>
    public class GetItem : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal GetItem
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        ///<param name="itemToGet">item to get</param>
        public GetItem(BotEntity bot, ItemTypes itemToGet)
            : base(bot, ItemTypeToGoalType(itemToGet))
        {
            _itemToGet = itemToGet;
            _giverTrigger = null;
            _logItemToGetText =
                String.Format(" Item: [{0,-12}]",
                              EnumUtil.GetDescription(_itemToGet));
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogActivateText + LogStatusText + LogItemToGetText);

            Status = StatusTypes.Active;

            _giverTrigger = null;

            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogActivateText + LogStatusText + LogItemToGetText +
                " RequestPathToItem");

            //request a path to the item
            Bot.PathPlanner.RequestPathToItem(_itemToGet, Bot);

            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogActivateText + LogStatusText + LogItemToGetText +
                LogAddSubgoalText + "Wander");

            //the bot may have to wait a few update cycles before a path is
            //calculated so for appearances sake it just wanders
            AddSubgoal(new Wander(Bot));
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogProcessText + LogStatusText
                + " ActivateIfInactive " + LogItemToGetText);

            ActivateIfInactive();

            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogProcessText + LogStatusText + LogItemToGetText);

            if (HasItemBeenStolen())
            {
                LogUtil.WriteLineIfLogGetItem(
                    LogPrefixText + LogProcessText + LogStatusText +
                    LogItemToGetText + " Stolen");

                Terminate();
            }
            else
            {
                LogUtil.WriteLineIfLogGetItem(
                    LogPrefixText + LogProcessText + LogStatusText +
                    LogItemToGetText +
                    " Before processing" + SubgoalsToString());

                //process the subgoals
                Status = ProcessSubgoals();

                LogUtil.WriteLineIfLogGetItem(
                    LogPrefixText + LogProcessText + LogStatusText +
                    LogItemToGetText +
                    " After processing" + SubgoalsToString());
            }

            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogProcessText + LogStatusText + LogItemToGetText);

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            RemoveAllSubgoals();
            Status = StatusTypes.Completed;

            LogUtil.WriteLineIfLogGetItem(
                LogPrefixText + LogTerminateText + LogStatusText + LogItemToGetText);
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
                LogUtil.WriteLineIfLogGetItem(
                    LogPrefixText + LogHandleMessageText + LogStatusText +
                    LogItemToGetText + " Message handled by subgoal");
            }

            //if the msg was not handled, test to see if this goal can handle it
            if (!isHandled)
            {
                switch (msg.Msg)
                {
                    case MessageTypes.PathReady:
                        LogUtil.WriteLineIfLogGetItem(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogItemToGetText + " Msg: path ready");
                        //clear any existing goals
                        RemoveAllSubgoals();
                        LogUtil.WriteLineIfLogGetItem(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogAddSubgoalText + "FollowPath: " + PathToString());
                        AddSubgoal(
                            new FollowPath(Bot, Bot.PathPlanner.GetPath()));
                        //get the reference to the item
                        _giverTrigger = msg.ExtraInfo as Trigger.Trigger;
                        return true; //msg handled

                    case MessageTypes.NoPathAvailable:
                        Status = StatusTypes.Failed;
                        LogUtil.WriteLineIfLogGetItem(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogItemToGetText + " Msg: no path available");
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
        //public override void Render()
        //{
        //    base.Render();
        //}
        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfGetItem();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_GET_ITEM
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_GET_ITEM")]
        public void DebugRenderIfGetItem()
        {
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///log text for ItemToGet
        ///</summary>
        protected string LogItemToGetText
        {
            get { return _logItemToGetText; }
        }

        ///<summary>
        /// 
        ///</summary>
        ///<returns>
        ///true if the bot sees that the item it is heading for has been picked
        ///up by an opponent
        /// </returns>
        private bool HasItemBeenStolen()
        {
            return _giverTrigger != null &&
                   !_giverTrigger.IsActive &&
                   _giverTrigger.TriggeringBot != Bot &&
                   Bot.HasLOS(_giverTrigger.Position);
        }

        #endregion

        #region Private, protected, internal fields

        private readonly ItemTypes _itemToGet;

        private readonly string _logItemToGetText;
        private Trigger.Trigger _giverTrigger;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}