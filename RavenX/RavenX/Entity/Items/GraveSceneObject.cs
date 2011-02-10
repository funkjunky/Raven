#region File description

//------------------------------------------------------------------------------
//GraveSceneObject.cs
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
    ///class for GraveSceneObject
    ///</summary>
    internal class GraveSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="GraveMarker"/>
        ///</summary>
        public GraveMarker GraveMarker
        {
            get { return _graveMarker; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="GraveMarker"/> cast as an <see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _graveMarker; }
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
            _graveMarker = entity as GraveMarker;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\gravestoneRIP";
            simpleMaterial.IsTranslucent = true;
            simpleMaterial.IsColorBlended = true;

            Material = simpleMaterial;

            Layer = 30;
            IsTemplate = false;
            Size = new Vector2(32, 32)*2;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private GraveMarker _graveMarker;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}