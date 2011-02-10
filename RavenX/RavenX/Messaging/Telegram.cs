#region File description

//------------------------------------------------------------------------------
//Telegram.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using MapContent;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Messaging
{
    ///<summary>
    ///This defines a telegram. A telegram is a data structure that
    ///records information required to dispatch messages. Messages 
    ///are used by game agents to communicate with each other.
    ///</summary>
    public class Telegram
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public Telegram()
            : this(-1, uint.MaxValue, uint.MaxValue, (MessageTypes) (-1), null)
        {
        }

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="dispatchTime"></param>
        ///<param name="sender"></param>
        ///<param name="receiver"></param>
        ///<param name="msg"></param>
        public Telegram(
            float dispatchTime,
            uint sender,
            uint receiver,
            MessageTypes msg)
            : this(dispatchTime, sender, receiver, msg, null)
        {
        }

        ///<summary>
        ////constructor
        ///</summary>
        ///<param name="dispatchTime"></param>
        ///<param name="sender"></param>
        ///<param name="receiver"></param>
        ///<param name="msg"></param>
        ///<param name="extraInfo"></param>
        public Telegram(
            float dispatchTime,
            uint sender,
            uint receiver,
            MessageTypes msg,
            object extraInfo)
        {
            DispatchTime = dispatchTime;
            Sender = sender;
            Receiver = receiver;
            Msg = msg;
            ExtraInfo = extraInfo;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //NOTE: our port does do this. Need to check.
        ////these telegrams will be stored in a priority queue. Therefore the >
        ////operator needs to be overloaded so that the PQ can sort the telegrams
        ////by time priority. Notice how the times must be smaller than
        ////SMALLEST_DELAY apart before two Telegrams are considered unique.

        private const float SMALLEST_DELAY = 0.25f;

        ///<summary>
        ///the entity that sent this telegram
        ///</summary>
        public uint Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        ///<summary>
        ///the entity that is to receive this telegram
        ///</summary>
        public uint Receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }

        ///<summary>
        ///the message itself.
        ///</summary>
        public MessageTypes Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }

        ///<summary>
        ///messages can be dispatched immediately or delayed for a specified 
        ///amount of time. If a delay is necessary this field is stamped with
        ///the time the message should be dispatched.
        ///</summary>
        public float DispatchTime
        {
            get { return _dispatchTime; }
            set { _dispatchTime = value; }
        }

        ///<summary>
        ///any additional information that may accompany the message
        ///</summary>
        public object ExtraInfo
        {
            get { return _extraInfo; }
            set { _extraInfo = value; }
        }

        #region Public methods

        ///<summary>
        ///used for outputting debug info
        ///</summary>
        ///<param name="msg"></param>
        ///<returns></returns>
        public string MessageToString(MessageTypes msg)
        {
            switch (msg)
            {
                case MessageTypes.PathReady:
                    return "PathReady";

                case MessageTypes.NoPathAvailable:
                    return "NoPathAvailable";

                case MessageTypes.TakeThatMF:
                    return "TakeThatMF";

                case MessageTypes.YouGotMeYouSOB:
                    return "YouGotMeYouSOB";

                case MessageTypes.GoalQueueEmpty:
                    return "GoalQueueEmpty";

                case MessageTypes.OpenSesame:
                    return "OpenSesame";

                case MessageTypes.GunshotSound:
                    return "GunshotSound";

                case MessageTypes.UserHasRemovedBot:
                    return "UserHasRemovedBot";

                default:
                    return "Undefined message!";
            }
        }

        ///<summary>
        ///messages that are considered the same
        ///TODO: we didn't use this ...
        ///</summary>
        ///<param name="t"></param>
        ///<returns></returns>
        public bool IsSameAs(Telegram t)
        {
            return Math.Abs(DispatchTime - t.DispatchTime) < SMALLEST_DELAY &&
                   Sender == t.Sender &&
                   Receiver == t.Receiver &&
                   Msg == t.Msg;
        }

        ///<summary>
        ///message ordering
        ///TODO: we didn't use this ...
        ///</summary>
        ///<param name="t"></param>
        ///<returns></returns>
        public bool IsEarlierThan(Telegram t)
        {
            if (this == t)
            {
                return false;
            }

            return (DispatchTime < t.DispatchTime);
        }

        ///<summary>
        ///convert message to readable format
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            return "time: " + DispatchTime + "  Sender: " + Sender +
                   "   Receiver: " + Receiver + "   Msg: " + Msg;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private float _dispatchTime;
        private object _extraInfo;
        private MessageTypes _msg;
        private uint _receiver;
        private uint _sender;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}