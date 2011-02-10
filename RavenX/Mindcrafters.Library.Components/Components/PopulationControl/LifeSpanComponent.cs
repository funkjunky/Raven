#region File description
//------------------------------------------------------------------------------
//LifeSpanComponent.cs
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
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary> 
    ///A component to kill the object after its life span
    ///</summary> 
    [TorqueXmlSchemaType]
    public class LifeSpanComponent : TorqueComponent, IAnimatedObject
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
        ///time to live
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "30")]
        public float LifeSpan
        {
            get { return _lifeSpan; }
            set { _lifeSpan = value; }
        }

        ///<summary>
        ///variation factor for life span
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "20")]
        public float LifeSpanVariation
        {
            get { return _lifeSpanVariation; }
            set { _lifeSpanVariation = value; }
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
            _age += elapsed;
            if (_age >= _lifeSpan)
            {
                SceneObject.MarkForDelete = true;
                //MyGame.Instance.SoundBank.PlayCue("MicrobeDeath");
            }
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            LifeSpanComponent obj2 = obj as LifeSpanComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //marked with the attribute [XmlIgnore]
            //obj2.Property = Property;
            obj2.LifeSpan = LifeSpan;
            obj2.LifeSpanVariation = LifeSpanVariation;
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

            //randomly adjust life span
            _lifeSpan +=
               (TorqueUtil.GetFastRandomFloat() - 0.5f) * _lifeSpanVariation;

            //MyGame.Instance.SoundBank.PlayCue("MicrobeBirth");

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

        float _lifeSpan;
        float _lifeSpanVariation;
        float _age;

        #endregion
    }
}
