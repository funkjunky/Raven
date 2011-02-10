#region File description

//------------------------------------------------------------------------------
//RailgunReader.cs
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
    ///Content Pipeline class for loading Railgun data from XNB format.
    ///</summary>
    public class RailgunReader : ContentTypeReader<RailgunData>
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
        protected override RailgunData Read(
            ContentReader input, RailgunData existingInstance)
        {
            RailgunData data = new RailgunData();

            data.Name = input.ReadString();
            data.Position = input.ReadVector2();
            data.Radius = input.ReadSingle();
            data.NodeIndex = input.ReadInt32();

            return data;
        }
    }
}