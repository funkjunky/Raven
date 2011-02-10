#region File description

//------------------------------------------------------------------------------
//NodeReader.cs
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
    ///Content Pipeline class for loading Node data from XNB format.
    ///</summary>
    public class NodeReader : ContentTypeReader<NodeData>
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
        protected override NodeData Read(
            ContentReader input, NodeData existingInstance)
        {
            NodeData data = new NodeData();

            data.Name = input.ReadString();
            data.Index = input.ReadInt32();
            data.Position = input.ReadVector2();

            return data;
        }
    }
}