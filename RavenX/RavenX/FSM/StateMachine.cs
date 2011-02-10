#region File description

//------------------------------------------------------------------------------
//StateMachine.cs
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
using Mindcrafters.RavenX.Messaging;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.FSM
{
    ///<summary>
    ///State machine class. Inherit from this class and create some 
    ///states to give your agents FSM functionality
    ///</summary>
    public class StateMachine<T>
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="owner">owner of this state machine</param>
        public StateMachine(T owner)
        {
            _owner = owner;
            _currentState = null;
            _previousState = null;
            _globalState = null;
        }

        ///<summary>
        ///the owner of the FSM
        ///</summary>
        public T Owner
        {
            get { return _owner; }
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Get the current state
        ///</summary>
        ///<returns></returns>
        public State<T> CurrentState
        {
            get { return _currentState; }
        }

        ///<summary>
        ///Get the global state
        ///</summary>
        ///<returns></returns>
        public State<T> GlobalState
        {
            get { return _globalState; }
        }

        ///<summary>
        ///Get the previous state
        ///</summary>
        ///<returns></returns>
        public State<T> PreviousState
        {
            get { return _previousState; }
        }

        #region Public methods

        ///<summary>
        ///set the current state
        ///</summary>
        ///<param name="s"></param>
        public void SetCurrentState(State<T> s)
        {
            _currentState = s;
        }

        ///<summary>
        ///set the global state (which is executed on every update)
        ///</summary>
        ///<param name="s"></param>
        public void SetGlobalState(State<T> s)
        {
            _globalState = s;
        }

        ///<summary>
        ///set the previous state
        ///</summary>
        ///<param name="s"></param>
        public void SetPreviousState(State<T> s)
        {
            _previousState = s;
        }

        ///<summary>
        ///call this to update the FSM
        ///</summary>
        public void Update()
        {
            //if a global state exists, call its execute method, else do nothing
            if (_globalState != null)
            {
                _globalState.Execute(Owner);
            }

            //same for the current state
            if (_currentState != null)
            {
                _currentState.Execute(Owner);
            }
        }

        ///<summary>
        ///handle messages
        ///</summary>
        ///<param name="msg"></param>
        ///<returns>true if message was handled</returns>
        public bool HandleMessage(Telegram msg)
        {
            //first see if the current state is valid and that it can handle
            //the message
            if (_currentState != null && _currentState.OnMessage(Owner, msg))
            {
                return true;
            }

            //if not, and if a global state has been implemented, send 
            //the message to the global state
            return _globalState != null && _globalState.OnMessage(Owner, msg);
        }

        ///<summary>
        ///change to a new state
        ///</summary>
        ///<param name="newState"></param>
        public void ChangeState(State<T> newState)
        {
            Assert.Fatal(newState != null,
                         "StateMachine.ChangeState: trying to assign null state to current");

            //keep a record of the previous state
            _previousState = _currentState;

            //call the exit method of the existing state
            _currentState.Exit(Owner);

            //change state to the new state
            _currentState = newState;

            //call the entry method of the new state
            _currentState.Enter(Owner);
        }

        ///<summary>
        ///change state back to the previous state
        ///</summary>
        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }

        ///<summary>
        ///Tests if the current state's type is equal to the type of the
        ///class passed as a parameter. 
        ///</summary>
        ///<param name="st"></param>
        ///<returns>
        ///true if the current state's type is equal to the type of the
        ///class passed as a parameter.
        ///</returns>
        public bool IsInState(State<T> st)
        {
            return _currentState.GetType() == st.GetType();
        }

        ///<summary>
        ///only ever used during debugging to grab the name of the current state
        ///</summary>
        ///<returns></returns>
        public string GetNameOfCurrentState()
        {
            string s = _currentState.GetType().Name;

            //TODO: need to check this (C# vs C++)
            //remove the 'class ' part from the front of the string
            if (s.Length > 5)
            {
                s = s.Substring(6);
            }

            return s;
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly T _owner;
        private State<T> _currentState;
        private State<T> _globalState;
        private State<T> _previousState;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}