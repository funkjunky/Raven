#region File description

//------------------------------------------------------------------------------
//ParameterWriter.cs
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
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

#endregion

#endregion

namespace MapContentPipeline
{
    ///<summary>
    ///Content Pipeline class for saving Wall data into XNB format.
    ///</summary>
    [ContentTypeWriter]
    public class ParameterWriter : ContentTypeWriter<ParameterData>
    {
        ///<summary>
        ///Compiles a strongly typed object into binary format.
        ///</summary>
        ///<param name="output">
        ///The content writer serializing the value.
        ///</param>
        ///<param name="value">The value to write.</param>
        protected override void Write(ContentWriter output, ParameterData value)
        {
            output.Write(value.ArriveWeight);
            output.Write(value.BlasterDefaultRounds);
            output.Write(value.BlasterFiringFreq);
            output.Write(value.BlasterIdealRange);
            output.Write(value.BlasterMaxRoundsCarried);
            output.Write(value.BlasterMaxSpeed);
            output.Write(value.BlasterSoundRange);
            output.Write(value.BoltDamage);
            output.Write(value.BoltMass);
            output.Write(value.BoltMaxForce);
            output.Write(value.BoltMaxSpeed);
            output.Write(value.BoltScale);
            output.Write(value.BotAggroGoalTweaker);
            output.Write(value.BotAimAccuracy);
            output.Write(value.BotAimPersistence);
            output.Write(value.BotFOV);
            output.Write(value.BotGoalAppraisalUpdateFreq);
            output.Write(value.BotHealthGoalTweaker);
            output.Write(value.BotMass);
            output.Write(value.BotMaxCrawlingSpeed);
            output.Write(value.BotMaxForce);
            output.Write(value.BotMaxHeadTurnRate);
            output.Write(value.BotMaxHealth);
            output.Write(value.BotMaxSpeed);
            output.Write(value.BotMaxSwimmingSpeed);
            output.Write(value.BotMemorySpan);
            output.Write(value.BotRailgunGoalTweaker);
            output.Write(value.BotReactionTime);
            output.Write(value.BotRocketLauncherGoalTweaker);
            output.Write(value.BotScale);
            output.Write(value.BotShotgunGoalTweaker);
            output.Write(value.BotTargetingUpdateFreq);
            output.Write(value.BotTriggerUpdateFreq);
            output.Write(value.BotVisionUpdateFreq);
            output.Write(value.BotWeaponSelectionFrequency);
            output.Write(value.DefaultGiverTriggerRange);
            output.Write(value.GraveLifetime);
            output.Write(value.HealthRespawnDelay);
            output.Write(value.HitFlashTime);
            output.Write(value.MaxSearchCyclesPerUpdateStep);
            output.Write(value.NavigationMemorySpan);
            output.Write(value.NumBots);
            output.Write(value.NumTeams);
            output.Write(value.NumCellsX);
            output.Write(value.NumCellsY);
            output.Write(value.PelletDamage);
            output.Write(value.PelletMass);
            output.Write(value.PelletMaxForce);
            output.Write(value.PelletMaxSpeed);
            output.Write(value.PelletPersistence);
            output.Write(value.PelletScale);
            output.Write(value.PlayCaptureTheFlag);
            output.Write(value.PropensityToSearchForAWeapon);
            output.Write(value.RailgunDefaultRounds);
            output.Write(value.RailgunFiringFreq);
            output.Write(value.RailgunIdealRange);
            output.Write(value.RailgunMaxRoundsCarried);
            output.Write(value.RailgunSoundRange);
            output.Write(value.RocketBlastRadius);
            output.Write(value.RocketDamage);
            output.Write(value.RocketExplosionDecayRate);
            output.Write(value.RocketLauncherDefaultRounds);
            output.Write(value.RocketLauncherFiringFreq);
            output.Write(value.RocketLauncherIdealRange);
            output.Write(value.RocketLauncherMaxRoundsCarried);
            output.Write(value.RocketLauncherSoundRange);
            output.Write(value.RocketMass);
            output.Write(value.RocketMaxForce);
            output.Write(value.RocketMaxSpeed);
            output.Write(value.RocketScale);
            output.Write(value.SeekWeight);
            output.Write(value.SeparationWeight);
            output.Write(value.ShotgunDefaultRounds);
            output.Write(value.ShotgunFiringFreq);
            output.Write(value.ShotgunIdealRange);
            output.Write(value.ShotgunMaxRoundsCarried);
            output.Write(value.ShotgunNumBallsInShell);
            output.Write(value.ShotgunSoundRange);
            output.Write(value.ShotgunSpread);
            output.Write(value.SlugDamage);
            output.Write(value.SlugMass);
            output.Write(value.SlugMaxForce);
            output.Write(value.SlugMaxSpeed);
            output.Write(value.SlugPersistence);
            output.Write(value.SlugScale);
            output.Write(value.StartMap);
            output.Write(value.ViewDistance);
            output.Write(value.WallAvoidanceWeight);
            output.Write(value.WallDetectionFeelerLength);
            output.Write(value.WanderWeight);
            output.Write(value.WaypointSeekDist);
            output.Write(value.WeaponRespawnDelay);
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime target type.
        ///</summary>
        ///<param name="targetPlatform">The target platform.</param>
        ///<returns>The qualified name.</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MapContent.ParameterData, MapContent";
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime loader for this type.
        ///</summary>
        ///<param name="targetPlatform">Name of the platform.</param>
        ///<returns>Name of the runtime loader.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MapContent.ParameterReader, MapContent";
        }
    }
}