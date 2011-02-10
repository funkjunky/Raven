#region File description

//------------------------------------------------------------------------------
//SeekToPosition.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Diagnostics;
#endregion

#region Microsoft
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

#region GarageGames
using GarageGames.Torque.GUI;
#endregion

#region Mindcrafters
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Memory.NavigationalMemory;
#endregion

#endregion


namespace Mindcrafters.RavenX.Goal.Atomic
{
    class SeekToTargetOffset : Goal
    {
        const float SATISFACTIONRADIUS = 1.0f;

        public SeekToTargetOffset(BotEntity bot, BotEntity target, Vector2 offset)
            : base(bot, GoalTypes.SeekToPosition)
        {
            _target = target;
            _offset = offset;
        }

        public BotEntity Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        BotEntity _target;
        Vector2 _offset;

        public override void Activate()
        {
            Status = StatusTypes.Active;

            Bot.Steering.Target = Target.Position + Offset;

            Bot.Steering.SeekIsOn = true;

        }

        public override Goal.StatusTypes Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //TODO: test to see if the bot has become stuck
            Bot.Steering.Target = Target.Position + Offset;

            //we have to take all of them and then whe we have all of them set the leader to leading, so that he can take all their evals and decide what to do for them.
            //if the bot has reached it's destination...
            //if(Bot.Position == Target.Position + Offset && Target.FormationStatus != FormationStatuses.Leading)


            return Status;
        }

        public override void Terminate()
        {
            throw new System.Exception("The method or operation is not implemented.");
        }
    }
}
