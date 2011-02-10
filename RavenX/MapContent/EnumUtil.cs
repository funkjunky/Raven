#region File description

//------------------------------------------------------------------------------
//EnumUtil.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.ComponentModel;
using System.Reflection;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///Entity types
    ///</summary>
    public enum EntityTypes
    {
        [Description("Default entity type")] DefaultEntityType = -1,
        Wall,
        Bot,
        Unused,
        Waypoint,
        Health,
        [Description("Spawn point")] SpawnPoint,
        Railgun,
        [Description("Rocket launcher")] RocketLauncher,
        Shotgun,
        Blaster,
        Obstacle,
        [Description("Sliding door")] SlidingDoor,
        [Description("Door Trigger")] DoorTrigger,
        [Description("Flag")] Flag
    } ;

    ///<summary>
    ///Item types
    ///</summary>
    public enum ItemTypes
    {
        Blaster,
        Health,
        Railgun,
        [Description("Rocket launcher")] RocketLauncher,
        Shotgun,
        Flag
    }

    ///<summary>
    ///Weapon types
    ///</summary>
    public enum WeaponTypes
    {
        Blaster,
        Railgun,
        [Description("Rocket launcher")] RocketLauncher,
        Shotgun,
    }

    ///<summary>
    ///Message types
    ///</summary>
    public enum MessageTypes
    {
        Blank,
        [Description("Path is ready")] PathReady,
        [Description("No path is available")] NoPathAvailable,
        [Description("Take that, My Friend")] TakeThatMF,
        [Description("Ouch")] YouGotMeYouSOB,
        [Description("The goal queue is empty")] GoalQueueEmpty,
        [Description("Open sesame")] OpenSesame,
        [Description("Bang")] GunshotSound,
        [Description("User has removed a bot from the game")] UserHasRemovedBot
    } ;

    ///<summary>
    ///A helper class for dealing with Enums
    ///</summary>
    public class EnumUtil
    {
        ///<summary>
        ///Get the string representation of the enum value
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                                             typeof (DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}