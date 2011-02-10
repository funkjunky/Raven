#region File description

//------------------------------------------------------------------------------
//GameManager.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;


#endregion

#region Microsoft

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using GarageGames.Torque.Platform;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;
#endregion

#region Mindcrafters
using Mindcrafters.Library.Components;
using Mindcrafters.Library.Math;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Armory;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Entity.Items;
using Mindcrafters.RavenX.Entity.Projectiles;
using Mindcrafters.RavenX.Map;
using Mindcrafters.RavenX.Messaging;
using Mindcrafters.RavenX.Navigation;
using Mindcrafters.Tx2D.GameAI;
using MapContent;
#endregion

#endregion

namespace Mindcrafters.RavenX.GameManager
{
    ///<summary>
    ///this class creates and stores all the entities that make up the
    ///Raven game environment. (walls, bots, health etc) and can read a
    ///Raven map file and recreate the necessary geometry.
    ///
    ///this class has methods for updating the game entities and for
    ///rendering them.
    ///</summary>
    public class GameManager
    {
        #region Static methods, fields, constructors

        #region Singleton pattern

        private static GameManager _instance;

        ///<summary>
        ///Accessor for the GameManager singleton instance.
        ///</summary>
        public static GameManager Instance
        {
            get
            {
                if (null == _instance)
                    new GameManager();

                Assert.Fatal(null != _instance,
                             "Singleton instance not set by constructor.");
                return _instance;
            }
        }

        #endregion

        #endregion

        #region Constructors

        ///<summary>
        ///constructor with default map
        ///</summary>
        public GameManager()
            : this(@"data\maps\mindcrafters")
        {
        }

        ///<summary>
        ///constructor with map file parameter
        ///</summary>
        ///<param name="filename"></param>
        public GameManager(string filename)
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;

            _selectedBot = null;
            MyGame.Instance.GamePaused = false;
            RemoveABot = false;
            _map = null;
            _pathManager = null;

            _botList = new List<BotEntity>();
            _projectiles = new List<Projectile>();
            _graveMarkerList = new List<GraveMarker>();

            _minimapComponentList = new List<MinimapComponent>();

            ////4 upper right minimaps

            //     _minimapComponentList.Add(CreateMinimapComponent(
            //       new Vector2(0, 0),
            //      new Vector2(32.0f, 32.0f),
            //     new Vector2(64.0f, 0.0f),
            //    new Vector2(128.0f, 128.0f),
            //   true));
            //   _minimapComponentList.Add(CreateMinimapComponent(
            //    new Vector2(0, 0),
            //  new Vector2(64.0f, 64.0f),
            //   new Vector2(768.0f, 0.0f),
            //  new Vector2(256.0f, 256.0f),
            //  true
            // ));
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(512.0f, 256.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true
            //   ));
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(768.0f, 256.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true
            //   ));

            ////4 lower left minimaps
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(0.0f, 512.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true));
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(256.0f, 512.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true
            //   ));
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(0.0f, 768.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true
            //   ));
            //_minimapComponentList.Add(CreateMinimapComponent(
            //   new Vector2(0, 0),
            //   new Vector2(64.0f, 64.0f),
            //   new Vector2(256.0f, 768.0f),
            //   new Vector2(256.0f, 256.0f),
            //   true
            //   ));

            try
            {
                Options = MyGame.Instance.Content.Load<OptionsData>(@"data\maps\Options");
            }
            catch (Exception e)
            {
                Assert.Fatal(false,
                             "GameManager.GameManager: Bad options filename -> " + e.Message);
                MyGame.Instance.Exit();
            }

            try
            {
                Parameters = MyGame.Instance.Content.Load<ParameterData>(@"data\maps\Parameters");
            }
            catch (Exception e)
            {
                Assert.Fatal(false,
                             "GameManager.GameManager: Bad parameters filename -> " + e.Message);
                MyGame.Instance.Exit();
            }

            //load in the map
            LoadMap(filename);


            int keyboardId = InputManager.Instance.FindDevice("keyboard");
            int gamepad = InputManager.Instance.FindDevice("gamepad0");
            InputMap map = PlayerManager.Instance.GetPlayer(0).InputMap;

            if (keyboardId > 0)
            {
                map.BindAction(
                    keyboardId,
                    (int) Keys.Space,
                    selectNextBot);
                map.BindAction(
                    keyboardId,
                    (int) Keys.X,
                    killBot);
            }
            if (gamepad > 0)
            {
                map.BindAction(
                    gamepad,
                    (int) XGamePadDevice.GamePadObjects.X,
                    selectNextBot);
                map.BindAction(
                    gamepad,
                    (int)XGamePadDevice.GamePadObjects.Y,
                    killBot);
            }
            if (Parameters.PlayCaptureTheFlag)
                _map.AddFlagsToMap();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        #region Teams enum

        ///<summary>
        ///teams
        ///</summary>
        public enum Teams
        {
            Blue = 0,
            Red = 1,
            Green = 2,
            Yellow = 3
        } ;

        public enum Ranks
        {
            SquadLeader = 0,
            Officer = 1,
            Enlisted = 2
        }

        #endregion

        ///<summary>
        ///number of bots in the game
        ///</summary>
        public int NumBots
        {
            get { return _botList.Count; }
        }

        ///<summary>
        ///a list of all the bots that are inhabiting the map
        ///</summary>
        public List<BotEntity> BotList
        {
            get { return _botList; }
        }

        ///<summary>
        ///the user may select a bot to control manually.
        ///Also used to determine which bot's info to display.
        ///</summary>
        public BotEntity SelectedBot
        {
            get { return _selectedBot; }
        }

        ///<summary>
        ///this class manages all the path planning requests
        ///</summary>
        public PathManager PathManager
        {
            get { return _pathManager; }
        }

        ///<summary>
        ///the current game map
        ///TODO: integrate or replace with <see cref="MapData"/>
        ///</summary>
        public Map.Map Map
        {
            get { return _map; }
        }

        ///<summary>
        ///map data
        ///</summary>
        public MapData MapData
        {
            get { return _mapData; }
            set { _mapData = value; }
        }

        ///<summary>
        ///game options (determine what info to display, etc)
        ///</summary>
        public OptionsData Options
        {
            get { return _options; }
            set { _options = value; }
        }

        ///<summary>
        ///game parameters (bot and weapon characteristics, etc)
        ///</summary>
        public ParameterData Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        ///<summary>
        ///this list contains any active projectiles (slugs, rockets, shotgun
        ///pellets, etc)
        ///</summary>
        public List<Projectile> Projectiles
        {
            get { return _projectiles; }
        }

        ///<summary>
        ///if true a bot is removed from the game
        ///</summary>
        public bool RemoveABot
        {
            get { return _removeABot; }
            set { _removeABot = value; }
        }

        ///<summary>
        ///List of current grave markers
        ///</summary>
        public List<GraveMarker> GraveMarkerList
        {
            get { return _graveMarkerList; }
        }

        ///<summary>
        ///List of minimap components
        ///</summary>
        public List<MinimapComponent> MinimapComponentList
        {
            get { return _minimapComponentList; }
        }

        #region Public methods

        ///<summary>
        ///tag bots within view range of the given bot
        ///</summary>
        ///<param name="bot"></param>
        ///<param name="range"></param>
        public void TagBotsWithinViewRange(Entity.Entity bot, float range)
        {
            Entity.Entity.TagNeighbors(bot, _botList, range);
        }

        ///<summary>
        ///deletes all the current objects ready for a map load
        ///</summary>
        public void Clear()
        {
            LogUtil.WriteLineIfLogCreate(
                "------------------------------ Cleanup -------------------------------");

            //delete the bots
            foreach (BotEntity curBot in _botList)
            {
                EntityManager.Instance.RemoveEntity(curBot);

                LogUtil.WriteLineIfLogCreate(
                    "Deleting entity id: " + curBot.ObjectId + " of type " +
                    EnumUtil.GetDescription(curBot.EntityType) + "(" +
                    curBot.EntityType + ")");
            }

            //delete any active projectiles
            foreach (Projectile curProjectile in Projectiles)
            {
                EntityManager.Instance.RemoveEntity(curProjectile);

                LogUtil.WriteLineIfLogCreate("Deleting projectile id: " +
                                             curProjectile.ObjectId);
            }

            //delete any active graves
            foreach (GraveMarker curGrave in GraveMarkerList)
            {
                EntityManager.Instance.RemoveEntity(curGrave);

                LogUtil.WriteLineIfLogCreate(
                    "Deleting grave id: " + curGrave.ObjectId);
            }

            //clear the containers
            Projectiles.Clear();
            _botList.Clear();
            GraveMarkerList.Clear();
            _selectedBot = null;
        }

        ///<summary>
        ///calls the update function of each entity
        ///</summary>
        ///<param name="dt">elapsed time since last update</param>
        public void Update(float dt)
        {
            //don't update if the user has paused the game
            if (MyGame.Instance.GamePaused)
                return;

            //update the grave markers
            int curG = 0;
            while (curG < GraveMarkerList.Count)
            {
                GraveMarkerList[curG].Update(dt);
                if (!GraveMarkerList[curG].MarkForDelete)
                {
                    ++curG;
                }
                else
                {
                    EntityManager.Instance.RemoveEntity(GraveMarkerList[curG]);
                    GraveMarkerList.RemoveAt(curG);
                }
            }

            //update all the queued searches in the path manager
            _pathManager.UpdateSearches();

            //update any doors
            foreach (Door curDoor in _map.Doors)
            {
                curDoor.Update(dt);
            }

            //update any current projectiles
            int curP = 0;
            while (curP < Projectiles.Count)
            {
                //test for any dead projectiles and remove them if necessary
                if (!Projectiles[curP].IsDead)
                {
                    Projectiles[curP].Update(dt);

                    ++curP;
                }
                else
                {
                    EntityManager.Instance.RemoveEntity(Projectiles[curP]);
                    Projectiles.RemoveAt(curP);
                }
            }

            //update the bots
            bool isSpawnPossible = true;

            foreach (BotEntity curBot in _botList)
            {
                //if this bot's status is 'respawning' attempt to resurrect it
                //from an unoccupied spawn point
                if (curBot.IsSpawning && isSpawnPossible)
                {
                    isSpawnPossible = AttemptToAddBot(curBot);
                }

                    //if this bot's status is 'dead' add a grave at its current
                    //location then change its status to 'respawning'
                else if (curBot.IsDead)
                {
                    //create a grave
                    AddGrave(curBot.Position);

                    //change its status to spawning
                    curBot.SetSpawning();
                }

                    //if this bot is alive update it.
                else if (curBot.IsAlive)
                {
                    curBot.Update(dt);
                }
            }

            //update the triggers
            _map.UpdateTriggerSystem(dt, _botList);

            //if the user has requested that the number of bots be decreased, 
            //remove one
            if (!RemoveABot)
                return;

            if (_botList.Count > 0)
            {
                BotEntity bot = _botList[_botList.Count - 1];
                if (bot == _selectedBot)
                {
                    _selectedBot = null;
                }
                NotifyAllBotsOfRemoval(bot);
                _botList.RemoveAt(_botList.Count - 1);
            }

            RemoveABot = false;
        }

        ///<summary>
        ///Attempt to add a bot to the game.
        ///To succeed, one of the spawn points must be clear.
        ///</summary>
        ///<param name="bot"></param>
        ///<returns></returns>
        public bool AttemptToAddBot(BotEntity bot)
        {
            //make sure there are some spawn points available
            if (_map.SpawnPoints.Count <= 0)
            {
                Assert.Fatal(false,
                             "GameManager.AttemptToAddBot: Map has no spawn points!");
                return false;
            }

            Vector2 spawnPoint = _map.SpawnPoints[(int) bot.Team];

            //check to see if it's occupied
            bool isSpawnPointAvailable = true;
            foreach (BotEntity curBot in _botList)
            {
                //if the spawn point is unoccupied spawn a bot
                if (Vector2.Distance(spawnPoint, curBot.Position) <
                    curBot.BoundingRadius)
                {
                    isSpawnPointAvailable = false;
                }
            }

            if (!isSpawnPointAvailable)
                return false;

            bot.Spawn(spawnPoint);
            return true;
        }

        ///<summary>
        ///Adds a bot and switches on the default steering behavior
        ///</summary>
        ///<param name="numBotsToAdd"></param>
        public void AddBots(int numBotsToAdd)
        {
            while (numBotsToAdd-- > 0)
            {
                MinimapComponent minimapComponent = null;
                if (MinimapComponentList.Count > 0)
                {
                    minimapComponent =
                        MinimapComponentList[
                            MinimapComponentList.Count - 1];
                    MinimapComponentList.RemoveAt(
                        MinimapComponentList.Count - 1);
                }
                Teams team = (Teams) (_botList.Count % Parameters.NumTeams);

                //get the ordinal rank of the membor of this team
                int rankNumber = (_botList.Count / Parameters.NumTeams);
                //the rank is maximum 2
                Ranks rank = (Ranks) (rankNumber <= 2 ? rankNumber : 2);

                //I'm not sure why these if statements were added. 
                //I commented them all out, because the constructor for BotStaticSprite caused an asserted error.
                //personally I don't see how any of these if statements make sense... ... owell.

                //if (_botList.Count % 12 < 4)
                //{
                //create a bot. (its position is irrelevant at this point
                //because it will not be rendered until it is spawned)
                IBotSceneObject botSceneObject = new BotShape3D(team, minimapComponent);
                //}
                /*else if (_botList.Count % 12 < 8)
                {
                    botSceneObject = 
                        new BotStaticSprite(team, minimapComponent);
                }
                else
                {
                    botSceneObject = 
                        new BotAnimatedSprite(team, minimapComponent);
                }*/

                ((T2DSceneObject) botSceneObject).CreateWithPhysics = true;
                TorqueObjectDatabase.Instance.Register((TorqueObject) botSceneObject);
                ((TorqueObject) botSceneObject).Name = "BOT_" +
                                                       ((TorqueObject) botSceneObject).ObjectId;

                BotEntity botEntity = new BotEntity(botSceneObject, Vector2.Zero);
                botEntity.Name = ((TorqueObject) botSceneObject).Name;

                //switch the default steering behaviors on
                botEntity.Steering.WallAvoidanceIsOn = true;
                botEntity.Steering.SeparationIsOn = true;
                botEntity.Rank = rank;
                _botList.Add(botEntity);

                //register the bot with the entity manager
                EntityManager.Instance.RegisterEntity(botEntity);

                LogUtil.WriteLineIfLogCreate("Adding bot <{0}> with ID {1:D}",
                                             botEntity.Name, botEntity.ObjectId);
            }
        }

        ///<summary>
        ///when a bot is removed from the game by a user all remaining bots
        ///must be notifies so that they can remove any references to that bot
        ///from their memory
        ///</summary>
        ///<param name="removedBot"></param>
        public void NotifyAllBotsOfRemoval(BotEntity removedBot)
        {
            foreach (BotEntity curBot in _botList)
            {
                MessageDispatcher.Instance.DispatchMsg(
                    MessageDispatcher.SEND_MSG_IMMEDIATELY,
                    MessageDispatcher.SENDER_ID_IRRELEVANT,
                    curBot.ObjectId,
                    MessageTypes.UserHasRemovedBot,
                    removedBot);
            }
        }

        ///<summary>
        ///removes the last bot to be added from the game
        ///</summary>
        public void RemoveBot()
        {
            RemoveABot = true;
        }

        ///<summary>
        ///add a bolt to the game
        ///</summary>
        ///<param name="shooter"></param>
        ///<param name="target"></param>
        public void AddBolt(BotEntity shooter, Vector2 target)
        {
            Projectile projectile =
                new ProjectileBolt(shooter, target, new BoltSceneObject());

            Projectiles.Add(projectile);

            projectile.Name = "TempBoltName";
            //register the bolt with the entity manager
            EntityManager.Instance.RegisterEntity(projectile);
            projectile.Name = "BOLT_" + projectile.ObjectId;

            LogUtil.WriteLineIfLogCreate("Adding a bolt " +
                                         projectile.ObjectId + " at position " + projectile.Position);
        }

        ///<summary>
        ///add a rocket to the game
        ///</summary>
        ///<param name="shooter"></param>
        ///<param name="target"></param>
        public void AddRocket(BotEntity shooter, Vector2 target)
        {
            Projectile projectile =
                new ProjectileRocket(shooter, target, new RocketSceneObject());

            Projectiles.Add(projectile);

            projectile.Name = "TempRocketName";
            //register the rocket with the entity manager
            EntityManager.Instance.RegisterEntity(projectile);
            projectile.Name = "ROCKET_" + projectile.ObjectId;

            LogUtil.WriteLineIfLogCreate("Adding a rocket " +
                                         projectile.ObjectId + " at position " + projectile.Position);
        }

        ///<summary>
        ///add a railgun to the game
        ///</summary>
        ///<param name="shooter"></param>
        ///<param name="target"></param>
        public void AddRailgunSlug(BotEntity shooter, Vector2 target)
        {
            Projectile projectile =
                new ProjectileSlug(shooter, target, new SlugSceneObject());

            Projectiles.Add(projectile);

            projectile.Name = "TempSlugName";
            //register the slug with the entity manager
            EntityManager.Instance.RegisterEntity(projectile);
            projectile.Name = "SLUG_" + projectile.ObjectId;

            LogUtil.WriteLineIfLogCreate("Adding a rail gun slug" +
                                         projectile.ObjectId + " at position " + projectile.Position);
        }

        ///<summary>
        ///add a shotgun pellet to the game
        ///</summary>
        ///<param name="shooter"></param>
        ///<param name="target"></param>
        public void AddShotgunPellet(BotEntity shooter, Vector2 target)
        {
            Projectile projectile =
                new ProjectilePellet(shooter, target, new PelletSceneObject());

            Projectiles.Add(projectile);

            projectile.Name = "TempPelletName";
            //register the pellet with the entity manager
            EntityManager.Instance.RegisterEntity(projectile);
            projectile.Name = "PELLET_" + projectile.ObjectId;

            LogUtil.WriteLineIfLogCreate("Adding a shotgun shell " +
                                         projectile.ObjectId + " at position " + projectile.Position);
        }

        ///<summary>
        ///add a grave marker to the game
        ///</summary>
        ///<param name="position"></param>
        public void AddGrave(Vector2 position)
        {
            GraveMarker graveMarker =
                new GraveMarker(
                    position,
                    new Vector2(0, -1), //facing
                    Instance.Parameters.GraveLifetime,
                    new GraveSceneObject());

            GraveMarkerList.Add(graveMarker);

            graveMarker.Name = "TempGraveName";
            //register the grave with the entity manager
            EntityManager.Instance.RegisterEntity(graveMarker);
            graveMarker.Name = "GRAVE_" + graveMarker.ObjectId;

            LogUtil.WriteLineIfLogCreate("Adding a grave " +
                                         graveMarker.ObjectId + " at position " + graveMarker.Position);
        }

        ///<summary>
        ///given a position on the map this method returns the bot found with
        ///its bounding radius of that position. If there is no bot at the
        ///position the method returns null
        ///</summary>
        ///<param name="cursorPos"></param>
        ///<returns></returns>
        public BotEntity GetBotAtPosition(Vector2 cursorPos)
        {
            foreach (BotEntity curBot in _botList)
            {
                if (Vector2.Distance(curBot.Position, cursorPos) >=
                    curBot.BoundingRadius)
                    continue;

                if (curBot.IsAlive)
                {
                    return curBot;
                }
            }

            return null;
        }

        ///<summary>
        ///sets up the game environment from map file
        ///</summary>
        ///<param name="filename"></param>
        ///<returns></returns>
        public bool LoadMap(string filename)
        {
            //clear any current bots and projectiles
            Clear();

            _map = new Map.Map();

            //make sure the entity manager is reset
            EntityManager.Instance.Reset();

            //load the new map data
            if (_map.LoadMap(filename, out _mapData))
            {
                _pathManager =
                    new PathManager(
                        Parameters.MaxSearchCyclesPerUpdateStep);

                AddBots(Parameters.NumBots);

                return true;
            }

            return false;
        }

        ///<summary>
        ///when called will release any possessed bot from user control
        ///</summary>
        public void ExorciseAnyPossessedBot()
        {
            if (_selectedBot != null) _selectedBot.Exorcise();
        }

        ////-------------------------- ClickRightMouseButton -----------------------------
        ////
        //// this method is called when the user clicks the right mouse button.
        ////
        //// the method checks to see if a bot is beneath the cursor. If so, the bot
        //// is recorded as selected.
        ////
        //// if the cursor is not over a bot then any selected bot/s will attempt to
        //// move to that position.
        ////-----------------------------------------------------------------------------

        //template <class VectorType>
        //void Game<VectorType>::ClickRightMouseButton(POINTS p)
        //{
        // Bot<VectorType>* pBot = GetBotAtPosition(POINTStoVector(p));

        // //if there is no selected bot just return;
        // if (!pBot && m_pSelectedBot == NULL) return;

        // //if the cursor is over a different bot to the existing selection,
        // //change selection
        // if (pBot && pBot != m_pSelectedBot)
        // { 
        //   if (m_pSelectedBot) m_pSelectedBot->Exorcise();
        //   m_pSelectedBot = pBot;

        //   return;
        // }

        // //if the user clicks on a selected bot twice it becomes possessed(under
        // //the player's control)
        // if (pBot && pBot == m_pSelectedBot)
        // {
        //   m_pSelectedBot->TakePossession();

        //   //clear any current goals
        //   m_pSelectedBot->Brain->RemoveAllSubgoals();
        // }

        // //if the bot is possessed then a right click moves the bot to the cursor
        // //position
        // if (m_pSelectedBot->isPossessed())
        // {
        //   //if the shift key is pressed down at the same time as clicking then the
        //   //movement command will be queued
        //   if (IS_KEY_PRESSED('Q'))
        //   {
        //     m_pSelectedBot->Brain->QueueGoalMoveToPosition(POINTStoVector(p));
        //   }
        //   else
        //   {
        //     //clear any current goals
        //     m_pSelectedBot->Brain->RemoveAllSubgoals();

        //     m_pSelectedBot->Brain->AddGoalMoveToPosition(POINTStoVector(p));
        //   }
        // }
        //}

        ////---------------------- ClickLeftMouseButton ---------------------------------
        ////-----------------------------------------------------------------------------
        //template <class VectorType>
        //void Game<VectorType>::ClickLeftMouseButton(POINTS p)
        //{
        // if (m_pSelectedBot && m_pSelectedBot->isPossessed())
        // {
        //   m_pSelectedBot->FireWeapon(POINTStoVector(p));
        // }
        //}

        ////------------------------ GetPlayerInput -------------------------------------
        ////
        //// if a bot is possessed the keyboard is polled for user input and any 
        //// relevant bot methods are called appropriately
        ////-----------------------------------------------------------------------------
        //template <class VectorType>
        //void Game<VectorType>::GetPlayerInput()const
        //{
        // if (m_pSelectedBot && m_pSelectedBot->isPossessed())
        // {
        //     m_pSelectedBot->RotateFacingTowardPosition(GetClientCursorPosition());
        //  }
        //}

        ///changes the weapon of the possessed bot
        ///</summary>
        ///<param name="weapon"></param>
        public void ChangeWeaponOfPossessedBot(int weapon)
        {
            //ensure one of the bots has been possessed
            if (_selectedBot == null)
                return;

            switch (weapon)
            {
                case (int) EntityTypes.Blaster:
                    SelectedBot.ChangeWeapon(WeaponTypes.Blaster);
                    return;

                case (int) EntityTypes.Shotgun:
                    SelectedBot.ChangeWeapon(WeaponTypes.Shotgun);
                    return;

                case (int) EntityTypes.RocketLauncher:
                    SelectedBot.ChangeWeapon(WeaponTypes.RocketLauncher);
                    return;

                case (int) EntityTypes.Railgun:
                    SelectedBot.ChangeWeapon(WeaponTypes.Railgun);
                    return;
            }
        }

        ///<summary>
        ///Tests if the ray between v1 and v2 is unobstructed.
        ///</summary>
        ///<param name="v1"></param>
        ///<param name="v2"></param>
        ///<returns>true if the ray between v1 and v2 is unobstructed.</returns>
        public bool IsLOSOkay(Vector2 v1, Vector2 v2)
        {
            return
                !WallIntersectionTests.DoWallsObstructLineSegment(
                     v1, v2, _map.Walls);
        }

        ///<summary>
        ///Tests if a bot moving from A to B would bump into world geometry.
        ///It achieves this by stepping from A to B in steps of size
        ///BoundingRadius and testing for intersection with world geometry at
        ///each point.
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="boundingRadius"></param>
        ///<returns>
        ///true if a bot moving from A to B would bump into world geometry.
        ///</returns>
        public bool IsPathObstructed(Vector2 a, Vector2 b, float boundingRadius)
        {
            Vector2 toB = Vector2.Normalize(b - a);
            Vector2 curPos = a;

            while (Vector2.DistanceSquared(curPos, b) >
                   boundingRadius*boundingRadius)
            {
                //advance curPos one step
                curPos += toB*0.5f*boundingRadius;

                //test all walls against the new position
                if (WallIntersectionTests.DoWallsIntersectCircle(
                    _map.Walls, curPos, boundingRadius))
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///Get all bots in the given bot's field of view.
        ///</summary>
        ///<param name="bot"></param>
        ///<returns>
        ///a list of bots within the given bot's field of view
        ///</returns>
        public List<BotEntity> GetAllBotsInFOV(BotEntity bot)
        {
            List<BotEntity> visibleBots = new List<BotEntity>();

            foreach (BotEntity curBot in _botList)
            {
                //make sure time is not wasted checking against the same
                //bot or against a bot that is dead or re-spawning
                if (curBot == bot || !curBot.IsAlive)
                    continue;

                //first of all test to see if this bot is within the FOV
                if (!Vector2Util.IsSecondInFOVOfFirst(
                         bot.Position,
                         bot.Facing,
                         curBot.Position,
                         bot.FieldOfView))
                    continue;

                //cast a ray from between the bots to test visibility. If the
                //bot is visible add it to the vector
                if (!WallIntersectionTests.DoWallsObstructLineSegment(
                         bot.Position,
                         curBot.Position,
                         _map.Walls))
                {
                    visibleBots.Add(curBot);
                }
            }

            return visibleBots;
        }

        ///<summary>
        ///Tests if second bot is visible to the first bot
        ///</summary>
        ///<param name="first"></param>
        ///<param name="second"></param>
        ///<returns></returns>
        public bool IsSecondVisibleToFirst(BotEntity first, BotEntity second)
        {
            //if the two bots are equal or if one of them is not alive,
            //return false
            if (!(first == second) && second.IsAlive)
            {
                //first of all test to see if this bot is within the FOV
                if (Vector2Util.IsSecondInFOVOfFirst(
                    first.Position,
                    first.Facing,
                    second.Position,
                    first.FieldOfView))
                {
                    //test the line segment connecting the bots' positions
                    //against the walls. If the bot is visible add it to the list
                    if (!WallIntersectionTests.DoWallsObstructLineSegment(
                             first.Position,
                             second.Position,
                             _map.Walls))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        ///<summary>
        ///Gets the position of the closest visible switch that triggers the
        ///door of the specified Id
        ///</summary>
        ///<param name="botPos"></param>
        ///<param name="doorId"></param>
        ///<returns>
        ///the position of the closest visible switch that triggers the door of
        ///the specified Id
        ///</returns>
        public Vector2 GetPosOfClosestSwitch(Vector2 botPos, uint doorId)
        {
            List<uint> switchIds = new List<uint>();

            //first we need to get the ids of the switches attached to this door
            foreach (Door curDoor in _map.Doors)
            {
                if (curDoor.ObjectId != doorId)
                    continue;

                switchIds = curDoor.SwitchIds;
                break;
            }

            Vector2 closest = Vector2.Zero;
            float closestDist = Single.MaxValue;

            //now test to see which one is closest and visible
            foreach (uint switchId in switchIds)
            {
                Entity.Entity trig = EntityManager.Instance.GetEntityFromId(switchId);

                if (!IsLOSOkay(botPos, trig.Position))
                    continue;

                float dist = Vector2.DistanceSquared(botPos, trig.Position);

                if (dist >= closestDist)
                    continue;

                closestDist = dist;
                closest = trig.Position;
            }

            return closest;
        }

        ///<summary>
        ///Render game entities, etc.
        ///Note: Torque scene objects are rendered separately by the engine.
        ///Note: This is called after the scene and GUI have already rendered.
        ///</summary>
        public void Render()
        {
            foreach (GraveMarker curGrave in GraveMarkerList)
            {
                curGrave.Render();
            }

            //render the map
            _map.Render();

            //render all the bots unless the user has selected the option to
            //only render those bots that are in the fov of the selected bot
            if (_selectedBot != null &&
                Instance.Options.OnlyShowBotsInTargetsFOV)
            {
                List<BotEntity> visibleBots = GetAllBotsInFOV(_selectedBot);

                foreach (BotEntity curBot in visibleBots)
                {
                    curBot.Render();
                }

                if (_selectedBot != null) _selectedBot.Render();
            }
            else
            {
                //render all the entities
                foreach (BotEntity curBot in _botList)
                {
                    if (curBot.IsAlive)
                    {
                        curBot.Render();
                    }
                }
            }

            //render any projectiles
            foreach (Projectile curProjectile in Projectiles)
            {
                curProjectile.Render();
            }

            TextUtil.DrawText(5, _map.SizeY - 5,
                              "Num Current Searches: " + _pathManager.NumActiveSearches);

            int i = 10; // Holds the initial Position of our text (Morteza)
            //Create a 'stat. for each bot on the screen
            // We display the team, name and the score and the weapon and number of ammo and other properties
            foreach (BotEntity bot in _botList)
            {
                TextUtil.DrawText(5, _map.SizeY + i,
                                  "(" + bot.Team + ") " + bot.Name + " SCORE: " + bot.Score + "  HEALTH: " +
                                  bot.CurrentHealth +
                                  " WEAPON: " + bot.WeaponSystem.CurrentWeapon.WeaponType + " AMMO: " +
                                  bot.WeaponSystem.CurrentWeapon.NumRoundsRemaining + "/" +
                                  _botList[1].WeaponSystem.CurrentWeapon.MaxRoundsCarried + " HEADING: " + bot.Heading);
                i += 10;
            }


            //render a red circle around the selected bot (blue if possessed)
            if (_selectedBot == null)
                return;

            if (_selectedBot.IsPossessed)
            {
                DrawUtil.Circle(
                    _selectedBot.Position,
                    _selectedBot.BoundingRadius + 1,
                    Color.Blue,
                    20);
            }
            else
            {
                DrawUtil.Circle(
                    _selectedBot.Position,
                    _selectedBot.BoundingRadius + 1,
                    Color.Red,
                    20);
            }

            if (Instance.Options.ShowOpponentsSensedBySelectedBot)
            {
                _selectedBot.SensoryMemory.RenderBoxesAroundRecentlySensed();
            }

            //render a square around the bot's target
            if (Instance.Options.ShowTargetOfSelectedBot &&
                _selectedBot.TargetBot != null)
            {
                Vector2 p = _selectedBot.TargetBot.Position;
                float b = _selectedBot.TargetBot.BoundingRadius;

                DrawUtil.Line(
                    new Vector2(p.X - b, p.Y - b),
                    new Vector2(p.X + b, p.Y - b),
                    Color.Red,
                    2);
                DrawUtil.Line(
                    new Vector2(p.X + b, p.Y - b),
                    new Vector2(p.X + b, p.Y + b),
                    Color.Red, 2);
                DrawUtil.Line(
                    new Vector2(p.X + b, p.Y + b),
                    new Vector2(p.X - b, p.Y + b),
                    Color.Red,
                    2);
                DrawUtil.Line(
                    new Vector2(p.X - b, p.Y + b),
                    new Vector2(p.X - b, p.Y - b),
                    Color.Red,
                    2);
            }

            //render the path of the bot
            if (Instance.Options.ShowPathOfSelectedBot)
            {
                _selectedBot.Brain.Render();
            }

            //display the bot's goal stack
            if (Instance.Options.ShowGoalsOfSelectedBot)
            {
                Vector2 p =
                    new Vector2(
                        _selectedBot.Position.X - 50,
                        _selectedBot.Position.Y);

                _selectedBot.Brain.RenderAtPos(ref p);
            }

            if (Instance.Options.ShowGoalAppraisals)
            {
                _selectedBot.Brain.RenderEvaluations(5, _map.SizeY - 15);
            }

            if (Instance.Options.ShowWeaponAppraisals)
            {
                _selectedBot.WeaponSystem.RenderDesirabilities();
            }

            //if (IS_KEY_PRESSED('Q') && _selectedBot.IsPossessed)
            //{
            //   TextUtil.DrawText(GetClientCursorPosition(), Color.Red, "Queuing");
            //}
        }

        #endregion

        #region Private, protected, internal methods

        /// <summary>
        /// this cycles throught the bots on the map as the player hits space(keyboard) or x(gamepad)
        /// </summary>
        /// <param name="keyPress">1 if keypress, 0 if keyrelease</param>
        private void selectNextBot(float keyPress)
        {
            if (keyPress == 1)
            {
                if (_selectedBot != null)
                {
                    _selectedBot = _botList[(_botList.IndexOf(_selectedBot) + 1)%_botList.Count];
                }
                else
                {
                    if (_botList.Count > 0)
                        _selectedBot = _botList[0];
                }
            }
        }

        /// <summary>
        /// used to kill a bot on command
        /// </summary>
        /// <param name="keyPress">1 if keypress, 0 if keyrelease</param>
        private void killBot(float keyPress)
        {
            if(_selectedBot != null && keyPress == 1)
            {
                _selectedBot.SetDead();
            }
        }

        private MinimapComponent CreateMinimapComponent(
            Vector2 cameraCenter,
            Vector2 cameraExtent,
            Vector2 viewPosition,
            Vector2 viewSize,
            bool showBorder)
        {
            MinimapComponent mini = new MinimapComponent();
            mini.CameraCenter = cameraCenter;
            mini.CameraExtent = cameraExtent;
            mini.ViewPosition = viewPosition;
            mini.ViewSize = viewSize;
            mini.ShowBorder = showBorder;
            return mini;
        }

        #endregion

        #region Private, protected, internal fields

        private readonly List<BotEntity> _botList;
        private readonly List<GraveMarker> _graveMarkerList;
        private readonly List<MinimapComponent> _minimapComponentList;
        private readonly List<Projectile> _projectiles;
        private Map.Map _map;
        private MapData _mapData;
        private OptionsData _options;
        private ParameterData _parameters;
        private PathManager _pathManager;
        private bool _removeABot;
        private BotEntity _selectedBot;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}