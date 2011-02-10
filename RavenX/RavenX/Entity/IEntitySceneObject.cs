#region File description

//------------------------------------------------------------------------------
//IEntitySceneObject.cs
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

using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity
{
    ///<summary>
    ///interface IEntitySceneObject
    ///</summary>
    public interface IEntitySceneObject
    {
        #region Public properties

        ///<summary>
        ///associated entity
        ///</summary>
        Entity Entity { get; }

        ///<summary>
        ///scene object
        ///</summary>
        T2DSceneObject SceneObject { get; }

        #endregion

        #region Public methods

        ///<summary>
        ///initialize using associated entity
        ///</summary>
        ///<param name="entity">associated entity</param>
        void Initialize(Entity entity);

        #endregion

        //======================================================================

        //======================================================================
    }
}