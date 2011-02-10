#region File description

//------------------------------------------------------------------------------
//MapData.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Mindcrafters.Library.Utility;

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
    ///class for map data
    ///</summary>
    public class MapData
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///Convert from Raven .map format to RavenX .xml
        ///</summary>
        ///<param name="ravenMapFilename"></param>
        ///<param name="xmlMapFilename"></param>
        public static void Convert(
            string ravenMapFilename, string xmlMapFilename)
        {
            Convert(ravenMapFilename, xmlMapFilename, 1.0f);
        }

        ///<summary>
        ///Convert from Raven .map format to RavenX .xml and change scale
        ///</summary>
        ///<param name="ravenMapFilename"></param>
        ///<param name="xmlMapFilename"></param>
        ///<param name="scale"></param>
        public static void Convert(
            string ravenMapFilename, string xmlMapFilename, float scale)
        {
            MapData mapData = new MapData();

            mapData.LoadFromRavenMap(ravenMapFilename);

            mapData.ChangeScale(scale);

            mapData.SaveToXml(xmlMapFilename);
        }

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Name used to find object by name (should be unique)
        ///</summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        ///<summary>
        ///list of nodes in the game graph
        ///</summary>
        public List<NodeData> NodeList
        {
            get { return _nodeList; }
            set { _nodeList = value; }
        }

        ///<summary>
        ///list of edges in the game graph
        ///</summary>
        public List<EdgeData> EdgeList
        {
            get { return _edgeList; }
            set { _edgeList = value; }
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
        ///list of walls in the map
        ///</summary>
        public List<WallData> WallList
        {
            get { return _wallList; }
            set { _wallList = value; }
        }

        ///<summary>
        ///list of doors in the map
        ///</summary>
        public List<DoorData> DoorList
        {
            get { return _doorList; }
            set { _doorList = value; }
        }

        ///<summary>
        ///list of door triggers in the map
        ///</summary>
        public List<DoorTriggerData> DoorTriggerList
        {
            get { return _doorTriggerList; }
            set { _doorTriggerList = value; }
        }

        ///<summary>
        ///list of spawn points in the map
        ///</summary>
        public List<SpawnPointData> SpawnPointList
        {
            get { return _spawnPointList; }
            set { _spawnPointList = value; }
        }

        ///<summary>
        ///list of health triggers in the map
        ///</summary>
        public List<HealthData> HealthList
        {
            get { return _healthList; }
            set { _healthList = value; }
        }

        ///<summary>
        ///list of railguns in the map
        ///</summary>
        public List<RailgunData> RailgunList
        {
            get { return _railgunList; }
            set { _railgunList = value; }
        }

        ///<summary>
        ///list of rocket launchers in the map
        ///</summary>
        public List<RocketLauncherData> RocketLauncherList
        {
            get { return _rocketLauncherList; }
            set { _rocketLauncherList = value; }
        }

        ///<summary>
        ///list of shotguns in the map
        ///</summary>
        public List<ShotgunData> ShotgunList
        {
            get { return _shotgunList; }
            set { _shotgunList = value; }
        }

        #region Public methods

#if !XBOX
        ///<summary>
        ///write data to xml file
        ///</summary>
        ///<param name="xmlMapFilename"></param>
        public void SaveToXml(string xmlMapFilename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(xmlMapFilename, settings))
            {
                IntermediateSerializer.Serialize(xmlWriter, this, null);
            }
        }
#endif

        ///<summary>
        ///load a map in Raven .map format
        ///</summary>
        ///<param name="ravenMapFilename">the Raven map file</param>
        ///<exception cref="ApplicationException"></exception>
        public void LoadFromRavenMap(string ravenMapFilename)
        {
            //used to find index of edge crossed by entity with given ravenId
            Dictionary<int, List<int>> idEdgeMap =
                new Dictionary<int, List<int>>();

            //used to find the index of door having trigger with given ravenId
            Dictionary<int, int> idDoorMap = new Dictionary<int, int>();

            //used to find the index of the door with the given ravenId
            Dictionary<int, int> doorIdIndexMap = new Dictionary<int, int>();

            DataStreamReader inStream = new DataStreamReader(ravenMapFilename);

            Name = ravenMapFilename;

            int numNodes = inStream.GetIntFromStream();
            NodeList = new List<NodeData>(numNodes);
            for (int i = 0; i < numNodes; i++)
            {
                NodeData nodeData = new NodeData();

                nodeData.Name = "Node_" + i;
                inStream.SkipNextFieldInStream(); //Index:
                nodeData.Index = inStream.GetIntFromStream();
                inStream.SkipNextFieldInStream(); //PosX:
                float x = inStream.GetIntFromStream();
                inStream.SkipNextFieldInStream(); //PosY:
                float y = inStream.GetIntFromStream();
                nodeData.Position = new Vector2(x, y);

                NodeList.Add(nodeData);
            }

            int numEdges = inStream.GetIntFromStream();
            EdgeList = new List<EdgeData>(numEdges);
            for (int i = 0; i < numEdges; i++)
            {
                EdgeData edgeData = new EdgeData();

                edgeData.Name = "Edge_" + i;
                inStream.SkipNextFieldInStream(); //From:
                edgeData.FromIndex = inStream.GetIntFromStream();
                inStream.SkipNextFieldInStream(); //To:
                edgeData.ToIndex = inStream.GetIntFromStream();
                inStream.SkipNextFieldInStream(); //Cost:
                edgeData.Cost = inStream.GetFloatFromStream();
                inStream.SkipNextFieldInStream(); //Flags:
                edgeData.BehaviorType =
                    (EdgeData.BehaviorTypes) inStream.GetIntFromStream();
                inStream.SkipNextFieldInStream(); //ID:
                //we'll save the id and use it later to fill in
                //the object name when we process the enities
                int idOfIntersectingEntity = inStream.GetIntFromStream();
                if (idOfIntersectingEntity > 0) //if valid
                {
                    if (!idEdgeMap.ContainsKey(idOfIntersectingEntity))
                    {
                        idEdgeMap.Add(idOfIntersectingEntity, new List<int>());
                    }
                    idEdgeMap[idOfIntersectingEntity].Add(i);
                }
                edgeData.NameOfIntersectingEntity = ""; //may replace later
                EdgeList.Add(edgeData);
            }

            SizeX = inStream.GetIntFromStream();
            SizeY = inStream.GetIntFromStream();

            WallList = new List<WallData>();
            DoorList = new List<DoorData>();
            DoorTriggerList = new List<DoorTriggerData>();
            SpawnPointList = new List<SpawnPointData>();
            HealthList = new List<HealthData>();
            RailgunList = new List<RailgunData>();
            RocketLauncherList = new List<RocketLauncherData>();
            ShotgunList = new List<ShotgunData>();
            while (inStream.Peek() >= 0)
            {
                EntityTypes entityType = (EntityTypes) inStream.GetIntFromStream();

                float x, y;

                switch (entityType)
                {
                    case EntityTypes.Wall:
                        WallData wallData = new WallData();

                        wallData.Name = "Wall_" + WallList.Count;
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        wallData.From = new Vector2(x, y);
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        wallData.To = new Vector2(x, y);
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        wallData.Normal = new Vector2(x, y);

                        WallList.Add(wallData);
                        break;

                    case EntityTypes.SlidingDoor:
                        DoorData doorData = new DoorData();

                        doorData.Name = "Door_" + DoorList.Count;
                        int ravenDoorId = inStream.GetIntFromStream();
                        doorIdIndexMap.Add(ravenDoorId, DoorList.Count); //save

                        if (idEdgeMap.ContainsKey(ravenDoorId))
                        {
                            foreach (int i in idEdgeMap[ravenDoorId])
                            {
                                EdgeList[i].NameOfIntersectingEntity =
                                    doorData.Name;
                            }
                        }

                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        doorData.From = new Vector2(x, y);

                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        doorData.To = new Vector2(x, y);

                        doorData.TriggerList = new List<string>();
                        int numDoorTriggers = inStream.GetIntFromStream();
                        for (int i = 0; i < numDoorTriggers; i++)
                        {
                            //we'll save the trigger id and use it later to
                            //fill in the object name when we process the door
                            //triggers
                            idDoorMap.Add(
                                inStream.GetIntFromStream(),
                                DoorList.Count);
                        }

                        DoorList.Add(doorData);
                        break;

                    case EntityTypes.DoorTrigger:
                        DoorTriggerData doorTriggerData =
                            new DoorTriggerData();

                        doorTriggerData.Name =
                            "DoorTrigger_" + DoorTriggerList.Count;
                        int ravenDoorTriggerId = inStream.GetIntFromStream();
                        DoorList[idDoorMap[ravenDoorTriggerId]].TriggerList.Add(
                            doorTriggerData.Name);
                        int ravenReceiverId = inStream.GetIntFromStream();
                        doorTriggerData.ReceiverName =
                            DoorList[doorIdIndexMap[ravenReceiverId]].Name;
                        doorTriggerData.MessageToSend =
                            (MessageTypes) inStream.GetIntFromStream();
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        doorTriggerData.Position = new Vector2(x, y);
                        doorTriggerData.Radius = inStream.GetFloatFromStream();

                        DoorTriggerList.Add(doorTriggerData);
                        break;

                    case EntityTypes.SpawnPoint:
                        SpawnPointData spawnPointData = new SpawnPointData();

                        spawnPointData.Name =
                            "SpawnPoint_" + SpawnPointList.Count;
                        inStream.SkipNextFieldInStream();
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        spawnPointData.Position = new Vector2(x, y);
                        inStream.SkipNextFieldInStream();
                        inStream.SkipNextFieldInStream();

                        SpawnPointList.Add(spawnPointData);
                        break;

                    case EntityTypes.Health:
                        HealthData healthData = new HealthData();

                        healthData.Name = "Health_" + HealthList.Count;
                        inStream.SkipNextFieldInStream(); //heathId
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        healthData.Position = new Vector2(x, y);
                        healthData.Radius = inStream.GetFloatFromStream();
                        healthData.HealthGiven = inStream.GetIntFromStream();
                        healthData.NodeIndex = inStream.GetIntFromStream();

                        HealthList.Add(healthData);
                        break;

                    case EntityTypes.Shotgun:
                        ShotgunData shotgunData = new ShotgunData();

                        shotgunData.Name = "Shotgun_" + ShotgunList.Count;
                        inStream.SkipNextFieldInStream(); //shotgunId
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        shotgunData.Position = new Vector2(x, y);
                        shotgunData.Radius = inStream.GetFloatFromStream();
                        shotgunData.NodeIndex = inStream.GetIntFromStream();

                        ShotgunList.Add(shotgunData);
                        break;

                    case EntityTypes.Railgun:
                        RailgunData railgunData = new RailgunData();

                        railgunData.Name = "Railgun_" + RailgunList.Count;
                        inStream.SkipNextFieldInStream(); //shotgunId
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        railgunData.Position = new Vector2(x, y);
                        railgunData.Radius = inStream.GetFloatFromStream();
                        railgunData.NodeIndex = inStream.GetIntFromStream();

                        RailgunList.Add(railgunData);
                        break;

                    case EntityTypes.RocketLauncher:
                        RocketLauncherData rocketLauncherData =
                            new RocketLauncherData();

                        rocketLauncherData.Name =
                            "RocketLauncher_" + RocketLauncherList.Count;
                        inStream.SkipNextFieldInStream(); //shotgunId
                        x = inStream.GetFloatFromStream();
                        y = inStream.GetFloatFromStream();
                        rocketLauncherData.Position = new Vector2(x, y);
                        rocketLauncherData.Radius =
                            inStream.GetFloatFromStream();
                        rocketLauncherData.NodeIndex =
                            inStream.GetIntFromStream();

                        RocketLauncherList.Add(rocketLauncherData);
                        break;

                    default:
                        throw new ApplicationException(
                            "MapData.LoadFromRavenMap: Attempting to load undefined object");
                }
            }
        }

        ///<summary>
        ///change map scale
        ///</summary>
        ///<param name="scale"></param>
        public void ChangeScale(float scale)
        {
            if (scale <= 0 || scale == 1)
                return;

            foreach (NodeData node in _nodeList)
            {
                node.Position *= scale;
            }

            foreach (WallData wall in _wallList)
            {
                wall.From *= scale;
                wall.To *= scale;
            }

            foreach (DoorData door in _doorList)
            {
                door.From *= scale;
                door.To *= scale;
            }

            foreach (DoorTriggerData doorTrigger in _doorTriggerList)
            {
                doorTrigger.Position *= scale;
                doorTrigger.Radius *= scale;
            }

            foreach (SpawnPointData spawnPoint in _spawnPointList)
            {
                spawnPoint.Position *= scale;
            }

            foreach (HealthData health in _healthList)
            {
                health.Position *= scale;
                health.Radius *= scale;
            }

            foreach (RailgunData railgun in _railgunList)
            {
                railgun.Position *= scale;
                railgun.Radius *= scale;
            }

            foreach (RocketLauncherData rocketLauncher in _rocketLauncherList)
            {
                rocketLauncher.Position *= scale;
                rocketLauncher.Radius *= scale;
            }

            foreach (ShotgunData shotgun in _shotgunList)
            {
                shotgun.Position *= scale;
                shotgun.Radius *= scale;
            }

            _sizeX = (int) (_sizeX*scale);
            _sizeY = (int) (_sizeY*scale);
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private List<DoorData> _doorList;
        private List<DoorTriggerData> _doorTriggerList;
        private List<EdgeData> _edgeList;
        private List<HealthData> _healthList;
        private string _name;
        private List<NodeData> _nodeList;
        private List<RailgunData> _railgunList;
        private List<RocketLauncherData> _rocketLauncherList;
        private List<ShotgunData> _shotgunList;
        private int _sizeX;
        private int _sizeY;
        private List<SpawnPointData> _spawnPointList;
        private List<WallData> _wallList;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}