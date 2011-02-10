#region File description

//------------------------------------------------------------------------------
//FindActiveTrigger.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

using MapContent;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Graph;

namespace Mindcrafters.RavenX.Navigation
{
    ///<summary>
    ///class for implementing test for successful search for an active trigger
    ///</summary>
    public class FindActiveTrigger
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Tests if the current node is linked to an active trigger of the
        ///desired type. Used for search termination.
        ///</summary>
        ///<param name="graph"></param>
        ///<param name="target"></param>
        ///<param name="currentNodeIdx"></param>
        ///<param name="bot">bot to verify against</param>
        ///<returns></returns>
        public static bool IsSatisfied(
            SparseGraph graph,
            //TODO: should not use target as node index and entity type
            int target,
            int currentNodeIdx,
            BotEntity bot)
        {
            bool bSatisfied = false;
            EntityTypes targetEntityType =
                Entity.Entity.ItemTypeToEntityType((ItemTypes) target);

            //get a reference to the node at the given node index
            NavGraphNode node = graph.GetNode(currentNodeIdx);

            //if the extrainfo field is pointing to a giver-trigger, test to
            //make sure it is active and that it is of the correct type.
            Trigger.Trigger t = node.ExtraInfo as Trigger.Trigger;
            if (t != null && t.IsActive && t.EntityType == targetEntityType && bot.FoundTriggers.List.Contains(t))
            {
                bSatisfied = true;
            }

            return bSatisfied;
        }

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}