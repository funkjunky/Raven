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
using Mindcrafters.RavenX.Goal.Atomic;
#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Composite
{
    class LeadFormation : CompositeGoal
    {
        public LeadFormation(BotEntity bot, Formation groupFormation)
            :base(bot, GoalTypes.LeadFormation)
        {
            GroupFormation = groupFormation;
        }

        public override void Activate()
        {
            //this means we already got a formation from someone else.
            if (GroupFormation != null)
                return;

            RemoveAllSubgoals(); //we want the bot to do nothing.
        }

        public override Goal.StatusTypes Process()
        {
            if (GroupFormation == null)
                return StatusTypes.Failed;

            RemoveAllSubgoals();    //do nothing

            return StatusTypes.Active;
        }

        public override void Terminate()
        {
            Status = StatusTypes.Completed;
        }

        public Formation GroupFormation
        {
            get { return _groupFormation; }
            set { _groupFormation = value; }
        }

        protected Formation _groupFormation;
    }
}
