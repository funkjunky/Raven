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

using Microsoft.Xna.Framework.Audio;

#endregion

#region GarageGames

using GarageGames.Torque.XNA;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    #region Class Program

    ///<summary>
    ///main program class for the components library
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
        ///The game sound bank
        ///</summary>
        public static SoundBank SoundBank
        {
            get { return _soundBank; }
        }
        private static SoundBank _soundBank;

        ///<summary>
        ///Delegate for the game exit method
        ///</summary>
        public delegate void ExitDelegate();

        ///<summary>
        ///The game exit method
        ///</summary>
        public static ExitDelegate Exit
        {
            get { return _exit; }
        }
        private static ExitDelegate _exit;

        ///<summary>
        ///Delegate used for properties
        ///</summary>
        ///<typeparam name="T">property type</typeparam>
        public delegate T Getter<T>();

        ///<summary>
        ///Delegate used for properties
        ///</summary>
        ///<param name="value">property value</param>
        ///<typeparam name="T">property type</typeparam>
        public delegate void Setter<T>(T value);

        ///<summary>
        ///The game GamePaused property
        ///</summary>
        public static bool GamePaused
        {
            get { return _gamePausedGetter(); }
            set { _gamePausedSetter(value); }
        }
        private static Getter<bool> _gamePausedGetter;
        private static Setter<bool> _gamePausedSetter;

        ///<summary>
        ///Initialize the library using delegates to hook back to the
        ///application
        ///</summary>
        ///<param name="engine">Torque engine component</param>
        ///<param name="soundBank">sound bank</param>
        ///<param name="exit">game exit</param>
        ///<param name="gamePausedGetter">
        ///getter for <see cref="GamePaused"/> property
        ///</param>
        ///<param name="gamePausedSetter">
        ///setter for <see cref="GamePaused"/> property
        ///</param>
        public static void Initialize(
            TorqueEngineComponent engine, 
            SoundBank soundBank, 
            ExitDelegate exit,
            Getter<bool> gamePausedGetter,
            Setter<bool> gamePausedSetter)
        {
            _engine = engine;
            _soundBank = soundBank;
            _exit = exit;

            _gamePausedGetter = gamePausedGetter;
            _gamePausedSetter = gamePausedSetter;
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
