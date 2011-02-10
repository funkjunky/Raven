#region File description
//------------------------------------------------------------------------------
// SteeringBehaviorComponent.cs
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
    public abstract class SteeringBehaviorComponent : TorqueComponent, ITickObject
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

        public enum SteeringBehaviorTypes { Arrive, Seek, Wander };

        [TorqueXmlSchemaType(DefaultValue = "true")]
        public bool ShowMarker
        {
            get { return _showMarker; }
            set { _showMarker = value; }
        }

        public T2DStaticSprite MarkerTemplate
        {
            get { return _markerTemplate; }
            set { _markerTemplate = value; }
        }

        [XmlIgnore]
        public SteeringBehaviorTypes SteeringBehaviorType
        {
            get { return _steeringBehaviorType; }
            set { _steeringBehaviorType = value; }
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

            // todo: perform processing for component here
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
            SteeringBehaviorComponent obj2 = obj as SteeringBehaviorComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.ShowMarker = ShowMarker;
            obj2.MarkerTemplate = MarkerTemplate;
        }

        public abstract void ResetBehavior();
        public abstract Vector2? DesiredVelocity();

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

        protected void SetMarker(string markerNamePrefix, Vector2 pos)
        {
            if (null != _marker && _marker.IsRegistered)
                _marker.MarkForDelete = true;

            if (null != _markerTemplate)
            {
                _marker = _markerTemplate.Clone() as T2DStaticSprite;

                if (null != _marker)
                {
                    _marker.Position = pos;
                    _marker.Size =
                        new Vector2(_satisfactionRadius, _satisfactionRadius);
                    _marker.Name = markerNamePrefix + SceneObject.ObjectId;
                    TorqueObjectDatabase.Instance.Register(_marker);
                }
            }
        }

        protected void ResetMarker()
        {
            if (null != _marker && _marker.IsRegistered)
                _marker.MarkForDelete = true;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        protected SteeringBehaviorTypes _steeringBehaviorType;

        T2DStaticSprite _marker = null;
        T2DStaticSprite _markerTemplate = null;
        bool _showMarker;

        protected float _satisfactionRadius;

        ValueInterface<float> _maxForwardSpeed;
        ValueInterface<float> _maxReverseSpeed;
        ValueInterface<float> _maxBrakingDeceleration;

        #endregion
    }
}
