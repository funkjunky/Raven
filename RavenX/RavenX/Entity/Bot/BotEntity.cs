#region File description

//------------------------------------------------------------------------------
//BotEntity.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.GUI;
using GarageGames.Torque.T2D;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Armory;
using Mindcrafters.RavenX.Goal.Composite;
using Mindcrafters.RavenX.Map;
using Mindcrafters.RavenX.Memory.NavigationalMemory;
using Mindcrafters.RavenX.Memory.SensoryMemory;
using Mindcrafters.RavenX.Messaging;
using Mindcrafters.RavenX.Navigation;
using Mindcrafters.RavenX.Trigger;
using Mindcrafters.Tx2D.GameAI;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Bot
{
    ///<summary>
    /// bot team formation status
    ///</summary>
    public enum FormationStatuses
    {
        NotReady,
        Ready,
        Leading,
        Following,
        HeadingToPosition,
        OnErrand
    };

    ///<summary>
    ///class for bots 
    ///</summary>
    public class BotEntity : MovingEntity
    {
        #region Static methods, fields, constructors

        private static readonly string _logUpdateText =
            String.Format("{0,-23}] ", "Update");

        ///<summary>
        ///log text for Update method(
        ///</summary>
        protected static string LogUpdateText
        {
            get { return _logUpdateText; }
        }

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="entitySceneObject"></param>
        ///<param name="pos"></param>
        public BotEntity(IEntitySceneObject entitySceneObject, Vector2 pos)
            : base(
                entitySceneObject,
                pos,
                GameManager.GameManager.Instance.Parameters.BotScale,
                Vector2.Zero,
                GameManager.GameManager.Instance.Parameters.BotMaxSpeed,
                Vector2.One, // initial heading
                GameManager.GameManager.Instance.Parameters.BotMass,
                Vector2.Normalize(Vector2.One)*
                GameManager.GameManager.Instance.Parameters.BotScale,
                GameManager.GameManager.Instance.Parameters.BotMaxHeadTurnRate,
                GameManager.GameManager.Instance.Parameters.BotMaxForce
                )
        {
            _maxHealth = GameManager.GameManager.Instance.Parameters.BotMaxHealth;
            CurrentHealth = 10;
            _pathPlanner = null;
            _steering = null;
            _brain = null;
            NumUpdatesHitPersistent =
                (int) GameManager.GameManager.Instance.Parameters.HitFlashTime;
            Hit = false;
            Score = 0;
            _status = Statuses.Spawning;
            IsPossessed = false;
            _fieldOfView =
                MathHelper.ToRadians(GameManager.GameManager.Instance.Parameters.BotFOV);
            _maxNormalSpeed = GameManager.GameManager.Instance.Parameters.BotMaxSpeed;
            _maxSwimmingSpeed =
                GameManager.GameManager.Instance.Parameters.BotMaxSwimmingSpeed;
            _maxCrawlingSpeed =
                GameManager.GameManager.Instance.Parameters.BotMaxCrawlingSpeed;

            EntityType = EntityTypes.Bot;

            //a bot starts off facing in the direction it is heading
            Facing = Heading;

            //create the navigation module
            _pathPlanner = new PathPlanner(this);

            //create the steering behavior class
            _steering = new Steering.Steering(this);

            //create the regulators
            _weaponSelectionRegulator =
                new Regulator(
                    GameManager.GameManager.Instance.Parameters.BotWeaponSelectionFrequency);
            _goalArbitrationRegulator =
                new Regulator(
                    GameManager.GameManager.Instance.Parameters.BotGoalAppraisalUpdateFreq);
            _targetSelectionRegulator =
                new Regulator(
                    GameManager.GameManager.Instance.Parameters.BotTargetingUpdateFreq);
            _triggerTestRegulator =
                new Regulator(
                    GameManager.GameManager.Instance.Parameters.BotTriggerUpdateFreq);
            _visionUpdateRegulator =
                new Regulator(
                    GameManager.GameManager.Instance.Parameters.BotVisionUpdateFreq);

            //create the goal queue
            _brain = new Think(this);

            //create the targeting system
            _targetingSystem = new TargetingSystem(this);

            _weaponSystem = new WeaponSystem(this,
                                             GameManager.GameManager.Instance.Parameters.BotReactionTime,
                                             GameManager.GameManager.Instance.Parameters.BotAimAccuracy,
                                             GameManager.GameManager.Instance.Parameters.BotAimPersistence);

            _sensoryMemory =
                new SensoryMemory(
                    this,
                    GameManager.GameManager.Instance.Parameters.BotMemorySpan);

            _navigationalMemory =
                new NavigationalMemory(this, GameManager.GameManager.Instance.Parameters.NavigationMemorySpan);

            //temp debbugging code
            _navigationalMemory.DumpFrequency = 10000; // 10 seconds

            FoundTriggers = new FoundTriggerList(Team);

            _logPrefixText =
                String.Format("[{0,-8}] [{1,17}.", Name, "BotEntity");

        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region Statuses enum

        ///<summary>
        ///bot states
        ///</summary>
        public enum Statuses
        {
            Alive,
            Dead,
            Spawning
        } ;

        #endregion

        ///<summary>
        ///bot state
        ///</summary>
        public Statuses Status
        {
            get { return _status; }
        }

        ///<summary>
        ///position for head to look at
        ///<remarks>
        ///Heading: direction of movement
        ///Facing: direction to shoot (tied to rotation). Not right for tanks
        ///        with turrets but okay for bipedal bots. Probably should
        ///        separate into aim (upper body/turret) and facing (lower body).
        ///LookTarget: direction of sight (head orientation, determines FOV)
        ///</remarks>
        ///</summary>
        public T2DSceneObject LookTarget
        {
            get { return _lookTarget; }
            set { _lookTarget = value; }
        }

        ///<summary>
        ///team
        ///</summary>
        public GameManager.GameManager.Teams Team
        {
            get { return _team; }
            set { _team = value; }
        }

        ///<summary>
        ///rank
        ///</summary>
        public GameManager.GameManager.Ranks Rank
        {
            get { return _rank; }
            set { _rank = value; }
        }

        ///<summary>
        ///Maximum normal (walking) speed
        ///</summary>
        public float MaxNormalSpeed
        {
            get { return _maxNormalSpeed; }
        }

        ///<summary>
        ///Maximum crawling speed
        ///</summary>
        public float MaxCrawlingSpeed
        {
            get { return _maxCrawlingSpeed; }
        }

        ///<summary>
        ///Maximum swimming speed
        ///</summary>
        public float MaxSwimmingSpeed
        {
            get { return _maxSwimmingSpeed; }
        }

        ///<summary>
        ///the percentage of health the bot has remaining
        ///</summary>
        public float HealthPercentage
        {
            get { return CurrentHealth/(float)MaxHealth; }
        }

        ///<summary>
        ///the bot's health. Every time the bot is shot this value is decreased.
        ///If it reaches zero then the bot dies (and respawns).
        ///</summary>
        public int CurrentHealth
        {
            get { return _health; }
            set { _health = value; }
        }

        /// <summary>
        /// List of triggers on the map that we have discovered (found)
        /// </summary>
        public FoundTriggerList FoundTriggers
        {
            get { return _foundTriggers; }
            set { _foundTriggers = value; }
        }

        ///<summary>
        ///the bot's maximum health value. It starts its life with health at
        ///this value
        ///</summary>
        public int MaxHealth
        {
            get { return _maxHealth; }
        }

        ///<summary>
        ///each time this bot kills another this value is incremented
        ///</summary>
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        ///<summary>
        ///a bot only perceives other bots within this field of view
        ///</summary>
        public float FieldOfView
        {
            get { return _fieldOfView; }
        }

        ///<summary>
        ///true if player controlled
        ///</summary>
        public bool IsPossessed
        {
            get { return _isPossessed; }
            set { _isPossessed = value; }
        }

        ///<summary>
        ///to show that a player has been hit it is surrounded by a thick 
        ///red circle for a fraction of a second. This variable represents
        ///the number of update-steps the circle gets drawn
        ///</summary>
        public int NumUpdatesHitPersistent
        {
            get { return _numUpdatesHitPersistent; }
            set { _numUpdatesHitPersistent = value; }
        }

        ///<summary>
        ///set to true when the bot is hit, and remains true until 
        ///<see cref="_numUpdatesHitPersistent"/> becomes zero. (used by the
        ///render method to draw a thick red circle around a bot to indicate
        ///it's been hit)
        ///</summary>
        public bool Hit
        {
            get { return _hit; }
            set { _hit = value; }
        }


        ///<summary>
        ///Is this bot currently carrying the flag
        ///</summary>
        public bool IsCarryingFlag
        {
            get { return _carryingFlag; }
            set{ _carryingFlag = value;}
        }

        ///<summary>
        ///dead
        ///</summary>
        public bool IsDead
        {
            get { return Status == Statuses.Dead; }
        }

        ///<summary>
        ///alive
        ///</summary>
        public bool IsAlive
        {
            get { return Status == Statuses.Alive; }
        }

        ///<summary>
        ///spawning (transitioning from dead to alive)
        ///</summary>
        public bool IsSpawning
        {
            get { return Status == Statuses.Spawning; }
        }

        ///<summary>
        /// status of team formations
        ///</summary>
        public FormationStatuses FormationStatus
        {
            get { return _formationStatus; }
            set { _formationStatus = value; }
        }

        ///<summary>
        /// status of team formations
        ///</summary>
        //public Formation CurrentFormation
        //{
        //    get { return _currentFormation; }
        //    set { _currentFormation = value; }
        //}

        ///<summary>
        ///Steering behavior - determines velocity
        ///</summary>
        public Steering.Steering Steering
        {
            get { return _steering; }
        }

        ///<summary>
        ///Path planner -- find routes in graph
        ///</summary>
        public PathPlanner PathPlanner
        {
            get { return _pathPlanner; }
        }

        ///<summary>
        ///Top level goal. Determines best subgoal to pursue. This object
        ///handles the arbitration and processing of high level goals
        ///</summary>
        public Think Brain
        {
            get { return _brain; }
        }

        ///<summary>
        ///Determines target to attack
        ///</summary>
        public TargetingSystem TargetingSystem
        {
            get { return _targetingSystem; }
        }

        ///<summary>
        ///the target
        ///</summary>
        public BotEntity TargetBot
        {
            get { return TargetingSystem.Target; }
        }

        ///<summary>
        ///this handles all the weapons. and has methods for aiming, selecting
        ///and shooting them
        ///</summary>
        public WeaponSystem WeaponSystem
        {
            get { return _weaponSystem; }
        }

        ///<summary>
        ///Short-term memory. Whenever this bot sees or hears an opponent, 
        ///a record of the event is updated in the memory.
        ///</summary>
        public SensoryMemory SensoryMemory
        {
            get { return _sensoryMemory; }
        }


        ///<summary>
        ///Long-term navigational memory. The bot will build up memories of its
        ///travels and record some useful statistics about travel times, etc.
        ///</summary>
        public NavigationalMemory NavigationalMemory
        {
            get { return _navigationalMemory; }
        }

        ///<summary>
        ///limits the update frequency of weapon selection
        ///</summary>
        public Regulator WeaponSelectionRegulator
        {
            get { return _weaponSelectionRegulator; }
        }

        ///<summary>
        ///limits the update frequency of goal selection
        ///</summary>
        public Regulator GoalArbitrationRegulator
        {
            get { return _goalArbitrationRegulator; }
        }

        ///<summary>
        ///limits the update frequency of target selection
        ///</summary>
        public Regulator TargetSelectionRegulator
        {
            get { return _targetSelectionRegulator; }
        }

        ///<summary>
        ///limits the update frequency of trigger activation testing
        ///</summary>
        public Regulator TriggerTestRegulator
        {
            get { return _triggerTestRegulator; }
        }

        ///<summary>
        ///limits the update frequency of sensory vision
        ///</summary>
        public Regulator VisionUpdateRegulator
        {
            get { return _visionUpdateRegulator; }
        }

        #region Public methods

        ///<summary>
        ///increase the score
        ///</summary>
        public void IncrementScore()
        {
            ++Score;
        }

        ///<summary>
        ///set status to spawning
        ///</summary>
        public void SetSpawning()
        {
            _status = Statuses.Spawning;
        }

        ///<summary>
        ///set status to dead
        ///</summary>
        public void SetDead()
        {
            IsCarryingFlag = false;
            _status = Statuses.Dead;
        }

        /// <summary>
        /// drop the flag when we return it to our base
        /// </summary>
        public void ScoreFlag()
        {
            IsCarryingFlag = false;
        }

        ///<summary>
        ///set status to alive
        ///</summary>
        public void SetAlive()
        {
            _status = Statuses.Alive;
        }

        ///<summary>
        ///spawns the bot at the given position
        ///</summary>
        ///<param name="pos"></param>
        public void Spawn(Vector2 pos)
        {
            SetAlive();
            Brain.RemoveAllSubgoals();
            TargetingSystem.ClearTarget();
            Position = pos;
            WeaponSystem.Initialize();
            RestoreHealthToMaximum();
            FoundTriggers.Flush();
        }

        ///<summary>
        ///Update the bot entity (part of the game update logic)
        ///</summary>
        ///<param name="dt">time since last update</param>
        public override void Update(float dt)
        {
            //if bot bounding radius intersects a wall,
            //move the bot away from the wall
            HandleWallPenetration();

            LogUtil.WriteLineIfLogBotState(LogPrefixText + LogUpdateText +
                                           (IsPossessed
                                                ? "[Possessed]"
                                                :
                                                    (GameManager.GameManager.Instance.SelectedBot == this
                                                         ?
                                                             "[Selected ]"
                                                         : "[Normal   ]")) +
                                           " dt: " + dt.ToString("F4") +
                                           " Position: " + Vector2Util.ToString(Position) +
                                           " Velocity: " + Vector2Util.ToString(Velocity) +
                                           " Speed: " + Velocity.Length().ToString("F2") +
                                           " Heading: " +
                                           T2DVectorUtil.AngleFromVector(Heading).ToString("F2") +
                                           " Facing: " +
                                           T2DVectorUtil.AngleFromVector(Facing).ToString("F2"));

            //process the currently active goal.
            //Note this is required even if the bot is under user control.
            //This is because a goal is created whenever a user clicks on an
            //area of the map that necessitates a path planning request.
            Brain.Process();

            //Calculate the steering force and update the bot's velocity
            UpdateMovement(dt);

            //if the bot is under AI control but not scripted
            if (IsPossessed)
                return;

            //examine all the opponents in the bots sensory memory and select
            //one to be the current target
            if (TargetSelectionRegulator.IsReady)
            {
                TargetingSystem.Update();
            }

            //appraise and arbitrate between all possible high level goals
            if (GoalArbitrationRegulator.IsReady)
            {
                Brain.Arbitrate();
            }

            //update the sensory memory with any visual stimulus
            if (VisionUpdateRegulator.IsReady)
            {
                SensoryMemory.UpdateVision();
                CheckForNewVisibleTriggers();
            }

            //select the appropriate weapon to use from the weapons currently
            //in the inventory
            if (WeaponSelectionRegulator.IsReady)
            {
                WeaponSystem.SelectWeapon();
            }

            //this method aims the bot's current weapon at the current target
            //and takes a shot if a shot is possible
            WeaponSystem.TakeAimAndShoot(dt);

        }

        ///<summary>
        ///Tests if the bot is ready to be tested against the world triggers
        ///</summary>
        ///<returns>
        ///true if the bot is ready to be tested against the world triggers
        ///</returns>
        public bool IsReadyForTriggerUpdate()
        {
            return TriggerTestRegulator.IsReady;
        }

        //TODO: RECEIVE MESSAGE from EvaluatorFollowFormation
        //the message should set it's formationstatus to follow, 
        //or just tell me when the message is made so I can put int he content =)
        public void followFormation()
        {
            FormationStatus = FormationStatuses.Following;
        }

        ///<summary>
        ///Handle messages sent to this bot.
        ///</summary>
        ///<param name="msg">the message</param>
        ///<returns>true if message was handled by this bot</returns>
        public override bool HandleMessage(Telegram msg)
        {
            //first see if the current goal accepts the message
            if (Brain.HandleMessage(msg))
                return true;

            //handle any messages not handled by the goals
            switch (msg.Msg)
            {
                case MessageTypes.TakeThatMF:
                    //just return if already dead or spawning
                    if (IsDead || IsSpawning)
                        return true;

                    //the extra info field of the telegram carries
                    //the amount of damage
                    ReduceHealth((int) msg.ExtraInfo);

                    //if this bot is now dead let the shooter know
                    if (IsDead)
                    {
                        MessageDispatcher.Instance.DispatchMsg(
                            MessageDispatcher.SEND_MSG_IMMEDIATELY,
                            ObjectId,
                            msg.Sender,
                            MessageTypes.YouGotMeYouSOB,
                            MessageDispatcher.NO_ADDITIONAL_INFO);
                    }

                    return true;

                case MessageTypes.YouGotMeYouSOB:
                    IncrementScore();

                    //the bot this bot has just killed should
                    //be removed as the target
                    TargetingSystem.ClearTarget();

                    return true;

                case MessageTypes.GunshotSound:
                    //add the source of this sound to the bot's percepts
                    SensoryMemory.UpdateWithSoundSource((BotEntity) msg.ExtraInfo);

                    return true;

                case MessageTypes.UserHasRemovedBot:
                    BotEntity removedBot = (BotEntity) msg.ExtraInfo;
                    SensoryMemory.RemoveBotFromMemory(removedBot);

                    //if the removed bot is the target, make
                    //sure the target is cleared
                    if (removedBot == TargetingSystem.Target)
                    {
                        TargetingSystem.ClearTarget();
                    }

                    return true;


                default:
                    return false;
            }
        }

        ///<summary>
        ///given a target position, this method rotates the bot's facing vector
        ///by an amount not greater than <see cref="MovingEntity.MaxTurnRate"/>
        ///until it directly faces the target.
        ///</summary>
        ///<param name="target">the target position</param>
        ///<returns>
        ///true when the heading is facing in the desired direction
        /// </returns>
        public bool RotateFacingTowardPosition(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(target - Position);
            float dot = Vector2.Dot(Facing, toTarget);

            //clamp to rectify any rounding errors
            dot = MathHelper.Clamp(dot, -1, 1);

            //determine the angle between the heading vector and the target
            float angle = (float) Math.Acos(dot);

            //return true if the bot's facing is within WeaponAimTolerance
            //radians of facing the target
            //TODO: should be a parameter (different than BotAimAccuracy??)
            const float weaponAimTolerance = 0.01f; //2 degs approx

            if (angle < weaponAimTolerance)
            {
                Facing = toTarget;
                return true;
            }

            //clamp the amount to turn to the max turn rate
            if (angle > MaxTurnRate) angle = MaxTurnRate;

            //The next few lines use a rotation matrix to rotate the player's
            //facing vector accordingly
            Matrix2 rotationMatrix = new Matrix2();

            //notice how the direction of rotation has to be determined when
            //creating the rotation matrix
            rotationMatrix.Rotate(angle*Vector2Util.Sign(Facing, toTarget));
            Facing = rotationMatrix.TransformVector2(Facing);

            return false;
        }

        ///<summary>
        ///Reduce health by the given amount
        ///</summary>
        ///<param name="val">amount to reduce health by</param>
        public void ReduceHealth(int val)
        {
            CurrentHealth -= val;

            if (CurrentHealth <= 0)
            {
                SetDead();
            }

            Hit = true;

            //TODO: modify code to use time instead of update count
            NumUpdatesHitPersistent =
                (int) GameManager.GameManager.Instance.Parameters.HitFlashTime;
        }

        ///<summary>
        ///this is called to allow a human player to control the bot
        ///</summary>
        public void TakePossession()
        {
            if ((IsSpawning || IsDead))
                return;

            IsPossessed = true;

            LogUtil.WriteLineIfLogging("Player possesses bot <" + Name + ">");
        }

        ///<summary>
        ///called when a human is exorcised from this bot
        ///and the AI takes control
        ///</summary>
        public void Exorcise()
        {
            IsPossessed = false;

            //when the player is exorcised, the bot should resume normal service
            Brain.AddGoalExplore(); // don't just stand there looking stupid

            LogUtil.WriteLineIfLogging("Player is exorcised from bot <" +
                                       Name + ">");
        }

        ///<summary>
        ///change weapon to one of the given type if in inventory
        ///</summary>
        ///<param name="weaponType">weapon type to change to</param>
        public void ChangeWeapon(WeaponTypes weaponType)
        {
            WeaponSystem.ChangeWeapon(weaponType);
        }

        ///<summary>
        ///deploy/carry/mount/hold weapon
        ///</summary>
        ///<param name="weapon">weapon to hold</param>
        public void HoldWeapon(Weapon weapon)
        {
            ((IBotSceneObject) SceneObject).HoldWeapon(weapon);
        }

        ///<summary>
        ///fires the current weapon at the given position
        ///</summary>
        ///<param name="position"></param>
        public void FireWeapon(Vector2 position)
        {
            WeaponSystem.ShootAt(position);
        }

        ///<summary>
        ///Calculates (or looks up) a value indicating the time in seconds it
        ///will take the bot to reach the given position at its maximum speed.
        ///<remarks>
        ///This uses Euclidean distance and maximum velocity in its calculation.
        ///It doesn't take into account actual path length or the fact that bots
        ///don't necessarily follow the path exactly anyway. As well, it doesn't
        ///take acceleration into account so it probably underestimates short
        ///paths.
        ///</remarks>
        ///</summary>
        ///<param name="position"></param>
        ///<param name="time"></param>
        ///<returns></returns>
        public bool CalculateTimeToReachPosition(
            Vector2 position,
            out float time)
        {
            return NavigationalMemory.TimeToReachDestination(position, out time);
        }

        ///<summary>
        ///Calculates (or looks up) a value indicating the time in seconds it
        ///will take the bot to reach the given node at its maximum speed.
        ///</summary>
        ///<param name="nodeIndex"></param>
        ///<param name="time"></param>
        ///<returns></returns>
        public bool CalculateTimeToReachNode(
            int nodeIndex,
            out float time)
        {
            Vector2 position = PathPlanner.GetNodePosition(nodeIndex);
            return CalculateTimeToReachPosition(position, out time);
        }

        ///<summary>
        ///Compute and record actual travel time statistics
        ///</summary>
        ///<param name="mode"></param>
        ///<param name="source"></param>
        ///<param name="destination"></param>
        ///<param name="timeTaken"></param>
        public void RecordActualTimeToReachPosition(
            string mode,
            int source,
            int destination,
            float timeTaken)
        {
        }

        ///<summary>
        ///Tests if the bot is close to the given position
        ///</summary>
        ///<param name="position"></param>
        ///<returns>true if the bot is close to the given position</returns>
        public bool IsAtPosition(Vector2 position)
        {
            return
                IsAtPosition(
                    position,
                    GameManager.GameManager.Instance.Parameters.WaypointSeekDist);
        }

        ///<summary>
        ///Tests if the bot is close to the given position using the given
        ///satisfaction radius.
        ///</summary>
        ///<param name="position"></param>
        ///<param name="satisfactionRadius"></param>
        ///<returns>
        ///true if the bot is close to the given position using the given
        ///satisfaction radius
        /// </returns>
        public bool IsAtPosition(Vector2 position, float satisfactionRadius)
        {
            return (Position - position).LengthSquared() <
                   satisfactionRadius*satisfactionRadius;
        }

        ///<summary>
        ///Tests if the bot has line of sight to the given position.
        ///</summary>
        ///<param name="position"></param>
        ///<returns>
        ///true if the bot has line of sight to the given position.
        ///</returns>
        public bool HasLOS(Vector2 position)
        {
            return GameManager.GameManager.Instance.IsLOSOkay(Position, position);
        }

        ///<summary>
        ///Tests if if this bot can move directly to the given position
        ///without bumping into any walls
        ///</summary>
        ///<param name="position"></param>
        ///<returns>
        ///true if this bot can move directly to the given position
        ///without bumping into any walls
        ///</returns>
        public bool CanWalkTo(Vector2 position)
        {
            return !GameManager.GameManager.Instance.IsPathObstructed(
                        Position, position, BoundingRadius);
        }

        ///<summary>
        ///Tests if the bot can move between the two
        ///given positions without bumping into any walls
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns>
        ///true if the bot can move between the two
        ///given positions without bumping into any walls
        ///</returns>
        public bool CanWalkBetween(Vector2 from, Vector2 to)
        {
            return !GameManager.GameManager.Instance.IsPathObstructed(
                        from, to, BoundingRadius);
        }

        ///<summary>
        ///Tests if there is space enough to step in the indicated direction
        ///<paramref name="positionOfStep"/> is assigned the offset position
        ///</summary>
        ///<param name="positionOfStep"></param>
        ///<returns>
        ///true if there is space enough to step in the indicated direction
        ///</returns>
        public bool CanStepLeft(out Vector2 positionOfStep)
        {
            float stepDistance = BoundingRadius*2;

            positionOfStep =
                Position - Vector2Util.Perp(Facing)*stepDistance -
                Vector2Util.Perp(Facing)*BoundingRadius;

            return CanWalkTo(positionOfStep);
        }

        ///<summary>
        ///Tests if there is space enough to step in the indicated direction
        ///<paramref name="positionOfStep"/> is assigned the offset position
        ///</summary>
        ///<param name="positionOfStep"></param>
        ///<returns>
        ///true if there is space enough to step in the indicated direction
        ///</returns>
        public bool CanStepRight(out Vector2 positionOfStep)
        {
            float stepDistance = BoundingRadius*2;

            positionOfStep =
                Position + Vector2Util.Perp(Facing)*stepDistance +
                Vector2Util.Perp(Facing)*BoundingRadius;

            return CanWalkTo(positionOfStep);
        }

        ///<summary>
        ///Tests if there is space enough to step in the indicated direction
        ///<paramref name="positionOfStep"/> is assigned the offset position
        ///</summary>
        ///<param name="positionOfStep"></param>
        ///<returns>
        ///true if there is space enough to step in the indicated direction
        ///</returns>
        public bool CanStepForward(out Vector2 positionOfStep)
        {
            float stepDistance = BoundingRadius*2;

            positionOfStep =
                Position + Facing*stepDistance +
                Facing*BoundingRadius;

            return CanWalkTo(positionOfStep);
        }

        ///<summary>
        ///Tests if there is space enough to step in the indicated direction
        ///<paramref name="positionOfStep"/> is assigned the offset position
        ///</summary>
        ///<param name="positionOfStep"></param>
        ///<returns>
        ///true if there is space enough to step in the indicated direction
        ///</returns>
        public bool CanStepBackward(out Vector2 positionOfStep)
        {
            float stepDistance = BoundingRadius*2;

            positionOfStep =
                Position - Facing*stepDistance -
                Facing*BoundingRadius;

            return CanWalkTo(positionOfStep);
        }

        ///<summary>
        ///Render bot
        ///<remarks>
        ///If the entity is a T2DSceneObject, it will be rendered by the engine,
        ///but we may still want to render some additional info
        ///</remarks>
        ///</summary>
        public override void Render()
        {
            //TODO: modify to use time instead of update count
            //when a bot is hit by a projectile this value is set to a constant
            //user defined value which dictates how long the bot should have a
            //thick red circle drawn around it (to indicate it's been hit).
            //The circle is drawn as long as this value is positive.
            NumUpdatesHitPersistent--;

            if (IsDead || IsSpawning) return;

            //render the bot's weapon
            WeaponSystem.RenderCurrentWeapon();

            //render a thick red circle if the bot gets hit by a weapon
            if (Hit)
            {
                DrawUtil.Circle(Position, BoundingRadius + 1, Color.Red, 20);

                if (NumUpdatesHitPersistent <= 0)
                {
                    Hit = false;
                }
            }

            if (GameManager.GameManager.Instance.Options.ShowBotIds)
            {
                TextUtil.DrawText(
                    new Vector2(Position.X - 10, Position.Y - 20),
                    Color.Green,
                    ObjectId.ToString());
            }

            if (GameManager.GameManager.Instance.Options.ShowBotHealth)
            {
                TextUtil.DrawText(
                    new Vector2(Position.X - 40, Position.Y - 5),
                    Color.Green,
                    "H:" + CurrentHealth);
            }

            if (GameManager.GameManager.Instance.Options.ShowScore)
            {
                TextUtil.DrawText(
                    new Vector2(Position.X - 40, Position.Y + 10),
                    Color.Green,
                    "Scr:" + Score);
            }

            Steering.Render();
        }

        ///<summary>
        ///restore health to <see cref="MaxHealth"/>
        ///</summary>
        public void RestoreHealthToMaximum()
        {
            CurrentHealth = MaxHealth;
        }

        ///<summary>
        ///increase health by the given amount
        ///</summary>
        ///<param name="val">amount to add</param>
        public void IncreaseHealth(int val)
        {
            CurrentHealth += val;
            CurrentHealth = MathUtil.Clamp(CurrentHealth, 0, MaxHealth);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///common text prefix for log: "[BOT_$objectId$] [$goalType$."
        ///</summary>
        protected string LogPrefixText
        {
            get { return _logPrefixText; }
        }

        ///<summary>
        ///this method is called from the update method. It calculates and
        ///applies the steering force for this time-step.
        ///</summary>
        ///<param name="dt">time since last update</param>
        private void UpdateMovement(float dt)
        {
            //calculate the combined steering force
            Vector2 force = Steering.Calculate(dt);

            //if no steering force is produced deccelerate the player by
            //applying a braking force
            if (Epsilon.IsZero(Steering.Force))
            {
                //TODO: should be a parameter
                const float BRAKING_RATE = 0.2f;

                Velocity = Velocity*(1 - BRAKING_RATE*dt);
            }

            //calculate the acceleration
            Vector2 accel = force/Mass;

            //update the velocity
            Velocity += accel*dt;

            //make sure entity does not exceed maximum velocity
            Velocity = Vector2Util.Truncate(Velocity, MaxSpeed);

            //if the entity has a non zero velocity the heading and side
            //vectors must be updated
            if (Epsilon.IsZero(Velocity))
                return;

            Heading = Vector2.Normalize(Velocity);

            Side = Vector2Util.Perp(Heading);
        }

        /// <summary>
        /// checks to see if we have discovered a new trigger and adds
        /// it to the list of triggers that we know
        /// </summary>
        private void CheckForNewVisibleTriggers()
        {
            //check each trigger in the map
            foreach (Trigger.Trigger currentTrigger in GameManager.GameManager.Instance.Map.Triggers)
                if(GameManager.GameManager.Instance.IsLOSOkay(Position,currentTrigger.Position))
                {
                    if (!currentTrigger.Name.Contains("Flag") || !currentTrigger.Name.Contains(((int)Team).ToString()))
                        FoundTriggers.AddFoundItem(currentTrigger);
                }
        }

        ///<summary>
        ///Find any walls that the bot's bounding radius penetrates and
        ///back it off by the amount of penetration
        ///TODO: should we be using Entity.EnforceNonPenetrationConstraint?
        ///</summary>
        private void HandleWallPenetration()
        {
            List<Wall> penetratedWallList = new List<Wall>();

            //test against the walls
            foreach (Wall curWall in GameManager.GameManager.Instance.Map.Walls)
            {
                //do a line segment intersection test
                if (Geometry.LineSegmentCircleIntersection(
                    curWall.From, curWall.To, Position, BoundingRadius))
                {
                    penetratedWallList.Add(curWall);
                }
            }

            foreach (Wall curWall in penetratedWallList)
            {
                float penetration = BoundingRadius -
                                    Geometry.DistToLineSegment(
                                        curWall.From,
                                        curWall.To,
                                        Position);
                Position += penetration*curWall.Normal;
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly Think _brain;
        private readonly float _fieldOfView;
        private readonly Regulator _goalArbitrationRegulator;
        private readonly string _logPrefixText;
        private readonly float _maxCrawlingSpeed;
        private readonly int _maxHealth;
        private readonly float _maxNormalSpeed;
        private readonly float _maxSwimmingSpeed;
        private readonly NavigationalMemory _navigationalMemory;
        private readonly PathPlanner _pathPlanner;
        private readonly SensoryMemory _sensoryMemory;
        private readonly Steering.Steering _steering;
        private readonly TargetingSystem _targetingSystem;
        private readonly Regulator _targetSelectionRegulator;
        private readonly Regulator _triggerTestRegulator;
        private readonly Regulator _visionUpdateRegulator;
        private readonly Regulator _weaponSelectionRegulator;
        private readonly WeaponSystem _weaponSystem;
        private int _health;
        private bool _hit;
        private bool _isPossessed;
        private T2DSceneObject _lookTarget;
        private int _numUpdatesHitPersistent;
        private int _score;
        private Statuses _status;
        private FormationStatuses _formationStatus;
        //private Formation _currentFormation;
        private GameManager.GameManager.Teams _team;
        private GameManager.GameManager.Ranks _rank;
        private bool _carryingFlag;

        private FoundTriggerList _foundTriggers;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}
