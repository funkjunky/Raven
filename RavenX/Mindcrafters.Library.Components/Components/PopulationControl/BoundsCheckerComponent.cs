#region File description
//------------------------------------------------------------------------------
//BoundsCheckerComponent.cs
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
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///A component that responds when the owner goes out of bounds
    ///</summary>
    [TorqueXmlSchemaType]
    public class BoundsCheckerComponent : TorqueComponent, IAnimatedObject
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///A delegate to kill an out of bounds object
        ///</summary>
        static public OnOutOfBoundsDelegate OnOutOfBoundsKill
        {
            get
            {
                if (null == _onOutOfBoundsKill)
                    _onOutOfBoundsKill = Kill;
                return _onOutOfBoundsKill;
            }
        }
        static private OnOutOfBoundsDelegate _onOutOfBoundsKill;

        ///<summary>
        ///A delegate to kill an object at the bounds
        ///</summary>
        static public OnAtBoundsDelegate OnAtBoundsKill
        {
            get
            {
                if (null == _onAtBoundsKill)
                    _onAtBoundsKill = Kill;
                return _onAtBoundsKill;
            }
        }
        static private OnAtBoundsDelegate _onAtBoundsKill;

        ///<summary>
        ///Kill (unregister) owner
        ///</summary>
        ///<param name="owner"> An object to kill</param>
        static protected void Kill(TorqueObject owner)
        {
            owner.Manager.Unregister(owner);
        }

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
        ///Delegate for method to call when object goes out of bounds
        ///</summary>
        ///<param name="owner"></param>
        public delegate void OnOutOfBoundsDelegate(TorqueObject owner);

        ///<summary>
        ///Delegate for method to call when object is at bounds
        ///</summary>
        ///<param name="owner"></param>
        public delegate void OnAtBoundsDelegate(TorqueObject owner);

        ///<summary>
        ///Method to call when object goes out of bounds
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "Mindcrafters.Tx2D.GameAI.BoundsCheckerComponent.OnOutOfBoundsKill", IsDefaultValueOf = true)]
        public OnOutOfBoundsDelegate OnOutOfBounds
        {
            get { return _onOutOfBounds; }
            set { _onOutOfBounds = value; }
        }

        //[TorqueXmlSchemaType(DefaultValue = "Mindcrafters.Tx2D.GameAI.BoundsCheckerComponent.OnAtBoundsKill", IsDefaultValueOf = true)]
        ///<summary>
        ///Method to call when object is at bounds
        ///</summary>
        public OnAtBoundsDelegate OnAtBounds
        {
            get { return _onAtBounds; }
            set { _onAtBounds = value; }
        }

        ///<summary>
        ///If true, bounds are relative to the camera bounds
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "false")]
        public bool UseCameraBounds
        {
            get { return _useCameraBounds; }
            set { _useCameraBounds = value; }
        }

        ///<summary>
        ///If true, we should look up the camera each frame in case it has
        ///changed.
        ///TODO: use an event instead
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "false")]
        public bool CheckCamera
        {
            get { return _checkCamera; }
            set { _checkCamera = value; }
        }

        ///<summary>
        ///Min bounds offset from (0,0) or Camera min
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 MinOffset
        {
            get { return _minOffset; }
            set { _minOffset = value; }
        }

        ///<summary>
        ///Max bounds offset from (0,0) or Camera max
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0 0")]
        public Vector2 MaxOffset
        {
            get { return _maxOffset; }
            set { _maxOffset = value; }
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
            if (CheckCamera)
                _camera = TorqueObjectDatabase.Instance.FindObject<T2DSceneCamera>("Camera");
            if (null == _camera)
                return;

            float minX = (UseCameraBounds ? _camera.SceneMin.X : 0) - MinOffset.X;
            float minY = (UseCameraBounds ? _camera.SceneMin.Y : 0) - MinOffset.Y;
            float maxX = (UseCameraBounds ? _camera.SceneMax.X : 0) + MaxOffset.X;
            float maxY = (UseCameraBounds ? _camera.SceneMax.Y : 0) + MaxOffset.Y;

            if (null != OnOutOfBounds &&
               (SceneObject.Position.X < minX - SceneObject.Size.X / 2.0f ||
               SceneObject.Position.X > maxX + SceneObject.Size.X / 2.0f ||
               SceneObject.Position.Y < minY - SceneObject.Size.Y / 2.0f ||
               SceneObject.Position.Y > maxY + SceneObject.Size.Y / 2.0f))
            {
                OnOutOfBounds(Owner);
            }

            if (null != OnAtBounds &&
               (SceneObject.Position.X < minX + SceneObject.Size.X / 2.0f ||
               SceneObject.Position.X > maxX - SceneObject.Size.X / 2.0f ||
               SceneObject.Position.Y < minY + SceneObject.Size.Y / 2.0f ||
               SceneObject.Position.Y > maxY - SceneObject.Size.Y / 2.0f))
            {
                OnAtBounds(Owner);
            }
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            BoundsCheckerComponent obj2 = obj as BoundsCheckerComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.UseCameraBounds = UseCameraBounds;
            obj2.CheckCamera = CheckCamera;
            obj2.MinOffset = MinOffset;
            obj2.MaxOffset = MaxOffset;
            obj2.OnOutOfBounds = OnOutOfBounds;
            obj2.OnAtBounds = OnAtBounds;
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

            if (!CheckCamera)
                _camera = 
                    TorqueObjectDatabase.Instance.FindObject<T2DSceneCamera>("Camera");

            //The following two lines are commented out so we could add a
            //the camera later. If there is no camera, the bounds checker has no
            //effect. If you uncomment the next two lines, we will fail if there
            //is no camera.
            //if (null == _camera)
            //  return false;

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

        T2DSceneCamera _camera;

        bool _useCameraBounds;
        bool _checkCamera;

        Vector2 _minOffset;
        Vector2 _maxOffset;

        OnOutOfBoundsDelegate _onOutOfBounds;
        OnAtBoundsDelegate _onAtBounds;

        #endregion
    }
}
