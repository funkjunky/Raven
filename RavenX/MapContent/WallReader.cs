#region File description

//------------------------------------------------------------------------------
//WallReader.cs
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
    ///Content Pipeline class for loading Wall data from XNB format.
    ///</summary>
    public class WallReader : ContentTypeReader<WallData>
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
        protected override WallData Read(
            ContentReader input, WallData existingInstance)
        {
            WallData data = new WallData();

            data.Name = input.ReadString();
            data.From = input.ReadVector2();
            data.To = input.ReadVector2();
            data.Normal = input.ReadVector2();

            return data;
        }
    }
}