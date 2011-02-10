#region File description

//------------------------------------------------------------------------------
//WeaponRocketLauncher.cs
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
    ///class to implement a rocket launcher
    ///</summary>
    public sealed class WeaponRocketLauncher : Weapon
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        public WeaponRocketLauncher(BotEntity owner)
            : base(WeaponTypes.RocketLauncher,
                   GameManager.GameManager.Instance.Parameters.RocketLauncherDefaultRounds,
                   GameManager.GameManager.Instance.Parameters.RocketLauncherMaxRoundsCarried,
                   GameManager.GameManager.Instance.Parameters.RocketLauncherFiringFreq,
                   GameManager.GameManager.Instance.Parameters.RocketLauncherIdealRange,
                   GameManager.GameManager.Instance.Parameters.RocketMaxSpeed,
                   owner)
        {
            //setup the fuzzy module
            InitializeFuzzyModule();
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

            //fire off a rocket!
            GameManager.GameManager.Instance.AddRocket(Owner, pos);

            DecrementRounds();

            UpdateTimeNextAvailable();

            //add a trigger to the game so that the other bots can hear
            //this shot (provided they are within range)
            GameManager.GameManager.Instance.Map.AddSoundTrigger(
                Owner,
                GameManager.GameManager.Instance.Parameters.RocketLauncherSoundRange);
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

            FuzzyVariable ammoStatus = FuzzyModule.CreateFLV("ammoStatus");
            FzSet ammoLoads =
                ammoStatus.AddRightShoulderSet("ammoLoads", 10, 30, 100);
            FzSet ammoOkay = ammoStatus.AddTriangularSet("ammoOkay", 0, 10, 30);
            FzSet ammoLow = ammoStatus.AddTriangularSet("ammoLow", 0, 0, 10);

            FuzzyModule.AddRule(new FzAND(targetClose, ammoLoads), undesirable);
            FuzzyModule.AddRule(new FzAND(targetClose, ammoOkay), undesirable);
            FuzzyModule.AddRule(new FzAND(targetClose, ammoLow), undesirable);

            FuzzyModule.AddRule(new FzAND(targetMedium, ammoLoads), veryDesirable);
            FuzzyModule.AddRule(new FzAND(targetMedium, ammoOkay), veryDesirable);
            FuzzyModule.AddRule(new FzAND(targetMedium, ammoLow), desirable);

            FuzzyModule.AddRule(new FzAND(targetFar, ammoLoads), desirable);
            FuzzyModule.AddRule(new FzAND(targetFar, ammoOkay), undesirable);
            FuzzyModule.AddRule(new FzAND(targetFar, ammoLow), undesirable);
        }

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}