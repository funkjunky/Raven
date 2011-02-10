#region File description

//------------------------------------------------------------------------------
//Program.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using MapContent;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContentPipeline
{
    ///<summary>
    ///main program class (useful for debugging library)
    ///</summary>
    public static class Program
    {
        ///<summary>
        ///main class
        ///</summary>
        public static void Main()
        {
            #region Map Conversion

            //you can use this to convert Raven .map files to
            //RavenX .xml files
            string[] ravenMapFilenames =
                {
                    @"..\..\..\..\RavenX\data\maps\RavenMap.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoors.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoorsAndItems.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMap.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoors.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoorsAndItems.map",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoorsAndItems.map"
                };

            string[] xmlMapFilenames =
                {
                    @"..\..\..\..\RavenX\data\maps\RavenMap.xml",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoors.xml",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoorsAndItems.xml",
                    @"..\..\..\..\RavenX\data\maps\RavenMap2.xml",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoors2.xml",
                    @"..\..\..\..\RavenX\data\maps\RavenMapWithDoorsAndItems2.xml",
                    @"..\..\..\..\RavenX\data\maps\mindcrafters.xml"
                };

            float[] scales =
                {
                    1.0f,
                    1.0f,
                    1.0f,
                    2.0f,
                    2.0f,
                    2.0f,
                    1.5f
                };

            int numToConvert =
                Math.Max(ravenMapFilenames.Length,
                         Math.Max(xmlMapFilenames.Length, scales.Length));

            for (int i = 0; i < numToConvert; i++)
            {
                MapData.Convert(ravenMapFilenames[i], xmlMapFilenames[i], scales[i]);
            }

            #endregion

            #region Options

            OptionsData optionsData = new OptionsData();

            optionsData.SetDefaults();

            optionsData.SaveToXml(@"..\..\..\..\RavenX\data\maps\Options.xml");

            #endregion

            #region Parameters

            ParameterData parameterData = new ParameterData();

            parameterData.SetDefaults();

            parameterData.SaveToXml(@"..\..\..\..\RavenX\data\maps\Parameters.xml");

            #endregion
        }
    }
}