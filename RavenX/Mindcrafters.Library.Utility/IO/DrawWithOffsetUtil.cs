#region File description
//------------------------------------------------------------------------------
//DrawWithOffsetUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft

using GarageGames.Torque.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.MathUtil;
using GarageGames.Torque.GUI;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Tx2D.GameAI
{
    #region Class DrawWithOffsetUtil

    ///<summary>
    ///class for drawing lines and shapes (with an offset)
    ///</summary>
    public class DrawWithOffsetUtil
    {
        //======================================================================
        #region Static methods, fields, constructors

        #region Singleton pattern

        ///<summary>
        ///Accessor for the DrawWithOffsetUtil singleton instance.
        ///</summary>
        public static DrawWithOffsetUtil Instance
        {
            get
            {
                if (null == _instance)
                    new DrawWithOffsetUtil();

                Assert.Fatal(null != _instance,
                    "Singleton instance not set by constructor.");
                return _instance;
            }
        }
        static DrawWithOffsetUtil _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private DrawWithOffsetUtil()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;
            _defaultOffset = Vector2.Zero;
        }

        #endregion

        #region Draw

        ///<summary>
        ///Draws an untextured filled rectangle from the upper left, 
        ///<paramref name="a"/>, of the rectangle to the lower right, 
        ///<paramref name="b"/> in the specified <paramref name="color"/>.
        ///</summary>
        public static void RectFill(Vector2 a, Vector2 b, Color color)
        {
            DrawUtil.RectFill(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color);
        }

        ///<summary>
        ///Draws an untextured filled rectangle from <paramref name="rect"/> 
        ///in the specified <paramref name="color"/>.
        ///</summary>
        public static void RectFill(RectangleF rect, Color color)
        {
            DrawUtil.RectFill(
                new RectangleF(rect.Point + Instance._defaultOffset, rect.Extent), 
                color);
        }

        ///<summary>
        ///Draws an untextured wireframe rectangle from the upper left point, 
        ///<paramref name="a"/>, of the rectangle to the lower right point, 
        ///<paramref name="b"/> in the specified <paramref name="color"/>.
        ///</summary>
        public static void Rect(Vector2 a, Vector2 b, Color color)
        {
            DrawUtil.Rect(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color);
        }

        ///<summary>
        ///Draws an untextured wireframe rectangle from 
        ///<paramref name="rect"/> in the specified <paramref name="color"/>.
        ///</summary>
        public static void Rect(RectangleF rect, Color color)
        {
            DrawUtil.Rect(
                new RectangleF(rect.Point + Instance._defaultOffset, rect.Extent), 
                color);
        }

        ///<summary>
        ///Draws a line from point <paramref name="a"/> to 
        ///point <paramref name="b"/> in the specified <paramref name="color"/>.
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="color"></param>
        public static void Line(Vector2 a, Vector2 b, Color color)
        {
            DrawUtil.Line(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color);
        }

        ///<summary>
        ///Draws a line from point <paramref name="a"/> to 
        ///point <paramref name="b"/> in the specified <paramref name="color"/>
        ///and with the specified <paramref name="thickness"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="color"></param>
        ///<param name="thickness">line thickness</param>
        public static void Line(
            Vector2 a, 
            Vector2 b, 
            Color color, 
            float thickness)
        {
            DrawUtil.Line(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color, thickness);
        }

        ///<summary>
        ///Draws a line from point <paramref name="a"/> to 
        ///point <paramref name="b"/> in the specified <paramref name="color"/>
        ///terminating with an arrowhead.
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="color"></param>
        public static void LineWithArrow(Vector2 a, Vector2 b, Color color)
        {
            DrawUtil.LineWithArrow(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color);
        }

        ///<summary>
        ///Draws a line from point <paramref name="a"/> to 
        ///point <paramref name="b"/> in the specified <paramref name="color"/>
        ///and with the specified <paramref name="thickness"/>
        ///terminating with an arrowhead.
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="color"></param>
        ///<param name="thickness">line thickness</param>
        public static void LineWithArrow(
            Vector2 a, 
            Vector2 b, 
            Color color, 
            float thickness)
        {
            DrawUtil.LineWithArrow(
                a + Instance._defaultOffset, 
                b + Instance._defaultOffset, 
                color, 
                thickness);
        }

        ///<summary>
        ///Draws a circle with <paramref name="center"/>
        ///and <paramref name="radius"/> in the specified <paramref name="color"/>
        ///using <paramref name="sides"/> line segments
        ///</summary>
        ///<param name="center"></param>
        ///<param name="radius"></param>
        ///<param name="color"></param>
        ///<param name="sides"></param>
        public static void Circle(
            Vector2 center, 
            float radius, 
            Color color, 
            int sides)
        {
            DrawUtil.Circle(
                center + Instance._defaultOffset, 
                radius, 
                color, 
                sides);
        }

        ///<summary>
        ///Draws a circle with <paramref name="center"/>
        ///and <paramref name="radius"/> in the specified <paramref name="color"/>
        ///using <paramref name="sides"/> line segments
        ///and with the specified <paramref name="thickness"/>
        ///</summary>
        ///<param name="center"></param>
        ///<param name="radius"></param>
        ///<param name="color"></param>
        ///<param name="sides"></param>
        ///<param name="thickness">line thickness</param>
        public static void Circle(
            Vector2 center, 
            float radius, 
            Color color, 
            int sides, 
            float thickness)
        {
            DrawUtil.Circle(
                center + Instance._defaultOffset, 
                radius, 
                color, 
                sides, 
                thickness);
        }

        ///<summary>
        ///Draws a filled circle with <paramref name="center"/>
        ///and <paramref name="radius"/> in the specified <paramref name="color"/>
        ///using <paramref name="sides"/> line segments
        ///</summary>
        ///<param name="center"></param>
        ///<param name="radius"></param>
        ///<param name="color"></param>
        ///<param name="sides"></param>
        public static void CircleFill(
            Vector2 center, 
            float radius, 
            Color color, 
            int sides)
        {
            DrawUtil.CircleFill(
                center + Instance._defaultOffset, 
                radius, 
                color, 
                sides);
        }

        ///<summary>
        ///Draws an ellipse starting from 0,0 with the given width and height.
        ///Vertices are generated using the parametric equation of an ellipse.
        ///</summary>
        ///<param name="position"></param>
        ///<param name="semimajorAxis">
        ///The width of the ellipse at its center.
        ///</param>
        ///<param name="semiminorAxis">
        ///The height of the ellipse at its center.
        ///</param>
        ///<param name="angleOffset">
        ///The counterclockwise rotation in radians.
        ///</param>
        ///<param name="color">The color</param>
        ///<param name="sides">
        ///The number of sides on the ellipse
        ///(a higher value yields more resolution).
        ///</param>
        public static void Ellipse(
            Vector2 position, 
            float semimajorAxis, 
            float semiminorAxis, 
            float angleOffset, 
            Color color, 
            int sides)
        {
            DrawUtil.Ellipse(
                position + Instance._defaultOffset, 
                semimajorAxis, 
                semiminorAxis, 
                angleOffset, 
                color, 
                sides);
        }

        ///<summary>
        ///Draws an ellipse starting from 0,0 with the given width and height.
        ///Vertices are generated using the parametric equation of an ellipse.
        ///Drawn with the given <paramref name="thickness"/>
        ///</summary>
        ///<param name="position"></param>
        ///<param name="semimajorAxis">
        ///The width of the ellipse at its center.
        ///</param>
        ///<param name="semiminorAxis">
        ///The height of the ellipse at its center.
        ///</param>
        ///<param name="angleOffset">
        ///The counterclockwise rotation in radians.
        ///</param>
        ///<param name="color">The color</param>
        ///<param name="sides">
        ///The number of sides on the ellipse
        ///(a higher value yields more resolution).
        ///</param>
        ///<param name="thickness">line thickness</param>
        public static void Ellipse(
            Vector2 position, 
            float semimajorAxis, 
            float semiminorAxis, 
            float angleOffset, 
            Color color, 
            int sides, 
            float thickness)
        {
            DrawUtil.Ellipse(
                position + Instance._defaultOffset, 
                semimajorAxis, semiminorAxis, 
                angleOffset, 
                color, 
                sides, 
                thickness);
        }

        ///<summary>
        ///Draws a filled ellipse starting from 0,0 with the given width and
        ///height. Vertices are generated using the parametric equation of an
        ///ellipse.
        ///</summary>
        ///<param name="position"></param>
        ///<param name="semimajorAxis">
        ///The width of the ellipse at its center.
        ///</param>
        ///<param name="semiminorAxis">
        ///The height of the ellipse at its center.
        ///</param>
        ///<param name="angleOffset">
        ///The counterclockwise rotation in radians.
        ///</param>
        ///<param name="color">The color</param>
        ///<param name="sides">
        ///The number of sides on the ellipse
        ///(a higher value yields more resolution).
        ///</param>
        public static void EllipseFill(
            Vector2 position, 
            float semimajorAxis, 
            float semiminorAxis, 
            float angleOffset, 
            Color color, 
            int sides)
        {
            DrawUtil.EllipseFill(
                position + Instance._defaultOffset, 
                semimajorAxis, 
                semiminorAxis, 
                angleOffset, 
                color, 
                sides);
        }

        ///<summary>
        ///Draws a polygon from a list of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        public static void Poly(List<Vector2> vertices, Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.Poly(newVertices, color);
        }

        ///<summary>
        ///Draws a polygon from a list of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        public static void Poly(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.Poly(newVertices, position, color);
        }

        ///<summary>
        ///Draws a polygon from a list of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/> at the
        ///with the given <paramref name="thickness"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        ///<param name="thickness"></param>
        public static void Poly(
            List<Vector2> vertices, 
            Color color, 
            float thickness)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.Poly(newVertices, color, thickness);
        }

        ///<summary>
        ///Draws a polygon from a list of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///with the given <paramref name="thickness"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        ///<param name="thickness"></param>
        public static void Poly(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color, 
            float thickness)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.Poly(newVertices, position, color, thickness);
        }

        ///<summary>
        ///Draws a filled polygon from a list of <paramref name="vertices"/>
        ///in the specified <paramref name="color"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        public static void PolyFill(List<Vector2> vertices, Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.PolyFill(newVertices, color);
        }

        ///<summary>
        ///Draws a filled polygon from a list of <paramref name="vertices"/>
        ///in the specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        public static void PolyFill(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.PolyFill(newVertices, position, color);
        }

        ///<summary>
        ///Draws a sequence of line segments (line strip) from a list
        ///of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        public static void LineStrip(List<Vector2> vertices, Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.LineStrip(newVertices, color);
        }

        ///<summary>
        ///Draws a sequence of line segments (line strip) from a list
        ///of <paramref name="vertices"/> in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        public static void LineStrip(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.LineStrip(newVertices, position, color);
        }

        ///<summary>
        ///Draws a sequence of line segments (line strip) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/>
        ///with the given <paramref name="thickness"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        ///<param name="thickness"></param>
        public static void LineStrip(
            List<Vector2> vertices, 
            Color color, 
            float thickness)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.LineStrip(newVertices, color, thickness);
        }

        ///<summary>
        ///Draws a sequence of line segments (line strip) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///with the given <paramref name="thickness"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        ///<param name="thickness"></param>
        public static void LineStrip(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color, 
            float thickness)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.LineStrip(newVertices, position, color, thickness);
        }

        ///<summary>
        ///Draws a sequence of filled triangles (triangle strip) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        public static void TriangleStripFill(
            List<Vector2> vertices, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.TriangleStripFill(newVertices, color);
        }

        ///<summary>
        ///Draws a sequence of filled triangles (triangle strip) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        public static void TriangleStripFill(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.TriangleStripFill(newVertices, position, color);
        }

        ///<summary>
        ///Draws a sequence of filled triangles (triangle fan) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="color"></param>
        public static void TriangleFanFill(List<Vector2> vertices, Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.TriangleFanFill(newVertices, color);
        }

        ///<summary>
        ///Draws a sequence of filled triangles (triangle fan) from a list
        ///of <paramref name="vertices"/>in the
        ///specified <paramref name="color"/> at the
        ///given <paramref name="position"/>
        ///</summary>
        ///<param name="vertices"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        public static void TriangleFanFill(
            List<Vector2> vertices, 
            Vector2 position, 
            Color color)
        {
            List<Vector2> newVertices = new List<Vector2>(vertices.Count);
            foreach (Vector2 v in vertices)
                newVertices.Add(v + Instance._defaultOffset);
            DrawUtil.TriangleFanFill(newVertices, position, color);
        }

        ///<summary>
        ///Draws a stretched sub-region of a texture.
        ///</summary>
        ///<param name="material">
        ///The material used when rendering. 
        ///The material must implement <see cref="ITextureMaterial"/>.
        ///</param>
        ///<param name="dstRect">
        ///Rectangle where the texture object will be drawn.
        ///</param>
        ///<param name="srcRect">
        ///Sub-region of the texture that will be applied over the
        ///<paramref name="dstRect"/>.
        ///</param>
        ///<param name="flipMode">
        ///Any flipping to be done of the source texture.
        ///</param>
        public static void BitmapStretchSR(
          RenderMaterial material,
          RectangleF dstRect,
          RectangleF srcRect,
          BitmapFlip flipMode)
        {
            dstRect.Point += Instance._defaultOffset;
            DrawUtil.BitmapStretchSR(material, dstRect, srcRect, flipMode);
        }

        ///<summary>
        ///Draws an unstretched bitmap.
        ///</summary>
        ///<param name="material">
        ///The material used when rendering. 
        ///The material must implement <see cref="ITextureMaterial"/>.
        ///</param>
        ///<param name="position">
        ///Where to draw the texture in 2d coordinates.
        ///</param>
        ///<param name="flipMode">
        ///Any flipping to be done of the source texture.
        ///</param>
        public static void Bitmap(
          RenderMaterial material,
          Vector2 position,
          BitmapFlip flipMode)
        {
            DrawUtil.Bitmap(
                material, 
                position + Instance._defaultOffset, 
                flipMode);
        }

        ///<summary>
        ///Draws a stretched bitmap.
        ///</summary>
        ///<param name="material">
        ///The material used when rendering. 
        ///The material must implement <see cref="ITextureMaterial"/>.
        ///</param>
        ///<param name="dstRect">
        ///Rectangle where the texture object will be drawn.
        ///</param>
        ///<param name="flipMode">
        ///Any flipping to be done of the source texture.
        ///</param>
        public static void BitmapStretch(
          RenderMaterial material,
          RectangleF dstRect,
          BitmapFlip flipMode)
        {
            dstRect.Point += Instance._defaultOffset;
            DrawUtil.BitmapStretch(material, dstRect, flipMode);
        }

        ///<summary>
        ///Draws an unstretched sub-region of a texture.
        ///</summary>
        ///<param name="material">
        ///The material used when rendering. 
        ///The material must implement <see cref="ITextureMaterial"/>.
        ///</param>
        ///<param name="position">
        ///Where to draw the texture in 2d coordinates.
        ///</param>
        ///<param name="srcRect">
        ///Sub-region of the texture to be drawn.
        ///</param>
        ///<param name="flipMode">
        ///Any flipping to be done of the source texture.
        ///</param>
        public static void BitmapSR(
          RenderMaterial material,
          Vector2 position,
          RectangleF srcRect,
          BitmapFlip flipMode)
        {
            DrawUtil.BitmapSR(
                material, 
                position + Instance._defaultOffset, 
                srcRect, flipMode);
        }

        ///<summary>
        ///Draws text at a location in the 2d gui coordinates.
        ///</summary>
        ///<param name="font">
        ///The font to draw with, usually specified by a GUIStyle.
        ///</param>
        ///<param name="offset">
        ///Point where to start drawing the text.
        ///</param>
        ///<param name="size">
        ///Margins used to calculate the text alignment.
        ///</param>
        ///<param name="horizontalAlignment">
        ///Horizontal justification of the text relative to the offset.
        ///</param>
        ///<param name="color">The color to draw the text as.</param>
        ///<param name="text">The string to be drawn.</param>
        public static void JustifiedText(
          Resource<SpriteFont> font,
          Vector2 offset,
          Vector2 size,
          HorizontalTextAlignment horizontalAlignment,
          Color color,
          string text)
        {
            DrawUtil.JustifiedText(
                font,
                offset,
                size,
                horizontalAlignment,
                color,
                text);
        }

        ///<summary>
        ///Draws text at a location in the 2d gui coordinates.
        ///</summary>
        ///<param name="font">
        ///The font to draw with, usually specified by a GUIStyle.
        ///</param>
        ///<param name="offset">
        ///Point where to start drawing the text.
        ///</param>
        ///<param name="size">
        ///Margins used to calculate the text alignment.
        ///</param>
        ///<param name="horizontalAlignment">
        ///Horizontal justification of the text relative to the offset.
        ///</param>
        ///<param name="horizontalPadding">left/right padding</param>
        ///<param name="verticalAlignment">
        ///Vertical justification of the text relative to the offset.
        ///</param>
        ///<param name="verticalPadding">top/bottom padding</param>
        ///<param name="color">The color to draw the text as.</param>
        ///<param name="text">The string to be drawn.</param>
        public static void JustifiedText(
          Resource<SpriteFont> font,
          Vector2 offset,
          Vector2 size,
          HorizontalTextAlignment horizontalAlignment,
          float horizontalPadding,
          VerticalTextAlignment verticalAlignment,
          float verticalPadding,
          Color color,
          string text)
        {
            DrawUtil.JustifiedText(
                font,
                offset + Instance._defaultOffset,
                size,
                horizontalAlignment,
                0,
                VerticalTextAlignment.Center,
                0,
                color,
                text);
        }

        #endregion

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///Default offset
        ///</summary>
        public Vector2 DefaultOffset
        {
            get { return _defaultOffset; }
            set { _defaultOffset = value; }
        }

        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        Vector2 _defaultOffset;

        #endregion
    }

    #endregion
}
