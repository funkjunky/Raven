#region File description

//------------------------------------------------------------------------------
//HealthSceneObject.cs
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
    ///class for HealthSceneObject
    ///</summary>
    internal class HealthSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="TriggerHealthGiver"/>
        ///</summary>
        public TriggerHealthGiver TriggerHealthGiver
        {
            get { return _triggerHealthGiver; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="TriggerHealthGiver"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _triggerHealthGiver; }
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
            _triggerHealthGiver = entity as TriggerHealthGiver;

            //Material =
            //    TorqueObjectDatabase.Instance.FindObject("healthKitMaterial")
            //    as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\GarageGames\healthKit";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;

            Layer = 30;
            IsTemplate = false;
            Size = new Vector2(16, 16);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private TriggerHealthGiver _triggerHealthGiver;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}