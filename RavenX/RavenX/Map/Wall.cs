#region File description

//------------------------------------------------------------------------------
//Wall.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.RavenX.Entity.Items;

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
    ///class to create and render 2D walls. Defined as the two 
    ///vectors A - B with a perpendicular normal. 
    ///</summary>
    public class Wall : Entity.Entity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public Wall()
            : this(Vector2.Zero, Vector2.One)
        {
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        public Wall(Vector2 from, Vector2 to)
            : base(new WallSceneObject())
        {
            //TODO: this is a mess. cleanup/refactor
            _from = from;
            _to = to;
            CalculateNormal();

            if ((to.X - from.X < 0) ||
                (to.X - from.X == 0 && to.Y - from.Y < 0))
            {
                from = To;
                to = From;
            }

            Vector2 dir = to - from;
            float length = dir.Length();
            dir.Normalize();
            Vector2 size = Vector2.One;

            if (dir.X > 0)
            {
                size.X *= length;
            }
            if (dir.Y > 0)
            {
                size.Y *= length;
            }

            SceneObject.Size = size;
            Position = from + (to - from)/2;

            Name = "Wall_" + _from + "_" + _to + _normal;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="normal"></param>
        public Wall(Vector2 from, Vector2 to, Vector2 normal)
            : base(new WallSceneObject())
        {
            //TODO: this is a mess. cleanup/refactor
            _from = from;
            _to = to;
            _normal = normal;

            if ((to.X - from.X < 0) ||
                (to.X - from.X == 0 && to.Y - from.Y < 0))
            {
                from = To;
                to = From;
            }

            Vector2 dir = to - from;
            float length = dir.Length();
            dir.Normalize();
            Vector2 size = Vector2.One;

            if (dir.X > 0)
            {
                size.X *= length;
            }
            if (dir.Y > 0)
            {
                size.Y *= length;
            }

            SceneObject.Size = size;
            Position = from + (to - from)/2;

            Name = "Wall_" + _from + "_" + _to + _normal;
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="wallData"></param>
        public Wall(WallData wallData)
            : base(new WallSceneObject())
        {
            //TODO: this is a mess. cleanup/refactor
            _from = wallData.From;
            _to = wallData.To;
            _normal = wallData.Normal;

            Assert.Fatal(From != To, "Wall.Wall: zero length wall!");

            Vector2 from = From;
            Vector2 to = To;
            if ((to.X - from.X < 0) ||
                (to.X - from.X == 0 && to.Y - from.Y < 0))
            {
                from = To;
                to = From;
            }

            Vector2 dir = to - from;
            float length = dir.Length();
            dir.Normalize();
            Vector2 size = Vector2.One;

            if (from.X != to.X && from.Y != to.Y)
            {
                float theta =
                    (float) Math.Acos(Vector2.Dot(dir, -Vector2.UnitX));
                SceneObject.Rotation = MathHelper.ToDegrees(theta);
            }

            if (dir.X > 0)
            {
                size.X *= length;
            }
            if (dir.Y > 0)
            {
                size.Y *= length;
            }

            SceneObject.Size = size;
            Position = from + (to - from)/2;

            Name = wallData.Name;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Wall from position
        ///</summary>
        public Vector2 From
        {
            get { return _from; }
            set
            {
                _from = value;
                CalculateNormal();
                AdjustWall();
            }
        }

        ///<summary>
        ///wall to position
        ///</summary>
        public Vector2 To
        {
            get { return _to; }
            set
            {
                _to = value;
                CalculateNormal();
                AdjustWall();
            }
        }

        ///<summary>
        ///wall normal
        ///</summary>
        public Vector2 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }

        ///<summary>
        ///wall center
        ///</summary>
        public Vector2 Center
        {
            get { return (_from + _to)/2.0f; }
        }

        #region Public methods

        ///<summary>
        ///Render wall (optionally show wall normals)
        ///</summary>
        ///<param name="renderNormals"></param>
        public virtual void Render(bool renderNormals)
        {
            //render the normals if rqd
            if (!renderNormals)
                return;

            int midX = (int) ((_from.X + _to.X)/2);
            int midY = (int) ((_from.Y + _to.Y)/2);

            DrawUtil.Line(
                new Vector2(midX, midY),
                new Vector2(midX + (_normal.X*5), midY + (_normal.Y*5)),
                Color.Black, 2);
        }

        ///<summary>
        ///Render wall (don't show wall normals)
        ///</summary>
        public override void Render()
        {
            Render(false);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///calculate wall normal
        ///</summary>
        protected void CalculateNormal()
        {
            Vector2 temp = Vector2.Normalize(_to - _from);

            _normal.X = -temp.Y;
            _normal.Y = temp.X;
        }

        ///<summary>
        ///Determine the position, size, and rotation of the associated
        ///scene object
        ///</summary>
        protected void AdjustWall()
        {
            //TODO: this is a mess. cleanup/refactor
            Vector2 from = From;
            Vector2 to = To;
            if ((to.X - from.X < 0) ||
                (to.X - from.X == 0 && to.Y - from.Y < 0))
            {
                from = To;
                to = From;
            }

            Vector2 dir = to - from;
            float length = dir.Length();
            dir.Normalize();
            Vector2 size = Vector2.One;

            if (from.X != to.X && from.Y != to.Y)
            {
                float theta = (float) Math.Acos(Vector2.Dot(dir, -Vector2.UnitX));
                SceneObject.Rotation = MathHelper.ToDegrees(theta);
            }

            if (dir.X > 0)
            {
                size.X *= length;
            }
            if (dir.Y > 0)
            {
                size.Y *= length;
            }

            if (size == Vector2.Zero)
            {
            }
            SceneObject.Size = size;
            Position = from + (to - from)/2;
        }

        #endregion

        #region Private, protected, internal fields

        private Vector2 _from;
        private Vector2 _normal;
        private Vector2 _to;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}