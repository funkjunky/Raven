#region File description
//------------------------------------------------------------------------------
//Program.cs
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

using GarageGames.Torque.XNA;

namespace Mindcrafters.Library.Utility
{
    #region Class Program

    ///<summary>
    ///main program class for the utility library
    ///(useful for debugging library).
    ///Also includes initialization and delegates to hook to application.
    ///</summary>
    public static class Program
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///The game engine component
        ///</summary>
        public static TorqueEngineComponent Engine
        {
            get { return _engine; }
        }
        private static TorqueEngineComponent _engine;


        ///<summary>
        ///Initialize the library using delegates to hook back to the
        ///application
        ///</summary>
        ///<param name="engine">Torque engine component</param>
        public static void Initialize(TorqueEngineComponent engine)
        {
            _engine = engine;
        }

        ///<summary>
        ///main class
        ///</summary>
        public static void Main()
        {
        }

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }

    #endregion
}
