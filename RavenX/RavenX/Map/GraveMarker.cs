#region File description

//------------------------------------------------------------------------------
//GraveMarker.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Map
{
    ///<summary>
    ///Class to record and render graves at the site of a bot's death
    ///</summary>
    public class GraveMarker : Entity.Entity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="position"></param>
        ///<param name="facing"></param>
        ///<param name="lifetime"></param>
        ///<param name="entitySceneObject"></param>
        public GraveMarker(
            Vector2 position,
            Vector2 facing,
            float lifetime,
            IEntitySceneObject entitySceneObject)
            : base(entitySceneObject)
        {
            _lifeTime = lifetime;
            _timeCreated = Time.TimeNow;
            Position = position;
            Facing = facing;
            _markForDelete = false;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///mark grave to be deleted from game
        ///</summary>
        public bool MarkForDelete
        {
            get { return _markForDelete; }
        }

        ///<summary>
        ///time grave marker created
        ///</summary>
        public float TimeCreated
        {
            get { return _timeCreated; }
        }

        ///<summary>
        ///how long a grave remains on screen
        ///</summary>
        public float LifeTime
        {
            get { return _lifeTime; }
        }

        #region Public methods

        ///<summary>
        ///Update the grave marker (part of the game update logic)
        ///</summary>
        ///<param name="dt">time since last update</param>
        public override void Update(float dt)
        {
            if (Time.TimeNow - TimeCreated > LifeTime)
            {
                _markForDelete = true;
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly float _lifeTime;
        private readonly float _timeCreated;
        private bool _markForDelete;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}