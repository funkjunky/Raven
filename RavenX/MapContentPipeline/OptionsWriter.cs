#region File description

//------------------------------------------------------------------------------
//OptionsWriter.cs
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
    ///Content Pipeline class for saving Wall data into XNB format.
    ///</summary>
    [ContentTypeWriter]
    public class OptionsWriter : ContentTypeWriter<OptionsData>
    {
        ///<summary>
        ///Compiles a strongly typed object into binary format.
        ///</summary>
        ///<param name="output">
        ///The content writer serializing the value.
        ///</param>
        ///<param name="value">The value to write.</param>
        protected override void Write(ContentWriter output, OptionsData value)
        {
            output.Write(value.ShowGraph);
            output.Write(value.ShowNodeIndices);
            output.Write(value.ShowPathOfSelectedBot);
            output.Write(value.ShowTargetOfSelectedBot);
            output.Write(value.ShowOpponentsSensedBySelectedBot);
            output.Write(value.OnlyShowBotsInTargetsFOV);
            output.Write(value.ShowGoalsOfSelectedBot);
            output.Write(value.ShowGoalAppraisals);
            output.Write(value.ShowWeaponAppraisals);
            output.Write(value.SmoothPathsQuick);
            output.Write(value.SmoothPathsPrecise);
            output.Write(value.ShowBotIds);
            output.Write(value.ShowBotHealth);
            output.Write(value.ShowScore);
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime target type.
        ///</summary>
        ///<param name="targetPlatform">The target platform.</param>
        ///<returns>The qualified name.</returns>
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "MapContent.OptionsData, MapContent";
        }

        ///<summary>
        ///Gets the assembly qualified name of the runtime loader for this type.
        ///</summary>
        ///<param name="targetPlatform">Name of the platform.</param>
        ///<returns>Name of the runtime loader.</returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "MapContent.OptionsReader, MapContent";
        }
    }
}