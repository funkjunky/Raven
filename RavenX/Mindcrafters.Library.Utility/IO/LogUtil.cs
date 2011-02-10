#region File description
//------------------------------------------------------------------------------
//LogUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System;
using System.Diagnostics;
using System.IO;

#endregion

#region Microsoft

using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    #region Class LogUtil

    ///<summary>
    ///Logging utility class with conditionally included methods
    ///</summary>
    public class LogUtil
    {
        //======================================================================
        #region Static methods, fields, constructors

        #region Singleton pattern

        ///<summary>
        ///Accessor for the LogUtil singleton instance.
        ///</summary>
        public static LogUtil Instance
        {
            get
            {
                if (null == _instance)
                    new LogUtil();

                Assert.Fatal(null != _instance,
                    "Singleton instance not set by constructor.");
                return _instance;
            }
        }
        static LogUtil _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private LogUtil()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;

            DefaultFontType = @"data\fonts\Arial8";
            DefaultTextColor = Color.Black;
            DefaultLogFacility = LogFacility.File;
            DefaultFilename = "log.txt";
            IsLogging = true;
        }

        #endregion

        ///<summary>
        ///Write text line to log if DEBUG is defined
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("DEBUG")]
        public static void WriteLine(string text)
        {
            if (!Instance.IsLogging)
                return;

            text = "[" + Time.TimeNowMs.ToString("D8") + "] " + text;
            switch (Instance.DefaultLogFacility)
            {
                case LogFacility.TorqueConsoleEcho:
                    TorqueConsole.Echo(text);
                    break;
                case LogFacility.TorqueConsoleWarn:
                    TorqueConsole.Warn(text);
                    break;
                case LogFacility.TorqueConsoleError:
                    TorqueConsole.Error(text);
                    break;
                case LogFacility.Console:
                    Console.WriteLine(text);
                    break;
                case LogFacility.File:
                    if (null == Instance._logStreamWriter)
                    {
                        Assert.Warn(!String.IsNullOrEmpty(Instance.DefaultFilename),
                            "LogUtil.WriteLine: log filename not set");
                        if (String.IsNullOrEmpty(Instance.DefaultFilename))
                            return;

                        Instance._logStreamWriter =
                            new StreamWriter(Instance.DefaultFilename);
                    }
                    Instance._logStreamWriter.WriteLine(text);
                    break;
            }
        }

        ///<summary>
        ///Write text line to log if DEBUG is defined
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("DEBUG")]
        public static void WriteLine(string text, params object[] args)
        {
            if (!Instance.IsLogging)
                return;

            text = "[" + Time.TimeNowMs.ToString("D8") + "] " + text;
            switch (Instance.DefaultLogFacility)
            {
                case LogFacility.TorqueConsoleEcho:
                    TorqueConsole.Echo(text, args);
                    break;
                case LogFacility.TorqueConsoleWarn:
                    TorqueConsole.Warn(text, args);
                    break;
                case LogFacility.TorqueConsoleError:
                    TorqueConsole.Error(text, args);
                    break;
                case LogFacility.Console:
                    Console.WriteLine(text, args);
                    break;
                case LogFacility.File:
                    if (null == Instance._logStreamWriter)
                    {
                        Assert.Warn(!String.IsNullOrEmpty(Instance.DefaultFilename),
                            "LogUtil.WriteLine: log filename not set");
                        if (String.IsNullOrEmpty(Instance.DefaultFilename))
                            return;

                        Instance._logStreamWriter =
                            new StreamWriter(Instance.DefaultFilename);
                    }
                    Instance._logStreamWriter.WriteLine(text, args);
                    break;
            }
        }

        ///<summary>
        ///Write text line to log if DEBUG and LOGGING defined
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOGGING")]
        public static void WriteLineIfLogging(string text)
        {
            WriteLine(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG and LOGGING defined
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOGGING")]
        public static void WriteLineIfLogging(string text, params object[] args)
        {
            WriteLine(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and LOG_VERBOSE
        ///are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_VERBOSE")]
        public static void WriteLineIfVerbose(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and LOG_VERBOSE
        ///are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_VERBOSE")]
        public static void WriteLineIfVerbose(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_CREATE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_CREATE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogCreate(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_CREATE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_CREATE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogCreate(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_THINK || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_THINK"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogThink(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_THINK || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_THINK"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogThink(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_EXPLORE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_EXPLORE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogExplore(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_EXPLORE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_EXPLORE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogExplore(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_BOT_STATE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_BOT_STATE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogBotState(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_BOT_STATE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_BOT_STATE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogBotState(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_FOLLOW_PATH || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_FOLLOW_PATH"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogFollowPath(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_FOLLOW_PATH || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_FOLLOW_PATH"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogFollowPath(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_GET_ITEM || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_GET_ITEM"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogGetItem(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_GET_ITEM || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_GET_ITEM"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogGetItem(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NEGOTIATE_DOOR || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_NEGOTIATE_DOOR"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNegotiateDoor(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NEGOTIATE_DOOR || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_NEGOTIATE_DOOR"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNegotiateDoor(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_MOVE_TO_POSITION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_MOVE_TO_POSITION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogMoveToPosition(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_MOVE_TO_POSITION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_MOVE_TO_POSITION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogMoveToPosition(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_HUNT_TARGET || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_HUNT_TARGET"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogHuntTarget(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_HUNT_TARGET || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_HUNT_TARGET"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogHuntTarget(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_ATTACK_TARGET || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_ATTACK_TARGET"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogAttackTarget(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_ATTACK_TARGET || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_ATTACK_TARGET"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogAttackTarget(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_WANDER || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_WANDER"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogWander(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_LOG_WANDER || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_WANDER"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogWander(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_TRAVERSE_EDGE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_TRAVERSE_EDGE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogTraverseEdge(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_TRAVERSE_EDGE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_TRAVERSE_EDGE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogTraverseEdge(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_SEEK_TO_POSITION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_SEEK_TO_POSITION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogSeekToPosition(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_SEEK_TO_POSITION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_SEEK_TO_POSITION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogSeekToPosition(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_DODGE_SIDE_TO_SIDE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_DODGE_SIDE_TO_SIDE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogDodgeSideToSide(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_DODGE_SIDE_TO_SIDE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_DODGE_SIDE_TO_SIDE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogDodgeSideToSide(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_ADJUST_RANGE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_ADJUST_RANGE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogAdjustRange(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_ADJUST_RANGE || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_ADJUST_RANGE"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogAdjustRange(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_MESSAGING || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_MESSAGING"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogMessaging(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_MESSAGING || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_MESSAGING"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogMessaging(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NAVIGATION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_NAVIGATION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNavigation(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NAVIGATION || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_NAVIGATION"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNavigation(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NAVIGATIONAL_MEMORY || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        [Conditional("LOG_NAVIGATIONAL_MEMORY"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNavigationalMemory(string text)
        {
            WriteLineIfLogging(text);
        }

        ///<summary>
        ///Write text line to log if DEBUG, LOGGING and 
        ///(LOG_NAVIGATIONAL_MEMORY || LOG_VERBOSE) are defined.
        ///</summary>
        ///<param name="text">text to write</param>
        ///<param name="args">arguments</param>
        [Conditional("LOG_NAVIGATIONAL_MEMORY"), Conditional("LOG_VERBOSE")]
        public static void WriteLineIfLogNavigationalMemory(string text, params object[] args)
        {
            WriteLineIfLogging(text, args);
        }

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///Where to send log text
        ///</summary>
        public enum LogFacility
        {
            TorqueConsoleEcho,
            TorqueConsoleWarn,
            TorqueConsoleError,
            Console,
            File
        };

        ///<summary>
        ///default filename (relative path) for log file
        ///</summary>
        public string DefaultFilename
        {
            get { return _defaultFilename; }
            set { _defaultFilename = value; }
        }

        ///<summary>
        ///default font for log (where applicable)
        ///<remarks>not used (yet)</remarks>
        ///</summary>
        public Resource<SpriteFont> DefaultFont
        {
            get { return _defaultFont; }
        }

        ///<summary>
        ///default font type (e.g., "Arial12") for log (where applicable)
        ///<remarks>not used (yet)</remarks>
        ///</summary>
        public string DefaultFontType
        {
            get { return _defaultFontType; }
            set
            {
                _defaultFontType = value;
                _defaultFont =
                    ResourceManager.Instance.LoadFont(_defaultFontType);
            }
        }

        ///<summary>
        ///default destination for log text
        ///</summary>
        public LogFacility DefaultLogFacility
        {
            get { return _defaultLogFacility; }
            set { _defaultLogFacility = value; }
        }

        ///<summary>
        ///default text color for log text (where applicable)
        ///<remarks>not used (yet)</remarks>
        ///</summary>
        public Color DefaultTextColor
        {
            get { return _defaultTextColor; }
            set { _defaultTextColor = value; }
        }

        ///<summary>
        ///If false, don't output log text
        ///</summary>
        public bool IsLogging
        {
            get { return _isLogging; }
            set { _isLogging = value; }
        }

        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        string _defaultFilename;
        string _defaultFontType;
        Resource<SpriteFont> _defaultFont;
        LogFacility _defaultLogFacility;
        Color _defaultTextColor;
        bool _isLogging;
        StreamWriter _logStreamWriter;

        #endregion
    }

    #endregion
}

