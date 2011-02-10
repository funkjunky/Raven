#region File description
//------------------------------------------------------------------------------
// TargetSelectorComponent.cs
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
    public class TargetSelectorComponent : TorqueComponent
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

        public TorqueObjectType TargetType
        {
            get { return _targetType; }
            set { _targetType = value; }
        }

        public T2DSceneObject Target
        {
            get { return _target; }
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
            TargetSelectorComponent obj2 = obj as TargetSelectorComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.TargetType = TargetType;
        }

        /// <summary>
        /// Select target to move to using Seek (or Arrive).
        /// Note: this currently assumes the move target and fire target are the same
        /// </summary>
        public void SelectTarget()
        {
            SeekComponent steer =
                Owner.Components.FindComponent<SeekComponent>();
            if (null == steer)
                return;

            if (null != steer.TargetObj &&
                (!steer.TargetObj.IsRegistered || _IsOffscreen(steer.TargetObj)))
            {
                _target = steer.TargetObj = null;
                steer.TargetPos = null;
            }

            if (null == steer.TargetObj && null == steer.TargetPos)
            {
                List<T2DSceneObject> possibleTargets =
                    TorqueObjectDatabase.Instance.FindObjects<T2DSceneObject>();

                foreach (T2DSceneObject so in possibleTargets)
                {
                    if (!so.IsTemplate && so.Visible && !_IsOffscreen(so) &&
                        so.TestObjectType(_targetType) &&
                        TorqueUtil.GetFastRandomFloat() < 0.25f) // chance of selecting target
                    {
                        _target = steer.TargetObj = so;
                        break;
                    }
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

            // store a reference to the camera (we're assuming the camera doesn't change)
            _camera = TorqueObjectDatabase.Instance.FindObject<T2DSceneCamera>("Camera");
            if (null == _camera)
                return false;

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

        protected bool _IsOffscreen(T2DSceneObject so)
        {
            if (so.Position.X < _camera.SceneMin.X - so.Size.X ||
                so.Position.X > _camera.SceneMax.X + so.Size.X)
            {
                return true;
            }
            if (so.Position.Y < _camera.SceneMin.Y - so.Size.Y ||
                so.Position.Y > _camera.SceneMax.Y + so.Size.Y)
            {
                return true;
            }
            return false;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        T2DSceneCamera _camera;

        TorqueObjectType _targetType;

        T2DSceneObject _target;

        #endregion
    }
}
