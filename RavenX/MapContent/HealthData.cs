#region File description

//------------------------------------------------------------------------------
//HealthData.cs
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
    public class HealthData
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
        ///amount of health given upon activation
        ///</summary>
        public int HealthGiven
        {
            get { return _healthGiven; }
            set { _healthGiven = value; }
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

        private int _healthGiven;
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