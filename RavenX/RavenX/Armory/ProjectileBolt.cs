#region File description

//------------------------------------------------------------------------------
//ProjectileBolt.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Map;
using Mindcrafters.RavenX.Messaging;

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
    ///class to implement a bolt type projectile
    ///</summary>
    public sealed class ProjectileBolt : Projectile
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="shooter"></param>
        ///<param name="target"></param>
        ///<param name="entitySceneObject"></param>
        public ProjectileBolt(
            BotEntity shooter,
            Vector2 target,
            IEntitySceneObject entitySceneObject)
            : base(
                entitySceneObject,
                target,
                shooter.ObjectId,
                shooter.Position,
                Vector2.Normalize(target - shooter.Position),
                (int) GameManager.GameManager.Instance.Parameters.BoltDamage,
                GameManager.GameManager.Instance.Parameters.BotScale,
                GameManager.GameManager.Instance.Parameters.BoltMaxSpeed,
                GameManager.GameManager.Instance.Parameters.BoltMass,
                GameManager.GameManager.Instance.Parameters.BoltMaxForce)
        {
            _previousPosition = Position;
        }

        #endregion

        #region Public methods

        ///<summary>
        ///update projectile's velocity, etc.
        ///</summary>
        ///<param name="dt"></param>
        public override void Update(float dt)
        {
            TestForImpact();

            _previousPosition = Position;

            if (!HasImpacted)
            {
                Velocity = MaxSpeed*Heading;

                //make sure entity does not exceed maximum velocity
                Velocity = Vector2Util.Truncate(Velocity, MaxSpeed);
            }
            else
            {
                Velocity = Vector2.Zero;
            }
        }

        ///<summary>
        ///Render entity
        ///<remarks>
        ///If the entity is a T2DSceneObject, it will be rendered by the engine,
        ///but we may still want to render some additional info
        ///</remarks>
        ///</summary>
        public override void Render()
        {
            //DrawUtil.Line(Position, _previousPosition, Color.Green, 2);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///if the projectile has reached the target position or it hits an
        ///entity or wall it should explode/inflict damage/whatever and then
        ///mark itself as dead 
        ///</summary>
        private void TestForImpact()
        {
            if (HasImpacted ||
                Epsilon.IsEqual(Position, _previousPosition))
                return;

            ImpactPoint = Position; // in case there's no impact

            //first check for potential impact with walls. If there is a
            //potential impact, roll the position back to the impact point.
            //Then check if it hit a bot. This way we end up with the impact
            //point at the first object hit.

            //test for impact with a wall
            float distanceToClosestImpact;
            if (WallIntersectionTests.FindClosestPointOfIntersectionWithWalls(
                _previousPosition,
                Position,
                GameManager.GameManager.Instance.Map.Walls,
                out distanceToClosestImpact,
                ref _impactPoint // TODO: see field declaration comments
                ))
            {
                HasImpacted = true; //impacted but not dead yet!
                _timeOfImpact = Time.TimeNow;
                IsDead = true;
                Position = ImpactPoint; //keep from penetrating walls
            }

            //test to see if the line segment connecting the bolt's current
            //position and previous position intersects with any bots.
            BotEntity hit =
                GetClosestIntersectingBot(_previousPosition, Position);


            if (hit == null)
                return;

            //else hit
            HasImpacted = true;
            _timeOfImpact = Time.TimeNow;
            IsDead = true;

            //not necessary to calculate the impact point on the bot's
            //bounding radius since the projectile is dead anyway
            Position = ImpactPoint = hit.Position;

            //send a message to the bot to let it know it's been hit,
            //and who the shot came from
            MessageDispatcher.Instance.DispatchMsg(
                MessageDispatcher.SEND_MSG_IMMEDIATELY,
                ShooterId,
                hit.ObjectId,
                MessageTypes.TakeThatMF,
                DamageInflicted);
        }

        #endregion

        #region Private, protected, internal fields

        private Vector2 _previousPosition;
        private float _timeOfImpact = Single.MinValue;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}