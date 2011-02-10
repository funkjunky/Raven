#region File description

//------------------------------------------------------------------------------
//DoorTriggerData.cs
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
    ///class for door trigger data
    ///</summary>
    public class DoorTriggerData
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
        ///door trigger position
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
        ///message to send when activated
        ///</summary>
        public MessageTypes MessageToSend
        {
            get { return _messageToSend; }
            set { _messageToSend = value; }
        }

        ///<summary>
        ///when triggered a message is sent to the entity with this name
        ///</summary>
        public string ReceiverName
        {
            get { return _receiverName; }
            set { _receiverName = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private MessageTypes _messageToSend;
        private string _name;
        private Vector2 _position;
        private float _radius;
        private string _receiverName;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}