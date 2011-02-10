#region File description

//------------------------------------------------------------------------------
//TargetingSystem.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;

    #endregion

    #region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Armory
{
    ///<summary>
    ///class to implement a targeting system
    ///</summary>
    public class TargetingSystem
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner"></param>
        public TargetingSystem(BotEntity owner)
        {
            _owner = owner;
            _currentTarget = null;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///true if there is a currently assigned target
        ///</summary>
        public bool IsTargetPresent
        {
            get { return _currentTarget != null; }
        }

        ///<summary>
        ///the target. null if no target current.
        ///</summary>
        public BotEntity Target
        {
            get { return _currentTarget; }
        }

        ///<summary>
        ///true if target is within FOV
        ///</summary>
        public bool IsTargetWithinFOV
        {
            get { return _owner.SensoryMemory.IsOpponentWithinFOV(_currentTarget); }
        }

        ///<summary>
        ///true if target is shootable
        ///</summary>
        public bool IsTargetShootable
        {
            get { return _owner.SensoryMemory.IsOpponentShootable(_currentTarget); }
        }

        ///<summary>
        ///last recorded position of the target
        ///</summary>
        public Vector2 LastRecordedPosition
        {
            get { return _owner.SensoryMemory.GetLastRecordedPosition(_currentTarget); }
        }

        ///<summary>
        ///the time the target has been visible
        ///</summary>
        public float TimeTargetVisible
        {
            get { return _owner.SensoryMemory.GetTimeVisible(_currentTarget); }
        }

        ///<summary>
        ///the time the target has been out of view
        ///</summary>
        public float TimeTargetOutOfView
        {
            get { return _owner.SensoryMemory.GetTimeOutOfView(_currentTarget); }
        }

        #region Public methods

        ///<summary>
        ///sets the target to null
        ///</summary>
        public void ClearTarget()
        {
            _currentTarget = null;
        }

        ///<summary>
        ///update target
        ///</summary>
        public void Update()
        {
            float closestDistSoFar = Single.MaxValue;
            _currentTarget = null;

            //grab a list of all the opponents the owner can sense
            List<BotEntity> sensedBots
                = _owner.SensoryMemory.GetListOfRecentlySensedOpponents();

            foreach (BotEntity curBot in sensedBots)
            {
                //make sure the bot is alive and that it is not the owner and is on a different team.
                if (!curBot.IsAlive || (curBot == _owner) || curBot.Team == _owner.Team)
                    continue;

                float dist =
                    (curBot.Position - _owner.Position).LengthSquared();

                int teammatesTargetingBot = curBot.SensoryMemory.GetListOfRecentlySensedTeammates().FindAll(delegate(BotEntity teammate) { return teammate.TargetBot == curBot.TargetBot; }).Count;

                const int maxBotsForMaxDesirability = 16;
                //f(x) = max((16 + 1) - (x-16)^2/16, 0)
                //in effect x=0 [no bots targeting the bot] means f(x) = 1
                //x = 16, f(x) = 17
                //x = 32, f(x) = 1
                //x = 33, f(x) = 0
                float teammatesTargetingBotMultiplier = (float)Math.Max(
                                                        (maxBotsForMaxDesirability + 1) - Math.Pow(teammatesTargetingBot - maxBotsForMaxDesirability, 2) / maxBotsForMaxDesirability, 
                                                        0.0f);
                dist /= teammatesTargetingBotMultiplier;

                //the multiplier makes bots appear closer than they are when choosing one.
                if (dist >= closestDistSoFar)
                    continue;

                closestDistSoFar = dist;
                _currentTarget = curBot;
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        //the owner of this system
        private readonly BotEntity _owner;

        //the current target (this will be null if there is no target assigned)
        private BotEntity _currentTarget;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}