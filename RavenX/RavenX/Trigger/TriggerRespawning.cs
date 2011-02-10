#region File description

//------------------------------------------------------------------------------
//TriggerRespawning.cs
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

using Mindcrafters.RavenX.Entity;

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///base class to create a trigger that is capable of respawning
    ///after a period of inactivity
    ///</summary>
    public abstract class TriggerRespawning : Trigger
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///Constructor for the base of a respawning trigger
        ///</summary>
        ///<param name="sceneObject"></param>
        protected TriggerRespawning(IEntitySceneObject sceneObject)
            : base(sceneObject)
        {
            _timeBetweenRespawns = 0;
            _timeUntilRespawn = 0;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //When a bot comes within this trigger's area of influence it is triggered
        //but then becomes inactive for a specified amount of time. These values
        //control the amount of time required to pass before the trigger becomes 
        //active once more.

        ///<summary>
        ///The number of seconds the trigger is inactive before respawning
        ///</summary>
        public float TmeBetweenRespawns
        {
            get { return _timeBetweenRespawns; }
            protected set { _timeBetweenRespawns = value; }
        }

        ///<summary>
        ///The number of seconds remaining until the trigger respawns
        ///</summary>
        public float TimeUntilRespawn
        {
            get { return _timeUntilRespawn; }
        }

        #region Public methods

        ///<summary>
        ///this is called each game-tick to update the trigger's internal state
        ///</summary>
        ///<param name="dt"></param>
        public override void Update(float dt)
        {
            _timeUntilRespawn -= dt;
            if ((_timeUntilRespawn <= 0) && !IsActive)
            {
                IsActive = true;
            }
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///Set the trigger inactive for <see cref="_timeBetweenRespawns"/> 
        ///seconds
        ///</summary>
        protected void Deactivate()
        {
            IsActive = false;
            _timeUntilRespawn = _timeBetweenRespawns;
        }

        #endregion

        #region Private, protected, internal fields

        private float _timeBetweenRespawns;
        private float _timeUntilRespawn;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}