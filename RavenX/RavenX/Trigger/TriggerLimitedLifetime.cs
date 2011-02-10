#region File description

//------------------------------------------------------------------------------
//TriggerLimitedLifetime.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsof

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

using Mindcrafters.RavenX.Entity;

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///defines a trigger that only remains in the game for a specified
    ///number of update steps
    ///</summary>
    public abstract class TriggerLimitedLifetime : Trigger
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="sceneObject"></param>
        ///<param name="lifetime"></param>
        protected TriggerLimitedLifetime(
            IEntitySceneObject sceneObject,
            int lifetime)
            : base(sceneObject)
        {
            Lifetime = lifetime;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the lifetime of this trigger in update-steps
        ///TODO: modify to use time instead of steps
        ///</summary>
        public int Lifetime
        {
            get { return _lifetime; }
            protected set { _lifetime = value; }
        }

        #region Public methods

        ///<summary>
        ///called each update-step of the game. This methods updates any
        ///internal state the trigger may have. Children of this class should
        ///call this from within their own update method
        ///</summary>
        ///<param name="dt"></param>
        public override void Update(float dt)
        {
            //if the lifetime counter expires set this trigger to be removed
            //from the game
            if (--Lifetime <= 0)
            {
                MarkForDelete = true;
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private int _lifetime;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}