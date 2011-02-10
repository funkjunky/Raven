#region File description
//------------------------------------------------------------------------------
//PlayerInputComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.Platform;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///A player input component
    ///</summary>
    [TorqueXmlSchemaType]
    [TorqueXmlSchemaDependency(Type = typeof(TankMovementComponent))]
    public class PlayerInputComponent : TorqueComponent, ITickObject
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///The game exit method
        ///</summary>
        ///<param name="val"></param>
        protected static void Exit(float val)
        {
            if (val > 0.0f)
            {
                Program.Exit();
            }
        }

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///Gets the owner of this component cast as a T2DSceneObject
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return Owner as T2DSceneObject; }
        }

        ///<summary>
        ///player index (0 to 3) corresponds to gamepad index
        ///</summary>
        [TorqueXmlSchemaType(DefaultValue = "0")]
        public int PlayerIndex
        {
            get { return _playerIndex; }
            set { _playerIndex = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Called each tick
        ///</summary>
        ///<param name="move"></param>
        ///<param name="dt">
        ///The amount of elapsed time since the last call, in seconds.
        ///</param>
        public virtual void ProcessTick(Move move, float dt)
        {
            if (Program.GamePaused)
                return;

            if (move == null)
                return;

            ThrustData td = new ThrustData();
            td.Id = SceneObject.ObjectId;
            td.AngularThrust = move.Sticks[0].X;
            td.ForwardThrust =
                MathHelper.Max(
                    MathHelper.Clamp(move.Sticks[0].Y, 0, 1),
                    move.Triggers[0].Value);
            td.Braking = move.Buttons[0].Pushed ? 1.0f : 0.0f;
            td.ReverseThrust =
                MathHelper.Max(
                    -MathHelper.Clamp(move.Sticks[0].Y, -1, 0),
                    move.Triggers[1].Value);
            td.LateralThrust = move.Sticks[1].X;
            TorqueEventManager.Instance.MgrPostEvent(_thrustEvent, td);
        }

        ///<summary>
        ///Used to interpolate between ticks
        ///</summary>
        ///<param name="k">
        ///The interpolation point (0 to 1) between start and
        ///end of the tick.
        ///</param>
        public virtual void InterpolateTick(float k)
        {
            //todo: interpolate between ticks as needed here
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            PlayerInputComponent obj2 = obj as PlayerInputComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.PlayerIndex = PlayerIndex;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        ///<summary>
        ///Called when the owner is registered
        ///</summary>
        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
                return false;

            //todo: perform initialization for the component

            //todo: look up interfaces exposed by other components
            //E.g., 
            //_theirInterface = 
            //     Owner.Components.GetInterface<ValueInterface<float>>(
            //         "float", "their interface name");  

            //activate tick callback for this component.
            ProcessList.Instance.AddTickCallback(Owner, this);

            BuildPlayerInputMap(PlayerIndex);

            _thrustEvent = new TorqueEvent<ThrustData>("thrustEvent");

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            //todo: perform de-initialization for the component

            base._OnUnregister();
        }

        ///<summary>
        ///Called after the owner is registered to allow interfaces
        ///to be registered
        ///</summary>
        protected override void _RegisterInterfaces(TorqueObject owner)
        {
            base._RegisterInterfaces(owner);

            //todo: register interfaces to be accessed by other components
            //E.g.,
            //Owner.RegisterCachedInterface(
            //     "float", "interface name", this, _ourInterface);
        }

        ///<summary>
        ///Builds the user's input mappings for the player's sprite.
        ///</summary>
        ///<param name="playerIndex">
        ///The player's index in the collection.
        ///</param>
        protected void BuildPlayerInputMap(int playerIndex)
        {
            //Set player as the controllable object
            PlayerManager.Instance.GetPlayer(playerIndex).ControlObject =
                SceneObject;

            //Get input map for this player and configure it
            InputMap inputMap =
                PlayerManager.Instance.GetPlayer(playerIndex).InputMap;

            int[] gamepad = new int[4];
            gamepad[0] = InputManager.Instance.FindDevice("gamepad0");
            gamepad[1] = InputManager.Instance.FindDevice("gamepad1");
            gamepad[2] = InputManager.Instance.FindDevice("gamepad2");
            gamepad[3] = InputManager.Instance.FindDevice("gamepad3");

            //xbox gamepad controls
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.LeftThumbX,
                MoveMapTypes.StickAnalogHorizontal,
                0);
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.RightThumbX,
                MoveMapTypes.StickAnalogHorizontal,
                1);
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.X,
                MoveMapTypes.Button,
                0);
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.A,
                MoveMapTypes.Button,
                0);
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.LeftTrigger,
                MoveMapTypes.TriggerAnalog,
                1);
            inputMap.BindMove(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.RightTrigger,
                MoveMapTypes.TriggerAnalog,
                0);
            inputMap.BindAction(
                gamepad[playerIndex],
                (int)XGamePadDevice.GamePadObjects.Back,
                Exit);


            int keyboardId = InputManager.Instance.FindDevice("keyboard");
            inputMap.BindAction(
                keyboardId,
                (int)
                Keys.Escape,
                Exit);

            if (keyboardId >= 0)
            {
                if (1 == playerIndex)
                {
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.Right,
                        MoveMapTypes.StickDigitalRight,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.Left,
                        MoveMapTypes.StickDigitalLeft,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.Up,
                        MoveMapTypes.StickDigitalUp,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.Down,
                        MoveMapTypes.StickDigitalDown,
                        0);
                }
                else if (0 == playerIndex)
                {
                    //WASD
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.D,
                        MoveMapTypes.StickDigitalRight,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.A,
                        MoveMapTypes.StickDigitalLeft,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.W,
                        MoveMapTypes.StickDigitalUp,
                        0);
                    inputMap.BindMove(
                        keyboardId,
                        (int)Keys.S,
                        MoveMapTypes.StickDigitalDown,
                        0);
                }
            }

            //configure fire button to only fire on make 
            //(when the button is first pushed)
            PlayerManager.Instance.GetPlayer(playerIndex).MoveManager.
                ConfigureButtonRepeat(0, false);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        int _playerIndex; //0-3
        TorqueEvent<ThrustData> _thrustEvent;

        #endregion
    }
}
