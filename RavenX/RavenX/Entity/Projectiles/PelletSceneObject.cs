#region File description

//------------------------------------------------------------------------------
//PelletSceneObject.cs
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
    ///class for PelletSceneObject
    ///</summary>
    internal class PelletSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="ProjectilePellet"/>
        ///</summary>
        public ProjectilePellet ProjectilePellet
        {
            get { return _projectilePellet; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="ProjectilePellet"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _projectilePellet; }
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
            _projectilePellet = entity as ProjectilePellet;

            //Material =
            //   TorqueObjectDatabase.Instance.FindObject("bulletMaterial")
            //   as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename = @"data\images\GarageGames\bullet";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;

            Layer = 0;
            IsTemplate = false;
            Size = new Vector2(4, 4);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private ProjectilePellet _projectilePellet;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}