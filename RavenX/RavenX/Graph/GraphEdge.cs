#region File description

//------------------------------------------------------------------------------
//GraphEdge.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Text;

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
    ///Class to define an edge connecting two nodes.
    ///         
    ///An edge has an associated cost.
    ///</summary>
    public class GraphEdge
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="cost"></param>
        public GraphEdge(int from, int to, float cost)
        {
            _cost = cost;
            _from = from;
            _to = to;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        public GraphEdge(int from, int to)
            : this(from, to, 1.0f)
        {
        }

        ///<summary>
        ///constructor
        ///</summary>
        public GraphEdge()
            : this(
                GraphNode.INVALID_NODE_INDEX,
                GraphNode.INVALID_NODE_INDEX,
                1.0f)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Get the edge's 'from' node index
        ///An edge connects two nodes.
        ///Valid node indices are always positive.
        ///</summary>
        public int From
        {
            get { return _from; }
            set { _from = value; }
        }

        ///<summary>
        ///Get the edge's 'to' node index
        ///An edge connects two nodes.
        ///Valid node indices are always positive.
        ///</summary>
        public int To
        {
            get { return _to; }
            set { _to = value; }
        }

        ///<summary>
        ///the cost of traversing the edge
        ///</summary>
        public float Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        #region Public methods

        ///<summary>
        ///Generate string representation of this edge
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            StringBuilder edgeText = new StringBuilder();
            edgeText.AppendFormat("{0}--[{1}]-->{2}", From, Cost, To);
            return edgeText.ToString();
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private float _cost;
        private int _from;
        private int _to;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}