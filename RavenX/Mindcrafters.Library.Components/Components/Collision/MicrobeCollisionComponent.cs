#region File description
//------------------------------------------------------------------------------
//MicrobeCollisionComponent.cs
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

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary> 
    ///Custom collision handler for microbes
    ///</summary> 
    [TorqueXmlSchemaType]
    public class MicrobeCollisionComponent : TorqueComponent
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
        ///Gets and sets what this object collides with.
        ///</summary>
        public TorqueObjectType CollidesWith
        {
            get { return _collidesWith; }
            set { _collidesWith = value; }
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
            MicrobeCollisionComponent obj2 = obj as MicrobeCollisionComponent; 
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //marked with the attribute [XmlIgnore]
            //obj2.Property = Property;
            obj2.CollidesWith = CollidesWith;
        }

        ///<summary>
        ///Callback for when the microbe collides with another object.
        ///</summary>
        ///<param name="ourObject">The microbe.</param>
        ///<param name="theirObject">The other object.</param>
        ///<param name="info">Information about the collision point.</param>
        ///<param name="resolve">Not used.</param>
        ///<param name="physicsMaterial">
        ///The physics properties of the objects.
        ///</param>
        public void OnCollision(
           T2DSceneObject ourObject,
           T2DSceneObject theirObject,
           T2DCollisionInfo info,
           ref T2DResolveCollisionDelegate resolve,
           ref T2DCollisionMaterial physicsMaterial)
        {
            if (!ourObject.MarkForDelete &&
               theirObject.TestObjectType(_collidesWith))
            {
                //handle microbe collision with another microbe
                if (theirObject.TestObjectType(
                   TorqueObjectDatabase.Instance.GetObjectType("microbe")))
                {
                    T2DPhysicsComponent.BounceCollision.Invoke(
                       ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.ClampCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.KillCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.RigidCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.StickyCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);
                }
                else if (theirObject.TestObjectType(
                   TorqueObjectDatabase.Instance.GetObjectType("projectile")))
                {
                    //T2DPhysicsComponent.BounceCollision.Invoke(
                    //  ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.ClampCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);

                    T2DPhysicsComponent.KillCollision.Invoke(
                     ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.RigidCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);

                    //T2DPhysicsComponent.StickyCollision.Invoke(
                    //ourObject, theirObject, ref info, physicsMaterial, false);
                }
                //TODO: add handling for other object types
            }
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

            //turn on collision handling and specify the handler
            SceneObject.CollisionsEnabled = true;
            SceneObject.Collision.OnCollision = OnCollision;

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

        TorqueObjectType _collidesWith;

        #endregion
    }
}
