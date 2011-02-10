#region File description

//------------------------------------------------------------------------------
//HuntTarget.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
using GarageGames.Torque.GUI;
using GarageGames.Torque.MathUtil;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;

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
    ///Causes a bot to search for its current target. Exits when target
    ///is in view
    ///</summary>
    public class HuntTarget : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal HuntTarget
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        public HuntTarget(BotEntity bot)
            : base(bot, GoalTypes.HuntTarget)
        {
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogActivateText + LogStatusText);

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogActivateText + LogStatusText);

            //if this goal is reactivated then there may be some existing subgoals that
            //must be removed
            RemoveAllSubgoals();

            //it is possible for the target to die whilst this goal is active so we
            //must test to make sure the bot always has an active target
            if (Bot.TargetingSystem.IsTargetPresent)
            {
                //grab a local copy of the last recorded position (LRP) of the target
                Vector2 lrp = Bot.TargetingSystem.LastRecordedPosition;

                //if the bot has reached the LRP and it still hasn't found the target
                //it starts to search by using the explore goal to move to random
                //map locations
                if (Epsilon.VectorIsZero(lrp) || Bot.IsAtPosition(lrp))
                {
                    LogUtil.WriteLineIfLogHuntTarget(
                        LogPrefixText + LogActivateText + LogStatusText +
                        LogAddSubgoalText + "Explore");
                    AddSubgoal(new Explore(Bot));
                }
                    //else move to the LRP
                else
                {
                    LogUtil.WriteLineIfLogHuntTarget(
                        LogPrefixText + LogActivateText + LogStatusText +
                        LogAddSubgoalText + "MoveToPosition: " + lrp);
                    AddSubgoal(new MoveToPosition(Bot, lrp));
                }
            }
                //if there is no active target then this goal can be removed from the queue
            else
            {
                Status = StatusTypes.Completed;
                LogUtil.WriteLineIfLogHuntTarget(
                    LogPrefixText + LogActivateText + LogStatusText);
            }
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogThink(
                LogPrefixText + LogProcessText + LogStatusText +
                " Before processing" + SubgoalsToString());

            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                " After processing" + SubgoalsToString());

            //if target is in view this goal is satisfied
            if (Bot.TargetingSystem.IsTargetWithinFOV)
            {
                Status = StatusTypes.Completed;
            }

            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogProcessText + LogStatusText);

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogHuntTarget(
                LogPrefixText + LogTerminateText + LogStatusText);
        }

        ///<summary>
        ///
        ///</summary>
        public override void Render()
        {
            //render last recorded position as a green circle
            if (Bot.TargetingSystem.IsTargetPresent)
            {
                DrawUtil.CircleFill(
                    Bot.TargetingSystem.LastRecordedPosition,
                    3,
                    Color.Red,
                    20);
            }

            //forward the request to the subgoals
            base.Render();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfHuntTarget();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_HUNT_TARGET
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_HUNT_TARGET")]
        public void DebugRenderIfHuntTarget()
        {
        }

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