#region File description

//------------------------------------------------------------------------------
//WallSceneObject.cs
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
using Mindcrafters.RavenX.Map;

    #endregion

    #region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Items
{
    ///<summary>
    ///class for WallSceneObject
    ///</summary>
    internal class WallSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="Wall"/>
        ///</summary>
        public Wall Wall
        {
            get { return _wall; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="Wall"/> cast as an <see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _wall; }
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
            _wall = entity as Wall;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\crate";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;

            Layer = 30;
            IsTemplate = false;
            Size = new Vector2(8, 8);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private Wall _wall;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}