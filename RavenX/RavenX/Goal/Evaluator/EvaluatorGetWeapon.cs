#region File description

//------------------------------------------------------------------------------
//EvaluatorGetWeapon.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using System.Diagnostics;
using GarageGames.Torque.Core;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.Tx2D.GameAI;

    #endregion

    #region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Evaluator
{
    ///<summary>
    ///class to calculate how desirable the goal of fetching a weapon item is
    ///</summary>
    public class EvaluatorGetWeapon : Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public EvaluatorGetWeapon(float characterBias, WeaponTypes weaponType)
            : base(characterBias)
        {
            _weaponType = weaponType;
        }

        #endregion

        #region Public methods

        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        public override float CalculateDesirability(BotEntity bot)
        {
            //grab the distance to the closest instance of the weapon type
            float distance = Feature.DistanceToItem(bot, Entity.Entity.WeaponTypeToItemType(_weaponType));

            //Added by Morteza
            //If the number of rounds we have is less then maximum, instead of picking up another weapon then,
            //Lets grab ammo for the weapon we have
            if (bot.WeaponSystem.CurrentWeapon.NumRoundsRemaining < bot.WeaponSystem.CurrentWeapon.MaxRoundsCarried)
            {
                if (Feature.DistanceToItem(bot, Entity.Entity.WeaponTypeToItemType(_weaponType)) != 1)
                {
                    return 1;
                }
            }

            //if the distance feature is rated with a value of 1 it means that the
            //item is either not present on the map or too far away to be worth 
            //considering, therefore the desirability is zero
            if (distance == 1)
            {
                return 0;
            }
            //value used to tweak the desirability
            float tweaker;
            switch (_weaponType)
            {
                case WeaponTypes.Railgun:
                    tweaker =
                        GameManager.GameManager.Instance.Parameters.BotRailgunGoalTweaker;
                    break;
                case WeaponTypes.RocketLauncher:
                    tweaker =
                        GameManager.GameManager.Instance.Parameters.BotRocketLauncherGoalTweaker;
                    break;
                case WeaponTypes.Shotgun:
                    tweaker =
                        GameManager.GameManager.Instance.Parameters.BotShotgunGoalTweaker;
                    break;
                default:
                    tweaker = 1.0f;
                    break;
            }

            float health = bot.HealthPercentage;

            float weaponStrength = Feature.IndividualWeaponStrength(bot, _weaponType);

            float desirability = (tweaker * health * (1 - weaponStrength) * bot.WeaponSystem.CurrentWeapon.PercentageRoundsLeft) / distance;

            //ensure the value is in the range 0 to 1
            desirability = MathHelper.Clamp(desirability, 0, 1);

            desirability *= _characterBias;

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            bot.Brain.AddGoalGetItem(Entity.Entity.WeaponTypeToItemType(_weaponType));
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
            string s;
            switch (_weaponType)
            {
                case WeaponTypes.Railgun:
                    s = "RG: ";
                    break;
                case WeaponTypes.RocketLauncher:
                    s = "RL: ";
                    break;
                case WeaponTypes.Shotgun:
                    s = "SG: ";
                    break;
                default:
                    Assert.Fatal(false, "EvaluatorGetWeapon.RenderInfo: unexpected weapon type");
                    s = "  : "; //shouldn't happen
                    break;
            }

            TextUtil.DrawText(position, s + CalculateDesirability(bot).ToString("F2"));
        }

        public override string ToString()
        {
            return base.ToString() + "(" + _weaponType + ")";
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly WeaponTypes _weaponType;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}