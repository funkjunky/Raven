#region File description
//------------------------------------------------------------------------------
//TankCollisionComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;
using GarageGames.Torque.XNA;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///Used to activate gamepad vibration and play appropriate sounds when a
    ///tank collides with another object.
    ///</summary>
    [TorqueXmlSchemaType]
    public class TankCollisionComponent : TorqueComponent
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///This method assumes two rigid bodies and reuses code from
        ///T2DPhysicsComponent's _resolveRigid() delegate.
        ///</summary>
        ///<param name="ourObject">The player's object.</param>
        ///<param name="theirObject">The other object.</param>
        ///<param name="info">Information about the point of collision.</param>
        ///<returns></returns>
        private static Vector2 _CalculateImpactVelocity(
            T2DSceneObject ourObject,
            T2DSceneObject theirObject,
            T2DCollisionInfo info)
        {
            //Positions
            Vector2 srcPosition = ourObject.Position;
            Vector2 dstPosition =
                theirObject != null ? theirObject.Position : Vector2.Zero;

            //Velocities
            Vector2 srcVelocity = ourObject.Physics.Velocity;
            Vector2 dstVelocity = Vector2.Zero;

            //Angular Velocities.
            float srcAngularVelocity =
                MathHelper.ToRadians(-ourObject.Physics.AngularVelocity);
            float dstAngularVelocity = 0.0f;

            if (theirObject != null && theirObject.Physics != null)
            {
                dstVelocity = theirObject.Physics.Velocity;
                dstAngularVelocity =
                    MathHelper.ToRadians(-theirObject.Physics.AngularVelocity);
            }

            //Contact Velocity.
            Vector2 srcContactDelta = info.Position - srcPosition;
            Vector2 dstContactDelta = info.Position - dstPosition;
            Vector2 srcContactDeltaPerp =
                new Vector2(-srcContactDelta.Y, srcContactDelta.X);
            Vector2 dstContactDeltaPerp =
                new Vector2(-dstContactDelta.Y, dstContactDelta.X);
            Vector2 srcVP =
                srcVelocity - srcAngularVelocity * srcContactDeltaPerp;
            Vector2 dstVP =
                dstVelocity - dstAngularVelocity * dstContactDeltaPerp;

            //Calculate Impact Velocity.
            return dstVP - srcVP;
        }

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///Gets the owner of this component cast as a T2DSceneObject
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return Owner as T2DSceneObject; }
        }

        ///<summary>
        ///Gets and sets what this object collides with.
        ///</summary>
        public TorqueObjectType CollidesWith
        {
            get { return _collidesWith; }
            set { _collidesWith = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            TankCollisionComponent obj2 = obj as TankCollisionComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.CollidesWith = CollidesWith;
        }

        ///<summary>
        ///Callback for when the tank collides with another object.
        ///</summary>
        ///<param name="ourObject">The tank.</param>
        ///<param name="theirObject">The other object.</param>
        ///<param name="info">Information about the collision point.</param>
        ///<param name="resolve">Not used.</param>
        ///<param name="physicsMaterial">
        ///The physics properties of the objects.
        ///</param>
        public void OnCollision(
            T2DSceneObject ourObject,
            T2DSceneObject theirObject,
            T2DCollisionInfo info,
            ref T2DResolveCollisionDelegate resolve,
            ref T2DCollisionMaterial physicsMaterial)
        {
            if (ourObject.MarkForDelete || 
                !theirObject.TestObjectType(_collidesWith))
                return;

            //Set up gamepad vibration if this is a player object
            //also set up maxVelocity
            GamepadVibrationComponent vib = 
                ourObject.Components.FindComponent<GamepadVibrationComponent>();
            TankMovementComponent mac = 
                ourObject.Components.FindComponent<TankMovementComponent>();

            float maxSpeed = 0.1f; //avoid possible divide by zero below
            if (null != mac)
            {
                maxSpeed = mac.MaxForwardSpeed;
            }

            //Calculate Impact Velocity.
            Vector2 deltaImpactVelocity =
                _CalculateImpactVelocity(ourObject, theirObject, info);
            float impact = deltaImpactVelocity.Length();
            float vibration =
                MathHelper.Clamp((impact / (maxSpeed * 2.0f)), 0.0f, 1.0f);

            //Perform sound (on all objects) and vibration (players only)
            if (vibration < 0.3)
            {
                //High speed vibration for the small collisions
                if (vib != null)
                {
                    vib.SetHighSpeedVibration(0.2f, (vibration / 0.3f) * 0.8f);
                }

                if (vibration >= 0.1 &&
                    TorqueEngineComponent.Instance.TorqueTime >= 
                    _nextSoftSoundPlayTime)
                {
                    Program.SoundBank.PlayCue("softCollision");
                    _nextSoftSoundPlayTime =
                        TorqueEngineComponent.Instance.TorqueTime +
                        SOUND_TIME_STEP +
                        TorqueUtil.GetFastRandomFloat(SOUND_TIME_STEP_VARIANCE);
                }
            }
            else
            {
                //Low speed vibration for the big collisions
                if (vib != null)
                {
                    vib.SetLowSpeedVibration(
                        0.2f,
                        ((vibration - 0.3f) / 0.7f) * 0.9f + 0.1f);
                }

                if (TorqueEngineComponent.Instance.TorqueTime >=
                    _nextHardSoundPlayTime)
                {
                    Program.SoundBank.PlayCue("hardCollision");
                    _nextHardSoundPlayTime =
                        TorqueEngineComponent.Instance.TorqueTime +
                        SOUND_TIME_STEP +
                        TorqueUtil.GetFastRandomFloat(SOUND_TIME_STEP_VARIANCE);
                }
            }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        ///<summary>
        ///Called when the owner is registered
        ///</summary>
        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
                return false;

            //todo: perform initialization for the component

            //todo: look up interfaces exposed by other components
            //E.g., 
            //_theirInterface = 
            //     Owner.Components.GetInterface<ValueInterface<float>>(
            //         "float", "their interface name");            

            SceneObject.CollisionsEnabled = true;
            SceneObject.Collision.OnCollision = OnCollision;

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            //todo: perform de-initialization for the component

            base._OnUnregister();
        }

        ///<summary>
        ///Called after the owner is registered to allow interfaces
        ///to be registered
        ///</summary>
        protected override void _RegisterInterfaces(TorqueObject owner)
        {
            base._RegisterInterfaces(owner);

            //todo: register interfaces to be accessed by other components
            //E.g.,
            //Owner.RegisterCachedInterface(
            //     "float", "interface name", this, _ourInterface);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        TorqueObjectType _collidesWith;

        float _nextSoftSoundPlayTime;
        float _nextHardSoundPlayTime;
        const float SOUND_TIME_STEP = 50.0f;
        const float SOUND_TIME_STEP_VARIANCE = 150.0f;

        #endregion
    }
}
