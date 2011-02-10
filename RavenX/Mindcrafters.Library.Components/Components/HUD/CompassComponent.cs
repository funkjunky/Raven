#region File description
//------------------------------------------------------------------------------
//CompassComponent.cs
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
using GarageGames.Torque.GUI;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///A compass class
    ///</summary>
    [TorqueXmlSchemaType]
    public class CompassComponent : TorqueComponent, ITickObject
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
        ///Reverse compass direction
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "false")]
        public bool Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        ///<summary>
        ///Offset to add to compass rotation
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0")]
        public float RotationOffset
        {
            get { return _rotationOffset; }
            set { _rotationOffset = value; }
        }

        ///<summary>
        ///Minimum value of compass parameter
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0")]
        public float MinRange
        {
            get { return _minRange; }
            set { _minRange = value; }
        }

        ///<summary>
        ///Maximum value of compass parameter
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "360")]
        public float MaxRange
        {
            get { return _maxRange; }
            set { _maxRange = value; }
        }

        ///<summary>
        ///Object whose direction is monitored
        ///</summary>
        public T2DSceneObject MonitoredObject
        {
            get { return _monitoredObject; }
            set { _monitoredObject = value; }
        }

        ///<summary>
        ///Interface to value being monitored
        ///</summary>
        public string MonitoredInterfaceName
        {
            get { return _monitoredInterfaceName; }
            set { _monitoredInterfaceName = value; }
        }

        ///<summary>
        ///Camera center position
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 CameraCenter
        {
            get { return _cameraCenter; }
            set { _cameraCenter = value; }
        }

        ///<summary>
        ///Camera size
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "1024 768")]
        public Vector2 CameraExtent
        {
            get { return _cameraExtent; }
            set { _cameraExtent = value; }
        }

        ///<summary>
        ///Compass location
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 ViewPosition
        {
            get { return _viewPosition; }
            set { _viewPosition = value; }
        }

        ///<summary>
        ///Compass size
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "1024 768")]
        public Vector2 ViewSize
        {
            get { return _viewSize; }
            set { _viewSize = value; }
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
            float inputParameter = 0;
            if (null != _monitoredInterface &&
                null != _monitoredObject && _monitoredObject.IsRegistered)
            {
                inputParameter = _monitoredInterface.Value;
            }

            float fractionOfMax = (inputParameter - _minRange) / _maxRange;
            if (_reverse)
                SceneObject.Rotation = 
                    fractionOfMax * -360.0f + _rotationOffset;
            else
                SceneObject.Rotation = 
                    fractionOfMax * 360.0f + _rotationOffset;
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
            CompassComponent obj2 = obj as CompassComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.Reverse = Reverse;
            obj2.RotationOffset = RotationOffset;
            obj2.MinRange = MinRange;
            obj2.MaxRange = MaxRange;
            obj2.MonitoredObject = MonitoredObject;
            obj2.MonitoredInterfaceName = MonitoredInterfaceName;
            obj2.CameraCenter = CameraCenter;
            obj2.CameraExtent = CameraExtent;
            obj2.ViewPosition = ViewPosition;
            obj2.ViewSize = ViewSize;
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

            if (null != _monitoredObject as T2DSpawnObject)
            {
                string name;
                if (_monitoredObject.Name.EndsWith("Template") ||
                    _monitoredObject.Name.EndsWith("template"))
                {
                    name =
                        _monitoredObject.Name.Substring(
                            0, _monitoredObject.Name.Length - 8);
                }
                else
                {
                    name = _monitoredObject.Name + "Instance";
                }
                _monitoredObject =
                    TorqueObjectDatabase.Instance.FindObject<T2DStaticSprite>(name);
            }

            if ((_monitoredObject != null) && (_monitoredInterfaceName != null))
            {
                _monitoredInterface =
                    _monitoredObject.Components.GetInterface<ValueInterface<float>>(
                        "float", _monitoredInterfaceName);
            }

            //create a camera for the compass
            T2DSceneCamera defaultCamera =
                TorqueObjectDatabase.Instance.FindObject<T2DSceneCamera>("Camera");
            if (defaultCamera != null)
            {
                T2DSceneCamera instrumentCam = 
                    (T2DSceneCamera)defaultCamera.Clone();
                instrumentCam.Name = "InstrumentCam";
                TorqueObjectType instrument =
                    TorqueObjectDatabase.Instance.GetObjectType("instrument");

                instrumentCam.CenterPosition = CameraCenter;
                instrumentCam.Extent = CameraExtent;

                TorqueObjectDatabase.Instance.Register(instrumentCam);

                //display the instrument camera on screen
                GUIControlStyle instrumentStyle = new GUIControlStyle();
                GUISceneview instrumentView = new GUISceneview();
                instrumentView.Name = "InstrumentView";
                instrumentView.Style = instrumentStyle;
                instrumentView.Camera = instrumentCam;

                //only render objects of type "instrument"
                instrumentView.RenderMask = instrument;
                instrumentView.Visible = true;
                instrumentView.Folder =
                    TorqueObjectDatabase.Instance.FindObject<GUISceneview>("DefaultSceneView");
                instrumentView.Position = ViewPosition;
                instrumentView.Size = ViewSize;
            }

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

        bool _reverse;
        float _rotationOffset;
        float _minRange;
        float _maxRange;

        T2DSceneObject _monitoredObject;
        string _monitoredInterfaceName;
        ValueInterface<float> _monitoredInterface;

        Vector2 _cameraCenter;
        Vector2 _cameraExtent;

        Vector2 _viewPosition;
        Vector2 _viewSize;

        #endregion
    }
}
