#region File description

//------------------------------------------------------------------------------
//TriggerRegion.cs
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
    ///base class to define a region of influence for a trigger. A 
    ///TriggerRegion has one method, IsTouching, which returns true if
    ///an entity of the given size and position is inside the region
    ///</summary>
    public abstract class TriggerRegion
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

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
        public abstract bool IsTouching(Vector2 entityCenter, float entityRadius);

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}