#region File description
//------------------------------------------------------------------------------
//BotMovementComponent.cs
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
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#region other
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    #region Class BotMovementComponent

    ///<summary>
    ///Summary description for BotMovementComponent
    ///</summary>
    [TorqueXmlSchemaType]
    [TorqueXmlSchemaDependency(Type = typeof(T2DPhysicsComponent))]
    public class BotMovementComponent : MovementComponent
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        //Note: override properties to change default values as needed

        /////<summary>
        /////Forward thrust with range 0 to 1
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        //public override float ForwardThrust
        //{
        //    get { return base.ForwardThrust; }
        //    set { base.ForwardThrust = value; }
        //}

        /////<summary>
        /////Reverse thrust with range 0 to 1
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        //public override float ReverseThrust
        //{
        //    get { return base.ReverseThrust; }
        //    set { base.ReverseThrust = value; }
        //}

        ///<summary>
        ///Rectilinear damping factor (simulates friction and inertia)
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0.99")]
        public override float Damping
        {
            get { return base.Damping; }
            set { base.Damping = value; }
        }

        /////<summary>
        /////Maximum forward speed
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "100.0")]
        //public override float MaxForwardSpeed
        //{
        //    get { return base.MaxForwardSpeed; }
        //    set { base.MaxForwardSpeed = value; }
        //}

        /////<summary>
        /////Maximum reverse speed
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "50.0")]
        //public override float MaxReverseSpeed
        //{
        //    get { return base.MaxReverseSpeed; }
        //    set { base.MaxReverseSpeed = value; }
        //}

        /////<summary>
        /////Strafing (lateral) thrust
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        //public override float LateralThrust
        //{
        //    get { return base.LateralThrust; }
        //    set { base.LateralThrust = value; }
        //}

        /////<summary>
        /////Maximum strafing (lateral) speed
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "50.0")]
        //public override float MaxLateralSpeed
        //{
        //    get { return base.MaxLateralSpeed; }
        //    set { base.MaxLateralSpeed = value; }
        //}

        /////<summary>
        /////Strafing (lateral movement) enabled
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "false")]
        //public override bool LateralThrustEnabled
        //{
        //    get { return base.LateralThrustEnabled; }
        //    set { base.LateralThrustEnabled = value; }
        //}

        /////<summary>
        /////Braking factor
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        //public override float Braking
        //{
        //    get { return base.Braking; }
        //    set { base.Braking = value; }
        //}

        /////<summary>
        /////Angular thrust
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        //public override float AngularThrust
        //{
        //    get { return base.AngularThrust; }
        //    set { base.AngularThrust = value; }
        //}

        /////<summary>
        /////Angular damping factor (simulates friction and inertia)
        /////</summary>
        //[TorqueXmlSchemaType(DefaultValue = "0.95")]
        //public override float AngularDamping
        //{
        //    get { return base.AngularDamping; }
        //    set { base.AngularDamping = value; }
        //}

        ///<summary>
        ///Maximum angular speed
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "80")]
        public override float MaxAngularSpeed
        {
            get { return base.MaxAngularSpeed; }
            set { base.MaxAngularSpeed = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        /////<summary>
        /////Called each tick
        /////</summary>
        /////<param name="move"></param>
        /////<param name="dt">
        /////The amount of elapsed time since the last call, in seconds.
        /////</param>
        //public override void ProcessTick(Move move, float dt)
        //{
        //    base.ProcessTick(move, dt);
        //}

        /////<summary>
        /////Used to interpolate between ticks
        /////</summary>
        /////<param name="k">
        /////The interpolation point (0 to 1) between start and
        /////end of the tick.
        /////</param>
        //public override void InterpolateTick(float k)
        //{
        //    base.InterpolateTick(k);
        //}

        /////<summary>
        /////Used in cloning
        /////</summary>
        //public override void CopyTo(TorqueComponent obj)
        //{
        //    base.CopyTo(obj);
        //    //BotMovementComponent obj2 = obj as BotMovementComponent;
        //    //if (obj2 == null)
        //    //    return;

        //    //TODO: add copy for each settable public property that isn't
        //    //marked with the attribute [XmlIgnore]
        //    //obj2.Property = Property;
        //}

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        /////<summary>
        /////Called when the owner is registered
        /////</summary>
        //protected override bool _OnRegister(TorqueObject owner)
        //{
        //    if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
        //        return false;

        //    //todo: perform initialization for the component

        //    //todo: look up interfaces exposed by other components
        //    //E.g., 
        //    //_theirInterface = 
        //    //     Owner.Components.GetInterface<ValueInterface<float>>(
        //    //         "float", "their interface name");  

        //    return true;
        //}

        /////<summary>
        /////Called when the owner is unregistered
        /////</summary>
        //protected override void _OnUnregister()
        //{
        //    //todo: perform de-initialization for the component
        //
        //    base._OnUnregister();
        //}

        /////<summary>
        /////Called after the owner is registered to allow interfaces
        /////to be registered
        /////</summary>
        //protected override void _RegisterInterfaces(TorqueObject owner)
        //{
        //    base._RegisterInterfaces(owner);

        //    //todo: register interfaces to be accessed by other components
        //    //E.g.,
        //    //Owner.RegisterCachedInterface(
        //    //     "float", "interface name", this, _ourInterface);
        //}

        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }

    #endregion
}

