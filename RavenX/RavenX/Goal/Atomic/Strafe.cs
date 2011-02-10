#region File description

//------------------------------------------------------------------------------
//Strafe.cs
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
using Mindcrafters.Library.Math;
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

namespace Mindcrafters.RavenX.Goal.Atomic
{
    ///<summary>
    ///this goal makes the bot dodge from side to side
    ///</summary>
    public class Strafe : Goal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///class for the atomic goal Strafe
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        public Strafe(BotEntity bot)
            : base(bot, GoalTypes.Strafe)
        {
            _clockwise = RandomUtil.RandomBool();
        }

        #endregion

        #region Public methods

        //------------------------------- Activate ------------------------------------
        //-----------------------------------------------------------------------------
        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogDodgeSideToSide(
                LogPrefixText + LogActivateText + LogStatusText);
            Status = StatusTypes.Active;
            LogUtil.WriteLineIfLogDodgeSideToSide(
                LogPrefixText + LogActivateText + LogStatusText);

            Bot.Steering.SeekIsOn = true;

            if (_clockwise)
            {
                if (Bot.CanStepRight(out _strafeTarget))
                {
                    LogUtil.WriteLineIfLogDodgeSideToSide(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " StepRight: " + _strafeTarget);
                    Bot.Steering.Target = _strafeTarget;
                }
                else
                {
                    Status = StatusTypes.Inactive;
                    _clockwise = !_clockwise;
                    LogUtil.WriteLineIfLogDodgeSideToSide(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " ChangeDodgeDirection: left");
                }
            }
            else
            {
                if (Bot.CanStepLeft(out _strafeTarget))
                {
                    LogUtil.WriteLineIfLogDodgeSideToSide(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " StepLeft: " + _strafeTarget);
                    Bot.Steering.Target = _strafeTarget;
                }
                else
                {
                    Status = StatusTypes.Inactive;
                    _clockwise = !_clockwise;
                    LogUtil.WriteLineIfLogDodgeSideToSide(
                        LogPrefixText + LogActivateText + LogStatusText +
                        " ChangeDodgeDirection: right");
                }
            }
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogDodgeSideToSide(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");
            //if status is inactive, call Activate()
            ActivateIfInactive();
            LogUtil.WriteLineIfLogDodgeSideToSide(
                LogPrefixText + LogProcessText + LogStatusText);

            //if target goes out of view terminate
            if (!Bot.TargetingSystem.IsTargetWithinFOV)
            {
                Status = StatusTypes.Completed;
                LogUtil.WriteLineIfLogDodgeSideToSide(
                    LogPrefixText + LogProcessText + LogStatusText +
                    " Target within FOV");
            }
                //else if bot reaches the target position set status to inactive so
                //the goal is reactivated on the next update-step
            else if (Bot.IsAtPosition(_strafeTarget))
            {
                Status = StatusTypes.Inactive;
                LogUtil.WriteLineIfLogDodgeSideToSide(
                    LogPrefixText + LogProcessText + LogStatusText +
                    "At position");
            }

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogDodgeSideToSide(
                LogPrefixText + LogProcessText + LogStatusText);
            Bot.Steering.SeekIsOn = false;
        }

        ///<summary>
        ///
        ///</summary>
        public override void Render()
        {
            base.Render();

            DrawUtil.Line(Bot.Position, _strafeTarget, Color.Orange);
            DrawUtil.Circle(_strafeTarget, 3, Color.Orange, 20);
        }

        ///<summary>
        ///Used to render debugging info if DEBUG is defined.
        ///</summary>
        public override void DebugRender()
        {
            DebugRenderIfStrafe();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_STRAFE
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_STRAFE")]
        public void DebugRenderIfStrafe()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private bool _clockwise;
        private Vector2 _strafeTarget;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}