#region File description

//------------------------------------------------------------------------------
//RocketSceneObject.cs
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
using Mindcrafters.RavenX.Armory;

    #endregion

    #region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Projectiles
{
    ///<summary>
    ///class for RocketSceneObject
    ///</summary>
    internal class RocketSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructor

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="ProjectileRocket"/>
        ///</summary>
        public ProjectileRocket ProjectileRocket
        {
            get { return _projectileRocket; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="ProjectileRocket"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _projectileRocket; }
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
            _projectileRocket = entity as ProjectileRocket;

            //Material =
            //   TorqueObjectDatabase.Instance.FindObject("HoverShellMaterial")
            //   as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\GarageGames\HoverShell";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;

            Layer = 0;
            IsTemplate = false;
            Size = new Vector2(8, 8);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private ProjectileRocket _projectileRocket;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}