#region File description
//------------------------------------------------------------------------------
//Transformations.cs
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
    #region Class Transformations

    ///<summary>
    ///class for some useful transformations
    ///</summary>
    public class Transformations
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///given a list of 2D vectors, a position, orientation and scale, this
        ///function transforms the 2D vectors into the object's world space
        ///</summary>
        ///<param name="points"></param>
        ///<param name="position"></param>
        ///<param name="forward"></param>
        ///<param name="side"></param>
        ///<param name="scale"></param>
        ///<returns>list of transformed points</returns>
        public static List<Vector2> WorldTransform(
            List<Vector2> points,
            Vector2 position,
            Vector2 forward,
            Vector2 side,
            Vector2 scale)
        {
            //copy the original points into the list about to be transformed
            List<Vector2> transformedPoints = new List<Vector2>(points);

            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();

            //scale
            if ((scale.X != 1.0) || (scale.Y != 1.0))
            {
                transformationMatrix.Scale(scale.X, scale.Y);
            }

            //rotate
            transformationMatrix.Rotate(forward, side);

            //and translate
            transformationMatrix.Translate(position.X, position.Y);

            //now transform the points
            transformationMatrix.TransformVector2(transformedPoints);

            return transformedPoints;
        }

        ///<summary>
        ///given a list of 2D vectors, a position and  orientation, this
        ///function transforms the 2D vectors into the object's world space
        ///</summary>
        ///<param name="points"></param>
        ///<param name="position"></param>
        ///<param name="forward"></param>
        ///<param name="side"></param>
        ///<returns>list of transformed points</returns>
        public static List<Vector2> WorldTransform(
            List<Vector2> points,
            Vector2 position,
            Vector2 forward,
            Vector2 side)
        {
            //copy the original points into the list about to be transformed
            List<Vector2> transformedPoints = new List<Vector2>(points);

            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();

            //rotate
            transformationMatrix.Rotate(forward, side);

            //and translate
            transformationMatrix.Translate(position.X, position.Y);

            //now transform the points
            transformationMatrix.TransformVector2(transformedPoints);

            return transformedPoints;
        }

        ///<summary>
        ///Transforms a point from the agent's local space into world space
        ///</summary>
        ///<param name="point"></param>
        ///<param name="agentHeading"></param>
        ///<param name="agentSide"></param>
        ///<param name="agentPosition"></param>
        ///<returns>the transformed point</returns>
        public static Vector2 PointToWorldSpace(
            Vector2 point,
            Vector2 agentHeading,
            Vector2 agentSide,
            Vector2 agentPosition)
        {
            //make a copy of the point
            Vector2 transformedPoint = point; //TODO: why copy?

            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();

            //rotate
            transformationMatrix.Rotate(agentHeading, agentSide);

            //and translate
            transformationMatrix.Translate(agentPosition.X, agentPosition.Y);

            //now transform the point
            transformationMatrix.TransformVector2(ref transformedPoint);

            return transformedPoint;
        }

        ///<summary>
        ///Transforms a point from world space to the agent's local space
        ///</summary>
        ///<param name="point"></param>
        ///<param name="agentHeading"></param>
        ///<param name="agentSide"></param>
        ///<param name="agentPosition"></param>
        ///<returns>the transformed point</returns>
        public static Vector2 PointToLocalSpace(
            Vector2 point,
            Vector2 agentHeading,
            Vector2 agentSide,
            Vector2 agentPosition)
        {
            //make a copy of the point
            Vector2 transformedPoint = point; //TODO: why copy?

            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();

            float tx = Vector2.Dot(-agentPosition, agentHeading);
            float ty = Vector2.Dot(-agentPosition, agentSide);

            //create the transformation matrix
            transformationMatrix.E11 = agentHeading.X;
            transformationMatrix.E12 = agentSide.X;
            transformationMatrix.E21 = agentHeading.Y;
            transformationMatrix.E22 = agentSide.Y;
            transformationMatrix.E31 = tx;
            transformationMatrix.E32 = ty;

            //now transform the vertices
            transformationMatrix.TransformVector2(ref transformedPoint);

            return transformedPoint;
        }

        ///<summary>
        ///rotates a vector <paramref name="angle"/> radians around the origin
        ///</summary>
        ///<param name="v">vector to rotate around the origin</param>
        ///<param name="angle">rotation angle in radians</param>
        public static void RotateVectorAroundOrigin(ref Vector2 v, float angle)
        {
            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();

            //rotate
            transformationMatrix.Rotate(angle);

            //now transform the object's vertices
            transformationMatrix.TransformVector2(ref v);
        }

        ///<summary>
        ///rotates a vector <paramref name="angle"/> radians around the origin
        ///</summary>
        ///<param name="v">vector to rotate around the origin</param>
        ///<param name="angle">rotation angle in radians</param>
        ///<returns>the rotated vector</returns>
        public static Vector2 RotateVectorAroundOrigin(Vector2 v, float angle)
        {
            RotateVectorAroundOrigin(ref v, angle);
            return v;
        }

        ///<summary>
        ///rotates a vector <paramref name="angle"/> radians around the origin
        ///</summary>
        ///<param name="v">vector to rotate around the origin</param>
        ///<param name="p"></param>
        ///<param name="angle">rotation angle in radians</param>
        ///<returns>the rotated vector</returns>
        public static Vector2 RotateVectorAroundPoint(Vector2 v, Vector2 p, float angle)
        {
            RotateVectorAroundPoint(ref v, p, angle);
            return v;
        }

        ///<summary>
        ///rotates a vector <paramref name="angle"/> radians around the origin
        ///</summary>
        ///<param name="v">vector to rotate around the origin</param>
        ///<param name="p"></param>
        ///<param name="angle">rotation angle in radians</param>
        ///<returns>the rotated vector</returns>
        public static void RotateVectorAroundPoint(ref Vector2 v, Vector2 p, float angle)
        {
            //create a transformation matrix
            Matrix2 transformationMatrix = new Matrix2();
            transformationMatrix.Translate(-p.X, -p.Y);
            transformationMatrix.Rotate(angle);
            transformationMatrix.Translate(p.X, p.Y);
            //now transform the object's vertices
            transformationMatrix.TransformVector2(ref v);
        }

        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums
        #endregion

        //======================================================================
        #region Public methods
        #endregion

        //======================================================================
        #region Private, protected, internal methods
        #endregion

        //======================================================================
        #region Private, protected, internal fields
        #endregion
    }

    #endregion
}
