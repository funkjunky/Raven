#region File description
//------------------------------------------------------------------------------
//Geometry.cs
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
    #region Class Geometry

    ///<summary>
    ///class for useful 2D geometry functions
    ///</summary>
    public class Geometry
    {
        //======================================================================
        #region Static methods, fields, constructors

        ///<summary>
        ///given a plane and a ray this function determines how far along the
        ///ray an intersection occurs. Returns 
        ///</summary>
        ///<param name="rayOrigin"></param>
        ///<param name="rayHeading"></param>
        ///<param name="planePoint"></param>
        ///<param name="planeNormal"></param>
        ///<returns>negative if the ray is parallel</returns>
        public static float DistanceToRayPlaneIntersection(
            Vector2 rayOrigin,
            Vector2 rayHeading,
            Vector2 planePoint,  //any point on the plane
            Vector2 planeNormal)
        {

            float d = -Vector2.Dot(planeNormal, planePoint);
            float numer = Vector2.Dot(planeNormal, rayOrigin) + d;
            float denom = Vector2.Dot(planeNormal, rayHeading);

            //normal is parallel to vector
            if ((denom < 0.000001f) && (denom > -0.000001f))
            {
                return (-1.0f);
            }

            return -(numer / denom);
        }

        ///<summary>
        ///point relation to plane
        ///</summary>
        public enum SpanType { Backside, Front, On };

        ///<summary>
        ///where is point in relation to plane
        ///</summary>
        ///<param name="point"></param>
        ///<param name="pointOnPlane"></param>
        ///<param name="planeNormal"></param>
        ///<returns>Backside, Front, or On</returns>
        public static SpanType WhereIsPoint(
            Vector2 point,
            Vector2 pointOnPlane, //any point on the plane
            Vector2 planeNormal)
        {
            Vector2 dir = pointOnPlane - point;

            float d = Vector2.Dot(dir, planeNormal);

            if (d < -0.000001f)
            {
                return SpanType.Front;
            }

            return d > 0.000001f ? SpanType.Backside : SpanType.On;
        }

        ///<summary>
        ///get the distance to the (first) intersecting point between a ray
        ///and a circle
        ///</summary>
        ///<param name="rayOrigin"></param>
        ///<param name="rayHeading"></param>
        ///<param name="circleOrigin"></param>
        ///<param name="radius"></param>
        ///<returns>the distance to the (first) intersecting point</returns>
        public static float GetRayCircleIntersect(
            Vector2 rayOrigin,
            Vector2 rayHeading,
            Vector2 circleOrigin,
            float radius)
        {
            Vector2 toCircle = circleOrigin - rayOrigin;
            float length = toCircle.Length();
            float v = Vector2.Dot(toCircle, rayHeading);
            float d = radius * radius - (length * length - v * v);

            //If there was no intersection, return -1
            if (d < 0.0f) return (-1.0f);

            //Return the distance to the (first) intersecting point
            return (v - (float)System.Math.Sqrt(d));
        }

        ///<summary>
        ///tests ray intersection with circle
        ///</summary>
        ///<param name="rayOrigin"></param>
        ///<param name="rayHeading"></param>
        ///<param name="circleOrigin"></param>
        ///<param name="radius"></param>
        ///<returns>false if there was no intersection</returns>
        public static bool DoRayCircleIntersect(
            Vector2 rayOrigin,
            Vector2 rayHeading,
            Vector2 circleOrigin,
            float radius)
        {
            Vector2 toCircle = circleOrigin - rayOrigin;
            float length = toCircle.Length();
            float v = Vector2.Dot(toCircle, rayHeading);
            float d = radius * radius - (length * length - v * v);

            //If there was no intersection, return false
            return (d < 0.0f);
        }

        ///<summary>
        ///Given a point P and a circle of radius R centered at C this
        ///function determines the two points on the circle that intersect
        ///with the tangents from P to the circle. Returns false if P is
        ///within the circle.
        ///<remarks>
        ///thanks to Dave Eberly for this one.
        ///</remarks>
        ///</summary>
        ///<param name="c">center C</param>
        ///<param name="r">radius R</param>
        ///<param name="p">point P</param>
        ///<param name="t1">tangent point T1</param>
        ///<param name="t2">tangent point T2</param>
        ///<returns></returns>
        public static bool GetTangentPoints(
            Vector2 c,
            float r,
            Vector2 p,
            ref Vector2 t1,
            ref Vector2 t2)
        {
            Vector2 pMc = p - c;
            float sqrLen = pMc.LengthSquared();
            float rSqr = r * r;
            if (sqrLen <= rSqr)
            {
                //P is inside or on the circle
                return false;
            }

            float invSqrLen = 1 / sqrLen;
            float root = (float)System.Math.Sqrt(System.Math.Abs(sqrLen - rSqr));

            t1.X = c.X + r * (r * pMc.X - pMc.Y * root) * invSqrLen;
            t1.Y = c.Y + r * (r * pMc.Y + pMc.X * root) * invSqrLen;
            t2.X = c.X + r * (r * pMc.X + pMc.Y * root) * invSqrLen;
            t2.Y = c.Y + r * (r * pMc.Y - pMc.X * root) * invSqrLen;

            return true;
        }

        ///<summary>
        ///given a line segment AB and a point P, this function calculates the 
        ///perpendicular distance between them
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="p">point P</param>
        ///<returns>
        ///the perpendicular distance between line segment AB and point P
        ///</returns>
        public static float DistToLineSegment(Vector2 a, Vector2 b, Vector2 p)
        {
            //if the angle is obtuse between PA and AB is obtuse then the
            //closest vertex must be A
            float dotA = (p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y);

            if (dotA <= 0) return Vector2.Distance(a, p);

            //if the angle is obtuse between PB and AB is obtuse then the
            //closest vertex must be B
            float dotB = (p.X - b.X) * (a.X - b.X) + (p.Y - b.Y) * (a.Y - b.Y);

            if (dotB <= 0) return Vector2.Distance(b, p);

            //calculate the point along AB that is the closest to P
            Vector2 point = a + ((b - a) * dotA) / (dotA + dotB);

            //calculate the distance P-Point
            return Vector2.Distance(p, point);
        }

        ///<summary>
        ///given a line segment AB and a point P, this function calculates the 
        ///square of the perpendicular distance between them
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="p">point P</param>
        ///<returns></returns>
        public static float DistToLineSegmentSq(Vector2 a, Vector2 b, Vector2 p)
        {
            //if the angle is obtuse between PA and AB is obtuse then the
            //closest vertex must be A
            float dotA = (p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y);

            if (dotA <= 0) return Vector2.DistanceSquared(a, p);

            //if the angle is obtuse between PB and AB is obtuse then the
            //closest vertex must be B
            float dotB = (p.X - b.X) * (a.X - b.X) + (p.Y - b.Y) * (a.Y - b.Y);

            if (dotB <= 0) return Vector2.DistanceSquared(b, p);

            //calculate the point along AB that is the closest to P
            Vector2 point = a + ((b - a) * dotA) / (dotA + dotB);

            //calculate the distance P-Point
            return Vector2.DistanceSquared(p, point);
        }

        ///<summary>
        ///Test if two lines in 2D space AB, CD intersect
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="c">point C</param>
        ///<param name="d">point C</param>
        ///<returns>true if lines AB and CD intersect</returns>
        public static bool LineIntersection(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Vector2 d)
        {
            float rTop = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            float sTop = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);

            float bottom =
                (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if (bottom == 0) //parallel
            {
                return false;
            }

            float invBottom = 1.0f / bottom;
            float r = rTop * invBottom;
            float s = sTop * invBottom;

            return (r > 0) && (r < 1) && (s > 0) && (s < 1);
        }

        ///<summary>
        ///Tests if two lines in 2D space AB, CD intersect and calculates
        ///the distance along AB where the intersection occurs
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="c">point C</param>
        ///<param name="d">point D</param>
        ///<param name="dist">
        ///the distance along AB where the intersection occurs
        ///(-1 if no intersection)
        ///</param>
        ///<returns>true if lines AB and CD intersect</returns>
        public static bool LineIntersection(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Vector2 d,
            out float dist)
        {
            dist = -1;

            float rTop = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            float sTop = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);

            float bottom =
                (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if (bottom == 0) //parallel
            {
                return Epsilon.IsZero(rTop) && Epsilon.IsZero(sTop);
            }

            float r = rTop / bottom;
            float s = sTop / bottom;

            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2.Distance(a, b) * r;

                return true;
            }
            return false;
        }

        ///<summary>
        ///Tests if two lines in 2D space AB, CD intersect, determines the
        ///point of intersection and calculates the distance along AB where
        ///the intersection occurs
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="c">point C</param>
        ///<param name="d">point D</param>
        ///<param name="dist">
        ///the distance along AB where the intersection occurs
        ///(-1 if no intersection)</param>
        ///<param name="point">point of intersection</param>
        ///<returns>true if lines AB and CD intersect</returns>
        public static bool LineIntersection(
            Vector2 a,
            Vector2 b,
            Vector2 c,
            Vector2 d,
            out float dist,
            out Vector2 point)
        {
            dist = -1;
            point = Vector2.Zero;

            float rTop = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            float rBottom =
                (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            float sTop = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
            float sBottom =
                (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if ((rBottom == 0) || (sBottom == 0))
            {
                //lines are parallel
                return false;
            }

            float r = rTop / rBottom;
            float s = sTop / sBottom;

            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2.Distance(a, b) * r;

                point = a + r * (b - a);

                return true;
            }

            return false;
        }

        ///<summary>
        ///Tests two polygons for intersection.
        ///<remarks>
        ///</remarks>
        ///Does not check for enclosure.
        ///</summary>
        ///<param name="lineStrip1">first list of points (line strip)</param>
        ///<param name="lineStrip2">second list of points (line strip)</param>
        ///<returns>true if polygons intersect</returns>
        public static bool ObjectIntersection2D(
            List<Vector2> lineStrip1,
            List<Vector2> lineStrip2)
        {
            //test each line segment of lineStrip1 against each segment of
            //lineStrip2
            for (int r = 0; r < lineStrip1.Count - 1; ++r)
            {
                for (int t = 0; t < lineStrip2.Count - 1; ++t)
                {
                    if (LineIntersection(
                            lineStrip2[t],
                            lineStrip2[t + 1],
                            lineStrip1[r],
                            lineStrip1[r + 1]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        ///<summary>
        ///Tests a line segment against a polygon for intersection
        /// <remarks>Does not check for enclosure.</remarks>
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="lineStrip">list of points (line strip)</param>
        ///<returns></returns>
        public static bool SegmentObjectIntersection2D(
            Vector2 a,
            Vector2 b,
            List<Vector2> lineStrip)
        {
            //test AB against each segment of object
            for (int r = 0; r < lineStrip.Count - 1; ++r)
            {
                if (LineIntersection(a, b, lineStrip[r], lineStrip[r + 1]))
                {
                    return true;
                }
            }

            return false;
        }

        ///<summary>
        ///Tests if two circles overlap
        ///</summary>
        ///<param name="x1">first circle center point x-coordinate X1</param>
        ///<param name="y1">first circle center point y-coordinate Y1</param>
        ///<param name="r1">first circle radius R1</param>
        ///<param name="x2">second circle center point x-coordinate X2</param>
        ///<param name="y2">second circle center point y-coordinate Y2</param>
        ///<param name="r2">second circle radius R2</param>
        ///<returns>true if the two circles overlap</returns>
        public static bool TwoCirclesOverlapped(
            float x1, float y1, float r1,
            float x2, float y2, float r2)
        {
            float distBetweenCenters =
                (float)System.Math.Sqrt(
                    (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            return (distBetweenCenters < (r1 + r2)) ||
                   (distBetweenCenters < System.Math.Abs(r1 - r2));
        }

        //----------------------------- TwoCirclesOverlapped ---------------------
        //
        // Returns true if the two circles overlap
        //------------------------------------------------------------------------
        ///<summary>
        ///Tests if two circles overlap
        ///</summary>
        ///<param name="c1">first circle center point C1</param>
        ///<param name="r1">first circle radius R1</param>
        ///<param name="c2">second circle center point C2</param>
        ///<param name="r2">second circle radius R2</param>
        ///<returns>true if the two circles overlap</returns>
        public static bool TwoCirclesOverlapped(
            Vector2 c1,
            float r1,
            Vector2 c2,
            float r2)
        {
            float distBetweenCenters =
                (float)System.Math.Sqrt(
                    (c1.X - c2.X) * (c1.X - c2.X) +
                    (c1.Y - c2.Y) * (c1.Y - c2.Y));

            if ((distBetweenCenters < (r1 + r2)) ||
                (distBetweenCenters < System.Math.Abs(r1 - r2)))
            {
                return true;
            }

            return false;
        }

        ///<summary>
        ///Tests if once circle encloses the other
        ///</summary>
        ///<param name="x1">first circle center point x-coordinate X1</param>
        ///<param name="y1">first circle center point y-coordinate Y1</param>
        ///<param name="r1">first circle radius R1</param>
        ///<param name="x2">second circle center point x-coordinate X2</param>
        ///<param name="y2">second circle center point y-coordinate Y2</param>
        ///<param name="r2">second circle radius R2</param>
        ///<returns>true if one circle encloses the other</returns>
        public static bool TwoCirclesEnclosed(
            float x1, float y1, float r1,
            float x2, float y2, float r2)
        {
            float distBetweenCenters =
                (float)System.Math.Sqrt(
                    (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            return distBetweenCenters < System.Math.Abs(r1 - r2);
        }

        ///<summary>
        ///Tests if two circles overlap and calculates the intersection points
        ///of any overlap
        ///<remarks>
        ///see http://astronomy.swin.edu.au/~pbourke/geometry/2circle/
        ///</remarks>
        ///</summary>
        ///<param name="x1">first circle center point x-coordinate X1</param>
        ///<param name="y1">first circle center point y-coordinate Y1</param>
        ///<param name="r1">first circle radius R1</param>
        ///<param name="x2">second circle center point x-coordinate X2</param>
        ///<param name="y2">second circle center point y-coordinate Y2</param>
        ///<param name="r2">second circle radius R2</param>
        ///<param name="p3X">first intersection point x-coordinate P3X</param>
        ///<param name="p3Y">first intersection point y-coordinate P3Y</param>
        ///<param name="p4X">second intersection point x-coordinate P4X</param>
        ///<param name="p4Y">second intersection point y-coordinate P4Y</param>
        ///<returns>false if no overlap found</returns>
        public static bool TwoCirclesIntersectionPoints(
            float x1, float y1, float r1,
            float x2, float y2, float r2,
            out float p3X, out float p3Y,
            out float p4X, out float p4Y)
        {
            p3X = p3Y = p4X = p4Y = 0;

            //first check to see if they overlap
            if (!TwoCirclesOverlapped(x1, y1, r1, x2, y2, r2))
            {
                return false;
            }

            //calculate the distance between the circle centers
            float d =
                (float)System.Math.Sqrt(
                    (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            //Now calculate the distance from the center of each circle to the
            //center of the line which connects the intersection points.
            float a = (r1 - r2 + (d * d)) / (2 * d);
            float b = (r2 - r1 + (d * d)) / (2 * d);

            //MAYBE A TEST FOR EXACT OVERLAP? 

            //calculate the point P2 which is the center of the line which 
            //connects the intersection points

            float p2X = x1 + a * (x2 - x1) / d;
            float p2Y = y1 + a * (y2 - y1) / d;

            //calculate first point
            float h1 = (float)System.Math.Sqrt((r1 * r1) - (a * a));

            p3X = p2X - h1 * (y2 - y1) / d;
            p3Y = p2Y + h1 * (x2 - x1) / d;

            //calculate second point
            float h2 = (float)System.Math.Sqrt((r2 * r2) - (b * b));

            p4X = p2X + h2 * (y2 - y1) / d;
            p4Y = p2Y - h2 * (x2 - x1) / d;

            return true;
        }

        ///<summary>
        ///Tests to see if two circles overlap and if so calculates the area
        ///defined by the union.
        ///<remarks>
        ///see http://mathforum.org/library/drmath/view/54785.html
        ///</remarks>
        ///</summary>
        ///<param name="x1">first circle center point x-coordinate X1</param>
        ///<param name="y1">first circle center point y-coordinate Y1</param>
        ///<param name="r1">first circle radius R1</param>
        ///<param name="x2">second circle center point x-coordinate X2</param>
        ///<param name="y2">second circle center point y-coordinate Y2</param>
        ///<param name="r2">second circle radius R2</param>
        ///<returns>area of overlap</returns>
        public static float TwoCirclesIntersectionArea(
            float x1, float y1, float r1,
            float x2, float y2, float r2)
        {
            //first calculate the intersection points
            float iX1, iY1, iX2, iY2;

            if (!TwoCirclesIntersectionPoints(
                x1, y1, r1, x2, y2, r2, out iX1, out iY1, out iX2, out iY2))
            {
                return 0.0f; //no overlap
            }

            //calculate the distance between the circle centers
            float d =
                (float)System.Math.Sqrt(
                    (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            //find the angles given that A and B are the two circle centers
            //and C and D are the intersection points
            float cbd =
                2 * (float)System.Math.Acos(
                    (r2 * r2 + d * d - r1 * r1) / (r2 * d * 2));

            float cad =
                2 * (float)System.Math.Acos(
                (r1 * r1 + d * d - r2 * r2) / (r1 * d * 2));


            //Then we find the segment of each of the circles cut off by the 
            //chord CD, by taking the area of the sector of the circle BCD and
            //subtracting the area of triangle BCD. Similarly we find the area
            //of the sector ACD and subtract the area of triangle ACD.

            float area =
                0.5f * cbd * r2 * r2 -
                0.5f * r2 * r2 * (float)System.Math.Sin(cbd) +
                0.5f * cad * r1 * r1 -
                0.5f * r1 * r1 * (float)System.Math.Sin(cad);

            return area;
        }

        ///<summary>
        ///given the radius, calculates the area of a circle
        ///</summary>
        ///<param name="radius">radius</param>
        ///<returns>area of circle</returns>
        public static float CircleArea(float radius)
        {
            return (float)System.Math.PI * radius * radius;
        }

        ///<summary>
        ///Tests if the point p is within the radius of the given circle
        ///</summary>
        ///<param name="center">center of circle</param>
        ///<param name="radius">radius of circle</param>
        ///<param name="p">point P</param>
        ///<returns>
        ///true if the point p is within the radius of the given circle
        ///</returns>
        public static bool PointInCircle(
            Vector2 center, float radius, Vector2 p)
        {
            float distFromCenterSquared = (p - center).LengthSquared();

            return distFromCenterSquared < (radius * radius);
        }

        ///<summary>
        ///tests if the line segment AB intersects with a circle at
        ///position P with radius Radius
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="p">circle center point P</param>
        ///<param name="radius">circle radius Radius</param>
        ///<returns>
        ///true if the line segment AB intersects with a circle at
        ///position P with radius Radius
        ///</returns>
        public static bool LineSegmentCircleIntersection(
            Vector2 a,
            Vector2 b,
            Vector2 p,
            float radius)
        {
            //first determine the distance from the center of the circle to
            //the line segment (working in distance squared space)
            float distToLineSq = DistToLineSegmentSq(a, b, p);

            return distToLineSq < radius * radius;
        }

        ///<summary>
        ///given a line segment AB and a circle center position and radius,
        ///this function determines if there is an intersection and stores
        ///the position of the closest intersection in IntersectionPoint
        ///</summary>
        ///<param name="a">point A</param>
        ///<param name="b">point B</param>
        ///<param name="center">circle center point Center</param>
        ///<param name="radius">circle radius Radius</param>
        ///<param name="intersectionPoint">
        ///intersection point IntersectionPoint
        ///</param>
        ///<returns>false if no intersection point is found</returns>
        public static bool GetLineSegmentCircleClosestIntersectionPoint(
            Vector2 a,
            Vector2 b,
            Vector2 center,
            float radius,
            ref Vector2 intersectionPoint)
        {
            Vector2 toBNorm = Vector2.Normalize(b - a);

            //move the circle into the local space defined by the vector B-A
            //with origin at A
            Vector2 localCenter =
                Transformations.PointToLocalSpace(
                    center, toBNorm, Vector2Util.Perp(toBNorm), a);

            bool ipFound = false;

            //if the local center + the radius is negative then the circle
            //lies behind point A so there is no intersection possible.
            //If the local center x minus the radius is greater than length A-B
            //then the circle cannot intersect the line segment
            if ((localCenter.X + radius >= 0) &&
                ((localCenter.X - radius)*(localCenter.X - radius) <=
                 Vector2.DistanceSquared(b, a)) && 
                 System.Math.Abs(localCenter.Y) < radius)
            {
                //now to do a line/circle intersection test. The center of
                //the circle is represented by lcX, lcY. The intersection
                //points are given by x = lcX +/-sqrt(r^2-lcY^2), y=0. We
                //only need to look at the smallest positive value of x.
                float lcX = localCenter.X;
                float lcY = localCenter.Y;

                float ip =
                    lcX - (float) System.Math.Sqrt(radius*radius - lcY*lcY);

                if (ip <= 0)
                {
                    ip =
                        lcX + (float) System.Math.Sqrt(radius*radius - lcY*lcY);
                }

                ipFound = true;

                intersectionPoint = a + toBNorm*ip;
            }

            return ipFound;
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
