#region File description

//------------------------------------------------------------------------------
//OptionsData.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

    #endregion

    #region Microsoft

#if !XBOX

#endif

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace MapContent
{
    ///<summary>
    ///class for options data
    ///</summary>
    public class OptionsData
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///should we draw the graph (draw nodes and edges)
        ///</summary>
        public bool ShowGraph
        {
            get { return _showGraph; }
            set { _showGraph = value; }
        }

        ///<summary>
        ///should we label nodes with their index
        ///</summary>
        public bool ShowNodeIndices
        {
            get { return _showNodeIndices; }
            set { _showNodeIndices = value; }
        }

        ///<summary>
        ///should we draw the path of the selected bot
        ///</summary>
        public bool ShowPathOfSelectedBot
        {
            get { return _showPathOfSelectedBot; }
            set { _showPathOfSelectedBot = value; }
        }

        ///<summary>
        ///should we draw an indication of the target of the selected bot
        ///</summary>
        public bool ShowTargetOfSelectedBot
        {
            get { return _showTargetOfSelectedBot; }
            set { _showTargetOfSelectedBot = value; }
        }

        ///<summary>
        ///should we show opponents sensed by the selected bot
        ///</summary>
        public bool ShowOpponentsSensedBySelectedBot
        {
            get { return _showOpponentsSensedBySelectedBot; }
            set { _showOpponentsSensedBySelectedBot = value; }
        }

        ///<summary>
        ///should we only show bots is target's FOV
        ///</summary>
        public bool OnlyShowBotsInTargetsFOV
        {
            get { return _onlyShowBotsInTargetsFOV; }
            set { _onlyShowBotsInTargetsFOV = value; }
        }

        ///<summary>
        ///should we show goals of the selected bot
        ///</summary>
        public bool ShowGoalsOfSelectedBot
        {
            get { return _showGoalsOfSelectedBot; }
            set { _showGoalsOfSelectedBot = value; }
        }

        ///<summary>
        ///should we show goal appraisals
        ///</summary>
        public bool ShowGoalAppraisals
        {
            get { return _showGoalAppraisals; }
            set { _showGoalAppraisals = value; }
        }

        ///<summary>
        ///should we show weapon appraisals
        ///</summary>
        public bool ShowWeaponAppraisals
        {
            get { return _showWeaponAppraisals; }
            set { _showWeaponAppraisals = value; }
        }

        ///<summary>
        ///should we use quick path smoothing
        ///</summary>
        public bool SmoothPathsQuick
        {
            get { return _smoothPathsQuick; }
            set { _smoothPathsQuick = value; }
        }

        ///<summary>
        ///should we use precise (slower) path smoothing
        ///</summary>
        public bool SmoothPathsPrecise
        {
            get { return _smoothPathsPrecise; }
            set { _smoothPathsPrecise = value; }
        }

        ///<summary>
        ///should we show bot ids
        ///</summary>
        public bool ShowBotIds
        {
            get { return _showBotIds; }
            set { _showBotIds = value; }
        }

        ///<summary>
        ///should we show bot health
        ///</summary>
        public bool ShowBotHealth
        {
            get { return _showBotHealth; }
            set { _showBotHealth = value; }
        }

        ///<summary>
        ///should we show the bot's score
        ///</summary>
        public bool ShowScore
        {
            get { return _showScore; }
            set { _showScore = value; }
        }

        #region Public methods

        ///<summary>
        ///set default option values
        ///</summary>
        public void SetDefaults()
        {
            ShowGraph = false;
            ShowNodeIndices = false;
            ShowPathOfSelectedBot = true;
            ShowTargetOfSelectedBot = false;
            ShowOpponentsSensedBySelectedBot = false;
            OnlyShowBotsInTargetsFOV = false;
            ShowGoalsOfSelectedBot = true;
            ShowGoalAppraisals = true;
            ShowWeaponAppraisals = true;
            SmoothPathsQuick = false;
            SmoothPathsPrecise = false;
            ShowBotIds = false;
            ShowBotHealth = true;
            ShowScore = false;
        }

#if !XBOX
        ///<summary>
        ///write data to xml file
        ///</summary>
        ///<param name="xmlOptionsFilename"></param>
        public void SaveToXml(string xmlOptionsFilename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(xmlOptionsFilename, settings))
            {
                IntermediateSerializer.Serialize(xmlWriter, this, null);
            }
        }
#endif

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private bool _onlyShowBotsInTargetsFOV;
        private bool _showBotHealth;
        private bool _showBotIds;
        private bool _showGoalAppraisals;
        private bool _showGoalsOfSelectedBot;
        private bool _showGraph;
        private bool _showNodeIndices;
        private bool _showOpponentsSensedBySelectedBot;
        private bool _showPathOfSelectedBot;
        private bool _showScore;
        private bool _showTargetOfSelectedBot;
        private bool _showWeaponAppraisals;
        private bool _smoothPathsPrecise;
        private bool _smoothPathsQuick;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}