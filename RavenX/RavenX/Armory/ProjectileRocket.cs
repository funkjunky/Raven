#region File description

//------------------------------------------------------------------------------
//ProjectileRocket.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

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
    ///class to implement a rocket
    ///</summary>
    public sealed class ProjectileRocket : Projectile
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
        public ProjectileRocket(
            BotEntity shooter,
            Vector2 target,
            IEntitySceneObject entitySceneObject)
            : base(
                entitySceneObject,
                target,
                shooter.ObjectId,
                shooter.Position,
                Vector2.Normalize(target - shooter.Position),
                (int) GameManager.GameManager.Instance.Parameters.RocketDamage,
                GameManager.GameManager.Instance.Parameters.RocketScale,
                GameManager.GameManager.Instance.Parameters.RocketMaxSpeed,
                GameManager.GameManager.Instance.Parameters.RocketMass,
                GameManager.GameManager.Instance.Parameters.RocketMaxForce)
        {
            _previousPosition = Position;
            _currentBlastRadius = 0.0f;
            _maxBlastRadius =
                GameManager.GameManager.Instance.Parameters.RocketBlastRadius;
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

                _currentBlastRadius += dt*
                                       GameManager.GameManager.Instance.Parameters.RocketExplosionDecayRate;

                //when the rendered blast circle becomes equal in size to the
                //maximum blast radius the rocket can be removed from the game
                if (_currentBlastRadius > _maxBlastRadius)
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
            DrawUtil.CircleFill(Position, 2, Color.Orange, 20);
            DrawUtil.Circle(Position, 2, Color.Red, 20);

            if (HasImpacted && _currentBlastRadius > 0)
            {
                DrawUtil.Circle(Position, _currentBlastRadius, Color.Red, 20);
            }
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///If the rocket has impacted we test all bots to see if they are
        ///within the blast radius and reduce their health accordingly
        ///</summary>
        private void InflictDamageOnBotsWithinBlastRadius()
        {
            foreach (BotEntity curBot in GameManager.GameManager.Instance.BotList)
            {
                if (Vector2.Distance(Position, curBot.Position) <
                    _maxBlastRadius + curBot.BoundingRadius)
                {
                    //send a message to the bot to let it know it's been hit,
                    //and who the shot came from
                    MessageDispatcher.Instance.DispatchMsg(
                        MessageDispatcher.SEND_MSG_IMMEDIATELY,
                        ShooterId,
                        curBot.ObjectId,
                        MessageTypes.TakeThatMF,
                        DamageInflicted);
                }
            }
        }

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
                Position = ImpactPoint; //keep from penetrating walls
            }

            //test to see if the line segment connecting the rocket's current
            //position and previous position intersects with any bots.
            BotEntity hit =
                GetClosestIntersectingBot(_previousPosition, Position);

            //if hit
            if (hit != null)
            {
                HasImpacted = true; //impacted but not dead yet!
                _timeOfImpact = Time.TimeNow;

                //TODO: should we calculate impact point on the bounding radius?
                Position = ImpactPoint = hit.Position;

                //send a message to the bot to let it know it's been hit, 
                //and who the shot came from
                MessageDispatcher.Instance.DispatchMsg(
                    MessageDispatcher.SEND_MSG_IMMEDIATELY,
                    ShooterId,
                    hit.ObjectId,
                    MessageTypes.TakeThatMF,
                    DamageInflicted);

                //test for bots within the blast radius and inflict damage
                InflictDamageOnBotsWithinBlastRadius();

                //Note: original code doesn't have a return here
                //I added one because it seems to me the if a rocket hits a bot
                //and explodes then it doesn't also continue travelling and
                //hitting a wall or exploding after reaching its target --- sdg

                return;
            }

            //if we get here and HasImpacted, then we hit a wall, but not a bot.
            //check for splash damage
            if (HasImpacted)
            {
                //test for bots within the blast radius and inflict damage
                InflictDamageOnBotsWithinBlastRadius();

                return;
            }

            //if we get here, we didn't hit any walls or bots.
            //test to see if rocket has reached target position. If so, test for
            //all bots in vicinity
            const float tolerance = 5.0f; //TODO: should be a paramter
            if (Vector2.DistanceSquared(Position, Target) >= tolerance*tolerance)
                return;

            HasImpacted = true; //impacted but not dead yet!
            _timeOfImpact = Time.TimeNow;
            InflictDamageOnBotsWithinBlastRadius();
        }

        #endregion

        #region Private, protected, internal fields

        //the radius of damage, once the rocket has impacted
        private readonly float _maxBlastRadius;

        //this is used to render the splash when the rocket impacts
        private float _currentBlastRadius;
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