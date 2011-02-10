#region File description

//------------------------------------------------------------------------------
//DoorSceneObject.cs
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
    ///class for DoorSceneObject
    ///</summary>
    internal class DoorSceneObject : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///associated <see cref="Door"/>
        ///</summary>
        public Door Door
        {
            get { return _door; }
        }

        #region IEntitySceneObject Members

        ///<summary>
        ///associated <see cref="Door"/> cast as an <see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _door; }
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
            _door = entity as Door;

            //Material =
            //   TorqueObjectDatabase.Instance.FindObject("corrodedMetalMaterial")
            //   as SimpleMaterial;

            SimpleMaterial simpleMaterial = new SimpleMaterial();
            simpleMaterial.TextureFilename =
                @"data\images\Mindcrafters\corrodedMetal";
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

        private Door _door;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}