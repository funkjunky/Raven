#region File description

//------------------------------------------------------------------------------
//TriggerSystem.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Mindcrafters.RavenX.Entity.Bot;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///Class to manage a collection of triggers. Triggers may be
    ///registered with an instance of this class. The instance then 
    ///takes care of updating those triggers and of removing them from
    ///the system if their lifetime has expired.
    ///</summary>
    public class TriggerSystem
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///list of triggers in the game
        ///</summary>
        public List<Trigger> Triggers
        {
            get { return _triggers; }
        }

        #region Public methods

        ///<summary>
        ///this deletes any current triggers and empties the trigger list
        ///</summary>
        public void Clear()
        {
            Triggers.Clear();
        }

        ///<summary>
        ///This method should be called each update-step of the game. It will
        ///first update the internal state of the triggers and then try each
        ///entity against each active trigger to test if any are triggered.
        ///</summary>
        ///<param name="dt">elapsed time since last update</param>
        ///<param name="entities">list of entities to test</param>
        public void Update(float dt, List<BotEntity> entities)
        {
            UpdateTriggers(dt);
            TryTriggers(entities);
        }

        ///<summary>
        ///this is used to register triggers with the <see cref="TriggerSystem"/>
        ///which will take care of tidying up memory used by a trigger
        ///</summary>
        ///<param name="trigger"></param>
        public void Register(Trigger trigger)
        {
            Triggers.Add(trigger);
        }

        ///<summary>
        ///some triggers must be rendered (like giver-triggers for example)
        ///</summary>
        public void Render()
        {
            foreach (Trigger curTrigger in Triggers)
            {
                curTrigger.Render();
            }
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///this method iterates through all the triggers present in the system
        ///and calls their Update method in order that their internal state can
        ///be updated if necessary. It also removes any triggers from the system
        ///that have their MarkForDelete field set to true. 
        ///</summary>
        ///<param name="dt">elapsed time since last update</param>
        private void UpdateTriggers(float dt)
        {
            int i = 0;
            while (i < Triggers.Count)
            {
                //remove trigger if dead
                if (Triggers[i].MarkForDelete)
                {
                    Triggers.RemoveAt(i);
                }
                else
                {
                    //update this trigger
                    Triggers[i].Update(dt);
                    ++i;
                }
            }
        }

        ///<summary>
        ///this method iterates through the list of entities passed as a
        ///parameter and passes each one to the Try method of each trigger
        ///*provided* the entity is alive and provided the entity is ready
        ///for a trigger update.
        ///</summary>
        ///<param name="entities">list of entities to try</param>
        private void TryTriggers(IEnumerable<BotEntity> entities)
        {
            //test each entity against the triggers
            foreach (BotEntity curEnt in entities)
            {
                //an entity must be ready for its next trigger update and it
                //must be alive before it is tested against each trigger.
                if (!curEnt.IsReadyForTriggerUpdate() || !curEnt.IsAlive)
                    continue;

                foreach (Trigger curTrigger in Triggers)
                {
                    curTrigger.Try(curEnt);
                }
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly List<Trigger> _triggers = new List<Trigger>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}