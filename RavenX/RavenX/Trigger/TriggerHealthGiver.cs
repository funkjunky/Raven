#region File description

//------------------------------------------------------------------------------
//TriggerHealthGiver.cs
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

#endregion

#endregion

using MapContent;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Entity.Items;

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///If a bot runs over an instance of this class its health is increased. 
    ///</summary>
    public class TriggerHealthGiver : TriggerRespawning
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="healthData"></param>
        public TriggerHealthGiver(HealthData healthData)
            : base(new HealthSceneObject())
        {
            //TODO: integrate with Torque
            //SceneObject.CreateWithCollision = false;
            //SceneObject.CreateWithPhysics = false;

            Name = healthData.Name;
            Position = healthData.Position;
            BoundingRadius = healthData.Radius;
            _healthGiven = healthData.HealthGiven;
            NodeIndex = healthData.NodeIndex;

            //create this trigger's region of influence
            AddCircularTriggerRegion(
                Position,
                GameManager.GameManager.Instance.Parameters.DefaultGiverTriggerRange);

            TmeBetweenRespawns =
                GameManager.GameManager.Instance.Parameters.HealthRespawnDelay;
            EntityType = EntityTypes.Health;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the amount of health an entity receives when it runs over this trigger
        ///</summary>
        public int HealthGiven
        {
            get { return _healthGiven; }
        }

        #region Public methods

        ///<summary>
        ///when this is called the trigger determines if the entity is within
        ///the trigger's region of influence. If it is then the trigger will be 
        ///triggered and the appropriate action will be taken.
        ///</summary>
        ///<param name="bot">entity activating the trigger</param>
        public override void Try(BotEntity bot)
        {
            if (!IsActive ||
                !IsTouchingTrigger(bot.Position, bot.BoundingRadius))
                return;

            TriggeringBot = bot;

            bot.IncreaseHealth(HealthGiven);

            Deactivate();
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly int _healthGiven;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}