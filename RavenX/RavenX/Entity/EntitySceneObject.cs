#region File description

//------------------------------------------------------------------------------
//EntitySceneObject.cs
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

using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity
{
    ///<summary>
    ///class for EntitySceneObject
    ///</summary>
    public class EntitySceneObject : IEntitySceneObject
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public EntitySceneObject()
            : this(new T2DSceneObject())
        {
        }

        ///<summary>
        ///constructor from given T2DSceneObject
        ///</summary>
        ///<param name="sceneObject"></param>
        public EntitySceneObject(T2DSceneObject sceneObject)
        {
            _sceneObject = sceneObject;
            _entity = null;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region IEntitySceneObject Members

        ///<summary>
        ///associated entity
        ///</summary>
        public Entity Entity
        {
            get { return _entity; }
        }

        ///<summary>
        ///scene object
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return _sceneObject; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///initialize using associated entity
        ///</summary>
        ///<param name="entity">associated entity</param>
        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly T2DSceneObject _sceneObject;
        private Entity _entity;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}