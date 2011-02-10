#region File description

//------------------------------------------------------------------------------
//NegotiateDoor.cs
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
using Mindcrafters.RavenX.Navigation;

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
    ///class for the composite goal NegotiateDoor
    ///</summary>
    public class NegotiateDoor : CompositeGoal
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for composite goal NegotiateDoor
        ///</summary>
        ///<param name="bot">bot entity that owns this goal</param>
        ///<param name="pathEdge">edge through door</param>
        ///<param name="lastEdgeInPath">true if last edge in path</param>
        public NegotiateDoor(BotEntity bot, PathEdge pathEdge, bool lastEdgeInPath)
            : base(bot, GoalTypes.NegotiateDoor)
        {
            _pathEdge = pathEdge;
            _lastEdgeInPath = lastEdgeInPath;
        }

        #endregion

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public override void Activate()
        {
            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText);

            Status = StatusTypes.Active;

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText);

            //if this goal is reactivated then there may be some existing
            //subgoals that must be removed
            RemoveAllSubgoals();

            //get the position of the closest navigable switch
            Vector2 posSw =
                GameManager.GameManager.Instance.GetPosOfClosestSwitch(
                    Bot.Position, _pathEdge.DoorId);

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText +
                " Door Id: " + _pathEdge.DoorId +
                " using switch at position: " + posSw);

            //because goals are *pushed* onto the front of the subgoal list
            //they must be added in reverse order.

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText + LogAddSubgoalText +
                "TraverseEdge: " + _pathEdge.Source + "-->" +
                _pathEdge.Destination);
            //first the goal to traverse the edge that passes through the door
            AddSubgoal(new TraverseEdge(Bot, _pathEdge, _lastEdgeInPath));

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText + LogAddSubgoalText +
                "MoveToPosition: edge start at: " + _pathEdge.Source);
            //next, the goal that moves the bot to the beginning of the
            //edge that passes through the door
            AddSubgoal(new MoveToPosition(Bot, _pathEdge.Source));

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogActivateText + LogStatusText + LogAddSubgoalText +
                "MoveToPosition: door switch at: " + posSw);
            //finally, the goal that directs the bot to the switch
            AddSubgoal(new MoveToPosition(Bot, posSw));
        }

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public override StatusTypes Process()
        {
            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogProcessText + LogStatusText +
                " ActivateIfInactive");

            //if status is inactive, call Activate()
            ActivateIfInactive();

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogProcessText + LogStatusText +
                " Before processing" + SubgoalsToString());

            //process the subgoals
            Status = ProcessSubgoals();

            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogProcessText + LogStatusText +
                " After processing" + SubgoalsToString());

            return Status;
        }

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public override void Terminate()
        {
            LogUtil.WriteLineIfLogNegotiateDoor(
                LogPrefixText + LogTerminateText + LogStatusText);
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
            DebugRenderIfNegotiateDoor();
        }

        ///<summary>
        ///Used to render debugging info if DEBUG and DEBUG_NEGOATIATE_DOOR
        ///is defined.
        ///</summary>
        [Conditional("DEBUG_NEGOATIATE_DOOR")]
        public void DebugRenderIfNegotiateDoor()
        {
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly bool _lastEdgeInPath;
        private readonly PathEdge _pathEdge;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}