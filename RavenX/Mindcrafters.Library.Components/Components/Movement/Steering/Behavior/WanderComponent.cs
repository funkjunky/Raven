#region File description
//------------------------------------------------------------------------------
// WanderComponent.cs
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
    public class WanderComponent : SteeringBehaviorComponent
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        [TorqueXmlSchemaType(DefaultValue = "70.7")]
        public float WanderRadius
        {
            get { return _wanderRadius; }
            set { _wanderRadius = value; _SetWanderJitter(); }
        }

        [TorqueXmlSchemaType(DefaultValue = "100.0")]
        public float WanderDistance
        {
            get { return _wanderDistance; }
            set { _wanderDistance = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "15.0")]
        public float WanderRate
        {
            get { return _wanderRate; }
            set { _wanderRate = value; _SetWanderJitter(); }
        }

        [TorqueXmlSchemaType(DefaultValue = "1.01")]
        public float RecenterFactor
        {
            get { return _recenterFactor; }
            set { _recenterFactor = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "5.0")]
        public float SatisfactionRadius
        {
            get { return _satisfactionRadius; }
            set { _satisfactionRadius = value; }
        }

        public Vector2? TargetPos
        {
            get { return _targetPos; }
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
            WanderComponent obj2 = obj as WanderComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.SatisfactionRadius = SatisfactionRadius;
            obj2.WanderDistance = WanderDistance;
            obj2.WanderRadius = WanderRadius;
            obj2.WanderRate = WanderRate;
            obj2.RecenterFactor = RecenterFactor;
        }

        public override Vector2? DesiredVelocity()
        {
            Vector2? desiredVelocity = null;

            ResetMarker();

            // drift back to center (angle is in radians)
            _theta /= RecenterFactor;
            _theta +=
                (float)TorqueUtil.GetRandomFloat(-_wanderJitter, _wanderJitter);
            _theta = _theta % (float)(Math.PI * 2.0f); // keep in range (0,2*PI)

            Vector2 desiredDirection =
                // heading vector (angle is in degrees)
                T2DVectorUtil.VectorFromAngle(SceneObject.Rotation);

            // project ahead --- WanderDistance affects velocity
            desiredDirection *= WanderDistance;

            Vector2 offset = new Vector2((float)(WanderRadius * Math.Sin(_theta)),
                -(float)(WanderRadius * Math.Cos(_theta))); // find offset on circle
            // rotate to match heading
            offset = _Rotate(
                        offset,
                        MathHelper.ToRadians(SceneObject.Rotation));

            desiredDirection += offset; // this is the (relative) desired target
            _targetPos = desiredDirection + SceneObject.Position;

            float distance = desiredDirection.Length();
            if (distance < SatisfactionRadius)
            {
                _targetPos = null;
                desiredVelocity = Vector2.Zero;
            }
            else
            {
                if (ShowMarker)
                    SetMarker("wanderMarker", (Vector2)_targetPos);
                desiredDirection.Normalize();
                desiredVelocity = desiredDirection * MaxForwardSpeed;
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

            SteeringBehaviorType = SteeringBehaviorTypes.Wander;

            _theta = TorqueUtil.GetRandomFloat(-(float)Math.PI, (float)Math.PI);
            _SetWanderJitter();

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

        protected void _SetWanderJitter()
        {
            _wanderJitter = MathHelper.ToRadians(_wanderRate) / 2.0f;
        }

        protected Vector2 _Rotate(Vector2 v, float angle)
        {
            float x = (float)(v.X * Math.Cos(angle) - v.Y * Math.Sin(angle));
            float y = (float)(v.Y * Math.Cos(angle) + v.X * Math.Sin(angle));
            v.X = x;
            v.Y = y;
            return v;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        float _wanderJitter;// = 0.0f;
        float _wanderRadius;// = 70.7f;// * 5.0f;
        float _wanderDistance;// = 100.0f;// * 5.0f;
        float _wanderRate;// = 15.0f;
        float _recenterFactor;// = 1.01f;

        float _theta;// = 0.0f;

        Vector2? _targetPos = null;

        #endregion
    }
}
