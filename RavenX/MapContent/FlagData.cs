#region File description

//------------------------------------------------------------------------------
//FlagData.cs
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
    ///class for health data
    ///</summary>
    public class FlagData
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        public FlagData(string name, Vector2 position, float radius, int team)
        {
            Name = name;
            Position = position;
            Radius = radius;
            Team = team;
        }

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
        ///health trigger position
        ///</summary>
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        ///<summary>
        ///trigger activation radius
        ///</summary>
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        ///<summary>
        ///team this belongs to
        ///</summary>
        public int Team
        {
            get { return _team; }
            set { _team = value; }
        }

        ///<summary>
        ///graph node used to find this trigger
        ///</summary>
        public int NodeIndex
        {
            get { return _nodeIndex; }
            set { _nodeIndex = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private int _team;
        private string _name;
        private int _nodeIndex;
        private Vector2 _position;
        private float _radius;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}