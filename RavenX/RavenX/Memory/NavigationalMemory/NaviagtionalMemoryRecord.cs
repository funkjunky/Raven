#region File description

//------------------------------------------------------------------------------
//NavigationalMemoryRecord.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using System;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Memory.NavigationalMemory
{
    ///<summary>
    ///class to implement long term memory of navigational info
    ///</summary>
    public class NavigationalMemoryRecord
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for the NavigationalMemoryRecord class
        ///</summary>
        public NavigationalMemoryRecord(
            BotEntity owner,
            NavigationalMemory.NavigationalMemoryKey key)
        {
            Mode = key.Mode;
            SourceNodeIndex = key.SourceNodeIndex;
            DestinationNodeIndex = key.DestinationNodeIndex;

            Vector2 position =
                owner.PathPlanner.GetNodePosition(key.DestinationNodeIndex);
            RunningAverageTimeTaken =
                Vector2.Distance(owner.Position, position)/
                owner.MaxSpeed*3.0f; //200% margin //TODO: make parameter

            //switch (key.Mode)
            //{
            //    case NavigationalMemory.TravelModes.FollowPath:       
            //        break;

            //    case NavigationalMemory.TravelModes.SeekToPosition:
            //        break;

            //    case NavigationalMemory.TravelModes.TraverseEdge:
            //        break;
            //}
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        public NavigationalMemory.TravelModes Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        public int SourceNodeIndex
        {
            get { return _sourceNodeIndex; }
            set { _sourceNodeIndex = value; }
        }

        public int DestinationNodeIndex
        {
            get { return _destinationNodeIndex; }
            set { _destinationNodeIndex = value; }
        }

        public float RunningAverageTimeTaken
        {
            get { return _runningAverageTimeTaken; }
            set { _runningAverageTimeTaken = value; }
        }

        public float CompletedMarginRunningAverage
        {
            get { return _completedMarginRunningAverage; }
            set { _completedMarginRunningAverage = value; }
        }

        public int CompletedCount
        {
            get { return _completedCount; }
            set { _completedCount = value; }
        }

        public float FailedMarginRunningAverage
        {
            get { return _failedMarginRunningAverage; }
            set { _failedMarginRunningAverage = value; }
        }

        public float FailedCount
        {
            get { return _failedCount; }
            set { _failedCount = value; }
        }

        public float FailedPerSecond
        {
            get { return _failedPerSecond; }
            set { _failedPerSecond = value; }
        }

        public float CompletedPerSecond
        {
            get { return _completedPerSecond; }
            set { _completedPerSecond = value; }
        }

        #region Public methods

        ///<summary>
        ///Create string representation of memory record
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            return String.Format(
                "S: {1,3} D: {2,3} T: {3,6:F2} CM: {4,6:F2} C: {5,5} " +
                "FM: {6,6:F2} F: {7,5} M: {0,-20} " +
                "CPS: {8,6:F2} FPS: {9,6:F2}",
                Mode,
                SourceNodeIndex,
                DestinationNodeIndex,
                RunningAverageTimeTaken,
                CompletedMarginRunningAverage,
                CompletedCount,
                FailedMarginRunningAverage,
                FailedCount,
                CompletedPerSecond,
                FailedPerSecond);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private int _completedCount;
        private float _completedMarginRunningAverage;
        private float _completedPerSecond;
        private int _destinationNodeIndex;
        private float _failedCount;
        private float _failedMarginRunningAverage;
        private float _failedPerSecond;
        private NavigationalMemory.TravelModes _mode;
        private float _runningAverageTimeTaken;
        private int _sourceNodeIndex;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}