#region File description

//------------------------------------------------------------------------------
//AdjustRange.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

using System.Diagnostics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Atomic
{
    ///<summary>
    ///class for the atomic goal AdjustRange
    ///TODO: this class is unfinished (and currently unused)
    ///</summary>
    public class AdjustRange : Goal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for atomic goal AdjustRange
        ///</summary>
        ///<param name="bot"></param>
        public AdjustRange(BotEntity bot)
            : base(bot, GoalTypes.AdjustRange)
        {
            _idealRange = 0;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///
        ///</summary>
        public float IdealRange
        {
            get { return _idealRange; }
        }

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogAdjustRange(
                LogPrefixText + LogActivateText + LogStatusText +
                " Target: " + Bot.TargetBot.Position);

            Bot.Steering.Target = Bot.TargetBot.Position;
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogAdjustRange(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive" + " Target: " + Bot.TargetBot.Position);
            //if status is inactive, call Activate()
            ActivateIfInactive();
            LogUtil.WriteLineIfLogAdjustRange(
                LogPrefixText + LogProcessText + LogStatusText +
                " Target: " + Bot.TargetBot.Position);

            //TODO: define IsInIdealWeaponRange for each weapon
            //if (Bot.WeaponSystem.CurrentWeapon.IsInIdealWeaponRange())
            //{
            //   Status = StatusTypes.Completed;
            //}

            LogUtil.WriteLineIfLogAdjustRange(
                LogPrefixText + LogProcessText + LogStatusText +
                " Target: " + Bot.TargetBot.Position);

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogAdjustRange(
                LogPrefixText + LogTerminateText + LogStatusText +
                " Target: " + Bot.TargetBot.Position);
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
            DebugRenderIfAdjustRanger();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_ADJUST_RANGE
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_ADJUST_RANGE")]
        public void DebugRenderIfAdjustRanger()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly float _idealRange;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}