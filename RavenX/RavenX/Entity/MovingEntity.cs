#region File description

//------------------------------------------------------------------------------
//MovingEntity.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using System;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity
{
    ///<summary>
    ///A base class defining an entity that moves. The entity has 
    ///a local coordinate system and members for defining its
    // mass and velocity.
    ///</summary>
    public abstract class MovingEntity : Entity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="entitySceneObject"></param>
        ///<param name="position"></param>
        ///<param name="radius"></param>
        ///<param name="velocity"></param>
        ///<param name="maxSpeed"></param>
        ///<param name="heading"></param>
        ///<param name="mass"></param>
        ///<param name="scale"></param>
        ///<param name="maxTurnRate"></param>
        ///<param name="maxForce"></param>
        protected MovingEntity(
            IEntitySceneObject entitySceneObject,
            Vector2 position,
            float radius,
            Vector2 velocity,
            float maxSpeed,
            Vector2 heading,
            float mass,
            Vector2 scale,
            float maxTurnRate,
            float maxForce)
            : base(entitySceneObject)
        {
            _heading = heading;
            _side = Vector2Util.Perp(heading);
            Velocity = velocity;
            Mass = mass;
            _maxSpeed = maxSpeed;
            _maxTurnRate = maxTurnRate;
            _maxForce = maxForce;
            Position = position;
            BoundingRadius = radius;
            Scale = scale;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///velocity
        ///</summary>
        public Vector2 Velocity
        {
            get { return SceneObject.Physics.Velocity; }
            set { SceneObject.Physics.Velocity = value; }
        }

        ///<summary>
        ///mass
        ///</summary>
        public float Mass
        {
            get { return SceneObject.Physics.Mass; }
            protected set { SceneObject.Physics.Mass = value; }
        }

        ///<summary>
        ///a normalized vector pointing in the direction the entity is heading. 
        ///</summary>
        public Vector2 Heading
        {
            get { return _heading; }
            set
            {
                if (Single.IsNaN(value.X) || Single.IsNaN(value.Y))
                {
                }

                _heading = value;
            }
        }

        ///<summary>
        ///a vector perpendicular to the heading vector
        ///</summary>
        public Vector2 Side
        {
            get { return _side; }
            set { _side = value; }
        }

        ///<summary>
        ///the maximum speed this entity may travel at.
        ///</summary>
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }

        ///<summary>
        ///the maximum force this entity can produce to power itself 
        ///(think rockets and thrust)
        ///</summary>
        public float MaxForce
        {
            get { return _maxForce; }
            set { _maxForce = value; }
        }

        ///<summary>
        ///is entity traveling at (or beyond) maximum speed
        ///</summary>
        public bool IsSpeedMaxedOut
        {
            get { return _maxSpeed*_maxSpeed <= Velocity.LengthSquared(); }
        }

        ///<summary>
        ///speed
        ///</summary>
        public float Speed
        {
            get { return Velocity.Length(); }
        }

        ///<summary>
        ///speed squared
        ///</summary>
        public float SpeedSq
        {
            get { return Velocity.LengthSquared(); }
        }

        ///<summary>
        ///the maximum rate (radians per second) this entity can rotate.
        ///Currently, this affects how quickly aim can be changed but
        ///not how quickly movement direction can be changed
        ///TODO: we really need separate turn rates for lower body
        ///(legs, tracks, wheels), upper body (arms, turret), head (FOV)  
        ///</summary>
        public float MaxTurnRate
        {
            get { return _maxTurnRate; }
            set { _maxTurnRate = value; }
        }

        #region Public methods

        ///<summary>
        ///Update the moving entity (part of the game update logic)
        ///</summary>
        ///<param name="dt">time since last update</param>
        public abstract override void Update(float dt);

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private Vector2 _heading;
        private float _maxForce;
        private float _maxSpeed;
        private float _maxTurnRate;
        private Vector2 _side;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}