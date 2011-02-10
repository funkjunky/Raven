#region File description

//------------------------------------------------------------------------------
//Region.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using GarageGames.Torque.GUI;
using GarageGames.Torque.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Tx2D.GameAI;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Map
{
    ///<summary>
    ///class for a region of the map
    ///</summary>
    public class Region
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public Region()
        {
            _top = 0;
            _bottom = 0;
            _left = 0;
            _right = 0;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="left"></param>
        ///<param name="top"></param>
        ///<param name="right"></param>
        ///<param name="bottom"></param>
        ///<param name="id"></param>
        public Region(float left, float top, float right, float bottom, int id)
        {
            _top = top;
            _bottom = bottom;
            _left = left;
            _right = right;
            _id = id;

            //calculate center of region
            _center = new Vector2((left + right)*0.5f, (top + bottom)*0.5f);

            _width = Math.Abs(right - left);
            _height = Math.Abs(bottom - top);
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="left"></param>
        ///<param name="top"></param>
        ///<param name="right"></param>
        ///<param name="bottom"></param>
        public Region(float left, float top, float right, float bottom)
            : this(left, top, right, bottom, -1)
        {
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region RegionModifier enum

        ///<summary>
        ///
        ///</summary>
        public enum RegionModifier
        {
            HalfSize,
            Normal
        } ;

        #endregion

        ///<summary>
        ///top (y-coordinate) of the region
        ///</summary>
        public float Top
        {
            get { return _top; }
            set { _top = value; }
        }

        ///<summary>
        ///bottom (y-coordinate) of the region
        ///</summary>
        public float Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }

        ///<summary>
        ///left (x-coordinate) of the region
        ///</summary>
        public float Left
        {
            get { return _left; }
            set { _left = value; }
        }

        ///<summary>
        ///right (x-coordinate) of the region
        ///</summary>
        public float Right
        {
            get { return _right; }
            set { _right = value; }
        }

        ///<summary>
        ///width of the region
        ///</summary>
        public float Width
        {
            get { return Math.Abs(_right - _left); }
            set { _width = value; }
        }

        ///<summary>
        ///height of the region
        ///</summary>
        public float Height
        {
            get { return Math.Abs(_top - _bottom); }
            set { _height = value; }
        }

        ///<summary>
        ///Length (maximum of width and height) of the region
        ///</summary>
        public float Length
        {
            get { return Math.Max(Width, Height); }
        }

        ///<summary>
        ///Breadth (minimum of width and height) of the region
        ///</summary>
        public float Breadth
        {
            get { return Math.Min(Width, Height); }
        }

        ///<summary>
        ///center of the region
        ///</summary>
        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        ///<summary>
        ///id of the region
        ///</summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        #region Public methods

        ///<summary>
        ///Gets a random position in the region
        ///</summary>
        ///<returns>a random position in the region</returns>
        public Vector2 GetRandomPosition()
        {
            return new Vector2(TorqueUtil.GetFastRandomFloat(_left, _right),
                               TorqueUtil.GetFastRandomFloat(_top, _bottom));
        }

        ///<summary>
        ///Tests if position is inside this region
        ///</summary>
        ///<param name="position"></param>
        ///<param name="regionModifier"></param>
        ///<returns>true if position is inside this region</returns>
        public bool Inside(Vector2 position, RegionModifier regionModifier)
        {
            if (regionModifier == RegionModifier.Normal)
            {
                return ((position.X > _left) && (position.X < _right) &&
                        (position.Y > _top) && (position.Y < _bottom));
            }

            float marginX = _width*0.25f;
            float marginY = _height*0.25f;

            return ((position.X > (_left + marginX)) &&
                    (position.X < (_right - marginX)) &&
                    (position.Y > (_top + marginY)) &&
                    (position.Y < (_bottom - marginY)));
        }

        ///<summary>
        ///Tests if position is inside this region
        ///</summary>
        ///<param name="position"></param>
        ///<returns>true if position is inside this region</returns>
        public bool Inside(Vector2 position)
        {
            return Inside(position, RegionModifier.Normal);
        }

        ///<summary>
        ///Render the region (optionally show id)
        ///</summary>
        ///<param name="showId">show id if true</param>
        public void Render(bool showId)
        {
            DrawUtil.Rect(
                new Vector2(_left, _top),
                new Vector2(_right, _bottom),
                Color.Green);

            if (showId)
            {
                TextUtil.DrawText(Center, Color.Green, _id.ToString());
            }
        }

        ///<summary>
        ///Render the region (don't show id)
        ///</summary>
        public void Render()
        {
            Render(false);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private float _bottom;
        private Vector2 _center;
        private float _height;
        private int _id;
        private float _left;
        private float _right;
        private float _top;
        private float _width;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}