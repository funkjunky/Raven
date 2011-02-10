#region File description
//------------------------------------------------------------------------------
//BotForwardState.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters

using Mindcrafters.Library.Math;

#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    #region Class BotForwardState

    ///<summary>
    ///class for BotForwardState (bot moving forward)
    ///</summary>
    public class BotForwardState : FSMState
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Optional Enter method that will be called by the Finite State Machine
        ///manager when entering this state.
        ///</summary>
        ///<param name="obj">
        ///The <see cref="IFSMObject"/> on which this state is being
        ///transitioned to.
        ///</param>
        public override void Enter(IFSMObject obj)
        {
        }

        ///<summary>
        ///Required Execute method that defines the rules by which this state
        ///will automatically choose another state to switch to. This method
        ///is overridden and defined to return the name of the state to switch
        ///to based on some criteria.
        ///</summary>
        ///<param name="obj">
        ///The <see cref="IFSMObject"/> on which this state is currently being
        ///executed.
        ///</param>
        ///<returns>
        ///A string containing the state that the specified
        ///<see cref="IFSMObject"/> should switch to.
        ///</returns>
        public override string Execute(IFSMObject obj)
        {
            T2DShape3D bot =
                ((BotAnimationComponent) obj).Owner as T2DShape3D;

            Assert.Fatal(null != bot,
                "BotForwardState.Execute: bot is null");

            //this should not happen
            if (null == bot)
            {
                return StateName;
            }

            Vector2 facing = T2DVectorUtil.VectorFromAngle(bot.Rotation);
            Vector2 forward = Vector2.Normalize(facing);
            Vector2 right = Vector2Util.Perp(forward);
            Vector2 projection = Vector2.Dot(bot.Physics.Velocity, forward) * forward;
            Vector2 normal = projection - bot.Physics.Velocity;

            if (bot.Physics.Velocity.Length() > 0.1f)
            {
                if (projection.Length() > normal.Length())
                {
                    if (Vector2.Dot(projection, forward) >= 0)
                    {
                        //bot.SetSequence("main", "run", 0); //already set
                        return FSM.Instance.GetState(obj, "forward").StateName;
                    }
                    bot.SetSequence("main", "back", 0);
                    return FSM.Instance.GetState(obj, "back").StateName;
                }

                if (Vector2.Dot(normal, right) >= 0)
                {
                    bot.SetSequence("main", "side", 0);
                    return FSM.Instance.GetState(obj, "right").StateName;
                }
                bot.SetSequence("main", "side", 0);
                return FSM.Instance.GetState(obj, "left").StateName;
            }

            if (bot.Physics.AngularVelocity > 0.1f)
            {
                bot.SetSequence("main", "side", 0);
                return FSM.Instance.GetState(obj, "turnRight").StateName;
            }

            if (bot.Physics.AngularVelocity < -0.1f)
            {
                bot.SetSequence("main", "side", 0);
                return FSM.Instance.GetState(obj, "turnLeft").StateName;
            }
            bot.SetSequence("main", "root", 0);
            return FSM.Instance.GetState(obj, "idle").StateName;
        }

        ///<summary>
        ///Optional Exit method that will be called by the Finite State Machine
        ///manager when leaving this state.
        ///</summary>
        ///<param name="obj">
        ///The <see cref="IFSMObject"/> on which this state is being
        ///transitioned from.</param>
        public override void Exit(IFSMObject obj)
        {
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }

    #endregion
}
