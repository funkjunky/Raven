#region File description

//------------------------------------------------------------------------------
//EvaluatorAttackTarget.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

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
    public class EvaluatorCaptureFlag : Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public EvaluatorCaptureFlag(float characterBias)
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

            //there is no point in trying to capture their flag when we don't even know where
            //to return it to
            if (bot.FoundTriggers.NumberOfFlags > 0 && bot.FoundTriggers.IsOurFlagDiscovered)
            {
                //if we are carrying a flag, we should really try to return it to our base
                if (bot.IsCarryingFlag)
                    desirability = 2;
                else
                {
                    //if we have decent health, good ammo, and we are a high ranking officer
                    //we want to get their flag, otherwise we will tend to guard
                    desirability = bot.HealthPercentage *
                                   bot.WeaponSystem.CurrentWeapon.PercentageRoundsLeft*
                                   3/(2-(int)bot.Rank);

                    desirability = MathHelper.Clamp(desirability, 0, 1);
                    //bias the value according to the personality of the bot
                    desirability *= _characterBias;
                }
            }

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            bot.Brain.AddGoalCaptureFlag();
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
            TextUtil.DrawText(position, "AT: " + CalculateDesirability(bot).ToString("F2"));

            return;
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