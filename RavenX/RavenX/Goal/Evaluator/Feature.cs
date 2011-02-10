#region File description

//------------------------------------------------------------------------------
//Feature.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Armory;
using Mindcrafters.RavenX.Entity.Bot;

    #endregion

    #region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Evaluator
{
    ///<summary>
    ///class that implements methods to extract feature specific
    ///information from the Raven game world and present it as 
    ///a value in the range 0 to 1
    ///</summary>
    public class Feature
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///returns a value between 0 and 1 based on the bot's closeness to the 
        ///given item. the further the item, the higher the rating. If there is no
        ///item of the given type present in the game world at the time this method
        ///is called the value returned is 1
        ///</summary>
        public static float DistanceToItem(BotEntity bot, ItemTypes itemType)
        {
            //determine the distance to the closest instance of the item type
            float distanceToItem = bot.PathPlanner.GetCostToClosestItem(itemType);

            //if the previous method returns a negative value then there is no item of
            //the specified type present in the game world at this time.
            if (distanceToItem < 0)
                return 1;

            //these values represent cutoffs. Any distance over maxDistance results in
            //a value of 0, and value below minDistance results in a value of 1
            const float maxDistance = 500.0f;
            const float minDistance = 50.0f;

            distanceToItem = MathHelper.Clamp(distanceToItem, minDistance, maxDistance);

            return distanceToItem/maxDistance;
        }

        //returns a value between 0 and 1 based on how much ammo the bot has for
        //the given weapon, and the maximum amount of ammo the bot can carry. The
        //closer the amount carried is to the max amount, the higher the score
        public static float IndividualWeaponStrength(BotEntity bot, WeaponTypes weaponType)
        {
            //grab a pointer to the gun (if the bot owns an instance)
            Weapon wp = bot.WeaponSystem.GetWeaponFromInventory(weaponType);

            if (wp != null)
            {
                return wp.NumRoundsRemaining/
                       GetMaxRoundsBotCanCarryForWeapon(weaponType);
            }
            return 0.0f;
        }

        //returns a value between 0 and 1 based on the total amount of ammo the
        //bot is carrying each of the weapons. Each of the three weapons a bot can
        //pick up can contribute a third to the score. In other words, if a bot
        //is carrying a RL and a RG and has max ammo for the RG but only half max
        //for the RL the rating will be 1/3 + 1/6 + 0 = 0.5
        public static float TotalWeaponStrength(BotEntity bot)
        {
            float maxRoundsForShotgun =
                GetMaxRoundsBotCanCarryForWeapon(
                    WeaponTypes.Shotgun);
            float maxRoundsForRailgun =
                GetMaxRoundsBotCanCarryForWeapon(
                    WeaponTypes.Railgun);
            float maxRoundsForRocketLauncher =
                GetMaxRoundsBotCanCarryForWeapon(
                    WeaponTypes.RocketLauncher);
            float totalRoundsCarryable =
                maxRoundsForShotgun + maxRoundsForRailgun + maxRoundsForRocketLauncher;

            float numSlugs =
                bot.WeaponSystem.GetAmmoRemaining(
                    WeaponTypes.Railgun);
            float numCartridges =
                bot.WeaponSystem.GetAmmoRemaining(
                    WeaponTypes.Shotgun);
            float numRockets =
                bot.WeaponSystem.GetAmmoRemaining(
                    WeaponTypes.RocketLauncher);

            //the value of the tweaker (must be in the range 0-1) indicates how much
            //desirability value is returned even if a bot has not picked up any weapons.
            //(it basically adds in an amount for a bot's persistent weapon -- the blaster)
            const float tweaker = 0.05f;

            return tweaker + (1 - tweaker)*(numSlugs + numCartridges + numRockets)/
                             totalRoundsCarryable;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// helper function to tidy up IndividualWeapon method
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns>maximum rounds of ammo a bot can carry for the given weapon</returns>
        private static float GetMaxRoundsBotCanCarryForWeapon(WeaponTypes weaponType)
        {
            switch (weaponType)
            {
                case WeaponTypes.Railgun:
                    return GameManager.GameManager.Instance.Parameters.RailgunMaxRoundsCarried;

                case WeaponTypes.RocketLauncher:
                    return GameManager.GameManager.Instance.Parameters.RocketLauncherMaxRoundsCarried;

                case WeaponTypes.Shotgun:
                    return GameManager.GameManager.Instance.Parameters.ShotgunMaxRoundsCarried;

                default:
                    throw new ApplicationException("trying to calculate  of unknown weapon");
            } //end switch
        }

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion
    }
}