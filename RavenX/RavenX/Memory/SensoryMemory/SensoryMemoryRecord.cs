#region File description

//------------------------------------------------------------------------------
//SensoryMemoryRecord.cs
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

namespace Mindcrafters.RavenX.Memory.SensoryMemory
{
    ///<summary>
    ///class to implement short term memory of sensory info
    ///</summary>
    public class SensoryMemoryRecord
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for the SensoryMemoryRecord class
        ///</summary>
        public SensoryMemoryRecord()
        {
            TimeLastSensed = -999;
            TimeBecameVisible = -999;
            TimeLastVisible = 0;
            IsWithinFOV = false;
            IsShootable = false;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///records the time the opponent was last sensed (seen or heard). This
        //is used to determine if a bot can 'remember' this record or not. 
        //(if Time.TimeNow - _timeLastSensed is greater than the bot's
        //memory span, the data in this record is made unavailable to clients)
        ///</summary>
        public float TimeLastSensed
        {
            get { return _timeLastSensed; }
            set { _timeLastSensed = value; }
        }


        ///<summary>
        ///it can be useful to know how long an opponent has been visible. This 
        ///variable is tagged with the current time whenever an opponent first
        ///becomes visible. It's then a simple matter to calculate how long the
        ///opponent has been in view
        ///(Time.TimeNow - <see cref="_timeBecameVisible"/>)
        ///</summary>
        public float TimeBecameVisible
        {
            get { return _timeBecameVisible; }
            set { _timeBecameVisible = value; }
        }

        ///<summary>
        ///it can also be useful to know the last time an opponent was seen
        ///</summary>
        public float TimeLastVisible
        {
            get { return _timeLastVisible; }
            set { _timeLastVisible = value; }
        }

        ///<summary>
        ///a vector marking the position where the opponent was last sensed.
        ///This can be used to help hunt down an opponent if it goes out of view
        ///</summary>
        public Vector2 LastSensedPosition
        {
            get { return _lastSensedPosition; }
            set { _lastSensedPosition = value; }
        }

        ///<summary>
        ///set to true if opponent is within the field of view of the owner
        ///</summary>
        public bool IsWithinFOV
        {
            get { return _isWithinFOV; }
            set { _isWithinFOV = value; }
        }

        ///<summary>
        ///set to true if there is no obstruction between the opponent and the
        ///owner, permitting a shot.
        ///</summary>
        public bool IsShootable
        {
            get { return _isShootable; }
            set { _isShootable = value; }
        }

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private bool _isShootable;
        private bool _isWithinFOV;
        private Vector2 _lastSensedPosition;
        private float _timeBecameVisible;
        private float _timeLastSensed;
        private float _timeLastVisible;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}