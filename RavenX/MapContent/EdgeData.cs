#region File description

//------------------------------------------------------------------------------
//EdgeData.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///class for edge data
    ///</summary>
    public class EdgeData
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region BehaviorTypes enum

        ///<summary>
        ///edge traversal behaviors
        ///</summary>
        [Flags]
        public enum BehaviorTypes
        {
            Normal = 0,
            Swim = 1 << 0,
            Crawl = 1 << 1,
            Creep = 1 << 3,
            Jump = 1 << 3,
            Fly = 1 << 4,
            Grapple = 1 << 5,
            GoesThroughDoor = 1 << 6
        }

        #endregion

        ///<summary>
        ///Name used to find object by name (should be unique)
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///<summary>
        ///traversal behavior for this edge
        ///</summary>
        public BehaviorTypes BehaviorType
        {
            get { return _behaviorType; }
            set { _behaviorType = value; }
        }

        ///<summary>
        ///edge's from node index
        ///</summary>
        public int FromIndex
        {
            get { return _fromIndex; }
            set { _fromIndex = value; }
        }

        ///<summary>
        ///edge's to node index
        ///</summary>
        public int ToIndex
        {
            get { return _toIndex; }
            set { _toIndex = value; }
        }

        ///<summary>
        ///edge traversal cost
        ///</summary>
        public float Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        ///<summary>
        ///name of the entity (if any) this edge intersects.
        ///for example, this is used to identify doors
        ///</summary>
        public string NameOfIntersectingEntity
        {
            get { return _nameOfIntersectingEntity; }
            set { _nameOfIntersectingEntity = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private BehaviorTypes _behaviorType;
        private float _cost;
        private int _fromIndex;
        private string _name;
        private string _nameOfIntersectingEntity;
        private int _toIndex;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}