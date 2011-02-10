#region File description
//------------------------------------------------------------------------------
// GoalGeneratorComponent.cs
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
    public class GoalGeneratorComponent : TorqueComponent, ITickObject
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

        [TorqueXmlSchemaType(DefaultValue = "1.0")]
        public float UpdatesPerSec
        {
            get { return _updatesPerSec; }
            set { _updatesPerSec = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public float UpdatePeriodVariator
        {
            get { return _updatePeriodVariator; }
            set { _updatePeriodVariator = value; }
        }

        [XmlIgnore]
        public Regulator GoalRegulator
        {
            get
            {
                if (null == _goalRegulator)
                {
                    _goalRegulator = new Regulator(UpdatesPerSec, UpdatePeriodVariator);
                }
                return _goalRegulator;
            }
            set { _goalRegulator = value; }
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
            if (MyGame.Instance.GamePaused)
                return;

            if (_goalRegulator.IsReady)
            {
                GenerateGoal();
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
            GoalGeneratorComponent obj2 = obj as GoalGeneratorComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.UpdatesPerSec = UpdatesPerSec;
            obj2.UpdatePeriodVariator = UpdatePeriodVariator;
            obj2.GoalRegulator = this.GoalRegulator;
        }

        public virtual void GenerateGoal()
        {
            // we set up a goal of arriving at Zero to bias the
            // object to return to the map center.
            ArriveComponent ac =
                Owner.Components.FindComponent<ArriveComponent>();
            if (null != ac)
            {
                ac.TargetPos = Vector2.Zero;
            }

            // sets up a seek target which is also the firing target
            // TODO: should have separate move and fire targets
            TargetSelectorComponent tsc =
                SceneObject.Components.FindComponent<TargetSelectorComponent>();
            if (null != tsc)
            {
                tsc.SelectTarget();
            }

            // choose weapon
            // TODO: use main gun at long range and machine guns at close range, etc
            WeaponSelectorComponent wsc =
                SceneObject.Components.FindComponent<WeaponSelectorComponent>();
            if (null != wsc)
            {
                wsc.SelectWeapon();
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

        Regulator _goalRegulator = null;

        //the number of updates to perform per second
        float _updatesPerSec;

        //the number of milliseconds the update period can vary per required
        //update-step. This is here to make sure any multiple clients of this
        //class have their updates spread evenly
        float _updatePeriodVariator;

        #endregion
    }
}
