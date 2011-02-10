#region File description

//------------------------------------------------------------------------------
//DoorReader.cs
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
    ///Content Pipeline class for loading Door data from XNB format.
    ///</summary>
    public class DoorReader : ContentTypeReader<DoorData>
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
        protected override DoorData Read(
            ContentReader input, DoorData existingInstance)
        {
            DoorData data = new DoorData();

            data.Name = input.ReadString();
            data.From = input.ReadVector2();
            data.To = input.ReadVector2();
            data.TriggerList = input.ReadObject<List<string>>();

            return data;
        }
    }
}