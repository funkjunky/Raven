#region File description

//------------------------------------------------------------------------------
//DoorButtonSceneObject.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using GarageGames.Torque.Materials;
using GarageGames.Torque.T2D;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Trigger;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Items
{
    ///<summary>
    ///class for DoorButtonSceneObject
    ///</summary>
    internal class DoorButtonSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="TriggerOnButtonSendMsg"/>
        ///</summary>
        public TriggerOnButtonSendMsg TriggerOnButtonSendMsg
        {
            get { return _triggerOnButtonSendMsg; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="TriggerOnButtonSendMsg"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _triggerOnButtonSendMsg; }
        }

        ///<summary>
        ///scene object
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return this; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///initialize using associated entity
        ///</summary>
        ///<param name="entity">associated entity</param>
        public void Initialize(Entity entity)
        {
            _triggerOnButtonSendMsg = entity as TriggerOnButtonSendMsg;

            SetClosed();

            Layer = 30;
            IsTemplate = false;
            Size = new Vector2(16, 16);
        }

        ///<summary>
        ///Change material to represent door trigger in open state
        ///</summary>
        public void SetOpen()
        {
            //Material =
            //    TorqueObjectDatabase.Instance.FindObject("GreenSquareMaterial")
            //    as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\GreenSquare";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;
        }

        ///<summary>
        ///Change material to represent door trigger in transitioning state
        ///</summary>
        public void SetTransitioning()
        {
            //Material =
            //    TorqueObjectDatabase.Instance.FindObject("YellowSquareMaterial")
            //    as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\YellowSquare";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;
        }

        ///<summary>
        ///Change material to represent door trigger in closed state
        ///</summary>
        public void SetClosed()
        {
            //Material =
            //    TorqueObjectDatabase.Instance.FindObject("RedSquareMaterial")
            //    as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\RedSquare";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private TriggerOnButtonSendMsg _triggerOnButtonSendMsg;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}