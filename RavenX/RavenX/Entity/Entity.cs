#region File description

//------------------------------------------------------------------------------
//Entity.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Messaging;

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
    ///Base class to define a common interface for all game entities
    ///</summary>
    public abstract class Entity
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Tests if an entity is overlapping any entities in the given list
        ///with given <paramref name="minDistBetweenObstacles"/>
        ///</summary>
        ///<param name="gameEntity"></param>
        ///<param name="gameEntities"></param>
        ///<param name="minDistBetweenObstacles"></param>
        ///<returns>
        ///true if an entity is overlapping any entities in the given list
        ///</returns>
        public static bool Overlapped(
            Entity gameEntity,
            List<Entity> gameEntities,
            float minDistBetweenObstacles)
        {
            foreach (Entity curEntity in gameEntities)
            {
                if (Geometry.TwoCirclesOverlapped(
                    gameEntity.Position,
                    gameEntity.BoundingRadius + minDistBetweenObstacles,
                    curEntity.Position,
                    curEntity.BoundingRadius))
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///Tests if an entity is overlapping any entities in the given list
        ///with default minDistBetweenObstacles of 40
        ///</summary>
        ///<param name="gameEntity"></param>
        ///<param name="gameEntities"></param>
        ///<returns>
        ///true if an entity is overlapping any entities in the given list
        ///</returns>
        public bool Overlapped(Entity gameEntity, List<Entity> gameEntities)
        {
            return Overlapped(gameEntity, gameEntities, 40.0f);
        }

        ///<summary>
        ///tags any entities contained in a list that are within the given
        ///radius of the given entity
        ///</summary>
        ///<param name="entity"></param>
        ///<param name="others"></param>
        ///<param name="radius"></param>
        ///<typeparam name="T"></typeparam>
        public static void TagNeighbors<T>(
            Entity entity,
            List<T> others,
            float radius)
            where T : Entity
        {
            //iterate through all entities checking for range
            foreach (T curEntity in others)
            {
                //first clear any current tag
                curEntity.IsTagged = false;

                //work in distance squared to avoid sqrts
                Vector2 to = curEntity.Position - entity.Position;

                //the bounding radius of the other is taken into account by
                //adding it to the range
                float range = radius + curEntity.BoundingRadius;

                //if entity within range, tag for further consideration
                if ((curEntity != entity) &&
                    (to.LengthSquared() < range*range))
                {
                    curEntity.IsTagged = true;
                }
            }
        }

        ///<summary>
        ///Given an entity and a list of nearby entities, this method checks
        ///to see if there is an overlap between entities. If there is, then
        ///the entities are moved away from each other
        ///</summary>
        ///<param name="gameEntity"></param>
        ///<param name="others"></param>
        ///<typeparam name="T"></typeparam>
        public static void EnforceNonPenetrationConstraint<T>(
            Entity gameEntity,
            List<T> others)
            where T : Entity
        {
            //iterate through all entities checking for any overlap of bounding
            //radii
            foreach (T curEntity in others)
            {
                //make sure we don't check against this entity
                if (curEntity == gameEntity)
                    continue;

                //calculate the distance between the positions of the entities
                Vector2 toEntity = gameEntity.Position - curEntity.Position;

                float distFromEachOther = toEntity.Length();

                //if this distance is smaller than the sum of their radii then
                //this entity must be moved away in the direction parallel to
                //the ToEntity vector   
                float amountOfOverLap =
                    curEntity.BoundingRadius +
                    gameEntity.BoundingRadius -
                    distFromEachOther;

                if (amountOfOverLap >= 0)
                {
                    //move the entity a distance away equivalent to the amount
                    //of overlap.
                    gameEntity.Position = gameEntity.Position +
                                          (toEntity/distFromEachOther)*amountOfOverLap;
                }
            }
        }

        ///<summary>
        ///Convert <paramref name="itemType"/> to its corresponding
        ///<see cref="EntityTypes"/>
        ///</summary>
        ///<param name="itemType">item type to convert</param>
        ///<returns>corresponding <see cref="EntityTypes"/></returns>
        ///<exception cref="ApplicationException"></exception>
        public static EntityTypes ItemTypeToEntityType(ItemTypes itemType)
        {
            switch (itemType)
            {
                case ItemTypes.Blaster:
                    return EntityTypes.Blaster;

                case ItemTypes.Health:
                    return EntityTypes.Health;

                case ItemTypes.Railgun:
                    return EntityTypes.Railgun;

                case ItemTypes.RocketLauncher:
                    return EntityTypes.RocketLauncher;

                case ItemTypes.Shotgun:
                    return EntityTypes.Shotgun;

                case ItemTypes.Flag:
                    return EntityTypes.Flag;
                default:
                    throw new ApplicationException(
                        "Entity.ItemTypeToEntityType: cannot determine item type");
            }
        }

        ///<summary>
        ///Convert <paramref name="weaponType"/> to its corresponding
        ///<see cref="EntityTypes"/>
        ///</summary>
        ///<param name="weaponType">weapon type to convert</param>
        ///<returns>corresponding <see cref="EntityTypes"/></returns>
        ///<exception cref="ApplicationException"></exception>
        public static EntityTypes WeaponTypeToEntityType(WeaponTypes weaponType)
        {
            switch (weaponType)
            {
                case WeaponTypes.Blaster:
                    return EntityTypes.Blaster;

                case WeaponTypes.Railgun:
                    return EntityTypes.Railgun;

                case WeaponTypes.RocketLauncher:
                    return EntityTypes.RocketLauncher;

                case WeaponTypes.Shotgun:
                    return EntityTypes.Shotgun;

                default:
                    throw new ApplicationException(
                        "Entity.WeaponTypeToEntityType: cannot determine weapon type");
            }
        }

        ///<summary>
        ///Convert <paramref name="weaponType"/> to its corresponding
        ///<see cref="ItemTypes"/>
        ///</summary>
        ///<param name="weaponType">weapon type to convert</param>
        ///<returns>corresponding <see cref="ItemTypes"/></returns>
        ///<exception cref="ApplicationException"></exception>
        public static ItemTypes WeaponTypeToItemType(WeaponTypes weaponType)
        {
            switch (weaponType)
            {
                case WeaponTypes.Blaster:
                    return ItemTypes.Blaster;

                case WeaponTypes.Railgun:
                    return ItemTypes.Railgun;

                case WeaponTypes.RocketLauncher:
                    return ItemTypes.RocketLauncher;

                case WeaponTypes.Shotgun:
                    return ItemTypes.Shotgun;

                default:
                    throw new ApplicationException(
                        "Entity.WeaponTypeToEntityType: cannot determine weapon type");
            }
        }

        ///<summary>
        ///Convert <paramref name="entityType"/> to its corresponding
        ///<see cref="WeaponTypes"/>
        ///</summary>
        ///<param name="entityType">entity type to convert</param>
        ///<returns>corresponding <see cref="WeaponTypes"/></returns>
        ///<exception cref="ApplicationException"></exception>
        public static WeaponTypes EntityTypeToWeaponType(EntityTypes entityType)
        {
            switch (entityType)
            {
                case EntityTypes.Blaster:
                    return WeaponTypes.Blaster;

                case EntityTypes.Railgun:
                    return WeaponTypes.Railgun;

                case EntityTypes.RocketLauncher:
                    return WeaponTypes.RocketLauncher;

                case EntityTypes.Shotgun:
                    return WeaponTypes.Shotgun;

                default:
                    throw new ApplicationException(
                        "Entity.EntityTypeToWeaponType: cannot determine weapon type");
            }
        }

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="entitySceneObject"></param>
        protected Entity(IEntitySceneObject entitySceneObject)
        {
            entitySceneObject.Initialize(this);
            SceneObject = entitySceneObject.SceneObject;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///scene object
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return _sceneObject; }
            set { _sceneObject = value; }
        }

        ///<summary>
        ///the direction the entity is facing.
        ///<remark>
        ///For moving entities, the heading (not facing) determines the
        ///direction of movement. In bots, facing determines aim and fov
        ///direction. Non-moving entities have a facing but not a heading.
        ///</remark>
        ///TODO: eliminate member field and only use _sceneObject.Rotation
        ///</summary>
        public Vector2 Facing
        {
            get
            {
                return null != _sceneObject
                           ?
                               T2DVectorUtil.VectorFromAngle(_sceneObject.Rotation)
                           :
                               _facing;
            }
            set
            {
                if (null != _sceneObject)
                {
                    if (!Single.IsNaN(value.X) && !Single.IsNaN(value.Y))
                    {
                        _sceneObject.Rotation =
                            T2DVectorUtil.AngleFromVector(value);
                    }
                    _facing = value;
                }
                else
                {
                    if (Single.IsNaN(value.X) || Single.IsNaN(value.Y))
                    {
                    }
                    _facing = value;
                }
            }
        }

        ///<summary>
        ///position
        ///TODO: eliminate member field and use _sceneObject.Position only
        ///</summary>
        public Vector2 Position
        {
            get { return null != _sceneObject ? _sceneObject.Position : _position; }
            set
            {
                if (null != _sceneObject)
                {
                    _sceneObject.Position = value;
                }
                else
                {
                    _position = value;
                }
            }
        }

        ///<summary>
        ///name
        ///TODO: eliminate member field and use _sceneObject.Name only
        ///</summary>
        public string Name
        {
            get { return null != _sceneObject ? _sceneObject.Name : _name; }
            set
            {
                if (null != _sceneObject)
                {
                    _sceneObject.Name = value;
                    _name = value;
                }
                else
                {
                    _name = value;
                }
            }
        }

        ///<summary>
        ///object Id
        ///TODO: eliminate member field and use _sceneObject.ObjectId only
        ///</summary>
        public uint ObjectId
        {
            get { return null != _sceneObject ? _sceneObject.ObjectId : 0; }
        }

        ///<summary>
        ///Scene object's components
        ///</summary>
        public TorqueComponentContainer Components
        {
            get { return null != _sceneObject ? _sceneObject.Components : null; }
        }

        ///<summary>
        ///Is entity tagged?
        ///</summary>
        public bool IsTagged
        {
            get { return _isTagged; }
            set { _isTagged = value; }
        }

        ///<summary>
        ///every entity has a type associated with it (health, troll, ammo etc)
        ///</summary>
        public EntityTypes EntityType
        {
            get { return _entityType; }
            set { _entityType = value; }
        }

        ///<summary>
        ///the magnitude of this object's bounding radius
        ///TODO: should base on Torque collision poly?
        ///</summary>
        public float BoundingRadius
        {
            get { return _boundingRadius; }
            set { _boundingRadius = value; }
        }

        ///<summary>
        ///object scale
        ///TODO: should tie to SceneObject size?
        ///</summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        #region Public methods

        ///<summary>
        ///Update the entity (part of the game update logic)
        ///</summary>
        ///<param name="dt">time since last update</param>
        public virtual void Update(float dt)
        {
        }

        ///<summary>
        ///Render entity
        ///<remarks>
        ///If the entity is a T2DSceneObject, it will be rendered by the engine,
        ///but we may still want to render some additional info
        ///</remarks>
        ///</summary>
        public virtual void Render()
        {
        }

        ///<summary>
        ///Handle messages sent to this entity. Default is to not handle
        ///messages.
        ///</summary>
        ///<param name="msg">the message</param>
        ///<returns>true if message was handled by this entity</returns>
        public virtual bool HandleMessage(Telegram msg)
        {
            return false;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private float _boundingRadius;
        private EntityTypes _entityType;
        private Vector2 _facing;
        private bool _isTagged;
        private string _name;
        private Vector2 _position;
        private Vector2 _scale;
        private T2DSceneObject _sceneObject;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}