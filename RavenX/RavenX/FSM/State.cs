#region File description

//------------------------------------------------------------------------------
//State.cs
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

using Mindcrafters.RavenX.Messaging;

namespace Mindcrafters.RavenX.FSM
{
    ///<summary>
    ///abstract base class to define an interface for a state
    ///</summary>
    public abstract class State<T>
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        ///<summary>
        ///this will execute when the state is entered
        ///</summary>
        ///<param name="entity"></param>
        public abstract void Enter(T entity);

        ///<summary>
        ///this is the state's normal update function
        ///</summary>
        ///<param name="entity"></param>
        public abstract void Execute(T entity);

        ///<summary>
        ///this will execute when the state is exited.
        ///</summary>
        ///<param name="entity"></param>
        public abstract void Exit(T entity);

        ///<summary>
        ///this executes if the agent receives a message from the 
        ///message dispatcher
        ///</summary>
        ///<param name="entity"></param>
        ///<param name="msg"></param>
        ///<returns></returns>
        public abstract bool OnMessage(T entity, Telegram msg);

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}