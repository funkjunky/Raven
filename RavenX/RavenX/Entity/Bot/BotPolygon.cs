#region File description

//------------------------------------------------------------------------------
//BotPolygon.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using GarageGames.Torque.T2D;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Components;
using Mindcrafters.RavenX.Armory;

    #endregion

    #region GarageGames

    #endregion

    #region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Bot
{
    ///<summary>
    ///BotPolygon is a specialized T2DPolygon with an associated
    ///<see cref="BotEntity"/>
    ///</summary>
    public class BotPolygon : T2DPolygonFill, IBotSceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for blue team with no minimap component
        ///</summary>
        public BotPolygon()
            : this(GameManager.GameManager.Teams.Blue, null)
        {
        }

        ///<summary>
        ///constructor for given team with no minimap component
        ///</summary>
        public BotPolygon(GameManager.GameManager.Teams team)
            : this(team, null)
        {
        }

        ///<summary>
        ///constructor for given team with given minimap component
        ///</summary>
        public BotPolygon(
            GameManager.GameManager.Teams team,
            MinimapComponent minimapComponent)
        {
            _team = team;
            _minimapComponent = minimapComponent;
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

            switch (_team)
            {
                case GameManager.GameManager.Teams.Blue:
                    Color = Microsoft.Xna.Framework.Graphics.Color.Blue.ToVector4();
                    break;
                case GameManager.GameManager.Teams.Red:
                    Color = Microsoft.Xna.Framework.Graphics.Color.Red.ToVector4();
                    break;
                case GameManager.GameManager.Teams.Green:
                    Color = Microsoft.Xna.Framework.Graphics.Color.Green.ToVector4();
                    break;
                case GameManager.GameManager.Teams.Yellow:
                    Color = Microsoft.Xna.Framework.Graphics.Color.Yellow.ToVector4();
                    break;
            }

            Primitive = 6;

            //set the layer     
            Layer = 0;
            Size = new Vector2(32, 32);

            T2DPhysicsComponent physics = new T2DPhysicsComponent();
            physics.InverseMass = 0.1f;
            Components.AddComponent(physics);

            if (null == _minimapComponent)
                return;

            _minimapComponent.MiniCam.Mount(this, "", false);
            _minimapComponent.MiniCam.TrackMountRotation = false;
            switch (_team)
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
        ///deploy/carry/mount/hold weapon
        ///</summary>
        ///<param name="weapon">weapon to hold</param>
        public void HoldWeapon(Weapon weapon)
        {
            //if (null == weapon && null != _currentWeapon)
            //{
            //    _currentWeapon = null;
            //}
            //else if (weapon != null)
            //{
            //    //TODO: need more weapon models
            //    switch (weapon.WeaponType)
            //    {
            //        case WeaponTypes.Railgun:
            //            break;

            //        case WeaponTypes.RocketLauncher:
            //            break;

            //        case WeaponTypes.Shotgun:
            //            break;

            //        //case WeaponTypes.Blaster:
            //        default:
            //            break;
            //    }
            //}
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly MinimapComponent _minimapComponent;
        private BotEntity _botEntity;
        private T2DSceneObject _currentWeapon;
        private T2DSceneObject _lookTarget;
        private GameManager.GameManager.Teams _team;

        #endregion

        #region IBotSceneObject Members

        ///<summary>
        ///associated <see cref="BotEntity"/> cast as an <see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _botEntity; }
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