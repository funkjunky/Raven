#region File description
//------------------------------------------------------------------------------
// WeightedSteeringComponent.cs
//
// Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Using directives

#region System

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.MathUtil;
using GarageGames.Torque.SceneGraph;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.Tx2D.GameAI
{
    /// <summary>
    /// TODO: add component description here
    /// </summary>
    [TorqueXmlSchemaType]
    public class WeightedSteeringComponent : SteeringComponent
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        [TorqueXmlSchemaType(DefaultValue = "0.1")]
        public float WanderWeight
        {
            get { return _wanderWeight; }
            set { _wanderWeight = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "0.9")]
        public float ArriveWeight
        {
            get { return _arriveWeight; }
            set { _arriveWeight = value; }
        }

        [TorqueXmlSchemaType(DefaultValue = "0.0")]
        public float SeekWeight
        {
            get { return _seekWeight; }
            set { _seekWeight = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        /// <summary>
        /// Used in cloning
        /// </summary>
        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            WeightedSteeringComponent obj2 = obj as WeightedSteeringComponent;
            //TODO: add copy for each settable public property that isn't
            //      marked with the attribute [XmlIgnore]
            //  obj2.Property = Property;
            obj2.ArriveWeight = ArriveWeight;
            obj2.SeekWeight = SeekWeight;
            obj2.WanderWeight = WanderWeight;
        }

        public override void Steer()
        {
            Vector2? weightedVelocity = DetermineWeightedVelocity();

            DetermineThrust(weightedVelocity);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        /// <summary>
        /// Called when the owner is registered
        /// </summary>
        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
                return false;

            // todo: perform initialization for the component

            // todo: look up interfaces exposed by other components
            // E.g., 
            // _theirInterface = 
            //      Owner.Components.GetInterface<ValueInterface<float>>(
            //          "float", "their interface name");            

            return true;
        }

        /// <summary>
        /// Called when the owner is unregistered
        /// </summary>
        protected override void _OnUnregister()
        {
            // todo: perform de-initialization for the component

            base._OnUnregister();
        }

        /// <summary>
        /// Called after the owner is registered to allow interfaces
        /// to be registered
        /// </summary>
        protected override void _RegisterInterfaces(TorqueObject owner)
        {
            base._RegisterInterfaces(owner);

            // todo: register interfaces to be accessed by other components
            // E.g.,
            // Owner.RegisterCachedInterface(
            //      "float", "interface name", this, _ourInterface);
        }

        private Vector2? DetermineWeightedVelocity()
        {
            Vector2? weightedVelocity = null;
            int count = 0;
            float totalWeight = 0;

            foreach (SteeringBehaviorComponent sb
                in Owner.Components.Itr<SteeringBehaviorComponent>())
            {
                Vector2? velocity = sb.DesiredVelocity();
                if (velocity != null)
                {
                    count++;
                    float weight;
                    switch (sb.SteeringBehaviorType)
                    {
                        case SteeringBehaviorComponent.SteeringBehaviorTypes.Arrive:
                            weight = ArriveWeight;
                            break;
                        case SteeringBehaviorComponent.SteeringBehaviorTypes.Seek:
                            weight = SeekWeight;
                            break;
                        case SteeringBehaviorComponent.SteeringBehaviorTypes.Wander:
                            weight = WanderWeight;
                            break;
                        default:
                            weight = 0;
                            break;
                    }
                    totalWeight += weight;

                    if (weightedVelocity == null)
                    {
                        weightedVelocity = velocity * weight;
                    }
                    else
                    {
                        weightedVelocity += velocity * weight;
                    }
                }
            }

            if (weightedVelocity != null && totalWeight != 0)//count != 0)
            {
                //weightedVelocity /= count;
                weightedVelocity /= totalWeight;
            }

            return weightedVelocity;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        float _wanderWeight;
        float _arriveWeight;
        float _seekWeight;

        #endregion
    }
}
