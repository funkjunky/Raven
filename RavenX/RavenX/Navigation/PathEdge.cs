#region File description

//------------------------------------------------------------------------------
//PathEdge.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using MapContent;
using Microsoft.Xna.Framework;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Navigation
{
    ///<summary>
    ///class to represent a path edge. This path can be used by a path
    ///planner in the creation of paths. 
    ///</summary>
    public class PathEdge
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="source"></param>
        ///<param name="destination"></param>
        ///<param name="behavior"></param>
        ///<param name="doorId"></param>
        public PathEdge(
            Vector2 source,
            Vector2 destination,
            EdgeData.BehaviorTypes behavior,
            uint doorId)
        {
            Source = source;
            Destination = destination;
            _behavior = behavior;
            _doorId = doorId;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="source"></param>
        ///<param name="destination"></param>
        ///<param name="behavior"></param>
        public PathEdge(
            Vector2 source,
            Vector2 destination,
            EdgeData.BehaviorTypes behavior)
            : this(source, destination, behavior, 0)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///edge source position
        ///</summary>
        public Vector2 Source
        {
            get { return _source; }
            set { _source = value; }
        }

        ///<summary>
        ///edge destination position
        ///</summary>
        public Vector2 Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        ///<summary>
        ///id of door edge pass through (or invalid id if none)
        ///</summary>
        public uint DoorId
        {
            get { return _doorId; }
        }

        ///<summary>
        ///edge traversal behavior
        ///</summary>
        public EdgeData.BehaviorTypes Behavior
        {
            get { return _behavior; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly EdgeData.BehaviorTypes _behavior;
        private readonly uint _doorId;
        private Vector2 _destination;
        private Vector2 _source;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}