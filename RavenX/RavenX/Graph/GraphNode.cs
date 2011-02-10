#region File description

//------------------------------------------------------------------------------
//GraphNode.cs
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

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///Node class to be used with graphs
    ///</summary>
    public class GraphNode
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///valid node indices are positive. This signifies an invalid index.
        ///</summary>
        public const int INVALID_NODE_INDEX = -1;

        ///<summary>
        ///Tests if the given node index is invalid
        ///</summary>
        ///<param name="index"></param>
        ///<returns></returns>
        public static bool IsInvalidIndex(int index)
        {
            return index == INVALID_NODE_INDEX;
        }

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public GraphNode()
        {
            _index = INVALID_NODE_INDEX;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="index"></param>
        public GraphNode(int index)
        {
            _index = index;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///every node has an index. A valid index is >= 0
        ///</summary>
        ///<returns></returns>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        #region Public methods

        ///<summary>
        ///Generate string representation of this node
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            return "NodeIndex: " +
                   (IsInvalidIndex(Index) ? "INVALID" : Index.ToString());
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private int _index;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}