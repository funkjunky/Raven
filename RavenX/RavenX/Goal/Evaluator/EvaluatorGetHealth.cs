#region File description

//------------------------------------------------------------------------------
//EvaluatorGetHealth.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

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
    ///class to calculate how desirable the goal of fetching a health item is
    ///</summary>
    public class EvaluatorGetHealth : Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public EvaluatorGetHealth(float characterBias)
            : base(characterBias)
        {
        }

        #endregion

        #region Public methods

        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        public override float CalculateDesirability(BotEntity bot)
        {
            //first grab the distance to the closest instance of a health item
            float distance = Feature.DistanceToItem(bot, ItemTypes.Health);

            //if the distance feature is rated with a value of 1 it means that the
            //item is either not present on the map or too far away to be worth 
            //considering, therefore the desirability is zero
            if (distance == 1)
            {
                return 0;
            }

            //the desirability of finding a health item is proportional to the amount
            //of health remaining and inversely proportional to the distance from the
            //nearest instance of a health item.
            float desirability =
                GameManager.GameManager.Instance.Parameters.BotHealthGoalTweaker*
                (1 - bot.HealthPercentage)/
                (Feature.DistanceToItem(bot, ItemTypes.Health));

            //ensure the value is in the range 0 to 1
            desirability = MathHelper.Clamp(desirability, 0, 1);

            //bias the value according to the personality of the bot
            desirability *= _characterBias;

            return desirability;
        }

        //adds the appropriate goal to the given bot's brain
        public override void SetGoal(BotEntity bot)
        {
            bot.Brain.AddGoalGetItem(ItemTypes.Health);
        }

        public override void RenderInfo(Vector2 position, BotEntity bot)
        {
            TextUtil.DrawText(position, "H: " + CalculateDesirability(bot).ToString("F2"));
            return;

            //string s =
            //   (1 - Feature.Health(bot)).ToString("F2") +
            //   ", " +
            //   Feature.DistanceToItem(bot, ItemTypes.Health).ToString("F2");
            //TextUtil.DrawText(position + new Vector2(0, 15), s);
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