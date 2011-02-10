#region File description

//------------------------------------------------------------------------------
//TriggerRegionCircle.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///class to define a circular region of influence
    ///</summary>
    public class TriggerRegionCircle : TriggerRegion
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="center"></param>
        ///<param name="radius"></param>
        public TriggerRegionCircle(Vector2 center, float radius)
        {
            _radius = radius;
            _center = center;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the center of the region
        ///</summary>
        public Vector2 Center
        {
            get { return _center; }
        }

        ///<summary>
        ///the radius of the region
        ///</summary>
        public float Radius
        {
            get { return _radius; }
        }

        #region Public methods

        ///<summary>
        ///Tests if an entity of the given size and position is intersecting the
        ///trigger region.
        ///</summary>
        ///<param name="entityCenter"></param>
        ///<param name="entityRadius"></param>
        ///<returns>
        ///true if an entity of the given size and position is intersecting the
        ///trigger region.
        ///</returns>
        public override bool IsTouching(
            Vector2 entityCenter,
            float entityRadius)
        {
            return (Center - entityCenter).LengthSquared() <
                   (entityRadius + Radius)*(entityRadius + Radius);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly Vector2 _center;
        private readonly float _radius;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}