#region File description

//------------------------------------------------------------------------------
//MapReader.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

    #endregion

    #region Microsoft

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///Content Pipeline class for loading Map data from XNB format.
    ///</summary>
    public class MapReader : ContentTypeReader<MapData>
    {
        ///<summary>
        ///Reads a strongly typed object from the current stream.
        ///</summary>
        ///<param name="input">
        ///The <see cref="ContentReader"/> used to read the object.
        ///</param>
        ///<param name="existingInstance">
        ///An existing object to read into.
        ///</param>
        ///<returns>The type of object to read.</returns>
        protected override MapData Read(
            ContentReader input, MapData existingInstance)
        {
            MapData data = new MapData();

            data.Name = input.ReadString();
            data.NodeList = input.ReadObject<List<NodeData>>();
            data.EdgeList = input.ReadObject<List<EdgeData>>();
            data.SizeX = input.ReadInt32();
            data.SizeY = input.ReadInt32();
            data.WallList = input.ReadObject<List<WallData>>();
            data.DoorList = input.ReadObject<List<DoorData>>();
            data.DoorTriggerList = input.ReadObject<List<DoorTriggerData>>();
            data.SpawnPointList = input.ReadObject<List<SpawnPointData>>();
            data.HealthList = input.ReadObject<List<HealthData>>();
            data.RailgunList = input.ReadObject<List<RailgunData>>();
            data.RocketLauncherList =
                input.ReadObject<List<RocketLauncherData>>();
            data.ShotgunList = input.ReadObject<List<ShotgunData>>();

            return data;
        }
    }
}