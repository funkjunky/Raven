#region File description
//------------------------------------------------------------------------------
//Regulator.cs
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
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    ///<summary>
    ///A regulator that is ready periodically
    ///</summary>
    [TorqueXmlSchemaType(ExportType = false)]
    public class Regulator
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor for the Regulator class.
        ///</summary>
        public Regulator()
        {
            _nextUpdateTime = Time.TimeNowMs +
                TorqueUtil.GetRandomFloat(1000.0f);
        }

        ///<summary>
        ///constructor for the Regulator class.
        ///</summary>
        ///<param name="updatesPerSec">
        ///number of times to update per second
        ///</param>
        public Regulator(float updatesPerSec)
            : this()
        {
            UpdatesPerSec = updatesPerSec;
        }

        ///<summary>
        ///constructor for the Regulator class.
        ///</summary>
        ///<param name="updatesPerSec">
        ///number of times to update per second
        ///</param>
        ///<param name="updatePeriodVariator">
        ///parameter for randomly varying the updates per second
        ///</param>
        public Regulator(float updatesPerSec, float updatePeriodVariator)
            : this(updatesPerSec)
        {
            UpdatePeriodVariator = updatePeriodVariator;
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///Is the regulator ready
        ///</summary>
        public bool IsReady
        {
            get
            {
                //if a regulator is instantiated with a zero freq then it goes
                //into stealth mode (doesn't regulate)
                if (UpdatePeriod == 0.0f) return true;

                //if the regulator is instantiated with a negative freq then it
                //will never allow the code to flow
                if (UpdatePeriod < 0.0f) return false;

                float currentTime = Time.TimeNowMs;

                if (currentTime >= _nextUpdateTime)
                {
                    _nextUpdateTime = currentTime + UpdatePeriod +
                        TorqueUtil.GetRandomFloat(
                            -UpdatePeriodVariator,
                            UpdatePeriodVariator);

                    return true;
                }

                return false;
            }
        }

        //[TorqueXmlSchemaType(DefaultValue = "10.0")]
        ///<summary>
        ///Number of updates per second
        ///</summary>
        public float UpdatesPerSec
        {
            get
            {
                if (0.0f == UpdatePeriod)
                {
                    return 0.0f;
                }

                if (UpdatePeriod < 0.0f)
                {
                    return -1.0f;
                }

                return 1000.0f / UpdatePeriod;
            }
            set
            {
                if (value > 0.0f)
                {
                    UpdatePeriod = 1000.0f / value;
                }
                else if (value == 0.0f)
                {
                    UpdatePeriod = 0.0f;
                }
                else if (value < 0.0f)
                {
                    UpdatePeriod = -1.0f;
                }
            }
        }

        //[XmlIgnore]
        ///<summary>
        ///Update period
        ///</summary>
        public float UpdatePeriod
        {
            get { return _updatePeriod; }
            set { _updatePeriod = value; }
        }

        //[TorqueXmlSchemaType(DefaultValue = "0.0")]
        ///<summary>
        ///Parameter for randomly varying the updates per second
        ///</summary>
        public float UpdatePeriodVariator
        {
            get { return _updatePeriodVariator; }
            set { _updatePeriodVariator = value; }
        }

        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        //the time period between updates 
        float _updatePeriod;

        //the number of milliseconds the update period can vary per required
        //update-step. This is here to make sure any multiple clients of this
        //class have their updates spread evenly
        float _updatePeriodVariator;

        //the next time the regulator allows code flow
        float _nextUpdateTime;

        #endregion
    }
}