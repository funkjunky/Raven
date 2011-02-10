#region File description
//------------------------------------------------------------------------------
//PriorityQueue.cs
//
//Author: Jim Mischel 
//http://www.mischel.com/pubs/priqueue.zip. 
//You are free to use the code in any manner you like.
//
//Modifications Copyright (C) Scott D. Goodwin <Mindcrafters.ca>
//All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#endregion

#region Microsoft
#endregion

#region GarageGames
#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    #region Class PriorityQueue

    ///<summary>
    ///A priority queue item whose value is of type 
    ///<typeparamref name="TValue"/> and whose priority is of type
    ///<typeparamref name="TPriority"/>
    ///</summary>
    ///<typeparam name="TValue"></typeparam>
    ///<typeparam name="TPriority"></typeparam>
    [Serializable]
    [ComVisible(false)]
    public struct PriorityQueueItem<TValue, TPriority>
    {
        private readonly TValue _value;

        ///<summary>
        ///Accessor for <see cref="_value"/>
        ///</summary>
        public TValue Value
        {
            get { return _value; }
        }

        private readonly TPriority _priority;

        ///<summary>
        ///Accessor for <see cref="_priority"/>
        ///</summary>
        public TPriority Priority
        {
            get { return _priority; }
        }

        internal PriorityQueueItem(TValue val, TPriority pri)
        {
            _value = val;
            _priority = pri;
        }
    }

    ///<summary>
    ///A Priority Queue class
    ///</summary>
    ///<typeparam name="TValue"></typeparam>
    ///<typeparam name="TPriority"></typeparam>
    [Serializable]
    [ComVisible(false)]
    public class PriorityQueue<TValue, TPriority> : ICollection,
        IEnumerable<PriorityQueueItem<TValue, TPriority>>
    {
        ///<summary>
        ///Priority order (highest first or lowest first)
        ///</summary>
        public enum PriorityOrder { HighFirst, LowFirst };

        private PriorityQueueItem<TValue, TPriority>[] _items;

        private const Int32 DEFAULT_CAPACITY = 16;
        private Int32 _capacity;
        private Int32 _numItems;
        private int _prioritySign;

        private Comparison<TPriority> _compareFunc;

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the default initial capacity, and uses the default comparer and
        ///is ordered high first.
        ///<see cref="IComparer"/>.
        ///</summary>
        public PriorityQueue()
            : this(
                DEFAULT_CAPACITY, 
                Comparer<TPriority>.Default, 
                PriorityOrder.HighFirst)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the default comparer and
        ///is ordered high first.
        ///</summary>
        ///<param name="initialCapacity"></param>
        public PriorityQueue(Int32 initialCapacity)
            : this(
                initialCapacity, 
                Comparer<TPriority>.Default, 
                PriorityOrder.HighFirst)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the default initial capacity, and uses the given comparer and
        ///is ordered high first.
        ///</summary>
        ///<param name="comparer"></param>
        public PriorityQueue(IComparer<TPriority> comparer)
            : this(DEFAULT_CAPACITY, comparer, PriorityOrder.HighFirst)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the given comparer and
        ///is ordered high first.
        ///</summary>
        ///<param name="initialCapacity"></param>
        ///<param name="comparer"></param>
        public PriorityQueue(int initialCapacity, IComparer<TPriority> comparer)
        {
            Init(initialCapacity, comparer.Compare, PriorityOrder.HighFirst);
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the default initial capacity, and uses the given comparison and
        ///is ordered high first.
        ///</summary>
        ///<param name="comparison"></param>
        public PriorityQueue(Comparison<TPriority> comparison)
            : this(DEFAULT_CAPACITY, comparison, PriorityOrder.HighFirst)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the given comparison and
        ///is ordered high first.
        ///</summary>
        ///<param name="initialCapacity"></param>
        ///<param name="comparison"></param>
        public PriorityQueue(
            int initialCapacity, 
            Comparison<TPriority> comparison)
        {
            Init(initialCapacity, comparison, PriorityOrder.HighFirst);
        }


        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the default comparer and
        ///has the given priority order.
        ///</summary>
        ///<param name="initialCapacity"></param>
        ///<param name="priorityOrder"></param>
        public PriorityQueue(Int32 initialCapacity, PriorityOrder priorityOrder)
            : this(initialCapacity, Comparer<TPriority>.Default, priorityOrder)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the default initial capacity, and uses the given comparer and
        ///has the given priority order.
        ///</summary>
        ///<param name="comparer"></param>
        ///<param name="priorityOrder"></param>
        public PriorityQueue(
            IComparer<TPriority> comparer, 
            PriorityOrder priorityOrder)
            : this(DEFAULT_CAPACITY, comparer, priorityOrder)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the given comparer and
        ///has the given priority order.
        ///</summary>
        ///<param name="initialCapacity"></param>
        ///<param name="comparer"></param>
        ///<param name="priorityOrder"></param>
        public PriorityQueue(
            int initialCapacity, 
            IComparer<TPriority> comparer, 
            PriorityOrder priorityOrder)
        {
            Init(initialCapacity, comparer.Compare, priorityOrder);
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the default initial capacity, and uses the given comparison and
        ///has the given priority order.
        ///</summary>
        ///<param name="comparison"></param>
        ///<param name="priorityOrder"></param>
        public PriorityQueue(
            Comparison<TPriority> comparison, 
            PriorityOrder priorityOrder)
            : this(DEFAULT_CAPACITY, comparison, priorityOrder)
        {
        }

        ///<summary>
        ///Initializes a new instance of the PriorityQueue class that is empty,
        ///has the given initial capacity, and uses the given comparison and
        ///has the given priority order.
        ///</summary>
        ///<param name="initialCapacity"></param>
        ///<param name="comparison"></param>
        ///<param name="priorityOrder"></param>
        public PriorityQueue(
            int initialCapacity, 
            Comparison<TPriority> comparison, 
            PriorityOrder priorityOrder)
        {
            Init(initialCapacity, comparison, priorityOrder);
        }

        private void Init(
            int initialCapacity, 
            Comparison<TPriority> comparison, 
            PriorityOrder priorityOrder)
        {
            _numItems = 0;
            _compareFunc = comparison;
            SetCapacity(initialCapacity);
            //multiplier to apply to result of compareFunc
            //1 for high priority first, -1 for low priority first
            _prioritySign = (priorityOrder == PriorityOrder.HighFirst) ? 1 : -1;
        }

        ///<summary>
        ///Gets number of items in the priority queue
        ///</summary>
        public int Count
        {
            get { return _numItems; }
        }

        ///<summary>
        ///Capacity of the priority queue
        ///</summary>
        public int Capacity
        {
            get { return _items.Length; }
            set { SetCapacity(value); }
        }

        private void SetCapacity(int newCapacity)
        {
            int newCap = newCapacity;
            if (newCap < DEFAULT_CAPACITY)
                newCap = DEFAULT_CAPACITY;

            //throw exception if newCapacity < NumItems
            if (newCap < _numItems)
                throw new ArgumentOutOfRangeException(
                    "newCapacity", "New capacity is less than Count");

            _capacity = newCap;
            if (_items == null)
            {
                _items = new PriorityQueueItem<TValue, TPriority>[newCap];
                return;
            }

            //Resize the array.
            Array.Resize(ref _items, newCap);
        }

        ///<summary>
        ///Enqueue <paramref name="newItem"/>
        ///</summary>
        ///<param name="newItem"></param>
        public void Enqueue(PriorityQueueItem<TValue, TPriority> newItem)
        {
            if (_numItems == _capacity)
            {
                //need to increase capacity
                //grow by 50 percent
                SetCapacity((3 * Capacity) / 2);
            }

            int i = _numItems;
            ++_numItems;
            while ((i > 0) &&
                (_prioritySign * 
                _compareFunc(_items[(i - 1) / 2].Priority, newItem.Priority) < 0))
            {
                _items[i] = _items[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            _items[i] = newItem;
            //if (!VerifyQueue())
            //{
            //  Console.WriteLine("ERROR: Queue out of order!");
            //}
        }

        ///<summary>
        ///Enqueue <paramref name="value"/> with <paramref name="priority"/>
        ///</summary>
        ///<param name="value"></param>
        ///<param name="priority"></param>
        public void Enqueue(TValue value, TPriority priority)
        {
            Enqueue(new PriorityQueueItem<TValue, TPriority>(value, priority));
        }

        private PriorityQueueItem<TValue, TPriority> RemoveAt(Int32 index)
        {
            PriorityQueueItem<TValue, TPriority> o = _items[index];
            --_numItems;
            //move the last item to fill the hole
            PriorityQueueItem<TValue, TPriority> tmp = _items[_numItems];
            //If you forget to clear this, you have a potential memory leak.
            _items[_numItems] = default(PriorityQueueItem<TValue, TPriority>);
            if (_numItems > 0 && index != _numItems)
            {
                //If the new item is greater than its parent, bubble up.
                int i = index;
                int parent = (i - 1) / 2;
                while (_prioritySign * 
                    _compareFunc(tmp.Priority, _items[parent].Priority) > 0)
                {
                    _items[i] = _items[parent];
                    i = parent;
                    parent = (i - 1) / 2;
                }

                //if i == index, then we didn't move the item up
                if (i == index)
                {
                    //bubble down ...
                    while (i < (_numItems) / 2)
                    {
                        int j = (2 * i) + 1;
                        if ((j < _numItems - 1) && 
                            (_prioritySign * 
                            _compareFunc(_items[j].Priority, _items[j + 1].Priority) < 0))
                        {
                            ++j;
                        }
                        if (_prioritySign * 
                            _compareFunc(_items[j].Priority, tmp.Priority) <= 0)
                        {
                            break;
                        }
                        _items[i] = _items[j];
                        i = j;
                    }
                }
                //Be sure to store the item in its place.
                _items[i] = tmp;
            }
            //if (!VerifyQueue())
            //{
            //  Console.WriteLine("ERROR: Queue out of order!");
            //}
            return o;
        }

        ///<summary>
        ///Function to check that the queue is coherent.
        ///</summary>
        ///<returns></returns>
        public bool VerifyQueue()
        {
            int i = 0;
            while (i < _numItems / 2)
            {
                int leftChild = (2 * i) + 1;
                int rightChild = leftChild + 1;
                if (_prioritySign * 
                    _compareFunc(_items[i].Priority, _items[leftChild].Priority) < 0)
                {
                    return false;
                }
                if (rightChild < _numItems && 
                    _prioritySign * 
                    _compareFunc(_items[i].Priority, _items[rightChild].Priority) < 0)
                {
                    return false;
                }
                ++i;
            }
            return true;
        }

        ///<summary>
        ///Dequeue
        ///</summary>
        ///<returns>item (value and priority)</returns>
        ///<exception cref="InvalidOperationException">empty queue</exception>
        public PriorityQueueItem<TValue, TPriority> Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException("The queue is empty");
            return RemoveAt(0);
        }

        ///<summary>
        ///Removes the item with the specified value from the queue.
        ///The passed equality comparison is used.
        ///</summary>
        ///<param name="item">The item to be removed.</param>
        ///<param name="comparer">
        ///An object that implements the <see cref="IEqualityComparer"/>
        ///interface for the type of item in the collection.
        ///</param>
        public void Remove(TValue item, IEqualityComparer comparer)
        {
            //need to find the PriorityQueueItem that has the Data value of item
            for (int index = 0; index < _numItems; ++index)
            {
                if (!comparer.Equals(item, _items[index].Value)) 
                    continue;

                RemoveAt(index);
                return;
            }
            throw new ApplicationException(
                "The specified item is not in the queue.");
        }

        ///<summary>
        ///Removes the item with the specified value from the queue.
        ///The default type comparison function is used.
        ///</summary>
        ///<param name="item">The item to be removed.</param>
        public void Remove(TValue item)
        {
            Remove(item, EqualityComparer<TValue>.Default);
        }

        ///<summary>
        ///Get (peek but not dequeue) first item
        ///</summary>
        ///<returns>first item in the priority queue</returns>
        ///<exception cref="InvalidOperationException">empty queue</exception>
        public PriorityQueueItem<TValue, TPriority> Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException("The queue is empty");
            return _items[0];
        }

        ///<summary>
        ///Clear priority queue
        ///</summary>
        public void Clear()
        {
            for (int i = 0; i < _numItems; ++i)
            {
                _items[i] = default(PriorityQueueItem<TValue, TPriority>);
            }
            _numItems = 0;
            TrimExcess();
        }

        ///<summary>
        ///Set the capacity to the actual number of items, if the current
        ///number of items is less than 90 percent of the current capacity.
        ///</summary>
        public void TrimExcess()
        {
            if (_numItems < (float)0.9 * _capacity)
            {
                SetCapacity(_numItems);
            }
        }

        ///<summary>
        ///Tests if priority queue contains the given value
        ///</summary>
        ///<param name="value"></param>
        ///<returns></returns>
        public bool Contains(TValue value)
        {
            foreach (PriorityQueueItem<TValue, TPriority> x in _items)
            {
                if (x.Value.Equals(value))
                    return true;
            }
            return false;
        }

        ///<summary>
        ///Copy to array starting at given array index.
        ///</summary>
        ///<param name="array"></param>
        ///<param name="arrayIndex"></param>
        ///<exception cref="ArgumentNullException"></exception>
        ///<exception cref="ArgumentOutOfRangeException"></exception>
        ///<exception cref="ArgumentException"></exception>
        public void CopyTo(
            PriorityQueueItem<TValue, TPriority>[] array, 
            int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(
                    "arrayIndex", "arrayIndex is less than 0.");
            if (array.Rank > 1)
                throw new ArgumentException("array is multidimensional.");
            if (_numItems == 0)
                return;
            if (arrayIndex >= array.Length)
                throw new ArgumentException(
                    "arrayIndex is equal to or greater than the length" +
                    " of the array.");
            if (_numItems > (array.Length - arrayIndex))
                throw new ArgumentException(
                    "The number of elements in the source ICollection is" +
                    " greater than the available space from arrayIndex to" +
                    " the end of the destination array.");

            for (int i = 0; i < _numItems; i++)
            {
                array[arrayIndex + i] = _items[i];
            }
        }

        #region ICollection Members

        ///<summary>
        ///Copy to array starting at given index.
        ///</summary>
        ///<param name="array"></param>
        ///<param name="index"></param>
        public void CopyTo(Array array, int index)
        {
            CopyTo((PriorityQueueItem<TValue, TPriority>[])array, index);
        }

        ///<summary>
        ///Gets a value indicating whether access to this priority queue is
        ///synchronized (thread safe).
        ///</summary>
        public bool IsSynchronized
        {
            get { return false; }
        }

        ///<summary>
        ///Gets an object that can be used to synchronize access to this
        ///priority queue.
        ///</summary>
        public object SyncRoot
        {
            get { return _items.SyncRoot; }
        }

        #endregion

        #region IEnumerable<PriorityQueueItem<TValue,TPriority>> Members

        ///<summary>
        ///Returns an enumerator that iterates through this priority queue.
        ///</summary>
        ///<returns>
        ///an enumerator that iterates through this priority queue.
        ///</returns>
        public IEnumerator<PriorityQueueItem<TValue, TPriority>> GetEnumerator()
        {
            for (int i = 0; i < _numItems; i++)
            {
                yield return _items[i];
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }

    #endregion
}

/////////////Another version /////////////

//  ///<summary>
//  ///basic heap based priority queue implementation
//  ///</summary>
//  public class PriorityQueue
//  {
//      //======================================================================
//      #region Static methods, fields, constructors

//      ////----------------------- Swap -------------------------------------------
//      ////
//      ////used to swap two values
//      ////------------------------------------------------------------------------
//      //static public void Swap(IList<float> list, int left, int right)
//      //{
//      //  float temp = list[left];
//      //  list[left] = list[right];
//      //  list[right] = temp;
//      //}

//      ////-------------------- ReorderUpwards ------------------------------------
//      ////
//      ////given a heap and a node in the heap, this function moves upwards
//      ////through the heap swapping elements until the heap is ordered
//      ////------------------------------------------------------------------------
//      //static public void ReorderUpwards(List<float> heap, int nd)
//      //{
//      //  //move up the heap swapping the elements until the heap is ordered
//      //  while ((nd > 1) && (heap[nd / 2] < heap[nd]))
//      //  {
//      //      Swap(heap, nd/2, nd);
//      //      nd /= 2;
//      //  }
//      //}

//      ////--------------------- ReorderDownwards ---------------------------------
//      ////
//      ////given a heap, the heapsize and a node in the heap, this function
//      ////reorders the elements in a top down fashion by moving down the heap
//      ////and swapping the current node with the greater of its two children
//      ////(provided a child is larger than the current node)
//      ////------------------------------------------------------------------------
//      //static public void ReorderDownwards(List<float> heap, int nd, int heapSize)
//      //{
//      ////move down the heap from node nd swapping the elements until
//      ////the heap is reordered
//      //while (2*nd <= heapSize)
//      //{
//      //  int child = 2 * nd;

//      //  //set child to largest of nd's two children
//      //  if ( (child < heapSize) && (heap[child] < heap[child+1]) )
//      //  {
//      //    ++child;
//      //  }

//      //  //if this nd is smaller than its child, swap
//      //  if (heap[nd] < heap[child])
//      //  {
//      //    Swap(heap, child, nd);

//      //    //move the current node down the tree
//      //    nd = child;
//      //  }
//      //  else
//      //  {
//      //    break;
//      //  }
//      //}
//      //}

//      #endregion

//      //======================================================================
//      #region Constructors

//      public PriorityQueue(int maxSize)
//      {
//          _maxSize = maxSize;
//          _size = 0;
//          _heap = new List<float>(maxSize+1);
//      }

//      #endregion

//      //======================================================================
//      #region Public properties, operators, constants, and enums
//      #endregion

//      //======================================================================
//      #region Public methods

//      public bool Empty()
//      {
//          return (_size==0);
//      }

//      //to insert an item into the queue it gets added to the end of the heap
//      //and then the heap is reordered
//      public void Insert(float item)
//      {

//          Assert.Fatal(_size+1 <= _maxSize, "PriorityQueue.Insert: overflow");

//          ++_size;

//          _heap[_size] = item;

//          ReorderUpwards(_heap, _size);
//      }

//      //to get the max item the first element is exchanged with the lowest
//      //in the heap and then the heap is reordered from the top down. 
//      public float Pop()
//      {
//          Swap(1, _size);

//          ReorderDownwards(_heap, 1, _size - 1);

//          return _heap[_size--];
//      }

//      //so we can take a peek at the first in line
//      public float Peek()
//      {
//          return _heap[1];
//      }

//      #endregion

//      //======================================================================
//      #region Private, protected, internal methods

//      void Swap(int a, int b)
//      {
//          float temp = _heap[a];
//          _heap[a] = _heap[b];
//          _heap[b] = temp;
//      }

//      //given a heap and a node in the heap, this function moves upwards
//      //through the heap swapping elements until the heap is ordered
//      void ReorderUpwards(List<float> heap, int nd)
//      {
//          //move up the heap swapping the elements until the heap is ordered
//          while ( (nd>1) && (heap[nd/2] < heap[nd]))
//          {
//            Swap(nd/2, nd);
//            nd /= 2;
//          }
//      }

//      //given a heap, the heapsize and a node in the heap, this function
//      //reorders the elements in a top down fashion by moving down the heap
//      //and swapping the current node with the greater of its two children
//      //(provided a child is larger than the current node)
//      void ReorderDownwards(List<float> heap, int nd, int heapSize)
//      {
//          //move down the heap from node nd swapping the elements until
//          //the heap is reordered
//          while (2 * nd <= heapSize)
//          {
//              int child = 2 * nd;

//              //set child to largest of nd's two children
//              if ((child < heapSize) && (heap[child] < heap[child + 1]))
//              {
//                  ++child;
//              }

//              //if this nd is smaller than its child, swap
//              if (heap[nd] < heap[child])
//              {
//                  Swap(child, nd);

//                  //move the current node down the tree
//                  nd = child;
//              }
//              else
//              {
//                  break;
//              }
//          }
//      }

//      #endregion

//      //======================================================================
//      #region Private, protected, internal fields

//      List<float> _heap;

//      int _size;
//      int _maxSize;

//      #endregion
//  }
