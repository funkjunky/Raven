#region File description
//------------------------------------------------------------------------------
//Matrix2.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Namespace imports

#region System

using System.Collections.Generic;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames
#endregion

#region Mindcrafters
#endregion

#endregion

namespace Mindcrafters.Library.Math
{
    #region Class Matrix2

    ///<summary>
    ///class for 2D matrix
    ///</summary>
    public class Matrix2
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors

        ///<summary>
        ///constructor for 2D matrix class
        ///</summary>
        public Matrix2()
        {
            //initialize the matrix to an identity matrix
            Identity();
        }

        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        ///<summary>
        ///matrix element 11
        ///</summary>
        public float E11
        {
            get { return _matrix._e11; }
            set { _matrix._e11 = value; }
        }

        ///<summary>
        ///matrix element 12
        ///</summary>
        public float E12
        {
            get { return _matrix._e12; }
            set { _matrix._e12 = value; }
        }

        ///<summary>
        ///matrix element 13
        ///</summary>
        public float E13
        {
            get { return _matrix._e13; }
            set { _matrix._e13 = value; }
        }

        ///<summary>
        ///matrix element 21
        ///</summary>
        public float E21
        {
            get { return _matrix._e21; }
            set { _matrix._e21 = value; }
        }

        ///<summary>
        ///matrix element 22
        ///</summary>
        public float E22
        {
            get { return _matrix._e22; }
            set { _matrix._e22 = value; }
        }

        ///<summary>
        ///matrix element 23
        ///</summary>
        public float E23
        {
            get { return _matrix._e23; }
            set { _matrix._e23 = value; }
        }

        ///<summary>
        ///matrix element 31
        ///</summary>
        public float E31
        {
            get { return _matrix._e31; }
            set { _matrix._e31 = value; }
        }

        ///<summary>
        ///matrix element 32
        ///</summary>
        public float E32
        {
            get { return _matrix._e32; }
            set { _matrix._e32 = value; }
        }

        ///<summary>
        ///matrix element 33
        ///</summary>
        public float E33
        {
            get { return _matrix._e33; }
            set { _matrix._e33 = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        ///<summary>
        ///applies a 2D transformation matrix to a list of Vector2s
        ///</summary>
        ///<param name="points">list of points</param>
        public void TransformVector2(List<Vector2> points)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                float tempX = (E11 * points[i].X) + (E21 * points[i].Y) + (E31);
                float tempY = (E12 * points[i].X) + (E22 * points[i].Y) + (E32);

                points[i] = new Vector2(tempX, tempY);
            }
        }

        ///<summary>
        ///applies a 2D transformation matrix to a single Vector2
        ///</summary>
        ///<param name="point">point</param>
        public void TransformVector2(ref Vector2 point)
        {

            float tempX = (E11 * point.X) + (E21 * point.Y) + (E31);
            float tempY = (E12 * point.X) + (E22 * point.Y) + (E32);

            point.X = tempX;
            point.Y = tempY;
        }

        ///<summary>
        ///applies a 2D transformation matrix to a single Vector2
        ///</summary>
        ///<param name="point"></param>
        ///<returns>transformed point</returns>
        public Vector2 TransformVector2(Vector2 point)
        {
            TransformVector2(ref point);
            return point;
        }

        ///<summary>
        ///create an identity matrix
        ///</summary>
        public void Identity()
        {
            E11 = 1; E12 = 0; E13 = 0;
            E21 = 0; E22 = 1; E23 = 0;
            E31 = 0; E32 = 0; E33 = 1;
        }

        ///<summary>
        ///create a transformation matrix
        ///</summary>
        ///<param name="x"></param>
        ///<param name="y"></param>
        public void Translate(float x, float y)
        {
            Matrix mat;

            mat._e11 = 1; mat._e12 = 0; mat._e13 = 0;
            mat._e21 = 0; mat._e22 = 1; mat._e23 = 0;
            mat._e31 = x; mat._e32 = y; mat._e33 = 1;

            //and multiply
            MatrixMultiply(mat);
        }

        ///<summary>
        ///create a scale matrix
        ///</summary>
        ///<param name="xScale"></param>
        ///<param name="yScale"></param>
        public void Scale(float xScale, float yScale)
        {
            Matrix mat;

            mat._e11 = xScale; mat._e12 = 0; mat._e13 = 0;
            mat._e21 = 0; mat._e22 = yScale; mat._e23 = 0;
            mat._e31 = 0; mat._e32 = 0; mat._e33 = 1;

            //and multiply
            MatrixMultiply(mat);
        }

        ///<summary>
        ///create a rotation matrix
        ///</summary>
        ///<param name="angle">angle in radians</param>
        public void Rotate(float angle)
        {
            Matrix mat;

            float sin = (float)System.Math.Sin(angle);
            float cos = (float)System.Math.Cos(angle);

            mat._e11 = cos; mat._e12 = sin; mat._e13 = 0;
            mat._e21 = -sin; mat._e22 = cos; mat._e23 = 0;
            mat._e31 = 0; mat._e32 = 0; mat._e33 = 1;

            //and multiply
            MatrixMultiply(mat);
        }

        ///<summary>
        ///create a rotation matrix from a 2D vector
        ///</summary>
        ///<param name="forward"></param>
        ///<param name="side"></param>
        public void Rotate(Vector2 forward, Vector2 side)
        {
            Matrix mat;

            mat._e11 = forward.X; mat._e12 = forward.Y; mat._e13 = 0;
            mat._e21 = side.X; mat._e22 = side.Y; mat._e23 = 0;
            mat._e31 = 0; mat._e32 = 0; mat._e33 = 1;

            //and multiply
            MatrixMultiply(mat);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        ///<summary>
        ///multiply this matrix by <paramref name="rightMatrix"/>.
        ///</summary>
        ///<param name="rightMatrix"></param>
        void MatrixMultiply(Matrix rightMatrix)
        {
            Matrix matTemp;

            //first row
            matTemp._e11 = 
                (E11 * rightMatrix._e11) + 
                (E12 * rightMatrix._e21) + 
                (E13 * rightMatrix._e31);
            matTemp._e12 = 
                (E11 * rightMatrix._e12) + 
                (E12 * rightMatrix._e22) + 
                (E13 * rightMatrix._e32);
            matTemp._e13 = 
                (E11 * rightMatrix._e13) + 
                (E12 * rightMatrix._e23) + 
                (E13 * rightMatrix._e33);

            //second
            matTemp._e21 = 
                (E21 * rightMatrix._e11) + 
                (E22 * rightMatrix._e21) + 
                (E23 * rightMatrix._e31);
            matTemp._e22 = 
                (E21 * rightMatrix._e12) + 
                (E22 * rightMatrix._e22) + 
                (E23 * rightMatrix._e32);
            matTemp._e23 = 
                (E21 * rightMatrix._e13) + 
                (E22 * rightMatrix._e23) + 
                (E23 * rightMatrix._e33);

            //third
            matTemp._e31 = 
                (E31 * rightMatrix._e11) + 
                (E32 * rightMatrix._e21) + 
                (E33 * rightMatrix._e31);
            matTemp._e32 = 
                (E31 * rightMatrix._e12) + 
                (E32 * rightMatrix._e22) + 
                (E33 * rightMatrix._e32);
            matTemp._e33 = 
                (E31 * rightMatrix._e13) + 
                (E32 * rightMatrix._e23) + 
                (E33 * rightMatrix._e33);

            _matrix = matTemp;
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        struct Matrix
        {
            public float _e11, _e12, _e13;
            public float _e21, _e22, _e23;
            public float _e31, _e32, _e33;
        }

        Matrix _matrix;

        #endregion
    }

    #endregion
}
