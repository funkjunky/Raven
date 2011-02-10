#region File description
//------------------------------------------------------------------------------
//MinimapComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Xml.Serialization;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///A component to display a minimap
    ///</summary>
    [TorqueXmlSchemaType]
    public class MinimapComponent : TorqueComponent
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
        ///An enum to make entering border colors easier
        ///</summary>
        public enum BorderColors : uint
        {
            Black = 0xff000000,     //Color.Black.PackedValue,
            Blue = 0xff0000ff,      //Color.Blue.PackedValue,
            Green = 0xff00ff00,     //Color.Green.PackedValue,
            Red = 0xffff0000,       //Color.Red.PackedValue,
            Yellow = 0xffffff00,    //Color.Yellow.PackedValue,
            White = 0xffffffff,     //Color.White.PackedValue,
        }

        ///<summary>
        ///The position the minimap camera is aiming at
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 CameraCenter
        {
            get { return _cameraCenter; }
            set { _cameraCenter = value; }
        }

        ///<summary>
        ///The size of the area the camera will view
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "2048 1536")]
        public Vector2 CameraExtent
        {
            get { return _cameraExtent; }
            set { _cameraExtent = value; }
        }

        ///<summary>
        ///Where to display the minimap
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "824 0")]
        public Vector2 ViewPosition
        {
            get { return _viewPosition; }
            set { _viewPosition = value; }
        }

        ///<summary>
        ///The size of the minimap
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "200 150")]
        public Vector2 ViewSize
        {
            get { return _viewSize; }
            set { _viewSize = value; }
        }

        ///<summary>
        ///Optionally provide a mask in which none but those TorqueObjectTypes
        ///passing the mask will render.
        ///</summary>
        public TorqueObjectType RenderMask
        {
            get { return _renderMask; }
            set { _renderMask = value; }
        }

        ///<summary>
        ///Optionally provide a mask in which all but those TorqueObjectTypes
        ///passing the mask will render.
        ///</summary>
        public TorqueObjectType NoRenderMask
        {
            get { return _noRenderMask; }
            set { _noRenderMask = value; }
        }

        ///<summary>
        ///If true, show the optional minimap border
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "false")]
        public bool ShowBorder
        {
            get { return _showBorder; }
            set { _showBorder = value; }
        }

        ///<summary>
        ///Color for optional minimap border
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "Red")]
        public BorderColors BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        ///<summary>
        ///The minimap camera
        ///</summary>
        [XmlIgnore]
        public T2DSceneCamera MiniCam
        {
            get 
            {
                if (null == _miniCam)
                    _miniCam = new T2DSceneCamera();
                return _miniCam; 
            }
            protected set { _miniCam = value; }
        }

        ///<summary>
        ///The minimap scene view
        ///</summary>
        [XmlIgnore]
        public GUISceneview MiniView
        {
            get 
            { 
                if (null == _miniView)
                    _miniView = new GUISceneview();
                return _miniView; 
            }
            protected set { _miniView= value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            MinimapComponent obj2 = obj as MinimapComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.CameraCenter = CameraCenter;
            obj2.CameraExtent = CameraExtent;
            obj2.ViewPosition = ViewPosition;
            obj2.ViewSize = ViewSize;
            obj2.RenderMask = RenderMask;
            obj2.NoRenderMask = NoRenderMask;
            obj2.BorderColor = BorderColor;
            obj2.ShowBorder = ShowBorder;
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

            //create a new camera to capture the minimap view
            MiniCam.Name = "MiniCam";
            MiniCam.CenterPosition = CameraCenter;
            MiniCam.Extent = CameraExtent;
            TorqueObjectDatabase.Instance.Register(MiniCam);

            //define the characteristics of the minimap scene view
            if (ShowBorder)
            {
                //set the border color
                //note: setting the color has the side-effect of
                //enabling the GUISceneview's border
                Color borderColor = new Color();
                borderColor.PackedValue = (uint)BorderColor;
                MiniView.BorderComponent.Style.
                   BorderColor[ControlColor.ColorBase] = borderColor;
            }
            MiniView.Name = "MinimapView";
            MiniView.Camera = MiniCam;
            MiniView.RenderMask = RenderMask;
            MiniView.NoRenderMask = NoRenderMask;
            MiniView.Visible = true;
            //add the minimap scene view as a child of the default scene view.
            MiniView.Folder =
               TorqueObjectDatabase.Instance.FindObject<GUISceneview>("DefaultSceneView");
            MiniView.Size = ViewSize;
            MiniView.Position = ViewPosition;

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

        T2DSceneCamera _miniCam;
        GUISceneview _miniView;

        Vector2 _cameraCenter;
        Vector2 _cameraExtent;

        Vector2 _viewPosition;
        Vector2 _viewSize;

        TorqueObjectType _renderMask = TorqueObjectType.AllObjects;
        TorqueObjectType _noRenderMask = TorqueObjectType.NoObjects;

        BorderColors _borderColor;
        bool _showBorder;

        #endregion
    }
}
