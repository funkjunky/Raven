#region File description
//------------------------------------------------------------------------------
//MovementComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft
#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;
using Microsoft.Xna.Framework;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///Thrust event data
    ///</summary>
    public struct ThrustData
    {
        ///<summary>
        ///Id
        ///</summary>
        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        ///<summary>
        ///Forward thrust with range 0 to 1
        ///</summary>
        public float ForwardThrust
        {
            get { return _forwardThrust; }
            set { _forwardThrust = value; }
        }

        ///<summary>
        ///Reverse thrust with range 0 to 1
        ///</summary>
        public float ReverseThrust
        {
            get { return _reverseThrust; }
            set { _reverseThrust = value; }
        }

        ///<summary>
        ///Lateral (strafing) thrust with range -1 to 1
        ///</summary>
        public float LateralThrust
        {
            get { return _lateralThrust; }
            set { _lateralThrust = value; }
        }

        ///<summary>
        ///Braking factor with range 0 to 1
        ///</summary>
        public float Braking
        {
            get { return _braking; }
            set { _braking = value; }
        }

        ///<summary>
        ///Angular thrust with range -1 to 1
        ///</summary>
        public float AngularThrust
        {
            get { return _angularThrust; }
            set { _angularThrust = value; }
        }

        private uint _id;
        private float _forwardThrust;
        private float _reverseThrust;
        private float _lateralThrust;
        private float _braking;
        private float _angularThrust;
    }

    ///<summary>
    ///Base class for movement components
    ///</summary>
    [TorqueXmlSchemaType]
    public abstract class MovementComponent : TorqueComponent, ITickObject
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
        public virtual T2DSceneObject SceneObject
        {
            get { return Owner as T2DSceneObject; }
        }

        ///<summary>
        ///Net thrust with range -1 to 1
        ///</summary>
        public virtual float Thrust
        {
            get { return ForwardThrust - ReverseThrust; }
        }

        ///<summary>
        ///Forward thrust with range 0 to 1
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public virtual float ForwardThrust
        {
            get { return _forwardThrust; }
            set { _forwardThrust = value; }
        }

        ///<summary>
        ///Reverse thrust with range 0 to 1
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public virtual float ReverseThrust
        {
            get { return _reverseThrust; }
            set { _reverseThrust = value; }
        }

        ///<summary>
        ///Rectilinear damping factor (simulates friction and inertia)
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.95")]
        public virtual float Damping
        {
            get { return _damping.Value; }
            set { _damping.Value = value; }
        }

        ///<summary>
        ///Maximum forward speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "100.0")]
        public virtual float MaxForwardSpeed
        {
            get { return _maxForwardSpeed.Value; }
            set { _maxForwardSpeed.Value = value; }
        }

        ///<summary>
        ///Maximum reverse speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "50.0")]
        public virtual float MaxReverseSpeed
        {
            get { return _maxReverseSpeed.Value; }
            set { _maxReverseSpeed.Value = value; }
        }

        ///<summary>
        ///Lateral (strafing) thrust with range -1 to 1
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public virtual float LateralThrust
        {
            get { return _lateralThrust; }
            set { _lateralThrust = value; }
        }

        ///<summary>
        ///Maximum lateral (strafing) speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "50.0")]
        public virtual float MaxLateralSpeed
        {
            get { return _maxLateralSpeed.Value; }
            set { _maxLateralSpeed.Value = value; }
        }

        ///<summary>
        ///Lateral (strafing movement) enabled
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "false")]
        public virtual bool LateralThrustEnabled
        {
            get { return _lateralThrustEnabled.Value; }
            set { _lateralThrustEnabled.Value = value; }
        }

        ///<summary>
        ///Braking factor
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public virtual float Braking
        {
            get { return _braking; }
            set { _braking = value; }
        }

        ///<summary>
        ///Angular thrust with range -1 to 1
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public virtual float AngularThrust
        {
            get { return _angularThrust; }
            set { _angularThrust = value; }
        }

        ///<summary>
        ///Angular damping factor (simulates friction and inertia)
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.95")]
        public virtual float AngularDamping
        {
            get { return _angularDamping.Value; }
            set { _angularDamping.Value = value; }
        }

        ///<summary>
        ///Maximum angular speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "20")]
        public virtual float MaxAngularSpeed
        {
            get { return _maxAngularSpeed.Value; }
            set { _maxAngularSpeed.Value = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Called each tick
        ///</summary>
        ///<param name="move"></param>
        ///<param name="dt">
        ///The amount of elapsed time since the last call, in seconds.
        ///</param>
        public virtual void ProcessTick(Move move, float dt)
        {
            if (Program.GamePaused)
            {
                //TODO: save velocity. zero on pause. restore on no pause?
                return;
            }

            ForwardThrust = MathHelper.Clamp(ForwardThrust, 0.0f, 1.0f);
            ReverseThrust = MathHelper.Clamp(ReverseThrust, 0.0f, 1.0f);
            LateralThrust = MathHelper.Clamp(LateralThrust, -1.0f, 1.0f);
            AngularThrust = MathHelper.Clamp(AngularThrust, -1.0f, 1.0f);
            Braking = MathHelper.Clamp(Braking, 0.0f, 1.0f);

            _heading.Value = SceneObject.Rotation;

            //Rotation
            SceneObject.Physics.AngularVelocity *= AngularDamping;
            SceneObject.Physics.AngularVelocity += (1.0f - AngularDamping) *
                AngularThrust * MaxAngularSpeed;

            //Forwards and Backwards
            SceneObject.Physics.VelocityX *= Damping;
            SceneObject.Physics.VelocityY *= Damping;

            SceneObject.Physics.VelocityX += (1.0f - Damping) *
                (float)System.Math.Sin(MathHelper.ToRadians(
                                           SceneObject.Rotation)) *
                (ForwardThrust * MaxForwardSpeed -
                ReverseThrust * MaxReverseSpeed);
            SceneObject.Physics.VelocityY -= (1.0f - Damping) *
                (float)System.Math.Cos(MathHelper.ToRadians(
                                           SceneObject.Rotation)) *
                (ForwardThrust * MaxForwardSpeed -
                ReverseThrust * MaxReverseSpeed);

            //Lateral thrust
            if (!LateralThrustEnabled)
                return;

            SceneObject.Physics.VelocityX += 
                (1.0f - Damping) *
                (float)System.Math.Cos(
                    MathHelper.ToRadians(SceneObject.Rotation)) * 
                    LateralThrust * MaxLateralSpeed;
            SceneObject.Physics.VelocityY += 
                (1.0f - Damping) *
                (float)System.Math.Sin(
                    MathHelper.ToRadians( SceneObject.Rotation)) *
                    LateralThrust * MaxLateralSpeed;
        }

        ///<summary>
        ///Used to interpolate between ticks
        ///</summary>
        ///<param name="k">
        ///The interpolation point (0 to 1) between start and
        ///end of the tick.
        ///</param>
        public virtual void InterpolateTick(float k)
        {
            //todo: interpolate between ticks as needed here
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            MovementComponent obj2 = obj as MovementComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.ForwardThrust = ForwardThrust;
            obj2.ReverseThrust = ReverseThrust;
            obj2.Braking = Braking;
            obj2.Damping = Damping;
            obj2.MaxForwardSpeed = MaxForwardSpeed;
            obj2.MaxReverseSpeed = MaxReverseSpeed;
            obj2.LateralThrust = LateralThrust;
            obj2.MaxLateralSpeed = MaxLateralSpeed;
            obj2.AngularThrust = AngularThrust;
            obj2.AngularDamping = AngularDamping;
            obj2.MaxAngularSpeed = MaxAngularSpeed;
            obj2.LateralThrustEnabled = LateralThrustEnabled;
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

            //activate tick callback for this component.
            ProcessList.Instance.AddTickCallback(Owner, this);

            _thrustEvent = new TorqueEvent<ThrustData>("thrustEvent");
            TorqueEventManager.Instance.MgrListenEvents(
                _thrustEvent, OnThrustEvent, this);

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            TorqueEventManager.Instance.MgrSilenceEvents(
                _thrustEvent, OnThrustEvent, this);

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

            Owner.RegisterCachedInterface(
               "float", "maxAngularSpeed", this, _maxAngularSpeed);
            Owner.RegisterCachedInterface(
                "float", "maxForwardSpeed", this, _maxForwardSpeed);
            Owner.RegisterCachedInterface(
                "float", "maxReverseSpeed", this, _maxReverseSpeed);
            Owner.RegisterCachedInterface(
                "float", "maxLateralSpeed", this, _maxLateralSpeed);
            Owner.RegisterCachedInterface(
                "float", "damping", this, _damping);
            Owner.RegisterCachedInterface(
                "float", "angularDamping", this, _angularDamping);
            Owner.RegisterCachedInterface(
                "bool", "lateralThrustEnabled", this, _lateralThrustEnabled);
            Owner.RegisterCachedInterface(
                "float", "maxBrakingDeceleration", this, _maxBrakingDeceleration);
            Owner.RegisterCachedInterface(
                "float", "heading", this, _heading);
        }

        ///<summary>
        ///Process a thrust event
        ///</summary>
        ///<param name="eventName"></param>
        ///<param name="thrustData"></param>
        protected virtual void OnThrustEvent(
            string eventName, ThrustData thrustData)
        {
            if (thrustData.Id != SceneObject.ObjectId)
                return;

            ForwardThrust = thrustData.ForwardThrust;
            ReverseThrust = thrustData.ReverseThrust;
            LateralThrust = thrustData.LateralThrust;
            Braking = thrustData.Braking;
            AngularThrust = thrustData.AngularThrust;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        float _forwardThrust;
        float _reverseThrust;
        float _angularThrust;
        float _lateralThrust;
        float _braking;

        readonly ValueInPlaceInterface<float> _maxAngularSpeed =
            new ValueInPlaceInterface<float>(20.0f);

        readonly ValueInPlaceInterface<float> _maxForwardSpeed =
            new ValueInPlaceInterface<float>(100.0f);

        readonly ValueInPlaceInterface<float> _maxReverseSpeed =
            new ValueInPlaceInterface<float>(50.0f);

        readonly ValueInPlaceInterface<float> _maxLateralSpeed =
            new ValueInPlaceInterface<float>(50.0f);

        readonly ValueInPlaceInterface<float> _maxBrakingDeceleration =
            new ValueInPlaceInterface<float>(1.0f);

        readonly ValueInPlaceInterface<float> _damping =
            new ValueInPlaceInterface<float>(0.99f);

        readonly ValueInPlaceInterface<float> _angularDamping =
            new ValueInPlaceInterface<float>(0.95f);

        readonly ValueInPlaceInterface<bool> _lateralThrustEnabled =
            new ValueInPlaceInterface<bool>(true);

        readonly ValueInPlaceInterface<float> _heading =
            new ValueInPlaceInterface<float>(0.0f);

        TorqueEvent<ThrustData> _thrustEvent;
        #endregion
    }
}
