#region File description
//------------------------------------------------------------------------------
//IndexedPriorityQueueLow.cs
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
    #region Class IndexedPriorityQueueLow

    ///<summary>
    ///Priority queue based on an index into a set of keys. The queue is
    ///maintained as a 2-way heap.
    ///
    ///The priority in this implementation is the lowest valued key
    ///</summary>
    public class IndexedPriorityQueueLow
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///Create a new indexed priority queue given the list that
        ///will be indexed into and the maximum size of the queue.
        ///</summary>
        ///<param name="keys"></param>
        ///<param name="maxSize"></param>
        public IndexedPriorityQueueLow(List<float> keys, int maxSize)
        {
            _keys = keys;
            _maxSize = maxSize;
            _size = 0;
            _heap = new List<int>(maxSize + 1);
            for (int i = 0; i < maxSize + 1; i++) _heap.Add(0);
            _invHeap = new List<int>(maxSize + 1);
            for (int i = 0; i < maxSize + 1; i++) _invHeap.Add(0);
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Tests if the priority queue is empty
        ///</summary>
        ///<returns>true if the priority queue is empty</returns>
        public bool Empty()
        {
            return (_size == 0);
        }

        ///<summary>
        ///to insert an item into the queue it gets added to the end of the heap
        ///and then the heap is reordered from the bottom up.
        ///</summary>
        ///<param name="idx"></param>
        public void Insert(int idx)
        {
            Assert.Fatal(_size + 1 <= _maxSize, 
                "IndexedPriorityQueue.Insert: overflow");

            ++_size;

            _heap[_size] = idx;

            _invHeap[idx] = _size;

            ReorderUpwards(_size);
        }

        ///<summary>
        ///to get the min item the first element is exchanged with the lowest
        ///in the heap and then the heap is reordered from the top down. 
        ///</summary>
        ///<returns></returns>
        public int Pop()
        {
            Swap(1, _size);

            ReorderDownwards(1, _size - 1);

            return _heap[_size--];
        }

        ///<summary>
        ///if the value of one of the client key's changes then call this with 
        ///the key's index to adjust the queue accordingly
        ///</summary>
        ///<param name="idx"></param>
        public void ChangePriority(int idx)
        {
            ReorderUpwards(_invHeap[idx]);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        void Swap(int a, int b)
        {
            int temp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = temp;

            //change the handles too
            _invHeap[_heap[a]] = a;
            _invHeap[_heap[b]] = b;
        }

        void ReorderUpwards(int nodeIndex)
        {
            //move up the heap swapping the elements until the heap is ordered
            while ((nodeIndex > 1) && 
                (_keys[_heap[nodeIndex / 2]] > _keys[_heap[nodeIndex]]))
            {
                Swap(nodeIndex / 2, nodeIndex);
                nodeIndex /= 2;
            }
        }

        void ReorderDownwards(int nodeIndex, int heapSize)
        {
            //move down the heap from node nodeIndex swapping the elements until
            //the heap is reordered
            while (2 * nodeIndex <= heapSize)
            {
                int child = 2 * nodeIndex;

                //set child to smaller of nodeIndex's two children
                if ((child < heapSize) && 
                    (_keys[_heap[child]] > _keys[_heap[child + 1]]))
                {
                    ++child;
                }

                //if this nodeIndex is larger than its child, swap
                if (_keys[_heap[nodeIndex]] <= _keys[_heap[child]])
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

        readonly List<float> _keys;
        readonly List<int> _heap;
        readonly List<int> _invHeap;

        int _size;
        readonly int _maxSize;

        #endregion
    }

    #endregion
}
