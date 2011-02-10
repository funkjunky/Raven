#region File description
//------------------------------------------------------------------------------
//Time.cs
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

namespace Mindcrafters.Library.Utility
{
    #region Class Time

    ///<summary>
    ///time keeper to measure time in seconds
    ///</summary>
    public class Time
    {
        ///<summary>
        ///time now in seconds
        ///</summary>
        public static float TimeNow
        {
            get { return TimeNowMs * 0.001f; }
        }

        ///<summary>
        ///time now in milliseconds
        ///</summary>
        public static int TimeNowMs
        {
            get { return (int)(Program.Engine.RealTime - _startTime); }
        }

        ///<summary>
        ///start time in milliseconds
        ///</summary>
        public static float StartTime
        {
            get { return _startTime; }
            //set { _startTime = value; }
        }

        private static readonly float _startTime = Program.Engine.RealTime;
    }

    #endregion
}
