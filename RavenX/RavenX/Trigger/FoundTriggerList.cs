using System.Collections.Generic;
using MapContent;
using Microsoft.Xna.Framework;

namespace Mindcrafters.RavenX.Trigger
{
    /// <summary>
    /// This is a list of triggers that we have found on the map
    /// </summary>
    public class FoundTriggerList
    {
        #region Public methods, fields, constructors

        /// <summary>
        /// the list that holds all the triggers, initially empty
        /// </summary>
        private readonly List<Trigger> _triggerList = new List<Trigger>();

        /// <summary>
        /// main constructor
        /// </summary>
        public FoundTriggerList(GameManager.GameManager.Teams ownerTeam)
        {
            _ownerTeam = ownerTeam;
            Reset();
        }

        /// <summary>
        /// current list of items
        /// </summary>
        public List<Trigger> List
        {
            get { return _triggerList; }
        }

        /// <summary>
        /// the count of the discovered flags
        /// </summary>
        public int NumberOfFlags
        {
            get { return _numberOfFlags; }
        }

        /// <summary>
        /// the count of the discovered health items
        /// </summary>
        public int NumberOfHealthItems
        {
            get { return _numberOfHealthItems; }
        }

        /// <summary>
        /// the count of the discovered rocket launchers
        /// </summary>
        public int NumberOfRocketLaunchers
        {
            get { return _numberOfRocketLaunchers; }
        }

        /// <summary>
        /// the count of the discovered railguns
        /// </summary>
        public int NumberOfRailguns
        {
            get { return _numberOfRailguns; }
        }

        /// <summary>
        /// the count of the discovered blasters
        /// </summary>
        public int NumberOfBlasters
        {
            get { return _numberOfBlasters; }
        }

        /// <summary>
        /// the count of the discovered shotguns
        /// </summary>
        public int NumberOfShotguns
        {
            get { return _numberOfShotguns; }
        }

        /// <summary>
        /// have we discovered our own flag yet
        /// </summary>
        public bool IsOurFlagDiscovered
        {
            get { return _ourFlagIsDiscovered; }
        }

        /// <summary>
        /// count of a given weapon type discovered
        /// </summary>
        public int NumberOfWeaponsByType(WeaponTypes type)
        {
            switch (type)
            {
                case WeaponTypes.Blaster:
                    return NumberOfBlasters;
                case WeaponTypes.Railgun:
                    return NumberOfRailguns;
                case WeaponTypes.RocketLauncher:
                    return NumberOfRocketLaunchers;
                case WeaponTypes.Shotgun:
                    return NumberOfShotguns;
                default:
                    return 1;
            }
        }

        #endregion

        #region Public methods

        public Vector2 LocationOfOurFlag
        {
            get { return _locationOfOurFlag; }
        }

        public void Flush()
        {
            _triggerList.Clear();
            Reset();
        }

        /// <summary>
        /// Add a new trigger to the list
        /// </summary>
        /// <param name="foundTrigger">newly found trigger</param>
        public void AddFoundItem(Trigger foundTrigger)
        {
            if (IsAddableType(foundTrigger.EntityType) && !_triggerList.Contains(foundTrigger))
            {
                _triggerList.Add(foundTrigger);
                countListItems();
            }
        }

        public Vector2? LocationOfClosestEnemyFlag(Vector2 sourceLocation)
        {
            float closestDistance = float.MaxValue;
            Vector2? closestFlagLocation = null;
            foreach (Trigger trigger in _triggerList)
            {
                if (trigger.EntityType == EntityTypes.Flag && !trigger.Name.Contains(_ownerTeam.ToString()))
                {
                    float distance = Vector2.Distance(trigger.Position, sourceLocation);
                    if (distance < closestDistance)
                    {
                        closestFlagLocation = trigger.Position;
                        closestDistance = distance;
                    }
                }
            }
            return closestFlagLocation;
        }

        #endregion

        #region Private, protected, internal methods

        private static bool IsAddableType(EntityTypes type)
        {
            if (type == EntityTypes.Health)
                return true;
            if (type == EntityTypes.Shotgun)
                return true;
            if (type == EntityTypes.Blaster)
                return true;
            if (type == EntityTypes.Railgun)
                return true;
            if (type == EntityTypes.RocketLauncher)
                return true;
            if (type == EntityTypes.Flag)
                return true;
            return false;
        }

        /// <summary>
        /// count the number of each type of item in the list
        /// </summary>
        private void countListItems()
        {
            Reset();
            foreach (Trigger trigger in _triggerList)
            {
                switch (trigger.EntityType)
                {
                    case EntityTypes.Health:
                        _numberOfHealthItems++;
                        break;
                    case EntityTypes.RocketLauncher:
                        _numberOfRocketLaunchers++;
                        break;
                    case EntityTypes.Blaster:
                        _numberOfBlasters++;
                        break;
                    case EntityTypes.Shotgun:
                        _numberOfShotguns++;
                        break;
                    case EntityTypes.Railgun:
                        _numberOfRailguns++;
                        break;
                    case EntityTypes.Flag:
                        {
                            if (trigger.Name.Contains(_ownerTeam.ToString()))
                            {
                                _ourFlagIsDiscovered = true;
                                _locationOfOurFlag = trigger.Position;
                            }
                            _numberOfFlags++;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Reset all counter variables
        /// </summary>
        private void Reset()
        {
            _numberOfBlasters = 0;
            _numberOfHealthItems = 0;
            _numberOfRailguns = 0;
            _numberOfRocketLaunchers = 0;
            _numberOfShotguns = 0;
            _numberOfFlags = 0;
            _ourFlagIsDiscovered = false;
        }

        #endregion

        #region Private, protected, internal fields

        private readonly GameManager.GameManager.Teams _ownerTeam;
        private Vector2 _locationOfOurFlag;
        private int _numberOfBlasters;
        private int _numberOfFlags;
        private int _numberOfHealthItems;
        private int _numberOfRailguns;
        private int _numberOfRocketLaunchers;
        private int _numberOfShotguns;
        private bool _ourFlagIsDiscovered;

        #endregion
    }
}