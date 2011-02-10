#region File description

//------------------------------------------------------------------------------
//NavGraphEdge.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Text;
using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using MapContent;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///navigation graph edge class
    ///</summary>
    public class NavGraphEdge : GraphEdge
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///We use zero as an invalid id. It's actually a valid Torque ObjectId
        ///but none of our objects will every get assigned this id.
        ///</summary>
        private const uint INVALID_ID = 0;

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="cost"></param>
        ///<param name="behaviorType"></param>
        ///<param name="id"></param>
        public NavGraphEdge(
            int from,
            int to,
            float cost,
            EdgeData.BehaviorTypes behaviorType,
            uint id)
            : base(from, to, cost)
        {
            _behaviorType = behaviorType;
            _intersectingEntityId = id;
            _intersectingEntityName = null;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="cost"></param>
        ///<param name="behaviorType"></param>
        public NavGraphEdge(
            int from,
            int to,
            float cost,
            EdgeData.BehaviorTypes behaviorType)
            : base(from, to, cost)
        {
            _behaviorType = behaviorType;
            _intersectingEntityId = INVALID_ID;
            _intersectingEntityName = null;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="cost"></param>
        public NavGraphEdge(
            int from,
            int to,
            float cost)
            : base(from, to, cost)
        {
            _behaviorType = EdgeData.BehaviorTypes.Normal;
            _intersectingEntityId = INVALID_ID;
            _intersectingEntityName = null;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="edgeData"></param>
        public NavGraphEdge(EdgeData edgeData)
        {
            From = edgeData.FromIndex;
            To = edgeData.ToIndex;
            Cost = edgeData.Cost;
            _behaviorType = edgeData.BehaviorType;
            _intersectingEntityId = INVALID_ID; //to be filled in later
            _intersectingEntityName = edgeData.NameOfIntersectingEntity;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Each edge has an associated behavior such as crawl, go through door
        ///</summary>
        public EdgeData.BehaviorTypes BehaviorType
        {
            get { return _behaviorType; }
            set { _behaviorType = value; }
        }

        ///<summary>
        ///if this edge intersects with an object (such as a door or lift), then
        ///this is that object's Id. 
        ///</summary>
        public uint IntersectingEntityId
        {
            get
            {
                if (_intersectingEntityId == INVALID_ID &&
                    !String.IsNullOrEmpty(_intersectingEntityName))
                {
                    T2DSceneObject intersectingEntity =
                        TorqueObjectDatabase.Instance.FindObject<T2DSceneObject>(
                            _intersectingEntityName);
                    Assert.Fatal(intersectingEntity != null,
                                 "NavGraphEdge.intersectingEntityId: no entity");
                    _intersectingEntityId = intersectingEntity != null
                                                ?
                                                    intersectingEntity.ObjectId
                                                : INVALID_ID;
                }
                return _intersectingEntityId;
            }
            set { _intersectingEntityId = value; }
        }

        ///<summary>
        ///Name of an entity this edge goes through
        ///Note: this is used to refer to entities before their Id is determined
        ///</summary>
        public string IntersectingEntityName
        {
            get { return _intersectingEntityName; }
            set { _intersectingEntityName = value; }
        }

        ///<summary>
        ///Generate string representation of this edge
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            StringBuilder edgeText = new StringBuilder();
            edgeText.AppendFormat("{0}--[{1}, {3}, {4}]-->{2}",
                                  From, Cost.ToString("F2"), To, BehaviorType,
                                  (String.IsNullOrEmpty(IntersectingEntityName)
                                       ?
                                           "NONE"
                                       : IntersectingEntityName));
            return edgeText.ToString();
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private EdgeData.BehaviorTypes _behaviorType;
        private uint _intersectingEntityId;
        private string _intersectingEntityName;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}