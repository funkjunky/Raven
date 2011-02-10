#region File description
//------------------------------------------------------------------------------
//MicrobeMovementComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary> 
    ///A component to control microbe movement via a list of pull forces
    ///Note: a negative pull is a push
    ///</summary> 
    [TorqueXmlSchemaType]
    public class MicrobeMovementComponent : TorqueComponent, IAnimatedObject
    {
        //======================================================================
        #region Static methods, fields, constructors
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
        ///Maximum microbe speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "50")]
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }

        ///<summary>
        ///Maximum change in speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "10")]
        public float MaxSpeedDelta
        {
            get { return _maxSpeedDelta; }
            set { _maxSpeedDelta = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Called every frame.
        ///</summary>
        ///<param name="elapsed">
        ///The amount of elapsed time since the last call, in seconds.
        ///</param>
        public void UpdateAnimation(float elapsed)
        {
            _netDesiredVelocity.X = 0.0f;
            _netDesiredVelocity.Y = 0.0f;

            foreach (TorqueComponent puller in _pull.Keys)
            {
                if (null == puller.Owner) 
                    continue;

                //Get the direction
                _desiredVelocity =
                    Vector2.Subtract(
                        ((T2DSceneObject)puller.Owner).Position,
                        SceneObject.Position);

                if (_desiredVelocity.LengthSquared() > 0.001f)
                {
                    _desiredVelocity.Normalize();
                }

                //scale by the strength
                _desiredVelocity.X *= _pull[puller];
                _desiredVelocity.Y *= _pull[puller];
                _netDesiredVelocity += _desiredVelocity;
            }

            //Cap the velocity with the max speed.
            if (_netDesiredVelocity.Length() > _maxSpeed)
            {
                _netDesiredVelocity.Normalize();
                _netDesiredVelocity.X *= _maxSpeed;
                _netDesiredVelocity.Y *= _maxSpeed;
            }

            //calculate our delta
            _desiredDelta =
               Vector2.Subtract(
                  _netDesiredVelocity,
                  SceneObject.Physics.Velocity);

            //Cap the acceleration with the max speed delta.
            if (_desiredDelta.Length() > _maxSpeedDelta)
            {
                _desiredDelta.Normalize();
                _desiredDelta.X *= _maxSpeedDelta;
                _desiredDelta.Y *= _maxSpeedDelta;
            }

            SceneObject.Physics.Velocity += _desiredDelta;
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            MicrobeMovementComponent obj2 = obj as MicrobeMovementComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //marked with the attribute [XmlIgnore]
            //obj2.Property = Property;
            obj2.MaxSpeed = MaxSpeed;
            obj2.MaxSpeedDelta = MaxSpeedDelta;
        }

        ///<summary>
        ///Add pull force
        ///</summary>
        ///<param name="source">pull source</param>
        ///<param name="strength">pull strength</param>
        public void AddPull(TorqueComponent source, float strength)
        {
            _pull[source] = strength;
        }

        ///<summary>
        ///Remove pull force
        ///</summary>
        ///<param name="source"></param>
        public void RemovePull(TorqueComponent source)
        {
            _pull.Remove(source);
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

            //activate animation callback for this component.
            ProcessList.Instance.AddAnimationCallback(Owner, this);

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

        readonly Dictionary<TorqueComponent, float> _pull =
            new Dictionary<TorqueComponent, float>();

        Vector2 _desiredDelta;
        Vector2 _desiredVelocity = new Vector2(0.0f, 0.0f);
        Vector2 _netDesiredVelocity = new Vector2(0.0f, 0.0f);

        float _maxSpeed;
        float _maxSpeedDelta;

        #endregion
    }
}
