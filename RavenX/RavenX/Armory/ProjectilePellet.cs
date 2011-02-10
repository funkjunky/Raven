#region File description

//------------------------------------------------------------------------------
//ProjectilePellet.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using System;
using GarageGames.Torque.GUI;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Map;
using Mindcrafters.RavenX.Messaging;

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

    #endregion

#endregion

namespace Mindcrafters.RavenX.Armory
{
    ///<summary>
    ///class to implement a pellet type projectile
    ///</summary>
    public sealed class ProjectilePellet : Projectile
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
        public ProjectilePellet(
            BotEntity shooter,
            Vector2 target,
            IEntitySceneObject entitySceneObject)
            : base(
                entitySceneObject,
                target,
                shooter.ObjectId,
                shooter.Position,
                Vector2.Normalize(target - shooter.Position),
                (int) GameManager.GameManager.Instance.Parameters.PelletDamage,
                GameManager.GameManager.Instance.Parameters.PelletScale,
                GameManager.GameManager.Instance.Parameters.PelletMaxSpeed,
                GameManager.GameManager.Instance.Parameters.PelletMass,
                GameManager.GameManager.Instance.Parameters.PelletMaxForce)
        {
            _previousPosition = Position;
            _timeShotIsVisible =
                GameManager.GameManager.Instance.Parameters.PelletPersistence;
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

                //we keep rendering the projectile for a while
                //so player can see its impact location
                if (!IsVisibleToPlayer())
                {
                    IsDead = true;
                }
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
//#if SHOW_IMPACT
            if (!IsVisibleToPlayer() || !HasImpacted)
                return;

            DrawUtil.Line(Origin, ImpactPoint, Color.Yellow);
            DrawUtil.CircleFill(ImpactPoint, 3, Color.Brown, 20);
//#endif
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///Tests if the shot is still to be rendered
        ///</summary>
        ///<returns>true if the shot is still to be rendered</returns>
        private bool IsVisibleToPlayer()
        {
            //return Time.TimeNow < TmeOfCreation + _timeShotIsVisible;
            return Time.TimeNow < _timeOfImpact + _timeShotIsVisible;
        }

        ///<summary>
        ///if the projectile has reached the target position or it hits an
        ///entity or wall it should explode/inflict damage/whatever and then
        ///mark itself as dead 
        ///</summary>
        private void TestForImpact()
        {
            if (HasImpacted ||
                Epsilon.IsEqual(Position, _previousPosition) ||
                Epsilon.IsEqual(Position, Origin))
            {
                return;
            }

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
                Position = ImpactPoint; //keep from penetrating walls
            }

            //test to see if the line segment connecting the pellet's current
            //position and previous position intersects with any bots.
            BotEntity hit =
                GetClosestIntersectingBot(_previousPosition, Position);

            //if hit
            if (hit == null)
                return;

            HasImpacted = true; //impacted but not dead yet!
            _timeOfImpact = Time.TimeNow;
            //in case there's no intersection (but there always should be one!)
            ImpactPoint = hit.Position;

            //determine the impact point with the bot's bounding circle so
            //that the shell can be rendered properly
            Geometry.GetLineSegmentCircleClosestIntersectionPoint(
                _previousPosition,
                Position,
                hit.Position,
                hit.BoundingRadius,
                ref _impactPoint // TODO: see field declaration comments
                );
            Position = ImpactPoint; // keep from penetrating bot

            //send a message to the bot to let it know it's been hit,
            //and who the shot came from
            MessageDispatcher.Instance.DispatchMsg(
                MessageDispatcher.SEND_MSG_IMMEDIATELY,
                ShooterId,
                hit.ObjectId,
                MessageTypes.TakeThatMF,
                DamageInflicted);

            return;
        }

        #endregion

        #region Private, protected, internal fields

        //when this projectile hits something its trajectory is rendered
        //for this amount of time
        private readonly float _timeShotIsVisible;
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