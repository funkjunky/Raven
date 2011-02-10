#region File description

//------------------------------------------------------------------------------
//CellSpacePartition.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System.Collections.Generic;
using GarageGames.Torque.Core;
using Microsoft.Xna.Framework;
using Mindcrafters.Library.Math;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Graph
{
    ///<summary>
    ///the subdivision class
    ///TODO: this will get phased out as we transition to using Torque's
    ///object database which has its own bin system.
    ///</summary>
    public class CellSpacePartition
    {
        #region Nested type: Cell

        ///<summary>
        ///defines a cell containing a list of pointers to entities
        ///</summary>
        public struct Cell
        {
            private InvertedAABBox2D _boundingBox;
            private List<NavGraphNode> _members;

            ///<summary>
            ///constructor
            ///</summary>
            ///<param name="topLeft"></param>
            ///<param name="bottomRight"></param>
            public Cell(Vector2 topLeft, Vector2 bottomRight)
            {
                _members = new List<NavGraphNode>();
                _boundingBox = new InvertedAABBox2D(topLeft, bottomRight);
            }

            ///<summary>
            ///all the entities inhabiting this cell
            ///</summary>
            public List<NavGraphNode> Members
            {
                get { return _members; }
                set { _members = value; }
            }

            ///<summary>
            ///the cell's bounding box (it's inverted because the Torque
            ///default co-ordinate system has a y axis that increases as it
            ///descends)
            ///</summary>
            public InvertedAABBox2D BoundingBox
            {
                get { return _boundingBox; }
                set { _boundingBox = value; }
            }
        }

        #endregion

        #region Static methods, fields, constructors

        #endregion

        #region Constructors

        ///<summary>
        ///
        ///</summary>
        ///<param name="width"></param>
        ///<param name="height"></param>
        ///<param name="cellsX"></param>
        ///<param name="cellsY"></param>
        ///<param name="maxEntities"></param>
        public CellSpacePartition(
            float width, //width of 2D space
            float height, //height...
            int cellsX, //number of divisions horizontally
            int cellsY, //and vertically
            int maxEntities) //maximum number of entities to partition
        {
            _spaceWidth = width;
            _spaceHeight = height;
            _numCellsX = cellsX;
            _numCellsY = cellsY;
            _neighbors = new List<NavGraphNode>(maxEntities + 1);
            for (int i = 0; i < maxEntities + 1; i++)
                Neighbors.Add(null);

            //calculate bounds of each cell
            _cellSizeX = width/cellsX;
            _cellSizeY = height/cellsY;

            //create the cells
            for (int y = 0; y < NumCellsY; ++y)
            {
                for (int x = 0; x < NumCellsX; ++x)
                {
                    float left = x*CellSizeX;
                    float right = left + CellSizeX;
                    float top = y*CellSizeY;
                    float bot = top + CellSizeY;

                    Cells.Add(
                        new Cell(
                            new Vector2(left, top),
                            new Vector2(right, bot)));
                }
            }
        }

        ///<summary>
        ///the required amount of cells in the space
        ///</summary>
        public List<Cell> Cells
        {
            get { return _cells; }
        }

        ///<summary>
        ///this is used to store any valid neighbors when an agent searches
        ///its neighboring space
        ///</summary>
        public List<NavGraphNode> Neighbors
        {
            get { return _neighbors; }
        }

        ///<summary>
        ///the width of the world space the entities inhabit
        ///</summary>
        public float SpaceWidth
        {
            get { return _spaceWidth; }
        }

        ///<summary>
        ///the height of the world space the entities inhabit
        ///</summary>
        public float SpaceHeight
        {
            get { return _spaceHeight; }
        }

        ///<summary>
        ///the number of cell rows the space is going to be divided up into
        ///</summary>
        public int NumCellsX
        {
            get { return _numCellsX; }
        }

        ///<summary>
        ///the number of cell columns the space is going to be divided up into
        ///</summary>
        public int NumCellsY
        {
            get { return _numCellsY; }
        }

        ///<summary>
        ///The width of a cell
        ///</summary>
        public float CellSizeX
        {
            get { return _cellSizeX; }
        }

        ///<summary>
        ///The height of a cell
        ///</summary>
        public float CellSizeY
        {
            get { return _cellSizeY; }
        }

        ///<summary>
        ///At end of list (set by enumerator)
        ///</summary>
        public bool AtEnd
        {
            get { return _atEnd; }
            set { _atEnd = value; }
        }

        #endregion

        #region Public methods

        ///<summary>
        ///This must be called to create the list of neighbors. This method 
        ///examines each cell within range of the target, If the cells contain
        ///entities then they are tested to see if they are situated within the
        ///target's neighborhood region. If they are they are added to
        ///neighbor list
        ///</summary>
        ///<param name="targetPos"></param>
        ///<param name="queryRadius"></param>
        public void CalculateNeighbors(Vector2 targetPos, float queryRadius)
        {
            //create an iterator and set it to the beginning of the
            //neighbor list
            int curNbor = 0;

            //create the query box that is the bounding box of the
            //target's query area
            InvertedAABBox2D queryBox =
                new InvertedAABBox2D(
                    targetPos - new Vector2(queryRadius, queryRadius),
                    targetPos + new Vector2(queryRadius, queryRadius));

            //iterate through each cell and test to see if its bounding box
            //overlaps with the query box. If it does and it also contains
            //entities then make further proximity tests.
            foreach (Cell curCell in Cells)
            {
                //test to see if this cell contains members and if it overlaps
                //the query box
                if (!curCell.BoundingBox.IsOverlappedWith(queryBox) ||
                    curCell.Members.Count <= 0)
                    continue;

                //add any entities found within query radius to the neighbor list
                foreach (NavGraphNode curNode in curCell.Members)
                {
                    if (GraphNode.IsInvalidIndex(curNode.Index))
                        continue;

                    if (Vector2.DistanceSquared(curNode.Position, targetPos) >=
                        queryRadius*queryRadius)
                        continue;

                    Neighbors[curNbor] = curNode;
                    curNbor++;
                }
            }

            //mark the end of the list with a null.
            Neighbors[curNbor] = null;
        }

        ///<summary>
        ///clears the cells of all entities
        ///</summary>
        public void EmptyCells()
        {
            foreach (Cell curCell in Cells)
            {
                curCell.Members.Clear();
            }
        }

        ///<summary>
        ///Given a position within the game world, this method calculates an
        ///index into its appropriate cell
        ///</summary>
        ///<param name="pos"></param>
        ///<returns></returns>
        public int PositionToIndex(Vector2 pos)
        {
            int idx = (int) (NumCellsX*pos.X/SpaceWidth) +
                      ((int) ((NumCellsY)*pos.Y/SpaceHeight)*NumCellsX);

            //if the entity's position is equal to
            //Vector2(_spaceWidth, _spaceHeight) then the index will overshoot.
            //We need to check for this and adjust
            if (idx > Cells.Count - 1)
            {
                idx = Cells.Count - 1;
            }

            return idx;
        }

        ///<summary>
        ///Used to add the entities to the data structure
        ///</summary>
        ///<param name="ent"></param>
        public void AddEntity(NavGraphNode ent)
        {
            Assert.Fatal(ent != null,
                         "CellSpacePartition.AddEntity: ent is null");

            if (ent == null)
                return;

            int idx = PositionToIndex(ent.Position);
            Cells[idx].Members.Add(ent);
        }

        ///<summary>
        ///Checks to see if an entity has moved cells. If so the data structure
        ///is updated accordingly
        ///</summary>
        ///<param name="ent"></param>
        ///<param name="oldPos"></param>
        public void UpdateEntity(NavGraphNode ent, Vector2 oldPos)
        {
            //if the index for the old pos and the new pos are not equal then
            //the entity has moved to another cell.
            int oldIdx = PositionToIndex(oldPos);
            int newIdx = PositionToIndex(ent.Position);

            if (newIdx == oldIdx) return;

            //the entity has moved into another cell so delete from current
            //cell and add to new one
            Cells[oldIdx].Members.Remove(ent);
            Cells[newIdx].Members.Add(ent);
        }

        ///<summary>
        ///render the cells
        ///</summary>
        public void RenderCells()
        {
            foreach (Cell curCell in Cells)
            {
                curCell.BoundingBox.Render(false);
            }
        }

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        private readonly List<Cell> _cells = new List<Cell>();
        private readonly float _cellSizeX;
        private readonly float _cellSizeY;
        private readonly List<NavGraphNode> _neighbors;
        private readonly int _numCellsX;
        private readonly int _numCellsY;
        private readonly float _spaceHeight;
        private readonly float _spaceWidth;
        private bool _atEnd;
        private List<NavGraphNode>.Enumerator _curNeighbor;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}