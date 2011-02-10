#region File description
//------------------------------------------------------------------------------
//BotAnimationComponent.cs
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

using GarageGames.Torque.Core;
using GarageGames.Torque.Sim;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    #region Class BotAnimationComponent

    ///<summary>
    ///class for BotAnimationComponent which determines which animation to play
    ///</summary>
    public class BotAnimationComponent : TorqueComponent, ITickObject, IFSMObject
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public BotAnimationComponent()
        {
            FSM.Instance.RegisterState<BotIdleState>(this, "idle");
            //FSM.Instance.RegisterState<BotFallState>(this, "fall");
            //FSM.Instance.RegisterState<BotJumpState>(this, "jump");
            FSM.Instance.RegisterState<BotBackState>(this, "back");
            FSM.Instance.RegisterState<BotForwardState>(this, "forward");
            FSM.Instance.RegisterState<BotRightState>(this, "right");
            FSM.Instance.RegisterState<BotLeftState>(this, "left");
            FSM.Instance.RegisterState<BotTurnRightState>(this, "turnRight");
            FSM.Instance.RegisterState<BotTurnLeftState>(this, "turnLeft");

            CurrentState = FSM.Instance.GetState(this, "idle");
        }
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///The current state of this <see cref="IFSMObject"/>.
        ///</summary>
        public FSMState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        ///<summary>
        ///The last valid state that this <see cref="IFSMObject"/> was in.
        ///</summary>
        public FSMState PreviousState
        {
            get { return _previousState; }
            set { _previousState = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Called everytime the engine processes a tick. This is guaranteed to
        ///happen at the tick rate, though it may not happen at the exact tick
        ///boundaries.
        ///</summary>
        ///<param name="move">
        ///The move structure that contains information about user input.
        ///</param>
        ///<param name="dt">
        ///The amount of time passed. This is always the tick rate.
        ///</param>
        public void ProcessTick(Move move, float dt)
        {
            FSM.Instance.Execute(this);
        }

        ///<summary>
        ///Called between ticks to interpolate objects, simulating a faster
        ///update than the tick rate.
        ///</summary>
        ///<param name="k">
        ///The percentage of time between ticks from 0 to 1.
        /// </param>
        public void InterpolateTick(float k) { }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        ///<summary>
        ///This method is called when the owning object is registered. Any
        ///initialization needed by the component should be done here. It is
        ///important to call base._OnRegister and respect its return value.
        ///</summary>
        ///<param name="owner">The owner of this component.</param>
        ///<returns>True if initialization successful.</returns>
        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner))
                return false;

            ProcessList.Instance.AddTickCallback(owner, this);

            return true;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        private FSMState _currentState;
        private FSMState _previousState;

        #endregion
    }

    #endregion
}
