#region File description
//------------------------------------------------------------------------------
//CameraComponent.cs
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
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///class for CameraComponent. Mounts camera on owner.
    ///</summary>
    [TorqueXmlSchemaType]
    public class CameraComponent : TorqueComponent
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
        ///camera center position
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 CameraCenter
        {
            get { return _cameraCenter; }
            set { _cameraCenter = value; }
        }

        ///<summary>
        ///camera size
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "1024 768")]
        public Vector2 CameraExtent
        {
            get { return _cameraExtent; }
            set { _cameraExtent = value; }
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
            CameraComponent obj2 = obj as CameraComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.CameraCenter = CameraCenter;
            obj2.CameraExtent = CameraExtent;
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

            T2DSceneCamera camera =
                TorqueObjectDatabase.Instance.FindObject<T2DSceneCamera>("Camera");

            camera.CenterPosition = CameraCenter;
            camera.Extent = CameraExtent;
            camera.Mount(SceneObject, "", false);
            camera.TrackMountRotation = false;

            GUISceneview sceneview = new GUISceneview();
            sceneview.Name = "DefaultSceneView";
            sceneview.Camera = camera;
            sceneview.NoRenderMask =
                TorqueObjectDatabase.Instance.GetObjectType("instrument");

            GUISceneview minimapView =
                TorqueObjectDatabase.Instance.FindObject<GUISceneview>("MinimapView");
            if (null != minimapView)
                minimapView.Folder = sceneview;
            GUISceneview instrumentView =
                TorqueObjectDatabase.Instance.FindObject<GUISceneview>("InstrumentView");
            if (null != instrumentView)
                instrumentView.Folder = sceneview;

            GUICanvas.Instance.SetContentControl(sceneview);

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

        Vector2 _cameraCenter;
        Vector2 _cameraExtent;

        #endregion
    }
}
