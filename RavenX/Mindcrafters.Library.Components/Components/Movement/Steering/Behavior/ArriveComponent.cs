#region File description
//------------------------------------------------------------------------------
// ArriveComponent.cs
//
// Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Using directives

#region System

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

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
    public class ArriveComponent : SteeringBehaviorComponent
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        [TorqueXmlSchemaType(DefaultValue = "40.0")]
        public float SatisfactionRadius
        {
            get { return _satisfactionRadius; }
            set { _satisfactionRadius = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "100.0")]
        public float BrakingRadius
        {
            get { return _brakingRadius; }
            set { _brakingRadius = value; }
        }

        [XmlIgnore]
        public Vector2? TargetPos
        {
            get { return _targetPos; }
            set { _targetPos = value; _targetObj = null; }
        }

        [XmlIgnore]
        public T2DSceneObject TargetObj
        {
            get { return _targetObj; }
            set
            {
                _targetObj = value;
                if (null != _targetObj)
                {
                    _targetPos = _targetObj.Position;
                }
            }
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
            ArriveComponent obj2 = obj as ArriveComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.SatisfactionRadius = SatisfactionRadius;
            obj2.BrakingRadius = BrakingRadius;
        }

        public override Vector2? DesiredVelocity()
        {
            Vector2? desiredVelocity = null;

            ResetMarker();

            if (_targetObj != null)
            {
                _targetPos = _targetObj.Position;
            }

            if (_targetPos != null)
            {
                Vector2 desiredDirection =
                    ((Vector2)_targetPos) -
                    SceneObject.Position;
                float distance = desiredDirection.Length();
                if (distance < SatisfactionRadius)
                {
                    _targetPos = null;
                    desiredVelocity = Vector2.Zero;
                }
                else
                {
                    if (ShowMarker)
                        SetMarker("arriveMarker", (Vector2)_targetPos);

                    float speed = MaxForwardSpeed;
                    if (distance < BrakingRadius)
                    {
                        speed *= (distance * distance) /
                            ((BrakingRadius * BrakingRadius) *
                            MaxBrakingDeceleration);
                    }
                    speed =
                        MathHelper.Clamp(
                            speed,
                            0.0f,
                            MaxForwardSpeed);
                    desiredDirection.Normalize();
                    desiredVelocity = desiredDirection * speed;
                }
            }

            return desiredVelocity;
        }

        public override void ResetBehavior()
        {
            _targetPos = null;
            ResetMarker();
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

            SteeringBehaviorType = SteeringBehaviorTypes.Arrive;

            return true;
        }

        /// <summary>
        /// Called when the owner is unregistered
        /// </summary>
        protected override void _OnUnregister()
        {
            // todo: perform de-initialization for the component

            ResetBehavior();

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

        Vector2? _targetPos = null;
        T2DSceneObject _targetObj = null;
        float _brakingRadius;

        #endregion
    }
}
