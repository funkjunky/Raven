#region File description

//------------------------------------------------------------------------------
//WallIntersectionTests.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
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

namespace Mindcrafters.RavenX.Map
{
    ///<summary>
    ///class for wall intersection tests
    ///TODO: should this be moved to Wall?
    ///</summary>
    public class WallIntersectionTests
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///given a line segment defined by the points from and to, iterate
        ///through all the map objects and walls and test for any intersection.
        ///</summary>
        ///<param name="from"></param>
        ///<param name="to"></param>
        ///<param name="walls"></param>
        ///<returns>true if an intersection occurs.</returns>
        public static bool DoWallsObstructLineSegment(
            Vector2 from,
            Vector2 to,
            List<Wall> walls)
        {
            //test against the walls
            foreach (Wall curWall in walls)
            {
                //do a line segment intersection test
                if (Geometry.LineIntersection(
                    from, to, curWall.From, curWall.To))
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///similar to above except this version checks to see if the sides
        ///described by the cylinder of length |AB| with the given radius
        ///intersect any walls. (this enables the trace to take into account
        ///any the bounding radii of entity objects)
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="boundingRadius"></param>
        ///<param name="walls"></param>
        ///<returns></returns>
        public bool DoWallsObstructCylinderSides(
            Vector2 a,
            Vector2 b,
            float boundingRadius,
            List<Wall> walls)
        {
            //the line segments that make up the sides of the cylinder must
            //be created
            Vector2 toB = Vector2.Normalize(b - a);
            Vector2 radialEdge = Vector2Util.Perp(toB)*boundingRadius;

            //A1B1 will be one side of the cylinder, A2B2 the other.

            //create the two sides of the cylinder
            Vector2 a1 = a + radialEdge;
            Vector2 b1 = b + radialEdge;

            Vector2 a2 = a - radialEdge;
            Vector2 b2 = b - radialEdge;

            //now test against them
            return DoWallsObstructLineSegment(a1, b1, walls) ||
                   DoWallsObstructLineSegment(a2, b2, walls);
        }

        ///<summary>
        ///tests a line segment against the container of walls  to calculate the
        ///closest intersection point, which is stored in the reference 'ip'. 
        ///The distance to the point is assigned to the reference 'distance'
        ///</summary>
        ///<param name="a"></param>
        ///<param name="b"></param>
        ///<param name="walls"></param>
        ///<param name="distance"></param>
        ///<param name="ip"></param>
        ///<returns>
        ///false if no intersection point found
        ///</returns>
        public static bool FindClosestPointOfIntersectionWithWalls(
            Vector2 a,
            Vector2 b,
            List<Wall> walls,
            out float distance,
            ref Vector2 ip)
        {
            distance = Single.MaxValue;

            foreach (Wall curWall in walls)
            {
                float dist;
                Vector2 point;

                if (!Geometry.LineIntersection(
                         a,
                         b,
                         curWall.From,
                         curWall.To,
                         out dist,
                         out point))
                    continue;

                if (dist >= distance)
                    continue;

                distance = dist;
                ip = point;
            }

            return distance < Single.MaxValue;
        }

        ///<summary>
        ///Tests if any walls intersect the circle of radius at point p
        ///</summary>
        ///<param name="walls"></param>
        ///<param name="p"></param>
        ///<param name="r"></param>
        ///<returns>
        ///true if any walls intersect the circle of radius at point p
        ///</returns>
        public static bool DoWallsIntersectCircle(
            List<Wall> walls,
            Vector2 p,
            float r)
        {
            //test against the walls
            foreach (Wall curWall in walls)
            {
                //do a line segment intersection test
                if (Geometry.LineSegmentCircleIntersection(
                    curWall.From, curWall.To, p, r))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal fields

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}