#region File description

//------------------------------------------------------------------------------
//Weapon.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Fuzzy;

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Armory
{
    ///<summary>
    ///Base class for all weapons
    ///</summary>
    public abstract class Weapon
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="weaponType"></param>
        ///<param name="defaultNumRounds"></param>
        ///<param name="maxRoundsCarried"></param>
        ///<param name="rateOfFire"></param>
        ///<param name="idealRange"></param>
        ///<param name="projectileSpeed"></param>
        ///<param name="ownerOfGun"></param>
        protected Weapon(
            WeaponTypes weaponType,
            int defaultNumRounds,
            int maxRoundsCarried,
            float rateOfFire,
            float idealRange,
            float projectileSpeed,
            BotEntity ownerOfGun)
        {
            _weaponType = weaponType;
            _numRoundsLeft = defaultNumRounds;
            _owner = ownerOfGun;
            _rateOfFire = rateOfFire;
            _maxRoundsCarried = maxRoundsCarried;
            _lastDesirabilityScore = 0;
            _idealRange = idealRange;
            _maxProjectileSpeed = projectileSpeed;
            _timeNextAvailable = Time.TimeNow;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the desirability score calculated in the last call to 
        ///<see cref="GetDesirability"/> (just used for debugging)
        ///</summary>
        ///<returns>
        ///the desirability score calculated in the last call to
        ///<see cref="GetDesirability"/>
        ///</returns>
        public float LastDesirabilityScore
        {
            get { return _lastDesirabilityScore; }
            set { _lastDesirabilityScore = value; }
        }

        ///<summary>
        ///the maximum speed of the projectile this weapon fires
        ///</summary>
        public float MaxProjectileSpeed
        {
            get { return _maxProjectileSpeed; }
        }

        ///<summary>
        ///the number of rounds remaining for the weapon
        ///</summary>
        public int NumRoundsRemaining
        {
            get { return _numRoundsLeft; }
        }

        ///<summary>
        ///the weapon type
        ///</summary>
        public WeaponTypes WeaponType
        {
            get { return _weaponType; }
        }

        ///<summary>
        ///this is the preferred distance from the enemy when using this weapon
        ///</summary>
        public float IdealRange
        {
            get { return _idealRange; }
        }

        ///<summary>
        ///The owner of this weapon. A weapon is always (in this game) carried
        ///by a bot.
        ///</summary>
        public BotEntity Owner
        {
            get { return _owner; }
        }

        ///<summary>
        //fuzzy logic is used to determine the desirability of a weapon. Each
        //weapon owns its own instance of a fuzzy module because each has a 
        //different rule set for inferring desirability.
        ///</summary>
        public FuzzyModule FuzzyModule
        {
            get { return _fuzzyModule; }
            set { _fuzzyModule = value; }
        }

        ///<summary>
        ///maximum number of rounds a bot can carry for this weapon
        ///</summary>
        public int MaxRoundsCarried
        {
            get { return _maxRoundsCarried; }
        }

        ///<summary>
        ///the number of times this weapon can be fired per second
        ///</summary>
        public float RateOfFire
        {
            get { return _rateOfFire; }
        }

        ///<summary>
        ///the earliest time the next shot can be taken
        ///</summary>
        public float TmeNextAvailable
        {
            get { return _timeNextAvailable; }
        }

        public float PercentageRoundsLeft
        {
            get
            {
                if (_weaponType == WeaponTypes.Blaster)
                    return 0;
                return NumRoundsRemaining/(float) MaxRoundsCarried;
            }
        }

        #region Public methods

        ///<summary>
        ///this method aims the weapon at the given target by rotating the 
        ///weapon's owner's facing direction (constrained by the bot's turning
        ///rate).
        ///</summary>
        ///<param name="target"></param>
        ///<returns>true if the weapon is directly facing the target.</returns>
        public bool AimAt(Vector2 target)
        {
            return _owner.RotateFacingTowardPosition(target);
        }

        ///<summary>
        ///this discharges a projectile from the weapon at the given target
        ///position (provided the weapon is ready to be discharged... every 
        ///weapon has its own rate of fire)
        ///</summary>
        ///<param name="pos"></param>
        public abstract void ShootAt(Vector2 pos);

        ///<summary>
        ///Render the weapon. If the weapon is a T2DSceneObject, it
        ///will be rendered by the engine, but we may still want to render
        ///some additional info
        ///</summary>
        public abstract void Render();

        ///<summary>
        ///this method returns a value representing the desirability of using the
        ///weapon. This is used by the AI to select the most suitable weapon for
        ///a bot's current situation. This value is calculated using fuzzy logic
        ///</summary>
        ///<param name="distToTarget"></param>
        ///<returns>
        ///a value representing the desirability of using the weapon
        ///</returns>
        public abstract float GetDesirability(float distToTarget);

        ///<summary>
        ///decrement the number of rounds of ammo for this weapon
        ///</summary>
        public void DecrementRounds()
        {
            if (_numRoundsLeft > 0) --_numRoundsLeft;
        }

        ///<summary>
        ///increment the number of rounds of ammo for this weapon
        ///</summary>
        ///<param name="num"></param>
        public void IncrementRounds(int num)
        {
            _numRoundsLeft += num;
            _numRoundsLeft =
                MathUtil.Clamp(_numRoundsLeft, 0, _maxRoundsCarried);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///The number of times a weapon can be discharges depends on its rate
        // of fire. This method returns true if the weapon is able to be 
        // discharged at the current time. (called from ShootAt() )
        ///</summary>
        ///<returns>true if the weapon is ready to be discharged</returns>
        protected bool IsReadyForNextShot()
        {
            return Time.TimeNow > _timeNextAvailable;
        }

        ///<summary>
        ///this is called when a shot is fired to update
        ///<see cref="_timeNextAvailable"/>
        ///</summary>
        protected void UpdateTimeNextAvailable()
        {
            _timeNextAvailable = Time.TimeNow + 1.0f/_rateOfFire;
        }

        ///<summary>
        ///this method initializes the fuzzy module with the appropriate fuzzy 
        ///variables and rule base.
        ///</summary>
        protected abstract void InitializeFuzzyModule();

        #endregion

        #region Private, protected, internal fields

        private readonly float _idealRange;
        private readonly float _maxProjectileSpeed;
        private readonly int _maxRoundsCarried;
        private readonly BotEntity _owner;
        private readonly float _rateOfFire;
        private readonly WeaponTypes _weaponType;
        private FuzzyModule _fuzzyModule = new FuzzyModule();
        private float _lastDesirabilityScore;
        private int _numRoundsLeft;
        private float _timeNextAvailable;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}