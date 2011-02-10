#region File description

//------------------------------------------------------------------------------
//IBotSceneObject.cs
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

#endregion

#region Mindcrafters

#endregion

#endregion

using Mindcrafters.RavenX.Armory;

namespace Mindcrafters.RavenX.Entity.Bot
{
    ///<summary>
    ///Summary description for IBotSceneObject
    ///</summary>
    public interface IBotSceneObject : IEntitySceneObject
    {
        #region Public properties

        /////<summary>
        /////associated entity
        /////</summary>
        //Entity Entity
        //{
        //   get;
        //}

        /////<summary>
        /////scene object
        /////</summary>
        //T2DSceneObject SceneObject
        //{
        //   get;
        //}

        #endregion

        #region Public methods

        /////<summary>
        /////initialize using associated entity
        /////</summary>
        /////<param name="entity">associated entity</param>
        //void Initialize(Entity entity);

        ///<summary>
        ///deploy/carry/mount/hold weapon
        ///</summary>
        ///<param name="weapon">weapon to hold</param>
        void HoldWeapon(Weapon weapon);

        #endregion

        //======================================================================

        //======================================================================
    }
}