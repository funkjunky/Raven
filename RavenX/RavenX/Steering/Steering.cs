#region File description

//------------------------------------------------------------------------------
//SteeringBehaviors.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Map;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Steering
{
    ///<summary>
    ///Steering class
    ///</summary>
    public class Steering
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///distance the wander circle is projected in front of the agent
        ///</summary>
        public const float WANDER_DIST = 2.0f;

        ///<summary>
        ///the maximum amount of displacement along the circle each frame
        ///</summary>
        public const float WANDER_JITTER_PER_SEC = 40.0f;

        ///<summary>
        ///the radius of the constraining circle for the wander behavior
        ///</summary>
        public const float WANDER_RAD = 1.2f;

        #endregion

        #region Constructors

        ///<summary>
        ///steering behavior constructor
        ///</summary>
        ///<param name="agent"></param>
        public Steering(BotEntity agent)
        {
            _bot = agent;
            Flags = 0;
            _weightSeparation = GameManager.GameManager.Instance.Parameters.SeparationWeight;
            _weightWander = GameManager.GameManager.Instance.Parameters.WanderWeight;
            _weightWallAvoidance = GameManager.GameManager.Instance.Parameters.WallAvoidanceWeight;
            _viewDistance = GameManager.GameManager.Instance.Parameters.ViewDistance;
            _wallDetectionFeelerLength = GameManager.GameManager.Instance.Parameters.WallDetectionFeelerLength;
            Feelers = new List<Vector2>(3);
            _deceleration = Decelerations.Normal;
            TargetAgent1 = null;
            TargetAgent2 = null;
            _wanderDistance = WANDER_DIST;
            _wanderJitter = WANDER_JITTER_PER_SEC;
            _wanderRadius = WANDER_RAD;
            _weightSeek = GameManager.GameManager.Instance.Parameters.SeekWeight;
            _weightArrive = GameManager.GameManager.Instance.Parameters.ArriveWeight;
            CellSpaceIsOn = true;
            SummingMethod = SummingMethods.Prioritized;

            //stuff for the wander behavior
            float theta = RandomUtil.RandomFloat()*(float) Math.PI*2.0f;

            //create a vector to a target position on the wander circle
            WanderTarget = new Vector2(WanderRadius*(float) Math.Cos(theta),
                                       WanderRadius*(float) Math.Sin(theta));
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region Decelerations enum

        ///<summary>
        ///Arrive makes use of these to determine how quickly a Bot
        ///should decelerate to its target
        ///</summary>
        public enum Decelerations
        {
            Slow = 3,
            Normal = 2,
            Fast = 1
        } ;

        #endregion

        #region SummingMethods enum

        ///<summary>
        ///methods of combining multiple steering behaviors
        ///</summary>
        public enum SummingMethods
        {
            WeightedAverage,
            Prioritized,
            Dithered
        } ;

        #endregion

        ///<summary>
        ///current target for some steering behaviors
        ///</summary>
        public Vector2 Target
        {
            get { return _target; }
            set { _target = value; }
        }

        ///<summary>
        ///can be used to keep track of friends, pursuers, or prey
        ///</summary>
        public BotEntity TargetAgent1
        {
            get { return _targetAgent1; }
            set { _targetAgent1 = value; }
        }

        ///<summary>
        ///can be used to keep track of friends, pursuers, or prey
        ///</summary>
        public BotEntity TargetAgent2
        {
            get { return _targetAgent2; }
            set { _targetAgent2 = value; }
        }

        ///<summary>
        ///steering force
        ///</summary>
        public Vector2 Force
        {
            get { return _steeringForce; }
        }

        ///<summary>
        ///method of combining multiple active steering behaviors
        ///</summary>
        public SummingMethods SummingMethod
        {
            get { return _summingMethod; }
            set { _summingMethod = value; }
        }

        ///<summary>
        ///true if seek behavior is on
        ///</summary>
        public bool SeekIsOn
        {
            get { return On(BehaviorTypes.Seek); }
            set
            {
                if (value)
                {
                    Flags |= (int) BehaviorTypes.Seek;
                }
                else if (On(BehaviorTypes.Seek))
                {
                    Flags ^= (int) BehaviorTypes.Seek;
                }
            }
        }

        ///<summary>
        ///true if arrive behavior is on
        ///</summary>
        public bool ArriveIsOn
        {
            get { return On(BehaviorTypes.Arrive); }
            set
            {
                if (value)
                {
                    Flags |= (int) BehaviorTypes.Arrive;
                }
                else if (On(BehaviorTypes.Arrive))
                {
                    Flags ^= (int) BehaviorTypes.Arrive;
                }
            }
        }

        ///<summary>
        ///true if wander behavior is on
        ///</summary>
        public bool WanderIsOn
        {
            get { return On(BehaviorTypes.Wander); }
            set
            {
                if (value)
                {
                    Flags |= (int) BehaviorTypes.Wander;
                }
                else if (On(BehaviorTypes.Wander))
                {
                    Flags ^= (int) BehaviorTypes.Wander;
                }
            }
        }

        ///<summary>
        ///true if separation behavior is on
        ///</summary>
        public bool SeparationIsOn
        {
            get { return On(BehaviorTypes.Separation); }
            set
            {
                if (value)
                {
                    Flags |= (int) BehaviorTypes.Separation;
                }
                else if (On(BehaviorTypes.Separation))
                {
                    Flags ^= (int) BehaviorTypes.Separation;
                }
            }
        }

        ///<summary>
        ///true if wall avoidance behavior is on
        ///</summary>
        public bool WallAvoidanceIsOn
        {
            get { return On(BehaviorTypes.WallAvoidance); }
            set
            {
                if (value)
                {
                    Flags |= (int) BehaviorTypes.WallAvoidance;
                }
                else if (On(BehaviorTypes.WallAvoidance))
                {
                    Flags ^= (int) BehaviorTypes.WallAvoidance;
                }
            }
        }

        ///<summary>
        ///a vertex buffer to contain the feelers required for wall avoidance
        ///</summary>
        public List<Vector2> Feelers
        {
            get { return _feelers; }
            set { _feelers = value; }
        }

        ///<summary>
        ///wander jitter factor
        ///</summary>
        public float WanderJitter
        {
            get { return _wanderJitter; }
        }

        ///<summary>
        ///distance ahead to project the wander circle
        ///</summary>
        public float WanderDistance
        {
            get { return _wanderDistance; }
        }

        ///<summary>
        ///radius of wander circle
        ///</summary>
        public float WanderRadius
        {
            get { return _wanderRadius; }
        }

        ///<summary>
        ///the owner of this instance
        ///</summary>
        public BotEntity Bot
        {
            get { return _bot; }
        }

        ///<summary>
        ///the length of the 'feeler(s)' used in wall detection
        ///</summary>
        public float WallDetectionFeelerLength
        {
            get { return _wallDetectionFeelerLength; }
        }

        ///<summary>
        ///the current position on the wander circle the agent is
        ///attempting to steer towards
        ///</summary>
        public Vector2 WanderTarget
        {
            get { return _wanderTarget; }
            set { _wanderTarget = value; }
        }

        ///<summary>
        ///multiplier can be adjusted to effect strength of the  
        ///separation behavior.
        ///</summary>
        public float WeightSeparation
        {
            get { return _weightSeparation; }
        }

        ///<summary>
        ///multiplier can be adjusted to effect strength of the  
        ///wander behavior.
        ///</summary>
        public float WeightWander
        {
            get { return _weightWander; }
        }

        ///<summary>
        ///multiplier can be adjusted to effect strength of the  
        ///wall avoidance behavior.
        ///</summary>
        public float WeightWallAvoidance
        {
            get { return _weightWallAvoidance; }
        }

        ///<summary>
        ///multiplier can be adjusted to effect strength of the  
        ///seek behavior.
        ///</summary>
        public float WeightSeek
        {
            get { return _weightSeek; }
        }

        ///<summary>
        ///multiplier can be adjusted to effect strength of the  
        ///arrive behavior.
        ///</summary>
        public float WeightArrive
        {
            get { return _weightArrive; }
        }

        ///<summary>
        ///how far the agent can 'see'
        ///</summary>
        public float ViewDistance
        {
            get { return _viewDistance; }
        }

        ///<summary>
        ///binary flags to indicate whether or not a behavior should be active
        ///</summary>
        public int Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        ///<summary>
        ///Arrive uses this to determine how quickly a Bot
        ///should decelerate to its target
        ///</summary>
        public Decelerations Deceleration
        {
            get { return _deceleration; }
        }

        ///<summary>
        ///is cell space partitioning to be used or not?
        ///</summary>
        public bool CellSpaceIsOn
        {
            get { return _cellSpaceIsOn; }
            set { _cellSpaceIsOn = value; }
        }

        #region Public methods

        ///<summary>
        ///calculates the accumulated steering force according to the method set
        // in _summingMethod
        ///</summary>
        ///<param name="dt"></param>
        ///<returns></returns>
        public Vector2 Calculate(float dt)
        {
            //reset the steering force
            _steeringForce = Vector2.Zero;

            //tag neighbors if any of the following 3 group behaviors are
            //switched on
            if (SeparationIsOn)
            {
                GameManager.GameManager.Instance.TagBotsWithinViewRange(Bot, ViewDistance);
            }

            _steeringForce = CalculatePrioritized(dt);

            return _steeringForce;
        }

        ///<summary>
        ///Calculates the forward component of the steering force
        ///</summary>
        ///<returns>the forward component of the steering force</returns>
        public float ForwardComponent()
        {
            return Vector2.Dot(Bot.Heading, _steeringForce);
        }

        ///<summary>
        ///Calculates the side component of the steering force
        ///</summary>
        ///<returns>the side component of the steering force</returns>
        public float SideComponent()
        {
            return Vector2.Dot(Bot.Side, _steeringForce);
        }

        ///<summary>
        ///This function calculates how much of its max steering force the 
        ///entity has left to apply and then applies that amount of the
        ///force to add.
        ///</summary>
        ///<param name="runningTotal"></param>
        ///<param name="forceToAdd"></param>
        ///<returns></returns>
        public bool AccumulateForce(ref Vector2 runningTotal, Vector2 forceToAdd)
        {
            //calculate how much steering force the entity has used so far
            float magnitudeSoFar = runningTotal.Length();

            //calculate how much steering force remains to be used by this entity
            float magnitudeRemaining = Bot.MaxForce - magnitudeSoFar;

            //return false if there is no more force left to use
            if (magnitudeRemaining <= 0.0f) return false;

            //calculate the magnitude of the force we want to add
            float magnitudeToAdd = forceToAdd.Length();

            //if the magnitude of the sum of forceToAdd and the running total
            //does not exceed the maximum force available to this entity, just
            //add together. Otherwise add as much of the forceToAdd vector is
            //possible without going over the max.
            if (magnitudeToAdd < magnitudeRemaining)
            {
                runningTotal += forceToAdd;
            }
            else
            {
                magnitudeToAdd = magnitudeRemaining;

                //add it to the steering force
                runningTotal += (Vector2.Normalize(forceToAdd)*magnitudeToAdd);
            }

            return true;
        }

        ///<summary>
        ///this method calls each active steering behavior in order of priority
        ///and accumulates their forces until the max steering force magnitude
        ///is reached, at which time the function returns the steering force 
        ///accumulated to that  point
        ///</summary>
        ///<param name="dt"></param>
        ///<returns></returns>
        public Vector2 CalculatePrioritized(float dt)
        {
            Vector2 force;

            if (WallAvoidanceIsOn)
            {
                force = WallAvoidance(dt, GameManager.GameManager.Instance.Map.Walls)*
                        WeightWallAvoidance;

                if (!AccumulateForce(ref _steeringForce, force))
                    return _steeringForce;
            }

            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)

            if (SeparationIsOn)
            {
                force =
                    Separation(GameManager.GameManager.Instance.BotList)*WeightSeparation;

                if (!AccumulateForce(ref _steeringForce, force))
                    return _steeringForce;
            }

            if (SeekIsOn)
            {
                force = Seek(Target)*WeightSeek;

                if (!AccumulateForce(ref _steeringForce, force))
                    return _steeringForce;
            }

            if (ArriveIsOn)
            {
                force = Arrive(Target, Deceleration)*WeightArrive;

                if (!AccumulateForce(ref _steeringForce, force))
                    return _steeringForce;
            }

            if (WanderIsOn)
            {
                force = Wander()*WeightWander;

                if (!AccumulateForce(ref _steeringForce, force))
                    return _steeringForce;
            }

            return _steeringForce;
        }

        ///<summary>
        ///Given a target, this behavior returns a steering force which will
        ///direct the agent towards the target
        ///</summary>
        ///<param name="target"></param>
        ///<returns></returns>
        public Vector2 Seek(Vector2 target)
        {
            Vector2 desiredVelocity = Vector2.Zero;

            if (!Epsilon.IsZero(target - Bot.Position))
            {
                desiredVelocity =
                    Vector2.Normalize(target - Bot.Position)*Bot.MaxSpeed;
            }

            return (desiredVelocity - Bot.Velocity);
        }

        ///<summary>
        ///This behavior is similar to seek but it attempts to arrive at the
        ///target with a zero velocity
        ///</summary>
        ///<param name="target"></param>
        ///<param name="deceleration"></param>
        ///<returns></returns>
        public Vector2 Arrive(Vector2 target, Decelerations deceleration)
        {
            Vector2 toTarget = target - Bot.Position;

            //calculate the distance to the target
            float dist = toTarget.Length();

            if (dist > 0)
            {
                //because Decelerations is enumerated as an int, this value is
                //required to provide fine tweaking of the deceleration..
                const float decelerationTweaker = 0.3f; //TODO: should be a parameter

                //calculate the speed required to reach the target given the
                //desired deceleration
                float speed = dist/((float) deceleration*decelerationTweaker);

                //make sure the velocity does not exceed the max
                speed = Math.Min(speed, Bot.MaxSpeed);

                //from here proceed just like Seek except we don't need to
                //normalize the toTarget vector because we have already gone
                //to the trouble of calculating its length: dist. 
                Vector2 desiredVelocity = toTarget*speed/dist;

                return (desiredVelocity - Bot.Velocity);
            }

            return Vector2.Zero;
        }

        ///<summary>
        ///This behavior makes the agent wander about randomly
        ///</summary>
        ///<returns></returns>
        public Vector2 Wander()
        {
            //first, add a small random vector to the target's position
            WanderTarget +=
                new Vector2(RandomUtil.RandomClamped()*WanderJitter,
                            RandomUtil.RandomClamped()*WanderJitter);

            //reproject this new vector back on to a unit circle
            WanderTarget = Vector2.Normalize(WanderTarget);

            //increase the length of the vector to the same as the radius
            //of the wander circle
            WanderTarget *= WanderRadius;

            //move the target into a position WANDER_DIST in front of the agent
            Vector2 target = WanderTarget + new Vector2(WanderDistance, 0);

            //project the target into world space
            Vector2 worldTarget =
                Transformations.PointToWorldSpace(
                    target,
                    Bot.Heading,
                    Bot.Side,
                    Bot.Position);

            //and steer towards it
            return worldTarget - Bot.Position;
        }

        ///<summary>
        ///This returns a steering force that will keep the agent away from any
        ///walls it may encounter
        ///</summary>
        ///<param name="dt"></param>
        ///<param name="walls"></param>
        ///<returns></returns>
        public Vector2 WallAvoidance(float dt, List<Wall> walls)
        {
            //the feelers are contained in a list _feelers
            CreateFeelers(dt);

            float distToClosestIP = Single.MaxValue;

            //this will hold an index into the vector of walls
            int closestWall = -1;

            Vector2 steeringForce = Vector2.Zero;
            //the closest intersection point
            Vector2 closestPoint = Vector2.Zero;

            //examine each feeler in turn
            for (int flr = 0; flr < Feelers.Count; ++flr)
            {
                //run through each wall checking for any intersection points
                for (int w = 0; w < walls.Count; ++w)
                {
                    float distToThisIP;
                    Vector2 point; //used for storing temporary info

                    if (!Geometry.LineIntersection(Bot.Position,
                                                   Feelers[flr],
                                                   walls[w].From,
                                                   walls[w].To,
                                                   out distToThisIP,
                                                   out point))
                        continue;

                    //is this the closest found so far? If so keep a record
                    if (distToThisIP >= distToClosestIP)
                        continue;

                    distToClosestIP = distToThisIP;
                    closestWall = w;
                    closestPoint = point;
                }

                //if an intersection point has been detected, calculate a force  
                //that will direct the agent away
                if (closestWall < 0)
                    continue;

                //calculate by what distance the projected position of the agent
                //will overshoot the wall
                Vector2 overShoot = Feelers[flr] - closestPoint;

                //create a force in the direction of the wall normal, with a 
                //magnitude of the overshoot
                steeringForce = walls[closestWall].Normal*overShoot.Length();
            }

            return steeringForce;
        }

        ///<summary>
        ///Creates the antenna utilized by <see cref="WallAvoidance"/>
        ///</summary>
        ///<param name="dt"></param>
        public void CreateFeelers(float dt)
        {
            Feelers = new List<Vector2>();

            //feeler pointing straight in front
            Feelers.Add(Bot.Position + Bot.Heading*
                                       ((Bot.BoundingRadius/2.0f) + 1.5f*WallDetectionFeelerLength*
                                                                    Bot.Speed*dt));

            //feeler to left
            Vector2 offsetHeading = Bot.Heading;
            Transformations.RotateVectorAroundOrigin(
                ref offsetHeading,
                (float) Math.PI/2.0f*3.5f);
            Feelers.Add(Bot.Position + offsetHeading*
                                       ((Bot.BoundingRadius/2.0f) + WallDetectionFeelerLength/2.0f));

            //feeler to right
            offsetHeading = Bot.Heading;
            Transformations.RotateVectorAroundOrigin(
                ref offsetHeading,
                (float) Math.PI/2.0f*0.5f);
            Feelers.Add(Bot.Position + offsetHeading*
                                       ((Bot.BoundingRadius/2.0f) + WallDetectionFeelerLength/2.0f));
        }

        ///<summary>
        ///this calculates a force repelling from the other neighbors
        ///</summary>
        ///<param name="neighbors"></param>
        ///<returns></returns>
        public Vector2 Separation(List<BotEntity> neighbors)
        {
            //iterate through all the neighbors and calculate the vector from the
            Vector2 steeringForce = Vector2.Zero;

            foreach (BotEntity curBot in neighbors)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough. ***also make sure it doesn't
                //include the evade target ***
                if ((curBot == Bot) || !curBot.IsTagged || (curBot == TargetAgent1))
                    continue;

                Vector2 toAgent = Bot.Position - curBot.Position;

                //scale the force inversely proportional to the agents distance  
                //from its neighbor.
                steeringForce += (Vector2.Normalize(Vector2.Normalize(toAgent) - curBot.Heading) * Bot.SceneObject.Size.X) / toAgent.Length();
            }

            return steeringForce;
        }

        ///<summary>
        ///Render the feelers
        ///</summary>
        public void Render()
        {
            for (int flr = 0; flr < Feelers.Count; ++flr)
            {
                DrawUtil.Line(Bot.Position, Feelers[flr], Color.Red);
            }
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///Tests if a specific bit of <see cref="_flags"/> is set
        ///</summary>
        ///<param name="bt"></param>
        ///<returns>
        ///true if a specific bit of <see cref="_flags"/> is set
        ///</returns>
        private bool On(BehaviorTypes bt)
        {
            return (Flags & (int) bt) == (int) bt;
        }

        #endregion

        #region Private, protected, internal fields

        private readonly BotEntity _bot;
        private readonly Decelerations _deceleration;
        private readonly float _viewDistance;
        private readonly float _wallDetectionFeelerLength;
        private readonly float _wanderDistance;
        private readonly float _wanderJitter;
        private readonly float _wanderRadius;
        private readonly float _weightArrive;
        private readonly float _weightSeek;
        private readonly float _weightSeparation;
        private readonly float _weightWallAvoidance;
        private readonly float _weightWander;
        private bool _cellSpaceIsOn;
        private List<Vector2> _feelers;
        private int _flags;
        private Vector2 _steeringForce;
        private SummingMethods _summingMethod;
        private Vector2 _target;
        private BotEntity _targetAgent1;
        private BotEntity _targetAgent2;
        private Vector2 _wanderTarget;

        [Flags]
        private enum BehaviorTypes
        {
            //None = 0x00000,
            Seek = 0x00002,
            Arrive = 0x00008,
            Wander = 0x00010,
            Separation = 0x00040,
            WallAvoidance = 0x00200,
        } ;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}