#region File description

//------------------------------------------------------------------------------
//AttackTarget.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Goal.Atomic;

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
    ///class for the composite goal AttackTarget
    ///</summary>
    public class AttackTarget : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal AttackTarget
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        public AttackTarget(BotEntity bot)
            : base(bot, GoalTypes.AttackTarget)
        {
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogActivateText + LogStatusText);
            Status = StatusTypes.Active;
            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogActivateText + LogStatusText);

            //if this goal is reactivated then there may be some existing
            //subgoals that must be removed
            RemoveAllSubgoals();

            //it is possible for a bot's target to die whilst this goal is
            //active so we must test to make sure the bot always has an active
            //target
            if (!Bot.TargetingSystem.IsTargetPresent)
            {
                Status = StatusTypes.Completed;
                LogUtil.WriteLineIfLogAttackTarget(
                    LogPrefixText + LogActivateText + LogStatusText +
                    " Target not present ");
                return;
            }

            //if the bot is able to shoot the target (there is LOS between bot
            //and target), then select a tactic to follow while shooting
            if (Bot.TargetingSystem.IsTargetShootable)
            {
                //if the bot has space to strafe then do so
                Vector2 dummy;
                if (Bot.CanStepLeft(out dummy) || Bot.CanStepRight(out dummy))
                {
                    LogUtil.WriteLineIfLogAttackTarget(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " Target: " + Bot.TargetingSystem.Target.Name +
                        LogAddSubgoalText + "Strafe");
                    AddSubgoal(new Strafe(Bot));
                }
                    //if not able to strafe, head directly at the target's position 
                else
                {
                    LogUtil.WriteLineIfLogAttackTarget(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " Target: " + Bot.TargetingSystem.Target.Name +
                        LogAddSubgoalText + "SeekToPosition: " +
                        Bot.TargetBot.Position);
                    AddSubgoal(new SeekToPosition(Bot, Bot.TargetBot.Position));
                }
            }
                //if the target is not visible, go hunt it.
            else
            {
                LogUtil.WriteLineIfLogAttackTarget(
                    LogPrefixText + LogActivateText + LogStatusText +
                    " Target: " + Bot.TargetingSystem.Target.Name +
                    LogAddSubgoalText + "HuntTarget");
                AddSubgoal(new HuntTarget(Bot));
            }
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");
            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                " Before processing" + SubgoalsToString());

            //process the subgoals
            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                " After processing" + SubgoalsToString());

            ReactivateIfFailed();

            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                SubgoalsToString());

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            Status = StatusTypes.Completed;
            LogUtil.WriteLineIfLogAttackTarget(
                LogPrefixText + LogProcessText + LogStatusText +
                SubgoalsToString());
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
            DebugRenderIfAttackTarget();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_ATTACK_TARGET
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_ATTACK_TARGET")]
        public void DebugRenderIfAttackTarget()
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