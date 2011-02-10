#region File description

//------------------------------------------------------------------------------
//EntityManager.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using GarageGames.Torque.Core;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity
{
    ///<summary>
    ///Singleton class to handle the  management of Entities.
    ///TODO: phase out (use TorqueObjectBase only)  
    ///</summary>
    public class EntityManager
    {
        #region Static methods, fields, constructors

        #region Singleton pattern

        private static EntityManager _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private EntityManager()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;
        }

        ///<summary>
        ///Accessor for the EntityManager singleton instance.
        ///</summary>
        public static EntityManager Instance
        {
            get
            {
                if (null == _instance)
                    new EntityManager();

                Assert.Fatal(null != _instance,
                             "Singleton instance not set by constructor.");
                return _instance;
            }
        }

        #endregion

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        ///<summary>
        ///clears all entities from the entity map
        ///</summary>
        public void Reset()
        {
            _entityMap.Clear();
        }

        ///<summary>
        ///Gets the entity with the given Id
        ///</summary>
        ///<param name="id"></param>
        ///<returns></returns>
        public Entity GetEntityFromId(uint id)
        {
            //assert that the entity is a member of the map
            Assert.Fatal(_entityMap.ContainsKey(id),
                         "EntityManager.GetEntityFromId: invalid Id");

            //Entity entity = TorqueObjectDatabase.Instance.FindObject(id) as Entity;

            //Assert.Fatal(entity == _entityMap[id],
            //   "EntityManager.GetEnityFromId: Torque id doesn't match raven id.");

            return _entityMap[id];
        }

        ///<summary>
        ///Remove the given entity from <see cref="_entityMap"/>.
        ///</summary>
        ///<param name="entity"></param>
        public void RemoveEntity(Entity entity)
        {
            Assert.Fatal(entity.SceneObject != null,
                         "EntityManager.RemoveEntity: no SceneObject");

            //if (null == entity.SceneObject)
            //{
            //   throw new ApplicationException("EntityManager.RemoveEntity: no SceneObject");
            //}

            TorqueObjectDatabase.Instance.Unregister(entity.SceneObject);
            _entityMap.Remove(entity.ObjectId);
        }

        ///<summary>
        ///Register the given entity
        ///</summary>
        ///<param name="newEntity"></param>
        public void RegisterEntity(Entity newEntity)
        {
            Assert.Fatal(!string.IsNullOrEmpty(newEntity.Name),
                         "EntityManager.RegisterEntity: no name");

            Assert.Fatal(newEntity.SceneObject != null,
                         "EntityManager.RegisterEntity: no SceneObject");

            //if (null == newEntity.SceneObject)
            //{
            //   throw new ApplicationException("EntityManager.RegisterEntity: no SceneObject");
            //}
            if (!newEntity.SceneObject.IsRegistered)
                TorqueObjectDatabase.Instance.Register(newEntity.SceneObject);
            _entityMap.Add(newEntity.ObjectId, newEntity);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly Dictionary<uint, Entity> _entityMap =
            new Dictionary<uint, Entity>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}