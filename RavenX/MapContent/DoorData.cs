#region File description

//------------------------------------------------------------------------------
//DoorData.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Microsoft.Xna.Framework;

    #endregion

    #region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///class for door data
    ///</summary>
    public class DoorData
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
        ///door's from position
        ///</summary>
        public Vector2 From
        {
            get { return _from; }
            set { _from = value; }
        }

        ///<summary>
        ///door's to position
        ///</summary>
        public Vector2 To
        {
            get { return _to; }
            set { _to = value; }
        }

        ///<summary>
        ///door triggers
        ///</summary>
        public List<string> TriggerList
        {
            get { return _triggerList; }
            set { _triggerList = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private Vector2 _from;
        private string _name;
        private Vector2 _to;

        private List<string> _triggerList;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}