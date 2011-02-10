#region File description

//------------------------------------------------------------------------------
//EvaluatorAttackTarget.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;

#endregion

#region Microsoft

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
    ///class to calculate how desirable the goal of attacking the bot's
    ///current target is
    ///</summary>
    public class EvaluatorAttackTarget : Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public EvaluatorAttackTarget(float characterBias)
            : base(characterBias)
        {
        }

        #endregion

        #region Public methods

        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        public override float CalculateDesirability(BotEntity bot)
        {
            float desirability = 0.0f;

            //only do the calculation if there is a target present
            if (bot.TargetingSystem.IsTargetPresent)
            {
                float tweaker =
                    GameManager.GameManager.Instance.Parameters.BotAggroGoalTweaker;

                int teammatesTargetingBot = bot.SensoryMemory.GetListOfRecentlySensedTeammates().FindAll(delegate(BotEntity teammate) { return teammate.TargetBot == bot.TargetBot; }).Count;

                const int maxBotsForMaxDesirability = 16;
                //f(x) = max((16 + 1) - (x-16)^2/16, 0)
                //in effect x=0 [no bots targeting the bot] means f(x) = 1
                //x = 16, f(x) = 17
                //x = 32, f(x) = 1
                //x = 33, f(x) = 0
                float teammatesTargetingBotMultiplier = (float)Math.Max(
                                                        (maxBotsForMaxDesirability + 1) - Math.Pow(teammatesTargetingBot - maxBotsForMaxDesirability, 2) / maxBotsForMaxDesirability,
                                                        1.0f);

                desirability = tweaker*
                               bot.HealthPercentage*
                               Feature.TotalWeaponStrength(bot)*
                               teammatesTargetingBotMultiplier;

                //bias the value according to the personality of the bot
                desirability *= _characterBias;
            }

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            bot.Brain.AddGoalAttackTarget();
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
            TextUtil.DrawText(position, "AT: " + CalculateDesirability(bot).ToString("F2"));

            return;

            //string s = 
            //   Feature.Health(bot)).ToString("F2") + 
            //       ", " + 
            //       Feature.TotalWeaponStrength(bot).ToString("F2");
            //TextUtil.DrawText(position+Vector2(0,12), s);
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