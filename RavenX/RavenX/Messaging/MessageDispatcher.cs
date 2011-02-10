#region File description

//------------------------------------------------------------------------------
//MessageDispatcher.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using GarageGames.Torque.Core;
using MapContent;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;

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
    ///A message dispatcher. Manages messages of the type Telegram.
    ///Instantiated as a singleton.
    ///</summary>
    public class MessageDispatcher
    {
        #region Static methods, fields, constructors

        #region Singleton pattern

        private static MessageDispatcher _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private MessageDispatcher()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;
            _logPrefixText =
                String.Format("[{0,-8}] [{1,17}.", "DISPATCH", "Dispatcher");
        }

        ///<summary>
        ///Accessor for the MessageDispatcher singleton instance.
        ///</summary>
        public static MessageDispatcher Instance
        {
            get
            {
                if (null == _instance)
                    new MessageDispatcher();

                Assert.Fatal(null != _instance,
                             "Singleton instance not set by constructor.");
                return _instance;
            }
        }

        #endregion

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //to make code easier to read

        ///<summary>
        ///There's no extra information
        ///</summary>
        public const int NO_ADDITIONAL_INFO = 0;

        ///<summary>
        ///message should be sent without delay
        ///</summary>
        public const float SEND_MSG_IMMEDIATELY = 0.0f;

        ///<summary>
        ///The id of the sender is irrelevant (system generated)
        ///</summary>
        public const uint SENDER_ID_IRRELEVANT = 0;

        ///<summary>
        ///a set is used as the container for the delayed messages
        ///because of the benefit of automatic sorting and avoidance
        ///of duplicates. Messages are sorted by their dispatch time.
        ///TODO: actually, set was not used. Need to check this.
        ///</summary>
        public PriorityQueue<Telegram, float> PQ
        {
            get { return _pq; }
        }

        #region Public methods

        ///<summary>
        ///given a message, a receiver, a sender and any time delay, this method
        ///routes the message to the correct agent (if no delay) or stores in
        ///the message queue to be dispatched at the correct time
        ///</summary>
        ///<param name="delay"></param>
        ///<param name="senderId"></param>
        ///<param name="receiverId"></param>
        ///<param name="msg"></param>
        ///<param name="extraInfo"></param>
        public void DispatchMsg(float delay,
                                uint senderId,
                                uint receiverId,
                                MessageTypes msg,
                                object extraInfo)
        {
            //get a pointer to the receiver
            Entity.Entity receiver =
                EntityManager.Instance.GetEntityFromId(receiverId);

            //make sure the receiver is valid
            if (receiver == null)
            {
                LogUtil.WriteLineIfLogMessaging(LogPrefixText +
                                                String.Format("{0,-23}] ", "DispatchMsg") +
                                                String.Format("[{0,-9}]", " ") + //skip status field
                                                " Warning! No Receiver with ID of " +
                                                receiverId + " found");
                return;
            }

            //create the telegram
            Telegram telegram =
                new Telegram(
                    SEND_MSG_IMMEDIATELY,
                    senderId,
                    receiverId,
                    msg,
                    extraInfo);

            //if there is no delay, route telegram immediately                       
            if (delay <= SEND_MSG_IMMEDIATELY)
            {
                LogUtil.WriteLineIfLogMessaging(LogPrefixText +
                                                String.Format("{0,-23}] ", "DispatchMsg") +
                                                String.Format("[{0,-9}]", " ") + //skip status field
                                                " Telegram dispatched at time: " +
                                                Time.TimeNow.ToString("F3") + " by " +
                                                (senderId == SENDER_ID_IRRELEVANT
                                                     ?
                                                         "god"
                                                     : senderId.ToString()) +
                                                " for " + receiverId + ". Msg is " + msg);
                //send the telegram to the recipient
                Discharge(receiver, telegram);
            }
                //else calculate the time when the telegram should be dispatched
            else
            {
                float currentTime = Time.TimeNow;

                telegram.DispatchTime = currentTime + delay;

                //and put it in the queue
                PQ.Enqueue(telegram, telegram.DispatchTime);

                LogUtil.WriteLineIfLogMessaging(LogPrefixText +
                                                String.Format("{0,-23}] ", "DispatchMsg") +
                                                String.Format("[{0,-9}]", " ") + //skip status field
                                                " Delayed telegram from " +
                                                (senderId == SENDER_ID_IRRELEVANT
                                                     ?
                                                         "god"
                                                     : senderId.ToString()) +
                                                " recorded at time " +
                                                Time.TimeNow.ToString("F3") +
                                                " to be delayed by " + delay.ToString(("F3") +
                                                                                      " secs. for " + receiverId +
                                                                                      ". Msg is " + msg));
            }
        }

        ///<summary>
        ///This method dispatches any telegrams with a timestamp that has
        ///expired. Any dispatched telegrams are removed from the queue
        ///</summary>
        public void DispatchDelayedMessages()
        {
            //first get current time
            float currentTime = Time.TimeNow;

            //now peek at the queue to see if any telegrams need dispatching.
            //remove all telegrams from the front of the queue that have gone
            //past their sell by date
            while (PQ.Count > 0 &&
                   (PQ.Peek().Value.DispatchTime < currentTime) &&
                   (PQ.Peek().Value.DispatchTime > 0))
            {
                //read the telegram from the front of the queue
                Telegram telegram = PQ.Peek().Value;

                //find the recipient
                Entity.Entity pReceiver =
                    EntityManager.Instance.GetEntityFromId(telegram.Receiver);

                LogUtil.WriteLineIfLogMessaging(LogPrefixText +
                                                String.Format("{0,-23}] ", "DispatchDelayedMessages") +
                                                String.Format("[{0,-9}]", " ") + //skip status field
                                                " Queued telegram ready for dispatch: Sent to " +
                                                pReceiver.ObjectId + ". Msg is " + telegram.Msg);

                //send the telegram to the recipient
                Discharge(pReceiver, telegram);

                //remove it from the queue
                PQ.Dequeue();
            }
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///common text prefix for log: "[DISPATCH] [Dispatcher."
        ///</summary>
        protected string LogPrefixText
        {
            get { return _logPrefixText; }
        }

        ///<summary>
        ///This method is utilized by <see cref="DispatchMsg"/> or 
        ///<see cref="DispatchDelayedMessages"/>.
        ///This method calls the message handling member of the receiving
        ///entity, receiver, with the newly created telegram
        ///</summary>
        ///<param name="receiver"></param>
        ///<param name="telegram"></param>
        public void Discharge(Entity.Entity receiver, Telegram telegram)
        {
            if (!receiver.HandleMessage(telegram))
            {
                //telegram could not be handled
                LogUtil.WriteLineIfLogMessaging(LogPrefixText +
                                                String.Format("{0,-23}] ", "Discharge") +
                                                String.Format("[{0,-9}]", " ") + //skip status field
                                                " Message <" + telegram + "> not handled");
            }
        }

        #endregion

        #region Private, protected, internal fields

        private readonly string _logPrefixText;

        private readonly PriorityQueue<Telegram, float> _pq =
            new PriorityQueue<Telegram, float>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}