#region File description
//------------------------------------------------------------------------------
// SteeringComponent.cs
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

namespace Mindcrafters.Tx2D.GameAI
{
    /// <summary>
    /// TODO: add component description here
    /// </summary>
    [TorqueXmlSchemaType]
    [TorqueXmlSchemaDependency(Type = typeof(TankMovementComponent))]
    public abstract class SteeringComponent : TorqueComponent, ITickObject
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

        [TorqueXmlSchemaType(DefaultValue = "10.0")]
        public float UpdatesPerSec
        {
            get { return _updatesPerSec; }
            set { _updatesPerSec = value; }
        }

        public Regulator SteeringRegulator
        {
            get
            {
                if (null == _steeringRegulator)
                {
                    _steeringRegulator = new Regulator(UpdatesPerSec);
                }
                return _steeringRegulator;
            }
            set { _steeringRegulator = value; }
        }

        public float MaxForwardSpeed
        {
            get
            {
                if (_maxForwardSpeed != null)
                {
                    return _maxForwardSpeed.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float MaxReverseSpeed
        {
            get
            {
                if (_maxReverseSpeed != null)
                {
                    return _maxReverseSpeed.Value;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float MaxBrakingDeceleration
        {
            get
            {
                if (_maxBrakingDeceleration != null)
                {
                    return _maxBrakingDeceleration.Value;
                }
                else
                {
                    return 0;
                }
            }
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
            if (MindcraftersComponentLibrary.GamePaused)
                return;

            if (_steeringRegulator.IsReady)
            {
                Steer();
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
            SteeringComponent obj2 = obj as SteeringComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.SteeringRegulator = this.SteeringRegulator;
            obj2.UpdatesPerSec = UpdatesPerSec;
        }

        public virtual void Steer() { }

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

            _thrustEvent = new TorqueEvent<ThrustData>("thrustEvent");

            _steeringRegulator = new Regulator(UpdatesPerSec);

            _maxForwardSpeed =
                Owner.Components.GetInterface<ValueInterface<float>>(
                    "float", "maxForwardSpeed");
            _maxReverseSpeed =
                Owner.Components.GetInterface<ValueInterface<float>>(
                    "float", "maxReverseSpeed");
            _maxBrakingDeceleration =
                Owner.Components.GetInterface<ValueInterface<float>>(
                    "float", "maxBrakingDeceleration");

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

        protected void DetermineThrust(Vector2? weightedVelocity)
        {
            if (weightedVelocity != null)
            {
                ThrustData td = new ThrustData();
                td.id = SceneObject.ObjectId;
                td.reverseThrust = 0.0f;
                td.lateralThrust = 0.0f;
                td.braking = 0.0f;

                if ((Vector2)weightedVelocity != Vector2.Zero)
                {
                    Vector2 heading =
                        T2DVectorUtil.VectorFromAngle(
                            SceneObject.Rotation);
                    float thrust =
                        Vector2.Dot((Vector2)weightedVelocity, heading) /
                            heading.Length();

                    td.forwardThrust = // what if thrust<0?
                        thrust / MaxForwardSpeed;
                    float desiredRotation =
                        T2DVectorUtil.AngleFromVector((Vector2)weightedVelocity);
                    td.angularThrust = 0.0f;
                    float diffRot =
                        (desiredRotation -
                            SceneObject.Rotation) % 360.0f;
                    if (diffRot < 0.0f) diffRot += 360.0f;
                    if (diffRot <= 1.0f || diffRot >= 359.0f)
                    {
                        td.angularThrust = 0.0f;
                        SceneObject.Rotation =
                            desiredRotation;
                    }
                    else if (diffRot < 359.0f && diffRot > 180.0f)
                    {
                        td.forwardThrust =
                            MathHelper.Clamp(
                                td.forwardThrust,
                                (float)Math.Abs(Math.Sin(
                                    MathHelper.ToRadians(diffRot))),
                                1.0f);
                        td.angularThrust = -1.0f;
                    }
                    else
                    {
                        td.forwardThrust =
                            MathHelper.Clamp(
                                td.forwardThrust,
                                (float)Math.Abs(Math.Sin(
                                    MathHelper.ToRadians(diffRot))),
                                1.0f);
                        td.angularThrust = 1.0f;
                    }
                }
                else
                {
                    td.forwardThrust = 0.0f;
                    td.angularThrust = 0.0f;
                }

                TorqueEventManager.Instance.MgrPostEvent<ThrustData>(
                       _thrustEvent, td);
            }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        Regulator _steeringRegulator = null;
        float _updatesPerSec = 10.0f;

        TorqueEvent<ThrustData> _thrustEvent;

        ValueInterface<float> _maxForwardSpeed;
        ValueInterface<float> _maxReverseSpeed;
        ValueInterface<float> _maxBrakingDeceleration;

        #endregion
    }
}
