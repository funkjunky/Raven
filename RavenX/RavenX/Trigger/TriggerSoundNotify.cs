#region File description

//------------------------------------------------------------------------------
//TriggerSoundNotify.cs
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

using MapContent;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Messaging;

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///whenever an agent makes a sound -- such as when a weapon fires --
    ///this trigger can be used to notify other bots of the event.
    ///
    ///This type of trigger has a circular trigger region and a lifetime
    ///of BotTriggerUpdateFreq update-step(s).
    ///TODO: modify to use time instead of steps
    ///</summary>
    public class TriggerSoundNotify : TriggerLimitedLifetime
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="source"></param>
        ///<param name="range"></param>
        public TriggerSoundNotify(BotEntity source, float range)
            : base(
                new EntitySceneObject(),
                (int) GameManager.GameManager.Instance.Parameters.BotTriggerUpdateFreq)
        {
            //TODO: integrate with Torque
            //SceneObject.CreateWithCollision = false;
            //SceneObject.CreateWithPhysics = false;

            _soundSource = source;

            //set position and range
            Position = SoundSource.Position;

            BoundingRadius = range;

            //create and set this trigger's region of fluence
            AddCircularTriggerRegion(Position, BoundingRadius);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the bot that has made the sound
        ///</summary>
        public BotEntity SoundSource
        {
            get { return _soundSource; }
        }

        #region Public methods

        ///<summary>
        ///when triggered this adds the bot that made the sound to the
        ///triggering bot's perception by sending a message to be processed by
        ///the triggering bot's sensory memory.
        ///</summary>
        ///<param name="bot"></param>
        public override void Try(BotEntity bot)
        {
            //is this bot within range of this sound
            if (!IsTouchingTrigger(bot.Position, bot.BoundingRadius))
                return;

            TriggeringBot = bot;

            MessageDispatcher.Instance.DispatchMsg(
                (int) MessageDispatcher.SEND_MSG_IMMEDIATELY,
                MessageDispatcher.SENDER_ID_IRRELEVANT,
                bot.ObjectId,
                MessageTypes.GunshotSound,
                SoundSource);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly BotEntity _soundSource;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}