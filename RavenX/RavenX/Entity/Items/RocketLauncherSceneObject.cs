#region File description

//------------------------------------------------------------------------------
//RocketLauncherSceneObject.cs
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
    ///class for RocketLauncherSceneObject
    ///</summary>
    internal class RocketLauncherSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="TriggerWeaponGiver"/>
        ///</summary>
        public TriggerWeaponGiver TriggerWeaponGiver
        {
            get { return _triggerWeaponGiver; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="TriggerWeaponGiver"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _triggerWeaponGiver; }
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
            _triggerWeaponGiver = entity as TriggerWeaponGiver;

            //Material =
            //   TorqueObjectDatabase.Instance.FindObject("HoverShellMaterial")
            //   as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\GarageGames\HoverShell";
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

        private TriggerWeaponGiver _triggerWeaponGiver;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}