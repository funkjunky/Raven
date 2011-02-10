#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Goal.Atomic;

    #endregion

    #region Microsoft

//using Microsoft.Xna.Framework.Graphics;

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Composite
{
    public enum FormationType
    {
        BackToBack,
        Circle,
        Concave,
        Convex,
        Line //BackToBack = 2 people, Circle = phalanx, Concave = vs 1, Convex = aggressive half-phalanx 
    }

    //Circle = initial formation, Concave = If their is only one enemy near., Convex = If majority of enemies are in a 180 degree sweep.

    public class Formation
    {
        protected FormationType _formationType;
        protected BotEntity _leader;
        protected List<BotEntity> _members;
        protected uint _position;

        public Formation(FormationType Type, BotEntity Lead, uint Pos, List<BotEntity> members)
        {
            _formationType = Type;
            _leader = Lead;
            _position = Pos;
            _members = members;
        }

        public Formation(Formation cloner)
        {
            _formationType = cloner.FormationType;
            _leader = cloner.Leader;
            _position = cloner.Position;
            _members = cloner.Members;
        }

        public FormationType FormationType
        {
            get { return _formationType; }
            set { _formationType = value; }
        }

        public BotEntity Leader
        {
            get { return _leader; }
            set { _leader = value; }
        }

        public uint Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public List<BotEntity> Members
        {
            get { return _members; }
            set { _members = value; }
        }

        public int membersReady()
        {
            int count = 1;
            foreach (BotEntity mybot in _members)
            {
                if (mybot.FormationStatus == FormationStatuses.Ready)
                    count++;
            }
            return count;
        }

        public bool allMembersPreparing()
        {
            bool allPreparing = true;
            foreach (BotEntity mybot in _members)
            {
                if (mybot != Leader)
                    allPreparing = (mybot.FormationStatus == FormationStatuses.HeadingToPosition);
            }
            return allPreparing;
        }

        public bool allMembersFollowing()
        {
            bool allFollowing = true;
            foreach (BotEntity mybot in _members)
            {
                if (mybot != Leader)
                    allFollowing = (mybot.FormationStatus == FormationStatuses.Following);
            }
            return allFollowing;
        }

        public Vector2 getOffsetVector()
        {
            Vector2 xVector, yVector;
            switch (FormationType)
            {
                case FormationType.BackToBack:
                    switch (Position)
                    {
                        case 1:
                            return Leader.SceneObject.Position;
                                //this tells the bot to stay where it is. This may be useful so that the other bot isn't spending all it's time chasing the other bot.
                        case 2:
                            xVector = Vector2.UnitX*Leader.SceneObject.Size.Y*
                                      (float) Math.Cos(Leader.SceneObject.Rotation);
                                //size of bot times the cos of rotation. Assuming the rotation starts at positive x.
                            yVector = Vector2.UnitY*Leader.SceneObject.Size.Y*
                                      (float) Math.Sin(Leader.SceneObject.Rotation);
                            return xVector + yVector;

                        default:
                            throw new Exception("No position #" + Position + " exists for BackToBack formation.");
                    }

                    //TODO: add code to figure out positions for circle formation.
                case FormationType.Line: //flip sin and cos for perpindicular
                    xVector = Vector2.UnitX*Leader.SceneObject.Size.Y*(float) Math.Sin(Leader.SceneObject.Rotation);
                        //size of bot times the cos of rotation. Assuming the rotation starts at positive x.
                    yVector = Vector2.UnitY*Leader.SceneObject.Size.Y*(float) Math.Cos(Leader.SceneObject.Rotation);
                        //+pi is so that it is on the bots back, not front.
                    return xVector + yVector*(Position*2.0f); //2 bot widths apart

                default:
                    break;
            }
            throw new Exception("No solution found in function getPositionVector(), in class FollowFormation.cs");
        }
    }

    internal class FollowFormation : CompositeGoal
    {
        private const float SATISFACTIONRADIUS = 2.0f;
        protected Formation _groupFormation;

        public FollowFormation(BotEntity bot, Formation groupFormation)
            : base(bot, GoalTypes.FollowFormation)
        {
            GroupFormation = groupFormation;
        }

        public Formation GroupFormation
        {
            get { return _groupFormation; }
            set { _groupFormation = value; }
        }

        public override void Activate()
        {
            //this means we already got a formation from someone else.
            if (GroupFormation != null)
                return;

            AddSubgoal(new SeekToTargetOffset(Bot, GroupFormation.Leader, GroupFormation.getOffsetVector()));
        }

        public override StatusTypes Process()
        {
            if (GroupFormation == null)
                return StatusTypes.Failed;


            //if(ProcessSubgoals() == StatusTypes.Completed && GroupFormation.Leader.FormationStatus == FormationStatuses.Ready)


            return StatusTypes.Active;
        }

        public override void Terminate()
        {
            Status = StatusTypes.Completed;
        }
    }
}