#region File description

//------------------------------------------------------------------------------
//NavigationalMemory.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using System.Diagnostics;
using GarageGames.Torque.Core;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity.Bot;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Memory.NavigationalMemory
{
    ///<summary>
    ///Navigational memory keeps track of travel times between locations
    ///TODO: one memory per bot versus per agent type???
    ///</summary>
    public class NavigationalMemory
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for NavigationalMemory class
        ///</summary>
        ///<param name="owner">bot that owns this navigational memory</param>
        ///<param name="memorySpan">how soon we forget (negative: never)</param>
        public NavigationalMemory(BotEntity owner, float memorySpan)
        {
            _owner = owner;
            _memorySpan = memorySpan;
            _memoryMap = new Dictionary<NavigationalMemoryKey, NavigationalMemoryRecord>();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region TravelModes enum

        ///<summary>
        ///used to keep separate records for different travel modes. For example,
        ///<see cref="SeekToPosition"/> is straight-line motion and therefore
        ///faster than <see cref="FollowPath"/>
        ///</summary>
        public enum TravelModes
        {
            FollowPath,
            TraverseEdge,
            SeekToPosition
        }

        #endregion

        #region TravelStatuses enum

        ///<summary>
        ///used to keep separate statistics for completed, failed, and
        ///abandoned movements
        ///</summary>
        public enum TravelStatuses
        {
            Completed,
            Abandoned, // used when bot changes goal
            Failed
        }

        #endregion

        ///<summary>
        ///the owner of this instance
        ///</summary>
        public BotEntity Owner
        {
            get { return _owner; }
        }

        ///<summary>
        ///this container is used to simulate memory of navigational events.
        ///A <see cref="NavigationalMemoryRecord"/> is created for source to
        ///destination movement (path, edge, seek, etc). Records are updated
        ///through calls to <see cref="RecordTimeTaken"/>.
        ///</summary>
        public Dictionary<NavigationalMemoryKey, NavigationalMemoryRecord> MemoryMap
        {
            get { return _memoryMap; }
        }

        ///<summary>
        ///a bot has a memory span equivalent to this value. When a bot requests
        ///navigational memory information this value is used to determine if
        ///the bot is able to remember or not.
        ///<remarks>
        ///not currently used. Might want to drop old records (drop LRU-policy)
        ///or establish a limit on the number of records kept ...
        ///</remarks>
        ///</summary>
        public float MemorySpan
        {
            get { return _memorySpan; }
        }

        ///<summary>
        ///just a holder for a memory (to avoid multiple lookups);
        ///</summary>
        public NavigationalMemoryRecord Current
        {
            get { return _current; }
            protected set { _current = value; }
        }

        ///<summary>
        ///how often to take a dump
        ///</summary>
        public int DumpFrequency
        {
            get { return _dumpFrequency; }
            set { _dumpFrequency = value; }
        }

        #region Public methods

        ///<summary>
        ///Sets a reference to a particular memory for ease of access (to avoid
        ///multiple lookups).
        ///</summary>
        ///<param name="mode"></param>
        ///<param name="sourceNodeIndex"></param>
        ///<param name="destinationNodeIndex"></param>
        public void MakeCurrent(
            TravelModes mode,
            int sourceNodeIndex,
            int destinationNodeIndex)
        {
            NavigationalMemoryKey key =
                new NavigationalMemoryKey(
                    mode,
                    sourceNodeIndex,
                    destinationNodeIndex);

            MakeNewRecordIfNotAlreadyPresent(key);
            Current = MemoryMap[key];
        }

        ///<summary>
        ///Remember this
        ///</summary>
        ///<param name="mode"></param>
        ///<param name="status"></param>
        ///<param name="sourceNodeIndex"></param>
        ///<param name="destinationNodeIndex"></param>
        ///<param name="timeTaken"></param>
        ///<param name="timeExpected"></param>
        public void RecordTimeTaken(
            TravelModes mode,
            TravelStatuses status,
            int sourceNodeIndex,
            int destinationNodeIndex,
            float timeTaken,
            float timeExpected)
        {
            NavigationalMemoryKey key =
                new NavigationalMemoryKey(
                    mode,
                    sourceNodeIndex,
                    destinationNodeIndex);

            MakeNewRecordIfNotAlreadyPresent(key);
            NavigationalMemoryRecord record = MemoryMap[key];

            switch (status)
            {
                case TravelStatuses.Completed:
                    record.CompletedCount++;
                    record.RunningAverageTimeTaken +=
                        (timeTaken - record.RunningAverageTimeTaken)/
                        record.CompletedCount;
                    record.CompletedMarginRunningAverage +=
                        (timeExpected - timeTaken -
                         record.CompletedMarginRunningAverage)/
                        record.CompletedCount;
                    break;

                case TravelStatuses.Failed:
                    record.FailedCount++;
                    float estimatedRemainingTime;
                    Owner.CalculateTimeToReachNode(
                        destinationNodeIndex,
                        out estimatedRemainingTime);
                    record.FailedMarginRunningAverage +=
                        (estimatedRemainingTime -
                         record.FailedMarginRunningAverage)/
                        record.FailedCount;
                    break;
                case TravelStatuses.Abandoned:
                    Assert.Fatal(false,
                                 "NavigationalMemory,RecordTimeTaken: " +
                                 "abandoned status not implemented yet.");
                    break;
            }

            record.CompletedPerSecond = record.CompletedCount/Time.TimeNow;
            record.FailedPerSecond = record.FailedCount/Time.TimeNow;

            LogUtil.WriteLineIfLogNavigationalMemory(
                "NavigationalMemory.RecordTimeTaken:        " + record);

            DumpMemory(DumpFrequency);
        }

        ///<summary>
        ///Get time to reach destination from current position
        ///</summary>
        ///<param name="destination"></param>
        ///<param name="time"></param>
        ///<returns></returns>
        public bool TimeToReachDestination(Vector2 destination, out float time)
        {
            int sourceNodeIndex =
                Owner.PathPlanner.GetClosestNodeToPosition(Owner.Position);
            int destinationNodeIndex =
                Owner.PathPlanner.GetClosestNodeToPosition(destination);

            NavigationalMemoryKey key =
                new NavigationalMemoryKey(
                    TravelModes.FollowPath,
                    sourceNodeIndex,
                    destinationNodeIndex);

            MakeNewRecordIfNotAlreadyPresent(key);
            NavigationalMemoryRecord record = MemoryMap[key];
            time = record.RunningAverageTimeTaken;

            LogUtil.WriteLineIfLogNavigationalMemory(
                "NavigationalMemory.TimeToReachDestination: " + record);

            return record.CompletedCount == 0; //if zero, time was calculated
        }

        ///<summary>
        ///Dump the navigational memory (used for debugging)
        ///</summary>
        [Conditional("LOG_NAVIGATIONAL_MEMORY")]
        [Conditional("DEBUG_NAVIGATIONAL_MEMORY")]
        public void DumpMemory(int dumpFrequency)
        {
            if (dumpFrequency == 0 ||
                Time.TimeNowMs - _timeLastDumped < dumpFrequency)
                return;

            _timeLastDumped = Time.TimeNowMs;

            string divider = new string('-', 111);
            LogUtil.WriteLineIfLogNavigationalMemory(divider);
            foreach (
                KeyValuePair<NavigationalMemoryKey, NavigationalMemoryRecord> kvp
                    in MemoryMap)
            {
                LogUtil.WriteLineIfLogNavigationalMemory(kvp.Value.ToString());
            }
            LogUtil.WriteLineIfLogNavigationalMemory(divider);
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///this methods checks to see if there is an existing record for the
        ///given key. If not a new <see cref="NavigationalMemoryRecord"/>
        ///record is made and added to the memory map. 
        /// </summary>
        private void MakeNewRecordIfNotAlreadyPresent(NavigationalMemoryKey key)
        {
            //check to see if this key already exists in memory. If it doesn't,
            //create a new record
            if (!MemoryMap.ContainsKey(key))
            {
                MemoryMap[key] = new NavigationalMemoryRecord(Owner, key);
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly Dictionary<NavigationalMemoryKey, NavigationalMemoryRecord> _memoryMap;
        private readonly float _memorySpan;
        private readonly BotEntity _owner;
        private NavigationalMemoryRecord _current;
        private int _dumpFrequency;
        private int _timeLastDumped;

        #endregion

        #region Nested type: NavigationalMemoryKey

        ///<summary>
        ///memory records are indexed by this key
        ///TODO: should we move this to a separate file??
        ///</summary>
        public struct NavigationalMemoryKey
        {
            private readonly int _destinationNodeIndex;
            private readonly TravelModes _mode;
            private readonly int _sourceNodeIndex;

            ///<summary>
            ///constructor
            ///</summary>
            ///<param name="mode"></param>
            ///<param name="sourceNodeIndex"></param>
            ///<param name="destinationNodeIndex"></param>
            public NavigationalMemoryKey(
                TravelModes mode,
                int sourceNodeIndex,
                int destinationNodeIndex)
            {
                _mode = mode;
                _sourceNodeIndex = sourceNodeIndex;
                _destinationNodeIndex = destinationNodeIndex;
            }

            ///<summary>
            ///travel mode
            ///</summary>
            public TravelModes Mode
            {
                get { return _mode; }
            }

            ///<summary>
            ///source node index
            ///</summary>
            public int SourceNodeIndex
            {
                get { return _sourceNodeIndex; }
            }

            ///<summary>
            ///destination node index
            ///</summary>
            public int DestinationNodeIndex
            {
                get { return _destinationNodeIndex; }
            }
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}