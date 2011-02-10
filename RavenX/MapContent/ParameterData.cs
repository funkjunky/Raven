#region File description

//------------------------------------------------------------------------------
//ParameterData.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

    #endregion

    #region Microsoft

#if !XBOX

#endif

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///class for parameter data
    ///</summary>
    public class ParameterData
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        /// <summary>
        /// the likelihood of searching for a weapon when we are not doing anything better
        /// </summary>
        public float PropensityToSearchForAWeapon
        {
            get { return _propensityToSearchForAWeapon; }
            set { _propensityToSearchForAWeapon = value; }
        }


        ///<summary>
        ///how long out navigational memory is
        ///</summary>
        public bool PlayCaptureTheFlag
        {
            get { return _playCaptureTheFlag; }
            set { _playCaptureTheFlag = value; }
        }

        ///<summary>
        ///how long out navigational memory is
        ///</summary>
        public float NavigationMemorySpan
        {
            get { return _navigationMemorySpan; }
            set { _navigationMemorySpan = value; }
        }

        ///<summary>
        ///the number of bots the game instantiates
        ///</summary>
        public int NumBots
        {
            get { return _numBots; }
            set { _numBots = value; }
        }

        /// <summary>
        /// the number of teams the game instantiates
        /// </summary>
        public int NumTeams
        {
            get { return _numTeams; }
            set { _numTeams = value; }
        }

        ///<summary>
        ///this is the maximum number of search cycles allocated to *all* 
        ///current path planning searches per update
        ///</summary>
        public int MaxSearchCyclesPerUpdateStep
        {
            get { return _maxSearchCyclesPerUpdateStep; }
            set { _maxSearchCyclesPerUpdateStep = value; }
        }

        ///<summary>
        ///the name of the default map
        ///</summary>
        public string StartMap
        {
            get { return _startMap; }
            set { _startMap = value; }
        }

        ///<summary>
        ///cell space partitioning default X
        ///</summary>
        public int NumCellsX
        {
            get { return _numCellsX; }
            set { _numCellsX = value; }
        }

        ///<summary>
        ///cell space partitioning default Y
        ///</summary>
        public int NumCellsY
        {
            get { return _numCellsY; }
            set { _numCellsY = value; }
        }

        ///<summary>
        ///how long the graves remain on screen
        ///</summary>
        public float GraveLifetime
        {
            get { return _graveLifetime; }
            set { _graveLifetime = value; }
        }

        ///<summary>
        ///maximum health
        ///</summary>
        public int BotMaxHealth
        {
            get { return _botMaxHealth; }
            set { _botMaxHealth = value; }
        }

        ///<summary>
        ///maximum speed (for normal movement)
        ///</summary>
        public float BotMaxSpeed
        {
            get { return _botMaxSpeed; }
            set { _botMaxSpeed = value; }
        }

        ///<summary>
        ///mass
        ///(force and mass determine maximum acceleration)
        ///</summary>
        public float BotMass
        {
            get { return _botMass; }
            set { _botMass = value; }
        }

        ///<summary>
        ///maximum steering force 
        ///(force and mass determine maximum acceleration)
        ///</summary>
        public float BotMaxForce
        {
            get { return _botMaxForce; }
            set { _botMaxForce = value; }
        }

        ///<summary>
        ///head/turret turn rate (to change facing for shooting)
        ///(specified in radians/sec)
        ///TODO: be consistent about angles in degrees or radians 
        ///</summary>
        public float BotMaxHeadTurnRate
        {
            get { return _botMaxHeadTurnRate; }
            set { _botMaxHeadTurnRate = value; }
        }

        ///<summary>
        ///scale
        ///</summary>
        public float BotScale
        {
            get { return _botScale; }
            set { _botScale = value; }
        }

        ///<summary>
        ///special movement speed (unused)
        ///</summary>
        public float BotMaxSwimmingSpeed
        {
            get { return _botMaxSwimmingSpeed; }
            set { _botMaxSwimmingSpeed = value; }
        }

        ///<summary>
        ///special movement speed (unused)
        ///</summary>
        public float BotMaxCrawlingSpeed
        {
            get { return _botMaxCrawlingSpeed; }
            set { _botMaxCrawlingSpeed = value; }
        }

        ///<summary>
        ///the number of times a second a bot 'thinks' about weapon selection
        ///<remarks>
        ///a frequency of -1 will disable the feature and a frequency of zero
        ///will ensure the feature is updated every bot update
        ///</remarks>
        ///</summary>
        public float BotWeaponSelectionFrequency
        {
            get { return _botWeaponSelectionFrequency; }
            set { _botWeaponSelectionFrequency = value; }
        }

        ///<summary>
        ///the number of times a second a bot 'thinks' about changing strategy
        ///<remarks>
        ///a frequency of -1 will disable the feature and a frequency of zero
        ///will ensure the feature is updated every bot update
        ///</remarks>
        ///</summary>
        public float BotGoalAppraisalUpdateFreq
        {
            get { return _botGoalAppraisalUpdateFreq; }
            set { _botGoalAppraisalUpdateFreq = value; }
        }

        ///<summary>
        ///the number of times a second a bot updates its target info
        ///<remarks>
        ///a frequency of -1 will disable the feature and a frequency of zero
        ///will ensure the feature is updated every bot update
        ///</remarks>
        ///</summary>
        public float BotTargetingUpdateFreq
        {
            get { return _botTargetingUpdateFreq; }
            set { _botTargetingUpdateFreq = value; }
        }

        ///<summary>
        ///the number of times a second the triggers are updated
        ///<remarks>
        ///a frequency of -1 will disable the feature and a frequency of zero
        ///will ensure the feature is updated every bot update
        ///</remarks>
        ///</summary>
        public float BotTriggerUpdateFreq
        {
            get { return _botTriggerUpdateFreq; }
            set { _botTriggerUpdateFreq = value; }
        }

        ///<summary>
        ///the number of times a second a bot updates its vision
        ///<remarks>
        ///a frequency of -1 will disable the feature and a frequency of zero
        ///will ensure the feature is updated every bot update
        ///</remarks>
        ///</summary>
        public float BotVisionUpdateFreq
        {
            get { return _botVisionUpdateFreq; }
            set { _botVisionUpdateFreq = value; }
        }

        ///<summary>
        ///the bot's field of view (in degrees)
        ///</summary>
        public float BotFOV
        {
            get { return _botFOV; }
            set { _botFOV = value; }
        }

        ///<summary>
        ///the bot's reaction time (in seconds)
        ///</summary>
        public float BotReactionTime
        {
            get { return _botReactionTime; }
            set { _botReactionTime = value; }
        }

        ///<summary>
        ///how long (in seconds) the bot will keep pointing its weapon at its
        ///target after the target goes out of view
        ///</summary>
        public float BotAimPersistence
        {
            get { return _botAimPersistence; }
            set { _botAimPersistence = value; }
        }

        ///<summary>
        ///how accurate the bots are at aiming. 0 is very accurate, (the value
        ///represents the max deviation in range (in radians))
        ///</summary>
        public float BotAimAccuracy
        {
            get { return _botAimAccuracy; }
            set { _botAimAccuracy = value; }
        }

        ///<summary>
        ///how long a flash is displayed when the bot is hit
        ///</summary>
        public float HitFlashTime
        {
            get { return _hitFlashTime; }
            set { _hitFlashTime = value; }
        }

        ///<summary>
        ///how long (in seconds) a bot's sensory memory persists
        ///</summary>
        public float BotMemorySpan
        {
            get { return _botMemorySpan; }
            set { _botMemorySpan = value; }
        }

        ///<summary>
        ///value used to tweak desirability of the get health goal
        ///</summary>
        public float BotHealthGoalTweaker
        {
            get { return _botHealthGoalTweaker; }
            set { _botHealthGoalTweaker = value; }
        }

        ///<summary>
        ///value used to tweak desirability of the get shotgun goal
        ///</summary>
        public float BotShotgunGoalTweaker
        {
            get { return _botShotgunGoalTweaker; }
            set { _botShotgunGoalTweaker = value; }
        }

        ///<summary>
        ///value used to tweak desirability of the get railgun goal
        ///</summary>
        public float BotRailgunGoalTweaker
        {
            get { return _botRailgunGoalTweaker; }
            set { _botRailgunGoalTweaker = value; }
        }

        ///<summary>
        ///value used to tweak desirability of the get rocket launcher goal
        ///</summary>
        public float BotRocketLauncherGoalTweaker
        {
            get { return _botRocketLauncherGoalTweaker; }
            set { _botRocketLauncherGoalTweaker = value; }
        }

        ///<summary>
        ///value used to tweak desirability of the attack target goal
        ///</summary>
        public float BotAggroGoalTweaker
        {
            get { return _botAggroGoalTweaker; }
            set { _botAggroGoalTweaker = value; }
        }

        ///<summary>
        ///use this value to tweak the amount that each steering force 
        ///contributes to the total steering force
        ///</summary>
        public float SeparationWeight
        {
            get { return _separationWeight; }
            set { _separationWeight = value; }
        }

        ///<summary>
        ///use this value to tweak the amount that each steering force 
        ///contributes to the total steering force
        ///</summary>
        public float WallAvoidanceWeight
        {
            get { return _wallAvoidanceWeight; }
            set { _wallAvoidanceWeight = value; }
        }

        ///<summary>
        ///use this value to tweak the amount that each steering force 
        ///contributes to the total steering force
        ///</summary>
        public float WanderWeight
        {
            get { return _wanderWeight; }
            set { _wanderWeight = value; }
        }

        ///<summary>
        ///use this value to tweak the amount that each steering force 
        ///contributes to the total steering force
        ///</summary>
        public float SeekWeight
        {
            get { return _seekWeight; }
            set { _seekWeight = value; }
        }

        ///<summary>
        ///use this value to tweak the amount that each steering force 
        ///contributes to the total steering force
        ///</summary>
        public float ArriveWeight
        {
            get { return _arriveWeight; }
            set { _arriveWeight = value; }
        }

        ///<summary>
        ///how close a neighbor must be before an agent considers it to be
        ///within its neighborhood (for separation)
        ///</summary>
        public float ViewDistance
        {
            get { return _viewDistance; }
            set { _viewDistance = value; }
        }

        ///<summary>
        ///max feeler length
        ///</summary>
        public float WallDetectionFeelerLength
        {
            get { return _wallDetectionFeelerLength; }
            set { _wallDetectionFeelerLength = value; }
        }

        ///<summary>
        ///used in path following. Determines how close a bot must be to a
        ///waypoint before it seeks the next waypoint
        ///</summary>
        public float WaypointSeekDist
        {
            get { return _waypointSeekDist; }
            set { _waypointSeekDist = value; }
        }

        ///<summary>
        ///how close a bot must be to a giver-trigger for it to affect it
        ///</summary>
        public float DefaultGiverTriggerRange
        {
            get { return _defaultGiverTriggerRange; }
            set { _defaultGiverTriggerRange = value; }
        }

        ///<summary>
        ///how many seconds before a giver-trigger reactivates itself
        ///</summary>
        public float HealthRespawnDelay
        {
            get { return _healthRespawnDelay; }
            set { _healthRespawnDelay = value; }
        }

        ///<summary>
        ///how many seconds before a giver-trigger reactivates itself
        ///</summary>
        public float WeaponRespawnDelay
        {
            get { return _weaponRespawnDelay; }
            set { _weaponRespawnDelay = value; }
        }

        ///<summary>
        ///blaster rate of fire (shots per second)
        ///</summary>
        public float BlasterFiringFreq
        {
            get { return _blasterFiringFreq; }
            set { _blasterFiringFreq = value; }
        }

        ///<summary>
        ///maximum speed of blaster (bolt) projectile
        ///TODO: this seems to duplicate <see cref="BoltMaxSpeed"/>
        ///</summary>
        public float BlasterMaxSpeed
        {
            get { return _blasterMaxSpeed; }
            set { _blasterMaxSpeed = value; }
        }

        ///<summary>
        ///not used, a blaster always has ammo
        ///</summary>
        public int BlasterDefaultRounds
        {
            get { return _blasterDefaultRounds; }
            set { _blasterDefaultRounds = value; }
        }

        ///<summary>
        ///not used, a blaster always has ammo
        ///</summary>
        public int BlasterMaxRoundsCarried
        {
            get { return _blasterMaxRoundsCarried; }
            set { _blasterMaxRoundsCarried = value; }
        }

        ///<summary>
        ///ideal range to target for blaster
        ///</summary>
        public float BlasterIdealRange
        {
            get { return _blasterIdealRange; }
            set { _blasterIdealRange = value; }
        }

        ///<summary>
        ///distance blaster sound is heard
        ///</summary>
        public float BlasterSoundRange
        {
            get { return _blasterSoundRange; }
            set { _blasterSoundRange = value; }
        }

        ///<summary>
        ///maximum speed of blaster (bolt) projectile
        ///TODO: this seems to duplicate <see cref="BlasterMaxSpeed"/>
        ///</summary>
        public float BoltMaxSpeed
        {
            get { return _boltMaxSpeed; }
            set { _boltMaxSpeed = value; }
        }

        ///<summary>
        ///mass of bolt projectile
        ///</summary>
        public float BoltMass
        {
            get { return _boltMass; }
            set { _boltMass = value; }
        }

        ///<summary>
        ///maximum steering force for bolt projectile
        ///</summary>
        public float BoltMaxForce
        {
            get { return _boltMaxForce; }
            set { _boltMaxForce = value; }
        }

        ///<summary>
        ///scale of bolt projectile
        ///</summary>
        public float BoltScale
        {
            get { return _boltScale; }
            set { _boltScale = value; }
        }

        ///<summary>
        ///damage inflicted by a bolt
        ///</summary>
        public float BoltDamage
        {
            get { return _boltDamage; }
            set { _boltDamage = value; }
        }

        ///<summary>
        ///rocket launcher rate of fire (shots per second)
        ///</summary>
        public float RocketLauncherFiringFreq
        {
            get { return _rocketLauncherFiringFreq; }
            set { _rocketLauncherFiringFreq = value; }
        }

        ///<summary>
        ///initial number of rounds carried
        ///</summary>
        public int RocketLauncherDefaultRounds
        {
            get { return _rocketLauncherDefaultRounds; }
            set { _rocketLauncherDefaultRounds = value; }
        }

        ///<summary>
        ///maximum number of rounds carriable
        ///</summary>
        public int RocketLauncherMaxRoundsCarried
        {
            get { return _rocketLauncherMaxRoundsCarried; }
            set { _rocketLauncherMaxRoundsCarried = value; }
        }

        ///<summary>
        ///ideal range to target for rocket launcher
        ///</summary>
        public float RocketLauncherIdealRange
        {
            get { return _rocketLauncherIdealRange; }
            set { _rocketLauncherIdealRange = value; }
        }

        ///<summary>
        ///distance rocket launcher sound is heard
        ///</summary>
        public float RocketLauncherSoundRange
        {
            get { return _rocketLauncherSoundRange; }
            set { _rocketLauncherSoundRange = value; }
        }

        ///<summary>
        ///blast radius of exploding rocket projectile
        ///</summary>
        public float RocketBlastRadius
        {
            get { return _rocketBlastRadius; }
            set { _rocketBlastRadius = value; }
        }

        ///<summary>
        ///maximum speed of rocket projectile
        ///</summary>
        public float RocketMaxSpeed
        {
            get { return _rocketMaxSpeed; }
            set { _rocketMaxSpeed = value; }
        }

        ///<summary>
        ///mass of rocket projectile
        ///</summary>
        public float RocketMass
        {
            get { return _rocketMass; }
            set { _rocketMass = value; }
        }

        ///<summary>
        ///maximum steering force for rocket projectile
        ///</summary>
        public float RocketMaxForce
        {
            get { return _rocketMaxForce; }
            set { _rocketMaxForce = value; }
        }

        ///<summary>
        ///scale of rocket projectile
        ///</summary>
        public float RocketScale
        {
            get { return _rocketScale; }
            set { _rocketScale = value; }
        }

        ///<summary>
        ///damage inflicted by a rocket projectile
        ///</summary>
        public float RocketDamage
        {
            get { return _rocketDamage; }
            set { _rocketDamage = value; }
        }

        ///<summary>
        ///how fast the explosion occurs (in radius units per sec)
        ///</summary>
        public float RocketExplosionDecayRate
        {
            get { return _rocketExplosionDecayRate; }
            set { _rocketExplosionDecayRate = value; }
        }

        ///<summary>
        ///railgun rate of fire (shots per second)
        ///</summary>
        public float RailgunFiringFreq
        {
            get { return _railgunFiringFreq; }
            set { _railgunFiringFreq = value; }
        }

        ///<summary>
        ///initial number of rounds carried
        ///</summary>
        public int RailgunDefaultRounds
        {
            get { return _railgunDefaultRounds; }
            set { _railgunDefaultRounds = value; }
        }

        ///<summary>
        ///maximum number of rounds carriable
        ///</summary>
        public int RailgunMaxRoundsCarried
        {
            get { return _railgunMaxRoundsCarried; }
            set { _railgunMaxRoundsCarried = value; }
        }

        ///<summary>
        ///ideal range to target for railgun
        ///</summary>
        public float RailgunIdealRange
        {
            get { return _railgunIdealRange; }
            set { _railgunIdealRange = value; }
        }

        ///<summary>
        ///distance railgun sound is heard
        ///</summary>
        public float RailgunSoundRange
        {
            get { return _railgunSoundRange; }
            set { _railgunSoundRange = value; }
        }

        ///<summary>
        ///maximum speed of slug projectile
        ///</summary>
        public float SlugMaxSpeed
        {
            get { return _slugMaxSpeed; }
            set { _slugMaxSpeed = value; }
        }

        ///<summary>
        ///mass of slug projectile
        ///</summary>
        public float SlugMass
        {
            get { return _slugMass; }
            set { _slugMass = value; }
        }

        ///<summary>
        ///maximum steering force for slug projectile
        ///</summary>
        public float SlugMaxForce
        {
            get { return _slugMaxForce; }
            set { _slugMaxForce = value; }
        }

        ///<summary>
        ///scale of slug projectile
        ///</summary>
        public float SlugScale
        {
            get { return _slugScale; }
            set { _slugScale = value; }
        }

        ///<summary>
        ///time slug (and trajectory) remain visible
        ///</summary>
        public float SlugPersistence
        {
            get { return _slugPersistence; }
            set { _slugPersistence = value; }
        }

        ///<summary>
        ///damage inflicted by a slug projectile
        ///</summary>
        public float SlugDamage
        {
            get { return _slugDamage; }
            set { _slugDamage = value; }
        }

        ///<summary>
        ///shotgun rate of fire (shots per second)
        ///</summary>
        public float ShotgunFiringFreq
        {
            get { return _shotgunFiringFreq; }
            set { _shotgunFiringFreq = value; }
        }

        ///<summary>
        ///initial number of rounds carried
        ///</summary>
        public int ShotgunDefaultRounds
        {
            get { return _shotgunDefaultRounds; }
            set { _shotgunDefaultRounds = value; }
        }

        ///<summary>
        ///maximum number of rounds carriable
        ///</summary>
        public int ShotgunMaxRoundsCarried
        {
            get { return _shotgunMaxRoundsCarried; }
            set { _shotgunMaxRoundsCarried = value; }
        }

        ///<summary>
        ///number of balls in a shotgun shell
        ///</summary>
        public int ShotgunNumBallsInShell
        {
            get { return _shotgunNumBallsInShell; }
            set { _shotgunNumBallsInShell = value; }
        }

        ///<summary>
        ///spread angle (in radians) for shotgun balls
        ///</summary>
        public float ShotgunSpread
        {
            get { return _shotgunSpread; }
            set { _shotgunSpread = value; }
        }

        ///<summary>
        ///ideal range to target for shotgun
        ///</summary>
        public float ShotgunIdealRange
        {
            get { return _shotgunIdealRange; }
            set { _shotgunIdealRange = value; }
        }

        ///<summary>
        ///distance shotgun sound is heard
        ///</summary>
        public float ShotgunSoundRange
        {
            get { return _shotgunSoundRange; }
            set { _shotgunSoundRange = value; }
        }

        ///<summary>
        ///maximum speed of pellet projectile
        ///</summary>
        public float PelletMaxSpeed
        {
            get { return _pelletMaxSpeed; }
            set { _pelletMaxSpeed = value; }
        }

        ///<summary>
        ///mass of pellet projectile
        ///</summary>
        public float PelletMass
        {
            get { return _pelletMass; }
            set { _pelletMass = value; }
        }

        ///<summary>
        ///maximum steering force for pellet projectile
        ///</summary>
        public float PelletMaxForce
        {
            get { return _pelletMaxForce; }
            set { _pelletMaxForce = value; }
        }

        ///<summary>
        ///scale of pellet projectile
        ///</summary>
        public float PelletScale
        {
            get { return _pelletScale; }
            set { _pelletScale = value; }
        }

        ///<summary>
        ///time pellet (and trajectory) remain visible
        ///</summary>
        public float PelletPersistence
        {
            get { return _pelletPersistence; }
            set { _pelletPersistence = value; }
        }

        ///<summary>
        ///damage inflicted by a pellet projectile
        ///</summary>
        public float PelletDamage
        {
            get { return _pelletDamage; }
            set { _pelletDamage = value; }
        }

        #region Public methods

        ///<summary>
        ///set parameter default values
        ///</summary>
        public void SetDefaults()
        {
            PropensityToSearchForAWeapon = 0.05f;
            PlayCaptureTheFlag = true;
            NavigationMemorySpan = -1;
            NumBots = 8;
            NumTeams = 2;
            MaxSearchCyclesPerUpdateStep = 1000;
            StartMap = @"data\maps\mindcrafters";
            NumCellsX = 10;
            NumCellsY = 10;
            GraveLifetime = 5;
            BotMaxHealth = 100;
            BotMaxSpeed = 60;
            BotMass = 1;
            BotMaxForce = 100.0f;
            BotMaxHeadTurnRate = 0.2f;
            BotScale = 20.0f;
            BotMaxSwimmingSpeed = BotMaxSpeed*0.2f;
            BotMaxCrawlingSpeed = BotMaxSpeed*0.6f;
            BotWeaponSelectionFrequency = 2;
            BotGoalAppraisalUpdateFreq = 4;
            BotTargetingUpdateFreq = 2;
            BotTriggerUpdateFreq = 8;
            BotVisionUpdateFreq = 4;
            BotFOV = 180; //degrees
            BotReactionTime = 0.2f;
            BotAimPersistence = 1;
            BotAimAccuracy = 0.0f; //radians
            HitFlashTime = 0.2f;
            BotMemorySpan = 5;
            BotHealthGoalTweaker = 1.0f;
            BotShotgunGoalTweaker = 0.8f;
            BotRailgunGoalTweaker = 0.8f;
            BotRocketLauncherGoalTweaker = 0.8f;
            BotAggroGoalTweaker = 1.0f;
            SeparationWeight = 10.0f;
            WallAvoidanceWeight = 2.0f;
            WanderWeight = 1.0f;
            SeekWeight = 0.5f;
            ArriveWeight = 1.0f;
            ViewDistance = 15.0f;
            WallDetectionFeelerLength = BotScale/2.0f;
            WaypointSeekDist = 20;
            DefaultGiverTriggerRange = 10;
            HealthRespawnDelay = 10;
            WeaponRespawnDelay = 15;
            BlasterFiringFreq = 3;
            BlasterMaxSpeed = 300;
            BlasterDefaultRounds = 0; //not used, a blaster always has ammo    
            BlasterMaxRoundsCarried = 0; //not used, a blaster always has ammo
            BlasterIdealRange = 50;
            BlasterSoundRange = 100;
            BoltMaxSpeed = 300;
            BoltMass = 1;
            BoltMaxForce = 6000.0f;
            BoltScale = 2;
            BoltDamage = 1;
            RocketLauncherFiringFreq = 1.5f;
            RocketLauncherDefaultRounds = 15;
            RocketLauncherMaxRoundsCarried = 50;
            RocketLauncherIdealRange = 150;
            RocketLauncherSoundRange = 400;
            RocketBlastRadius = 20;
            RocketMaxSpeed = 180;
            RocketMass = 1;
            RocketMaxForce = 600.0f;
            RocketScale = 1;
            RocketDamage = 10;
            RocketExplosionDecayRate = 40.0f;
            RailgunFiringFreq = 1;
            RailgunDefaultRounds = 15;
            RailgunMaxRoundsCarried = 50;
            RailgunIdealRange = 200;
            RailgunSoundRange = 400;
            SlugMaxSpeed = 1000000.0f;
            SlugMass = 0.1f;
            SlugMaxForce = 1000000.0f;
            SlugScale = 1;
            SlugPersistence = 0.2f;
            SlugDamage = 10;
            ShotgunFiringFreq = 1;
            ShotgunDefaultRounds = 15;
            ShotgunMaxRoundsCarried = 30;
            ShotgunNumBallsInShell = 10;
            ShotgunSpread = 0.15f;
            ShotgunIdealRange = 100;
            ShotgunSoundRange = 400;
            PelletMaxSpeed = 600;
            PelletMass = 0.1f;
            PelletMaxForce = 6000.0f;
            PelletScale = 1;
            PelletPersistence = 1f;
            PelletDamage = 1;
        }

#if !XBOX
        ///<summary>
        ///write data to xml file
        ///</summary>
        ///<param name="xmlParameterFilename"></param>
        public void SaveToXml(string xmlParameterFilename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(xmlParameterFilename, settings))
            {
                IntermediateSerializer.Serialize(xmlWriter, this, null);
            }
        }
#endif

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private float _navigationMemorySpan;
        private float _arriveWeight;
        private int _blasterDefaultRounds;
        private float _blasterFiringFreq;
        private float _blasterIdealRange;
        private int _blasterMaxRoundsCarried;
        private float _blasterMaxSpeed;
        private float _blasterSoundRange;
        private float _boltDamage;
        private float _boltMass;
        private float _boltMaxForce;
        private float _boltMaxSpeed;
        private float _boltScale;
        private float _botAggroGoalTweaker;
        private float _botAimAccuracy;
        private float _botAimPersistence;
        private float _botFOV;
        private float _botGoalAppraisalUpdateFreq;
        private float _botHealthGoalTweaker;
        private float _botMass;
        private float _botMaxCrawlingSpeed;
        private float _botMaxForce;
        private float _botMaxHeadTurnRate;
        private int _botMaxHealth;
        private float _botMaxSpeed;
        private float _botMaxSwimmingSpeed;
        private float _botMemorySpan;
        private float _botRailgunGoalTweaker;
        private float _botReactionTime;
        private float _botRocketLauncherGoalTweaker;
        private float _botScale;
        private float _botShotgunGoalTweaker;
        private float _botTargetingUpdateFreq;
        private float _botTriggerUpdateFreq;
        private float _botVisionUpdateFreq;
        private float _botWeaponSelectionFrequency;
        private float _defaultGiverTriggerRange;
        private float _graveLifetime;
        private float _healthRespawnDelay;
        private float _hitFlashTime;
        private int _maxSearchCyclesPerUpdateStep;
        private int _numBots;
        private int _numCellsX;
        private int _numCellsY;
        private int _numTeams;
        private float _pelletDamage;
        private float _pelletMass;
        private float _pelletMaxForce;
        private float _pelletMaxSpeed;
        private float _pelletPersistence;
        private float _pelletScale;
        private bool _playCaptureTheFlag;
        private int _railgunDefaultRounds;
        private float _railgunFiringFreq;
        private float _railgunIdealRange;
        private int _railgunMaxRoundsCarried;
        private float _railgunSoundRange;
        private float _rocketBlastRadius;
        private float _rocketDamage;
        private float _rocketExplosionDecayRate;
        private int _rocketLauncherDefaultRounds;
        private float _rocketLauncherFiringFreq;
        private float _rocketLauncherIdealRange;
        private int _rocketLauncherMaxRoundsCarried;
        private float _rocketLauncherSoundRange;
        private float _rocketMass;
        private float _rocketMaxForce;
        private float _rocketMaxSpeed;
        private float _rocketScale;
        private float _seekWeight;
        private float _separationWeight;
        private int _shotgunDefaultRounds;
        private float _shotgunFiringFreq;
        private float _shotgunIdealRange;
        private int _shotgunMaxRoundsCarried;
        private int _shotgunNumBallsInShell;
        private float _shotgunSoundRange;
        private float _shotgunSpread;
        private float _slugDamage;
        private float _slugMass;
        private float _slugMaxForce;
        private float _slugMaxSpeed;
        private float _slugPersistence;
        private float _slugScale;
        private string _startMap;
        private float _viewDistance;
        private float _wallAvoidanceWeight;
        private float _wallDetectionFeelerLength;
        private float _wanderWeight;
        private float _waypointSeekDist;
        private float _weaponRespawnDelay;
        private float _propensityToSearchForAWeapon;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}