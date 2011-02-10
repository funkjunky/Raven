#region File description

//------------------------------------------------------------------------------
//TriggerOnButtonSendMsg.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Entity.Items;
using Mindcrafters.RavenX.Messaging;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///trigger class to define a button that sends a message to a 
    ///specific entity when activated.
    ///</summary>
    public class TriggerOnButtonSendMsg : Trigger
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Id signifying an invalid receiver.
        ///Note: our entities will never be assigned ObjectId 0 so safe to use
        ///</summary>
        public const uint INVALID_RECEIVER = 0;

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="doorTriggerData"></param>
        public TriggerOnButtonSendMsg(DoorTriggerData doorTriggerData)
            : base(new DoorButtonSceneObject())
        {
            //grap the name
            Name = doorTriggerData.Name;

            //grab the name of the entity it messages
            _receiverName = doorTriggerData.ReceiverName;
            _receiverId = INVALID_RECEIVER; //use lazy lookup later

            //grab the message type
            _messageToSend = doorTriggerData.MessageToSend;

            //grab the position and radius
            Position = doorTriggerData.Position;
            BoundingRadius = doorTriggerData.Radius;

            //create and set this trigger's region of influence
            AddRectangularTriggerRegion(
                //top left corner
                Position - new Vector2(BoundingRadius, BoundingRadius),
                //bottom right corner
                Position + new Vector2(BoundingRadius, BoundingRadius));
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///when triggered a message is sent to the entity with the following Id
        ///</summary>
        public uint ReceiverId
        {
            get
            {
                if (_receiverId == INVALID_RECEIVER)
                {
                    T2DSceneObject receiver = TorqueObjectDatabase.Instance.
                        FindObject<T2DSceneObject>(ReceiverName);
                    Assert.Fatal(receiver != null,
                                 "TriggerOnButtonSendMsg.ReceiverId: no receiver");

                    _receiverId = receiver != null
                                      ?
                                          receiver.ObjectId
                                      : INVALID_RECEIVER;
                }
                return _receiverId;
            }
        }

        ///<summary>
        ///Name of receiver 
        ///(used to identify the receiver before id is assigned)
        ///</summary>
        public string ReceiverName
        {
            get { return _receiverName; }
        }

        ///<summary>
        ///the message that is sent
        ///</summary>
        public MessageTypes MessageToSend
        {
            get { return _messageToSend; }
        }

        #region Public methods

        ///<summary>
        ///when this is called the trigger determines if the entity is within
        ///the trigger's region of influence. If it is then the trigger will be 
        ///triggered and the appropriate action will be taken.
        ///</summary>
        ///<param name="bot">entity activating the trigger</param>
        public override void Try(BotEntity bot)
        {
            if (!IsTouchingTrigger(bot.Position, bot.BoundingRadius))
                return;

            TriggeringBot = bot;

            MessageDispatcher.Instance.DispatchMsg(
                MessageDispatcher.SEND_MSG_IMMEDIATELY,
                ObjectId,
                ReceiverId,
                MessageToSend,
                MessageDispatcher.NO_ADDITIONAL_INFO);
        }

        ///<summary>
        ///called each update-step of the game. This methods updates any
        ///internal state the trigger may have.
        ///</summary>
        ///<param name="dt">time since last update</param>
        public override void Update(float dt)
        {
        }

        ///<summary>
        ///Handle messages sent to this entity. Default is to not handle
        ///messages.
        ///</summary>
        ///<param name="msg">the message</param>
        ///<returns>true if message was handled by this entity</returns>
        public override bool HandleMessage(Telegram msg)
        {
            return false;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly MessageTypes _messageToSend;
        private readonly string _receiverName;
        private uint _receiverId;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}