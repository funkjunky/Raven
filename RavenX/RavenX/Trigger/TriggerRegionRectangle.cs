#region File description

//------------------------------------------------------------------------------
//TriggerRegionRectangle.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///class to define a rectangular region of influence
    ///</summary>
    public class TriggerRegionRectangle : TriggerRegion
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="topLeft"></param>
        ///<param name="bottomRight"></param>
        public TriggerRegionRectangle(Vector2 topLeft, Vector2 bottomRight)
        {
            _rectangle = new InvertedAABBox2D(topLeft, bottomRight);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Rectangular trigger region
        ///</summary>
        public InvertedAABBox2D Rectangle
        {
            get { return _rectangle; }
        }

        #region Public methods

        ///<summary>
        ///there's no need to do an accurate (and expensive) circle versus
        ///rectangle intersection test. Instead we'll just test the bounding box
        ///of the given circle with the rectangle.
        ///</summary>
        ///<param name="entityCenter"></param>
        ///<param name="entityRadius"></param>
        ///<returns></returns>
        public override bool IsTouching(Vector2 entityCenter, float entityRadius)
        {
            InvertedAABBox2D box =
                new InvertedAABBox2D(
                    new Vector2(
                        entityCenter.X - entityRadius,
                        entityCenter.Y - entityRadius),
                    new Vector2(
                        entityCenter.X + entityRadius,
                        entityCenter.Y + entityRadius));

            return box.IsOverlappedWith(Rectangle);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly InvertedAABBox2D _rectangle;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}