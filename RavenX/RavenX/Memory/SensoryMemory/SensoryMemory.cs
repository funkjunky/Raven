#region File description

//------------------------------------------------------------------------------
//SensoryMemory.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Math;
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

namespace Mindcrafters.RavenX.Memory.SensoryMemory
{
    ///<summary>
    ///Sensory memory keeps track of opponents detected by sound or sight
    ///TODO: how about add smell or tracking if crossing recent path?
    ///</summary>
    public class SensoryMemory
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for SensoryMemory class
        ///</summary>
        ///<param name="owner">bot that owns this sensory memory</param>
        ///<param name="memorySpan">how soon we forget</param>
        public SensoryMemory(BotEntity owner, float memorySpan)
        {
            _owner = owner;
            _memorySpan = memorySpan;
            _memoryMap = new Dictionary<BotEntity, SensoryMemoryRecord>();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the owner of this instance
        ///</summary>
        public BotEntity Owner
        {
            get { return _owner; }
        }

        ///<summary>
        ///this container is used to simulate memory of sensory events.
        ///A <see cref="SensoryMemoryRecord"/> is created for each opponent in the
        ///environment. Each record is updated whenever the opponent is
        ///encountered. (when it is seen or heard)
        ///</summary>
        public Dictionary<BotEntity, SensoryMemoryRecord> MemoryMap
        {
            get { return _memoryMap; }
        }

        ///<summary>
        ///a bot has a memory span equivalent to this value. When a bot requests
        ///a list of all recently sensed opponents this value is used to
        ///determine if the bot is able to remember an opponent or not.
        ///</summary>
        public float MemorySpan
        {
            get { return _memorySpan; }
        }

        #region Public methods

        ///<summary>
        ///this removes a bot's record from memory
        ///</summary>
        ///<param name="bot"></param>
        public void RemoveBotFromMemory(BotEntity bot)
        {
            MemoryMap.Remove(bot);
        }

        ///<summary>
        ///this updates the record for an individual opponent.
        ///<remarks>
        ///Note, there is no need to test if the opponent is within the FOV
        ///because that test will be done when the <see cref="UpdateVision"/>
        ///method is called
        /// </remarks>
        ///</summary>
        ///<param name="noiseMaker">bot that made the sound</param>
        public void UpdateWithSoundSource(BotEntity noiseMaker)
        {
            //make sure the bot being examined is not this bot
            if (Owner == noiseMaker)
                return;

            //if the bot is already part of the memory then update its data,
            //else create a new memory record and add it to the memory
            //NOTE: Should bots of the same team also be registered? 
            //  Yes if no communication, otherwise they should maybe have to identify themselves?
            MakeNewRecordIfNotAlreadyPresent(noiseMaker);

            SensoryMemoryRecord info = MemoryMap[noiseMaker];

            //test if there is LOS between bots 
            if (GameManager.GameManager.Instance.IsLOSOkay(
                Owner.Position, noiseMaker.Position))
            {
                info.IsShootable = true;

                //record the position of the bot
                info.LastSensedPosition = noiseMaker.Position;
            }
            else
            {
                info.IsShootable = false;
            }

            //record the time it was sensed
            info.TimeLastSensed = Time.TimeNow;
        }

        ///<summary>
        ///this method iterates through all the bots in the game world to test
        ///if they are in the field of view. Each bot's memory record is
        ///updated accordingly
        /// 
        /// The target is what the bot performs actions on. 
        /// Currently I believe their is only one action, shoot.
        /// Therefor the bot targets whichever enemy is closest.
        ///</summary>
        public void UpdateVision()
        {
            //for each bot in the world test to see if it is visible to the
            //owner of this class
            List<BotEntity> bots = GameManager.GameManager.Instance.BotList;
            foreach (BotEntity bot in bots)
            {
                //make sure the bot being examined is not this bot
                if (Owner == bot)
                    continue;

                //Checks to see if the bot is on the same team. If so don't bother target him.
                //TODO: we may still want to target a team mate, perhaps to shoot bullets of healing salves at them.
                //if (Owner.Team == bot.Team)
                //    continue;

                //make sure it is part of the memory map
                MakeNewRecordIfNotAlreadyPresent(bot);

                //get a reference to this bot's data
                SensoryMemoryRecord info = MemoryMap[bot];

                //test if there is LOS between bots 
                if (GameManager.GameManager.Instance.IsLOSOkay(
                    Owner.Position, bot.Position))
                {
                    info.IsShootable = true;

                    //test if the bot is within FOV
                    if (Vector2Util.IsSecondInFOVOfFirst(
                        Owner.Position,
                        Owner.Facing,
                        bot.Position,
                        Owner.FieldOfView))
                    {
                        info.TimeLastSensed = Time.TimeNow;
                        info.LastSensedPosition = bot.Position;
                        info.TimeLastVisible = Time.TimeNow;

                        if (info.IsWithinFOV == false)
                        {
                            info.IsWithinFOV = true;
                            info.TimeBecameVisible = info.TimeLastSensed;
                        }
                    }
                    else
                    {
                        info.IsWithinFOV = false;
                    }
                }
                else
                {
                    info.IsShootable = false;
                    info.IsWithinFOV = false;
                }
            }
        }

        ///<summary>
        ///Gets the list of recently sensed opponents
        ///</summary>
        ///<returns>list of the bots that have been sensed recently</returns>
        public List<BotEntity> GetListOfRecentlySensedOpponents()
        {
            return getListOfOpponentsNotTeammates(true);
        }

        ///<summary>
        ///Gets the list of recently sensed teammates
        ///</summary>
        ///<returns>list of the bots that have been sensed recently</returns>
        public List<BotEntity> GetListOfRecentlySensedTeammates()
        {
            return getListOfOpponentsNotTeammates(false);
        }

        ///<summary>
        ///Tests if opponent is shootable
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>
        ///true if opponent can be shot (i.e. its not obscured by walls)
        /// </returns>
        public bool IsOpponentShootable(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent))
            {
                return MemoryMap[opponent].IsShootable;
            }

            return false;
        }

        ///<summary>
        ///Tests if opponent within FOV
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>true if opponent is within FOV</returns>
        public bool IsOpponentWithinFOV(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent))
            {
                return MemoryMap[opponent].IsWithinFOV;
            }

            return false;
        }

        ///<summary>
        ///Gets the last recorded position of opponent
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>the last recorded position of opponent</returns>
        ///<exception cref="ApplicationException"></exception>
        public Vector2 GetLastRecordedPosition(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent))
            {
                return MemoryMap[opponent].LastSensedPosition;
            }

            throw new ApplicationException(
                "SensoryMemory.GetLastRecordedPosition: " +
                "Attempting to get position of unrecorded bot");
        }

        ///<summary>
        ///Gets the time opponent has been visible
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>the amount of time opponent has been visible</returns>
        public float GetTimeVisible(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent) &&
                MemoryMap[opponent].IsWithinFOV)
            {
                return Time.TimeNow - MemoryMap[opponent].TimeBecameVisible;
            }

            return 0;
        }

        ///<summary>
        ///Gets the time opponent has been out of view
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>
        ///the amount of time the given opponent has remained out of view
        ///(or a high value if opponent has never been seen or not present)
        ///</returns>
        public float GetTimeOutOfView(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent))
            {
                return Time.TimeNow - MemoryMap[opponent].TimeLastVisible;
            }

            return Single.MaxValue;
        }

        ///<summary>
        ///Get the time since opponent was last sensed
        ///</summary>
        ///<param name="opponent"></param>
        ///<returns>the amount of time opponent has been visible</returns>
        public float GetTimeSinceLastSensed(BotEntity opponent)
        {
            if (opponent != null && MemoryMap.ContainsKey(opponent) &&
                MemoryMap[opponent].IsWithinFOV)
            {
                return Time.TimeNow - MemoryMap[opponent].TimeLastSensed;
            }

            return 0;
        }

        ///<summary>
        ///renders boxes around the opponents it has sensed recently.
        ///</summary>
        public void RenderBoxesAroundRecentlySensed()
        {
            List<BotEntity> opponents = GetListOfRecentlySensedOpponents();
            foreach (BotEntity bot in opponents)
            {
                Vector2 topLeft = bot.Position - new Vector2(bot.BoundingRadius);
                Vector2 bottomRight = bot.Position + new Vector2(bot.BoundingRadius);

                DrawUtil.Rect(topLeft, bottomRight, Color.Orange);
            }
        }

        #endregion

        #region Private, protected, internal methods

        private List<BotEntity> getListOfOpponentsNotTeammates(bool getOpponents)
        {
            //this will store all the opponents the bot can remember
            List<BotEntity> bots = new List<BotEntity>();

            float currentTime = Time.TimeNow;
            foreach (KeyValuePair<BotEntity, SensoryMemoryRecord> kvp in MemoryMap)
            {
                //if this bot has been updated in the memory recently, 
                //add to list
                //also if we're getting opponents, and they are on the same team, XOR will fail and they won't be added, or vice versa. =)
                if (((currentTime - kvp.Value.TimeLastSensed) <= MemorySpan) && ((kvp.Key.Team == Owner.Team) ^ getOpponents))
                {
                    bots.Add(kvp.Key);
                }
            }

            return bots;
        }

        ///<summary>
        ///this methods checks to see if there is an existing record for pBot
        ///opponent. If not a new <see cref="SensoryMemoryRecord"/> record is made and
        ///added to the memory map. 
        ///<remarks>
        ///called by <see cref="UpdateWithSoundSource"/> and 
        /// <see cref="UpdateVision"/> 
        ///</remarks>
        /// </summary>
        /// <param name="opponent"></param>
        private void MakeNewRecordIfNotAlreadyPresent(BotEntity opponent)
        {
            //else check to see if this Opponent already exists in the memory. If it doesn't,
            //create a new record
            if (!MemoryMap.ContainsKey(opponent))
            {
                MemoryMap[opponent] = new SensoryMemoryRecord();
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly Dictionary<BotEntity, SensoryMemoryRecord> _memoryMap;
        private readonly float _memorySpan;
        private readonly BotEntity _owner;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}