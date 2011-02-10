#region File description
//------------------------------------------------------------------------------
//EngineAudioComponent.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System
#endregion

#region Microsoft

using Microsoft.Xna.Framework.Audio;

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
    ///class for EngineAudioComponent (tank engine sounds)
    ///</summary>
    [TorqueXmlSchemaType]
    public class EngineAudioComponent : TorqueComponent, ITickObject
    {
        //======================================================================
        #region Static methods, fields, constructors
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
        ///Gets and sets the engine sounds pause mode. If paused then all engine 
        ///sounds are turned off and will pick up where it left off once resumed
        ///</summary>
        public bool PauseEngineSound
        {
            get { return _paused; }
            set
            {
                _paused = value;
                if (_paused)
                {
                    if (_engineCue != null && _engineCue.IsPlaying)
                    {
                        _engineCue.Pause();
                    }
                }
                else
                {
                    if (_engineCue != null && _engineCue.IsPaused)
                    {
                        _engineCue.Resume();
                    }
                }
            }
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
            PauseEngineSound = Program.GamePaused;

            if (_engineCue == null)
                return;

            _engineCue.SetVariable(
                "EngineSpeed",
                (SceneObject.Physics.Velocity.LengthSquared() /
                 10000.0f * 100.0f)); //maxVelSq
            _engineCue.SetVariable(
                "AngularSpeed",
                (System.Math.Abs(SceneObject.Physics.AngularVelocity)
                 / 75.0f * 100.0f)); //maxAngVel
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
            EngineAudioComponent obj2 = obj as EngineAudioComponent;
            if (obj2 == null)
                return;

            //TODO: add copy for each settable public property that isn't
            //     marked with the attribute [XmlIgnore]
            // obj2.Property = Property;
            obj2.PauseEngineSound = PauseEngineSound;
        }

        ///<summary>
        ///Stop all tank engine sounds.
        ///</summary>
        ///<param name="now">
        ///Determines if all sounds should stop immediately or as setup in XACT.
        ///</param>
        public void StopAllSound(bool now)
        {
            if (_engineCue == null || 
                (!_engineCue.IsPlaying && !_engineCue.IsPaused)) 
                return;

            if (now)
            {
                _engineCue.Stop(AudioStopOptions.Immediate);
            }
            else
            {
                _engineCue.Stop(AudioStopOptions.AsAuthored);
            }
        }

        ///<summary>
        ///Start all tank engine sounds.
        ///</summary>
        public void StartAllSound()
        {
            if (_engineCue != null && !_engineCue.IsStopped)
            {
                _engineCue.Stop(AudioStopOptions.Immediate);
            }

            _engineCue = Program.SoundBank.GetCue("tankEngine");
            _engineCue.SetVariable("EngineSpeed", 0.0f);
            _engineCue.SetVariable("AngularSpeed", 0.0f);
            _engineCue.Play();
        }

        ///<summary>
        ///Called by the scheduler to start sounds
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="scheduleEventArguments"></param>
        public void ScheduledStartSound(
            object sender, ScheduledEventArguments scheduleEventArguments)
        {
            StartAllSound();
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

            Program.Engine.GameTimeSchedule.Schedule(100, ScheduledStartSound);

            return true;
        }

        ///<summary>
        ///Called when the owner is unregistered
        ///</summary>
        protected override void _OnUnregister()
        {
            //todo: perform de-initialization for the component

            StopAllSound(true);

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

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        Cue _engineCue;
        bool _paused;

        #endregion
    }
}
