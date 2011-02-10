#region File description

//------------------------------------------------------------------------------
//WeaponShotgun.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using GarageGames.Torque.Util;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Fuzzy;

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
    ///class to implement a shot gun
    ///</summary>
    public sealed class WeaponShotgun : Weapon
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        public WeaponShotgun(BotEntity owner)
            : base(WeaponTypes.Shotgun,
                   GameManager.GameManager.Instance.Parameters.ShotgunDefaultRounds,
                   GameManager.GameManager.Instance.Parameters.ShotgunMaxRoundsCarried,
                   GameManager.GameManager.Instance.Parameters.ShotgunFiringFreq,
                   GameManager.GameManager.Instance.Parameters.ShotgunIdealRange,
                   GameManager.GameManager.Instance.Parameters.PelletMaxSpeed,
                   owner)
        {
            _numBallsInShell =
                GameManager.GameManager.Instance.Parameters.ShotgunNumBallsInShell;
            _spread = GameManager.GameManager.Instance.Parameters.ShotgunSpread;

            //setup the fuzzy module
            InitializeFuzzyModule();
        }

        ///<summary>
        ///how many pellets each shell contains
        ///</summary>
        public int NumBallsInShell
        {
            get { return _numBallsInShell; }
        }

        ///<summary>
        ///how much the pellets spreads out when a cartridge is discharged
        ///</summary>
        public float Spread
        {
            get { return _spread; }
        }

        ///<summary>
        ///list of targets (each pellet has its own target)
        ///</summary>
        public List<Vector2> Targets
        {
            get { return _targets; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///this discharges a projectile from the weapon at the given target
        ///position (provided the weapon is ready to be discharged... every 
        ///weapon has its own rate of fire)
        ///</summary>
        ///<param name="pos"></param>
        public override void ShootAt(Vector2 pos)
        {
            if (NumRoundsRemaining <= 0 || !IsReadyForNextShot())
                return;

            //temp debugging code
            Targets.Clear();

            //a shotgun cartridge contains lots of tiny metal balls called
            //pellets. Therefore, every time the shotgun is discharged we 
            //have to calculate the spread of the pellets and add one for 
            //each trajectory
            for (int b = 0; b < NumBallsInShell; ++b)
            {
                //determine deviation from target using a bell curve type
                //distribution
                float deviation =
                    TorqueUtil.GetFastRandomFloat(0, Spread) +
                    TorqueUtil.GetFastRandomFloat(0, Spread) - Spread;

                Vector2 adjustedTarget = pos;

                //rotate the target vector by the deviation
                Transformations.RotateVectorAroundPoint(
                    ref adjustedTarget,
                    Owner.Position,
                    deviation);

                //add a pellet to the game world
                GameManager.GameManager.Instance.AddShotgunPellet(
                    Owner,
                    adjustedTarget);

                //temp debugging code
                Targets.Add(adjustedTarget);
            }

            DecrementRounds();

            UpdateTimeNextAvailable();

            //add a trigger to the game so that the other bots can hear
            //this shot (provided they are within range)
            GameManager.GameManager.Instance.Map.AddSoundTrigger(
                Owner,
                GameManager.GameManager.Instance.Parameters.ShotgunSoundRange);
        }

        ///<summary>
        ///this method returns a value representing the desirability of using the
        ///weapon. This is used by the AI to select the most suitable weapon for
        ///a bot's current situation. This value is calculated using fuzzy logic
        ///</summary>
        ///<param name="distToTarget"></param>
        ///<returns>
        ///a value representing the desirability of using the weapon
        ///</returns>
        public override float GetDesirability(float distToTarget)
        {
            if (NumRoundsRemaining == 0)
            {
                LastDesirabilityScore = 0;
            }
            else
            {
                //fuzzify distance and amount of ammo
                FuzzyModule.Fuzzify("distanceToTarget", distToTarget);
                FuzzyModule.Fuzzify("ammoStatus", NumRoundsRemaining);
                LastDesirabilityScore =
                    FuzzyModule.DeFuzzify(
                        "desirability",
                        FuzzyModule.DefuzzifyMethod.MaxAv);
            }

            return LastDesirabilityScore;
        }

        ///<summary>
        ///Render the weapon. If the weapon is a T2DSceneObject, it
        ///will be rendered by the engine, but we may still want to render
        ///some additional info
        ///</summary>
        public override void Render()
        {
#if SHOW_TARGET
            foreach (Vector2 t in Targets)
            {
                DrawUtil.CircleFill(t, 3, Color.Blue, 20);
            }
#endif
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///this method initializes the fuzzy module with the appropriate fuzzy 
        ///variables and rule base.
        ///</summary>
        protected override void InitializeFuzzyModule()
        {
            FuzzyVariable distanceToTarget =
                FuzzyModule.CreateFLV("distanceToTarget");

            FzSet targetClose =
                distanceToTarget.AddLeftShoulderSet("targetClose", 0, 50, 300);
            FzSet targetMedium =
                distanceToTarget.AddTriangularSet("targetMedium", 50, 300, 600);
            FzSet targetFar =
                distanceToTarget.AddRightShoulderSet("targetFar", 300, 600, 2000);

            FuzzyVariable desirability =
                FuzzyModule.CreateFLV("desirability");

            FzSet veryDesirable =
                desirability.AddRightShoulderSet("veryDesirable", 50, 75, 100);
            FzSet desirable =
                desirability.AddTriangularSet("desirable", 25, 50, 75);
            FzSet undesirable =
                desirability.AddLeftShoulderSet("undesirable", 0, 25, 50);

            FuzzyVariable ammoStatus =
                FuzzyModule.CreateFLV("ammoStatus");
            FzSet ammoLoads =
                ammoStatus.AddRightShoulderSet("ammoLoads", 30, 60, 100);
            FzSet ammoOkay =
                ammoStatus.AddTriangularSet("ammoOkay", 0, 30, 60);
            FzSet ammoLow =
                ammoStatus.AddTriangularSet("ammoLow", 0, 0, 30);

            FuzzyModule.AddRule(new FzAND(targetClose, ammoLoads), veryDesirable);
            FuzzyModule.AddRule(new FzAND(targetClose, ammoOkay), veryDesirable);
            FuzzyModule.AddRule(new FzAND(targetClose, ammoLow), veryDesirable);

            FuzzyModule.AddRule(new FzAND(targetMedium, ammoLoads), veryDesirable);
            FuzzyModule.AddRule(new FzAND(targetMedium, ammoOkay), desirable);
            FuzzyModule.AddRule(new FzAND(targetMedium, ammoLow), undesirable);

            FuzzyModule.AddRule(new FzAND(targetFar, ammoLoads), desirable);
            FuzzyModule.AddRule(new FzAND(targetFar, ammoOkay), undesirable);
            FuzzyModule.AddRule(new FzAND(targetFar, ammoLow), undesirable);
        }

        #endregion

        #region Private, protected, internal fields

        private readonly int _numBallsInShell;

        //
        private readonly float _spread;

        private readonly List<Vector2> _targets = new List<Vector2>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}