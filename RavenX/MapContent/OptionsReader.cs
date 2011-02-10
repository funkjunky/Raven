#region File description

//------------------------------------------------------------------------------
//OptionsReader.cs
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
    public class OptionsReader : ContentTypeReader<OptionsData>
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
        protected override OptionsData Read(
            ContentReader input, OptionsData existingInstance)
        {
            OptionsData data = new OptionsData();

            data.ShowGraph = input.ReadBoolean();
            data.ShowNodeIndices = input.ReadBoolean();
            data.ShowPathOfSelectedBot = input.ReadBoolean();
            data.ShowTargetOfSelectedBot = input.ReadBoolean();
            data.ShowOpponentsSensedBySelectedBot = input.ReadBoolean();
            data.OnlyShowBotsInTargetsFOV = input.ReadBoolean();
            data.ShowGoalsOfSelectedBot = input.ReadBoolean();
            data.ShowGoalAppraisals = input.ReadBoolean();
            data.ShowWeaponAppraisals = input.ReadBoolean();
            data.SmoothPathsQuick = input.ReadBoolean();
            data.SmoothPathsPrecise = input.ReadBoolean();
            data.ShowBotIds = input.ReadBoolean();
            data.ShowBotHealth = input.ReadBoolean();
            data.ShowScore = input.ReadBoolean();

            return data;
        }
    }
}