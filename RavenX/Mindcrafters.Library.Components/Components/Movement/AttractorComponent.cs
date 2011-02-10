#region File description
//------------------------------------------------------------------------------
//AttractorComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft
#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.SceneGraph;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary> 
    ///A component to add a pull force to objects in its effect radius
    ///</summary>
    [TorqueXmlSchemaType]
    public class AttractorComponent : TorqueComponent, IAnimatedObject
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
        ///The strength of attraction
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "20")]
        public float Strength
        {
            get { return _strength; }
            set { _strength = value; }
        }

        ///<summary>
        ///The radius of attraction
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "400")]
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
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
            //Keep track of which objects were nearby last tick.
            _oldNearbyObjects.Clear();
            _oldNearbyObjects.AddRange(_nearbyObjects);

            //Use the FindObjects method to get all objects in the scenegraph
            //in a certain radius.
            _nearbyObjects.Clear();
            T2DSceneGraph mySceneGraph =
               TorqueObjectDatabase.Instance.FindObject<T2DSceneGraph>("DefaultSceneGraph");
            mySceneGraph.FindObjects(
               SceneObject.Position,
               _radius,
               TorqueObjectType.AllObjects,
               0xFFFFFFFF,
               _nearbyObjects);

            //Now tell the objects that are within our attraction radius that
            //we're attracting them.
            foreach (ISceneContainerObject nearbyObject in _nearbyObjects)
            {
                //can't attract yourself!
                if (nearbyObject == Owner)
                {
                    continue;
                }
                if (!(nearbyObject is T2DSceneObject))
                    continue;

                T2DSceneObject nObj = nearbyObject as T2DSceneObject;
                MicrobeMovementComponent mmc =
                    nObj.Components.FindComponent<MicrobeMovementComponent>();
                if (mmc != null)
                {
                    //tell the object who we are, and how hard we're
                    //pulling.
                    mmc.AddPull(this, _strength);
                }
            }

            //Remember how we kept track of which objects we were pulling last
            //tick? Well, if we're not going to pull them anymore, we need to
            //tell them.
            _noLongerPullingObjects = _oldNearbyObjects.FindAll(_NotInNewList);

            foreach (ISceneContainerObject oldObject in _noLongerPullingObjects)
            {
                if (!(oldObject is T2DSceneObject))
                    continue;

                T2DSceneObject oObj = oldObject as T2DSceneObject;
                MicrobeMovementComponent mmc =
                    oObj.Components.FindComponent<MicrobeMovementComponent>();
                if (mmc != null)
                {
                    //it's an object we can affect...
                    mmc.RemovePull(this);
                }
            }
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            AttractorComponent obj2 = obj as AttractorComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //marked with the attribute [XmlIgnore]
            //obj2.Property = Property;
            obj2.Strength = Strength;
            obj2.Radius = Radius;
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

        private bool _NotInNewList(ISceneContainerObject obj)
        {
            return !_nearbyObjects.Contains(obj);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        protected float _radius;
        protected float _strength;

        protected List<ISceneContainerObject> _nearbyObjects =
           new List<ISceneContainerObject>();
        protected List<ISceneContainerObject> _oldNearbyObjects =
           new List<ISceneContainerObject>();
        protected List<ISceneContainerObject> _noLongerPullingObjects;

        #endregion
    }
}
