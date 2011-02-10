#region File description

//------------------------------------------------------------------------------
//WeaponSystem.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Util;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.Tx2D.GameAI;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters
using Mindcrafters.RavenX.Goal;
#endregion

#endregion

namespace Mindcrafters.RavenX.Armory
{
    ///<summary>
    ///class to implement the bot's weapon system
    ///</summary>
    public class WeaponSystem
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        ///<param name="reactionTime"></param>
        ///<param name="aimAccuracy"></param>
        ///<param name="aimPersistence"></param>
        public WeaponSystem(
            BotEntity owner,
            float reactionTime,
            float aimAccuracy,
            float aimPersistence)
        {
            _owner = owner;
            _reactionTime = reactionTime;
            _aimAccuracy = aimAccuracy;
            _aimPersistence = aimPersistence;
            Initialize();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the current weapon
        ///</summary>
        public Weapon CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                _currentWeapon = value;
                _owner.HoldWeapon(_currentWeapon);
            }
        }

        ///<summary>
        ///reaction time
        ///</summary>
        public float ReactionTime
        {
            get { return _reactionTime; }
        }

        /// <summary>
        /// the number of weapons we currently hold
        /// </summary>
        public int NumberOfWeaponsInInventory
        {
            get
            {
                int count = 0;
                foreach (KeyValuePair<WeaponTypes, Weapon> currentMapping in _weaponMap)
                    if (currentMapping.Value != null)
                        count++;
                return count;
            }
        }

        #region Public methods

        ///<summary>
        ///initializes the weapons
        ///</summary>
        public void Initialize()
        {
            _weaponMap.Clear();

            //set up the container
            CurrentWeapon = new WeaponBlaster(_owner);

            _weaponMap[WeaponTypes.Blaster] = CurrentWeapon;
            _weaponMap[WeaponTypes.Shotgun] = null;
            _weaponMap[WeaponTypes.Railgun] = null;
            _weaponMap[WeaponTypes.RocketLauncher] = null;
        }

        ///<summary>
        ///Select weapon
        ///</summary>
        public void SelectWeapon()
        {
            //if a target is present use fuzzy logic to determine the most
            //desirable weapon.
            if (_owner.TargetingSystem.IsTargetPresent)
            {
                //calculate the distance to the target
                float distToTarget =
                    (_owner.Position -
                     _owner.TargetingSystem.Target.Position).Length();

                //for each weapon in the inventory calculate its desirability
                //given the current situation. The most desirable weapon is
                //selected
                float bestSoFar = Single.MinValue;

                foreach (KeyValuePair<WeaponTypes, Weapon> kvp in _weaponMap)
                {
                    //grab the desirability of this weapon (desirability is
                    //based upon distance to target and ammo remaining)
                    if (kvp.Value == null)
                        continue;

                    float score = kvp.Value.GetDesirability(distToTarget);

                    //if it is the most desirable so far select it
                    if (score <= bestSoFar)
                        continue;

                    bestSoFar = score;

                    //place the weapon in the bot's hand.
                    CurrentWeapon = kvp.Value;
                }
            }
            else
            {
                CurrentWeapon = _weaponMap[WeaponTypes.Blaster];
            }
        }

        ///<summary>
        ///this is called by a weapon affector and will add a weapon of the 
        ///specified type to the bot's inventory.
        ///
        ///if the bot already has a weapon of this type, only the ammo is added
        ///</summary>
        ///<param name="weaponType"></param>
        public void AddWeapon(WeaponTypes weaponType)
        {
            //create an instance of this weapon
            Weapon w = null;

            switch (weaponType)
            {
                case WeaponTypes.Railgun:
                    w = new WeaponRailgun(_owner);
                    break;

                case WeaponTypes.Shotgun:
                    w = new WeaponShotgun(_owner);
                    break;

                case WeaponTypes.RocketLauncher:
                    w = new WeaponRocketLauncher(_owner);
                    break;
            }

            if (null == w)
                return;


            //if the bot already holds a weapon of this type, just add its ammo
            Weapon present = GetWeaponFromInventory(weaponType);

            if (present != null)
            {
                present.IncrementRounds(w.NumRoundsRemaining);
            }
                //if not already holding, add to inventory
            else
            {
                _weaponMap[weaponType] = w;
            }
        }

        ///<summary>
        ///Get any matching weapon in inventory or
        ///null pointer if the weapon is not present
        ///</summary>
        ///<param name="weaponType"></param>
        ///<returns>
        ///any matching weapon in inventory or
        ///null pointer if the weapon is not present
        ///</returns>
        public Weapon GetWeaponFromInventory(WeaponTypes weaponType)
        {
            return _weaponMap[weaponType];
        }

        ///<summary>
        ///Change weapon
        ///</summary>
        ///<param name="weaponType"></param>
        public void ChangeWeapon(WeaponTypes weaponType)
        {
            Weapon w = GetWeaponFromInventory(weaponType);

            if (w != null)
            {
                CurrentWeapon = w;
            }
        }

        ///<summary>
        ///this method aims the bots current weapon at the target (if there is
        ///a target) and, if aimed correctly, fires a round
        ///</summary>
        ///<param name="dt"></param>
        public void TakeAimAndShoot(float dt)
        {
            //aim the weapon only if the current target is shootable or if it 
            //has only very recently gone out of view (this latter condition is
            //to ensure the weapon is aimed at the target even if it temporarily
            //dodges behind a wall or other cover)
            if (_owner.TargetingSystem.IsTargetShootable ||
                (_owner.TargetingSystem.TimeTargetOutOfView < _aimPersistence))
            {
                //If any teammates are obstructing the line between this bot and the target, 
                //send a message to him to move out of the way.
                //now in IF statement above. It returns true if a teammate is in the way.

                //the position the weapon will be aimed at
                _aimingPos = _owner.TargetBot.Position;

                //if the current weapon is not an instant hit type gun the
                //target position must be adjusted to take into account the
                //predicted movement of the target
                if (CurrentWeapon.WeaponType == WeaponTypes.RocketLauncher ||
                    CurrentWeapon.WeaponType == WeaponTypes.Blaster)
                {
                    _aimingPos = PredictFuturePositionOfTarget(dt);

                    //if the weapon is aimed correctly, there is line of sight
                    //between the bot and the aiming position and it has been in
                    //view for a period longer than the bot's reaction time, 
                    //shoot the weapon
                    if (_owner.RotateFacingTowardPosition(_aimingPos) &&
                        (_owner.TargetingSystem.TimeTargetVisible > _reactionTime) &&
                        _owner.HasLOS(_aimingPos) && !handleTeamObstructions())
                    {
                        AddNoiseToAim(ref _aimingPos);
                        CurrentWeapon.ShootAt(_aimingPos);
                    }
                }
                    //no need to predict movement, aim directly at target
                else
                {
                    //if the weapon is aimed correctly and it has been in view
                    //for a period longer than the bot's reaction time, shoot
                    //the weapon
                    if (_owner.RotateFacingTowardPosition(_aimingPos) &&
                        (_owner.TargetingSystem.TimeTargetVisible >
                         _reactionTime) && !handleTeamObstructions())
                    {
                        AddNoiseToAim(ref _aimingPos);
                        CurrentWeapon.ShootAt(_aimingPos);
                    }
                }
            }
                //no target to shoot at so rotate facing to be parallel with the
                //bot's heading direction
            else
            {
                _owner.RotateFacingTowardPosition(
                    _owner.Position + _owner.Heading);
            }
        }
        /// <summary>
        /// Checks to see if any teammates are in the line of fire. 
        /// If they are this function will send them a message to move perpendicular to the line of sight away.
        /// </summary>
        private bool handleTeamObstructions()
        {
            bool teammateInTheWay = false;

            Vector2 lineOfSight = _owner.TargetBot.Position - _owner.Position;
            Vector2 lineOfSightNorm = lineOfSight;  //assuming this will pass by value.
            lineOfSightNorm.Normalize();

            List<BotEntity> teammates = _owner.SensoryMemory.GetListOfRecentlySensedTeammates();
            foreach (BotEntity teammate in teammates)
            {
                Vector2 lineToTeammate = teammate.Position - _owner.Position;
                Vector2 lineToTeammateNorm = lineToTeammate;
                lineToTeammateNorm.Normalize();

                //the threshhold is determined by the size of the teammate and the distance from the teammate.
                //the further the distance (larger the length), the lower the threshhold of being in LoS.
                //the larger the teammate, the higher the threshhold of being in LoS.

                //Their could be a static threshhold, but then teammates at a distance would be considered obstructions often. (Think a fan/cone shape)
                //.707 is approximtely the maximum amount the normalized component can be and I don't want it to be huge.
                float threshhold =
                    Math.Max(Math.Min(teammate.SceneObject.Size.X/lineToTeammate.LengthSquared(), 0.001f), (.707f / 2));

                //if both the x and y are within the threshhold, then the teammate is in the way.(similar direction, closer than opponent) Send a message to him to get out of the way.
                if (Math.Abs(lineToTeammateNorm.X - lineOfSightNorm.X) < threshhold && Math.Abs(lineToTeammateNorm.X - lineOfSightNorm.X) < threshhold && lineOfSight.Length() > lineToTeammate.Length())
                {
                    teammateInTheWay = true;

                    //if the teammate is currently just attacking a target, tell him to move out of the way. 
                    //If he's doing anything else he will more than likely move out of the way anyways, 
                    //or eventually move out of the way or be attacking to and then get the message.
                    if (teammate.Brain.Subgoals.Count == 0 || teammate.Brain.Subgoals.Peek().GoalType == Goal.Goal.GoalTypes.AttackTarget)
                    {
                        //inverse of the line of sight vector.
                        Vector2 outOfTheWay = new Vector2(lineOfSightNorm.Y, lineOfSightNorm.X);
                        
                        //Mulitply this by the size of the bot, so the bot moves fully out of the line of sight.
                        outOfTheWay *= teammate.SceneObject.Size.X;

                        //negative inverse of the line of sight vector. (if in quadrant 1 or 3[both +ve or both -ve], flip y, otherwise flip x.)
                        if (outOfTheWay.X > 0 ^ outOfTheWay.Y > 0)
                            outOfTheWay.Y *= -1;
                        else
                            outOfTheWay.X *= -1;

                        System.Console.WriteLine("dodging out of fire! team: " + teammate.Team + ", position: " + teammate.Position + ", enemies Position: " + _owner.TargetBot.Position + ", lineOfSight: " + lineOfSight);
                        
                        //see if the bot can move the this new vector. 
                        //[we add the current position, because our vector is in relation to our current position.]
                        //(more consie it's the amount and direction the bot has to move)
                        if (teammate.CanWalkTo(outOfTheWay + teammate.Position))
                            teammate.Brain.Subgoals.Push(new Goal.Atomic.SeekToPosition(teammate, outOfTheWay + teammate.Position));
                        else
                        {
                            //if that direction failed, try the opposite direction.
                            outOfTheWay.X *= -1;
                            outOfTheWay.Y *= -1;

                            if (teammate.CanWalkTo(outOfTheWay + teammate.Position))
                            {
                                teammate.Brain.Subgoals.Push(new Goal.Atomic.SeekToPosition(teammate, outOfTheWay + teammate.Position));
                            }
                            else
                            {
                                //if we can't go in either perpendicular directions, then just run straight towards the target, and a little past him.
                                outOfTheWay = (lineOfSight - (lineToTeammate/2)) + teammate.Position;
                                teammate.Brain.Subgoals.Push(new Goal.Atomic.SeekToPosition(teammate, outOfTheWay));
                            }
                        }
                    }
                }
            }

            return teammateInTheWay;
        }

        ///<summary>
        ///adds a random deviation to the firing angle not greater than 
        ///<see cref="_aimAccuracy"/> radians
        ///</summary>
        ///<param name="aimingPos"></param>
        public void AddNoiseToAim(ref Vector2 aimingPos)
        {
            Vector2 toPos = aimingPos - _owner.Position;

            Transformations.RotateVectorAroundOrigin(
                ref toPos,
                TorqueUtil.GetFastRandomFloat(-_aimAccuracy, _aimAccuracy));

            aimingPos = toPos + _owner.Position;
        }

        ///<summary>
        ///predicts where the target will be located in the time it takes for a
        ///projectile to reach it. This uses a similar logic to the Pursuit 
        ///steering behavior.
        ///</summary>
        ///<param name="dt"></param>
        ///<returns></returns>
        public Vector2 PredictFuturePositionOfTarget(float dt)
        {
            float maxSpeed = CurrentWeapon.MaxProjectileSpeed;

            //if the target is ahead and facing the agent shoot at its
            //current position
            Vector2 toEnemy = _owner.TargetBot.Position - _owner.Position;

            //the lookahead time is proportional to the distance between the
            //enemy and the pursuer; and is inversely proportional to the sum
            //of the agents' velocities
            float lookAheadTime =
                toEnemy.Length()/(maxSpeed + _owner.TargetBot.MaxSpeed);

            //return the predicted future position of the enemy
            return _owner.TargetBot.Position +
                   _owner.TargetBot.Velocity*lookAheadTime*dt;
        }

        ///<summary>
        ///Gets the amount of ammo remaining for the specified weapon. 
        ///</summary>
        ///<param name="weaponType"></param>
        ///<returns>
        ///the amount of ammo remaining for the specified weapon or zero if the 
        ///weapon is not in the inventory
        ///</returns>
        public int GetAmmoRemaining(WeaponTypes weaponType)
        {
            return _weaponMap[weaponType] != null
                       ?
                           _weaponMap[weaponType].NumRoundsRemaining
                       : 0;
        }

        ///<summary>
        ///shoots the current weapon at the given position
        ///</summary>
        ///<param name="position"></param>
        public void ShootAt(Vector2 position)
        {
            CurrentWeapon.ShootAt(position);
        }

        ///<summary>
        ///Render the current weapon. If the weapon is a T2DSceneObject, it
        ///will be rendered by the engine, but we may still want to render
        ///some additional info
        ///</summary>
        public void RenderCurrentWeapon()
        {
            CurrentWeapon.Render();
        }

        ///<summary>
        ///Render desirability scores for the weapons
        ///</summary>
        public void RenderDesirabilities()
        {
            Vector2 p = _owner.Position;
            int num = 0;

            foreach (KeyValuePair<WeaponTypes, Weapon> kvp in _weaponMap)
            {
                if (kvp.Value != null) num++;
            }

            int offset = 15*num;

            foreach (KeyValuePair<WeaponTypes, Weapon> kvp2 in _weaponMap)
            {
                if (kvp2.Value == null)
                    continue;

                float score = kvp2.Value.LastDesirabilityScore;
                string type =
                    EnumUtil.GetDescription(
                        Entity.Entity.WeaponTypeToEntityType(
                            kvp2.Value.WeaponType));

                TextUtil.DrawText(
                    new Vector2(
                        p.X + 10.0f, p.Y - offset),
                    score.ToString("F2") + " " + type);

                offset += 15;
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //each time the current weapon is fired a certain amount of random
        //noise is added to the the angle of the shot. This prevents the bots
        //from hitting their opponents 100% of the time. The lower this value
        //the more accurate a bot's aim will be. Recommended values are between
        //0 and 0.2 (the value represents the max deviation in radians that can
        //be added to each shot).
        private readonly float _aimAccuracy;

        //the amount of time a bot will continue aiming at the position of the
        //target even if the target disappears from view.
        private readonly float _aimPersistence;
        private readonly BotEntity _owner;
        private readonly float _reactionTime;

        private readonly Dictionary<WeaponTypes, Weapon> _weaponMap =
            new Dictionary<WeaponTypes, Weapon>();

        private Vector2 _aimingPos;
        private Weapon _currentWeapon;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}