#region File description
//------------------------------------------------------------------------------
//InvertedAABBox2D.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames
#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class InvertedAABBox2D

    ///<summary>
    ///simple inverted (y increases down screen) axis aligned bounding box class
    ///</summary>
    public class InvertedAABBox2D
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor for inverted axis aligned bounding box class
        ///</summary>
        ///<param name="topLeft">top left point</param>
        ///<param name="bottomRight">bottom right point</param>
        public InvertedAABBox2D(Vector2 topLeft, Vector2 bottomRight)
        {
            _topLeft = topLeft;
            _bottomRight = bottomRight;
            _center = (topLeft + bottomRight) / 2.0f;
            _topRight = new Vector2(Right, Top);
            _bottomLeft = new Vector2(Left,Bottom);
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///top left point of the inverted axis aligned bounding box
        ///</summary>
        public Vector2 TopLeft
        {
            get { return _topLeft; }
        }

        ///<summary>
        ///bottom right point of the inverted axis aligned bounding box
        ///</summary>
        public Vector2 BottomRight
        {
            get { return _bottomRight; }
        }

        ///<summary>
        ///bottom left point of the inverted axis aligned bounding box
        ///</summary>
        public Vector2 BottomLeft
        {
            get { return _bottomLeft; }
        }

        ///<summary>
        ///top right point of the inverted axis aligned bounding box
        ///</summary>
        public Vector2 TopRight
        {
            get { return _topRight; }
        }

        ///<summary>
        ///y-coordinate of the top left point of the inverted axis aligned
        ///bounding box
        ///</summary>
        public float Top
        {
            get { return _topLeft.Y; }
        }

        ///<summary>
        ///x-coordinate of the top left point of the inverted axis aligned
        ///bounding box
        ///</summary>
        public float Left
        {
            get { return _topLeft.X; }
        }

        ///<summary>
        ///y-coordinate of the bottom right point of the inverted axis aligned
        ///bounding box
        ///</summary>
        public float Bottom
        {
            get { return _bottomRight.Y; }
        }

        ///<summary>
        ///x-coordinate of the bottom right point of the inverted axis aligned
        ///bounding box
        ///</summary>
        public float Right
        {
            get { return _bottomRight.X; }
        }

        ///<summary>
        ///center point of the inverted axis aligned bounding box
        ///</summary>
        public Vector2 Center
        {
            get { return _center; }
        }

        #endregion

        //======================================================================
        #region Public methods

        //returns 
        ///<summary>
        ///Tests if the bounding box described by other intersects with this one
        ///</summary>
        ///<param name="other"></param>
        ///<returns>
        ///true if the bounding box described by other intersects with this one
        ///</returns>
        public bool IsOverlappedWith(InvertedAABBox2D other)
        {
            return !((other.Top > Bottom) ||
                   (other.Bottom < Top) ||
                   (other.Left > Right) ||
                   (other.Right < Left));
        }

        ///<summary>
        ///Render the inverted axis aligned bounding box
        ///</summary>
        ///<param name="renderCenter">if true, render the center</param>
        public void Render(bool renderCenter)
        {
            DrawUtil.Line(TopLeft, TopRight, Color.Black);
            DrawUtil.Line(BottomLeft, BottomRight, Color.Black);
            DrawUtil.Line(TopLeft, BottomLeft, Color.Black);
            DrawUtil.Line(TopRight, BottomRight, Color.Black);

            if (!renderCenter)
                return;

            DrawUtil.CircleFill(Center, 5, Color.White, 20);
            DrawUtil.Circle(Center, 5, Color.Black, 20);
        }

        ///<summary>
        ///Render the inverted axis aligned bounding box
        ///</summary>
        public void Render()
        {
            Render(false);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        readonly Vector2 _topLeft;
        readonly Vector2 _bottomRight;
        readonly Vector2 _topRight;
        readonly Vector2 _bottomLeft;
        readonly Vector2 _center;

        #endregion
    }

    #endregion
}
