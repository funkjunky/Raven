#region File description

//------------------------------------------------------------------------------
//NavGraphNode.cs
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
using Mindcrafters.Library.Math;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///Graph node for use in creating a navigation graph.This node contains
    ///the position of the node and a pointer to a BaseGameEntity... useful
    ///if you want your nodes to represent health packs, gold mines and the like
    ///</summary>
    public class NavGraphNode : GraphNode
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public NavGraphNode()
        {
            _extraInfo = null;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="index"></param>
        ///<param name="position"></param>
        public NavGraphNode(int index, Vector2 position)
            : base(index)
        {
            _position = position;
            _extraInfo = null;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="nodeData"></param>
        public NavGraphNode(NodeData nodeData)
        {
            _extraInfo = null;
            Index = nodeData.Index;
            _position = nodeData.Position;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///position of the node
        ///</summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        ///<summary>
        ///often you will require a navgraph node to contain additional
        ///information. For example a node might represent a pickup such as
        ///armor in which case <see cref="_extraInfo"/> could be an enumerated
        ///value denoting the pickup type, thereby enabling a search algorithm
        ///to search a graph for specific items. Going one step further, 
        ///<see cref="_extraInfo"/> could be a reference to the instance of the
        ///item type the node is twinned with. This would allow a search
        ///algorithm to test the status of the pickup during the search. 
        ///</summary>
        public object ExtraInfo
        {
            get { return _extraInfo; }
            set { _extraInfo = value; }
        }

        #region Public methods

        ///<summary>
        ///Generate string representation of this node
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            return base.ToString() +
                   " Position: " + Vector2Util.ToString(Position) +
                   " ExtraInfo: " + ExtraInfo;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private object _extraInfo; //TODO: not typesafe
        private Vector2 _position;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}