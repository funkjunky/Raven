#region File description
//------------------------------------------------------------------------------
// CombustibleComponent.cs
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
    public class CombustibleComponent : TorqueComponent
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
        /// Gets and sets what this projectile collides with.
        /// </summary>
        public TorqueObjectType CollidesWith
        {
            get { return _collidesWith; }
            set { _collidesWith = value; }
        }

        /// <summary>
        /// Gets and sets if this projectile should be unregistered upon collision.
        /// </summary>
        public bool DestroyOnCollision
        {
            get { return _destroyOnCollision; }
            set { _destroyOnCollision = value; }
        }

        /// <summary>
        /// Gets and sets the object that fired this projectile.
        /// </summary>
        public T2DSceneObject ParentObject
        {
            get { return _parentObj; }
            set { _parentObj = value; }
        }

        /// <summary>
        /// Gets and sets the sound index on the ParentObject used for this
        /// projectile.  Used to stop the projectiles 'flying' sound effect
        /// when it is destroyed.
        /// </summary>
        public int ParentSoundIndex
        {
            get { return _projectileSoundRef; }
            set { _projectileSoundRef = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        /// <summary>
        /// Used in cloning
        /// </summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            CombustibleComponent obj2 = obj as CombustibleComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.CollidesWith = CollidesWith;
            obj2.DestroyOnCollision = DestroyOnCollision;
            obj2.ParentObject = ParentObject;
            obj2.ParentSoundIndex = ParentSoundIndex;
        }

        /// <summary>
        /// Produces an explosion at the point of impact.
        /// </summary>
        /// <param name="info">Information about the collision point.</param>
        /// <param name="sparks">
        /// Should the explosion includes sparks (tank collision) or not 
        /// (barrier collision).
        /// </param>
        public void Explode(T2DCollisionInfo info, bool sparks)
        {
            if (sparks)
            {
                // Trigger the tank sparks
                T2DParticleEffect sparksEffect =
                    (TorqueObjectDatabase.Instance.FindObject<T2DParticleEffect>(
                        "SparkEffect")).Clone() as T2DParticleEffect;
                if (sparksEffect != null)
                {
                    sparksEffect.Position = info.Position;
                    sparksEffect.Rotation = T2DVectorUtil.AngleFromVector(info.Normal);
                    sparksEffect.Size = new Vector2(10.0f, 10.0f);
                    Owner.Manager.Register(sparksEffect);
                }

                MindcraftersComponentsLibrary.SoundBank.PlayCue("explodeTank");
            }
            else
            {
                // Trigger smoke
                T2DParticleEffect smokeEffect =
                    (TorqueObjectDatabase.Instance.FindObject<T2DParticleEffect>(
                        "SmokeEffect")).Clone() as T2DParticleEffect;
                if (smokeEffect != null)
                {
                    smokeEffect.Position = info.Position;

                    // For non-tank hit ie: barriers, we do a little 
                    // calculation so that glancing blows look like they carry 
                    // the momentum of the shell while more direct shots
                    // reflect back towards the direction the shell came from.
                    Vector2 I = SceneObject.Physics.Velocity;
                    I.Normalize();
                    Vector2 R =
                        I - (Vector2.Dot(2.0f * I, info.Normal) * info.Normal);
                    float dot = Vector2.Dot(info.Normal, -I);
                    smokeEffect.Rotation =
                        T2DVectorUtil.AngleFromVector(dot * -I + (1.0f - dot) * R);

                    smokeEffect.Size = new Vector2(10.0f, 10.0f);
                    Owner.Manager.Register(smokeEffect);
                }

                MindcraftersComponentsLibrary.SoundBank.PlayCue("explodeBarrier");
            }

            // Stop the projectile's playing sound on the parent object.  
            // It is stored there to allow the sound to quickly fade rather
            // than just cut off when the projectile object is unregistered.
            MainGunComponent mgc =
                _parentObj.Components.FindComponent<MainGunComponent>();
            if (mgc != null)
            {
                mgc.StopProjectileSound(_projectileSoundRef);
            }

            // Clean up the projectile.
            if (DestroyOnCollision)
                Owner.MarkForDelete = true;
        }

        /// <summary>
        /// Callback for when the projectile collides with another object.
        /// </summary>
        /// <param name="ourObject">The projectile.</param>
        /// <param name="theirObject">The other object.</param>
        /// <param name="info">Information about the collision point.</param>
        /// <param name="resolve">Not used.</param>
        /// <param name="physicsMaterial">The physics properties of the objects.</param>
        public void OnCollision(
            T2DSceneObject ourObject,
            T2DSceneObject theirObject,
            T2DCollisionInfo info,
            ref T2DResolveCollisionDelegate resolve,
            ref T2DCollisionMaterial physicsMaterial)
        {
            if (!ourObject.MarkForDelete &&
                theirObject.TestObjectType(_collidesWith))
            {
                // If we hit a player object then activate its game pad vibration
                T2DStaticSprite psprite = theirObject as T2DStaticSprite;
                if (psprite != null)
                {
                    GamepadVibrationComponent vib =
                        psprite.Components.FindComponent<GamepadVibrationComponent>();
                    if (vib != null)
                    {
                        vib.SetLowSpeedVibration(0.1f, 0.8f);
                    }
                }

                if (theirObject.TestObjectType(TorqueObjectDatabase.Instance.GetObjectType("tank")))
                {
                    Explode(info, true);
                }
                else
                {
                    Explode(info, false);
                }
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

            SceneObject.CollisionsEnabled = true;
            SceneObject.Collision.OnCollision = OnCollision;

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

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        TorqueObjectType _collidesWith;
        bool _destroyOnCollision;
        T2DSceneObject _parentObj = null;
        int _projectileSoundRef = -1;

        #endregion
    }
}
