#region File description

//------------------------------------------------------------------------------
//Door.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using MapContent;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Items;
using Mindcrafters.RavenX.Messaging;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Map
{
    ///<summary>
    ///class for a door (with an associated scene object)
    ///</summary>
    public class Door : Entity.Entity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="map"></param>
        ///<param name="doorData"></param>
        public Door(Map map, DoorData doorData)
            : base(new EntitySceneObject()) //not needed??
        {
            _status = DoorStatus.Closed;
            _numTicksStayOpen = 600; //TODO: this should be a parameter

            Name = doorData.Name;

            _p1 = doorData.From;
            _p2 = doorData.To;

            _switchIds = new List<uint>();
            foreach (string triggerName in doorData.TriggerList)
            {
                T2DSceneObject trigger =
                    TorqueObjectDatabase.Instance.FindObject<T2DSceneObject>(triggerName);
                Assert.Fatal(trigger != null,
                             "Door.Door: no trigger");
                if (trigger != null) _switchIds.Add(trigger.ObjectId);
            }

            _toP2Norm = Vector2.Normalize(_p2 - _p1);
            _currentSize = _doorSize = Vector2.Distance(_p2, _p1);

            Vector2 perp = Vector2Util.Perp(_toP2Norm);

            //create the walls that make up the door's geometry
            _wall1 = map.AddWall(_p1 + perp, _p2 + perp);
            _wall2 = map.AddWall(_p2 - perp, _p1 - perp);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region DoorStatus enum

        ///<summary>
        ///door states
        ///</summary>
        public enum DoorStatus
        {
            Open,
            Opening,
            Closed,
            Closing
        } ;

        #endregion

        ///<summary>
        ///door state
        ///</summary>
        public DoorStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        ///<summary>
        ///a sliding door is created from two walls, back to back.These walls
        ///must be added to a map's geometry in order for an agent to detect
        ///them
        ///</summary>
        public Wall Wall1
        {
            get { return _wall1; }
            set { _wall1 = value; }
        }

        ///<summary>
        ///a sliding door is created from two walls, back to back.These walls
        ///must be added to a map's geometry in order for an agent to detect
        ///them
        ///</summary>
        public Wall Wall2
        {
            get { return _wall2; }
            set { _wall2 = value; }
        }

        ///<summary>
        ///a list of the id's of the triggers able to open this door
        ///</summary>
        public List<uint> SwitchIds
        {
            get { return _switchIds; }
            set { _switchIds = value; }
        }

        ///<summary>
        ///how long the door remains open before it starts to shut again
        ///</summary>
        public int NumTicksStayOpen
        {
            get { return _numTicksStayOpen; }
            set { _numTicksStayOpen = value; }
        }

        ///<summary>
        ///how long the door has been open (0 if status is not open)
        ///</summary>
        public int NumTicksCurrentlyOpen
        {
            get { return _numTicksCurrentlyOpen; }
            set { _numTicksCurrentlyOpen = value; }
        }

        ///<summary>
        ///the door's (wall1) position when in the open position
        ///</summary>
        public Vector2 P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        ///<summary>
        ///the door's (wall2) position when in the open position
        ///</summary>
        public Vector2 P2
        {
            get { return _p2; }
            set { _p2 = value; }
        }

        ///<summary>
        ///the door's when in the closed position
        ///</summary>
        public float DoorSize
        {
            get { return _doorSize; }
            set { _doorSize = value; }
        }

        ///<summary>
        ///a normalized vector facing along the door. This is used frequently
        ///by the other methods so we might as well just calculate it once in
        ///the constructor
        ///</summary>
        public Vector2 ToP2Norm
        {
            get { return _toP2Norm; }
            set { _toP2Norm = value; }
        }

        ///<summary>
        ///the door's current size
        ///</summary>
        public float CurrentSize
        {
            get { return _currentSize; }
            set { _currentSize = value; }
        }

        #region Public methods

        ///<summary>
        ///Update the door (part of the game update logic)
        ///TODO: should we cache the trigger objects?
        ///</summary>
        ///<param name="dt">time since last update</param>
        public override void Update(float dt)
        {
            switch (_status)
            {
                case DoorStatus.Opening:
                    Open();
                    foreach (uint triggerId in _switchIds)
                    {
                        DoorButtonSceneObject trigger =
                            TorqueObjectDatabase.Instance.FindObject<DoorButtonSceneObject>(triggerId);
                        trigger.SetTransitioning();
                    }
                    break;

                case DoorStatus.Closing:
                    Close();
                    foreach (uint triggerId in _switchIds)
                    {
                        DoorButtonSceneObject trigger =
                            TorqueObjectDatabase.Instance.FindObject<DoorButtonSceneObject>(triggerId);
                        trigger.SetTransitioning();
                    }
                    break;

                case DoorStatus.Open:
                    {
                        foreach (uint triggerId in _switchIds)
                        {
                            DoorButtonSceneObject trigger =
                                TorqueObjectDatabase.Instance.FindObject<DoorButtonSceneObject>(triggerId);
                            trigger.SetOpen();
                        }
                        if (_numTicksCurrentlyOpen-- < 0)
                        {
                            _status = DoorStatus.Closing;
                        }
                    }
                    break;
                default:
                    foreach (uint triggerId in _switchIds)
                    {
                        DoorButtonSceneObject trigger =
                            TorqueObjectDatabase.Instance.FindObject<DoorButtonSceneObject>(triggerId);
                        trigger.SetClosed();
                    }
                    break;
            }
        }

        ///<summary>
        ///adds the id of a trigger for the door to notify when operating
        ///</summary>
        ///<param name="id"></param>
        public void AddSwitch(uint id)
        {
            //only add the trigger if it isn't already present
            if (!_switchIds.Contains(id))
            {
                _switchIds.Add(id);
            }
        }

        ///<summary>
        ///Handle messages sent to this door.
        ///</summary>
        ///<param name="msg">the message</param>
        ///<returns>true if message was handled by this door</returns>
        public override bool HandleMessage(Telegram msg)
        {
            if (msg.Msg == MessageTypes.OpenSesame)
            {
                if (_status != DoorStatus.Open)
                {
                    _status = DoorStatus.Opening;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region Private, protected, internal methods

        //---------------------------- ChangePosition ---------------------------------
        ///<summary>
        ///change position of the door's walls
        ///</summary>
        ///<param name="newP1"></param>
        ///<param name="newP2"></param>
        protected void ChangePosition(Vector2 newP1, Vector2 newP2)
        {
            _p1 = newP1;
            _p2 = newP2;

            _wall1.From = _p1 + Vector2Util.Perp(_toP2Norm);
            _wall1.To = _p2 + Vector2Util.Perp(_toP2Norm);

            _wall2.From = _p2 - Vector2Util.Perp(_toP2Norm);
            _wall2.To = _p1 - Vector2Util.Perp(_toP2Norm);
        }

        ///<summary>
        ///open door
        ///</summary>
        protected void Open()
        {
            if (_status != DoorStatus.Opening)
                return;

            if (_currentSize < 2) //TODO: should be a parameter
            {
                _status = DoorStatus.Open;
                _numTicksCurrentlyOpen = _numTicksStayOpen;
                return;
            }

            //reduce the current size
            _currentSize -= 1;

            _currentSize = MathHelper.Clamp(_currentSize, 0, _doorSize);

            ChangePosition(_p1, _p1 + _toP2Norm*_currentSize);
        }

        ///<summary>
        ///close door
        ///</summary>
        protected void Close()
        {
            if (_status != DoorStatus.Closing)
                return;

            if (_currentSize == _doorSize)
            {
                _status = DoorStatus.Closed;
                return;
            }

            //reduce the current size
            _currentSize += 1;

            _currentSize = MathHelper.Clamp(_currentSize, 0, _doorSize);

            ChangePosition(_p1, _p1 + _toP2Norm*_currentSize);
        }

        #endregion

        #region Private, protected, internal fields

        private float _currentSize;
        private float _doorSize;
        private int _numTicksCurrentlyOpen;
        private int _numTicksStayOpen;
        private Vector2 _p1;
        private Vector2 _p2;
        private DoorStatus _status;
        private List<uint> _switchIds;
        private Vector2 _toP2Norm;
        private Wall _wall1;
        private Wall _wall2;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}