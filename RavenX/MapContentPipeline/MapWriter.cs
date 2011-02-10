#region File description

//------------------------------------------------------------------------------
//MapWriter.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

#endregion

#endregion

namespace MapContentPipeline
{
    ///<summary>
    ///Content Pipeline class for saving Map data into XNB format.
    ///</summary>
    [ContentTypeWriter]
    public class MapWriter : ContentTypeWriter<MapData>
    {
        ///<summary>
        ///Compiles a strongly typed object into binary format.
        ///</summary>
        ///<param name="output">
        ///The content writer serializing the value.
        ///</param>
        ///<param name="value">The value to write.</param>
        protected override void Write(ContentWriter output, MapData value)
        {
            output.Write(value.Name);
            output.WriteObject(value.NodeList);
            output.WriteObject(value.EdgeList);
            output.Write(value.SizeX);
            output.Write(value.SizeY);
            output.WriteObject(value.WallList);
            output.WriteObject(value.DoorList);
            output.WriteObject(value.DoorTriggerList);
            output.WriteObject(value.SpawnPointList);
            output.WriteObject(value.HealthList);
            output.WriteObject(value.RailgunList);
            output.WriteObject(value.RocketLauncherList);
            output.WriteObject(value.ShotgunList);
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime target type.
        ///</summary>
        ///<param name="targetPlatform">The target platform.</param>
        ///<returns>The qualified name.</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MapContent.MapReader, MapContent";
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime loader for this type.
        ///</summary>
        ///<param name="targetPlatform">Name of the platform.</param>
        ///<returns>Name of the runtime loader.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MapContent.MapReader, MapContent";
        }
    }
}