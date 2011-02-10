#region File description
//------------------------------------------------------------------------------
//DataStreamReader.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using System.IO;

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
    #region Class DataStreamReader

    ///<summary>
    ///class for reading data from a text stream
    ///</summary>
    public class DataStreamReader : StreamReader
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor for DataStreamReader class
        ///</summary>
        ///<param name="inStream">an existing input stream</param>
        public DataStreamReader(Stream inStream)
            : base(inStream)
        {
        }

        ///<summary>
        ///constructor for DataStreamReader class
        ///</summary>
        ///<param name="path">relative file path</param>
        public DataStreamReader(string path)
            : base(path)
        {
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///Tests if we have reached the end of the stream.
        ///If <see cref="_fieldQueue"/> is empty, read and
        ///split the next line (if any).
        ///</summary>
        ///<returns>true if finished with the stream</returns>
        public bool IsDone()
        {
            while (_fieldQueue.Count == 0 && Peek() != -1)
            {
                string[] fields =
                    ReadLine().Split(
                        new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < fields.Length; i++)
                {
                    _fieldQueue.Enqueue(fields[i]);
                }
            }
            return _fieldQueue.Count == 0;
        }

        ///<summary>
        ///Skip (dequeue) the next field (if any)
        ///</summary>
        public void SkipNextFieldInStream()
        {
            if (!IsDone())
            {
                _fieldQueue.Dequeue();
            }
        }

        ///<summary>
        ///Get (peek but don't dequeue) the next field (if any)
        ///</summary>
        ///<returns>the next field if any otherwise return null</returns>
        public string PeekNextFieldInStream()
        {
            return IsDone() ? null : _fieldQueue.Peek();
        }

        ///<summary>
        ///Get an unsigned int from the stream
        ///</summary>
        ///<returns>the unsigned int</returns>
        public uint GetUIntFromStream()
        {
            uint result;
            GetUIntFromStream(out result);
            return result;
        }

        ///<summary>
        ///Get an unsigned int from the stream
        ///</summary>
        ///<param name="result">the unsigned int</param>
        ///<exception cref="ApplicationException">if attempt fails</exception>
        public void GetUIntFromStream(out uint result)
        {
            if (!TryGetUIntFromStream(out result))
                throw new ApplicationException(
                    "DataStreamReader.GetUIntFromStream: failed");
        }

        ///<summary>
        ///Get an unsigned int from the stream
        ///<remarks>
        ///XBox doesn't support TryParse so we have to use a try/catch
        ///</remarks>
        ///</summary>
        ///<param name="result">the unsigned int</param>
        ///<returns>true if successful</returns>
        public bool TryGetUIntFromStream(out uint result)
        {
            result = 0;

            string next = PeekNextFieldInStream();

            if (String.IsNullOrEmpty(next))
                return false;
#if XBOX
            try
            {
                result = uint.Parse(next);
                SkipNextFieldInStream();
                return true;
            }
            catch (ArgumentException ae)
            {
                return false;
            }
            catch (FormatException fe)
            {
                return false;
            }
            catch (OverflowException oe)
            {
                return false;
            }
#else
            if (uint.TryParse(next, out result))
            {
                SkipNextFieldInStream();
                return true;
            }
            return false;
#endif
        }

        ///<summary>
        ///Get an int from the stream
        ///</summary>
        ///<returns>the int</returns>
        public int GetIntFromStream()
        {
            int result;
            GetIntFromStream(out result);
            return result;
        }

        ///<summary>
        ///Get an int from the stream
        ///</summary>
        ///<param name="result">the int</param>
        ///<exception cref="ApplicationException">if attempt fails</exception>
        public void GetIntFromStream(out int result)
        {
            if (!TryGetIntFromStream(out result))
                throw new ApplicationException(
                    "DataStreamReader.GetIntFromStream: failed");
        }

        ///<summary>
        ///Get an int from the stream
        ///<remarks>
        ///XBox doesn't support TryParse so we have to use a try/catch
        ///</remarks>
        ///</summary>
        ///<param name="result">the int</param>
        ///<returns>true if successful</returns>
        public bool TryGetIntFromStream(out int result)
        {
            result = 0;

            string next = PeekNextFieldInStream();

            if (String.IsNullOrEmpty(next))
                return false;
#if XBOX
            try
            {
                result = int.Parse(next);
                SkipNextFieldInStream();
                return true;
            }
            catch (ArgumentException ae)
            {
                return false;
            }
            catch (FormatException fe)
            {
                return false;
            }
            catch (OverflowException oe)
            {
                return false;
            }
#else
            if (int.TryParse(next, out result))
            {
                SkipNextFieldInStream();
                return true;
            }
            return false;
#endif
        }

        ///<summary>
        ///Get a float from the stream
        ///</summary>
        ///<returns>the float</returns>
        public float GetFloatFromStream()
        {
            float result;
            GetFloatFromStream(out result);
            return result;
        }

        ///<summary>
        ///Get a float from the stream
        ///</summary>
        ///<param name="result">the float</param>
        ///<exception cref="ApplicationException">if attempt fails</exception>
        public void GetFloatFromStream(out float result)
        {
            if (!TryGetFloatFromStream(out result))
                throw new ApplicationException(
                    "DataStreamReader.GetFloatFromStream: failed");
        }

        ///<summary>
        ///Get a float from the stream
        ///</summary>
        ///<param name="result">the float</param>
        ///<returns>true if successful</returns>
        public bool TryGetFloatFromStream(out float result)
        {
            result = 0;

            string next = PeekNextFieldInStream();

            if (String.IsNullOrEmpty(next))
                return false;
#if XBOX
            try
            {
                result = Single.Parse(next);
                SkipNextFieldInStream();
                return true;
            }
            catch (ArgumentException ae)
            {
                return false;
            }
            catch (FormatException fe)
            {
                return false;
            }
            catch (OverflowException oe)
            {
                return false;
            }
#else
            if (Single.TryParse(next, out result))
            {
                SkipNextFieldInStream();
                return true;
            }
            return false;
#endif
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields

        private readonly Queue<string> _fieldQueue = new Queue<string>();

        #endregion
    }

    #endregion
}
