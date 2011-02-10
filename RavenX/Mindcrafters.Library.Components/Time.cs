#region File description
//------------------------------------------------------------------------------
// Time.cs
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
    #region Class Time

    /// <summary>
    /// time keeper to measure time in seconds
    /// </summary>
    public class Time
    {
        public static float StartTime = MindcraftersComponentLibrary.Engine.RealTime;
        public static float TimeNow
        {
            get { return TimeNowMs * 0.001f; }
        }

        public static float TimeNowMs
        {
            get { return MindcraftersComponentLibrary.Engine.RealTime - StartTime; }
        }
    }

    #endregion
}
