#region File description
//------------------------------------------------------------------------------
//PriorityQueueLow.cs
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

using GarageGames.Torque.Core;

#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    #region Class PriorityQueueLow

    ///<summary>
    ///basic 2-way heap based priority queue implementation. The priority
    ///is given to the lowest valued key
    ///</summary>
    public class PriorityQueueLow
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor
        ///</summary>
        ///<param name="maxSize"></param>
        public PriorityQueueLow(int maxSize)
        {
            _size = 0;
            _heap = new List<float>(maxSize + 1);
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Tests if queue is empty
        ///</summary>
        ///<returns>true if queue is empty</returns>
        public bool Empty()
        {
            return (_size == 0);
        }

        ///<summary>
        ///to insert an item into the queue it gets added to the end of the heap
        ///and then the heap is reordered
        ///</summary>
        ///<param name="item">item to add</param>
        public void Insert(float item)
        {
            Assert.Fatal(_size + 1 <= _size, 
                "PriorityQueueLow.Insert: overflow");

            ++_size;

            _heap[_size] = item;

            ReorderUpwards(_heap, _size);
        }

        ///<summary>
        ///to get the min item the first element is exchanged with the lowest
        ///in the heap and then the heap is reordered from the top down. 
        ///</summary>
        ///<returns></returns>
        public float Pop()
        {
            Swap(1, _size);

            ReorderDownwards(_heap, 1, _size - 1);

            return _heap[_size--];
        }

        ///<summary>
        ///so we can take a peek at the first in line
        ///</summary>
        ///<returns></returns>
        public float Peek()
        {
            return _heap[1];
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        void Swap(int a, int b)
        {
            float temp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = temp;
        }

        ///<summary>
        ///given a heap and a node in the heap, this function moves upwards
        ///through the heap swapping elements until the heap is ordered 
        ///</summary>
        ///<param name="heap"></param>
        ///<param name="nd"></param>
        void ReorderUpwards(IList<float> heap, int nd)
        {
            //move up the heap swapping the elements until the heap is ordered
            while ((nd > 1) && (heap[nd / 2] > heap[nd]))
            {
                Swap(nd / 2, nd);
                nd /= 2;
            }
        }

        ///<summary>
        ///given a heap, the <paramref name="heapSize"/> and a node in the heap,
        ///this function reorders the elements in a top down fashion by moving
        ///down the heap and swapping the current node with the smaller of its
        ///two children (provided a child is larger than the current node)
        ///</summary>
        ///<param name="heap"></param>
        ///<param name="nodeIndex"></param>
        ///<param name="heapSize"></param>
        public void ReorderDownwards(
            List<float> heap, 
            int nodeIndex, 
            int heapSize)
        {
            //move down the heap from node nodeIndex swapping the elements until
            //the heap is reordered
            while (2 * nodeIndex <= heapSize)
            {
                int child = 2 * nodeIndex;

                //set child to largest of nodeIndex's two children
                if ((child < heapSize) && (heap[child] > heap[child + 1]))
                {
                    ++child;
                }

                //if this nodeIndex is smaller than its child, swap
                if (heap[nodeIndex] <= heap[child])
                {
                    break;
                }
                Swap(child, nodeIndex);

                //move the current node down the tree
                nodeIndex = child;
            }
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        readonly List<float> _heap;
        int _size;

        #endregion
    }

    #endregion
}
