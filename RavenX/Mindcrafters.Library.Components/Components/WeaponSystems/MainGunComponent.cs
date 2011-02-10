#region File description
//------------------------------------------------------------------------------
// MainGunComponent.cs
//
// Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Using directives

#region System

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.MathUtil;
using GarageGames.Torque.SceneGraph;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    /// <summary>
    /// TODO: add component description here
    /// </summary>
    [TorqueXmlSchemaType]
    //[TorqueXmlSchemaDependency(Type = typeof(TargetSelectorComponent))]
    public class MainGunComponent : TorqueComponent, ITickObject
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        /// <summary>
        /// Gets the owner of this component cast as a T2DSceneObject
        /// </summary>
        public T2DSceneObject SceneObject
        {
            get { return Owner as T2DSceneObject; }
        }

        /// <summary>
        /// How fast may a tank fire a shell (in seconds).
        /// </summary>
        [TorqueXmlSchemaType(DefaultValue = "0.15")]
        public float FireRate
        {
            get { return _fireRate; }
            set { _fireRate = value; }
        }

        /// <summary>
        /// Gets and sets the template used for the tank's shell sprite.
        /// </summary>
        public T2DSceneObject ProjectileTemplate
        {
            get { return _projectileTemplate; }
            set { _projectileTemplate = value; }
        }

        /// <summary>
        /// Gets and sets what this tank's projectile should not collide with.
        /// Usually set to the firing tank itself but could also be used to
        /// not hit team mates.
        /// </summary>
        public TorqueObjectType ProjectileDoNotHitType
        {
            get { return _projectileDoNotHitType; }
            set { _projectileDoNotHitType = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "true")]
        public bool IsAI
        {
            get { return _isAI; }
            set { _isAI = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        /// <summary>
        /// Called each tick
        /// </summary>
        /// <param name="dt">
        /// The amount of elapsed time since the last call, in seconds.
        /// </param>
        public virtual void ProcessTick(Move move, float dt)
        {
            if (MindcraftersComponentsLibrary.GamePaused)
                return;

            // Timer to restrict how fast a tank shell may be fired.
            if (_countdownToFire > 0f)
            {
                _countdownToFire -= dt;
            }

            if (!IsAI && move != null)
            {
                // The user has requested a tank shell be fired.
                if (move.Buttons[0].Pushed)
                {
                    if (_countdownToFire <= 0f)
                    {
                        _Fire();
                    }
                }
            }
            else if (IsAI && _HaveTargetInLOS())
            {
                if (_countdownToFire <= 0f)
                {
                    _Fire();
                }
            }
        }

        /// <summary>
        /// Used to interpolate between ticks
        /// </summary>
        /// <param name="dt">
        /// The interpolation point (0 to 1) between start and
        /// end of the tick.
        /// </param>
        public virtual void InterpolateTick(float k)
        {
            // todo: interpolate between ticks as needed here
        }

        /// <summary>
        /// Used in cloning
        /// </summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            MainGunComponent obj2 = obj as MainGunComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.FireRate = FireRate;
            obj2.ProjectileTemplate = ProjectileTemplate;
            obj2.ProjectileDoNotHitType = ProjectileDoNotHitType;
            obj2.IsAI = IsAI;
        }

        /// <summary>
        /// Stops the projectile flying through the air sound. It is controlled
        /// here rather than on the projectile itself to allow the sound to
        /// quickly fade rather than cut off when the projectile is destroyed.
        /// </summary>
        /// <param name="index">
        /// Index into the projectile sound list.  
        /// Assigned when the projectile is fired.
        /// </param>
        public void StopProjectileSound(int index)
        {
            Assert.Fatal(index >= 0 && index < _maxProjectileSounds,
                "MainGunComponent.StopProjectileSound() sound index out of range");
            if (index < 0 || index >= _maxProjectileSounds)
                return;

            if (_projectileSounds[index] != null &&
                _projectileSounds[index].IsPlaying)
            {
                _projectileSounds[index].Stop(AudioStopOptions.AsAuthored);
            }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        /// <summary>
        /// Called when the owner is registered
        /// </summary>
        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
                return false;

            // todo: perform initialization for the component

            // todo: look up interfaces exposed by other components
            // E.g., 
            // _theirInterface = 
            //      Owner.Components.GetInterface<ValueInterface<float>>(
            //          "float", "their interface name");  

            // activate tick callback for this component.
            ProcessList.Instance.AddTickCallback(Owner, this);

            _countdownToFire = 0.0f;

            return true;
        }

        /// <summary>
        /// Called when the owner is unregistered
        /// </summary>
        protected override void _OnUnregister()
        {
            // todo: perform de-initialization for the component

            base._OnUnregister();
        }

        /// <summary>
        /// Called after the owner is registered to allow interfaces
        /// to be registered
        /// </summary>
        protected override void _RegisterInterfaces(TorqueObject owner)
        {
            base._RegisterInterfaces(owner);

            // todo: register interfaces to be accessed by other components
            // E.g.,
            // Owner.RegisterCachedInterface(
            //      "float", "interface name", this, _ourInterface);
        }

        /// <summary>
        /// Fire a tank shell.
        /// </summary>
        void _Fire()
        {
            T2DSceneObject proj = (T2DSceneObject)_projectileTemplate.Clone();
            proj.Rotation = SceneObject.Rotation;

            // Allow the projectile to blow up.
            CombustibleComponent cc =
                proj.Components.FindComponent<CombustibleComponent>();
            if (cc != null)
            {
                cc.ParentObject = SceneObject;
                // Assign a sound index to this projectile.
                cc.ParentSoundIndex = _nextFreeProjectileSound;
            }

            // Remove the firing player from the projectile's collides with
            // list and add AI players
            proj.Collision.CollidesWith -= _projectileDoNotHitType;
            //proj.Collision.CollidesWith += GameData.Instance.AIPlayerObjectType;

            // Calculate a firing solution
            Vector2 firepos = new Vector2(0.0f, -32.5f);
            Rotation2D rot = new Rotation2D(MathHelper.ToRadians(SceneObject.Rotation));
            firepos = rot.Rotate(firepos);
            proj.Position = SceneObject.Position + firepos;

            Vector2 vel = new Vector2();
            vel.X = (float)System.Math.Sin((double)MathHelper.ToRadians(-proj.Rotation)) *
                -_projectileVelocity;
            vel.Y = (float)System.Math.Cos((double)MathHelper.ToRadians(proj.Rotation)) *
                -_projectileVelocity;

            proj.Physics.Velocity = vel;

            Owner.Manager.Register(proj);

            // Apply an impulse to the firing player
            Vector2 impulse = new Vector2();
            impulse.X = (float)System.Math.Sin((double)MathHelper.ToRadians(
                -SceneObject.Rotation + 180.0f)) * -_projectileKickback;
            impulse.Y = (float)System.Math.Cos((double)MathHelper.ToRadians(
                SceneObject.Rotation - 180.0f)) * -_projectileKickback;
            SceneObject.Physics.ApplyImpulse(impulse);

            // Apply game pad vibration to the firing player if appropriate
            T2DStaticSprite psprite = SceneObject as T2DStaticSprite;
            if (psprite != null)
            {
                GamepadVibrationComponent vib =
                    psprite.Components.FindComponent<GamepadVibrationComponent>();
                if (vib != null)
                {
                    vib.SetHighSpeedVibration(0.1f, 0.5f);
                }
            }

            // Play muzzle flash particles
            T2DParticleEffect flash =
                SceneObject.GetMountedObject("muzzle") as T2DParticleEffect;
            if (flash != null)
            {
                flash.PlayEffect(false);
            }

            // Play sounds.  Projectile whistle sounds are buffered on us to
            // allow them to fade when the projectile is destroyed.  Otherwise
            // the sound would just stop.
            MindcraftersComponentsLibrary.SoundBank.PlayCue("fireShell");
            if (_projectileSounds[_nextFreeProjectileSound] != null &&
                _projectileSounds[_nextFreeProjectileSound].IsPlaying)
                _projectileSounds[_nextFreeProjectileSound].Stop(
                    AudioStopOptions.Immediate);
            _projectileSounds[_nextFreeProjectileSound] =
                MindcraftersComponentsLibrary.SoundBank.GetCue("shellWhistle");
            _projectileSounds[_nextFreeProjectileSound].Play();

            _nextFreeProjectileSound++;
            if (_nextFreeProjectileSound >= _maxProjectileSounds)
                _nextFreeProjectileSound = 0;

            //TODO: update shots fired statistic

            _countdownToFire = FireRate;
        }

        bool _HaveTargetInLOS()
        {
            TargetSelectorComponent tsc =
                SceneObject.Components.FindComponent<TargetSelectorComponent>();
            if (null != tsc && null != tsc.Target && tsc.Target.IsRegistered)
            {
                float angle =
                    T2DVectorUtil.AngleFromTarget(
                        SceneObject.Position, tsc.Target.Position) -
                        SceneObject.Rotation;
                if (System.Math.Abs(angle) < 1)
                    return true;
            }
            return false;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        float _fireRate;
        float _countdownToFire;

        T2DSceneObject _projectileTemplate;
        TorqueObjectType _projectileDoNotHitType;

        float _projectileVelocity = 300.0f;
        float _projectileKickback = 20000.0f;

        // For projectile sounds
        const int _maxProjectileSounds = 20;
        Cue[] _projectileSounds = new Cue[_maxProjectileSounds];
        int _nextFreeProjectileSound = 0;

        bool _isAI;

        #endregion
    }
}
