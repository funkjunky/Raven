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
    public class CaptureFlag : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal AttackTarget
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        public CaptureFlag(BotEntity bot)
            : base(bot, GoalTypes.CaptureFlag)
        {
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            Status = StatusTypes.Active;

            //if this goal is reactivated then there may be some existing
            //subgoals that must be removed
            RemoveAllSubgoals();
            Vector2? closestFlag = Bot.FoundTriggers.LocationOfClosestEnemyFlag(Bot.Position);
            if (closestFlag != null)
            {
                if (!Bot.IsCarryingFlag)
                {
                    AddSubgoal(new MoveToPosition(Bot, closestFlag.Value));
                    //AddSubgoal(new SeekToPosition(Bot, closestFlag.Value));
                }
                AddSubgoal(new MoveToPosition(Bot, Bot.FoundTriggers.LocationOfOurFlag));
            }
            else
            {
                Status = StatusTypes.Completed;
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