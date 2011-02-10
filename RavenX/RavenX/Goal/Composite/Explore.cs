#region Namespace imports

#region System

using System.Diagnostics;
using MapContent;
using Microsoft.Xna.Framework;
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
    ///class for the composite goal Explore
    ///</summary>
    public class Explore : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///Constructor for composite goal Explore
        ///</summary>
        ///<param name="bot"></param>
        public Explore(BotEntity bot)
            : base(bot, GoalTypes.Explore)
        {
            _destinationIsSet = false;
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogActivateText + LogStatusText);

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogActivateText + LogStatusText);

            //if this goal is reactivated then there may be some existing
            //subgoals that must be removed
            RemoveAllSubgoals();

            if (!_destinationIsSet)
            {
                //grab a random position
                _destination =
                    GameManager.GameManager.Instance.Map.GetRandomNodeLocation();
                _destinationIsSet = true;
            }

            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogActivateText + LogStatusText +
                LogRequestPathToPositionText + _destination);

            //and request a path to that position
            Bot.PathPlanner.RequestPathToPosition(_destination,Bot);

            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogActivateText + LogStatusText +
                LogAddSubgoalText + LogSeekToPositionText + _destination);

            //the bot may have to wait a few update cycles before a path is
            //calculated so for appearances sake it simply ARRIVES toward the
            //destination until a path has been found
            //AddSubgoal(new ArriveAtPos(Bot, _destination));
            AddSubgoal(new SeekToPosition(Bot, _destination));
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");
            ActivateIfInactive();

            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogProcessText + LogStatusText +
                " Before processing" + SubgoalsToString());

            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogProcessText + LogStatusText +
                " After processing" + SubgoalsToString());

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogExplore(
                LogPrefixText + LogProcessText + LogStatusText +
                SubgoalsToString());
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
                LogUtil.WriteLineIfLogExplore(
                    LogPrefixText + LogHandleMessageText + LogStatusText +
                    LogDestinationText + _destination +
                    " Message handled by subgoal");
            }

            //if the msg was not handled, test to see if this goal can handle it
            if (isHandled == false)
            {
                switch (msg.Msg)
                {
                    case MessageTypes.PathReady:
                        LogUtil.WriteLineIfLogExplore(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogDestinationText + _destination +
                            " Msg: path ready");
                        //clear any existing goals
                        RemoveAllSubgoals();
                        AddSubgoal(
                            new FollowPath(Bot, Bot.PathPlanner.GetPath()));
                        return true; //msg handled

                    case MessageTypes.NoPathAvailable:
                        LogUtil.WriteLineIfLogExplore(
                            LogPrefixText + LogHandleMessageText + LogStatusText +
                            LogDestinationText + _destination +
                            " Msg: no path available");
                        Status = StatusTypes.Failed;
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
            DebugRenderIfExplore();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_EXPLORE
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_EXPLORE")]
        public void DebugRenderIfExplore()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private Vector2 _destination = Vector2.Zero;
        //set to true when the destination for the exploration has been
        //established
        private bool _destinationIsSet;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}