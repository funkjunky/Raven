#region File description

//------------------------------------------------------------------------------
//WallData.cs
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

namespace MapContent
{
    ///<summary>
    ///class for wall data
    ///</summary>
    public class WallData
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Name used to find object by name (should be unique)
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///<summary>
        ///wall's from position
        ///</summary>
        public Vector2 From
        {
            get { return _from; }
            set { _from = value; }
        }

        ///<summary>
        ///wall's to position
        ///</summary>
        public Vector2 To
        {
            get { return _to; }
            set { _to = value; }
        }

        ///<summary>
        ///wall's normal vector
        ///</summary>
        public Vector2 Normal
        {
            get { return _normal; }
            set { _normal = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private Vector2 _from;
        private string _name;
        private Vector2 _normal;
        private Vector2 _to;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}