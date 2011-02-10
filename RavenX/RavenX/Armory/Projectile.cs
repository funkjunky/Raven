#region File description

//------------------------------------------------------------------------------
//Projectile.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;

    #endregion

    #region Microsoft

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Armory
{
    ///<summary>
    ///Abstract base class for projectiles
    ///TODO: some code in the derived classes can be generalized and moved here
    ///</summary>
    public abstract class Projectile : MovingEntity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="entitySceneObject"></param>
        ///<param name="target"></param>
        ///<param name="shooterId"></param>
        ///<param name="origin"></param>
        ///<param name="heading"></param>
        ///<param name="damage"></param>
        ///<param name="scale"></param>
        ///<param name="maxSpeed"></param>
        ///<param name="mass"></param>
        ///<param name="maxForce"></param>
        protected Projectile(
            IEntitySceneObject entitySceneObject,
            Vector2 target, //the target's position
            uint shooterId, //the Id of the bot that fired this shot
            Vector2 origin, //the start position of the projectile
            Vector2 heading, //the heading of the projectile
            int damage, //how much damage it inflicts
            float scale,
            float maxSpeed,
            float mass,
            float maxForce)
            : base(
                entitySceneObject,
                origin,
                scale,
                new Vector2(0, 0),
                maxSpeed,
                heading,
                mass,
                new Vector2(scale, scale),
                0, //max turn rate irrelevant here, all shots go straight
                maxForce)
        {
            _target = target;
            _isDead = false;
            _hasImpacted = false;
            _damageInflicted = damage;
            _origin = origin;
            _shooterId = shooterId;
            _timeOfCreation = Time.TimeNow;
            Facing = Heading; //for projectiles, Facing == Heading
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///true if the projectile has impacted but is not yet dead (because it
        ///may be exploding outwards from the point of impact for example)
        ///</summary>
        public bool HasImpacted
        {
            get { return _hasImpacted; }
            protected set { _hasImpacted = value; }
        }

        ///<summary>
        ///set to true if the projectile has impacted and has finished any
        ///explosion sequence. When true the projectile will be removed from
        ///the game
        ///</summary>
        public bool IsDead
        {
            get { return _isDead; }
            protected set { _isDead = value; }
        }

        ///<summary>
        ///Id of entity that owns this projectile
        ///</summary>
        public uint ShooterId
        {
            get { return _shooterId; }
            protected set { _shooterId = value; }
        }

        ///<summary>
        ///Target position this projectile is heading for.
        ///</summary>
        public Vector2 Target
        {
            get { return _target; }
            protected set { _target = value; }
        }

        ///<summary>
        ///Where the shot was fired from
        ///</summary>
        public Vector2 Origin
        {
            get { return _origin; }
            protected set { _origin = value; }
        }

        ///<summary>
        ///Amount of damage a projectile of this type does
        ///</summary>
        public int DamageInflicted
        {
            get { return _damageInflicted; }
            protected set { _damageInflicted = value; }
        }

        ///<summary>
        ///Time when projectile was fired. This enables the shot to be rendered
        ///for a specific length of time
        ///</summary>
        public float TmeOfCreation
        {
            get { return _timeOfCreation; }
            set { _timeOfCreation = value; }
        }

        ///<summary>
        ///the position where this projectile impacts an object
        ///</summary>
        public Vector2 ImpactPoint
        {
            get { return _impactPoint; }
            set { _impactPoint = value; }
        }

        #region Public methods

        ///<summary>
        ///update projectile's velocity, etc.
        ///</summary>
        ///<param name="dt"></param>
        public abstract override void Update(float dt);

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///Get closest intersecting bot
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns></returns>
        protected BotEntity GetClosestIntersectingBot(Vector2 from, Vector2 to)
        {
            BotEntity closestIntersectingBot = null;
            double closestSoFar = Single.MaxValue;

            //iterate through all entities checking against the line segment
            foreach (BotEntity curBot in GameManager.GameManager.Instance.BotList)
            {
                //make sure we don't check against the shooter of the projectile
                if (curBot.ObjectId == _shooterId)
                    continue;

                //if the distance to the line segment is less than the entity's
                //bounding radius then there is an intersection
                if (Geometry.DistToLineSegment(from, to, curBot.Position) >=
                    curBot.BoundingRadius)
                    continue;

                //test to see if this is the closest so far
                double dist = (curBot.Position - _origin).LengthSquared();

                if (dist >= closestSoFar)
                    continue;

                closestSoFar = dist;
                closestIntersectingBot = curBot;
            }

            return closestIntersectingBot;
        }

        ///<summary>
        ///Get list of bots intersecting the line segment between
        ///<paramref name="from"/> and <paramref name="to"/>
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns></returns>
        protected List<BotEntity> GetListOfIntersectingBots(
            Vector2 from, Vector2 to)
        {
            //holds any bots that are intersecting with the line segment
            List<BotEntity> hits = new List<BotEntity>();

            //iterate through all entities checking against the line segment
            foreach (BotEntity curBot in GameManager.GameManager.Instance.BotList)
            {
                //make sure we don't check against the shooter of the projectile
                if ((curBot.ObjectId == _shooterId))
                    continue;

                //if the distance to the line segment is less than the entity's
                //bounding radius, there is an intersection so add it to hits
                if (Geometry.DistToLineSegment(from, to, curBot.Position) <
                    curBot.BoundingRadius)
                {
                    hits.Add(curBot);
                }
            }

            return hits;
        }

        #endregion

        #region Private, protected, internal fields

        private int _damageInflicted;
        private bool _hasImpacted;

        ///<summary>
        ///the position where this projectile impacts an object
        ///<remarks>
        ///Since we cannot pass the <see cref="ImpactPoint"/> property as a ref
        ///parameter in some intersection tests, we make this field protected.
        ///This breaks encapsulation.
        ///TODO: Might be useful to rethink the design of the intersection tests
        ///</remarks>
        ///</summary>
        protected Vector2 _impactPoint;

        private bool _isDead;
        private Vector2 _origin;
        private uint _shooterId;
        private Vector2 _target;
        private float _timeOfCreation;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}