#region File description
//------------------------------------------------------------------------------
//ShadowComponent.cs
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
    ///class to attach a shadow sprite to an object
    ///</summary>
    [TorqueXmlSchemaType]
    public class ShadowComponent : TorqueComponent, IAnimatedObject
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
        ///Shadow offset
        ///</summary>
        public Vector2 Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        ///<summary>
        ///Gets and sets the shadow sprite.
        ///</summary>
        public T2DStaticSprite ShadowTemplate
        {
            get { return _shadowTemplate; }
            set { _shadowTemplate = value; }
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
            Vector2 offset = Offset;

            if (SceneObject != null && _shadow != null)
            {
                _shadow.Position = SceneObject.Position - offset;
                _shadow.Rotation = SceneObject.Rotation;
            }
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            ShadowComponent obj2 = obj as ShadowComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.ShadowTemplate = ShadowTemplate;
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

            if (ShadowTemplate != null)
            {
                ShadowSprite = (T2DStaticSprite)ShadowTemplate.Clone();
                TorqueObjectDatabase.Instance.Register(ShadowSprite);
                ShadowSprite.Size = SceneObject.Size * 1.2f;
                ShadowSprite.Layer = SceneObject.Layer + 2;
            }

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            //todo: perform de-initialization for the component

            if (_shadow != null)
            {
                TorqueObjectDatabase.Instance.Unregister(_shadow);
                _shadow = null;
            }

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

        ///<summary>
        ///Gets and sets the shadow sprite.
        ///</summary>
        protected T2DStaticSprite ShadowSprite
        {
            get { return _shadow; }
            set { _shadow = value; }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        Vector2 _offset = new Vector2(3.0f, -4.0f);

        T2DStaticSprite _shadow;
        T2DStaticSprite _shadowTemplate;

        #endregion
    }
}
