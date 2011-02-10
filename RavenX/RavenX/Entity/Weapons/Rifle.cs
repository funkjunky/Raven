#region File description

//------------------------------------------------------------------------------
//Rifle.cs
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

using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using Microsoft.Xna.Framework;

#endregion

#region Mindcrafters

#endregion

#region other

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Weapons
{
    ///<summary>
    ///class for Rifle
    ///</summary>
    public class Rifle : T2DShape3D
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public Rifle()
        {
            SetShape(@"data\shapes\weapons\rifle\prifle.dts");

            //set the size of the model in the scene    
            ShapeScale = new Vector3(16);

            AddThread("main", "fire", false);

            TorqueObjectDatabase.Instance.Register(this);
        }

        #endregion

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal field

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}