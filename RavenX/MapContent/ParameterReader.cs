#region File description

//------------------------------------------------------------------------------
//ParameterReader.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework.Content;

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///Content Pipeline class for loading Wall data from XNB format.
    ///</summary>
    public class ParameterReader : ContentTypeReader<ParameterData>
    {
        ///<summary>
        ///Reads a strongly typed object from the current stream.
        ///</summary>
        ///<param name="input">
        ///The <see cref="ContentReader"/> used to read the object.
        ///</param>
        ///<param name="existingInstance">
        ///An existing object to read into.
        ///</param>
        ///<returns>The type of object to read.</returns>
        protected override ParameterData Read(
            ContentReader input, ParameterData existingInstance)
        {
            ParameterData data = new ParameterData();

            data.ArriveWeight = input.ReadSingle();
            data.BlasterDefaultRounds = input.ReadInt32();
            data.BlasterFiringFreq = input.ReadSingle();
            data.BlasterIdealRange = input.ReadSingle();
            data.BlasterMaxRoundsCarried = input.ReadInt32();
            data.BlasterMaxSpeed = input.ReadSingle();
            data.BlasterSoundRange = input.ReadSingle();
            data.BoltDamage = input.ReadSingle();
            data.BoltMass = input.ReadSingle();
            data.BoltMaxForce = input.ReadSingle();
            data.BoltMaxSpeed = input.ReadSingle();
            data.BoltScale = input.ReadSingle();
            data.BotAggroGoalTweaker = input.ReadSingle();
            data.BotAimAccuracy = input.ReadSingle();
            data.BotAimPersistence = input.ReadSingle();
            data.BotFOV = input.ReadSingle();
            data.BotGoalAppraisalUpdateFreq = input.ReadSingle();
            data.BotHealthGoalTweaker = input.ReadSingle();
            data.BotMass = input.ReadSingle();
            data.BotMaxCrawlingSpeed = input.ReadSingle();
            data.BotMaxForce = input.ReadSingle();
            data.BotMaxHeadTurnRate = input.ReadSingle();
            data.BotMaxHealth = input.ReadInt32();
            data.BotMaxSpeed = input.ReadSingle();
            data.BotMaxSwimmingSpeed = input.ReadSingle();
            data.BotMemorySpan = input.ReadSingle();
            data.BotRailgunGoalTweaker = input.ReadSingle();
            data.BotReactionTime = input.ReadSingle();
            data.BotRocketLauncherGoalTweaker = input.ReadSingle();
            data.BotScale = input.ReadSingle();
            data.BotShotgunGoalTweaker = input.ReadSingle();
            data.BotTargetingUpdateFreq = input.ReadSingle();
            data.BotTriggerUpdateFreq = input.ReadSingle();
            data.BotVisionUpdateFreq = input.ReadSingle();
            data.BotWeaponSelectionFrequency = input.ReadSingle();
            data.DefaultGiverTriggerRange = input.ReadSingle();
            data.GraveLifetime = input.ReadSingle();
            data.HealthRespawnDelay = input.ReadSingle();
            data.HitFlashTime = input.ReadSingle();
            data.MaxSearchCyclesPerUpdateStep = input.ReadInt32();
            data.NavigationMemorySpan = input.ReadSingle();
            data.NumBots = input.ReadInt32();
            data.NumTeams = input.ReadInt32();
            data.NumCellsX = input.ReadInt32();
            data.NumCellsY = input.ReadInt32();
            data.PelletDamage = input.ReadSingle();
            data.PelletMass = input.ReadSingle();
            data.PelletMaxForce = input.ReadSingle();
            data.PelletMaxSpeed = input.ReadSingle();
            data.PelletPersistence = input.ReadSingle();
            data.PelletScale = input.ReadSingle();
            data.PlayCaptureTheFlag = input.ReadBoolean();
            data.PropensityToSearchForAWeapon = input.ReadSingle();
            data.RailgunDefaultRounds = input.ReadInt32();
            data.RailgunFiringFreq = input.ReadSingle();
            data.RailgunIdealRange = input.ReadSingle();
            data.RailgunMaxRoundsCarried = input.ReadInt32();
            data.RailgunSoundRange = input.ReadSingle();
            data.RocketBlastRadius = input.ReadSingle();
            data.RocketDamage = input.ReadSingle();
            data.RocketExplosionDecayRate = input.ReadSingle();
            data.RocketLauncherDefaultRounds = input.ReadInt32();
            data.RocketLauncherFiringFreq = input.ReadSingle();
            data.RocketLauncherIdealRange = input.ReadSingle();
            data.RocketLauncherMaxRoundsCarried = input.ReadInt32();
            data.RocketLauncherSoundRange = input.ReadSingle();
            data.RocketMass = input.ReadSingle();
            data.RocketMaxForce = input.ReadSingle();
            data.RocketMaxSpeed = input.ReadSingle();
            data.RocketScale = input.ReadSingle();
            data.SeekWeight = input.ReadSingle();
            data.SeparationWeight = input.ReadSingle();
            data.ShotgunDefaultRounds = input.ReadInt32();
            data.ShotgunFiringFreq = input.ReadSingle();
            data.ShotgunIdealRange = input.ReadSingle();
            data.ShotgunMaxRoundsCarried = input.ReadInt32();
            data.ShotgunNumBallsInShell = input.ReadInt32();
            data.ShotgunSoundRange = input.ReadSingle();
            data.ShotgunSpread = input.ReadSingle();
            data.SlugDamage = input.ReadSingle();
            data.SlugMass = input.ReadSingle();
            data.SlugMaxForce = input.ReadSingle();
            data.SlugMaxSpeed = input.ReadSingle();
            data.SlugPersistence = input.ReadSingle();
            data.SlugScale = input.ReadSingle();
            data.StartMap = input.ReadString();
            data.ViewDistance = input.ReadSingle();
            data.WallAvoidanceWeight = input.ReadSingle();
            data.WallDetectionFeelerLength = input.ReadSingle();
            data.WanderWeight = input.ReadSingle();
            data.WaypointSeekDist = input.ReadSingle();
            data.WeaponRespawnDelay = input.ReadSingle();

            return data;
        }
    }
}