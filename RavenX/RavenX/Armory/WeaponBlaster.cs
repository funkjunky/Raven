#region File description

//------------------------------------------------------------------------------
//WeaponBlaster.cs
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
    ///class to implement a blaster
    ///</summary>
    public sealed class WeaponBlaster : Weapon
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        public WeaponBlaster(BotEntity owner)
            : base(WeaponTypes.Blaster,
                   GameManager.GameManager.Instance.Parameters.BlasterDefaultRounds,
                   GameManager.GameManager.Instance.Parameters.BlasterMaxRoundsCarried,
                   GameManager.GameManager.Instance.Parameters.BlasterFiringFreq,
                   GameManager.GameManager.Instance.Parameters.BlasterIdealRange,
                   GameManager.GameManager.Instance.Parameters.BlasterMaxSpeed,
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
            if (!IsReadyForNextShot())
                return;

            //fire!
            GameManager.GameManager.Instance.AddBolt(Owner, pos);

            UpdateTimeNextAvailable();

            //add a trigger to the game so that the other bots can hear 
            //this shot (provided they are within range)
            GameManager.GameManager.Instance.Map.AddSoundTrigger(
                Owner,
                GameManager.GameManager.Instance.Parameters.BlasterSoundRange);
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
            //fuzzify distance and amount of ammo
            FuzzyModule.Fuzzify("distanceToTarget", distToTarget);

            LastDesirabilityScore =
                FuzzyModule.DeFuzzify(
                    "desirability",
                    FuzzyModule.DefuzzifyMethod.MaxAv);

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
            FuzzyModule = new FuzzyModule();

            FuzzyVariable distToTarget =
                FuzzyModule.CreateFLV("distanceToTarget");

            FzSet targetClose =
                distToTarget.AddLeftShoulderSet("targetClose", 0, 50, 300);
            FzSet targetMedium =
                distToTarget.AddTriangularSet("targetMedium", 50, 300, 600);
            FzSet targetFar =
                distToTarget.AddRightShoulderSet("targetFar", 300, 600, 2000);

            FuzzyVariable desirability =
                FuzzyModule.CreateFLV("desirability");
#pragma warning disable 168
            FzSet veryDesirable =
#pragma warning restore 168
                desirability.AddRightShoulderSet("veryDesirable", 50, 75, 100);
            FzSet desirable =
                desirability.AddTriangularSet("Desirable", 25, 50, 75);
            FzSet undesirable =
                desirability.AddLeftShoulderSet("undesirable", 0, 25, 50);

            FuzzyModule.AddRule(targetClose, desirable);
            FuzzyModule.AddRule(targetMedium, new FzVery(undesirable));
            FuzzyModule.AddRule(targetFar, new FzVery(undesirable));
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