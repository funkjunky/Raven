#region File description

//------------------------------------------------------------------------------
//EdgeReader.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework.Content;

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///Content Pipeline class for loading Edge data from XNB format.
    ///</summary>
    public class EdgeReader : ContentTypeReader<EdgeData>
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
        protected override EdgeData Read(
            ContentReader input, EdgeData existingInstance)
        {
            EdgeData data = new EdgeData();

            data.Name = input.ReadString();
            data.FromIndex = input.ReadInt32();
            data.ToIndex = input.ReadInt32();
            data.Cost = input.ReadSingle();
            data.BehaviorType =
                (EdgeData.BehaviorTypes) input.ReadInt32();
            //input.ReadObject<EdgeData.BehaviorTypes>();
            data.NameOfIntersectingEntity = input.ReadString();

            return data;
        }
    }
}