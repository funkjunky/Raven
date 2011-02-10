#region File description

//------------------------------------------------------------------------------
//Trigger.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Graph;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Trigger
{
    ///<summary>
    ///base class for triggers
    ///</summary>
    public abstract class Trigger : Entity.Entity
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="sceneObject"></param>
        protected Trigger(IEntitySceneObject sceneObject)
            : base(sceneObject)
        {
            MarkForDelete = false;
            IsActive = true;
            NodeIndex = GraphNode.INVALID_NODE_INDEX;
            RegionOfInfluence = null;
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///some types of trigger are twinned with a graph node. This enables
        ///the pathfinding component of an AI to search a navgraph for a
        ///specific type of trigger.
        ///</summary>
        public int NodeIndex
        {
            get { return _nodeIndex; }
            protected set { _nodeIndex = value; }
        }

        ///<summary>
        ///if this is true the trigger will be removed from the game
        ///</summary>
        public bool MarkForDelete
        {
            get { return _markForDelete; }
            protected set { _markForDelete = value; }
        }

        ///<summary>
        ///it's convenient to be able to deactivate certain types of triggers
        ///on an event. Therefore a trigger can only be triggered when this
        ///value is true (respawning triggers make good use of this facility)
        ///</summary>
        public bool IsActive
        {
            get { return _isActive; }
            protected set
            {
                _isActive = value;
                SceneObject.Visible = value;
            }
        }

        ///<summary>
        ///Every trigger owns a trigger region. If an entity comes within this 
        ///region the trigger is activated
        ///</summary>
        public TriggerRegion RegionOfInfluence
        {
            get { return _regionOfInfluence; }
            protected set { _regionOfInfluence = value; }
        }

        ///<summary>
        ///the bot that set of the trigger
        ///</summary>
        public BotEntity TriggeringBot
        {
            get { return _triggeringBot; }
            protected set { _triggeringBot = value; }
        }

        #region Public methods

        ///<summary>
        ///when this is called the trigger determines if the entity is within
        ///the trigger's region of influence. If it is then the trigger will be 
        ///triggered and the appropriate action will be taken.
        ///</summary>
        ///<param name="bot">entity activating the trigger</param>
        public abstract void Try(BotEntity bot);

        ///<summary>
        ///called each update-step of the game. This methods updates any
        ///internal state the trigger may have.
        ///</summary>
        ///<param name="dt">time since last update</param>
        public abstract override void Update(float dt);

        ///<summary>
        ///Add a circular trigger activation region
        ///</summary>
        ///<param name="center"></param>
        ///<param name="radius"></param>
        public void AddCircularTriggerRegion(Vector2 center, float radius)
        {
            RegionOfInfluence = new TriggerRegionCircle(center, radius);
        }

        ///<summary>
        ///Add a triangular trigger activation region
        ///</summary>
        ///<param name="topLeft"></param>
        ///<param name="bottomRight"></param>
        public void AddRectangularTriggerRegion(
            Vector2 topLeft,
            Vector2 bottomRight)
        {
            RegionOfInfluence =
                new TriggerRegionRectangle(topLeft, bottomRight);
        }

        ///<summary>
        ///Is the entity at the given position with the given radius touching
        ///the trigger region
        ///</summary>
        ///<param name="entityPosition"></param>
        ///<param name="entityRadius"></param>
        ///<returns></returns>
        public bool IsTouchingTrigger(
            Vector2 entityPosition,
            float entityRadius)
        {
            return RegionOfInfluence != null &&
                   RegionOfInfluence.IsTouching(entityPosition, entityRadius);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private bool _isActive;
        private bool _markForDelete;
        private int _nodeIndex;
        private TriggerRegion _regionOfInfluence;
        private BotEntity _triggeringBot;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}