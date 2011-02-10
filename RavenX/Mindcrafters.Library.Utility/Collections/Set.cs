#region File description
//------------------------------------------------------------------------------
//Set.cs
//
//Copyright (c) 2004, Rüdiger Klaehn
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without 
//modification, are permitted provided that the following conditions are met:
//
//* Redistributions of source code must retain the above copyright notice, this
//list of conditions and the following disclaimer.
//* Redistributions in binary form must reproduce the above copyright notice,
//this list of conditions and the following disclaimer in the documentation
//and/or other materials provided with the distribution.
//* Neither the name of lambda computing nor the names of its contributors may
//be used to endorse or promote products derived from this software without
//specific prior written permission.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
//AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
//IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
//ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
//LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
//SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//POSSIBILITY OF SUCH DAMAGE.
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

#endregion

#endregion

namespace Mindcrafters.Library.Utility
{
    #region Class Set

    ///<summary>
    ///A Set class
    ///</summary>
    ///<typeparam name="T">element type</typeparam>
    [Serializable]
    public class Set<T> : ICollection<T>, ICollection
    {
        private struct Dummy { }

        private static readonly Dummy _dummy = new Dummy();
        private readonly Dictionary<T, Dummy> _data;

        ///<summary>
        ///constructor for Set
        ///</summary>
        public Set()
        {
            _data = new Dictionary<T, Dummy>();
        }

        ///<summary>
        ///constructor for Set
        ///</summary>
        ///<param name="capacity">capacity</param>
        public Set(int capacity)
        {
            _data = new Dictionary<T, Dummy>(capacity);
        }

        ///<summary>
        ///copy constructor for Set
        ///</summary>
        ///<param name="original">original set</param>
        public Set(Set<T> original)
        {
            _data = new Dictionary<T, Dummy>(original._data);
        }

        ///<summary>
        ///copy constructor for Set
        ///</summary>
        ///<param name="original"></param>
        public Set(IEnumerable<T> original)
        {
            _data = new Dictionary<T, Dummy>();
            AddRange(original);
        }

        ///<summary>
        ///number of elements in the set
        ///</summary>
        public int Count
        {
            get { return _data.Count; }
        }

        ///<summary>
        ///Add element <paramref name="a"/> to the set
        ///</summary>
        ///<param name="a">element to add</param>
        public void Add(T a)
        {
            _data[a] = _dummy;
        }

        ///<summary>
        ///Add a range of elements to the set
        ///</summary>
        ///<param name="range">the range to add</param>
        public void AddRange(IEnumerable<T> range)
        {
            foreach (T a in range)
                Add(a);
        }

        ///<summary>
        ///Create new set of type <typeparamref name="U"/> from this set
        ///using the type converter <paramref name="converter"/>
        ///</summary>
        ///<param name="converter">type converter</param>
        ///<typeparam name="U">type of new set</typeparam>
        ///<returns>new set of type <typeparamref name="U"/></returns>
        public Set<U> ConvertAll<U>(Converter<T, U> converter)
        {
            Set<U> result = new Set<U>(Count);
            foreach (T element in this)
                result.Add(converter(element));
            return result;
        }

        ///<summary>
        ///Tests if predicate <paramref name="predicate"/> is true for all
        ///elements of this set
        ///</summary>
        ///<param name="predicate">a predicate</param>
        ///<returns>
        ///true if predicate <paramref name="predicate"/> is true for all
        ///elements of this set
        ///</returns>
        public bool TrueForAll(Predicate<T> predicate)
        {
            foreach (T element in this)
                if (!predicate(element))
                    return false;
            return true;
        }

        ///<summary>
        ///Find all elements of this set that satisfy the
        ///predicate <paramref name="predicate"/>
        ///</summary>
        ///<param name="predicate">a predicate</param>
        ///<returns>
        ///the set of elements from this set that satisfy the
        ///predicate <paramref name="predicate"/>
        ///</returns>
        public Set<T> FindAll(Predicate<T> predicate)
        {
            Set<T> result = new Set<T>();
            foreach (T element in this)
                if (predicate(element))
                    result.Add(element);
            return result;
        }

        ///<summary>
        ///Calls the method <paramref name="action"/> for each element
        ///of this set.
        ///</summary>
        ///<param name="action">a method</param>
        public void ForEach(Action<T> action)
        {
            foreach (T element in this)
                action(element);
        }

        ///<summary>
        ///Clears the set
        ///</summary>
        public void Clear()
        {
            _data.Clear();
        }

        ///<summary>
        ///Tests if this set contains key <paramref name="a"/>
        ///</summary>
        ///<param name="a">a key</param>
        ///<returns>true if this set contains key <paramref name="a"/></returns>
        public bool Contains(T a)
        {
            return _data.ContainsKey(a);
        }

        ///<summary>
        ///Copy the keys of this set to the array <paramref name="array"/>
        ///starting at index <paramref name="index"/>
        ///</summary>
        ///<param name="array">the array</param>
        ///<param name="index">the index to start copying to</param>
        public void CopyTo(T[] array, int index)
        {
            _data.Keys.CopyTo(array, index);
        }

        ///<summary>
        ///Removes the element with key <paramref name="a"/> from this set.
        ///</summary>
        ///<param name="a"></param>
        ///<returns></returns>
        public bool Remove(T a)
        {
            return _data.Remove(a);
        }

        ///<summary>
        ///Gets an enumerator for the set
        ///</summary>
        ///<returns>an enumerator for the set</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _data.Keys.GetEnumerator();
        }

        ///<summary>
        ///Gets a value indicating whether the set is read-only.
        ///</summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        ///<summary>
        ///Union of two sets
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///the union of sets <paramref name="a"/> and <paramref name="b"/>
        ///</returns>
        public static Set<T> operator |(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>(a);
            result.AddRange(b);
            return result;
        }

        ///<summary>
        ///Union of this set with a collection of elements
        ///</summary>
        ///<param name="b"></param>
        ///<returns>
        ///the union of this set and the elements enumerated
        ///by <paramref name="b"/>
        ///</returns>
        public Set<T> Union(IEnumerable<T> b)
        {
            return this | new Set<T>(b);
        }

        ///<summary>
        ///Intersection of two sets
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///the intersection of set <paramref name="a"/> and <paramref name="b"/>
        ///</returns>
        public static Set<T> operator &(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach (T element in a)
                if (b.Contains(element))
                    result.Add(element);
            return result;
        }

        ///<summary>
        ///Intersection of this set and a collection of elements
        ///</summary>
        ///<param name="b"></param>
        ///<returns>
        ///the intersection of this set and the elements enumerated
        ///by <paramref name="b"/>
        ///</returns>
        public Set<T> Intersection(IEnumerable<T> b)
        {
            return this & new Set<T>(b);
        }

        ///<summary>
        ///Set difference
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///the set of elements in <paramref name="a"/> but not in <paramref name="b"/>
        ///</returns>
        public static Set<T> operator -(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach (T element in a)
                if (!b.Contains(element))
                    result.Add(element);
            return result;
        }

        ///<summary>
        ///Set difference
        ///</summary>
        ///<param name="b"></param>
        ///<returns>
        ///the set of elements in this set but not in elements enumerated
        ///by <paramref name="b"/>
        ///</returns>
        public Set<T> Difference(IEnumerable<T> b)
        {
            return this - new Set<T>(b);
        }

        ///<summary>
        ///Symmetric difference
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///the elements in set <paramref name="a"/> but not in
        ///set <paramref name="b"/> plus those in <paramref name="b"/>
        ///but not in <paramref name="a"/>
        ///</returns>
        public static Set<T> operator ^(Set<T> a, Set<T> b)
        {
            Set<T> result = new Set<T>();
            foreach (T element in a)
                if (!b.Contains(element))
                    result.Add(element);
            foreach (T element in b)
                if (!a.Contains(element))
                    result.Add(element);
            return result;
        }

        ///<summary>
        ///Symmetric difference
        ///</summary>
        ///<param name="b"></param>
        ///<returns>
        ///the symmetric difference of this set with the elements enumerated
        ///by <paramref name="b"/>
        ///</returns>
        public Set<T> SymmetricDifference(IEnumerable<T> b)
        {
            return this ^ new Set<T>(b);
        }

        ///<summary>
        ///Gets an empty set
        ///</summary>
        public static Set<T> Empty
        {
            get { return new Set<T>(0); }
        }

        ///<summary>
        ///Tests if set <paramref name="a"/> is a subset of
        ///set <paramref name="b"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="a"/> is a subset of
        ///set <paramref name="b"/>
        ///</returns>
        public static bool operator <=(Set<T> a, Set<T> b)
        {
            foreach (T element in a)
                if (!b.Contains(element))
                    return false;
            return true;
        }

        ///<summary>
        ///Tests if set <paramref name="a"/> is a proper subset of
        ///set <paramref name="b"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="a"/> is a proper subset of
        ///set <paramref name="b"/>
        ///</returns>
        public static bool operator <(Set<T> a, Set<T> b)
        {
            return (a.Count < b.Count) && (a <= b);
        }

        ///<summary>
        ///Tests if set <paramref name="a"/> equals set <paramref name="b"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="a"/> equals set <paramref name="b"/>
        ///</returns>
        public static bool operator ==(Set<T> a, Set<T> b)
        {
            return (a.Count == b.Count) && (a <= b);
        }

        ///<summary>
        ///Tests if set <paramref name="b"/> is a proper subset of
        ///set <paramref name="a"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="b"/> is a proper subset of
        ///set <paramref name="a"/>
        ///</returns>
        public static bool operator >(Set<T> a, Set<T> b)
        {
            return b < a;
        }

        ///<summary>
        ///Tests if set <paramref name="b"/> is a subset of
        ///set <paramref name="a"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="b"/> is a subset of
        ///set <paramref name="a"/>
        ///</returns>
        public static bool operator >=(Set<T> a, Set<T> b)
        {
            return (b <= a);
        }

        ///<summary>
        ///Tests if set <paramref name="a"/> not equal to
        ///set <paramref name="b"/>
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<returns>
        ///true if set <paramref name="a"/> not equal to
        ///set <paramref name="b"/>
        ///</returns>
        public static bool operator !=(Set<T> a, Set<T> b)
        {
            return !(a == b);
        }

        ///<summary>
        ///Tests if this set == <paramref name="obj"/>
        ///</summary>
        ///<param name="obj"></param>
        ///<returns></returns>
        public override bool Equals(object obj)
        {
            Set<T> a = this;
            Set<T> b = obj as Set<T>;
            if (b == null)
                return false;
            return a == b;
        }

        ///<summary>
        ///Generates a hash code for this set.
        ///</summary>
        ///<returns>A hash code for this set.</returns>
        public override int GetHashCode()
        {
            int hashcode = 0;
            foreach (T element in this)
                hashcode ^= element.GetHashCode();
            return hashcode;
        }

        ///<summary>
        ///Copies the elements of set to an array <paramref name="array"/>,
        ///starting at a index <paramref name="index"/>.
        ///</summary>
        ///<param name="array">the array to copy to</param>
        ///<param name="index">the index to start copying at</param>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_data.Keys).CopyTo(array, index);
        }

        ///<summary>
        ///Gets an object that can be used to synchronize access to this set.
        ///</summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)_data.Keys).SyncRoot; }
        }

        ///<summary>
        ///Gets a value indicating whether access to this set is synchronized
        ///(thread safe).
        ///</summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_data.Keys).IsSynchronized; }
        }

        ///<summary>
        ///Returns an enumerator that iterates through this set.
        ///</summary>
        ///<returns>an enumerator that iterates through this set.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_data.Keys).GetEnumerator();
        }
    }
    
    #endregion   
}
