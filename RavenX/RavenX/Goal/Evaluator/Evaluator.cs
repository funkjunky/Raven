#region File description

//------------------------------------------------------------------------------
//Evaluator.cs
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

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Goal.Evaluator
{
    ///<summary>
    ///class template that defines an interface for objects that are
    ///able to evaluate the desirability of a specific strategy level goal
    ///</summary>
    public abstract class Evaluator
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        protected Evaluator(float characterBias)
        {
            _characterBias = characterBias;
        }

        #endregion

        #region Public methods

        //returns a score between 0 and 1 representing the desirability of the
        //strategy the concrete subclass represents
        public abstract float CalculateDesirability(BotEntity bc);

        //adds the appropriate goal to the given bot's brain
        public abstract void SetGoal(BotEntity bc);

        //used to provide debugging/tweaking support
        public abstract void RenderInfo(Vector2 position, BotEntity bot);

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //when the desirability score for a goal has been evaluated it is multiplied 
        //by this value. It can be used to create bots with preferences based upon
        //their personality
        protected float _characterBias;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}