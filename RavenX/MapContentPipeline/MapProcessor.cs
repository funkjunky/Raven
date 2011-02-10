#region File description

//------------------------------------------------------------------------------
//MapWriter.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region Microsoft

using MapContent;
using Microsoft.Xna.Framework.Content.Pipeline;

#endregion

#endregion

namespace MapContentPipeline
{
    ///<summary>
    ///Custom Content Pipeline processor class for map data.
    ///</summary>
    [ContentProcessor]
    public class MapProcessor : ContentProcessor<MapData, MapData>
    {
        ///<summary>
        ///Processes the specified input data and returns the result.
        ///</summary>
        ///<param name="input"> Existing content object being processed.</param>
        ///<param name="context">
        ///Contains any required custom process parameters.
        ///</param>
        ///<returns>A typed object representing the processed input.</returns>
        public override MapData Process(MapData input, ContentProcessorContext context)
        {
            return input;
        }
    }
}