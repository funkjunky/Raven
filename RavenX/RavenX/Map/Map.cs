#region File description

//------------------------------------------------------------------------------
//Map.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using GarageGames.Torque.Util;
using MapContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.Entity;
using Mindcrafters.RavenX.Entity.Bot;
using Mindcrafters.RavenX.Graph;
using Mindcrafters.RavenX.Trigger;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Map
{
    ///<summary>
    ///this class creates and stores all the entities that make up the
    ///Raven game environment. (walls, bots, health etc)
    ///TODO: integrate or replace with <see cref="MapData"/>
    ///</summary>
    public class Map
    {
        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        public Map()
        {
            NavGraph = null;
            CellSpace = null;
            SizeY = 0;
            SizeX = 0;
            CellSpaceNeighborhoodRange = 0;

            //delete the triggers
            _triggerSystem = new TriggerSystem();

            //delete the doors
            _doors = new List<Door>();

            _walls = new List<Wall>();
            _spawnPoints = new List<Vector2>();
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Get the list of triggers
        ///</summary>
        public List<Trigger.Trigger> Triggers
        {
            get { return TriggerSystem.Triggers; }
        }

        ///<summary>
        ///the walls that comprise the current map's architecture. 
        ///</summary>
        public List<Wall> Walls
        {
            get { return _walls; }
        }

        ///<summary>
        ///this map's accompanying navigation graph
        ///</summary>
        public SparseGraph NavGraph
        {
            get { return _navGraph; }
            set { _navGraph = value; }
        }

        ///<summary>
        ///a map may contain a number of sliding doors.
        ///</summary>
        public List<Door> Doors
        {
            get { return _doors; }
        }

        ///<summary>
        ///this holds a number of spawn positions. When a bot is instantiated
        ///it will appear at a randomly selected point chosen from this vector
        ///</summary>
        public List<Vector2> SpawnPoints
        {
            get { return _spawnPoints; }
        }

        ///<summary>
        ///the graph nodes will be partitioned enabling fast lookup
        ///TODO: phase out and use Torque's object database
        ///</summary>
        public CellSpacePartition CellSpace
        {
            get { return _cellSpace; }
            set { _cellSpace = value; }
        }

        ///<summary>
        ///map width
        ///</summary>
        public int SizeX
        {
            get { return _sizeX; }
            set { _sizeX = value; }
        }

        ///<summary>
        ///map height
        ///</summary>
        public int SizeY
        {
            get { return _sizeY; }
            set { _sizeY = value; }
        }

        ///<summary>
        ///the size of the search radius the cell space partition uses when
        ///looking for neighbors 
        ///</summary>
        public float CellSpaceNeighborhoodRange
        {
            get { return _cellSpaceNeighborhoodRange; }
            set { _cellSpaceNeighborhoodRange = value; }
        }

        ///<summary>
        ///trigger are objects that define a region of space. When a raven bot
        ///enters that area, it 'triggers' an event. That event may be anything
        ///from increasing a bot's health to opening a door or requesting a lift.
        ///</summary>
        public TriggerSystem TriggerSystem
        {
            get { return _triggerSystem; }
        }

        ///<summary>
        ///this will hold a pre-calculated lookup table of the cost to travel
        ///from one node to any other.
        ///</summary>
        public List<List<float>> PathCosts
        {
            get { return _pathCosts; }
            set { _pathCosts = value; }
        }

        #region Public methods

        ///<summary>
        ///Select a random spawn point
        ///</summary>
        ///<returns></returns>
        public Vector2 GetRandomSpawnPoint()
        {
            return SpawnPoints[TorqueUtil.GetFastRandomInt(0, SpawnPoints.Count - 1)];
        }

        ///<summary>
        ///deletes all the current objects ready for a map load
        ///</summary>
        public void Clear()
        {
            TriggerSystem.Clear();
            Doors.Clear();
            Walls.Clear();
            SpawnPoints.Clear();
            NavGraph = null;
            CellSpace = null;
        }

        ///<summary>
        ///Add a wall using given wall data
        ///</summary>
        ///<param name="wallData"></param>
        public void AddWall(WallData wallData)
        {
            Wall wall = new Wall(wallData);
            Walls.Add(wall);

            //register the entity 
            EntityManager.Instance.RegisterEntity(wall);
        }

        ///<summary>
        ///Add a wall at given position
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<returns></returns>
        public Wall AddWall(Vector2 from, Vector2 to)
        {
            Wall wall = new Wall(from, to);

            Walls.Add(wall);

            //register the entity 
            EntityManager.Instance.RegisterEntity(wall);

            return wall;
        }

        ///<summary>
        ///Add a door using given door data
        ///</summary>
        ///<param name="doorData"></param>
        public void AddDoor(DoorData doorData)
        {
            Door door = new Door(this, doorData);
            Doors.Add(door);

            //register the entity 
            EntityManager.Instance.RegisterEntity(door);
        }

        ///<summary>
        ///Add a door trigger using given door trigger data
        ///</summary>
        ///<param name="doorTriggerData"></param>
        public void AddDoorTrigger(DoorTriggerData doorTriggerData)
        {
            TriggerOnButtonSendMsg tr =
                new TriggerOnButtonSendMsg(doorTriggerData);

            TriggerSystem.Register(tr);

            //register the entity 
            EntityManager.Instance.RegisterEntity(tr);
        }

        ///<summary>
        ///Add a spawn point using given spawn point data
        ///</summary>
        ///<param name="spawnPointData"></param>
        public void AddSpawnPoint(SpawnPointData spawnPointData)
        {
            SpawnPoints.Add(spawnPointData.Position);
        }

        ///<summary>
        ///Add health using given health data
        ///</summary>
        ///<param name="healthData"></param>
        public void AddHealth(HealthData healthData)
        {
            TriggerHealthGiver hg = new TriggerHealthGiver(healthData);

            TriggerSystem.Register(hg);

            //let the corresponding navgraph node point to this object
            NavGraphNode node = NavGraph.GetNode(hg.NodeIndex);

            node.ExtraInfo = hg;

            //register the entity 
            EntityManager.Instance.RegisterEntity(hg);
        }

        ///<summary>
        ///Add a weapon giver
        ///</summary>
        ///<param name="wg"></param>
        public void AddWeaponGiver(TriggerWeaponGiver wg)
        {
            //add it to the appropriate vectors
            TriggerSystem.Register(wg);

            //let the corresponding navgraph node point to this object
            NavGraphNode node = NavGraph.GetNode(wg.NodeIndex);

            node.ExtraInfo = wg;

            //register the entity 
            EntityManager.Instance.RegisterEntity(wg);
        }

        //TODO make weaponData superclass

        ///<summary>
        ///Add a railgun using given railgun data
        ///</summary>
        ///<param name="railgunData"></param>
        public void AddRailgun(RailgunData railgunData)
        {
            TriggerWeaponGiver wg = new TriggerWeaponGiver(railgunData);

            AddWeaponGiver(wg);
        }

        ///<summary>
        ///Add a rocket launcher using given rocket launcher data
        ///</summary>
        ///<param name="rocketLauncher"></param>
        public void AddRocketLauncher(RocketLauncherData rocketLauncher)
        {
            TriggerWeaponGiver wg = new TriggerWeaponGiver(rocketLauncher);

            AddWeaponGiver(wg);
        }

        ///<summary>
        ///Add a shotgun using given shotgun data
        ///</summary>
        ///<param name="shotgunData"></param>
        public void AddShotgun(ShotgunData shotgunData)
        {
            TriggerWeaponGiver wg = new TriggerWeaponGiver(shotgunData);

            AddWeaponGiver(wg);
        }

        ///<summary>
        ///sets up the game environment from map file
        ///</summary>
        ///<param name="filename"></param>
        ///<param name="mapData"></param>
        ///<returns></returns>
        public bool LoadMap(string filename, out MapData mapData)
        {
            mapData = null;

            try
            {
                mapData = MyGame.Instance.Content.Load<MapData>(filename);
            }
            catch (Exception e)
            {
                Assert.Fatal(false,
                             "Map.LoadMap: Bad Map Filename -> " + e.Message);
                return false;
            }

            Clear();

            //first of all read and create the navgraph. This must be done
            //before the entities are read from the map file because many of
            //the entities will be linked to a graph node (the graph node will
            //own a pointer to an instance of the entity)
            NavGraph = new SparseGraph(false);

            NavGraph.Load(mapData);

            LogUtil.WriteLineIfLogCreate("NavGraph for " + filename +
                                         " loaded okay");
            LogUtil.WriteLineIfLogCreate(NavGraph.ToString());

            //determine the average distance between graph nodes so that we can
            //partition them efficiently
            CellSpaceNeighborhoodRange =
                GraphUtil.CalculateAverageGraphEdgeLength(NavGraph) + 1;

            LogUtil.WriteLineIfLogCreate("Average edge length is " +
                                         GraphUtil.CalculateAverageGraphEdgeLength(NavGraph));

            LogUtil.WriteLineIfLogCreate("Neighborhood range set to " +
                                         CellSpaceNeighborhoodRange);

            //load in the map size
            SizeX = mapData.SizeX;
            SizeY = mapData.SizeY;

            LogUtil.WriteLineIfLogCreate("Partitioning navgraph nodes...");

            //partition the graph nodes
            PartitionNavGraph();

            LogUtil.WriteLineIfLogCreate("Loading map...");
            foreach (WallData wallData in mapData.WallList)
            {
                LogUtil.WriteLineIfLogCreate("Creating a wall <" +
                                             wallData.Name + "> between " + wallData.From + " and " +
                                             wallData.To);

                AddWall(wallData);
            }

            //note: add triggers before doors (or else!)
            foreach (DoorTriggerData doorTriggerData in mapData.DoorTriggerList)
            {
                LogUtil.WriteLineIfLogCreate("Creating a door trigger <" +
                                             doorTriggerData.Name + "> at " + doorTriggerData.Position);

                AddDoorTrigger(doorTriggerData);
            }

            foreach (DoorData doorData in mapData.DoorList)
            {
                LogUtil.WriteLineIfLogCreate("Creating a door <" +
                                             doorData.Name + "> between " + doorData.From + " and " +
                                             doorData.To);

                AddDoor(doorData);
            }

            foreach (SpawnPointData spawnPointData in mapData.SpawnPointList)
            {
                LogUtil.WriteLineIfLogCreate("Creating a spawn point <" +
                                             spawnPointData.Name + "> at " + spawnPointData.Position);

                AddSpawnPoint(spawnPointData);
            }

            foreach (HealthData healthData in mapData.HealthList)
            {
                LogUtil.WriteLineIfLogCreate(
                    "Creating a health giver trigger <" +
                    healthData.Name + "> at " + healthData.Position);

                AddHealth(healthData);
            }

            foreach (RailgunData railgunData in mapData.RailgunList)
            {
                LogUtil.WriteLineIfLogCreate(
                    "Creating a rail gun weapon giver trigger <" +
                    railgunData.Name + "> at " + railgunData.Position);

                AddRailgun(railgunData);
            }

            foreach (RocketLauncherData rocketLauncherData 
                in mapData.RocketLauncherList)
            {
                LogUtil.WriteLineIfLogCreate(
                    "Creating a rocket launcher weapon giver trigger <" +
                    rocketLauncherData.Name + "> at " +
                    rocketLauncherData.Position);

                AddRocketLauncher(rocketLauncherData);
            }

            foreach (ShotgunData shotgunData in mapData.ShotgunList)
            {
                LogUtil.WriteLineIfLogCreate(
                    "Creating a shot gun weapon giver trigger <" +
                    shotgunData.Name + "> at " + shotgunData.Position);

                AddShotgun(shotgunData);
            }

            LogUtil.WriteLineIfLogCreate(filename + " loaded okay");

            //calculate the cost lookup table
            PathCosts = GraphUtil.CreateAllPairsCostsTable(NavGraph);

            return true;
        }

        ///<summary>
        ///Uses the pre-calculated lookup table to determine the cost of traveling
        ///from nd1 to nd2
        ///</summary>
        ///<param name="nd1"></param>
        ///<param name="nd2"></param>
        ///<returns></returns>
        public float CalculateCostBetweenNodes(int nd1, int nd2)
        {
            Assert.Fatal(nd1 >= 0 && nd1 < NavGraph.NumNodes &&
                         nd2 >= 0 && nd2 < NavGraph.NumNodes,
                         "Map.CostBetweenNodes: invalid index");

            return PathCosts[nd1][nd2];
        }

        /// <summary>
        /// Add appropriate team flags to the map
        /// </summary>
        public void AddFlagsToMap()
        {
            if (GameManager.GameManager.Instance.Parameters.NumTeams > 0)
            {
                TriggerFlag flagTrigger =
                    new TriggerFlag(new FlagData("BlueFlag", new Vector2((SizeX/100f)*5, SizeY/2.0f), 10, 0));
                TriggerSystem.Register(flagTrigger);
                NavGraphNode node = NavGraph.FindClosestAccessibleNodeToPosition(flagTrigger.Position);
                node.ExtraInfo = flagTrigger;
                EntityManager.Instance.RegisterEntity(flagTrigger);
                if (GameManager.GameManager.Instance.Parameters.NumTeams > 1)
                {
                    flagTrigger =
                        new TriggerFlag(new FlagData("RedFlag", new Vector2((SizeX / 100f) * 95, SizeY / 2.0f), 10, 1));
                    TriggerSystem.Register(flagTrigger);
                    node = NavGraph.FindClosestAccessibleNodeToPosition(flagTrigger.Position);
                    node.ExtraInfo = flagTrigger;
                    EntityManager.Instance.RegisterEntity(flagTrigger);
                    if (GameManager.GameManager.Instance.Parameters.NumTeams > 2)
                    {
                        flagTrigger =
                            new TriggerFlag(new FlagData("GreenFlag", new Vector2(SizeX / 2.0f, (SizeY / 100f) * 95), 10, 2));
                        TriggerSystem.Register(flagTrigger);
                        node = NavGraph.FindClosestAccessibleNodeToPosition(flagTrigger.Position);
                        node.ExtraInfo = flagTrigger;
                        EntityManager.Instance.RegisterEntity(flagTrigger);
                        if (GameManager.GameManager.Instance.Parameters.NumTeams > 3)
                        {
                            flagTrigger =
                                new TriggerFlag(new FlagData("YellowFlag", new Vector2(SizeX / 2.0f, (SizeY / 100f) * 5), 10,
                                                             3));
                            TriggerSystem.Register(flagTrigger);
                            node = NavGraph.FindClosestAccessibleNodeToPosition(flagTrigger.Position);
                            node.ExtraInfo = flagTrigger;
                            EntityManager.Instance.RegisterEntity(flagTrigger);
                        }
                    }
                }
            }
        }

        ///<summary>
        ///create the cell space partition for the navgraph
        ///</summary>
        public void PartitionNavGraph()
        {
            CellSpace =
                new CellSpacePartition(
                    SizeX,
                    SizeY,
                    GameManager.GameManager.Instance.Parameters.NumCellsX,
                    GameManager.GameManager.Instance.Parameters.NumCellsY,
                    NavGraph.NumNodes);

            //add the graph nodes to the space partition
            foreach (NavGraphNode curNode in NavGraph.Nodes)
            {
                if (GraphNode.IsInvalidIndex(curNode.Index))
                    continue;

                CellSpace.AddEntity(curNode);
            }
        }

        ///<summary>
        ///given the bot that has made a sound, this method adds a sound trigger
        ///</summary>
        ///<param name="soundSource"></param>
        ///<param name="range"></param>
        public void AddSoundTrigger(BotEntity soundSource, float range)
        {
            TriggerSystem.Register(new TriggerSoundNotify(soundSource, range));
        }

        ///<summary>
        ///given a list of entities in the world this method updates them
        ///against all the triggers
        ///</summary>
        ///<param name="dt"></param>
        ///<param name="bots"></param>
        public void UpdateTriggerSystem(float dt, List<BotEntity> bots)
        {
            TriggerSystem.Update(dt, bots);
        }

        ///<summary>
        ///Gets the position of a graph node selected at random
        ///</summary>
        ///<returns>the position of a graph node selected at random</returns>
        public Vector2 GetRandomNodeLocation()
        {
            int randIndex =
                TorqueUtil.GetFastRandomInt(0, NavGraph.NumActiveNodes() - 1);

            NavGraphNode node = NavGraph.Nodes[randIndex];

            return node.Position;
        }

        ///<summary>
        ///render map
        ///</summary>
        public void Render()
        {
            //render the navgraph
            if (GameManager.GameManager.Instance.Options.ShowGraph)
            {
                GraphUtil.Draw(
                    NavGraph,
                    Color.Gray,
                    GameManager.GameManager.Instance.Options.ShowNodeIndices);
            }

            //render any doors
            foreach (Door curDoor in Doors)
            {
                curDoor.Render();
            }

            //render all the triggers
            TriggerSystem.Render();

            //render all the walls
            foreach (Wall curWall in Walls)
            {
                curWall.Render();
            }

            foreach (Vector2 curSp in SpawnPoints)
            {
                DrawUtil.CircleFill(curSp, 7, Color.Gray, 20);
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly List<Door> _doors;
        private readonly List<Vector2> _spawnPoints;
        private readonly TriggerSystem _triggerSystem;
        private readonly List<Wall> _walls;
        private CellSpacePartition _cellSpace;
        private float _cellSpaceNeighborhoodRange;
        private SparseGraph _navGraph;
        private List<List<float>> _pathCosts;
        private int _sizeX;
        private int _sizeY;

        //

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}