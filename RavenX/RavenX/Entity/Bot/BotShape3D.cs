#region File description

//------------------------------------------------------------------------------
//BotShape3D.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Components;
using Mindcrafters.RavenX.Armory;
using Mindcrafters.RavenX.Entity.Weapons;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Bot
{
    ///<summary>
    ///BotShape3D is a specialized T2DShape3D with an associated BotEntity
    ///</summary>
    public class BotShape3D : T2DShape3D, IBotSceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for blue team with no minimap component
        ///</summary>
        public BotShape3D()
            : this(GameManager.GameManager.Teams.Blue, null)
        {
        }

        ///<summary>
        ///constructor for given team with no minimap component
        ///</summary>
        public BotShape3D(GameManager.GameManager.Teams team)
            : this(team, null)
        {
        }

        ///<summary>
        ///constructor for given team with given minimap component
        ///</summary>
        public BotShape3D(
            GameManager.GameManager.Teams team,
            MinimapComponent minimapComponent)
        {
            Team = team;
            _minimapComponent = minimapComponent;
            Create();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="BotEntity"/>
        ///</summary>
        public BotEntity BotEntity
        {
            get { return _botEntity; }
        }

        ///<summary>
        ///look target for bot's head
        ///</summary>
        public T2DSceneObject LookTarget
        {
            get { return _lookTarget; }
            set { _lookTarget = value; }
        }

        ///<summary>
        ///bot's team
        ///</summary>
        public GameManager.GameManager.Teams Team
        {
            get { return _team; }
            set { _team = value; }
        }

        #region Public methods

        ///<summary>
        ///initialize using associated entity
        ///</summary>
        ///<param name="entity">associated entity</param>
        public void Initialize(Entity entity)
        {
            _botEntity = entity as BotEntity;
            _botEntity.Team = Team;
        }

        ///<summary>
        ///deploy/carry/mount/hold weapon
        ///</summary>
        ///<param name="weapon">weapon to hold</param>
        public void HoldWeapon(Weapon weapon)
        {
            if (null == weapon && null != _currentWeapon)
            {
                UnmountShape(_currentWeapon);
                _currentWeapon = null;
            }
            else if (weapon != null)
            {
                //TODO: need more weapon models
                switch (weapon.WeaponType)
                {
                    case WeaponTypes.Railgun:
                        if (null == _gl)
                        {
                            _gl = new GrenadeLauncher();
                        }
                        _currentWeapon = _gl;
                        break;

                    case WeaponTypes.RocketLauncher:
                        if (null == _gl)
                        {
                            _gl = new GrenadeLauncher();
                        }
                        _currentWeapon = _gl;
                        break;

                    case WeaponTypes.Shotgun:
                        if (null == _rifle)
                        {
                            _rifle = new Rifle();
                        }
                        _currentWeapon = _rifle;
                        break;

                        //case WeaponTypes.Blaster:
                    default:
                        if (null == _rifle)
                        {
                            _rifle = new Rifle();
                        }
                        _currentWeapon = _rifle;
                        break;
                }
                MountShape(_currentWeapon, "Mount0");
            }
        }

        ///<summary>
        ///Set up the T2DShape3D and its components
        ///</summary>
        public void Create()
        {
            switch (Team)
            {
                case GameManager.GameManager.Teams.Blue:
                    SetShape(@"data\shapes\boombot\blue\blue_player.dts");
                    break;
                case GameManager.GameManager.Teams.Red:
                    SetShape(@"data\shapes\boombot\red\red_player.dts");
                    break;
                case GameManager.GameManager.Teams.Green:
                    SetShape(@"data\shapes\boombot\green\green_player.dts");
                    break;
                case GameManager.GameManager.Teams.Yellow:
                    SetShape(@"data\shapes\boombot\yellow\yellow_player.dts");
                    break;
            }

            //set the layer     
            Layer = 0;
            Size = new Vector2(24, 24);
            //set the size of the model in the scene    
            ShapeScale = new Vector3(12);
            Rotation2 = new Vector3(0, 0, MathHelper.ToRadians(Rotation));

            //T2DPhysicsComponent physics = new T2DPhysicsComponent();
            //physics.InverseMass = 0.1f;
            //Components.AddComponent(physics);
            //Physics = new T2DPhysicsComponent();
            //Physics.InverseMass = 0.1f;
            //Components.AddComponent(Physics);

            BotAnimationComponent bac = new BotAnimationComponent();
            Components.AddComponent(bac);
            AddThread("main", "root", true);
            AddThread("look", "look", false);
            AddThread("headside", "headside", false);
            //root, run, side, back, jump, fall, land, light_recoil, look,
            //headside, Head, standjump
            OnAnimationTrigger = OnAnimationTriggerHandler;
            OnAnimateShape = OnAnimateShapeHandler;

            if (null == _minimapComponent)
                return;

            _minimapComponent.MiniCam.Mount(this, "", false);
            _minimapComponent.MiniCam.TrackMountRotation = false;
            switch (Team)
            {
                case GameManager.GameManager.Teams.Blue:
                    _minimapComponent.BorderColor =
                        MinimapComponent.BorderColors.Blue;
                    break;
                case GameManager.GameManager.Teams.Red:
                    _minimapComponent.BorderColor =
                        MinimapComponent.BorderColors.Red;
                    break;
                case GameManager.GameManager.Teams.Green:
                    _minimapComponent.BorderColor =
                        MinimapComponent.BorderColors.Green;
                    break;
                case GameManager.GameManager.Teams.Yellow:
                    _minimapComponent.BorderColor =
                        MinimapComponent.BorderColors.Yellow;
                    break;
                default:
                    _minimapComponent.BorderColor =
                        MinimapComponent.BorderColors.Black;
                    break;
            }
            Components.AddComponent(_minimapComponent);
        }

        ///<summary>
        ///When a trigger event occurs in one of the shapes
        ///trigger channels, this callback is fired.
        ///</summary>
        ///<param name="channel">The channel the trigger occurred in.</param>
        public void OnAnimationTriggerHandler(int channel)
        {
            switch (channel)
            {
                case 1:
                    //left foot down ... 
                    //add foot puff
                    //add foot step sound
                    //apply force
                    break;
                case 2:
                    //right foot down
                    //add foot puff
                    //add foot step sound
                    //apply force
                    break;
            }
        }

        ///<summary>
        ///Deal with bot head animations (probably overkill in 2D)
        ///</summary>
        ///<param name="dt">The animation time delta.</param>
        public void OnAnimateShapeHandler(float dt)
        {
            Rotation = T2DVectorUtil.AngleFromVector(_botEntity.Facing);
            Rotation2 = new Vector3(0, 0, MathHelper.ToRadians(Rotation));

            //TODO: verify next two comments are correct
            const float look = 0.5f;
            float headside = 0.5f; //look forward (0=left, 1=right)

            //top down game
            if (null != _lookTarget)
            {
                float angle =
                    T2DVectorUtil.AngleFromTarget(
                        Position,
                        _lookTarget.Position);
                float diff = (angle - Rotation + 360.0f)%360.0f;
                Assert.Fatal(angle >= 0.0f && angle < 360.0f,
                             "BotShape3D.OnAnimateShapeHandler: Angle out of range");
                if (diff <= 180.0f)
                {
                    headside = 0.5f + diff/360.0f;
                }
                else
                {
                    headside = -0.5f + diff/360.0f;
                }
                //TODO: ouch. brutal neck snap when headside changes from 0 to 1 ...
                //should limit max change per frame ...
            }

            SetSequence("look", "look", look);
            SetSequence("headside", "headside", headside);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly MinimapComponent _minimapComponent;
        private BotEntity _botEntity;
        private T2DShape3D _currentWeapon;
        private GrenadeLauncher _gl;
        private T2DSceneObject _lookTarget;
        private Rifle _rifle;
        private GameManager.GameManager.Teams _team;

        #endregion

        #region IBotSceneObject Members

        ///<summary>
        ///associated <see cref="BotEntity"/> cast as an <see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _botEntity; }
            //set { _botEntity = (BotEntity)value; }
        }

        ///<summary>
        ///scene object
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return this; }
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}