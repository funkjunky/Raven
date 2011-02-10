#region File description
//------------------------------------------------------------------------------
//TextUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.GFX;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    #region Class TextWithOffsetUtil

    ///<summary>
    ///class for drawing text (with an offset)
    ///</summary>
    public class TextWithOffsetUtil
    {
        //======================================================================
        #region Static methods, fields, constructors

        #region Singleton pattern

        ///<summary>
        ///Accessor for the TextWithOffsetUtil singleton instance.
        ///</summary>
        public static TextWithOffsetUtil Instance
        {
            get
            {
                if (null == _instance)
                    new TextWithOffsetUtil();

                Assert.Fatal(null != _instance,
                    "Singleton instance not set by constructor.");
                return _instance;
            }
        }
        static TextWithOffsetUtil _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private TextWithOffsetUtil()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;

            DefaultFontType = @"data\fonts\Arial8";
            DefaultTextColor = Color.Black;
        }

        #endregion

        #region DrawText

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="font"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(
            Resource<SpriteFont> font, 
            Vector2 position, 
            Color color, 
            string text)
        {
            FontRenderer.Instance.DrawString(
                font, 
                position + Instance._defaultOffset, 
                color, 
                text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="font"></param>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(
            Resource<SpriteFont> font, 
            float x, 
            float y, 
            Color color, 
            string text)
        {
            DrawText(font, new Vector2(x, y), color, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="font"></param>
        ///<param name="position"></param>
        ///<param name="text"></param>
        static public void DrawText(
            Resource<SpriteFont> font, 
            Vector2 position, 
            string text)
        {
            DrawText(font, position, Instance._defaultTextColor, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="font"></param>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="text"></param>
        static public void DrawText(
            Resource<SpriteFont> font, 
            float x, 
            float y, 
            string text)
        {
            DrawText(font, new Vector2(x, y), Instance._defaultTextColor, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="position"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(Vector2 position, Color color, string text)
        {
            DrawText(Instance._defaultFont, position, color, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(float x, float y, Color color, string text)
        {
            DrawText(Instance._defaultFont, new Vector2(x, y), color, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="position"></param>
        ///<param name="text"></param>
        static public void DrawText(Vector2 position, string text)
        {
            DrawText(
                Instance._defaultFont, 
                position, 
                Instance._defaultTextColor, 
                text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="text"></param>
        static public void DrawText(float x, float y, string text)
        {
            DrawText(
                Instance._defaultFont, 
                new Vector2(x, y), 
                Instance._defaultTextColor, 
                text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="fontType"></param>
        ///<param name="position"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(
            string fontType, 
            Vector2 position, 
            Color color, 
            string text)
        {
            Resource<SpriteFont> font = 
                ResourceManager.Instance.LoadFont(fontType);
            DrawText(font, position, color, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="fontType"></param>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="color"></param>
        ///<param name="text"></param>
        static public void DrawText(
            string fontType, 
            float x, 
            float y, 
            Color color, 
            string text)
        {
            DrawText(fontType, new Vector2(x, y), color, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="fontType"></param>
        ///<param name="position"></param>
        ///<param name="text"></param>
        static public void DrawText(
            string fontType, 
            Vector2 position, 
            string text)
        {
            DrawText(fontType, position, Instance._defaultTextColor, text);
        }

        ///<summary>
        ///Draw text
        ///</summary>
        ///<param name="fontType"></param>
        ///<param name="x"></param>
        ///<param name="y"></param>
        ///<param name="text"></param>
        static public void DrawText(
            string fontType, 
            float x, 
            float y, 
            string text)
        {
            DrawText(
                fontType, 
                new Vector2(x, y), 
                Instance._defaultTextColor, 
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
        ///Default font type (e.g., "Arial12")
        ///</summary>
        public string DefaultFontType
        {
            get { return _defaultFontType; }
            set
            {
                _defaultFontType = value;
                _defaultFont = 
                    ResourceManager.Instance.LoadFont(_defaultFontType);
            }
        }

        ///<summary>
        ///Default font
        ///</summary>
        public Resource<SpriteFont> DefaultFont
        {
            get { return _defaultFont; }
        }

        ///<summary>
        ///Default text color
        ///</summary>
        public Color DefaultTextColor
        {
            get { return _defaultTextColor; }
            set { _defaultTextColor = value; }
        }

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

        string _defaultFontType;
        Resource<SpriteFont> _defaultFont;
        Color _defaultTextColor;
        Vector2 _defaultOffset;

        #endregion
    }

    #endregion
}
