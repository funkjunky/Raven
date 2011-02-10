#region File description

//------------------------------------------------------------------------------
//EvaluatorExplore.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using System.Diagnostics;
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
    ///class to calculate how desirable the goal of exploring is
    ///</summary>
    public class EvaluatorExplore : Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public EvaluatorExplore(float characterBias)
            : base(characterBias)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This checks to see if we need to explore to find weapons or health
        /// Once we know where at least one health pack is,
        /// and one weapon, we really have reason to explore anymore
        /// </summary>
        /// <param name="bot">bot to evaluate for</param>
        /// <returns></returns>
        public override float CalculateDesirability(BotEntity bot)
        {
            //figure out desirability of exploring for health
            //if we are unhealthy and we don't know where any health is
            float howBadIWantToExploreForHealth = (1 - bot.HealthPercentage)*(bot.FoundTriggers.NumberOfHealthItems > 1
                                                                                  ?
                                                                                      1
                                                                                  :
                                                                                      0);
            //figure out desirability of exploring for the weapon we are holding
            //if we are unloaded and we don't know where any of the weapon is
            //by default the bots walk around carrying their blaster...
            float howBadIWantToExploreForAWeapon;
            if(bot.WeaponSystem.CurrentWeapon.WeaponType == WeaponTypes.Blaster)
            {
                howBadIWantToExploreForAWeapon = GameManager.GameManager.Instance.Parameters.PropensityToSearchForAWeapon;
            }
            else
            {
                howBadIWantToExploreForAWeapon = (1 - bot.WeaponSystem.CurrentWeapon.PercentageRoundsLeft) *
                                                       (bot.FoundTriggers.NumberOfWeaponsByType(bot.WeaponSystem.CurrentWeapon.WeaponType) < 1
                                                            ?
                                                                1
                                                            :
                                                                0);
            }
            //get the maximum desire to explore for something
            float desirability = MathHelper.Max(howBadIWantToExploreForHealth, howBadIWantToExploreForAWeapon);

            //ensure the value is in the range 0 to 1
            desirability = MathHelper.Clamp(desirability, 0.0f, 1.0f);

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            bot.Brain.AddGoalExplore();

            //GoalComponent gc = 
            //   bc.Components.FindComponent<GoalComponent>();
            //if (gc != null)
            //{
            //   if (gc.Goal != null)
            //   {
            //       Think t = gc.Goal as Think;
            //       if (t != null)
            //       {
            //           t.AddGoalExplore();
            //       }
            //   }
            //}
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
            TextUtil.DrawText(position, "EX: " + CalculateDesirability(bot).ToString("2"));
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