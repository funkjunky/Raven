#region File description

//------------------------------------------------------------------------------
//Wander.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
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
    ///class for the atomic goal Wander
    ///</summary>
    public class Wander : Goal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for atomic goal Wander
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        public Wander(BotEntity bot)
            : base(bot, GoalTypes.Wander)
        {
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogWander(
                LogPrefixText + LogActivateText + LogStatusText);
            Status = StatusTypes.Active;
            LogUtil.WriteLineIfLogWander(
                LogPrefixText + LogActivateText + LogStatusText);
            Bot.Steering.WanderIsOn = true;
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogWander(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogWander(
                LogPrefixText + LogProcessText + LogStatusText);
            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogWander(
                LogPrefixText + LogTerminateText + LogStatusText);
            Bot.Steering.WanderIsOn = false;
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
            DebugRenderIfWander();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_WANDER
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_WANDER")]
        public void DebugRenderIfWander()
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