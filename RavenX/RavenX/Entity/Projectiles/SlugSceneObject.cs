#region File description

//------------------------------------------------------------------------------
//SlugSceneObject.cs
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
    ///class for SlugSceneObject
    ///</summary>
    internal class SlugSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="ProjectileSlug"/>
        ///</summary>
        public ProjectileSlug ProjectileSlug
        {
            get { return _projectileSlug; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="ProjectileSlug"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _projectileSlug; }
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
            _projectileSlug = entity as ProjectileSlug;

            //Material =
            //   TorqueObjectDatabase.Instance.FindObject("RedBallMaterial")
            //   as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\RedBall";
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

        private ProjectileSlug _projectileSlug;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}