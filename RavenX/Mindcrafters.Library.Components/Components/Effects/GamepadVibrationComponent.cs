#region File description
//------------------------------------------------------------------------------
//GamepadVibrationComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft
#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Components
{
    ///<summary>
    ///Used to vibrate a player's gamepad.  Allows for setting the strength and
    ///length of vibration for Xbox 360 gamepad motors.
    ///</summary>
    [TorqueXmlSchemaType]
    public class GamepadVibrationComponent : TorqueComponent, IAnimatedObject
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///Gets and sets if gamepad vibration is currently allowed.
        ///</summary>
        static public bool AllowVibration
        {
            get { return _allowVibration; }
            set { _allowVibration = value; }
        }

        ///<summary>
        ///Gets and sets the global gamepad vibration pause mode.  If paused
        ///then all gamepad vibration is turned off and will pick up where it
        ///left off once resumed.
        ///</summary>
        static public bool PauseVibration
        {
            get { return _paused; }
            set
            {
                _paused = value;
                if (_paused)
                {
                    //Turn off all vibration
                    _componentList.ForEach(
                        delegate(GamepadVibrationComponent gpv)
                    {
                        InputManager.Instance.SetVibration(
                            gpv._deviceIndex, 0.0f, 0.0f);
                    });
                }
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
        ///Gets and sets the gamepad device to vibrate.
        ///</summary>
        public int GamePadDeviceIndex
        {
            get { return _deviceIndex; }
            set { _deviceIndex = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Called every frame.
        ///</summary>
        ///<param name="elapsed">
        ///The amount of elapsed time since the last call, in seconds.
        ///</param>
        public void UpdateAnimation(float elapsed)
        {
            PauseVibration = Program.GamePaused;

            if (_paused)
            {
                _wasPaused = true;
                return;
            }

            bool update = false;

            //If we just came out of a paused state then force an update to 
            //continue the vibration from where we left off.
            if (_wasPaused)
            {
                _wasPaused = false;
                update = true;
            }

            //If the low speed motor is currently active then count down
            //its timer.
            if (!_lowSpeedConstant && _lowSpeedTimer > 0.0f)
            {
                _lowSpeedTimer -= elapsed;
                if (_lowSpeedTimer <= 0.0f)
                {
                    _lowSpeedAmount = 0.0f;
                    update = true;
                }
            }

            //If the high speed motor is currently active then count down
            //its timer.
            if (!_highSpeedConstant && _highSpeedTimer > 0.0f)
            {
                _highSpeedTimer -= elapsed;
                if (_highSpeedTimer <= 0.0f)
                {
                    _highSpeedAmount = 0.0f;
                    update = true;
                }
            }

            if (update)
                _UpdateVibration();
        }

        ///<summary>
        ///Used in cloning
        ///</summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            GamepadVibrationComponent obj2 = obj as GamepadVibrationComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.GamePadDeviceIndex = GamePadDeviceIndex;
        }

        ///<summary>
        ///Set an amount of gamepad low speed vibration
        ///</summary>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void SetLowSpeedVibration(float amount)
        {
            _lowSpeedAmount = amount;
            _lowSpeedTimer = 0.0f;
            _lowSpeedConstant = true;

            _UpdateVibration();
        }

        ///<summary>
        ///Set an amount of gamepad low speed vibration to last a given time
        ///in seconds.
        ///</summary>
        ///<param name="duration">
        ///Length of time for the vibration to last.
        ///</param>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void SetLowSpeedVibration(float duration, float amount)
        {
            _lowSpeedAmount = amount;
            _lowSpeedTimer = duration;
            _lowSpeedConstant = false;

            _UpdateVibration();
        }

        ///<summary>
        ///Add an amount of gamepad low speed vibration to the current 
        ///vibration. If the current vibration amount is less that the given
        ///<paramref name="minAmount"/> then the vibration will be set to the
        ///minimum.  A new duration is also given.
        ///</summary>
        ///<param name="minAmount">
        ///Minimum amount of allowed vibration, ranges from 0 (off) to 1 (max)
        ///</param>
        ///<param name="duration">
        ///Length of time for the vibration to last.
        ///</param>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void AddLowSpeedVibration(
            float minAmount, float duration, float amount)
        {
            if (_lowSpeedAmount < minAmount)
            {
                _lowSpeedAmount = minAmount;
            }
            else
            {
                _lowSpeedAmount += amount;
                if (_lowSpeedAmount > 1.0f)
                    _lowSpeedAmount = 1.0f;
            }

            _lowSpeedTimer = duration;
            _lowSpeedConstant = false;

            _UpdateVibration();
        }

        ///<summary>
        ///Set an amount of gamepad high speed vibration
        ///</summary>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void SetHighSpeedVibration(float amount)
        {
            _highSpeedAmount = amount;
            _highSpeedTimer = 0.0f;
            _highSpeedConstant = true;

            _UpdateVibration();
        }

        ///<summary>
        ///Set an amount of gamepad high speed vibration to last a given time
        ///in seconds
        ///</summary>
        ///<param name="duration">
        ///Length of time for the vibration to last.
        ///</param>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void SetHighSpeedVibration(float duration, float amount)
        {
            _highSpeedAmount = amount;
            _highSpeedTimer = duration;
            _highSpeedConstant = false;

            _UpdateVibration();
        }

        ///<summary>
        ///Add an amount of gamepad high speed vibration to the current 
        ///vibration. If the current vibration amount is less that the given
        ///<paramref name="minAmount"/> then the vibration will be set to the
        ///minimum.  A new duration is also given.
        ///</summary>
        ///<param name="minAmount">
        ///Minimum amount of allowed vibration, ranges from 0 (off) to 1 (max)
        ///</param>
        ///<param name="duration">
        ///Length of time for the vibration to last.
        ///</param>
        ///<param name="amount">
        ///Amount of vibration ranges from 0 (off) to 1 (max)
        ///</param>
        public void AddHighSpeedVibration(
            float minAmount, float duration, float amount)
        {
            if (_highSpeedAmount < minAmount)
            {
                _highSpeedAmount = minAmount;
            }
            else
            {
                _highSpeedAmount += amount;
                if (_highSpeedAmount > 1.0f)
                    _highSpeedAmount = 1.0f;
            }

            _highSpeedTimer = duration;
            _highSpeedConstant = false;

            _UpdateVibration();
        }

        ///<summary>
        ///Stops all gamepad vibration
        ///</summary>
        public void StopVibration()
        {
            _lowSpeedAmount = 0.0f;
            _lowSpeedTimer = 0.0f;
            _lowSpeedConstant = false;

            _highSpeedAmount = 0.0f;
            _highSpeedTimer = 0.0f;
            _highSpeedConstant = false;

            _UpdateVibration();
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

            //activate animation callback for this component.
            ProcessList.Instance.AddAnimationCallback(Owner, this);

            //Add ourselves to the static list
            _componentList.Add(this);

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            //todo: perform de-initialization for the component

            //Make sure we don't leave an active vibration
            InputManager.Instance.StopVibration(_deviceIndex);

            //Remove ourselves from the static list
            if (_componentList.Contains(this))
            {
                _componentList.Remove(this);
            }

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
        ///Update the gamepad vibration settings
        ///</summary>
        protected void _UpdateVibration()
        {
            if (_allowVibration)
            {
                InputManager.Instance.SetVibration(
                    _deviceIndex, _lowSpeedAmount, _highSpeedAmount);
            }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        int _deviceIndex;
        bool _wasPaused;

        float _lowSpeedTimer;
        float _lowSpeedAmount;
        bool _lowSpeedConstant;

        float _highSpeedTimer;
        float _highSpeedAmount;
        bool _highSpeedConstant;

        static readonly List<GamepadVibrationComponent> _componentList =
            new List<GamepadVibrationComponent>();
        static bool _allowVibration = true;
        static bool _paused;

        #endregion
    }
}
