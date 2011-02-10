#region File description

//------------------------------------------------------------------------------
//TriggerWeaponGiver.cs
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
    ///This trigger 'gives' the triggering bot a weapon of the specified type 
    ///</summary>
    public class TriggerWeaponGiver : TriggerRespawning
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="railgunData"></param>
        public TriggerWeaponGiver(RailgunData railgunData)
            : base(new RailgunSceneObject())
        {
            Name = railgunData.Name;
            EntityType = EntityTypes.Railgun;
            Position = railgunData.Position;
            BoundingRadius = railgunData.Radius;
            NodeIndex = railgunData.NodeIndex;

            //create this trigger's region of influence
            AddCircularTriggerRegion(
                Position,
                GameManager.GameManager.Instance.Parameters.DefaultGiverTriggerRange);

            TmeBetweenRespawns =
                GameManager.GameManager.Instance.Parameters.WeaponRespawnDelay;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="rocketLauncherData"></param>
        public TriggerWeaponGiver(RocketLauncherData rocketLauncherData)
            : base(new RocketLauncherSceneObject())
        {
            Name = rocketLauncherData.Name;
            EntityType = EntityTypes.RocketLauncher;
            Position = rocketLauncherData.Position;
            BoundingRadius = rocketLauncherData.Radius;
            NodeIndex = rocketLauncherData.NodeIndex;

            //create this trigger's region of influence
            AddCircularTriggerRegion(
                Position,
                GameManager.GameManager.Instance.Parameters.DefaultGiverTriggerRange);

            TmeBetweenRespawns =
                GameManager.GameManager.Instance.Parameters.WeaponRespawnDelay;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="shotgunData"></param>
        public TriggerWeaponGiver(ShotgunData shotgunData)
            : base(new ShotgunSceneObject())
        {
            Name = shotgunData.Name;
            EntityType = EntityTypes.Shotgun;
            Position = shotgunData.Position;
            BoundingRadius = shotgunData.Radius;
            NodeIndex = shotgunData.NodeIndex;

            //create this trigger's region of influence
            AddCircularTriggerRegion(
                Position,
                GameManager.GameManager.Instance.Parameters.DefaultGiverTriggerRange);

            TmeBetweenRespawns =
                GameManager.GameManager.Instance.Parameters.WeaponRespawnDelay;
        }

        #endregion

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

            bot.WeaponSystem.AddWeapon(EntityTypeToWeaponType(EntityType));

            Deactivate();
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