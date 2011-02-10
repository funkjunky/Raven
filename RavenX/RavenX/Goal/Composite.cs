#region File description

//------------------------------------------------------------------------------
//CompositeGoal.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using System.Text;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Goal.Atomic;
using Mindcrafters.RavenX.Messaging;
using Mindcrafters.RavenX.Navigation;

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
    ///Base composite goal class
    ///</summary>
    public abstract class CompositeGoal : Goal
    {
        #region Static methods, fields, constructors

        private static readonly string _logForwardMsgText =
            String.Format("{0,-23}] ", "ForwardMsg");

        ///<summary>
        ///log text for <see cref="ForwardMessageToFrontMostSubgoal"/> method
        ///</summary>
        protected static string LogForwardMsgText
        {
            get { return _logForwardMsgText; }
        }

        ///<summary>
        ///Generate a string representation of a path
        ///</summary>
        ///<returns></returns>
        protected static string PathToString(List<PathEdge> path)
        {
            StringBuilder pathText = new StringBuilder();
            int i = 0;
            foreach (PathEdge curEdge in path)
            {
                pathText.AppendLine();
                pathText.AppendFormat("{0}: {1}", i, EdgeToString(curEdge));
                i++;
            }
            return pathText.ToString();
        }

        #endregion

        #region Constructors

        ///<summary>
        ///Base class for composite goals
        ///</summary>
        ///<param name="bot">Bot that owns this goal</param>
        ///<param name="goalType">The type of this goal</param>
        protected CompositeGoal(BotEntity bot, GoalTypes goalType)
            : base(bot, goalType)
        {
            _subgoals = new Stack<Goal>();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///The list of subgoals of this composite goal
        ///</summary>
        public Stack<Goal> Subgoals
        {
            get { return _subgoals; }
        }

        #region Public methods

        ///<summary>
        ///logic to run when the goal is activated.
        ///</summary>
        public abstract override void Activate();

        ///<summary>
        ///logic to run each update-step.
        ///</summary>
        ///<returns></returns>
        public abstract override StatusTypes Process();

        ///<summary>
        ///logic to run prior to the goal's destruction
        ///</summary>
        public abstract override void Terminate();

        ///<summary>
        ///if a child class of Composite does not define a message handler the
        ///default behavior is to forward the message to the front-most subgoal
        ///</summary>
        ///<param name="msg"></param>
        ///<returns></returns>
        public override bool HandleMessage(Telegram msg)
        {
            return ForwardMessageToFrontMostSubgoal(msg);
        }

        ///<summary>
        ///adds a subgoal to the front of the subgoal list
        ///</summary>
        ///<param name="goal"></param>
        public override void AddSubgoal(Goal goal)
        {
            Subgoals.Push(goal);
        }

        ///<summary>
        ///remove and terminate all subgoals of this composite goal
        ///</summary>
        public void RemoveAllSubgoals()
        {
            foreach (Goal goal in Subgoals)
            {
                goal.Terminate();
            }
            Subgoals.Clear();
        }

        ///<summary>
        ///passes the message to the goal at the front of the queue
        ///</summary>
        ///<param name="msg"></param>
        ///<returns>false if the message has not been handled</returns>
        public bool ForwardMessageToFrontMostSubgoal(Telegram msg)
        {
            if (Subgoals.Count > 0)
            {
                LogUtil.WriteLineIfVerbose(LogPrefixText + LogForwardMsgText +
                                           LogStatusText +
                                           " {" + msg.DispatchTime + ", " +
                                           msg.Sender + ", " +
                                           msg.Receiver + ", " +
                                           msg.Msg + "} --> Subgoal: " +
                                           EnumUtil.GetDescription(Subgoals.Peek().GoalType));
            }

            return Subgoals.Count > 0 && Subgoals.Peek().HandleMessage(msg);
        }

        ///<summary>
        ///render the first subgoal of this composite goal
        ///</summary>
        public override void Render()
        {
            base.Render();

            if (Subgoals.Count > 0)
            {
                Subgoals.Peek().Render();
            }
        }

        ///<summary>
        ///this is used to draw the name of the goal at the specific position
        ///and to draw (with indent) the subgoals of this composite goal.
        ///used for debugging
        ///</summary>
        ///<param name="pos"></param>
        public override void RenderAtPos(ref Vector2 pos)
        {
            base.RenderAtPos(ref pos);

            pos.X += 10;

            foreach (Goal curGoal in Subgoals)
            {
                curGoal.RenderAtPos(ref pos);
            }

            pos.X -= 10;
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///this method first removes any completed goals from the front of the
        ///subgoal list. It then processes the next goal in the list (if there
        ///is one)
        ///</summary>
        ///<returns></returns>
        protected StatusTypes ProcessSubgoals()
        {
            //remove all completed and failed goals from the front of the
            //subgoal list
            while (Subgoals.Count > 0 &&
                   (Subgoals.Peek().IsComplete || Subgoals.Peek().HasFailed))
            {
                Subgoals.Peek().Terminate();
                Subgoals.Pop();
            }

            //if any subgoals remain, process the one at the front of the list
            if (Subgoals.Count > 0)
            {
                //grab the status of the front-most subgoal
                StatusTypes statusOfSubGoals = Subgoals.Peek().Process();

                //we have to test for the special case where the front-most
                //subgoal reports 'completed' *and* the subgoal list contains
                //additional goals. When this is the case, to ensure the parent
                //keeps processing its subgoal list we must return the 'active'
                //status.
                if (statusOfSubGoals == StatusTypes.Completed &&
                    Subgoals.Count > 1)
                {
                    return StatusTypes.Active;
                }

                return statusOfSubGoals;
            }

            //no more subgoals to process - return 'completed'
            return StatusTypes.Completed;
        }

        ///<summary>
        ///Generate a string representation of the subgoals
        ///</summary>
        ///<returns></returns>
        protected string SubgoalsToString()
        {
            StringBuilder subgoalsText = new StringBuilder();
            subgoalsText.AppendFormat(" {0}.Subgoals = {{", GoalType);
            int i = Subgoals.Count;
            foreach (Goal sg in Subgoals)
            {
                subgoalsText.Append(sg.GoalType);
                if (sg.GoalType == GoalTypes.TraverseEdge)
                {
                    subgoalsText.AppendFormat("[{0}]",
                                              EdgeToString((sg as TraverseEdge).EdgeToFollow));
                }
                if (i > 1) subgoalsText.Append(", ");
                i--;
            }
            subgoalsText.Append("}");
            return subgoalsText.ToString();
        }

        ///<summary>
        ///Generate a string representation of a path
        ///</summary>
        ///<returns></returns>
        protected string PathToString()
        {
            return PathToString(Bot.PathPlanner.GetPath());
        }

        #endregion

        #region Private, protected, internal fields

        private readonly Stack<Goal> _subgoals;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}