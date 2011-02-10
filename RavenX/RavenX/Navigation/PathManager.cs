#region File description

//------------------------------------------------------------------------------
//PathManager.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Navigation
{
    ///<summary>
    ///a class to manage a number of graph searches, and to 
    ///distribute the calculation of each search over several update-steps
    ///</summary>
    public class PathManager
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///TODO: could add time resource limit
        ///</summary>
        ///<param name="numCyclesPerUpdate"></param>
        public PathManager(int numCyclesPerUpdate)
        {
            _numSearchCyclesPerUpdate = numCyclesPerUpdate;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///the number of path requests currently active.
        ///</summary>
        public int NumActiveSearches
        {
            get { return SearchRequests.Count; }
        }

        ///<summary>
        ///a list of all the active search requests
        ///</summary>
        public List<PathPlanner> SearchRequests
        {
            get { return _searchRequests; }
        }

        ///<summary>
        ///this is the total number of search cycles allocated to the manager. 
        ///Each update-step these are divided equally amongst all registered
        ///path requests
        ///TODO: could add priority to search requests
        ///</summary>
        public int NumSearchCyclesPerUpdate
        {
            get { return _numSearchCyclesPerUpdate; }
        }

        #region Public methods

        ///<summary>
        ///This method iterates through all the active path planning requests 
        ///updating their searches until the user specified total number of
        ///search cycles has been satisfied.
        ///
        ///If a path is found or the search is unsuccessful the relevant agent
        ///is notified accordingly by Telegram
        ///</summary>
        public void UpdateSearches()
        {
            int numCyclesRemaining = NumSearchCyclesPerUpdate;

            //iterate through the search requests until either all requests have
            //been fulfilled or there are no search cycles remaining for this
            //update-step.
            int curPathIndex = 0;
            while (numCyclesRemaining-- > 0 && SearchRequests.Count > 0)
            {
                //make one search cycle of this path request
                int result = SearchRequests[curPathIndex].CycleOnce();

                //if the search has terminated remove from the list
                if ((result == (int) GraphSearchTimeSliced.SearchResults.TargetFound) ||
                    (result == (int) GraphSearchTimeSliced.SearchResults.TargetNotFound))
                {
                    //remove this path from the path list
                    SearchRequests.RemoveAt(curPathIndex);
                }
                    //move on to the next
                else
                {
                    ++curPathIndex;
                }

                //the iterator may now be pointing to the end of the list. If
                //this is so, it must be reset to the beginning.
                if (curPathIndex >= SearchRequests.Count)
                {
                    curPathIndex = 0;
                }
            }
        }

        ///<summary>
        ///this is called to register a search with the path manager.
        ///</summary>
        ///<param name="pathPlanner"></param>
        public void Register(PathPlanner pathPlanner)
        {
            //make sure the bot does not already have a current search
            if (!SearchRequests.Contains(pathPlanner))
            {
                //add to the list
                SearchRequests.Add(pathPlanner);
            }
        }

        ///<summary>
        ///this is called to unregister a search with the path manager.
        ///</summary>
        ///<param name="pathPlanner"></param>
        public void UnRegister(PathPlanner pathPlanner)
        {
            SearchRequests.Remove(pathPlanner);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly int _numSearchCyclesPerUpdate;

        private readonly List<PathPlanner> _searchRequests =
            new List<PathPlanner>();

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}