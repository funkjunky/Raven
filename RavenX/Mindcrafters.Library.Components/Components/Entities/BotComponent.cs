#region File description
//------------------------------------------------------------------------------
// BotComponent.cs
//
// Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------
#endregion

#region Using directives

#region System

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#region Microsoft

using Microsoft.Xna.Framework;

#endregion

#region GarageGames

using GarageGames.Torque.Core;
using GarageGames.Torque.MathUtil;
using GarageGames.Torque.SceneGraph;
using GarageGames.Torque.Sim;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.Tx2D.GameAI
{
    #region Class BotComponent

    [TorqueXmlSchemaType]
    public class BotComponent : TorqueComponent, ITickObject
    {
        //======================================================================
        #region Static methods, fields, constructors
        #endregion

        //======================================================================
        #region Constructors
        #endregion

        //======================================================================
        #region Public properties, operators, constants, and enums

        public T2DSceneObject SceneObject
        {
            get { return Owner as T2DSceneObject; }
        }

        //public enum MoveState
        //{
        //    ModeStop,
        //    ModeMove,
        //    ModeStuck
        //}

        public bool IsPossessed
        {
            get { return _isPossessed; }
            set { _isPossessed = value; }
        }

        #endregion

        //======================================================================
        #region Public methods

        public virtual void ProcessTick(Move move, float dt)
        {

        }

        public virtual void InterpolateTick(float k)
        {
            // todo: interpolate between ticks as needed here
        }

        public override void CopyTo(TorqueComponent obj)
        {
            base.CopyTo(obj);
            BotComponent obj2 = obj as BotComponent;
            obj2.IsPossessed = IsPossessed;
        }

        //  returns a value indicating the time in seconds it will take the bot
        //  to reach the given position at its current speed.
        public float CalculateTimeToReachPos(Vector2 pos)
        {
            float speed = 100.0f; // TODO: hook in real speed instead of 100
            float multiplier = 3.0f; // allow three times the needed time
            return multiplier * (SceneObject.Position - pos).Length() / speed;
        }

        //  returns true if the bot is close to the given position
        public bool IsAtPos(Vector2 pos, float satisfactionRadius)
        {
            return (SceneObject.Position - pos).LengthSquared()
                < satisfactionRadius * satisfactionRadius;
        }

        //public bool IsBlocked()
        //{
        //    //Vector2 heading = T2DVectorUtil.VectorFromAngle(SceneObject.Rotation);
        //    //Vector2 normal = T2DVectorUtil.VectorFromAngle(SceneObject.Rotation+90.0f);

        //    //Box2 ourBox = new Box2(SceneObject.Position,
        //    //   heading, normal, SceneObject.Size.Y / 2.0f, SceneObject.Size.X / 2.0f);

        //    //Vector2[] ourVertices = new Vector2[4];
        //    //ourBox.ComputeCounterClockwiseVertices(ref ourVertices);
        //    //ConvexPolygon ourPoly = new ConvexPolygon(4, ourVertices);

        //    //Box2 theirBox = new Box2(SceneObject.Position,
        //    //   heading, normal, SceneObject.Size.Y / 2.0f, SceneObject.Size.X / 2.0f);

        //    //Vector2[] theirVertices = new Vector2[4];
        //    //theirBox.ComputeCounterClockwiseVertices(ref theirVertices);
        //    //ConvexPolygon theirPoly = new ConvexPolygon(4, theirVertices);

        //    //bool boxTest = ourBox.IntersectsWith(theirBox);
        //    //bool polyTest = ourPoly.IntersectsWith(theirPoly);
        //    //System.Diagnostics.Debug.Assert(boxTest == polyTest, "Yikes");

        //    //Vector2[] ourVertices = new Vector2[4];
        //    //ourVertices[0] = new Vector2(-1, -1);
        //    //ourVertices[1] = new Vector2(-1, 1);
        //    //ourVertices[2] = new Vector2(1, 1);
        //    //ourVertices[3] = new Vector2(1, -1);
        //    //ConvexPolygon ourPoly = new ConvexPolygon(4, ourVertices);

        //    //Vector2[] theirVertices = new Vector2[4];
        //    //theirVertices[0] = new Vector2(0, -2);
        //    //theirVertices[1] = new Vector2(0, 0);
        //    //theirVertices[2] = new Vector2(5, 0);
        //    //theirVertices[3] = new Vector2(5, -2);
        //    //ConvexPolygon theirPoly = new ConvexPolygon(4, theirVertices);

        //    //bool boxTest = ourBox.IntersectsWith(theirBox);
        //    //bool polyTest = ourPoly.IntersectsWith(theirPoly);
        //   // System.Diagnostics.Debug.Assert(boxTest == polyTest, "Yikes");



        //    const float dist = 74.0f;

        //    //List<ISceneContainerObject> foundObjects = new List<ISceneContainerObject>();
        //    //T2DSceneGraph.Instance.FindObjects(SceneObject.Position, SceneObject.Size.Y + dist, TorqueObjectType.AllObjects, SceneObject.LayerMask, foundObjects);

        //    //if (foundObjects.Count <= 1)
        //        return false;

        //    Vector2 heading = T2DVectorUtil.VectorFromAngle(SceneObject.Rotation);
        //    Vector2 normal = T2DVectorUtil.VectorFromAngle(SceneObject.Rotation + 90.0f);
        //    Box2 ourBox = new Box2(SceneObject.Position + heading * (SceneObject.Size.Y / 2.0f + dist / 2.0f),
        //        heading, normal, dist / 2.0f, SceneObject.Size.X / 2.0f);

        //    Vector2[] ourVertices = new Vector2[4];
        //    ourBox.ComputeCounterClockwiseVertices(ref ourVertices);
        //    ConvexPolygon ourPoly = new ConvexPolygon(4, ourVertices);


        //    //foreach (T2DSceneObject them in foundObjects)
        //    //{
        //    //    if (them == SceneObject) continue;

        //    //    //ReadOnlyArray<T2DCollisionImage> images = them.Collision.Images;
        //    //    //for (int i = 0; i < images.Count; i++)
        //    //    //{
        //    //    //    T2DCollisionImage image = images[i];
        //    //    //}
        //    //    //TODO: test collision with collision image instead of box

        //    //    Vector2 theirHeading = T2DVectorUtil.VectorFromAngle(them.Rotation);
        //    //    Vector2 theirNormal = T2DVectorUtil.VectorFromAngle(them.Rotation + 90.0f);

        //    //    Box2 theirBox = new Box2(them.Position,
        //    //        theirHeading, theirNormal, them.Size.Y / 2.0f, them.Size.X / 2.0f);

        //    //    Vector2[] theirVertices = new Vector2[4];
        //    //    theirBox.ComputeCounterClockwiseVertices(ref theirVertices);
        //    //    ConvexPolygon theirPoly = new ConvexPolygon(4, theirVertices);

        //    //    bool boxTest = ourBox.IntersectsWith(theirBox);
        //    //    bool polyTest = ourPoly.IntersectsWith(theirPoly);
        //    //    System.Diagnostics.Debug.Assert(boxTest == polyTest, "Yikes");

        //    //    if (boxTest)
        //    //    {
        //    //        return true;
        //    //    }
        //    //}

        //    return false;
        //}

        //public bool RectanglesIntersect(Vector2[] rectA, Vector2[] rectB)
        //{
        //    for (int i=0;i<4;i++)
        //        for (int j = 0; j < 4; j++)
        //        {
        //            if (LinesIntersect(rectA[i], rectA[(i + 1) % 4], rectB[j], rectB[(j + 1) % 4]))
        //                return true;
        //        }
        //    return false;
        //}

        //public static bool LinesIntersect(Vector2 StartPosA, Vector2 EndPosA, Vector2 StartPosB, Vector2 EndPosB)
        //{
        //    double Ua, Ub;

        //    // Equations to determine whether lines intersect

        //    double denom = ((EndPosB.Y - StartPosB.Y) * (EndPosA.X - StartPosA.X) - (EndPosB.X - StartPosB.X) * (EndPosA.Y - StartPosA.Y));

        //    if (denom == 0.0)
        //    {
        //        // parallel
        //    }

        //    double numerA = ((EndPosB.X - StartPosB.X) * (StartPosA.Y - StartPosB.Y) - (EndPosB.Y - StartPosB.Y) * (StartPosA.X - StartPosB.X));

        //    if (numerA == 0.0 && denom == 0.0)
        //    {
        //        // hmmm
        //    }

        //    double numerB = ((EndPosA.X - StartPosA.X) * (StartPosA.Y - StartPosB.Y) - (EndPosA.Y - StartPosA.Y) * (StartPosA.X - StartPosB.X));

        //    if (numerB == 0.0 && denom == 0.0)
        //    {
        //        // hmmm
        //    }



        //    Ua = numerA / denom;

        //    Ub = numerB / denom;

        //    if (Ua >= 0.0f && Ua <= 1.0f && Ub >= 0.0f && Ub <= 1.0f)
        //    {
        //        double x = StartPosA.X + Ua * (EndPosA.X - StartPosA.X);
        //        double y = StartPosA.Y + Ua * (EndPosA.Y - StartPosA.Y);
        //        System.Diagnostics.Debug.WriteLine("Intersect: " + x + ", " + y);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        #endregion

        //======================================================================
        #region Private, protected, internal methods

        protected override bool _OnRegister(TorqueObject owner)
        {
            if (!base._OnRegister(owner) || !(owner is T2DSceneObject))
                return false;

            // activate tick callback for this component.
            ProcessList.Instance.AddTickCallback(Owner, this);

            return true;
        }

        protected override void _OnUnregister()
        {
            // todo: perform de-initialization for the component

            base._OnUnregister();
        }

        protected override void _RegisterInterfaces(TorqueObject owner)
        {
            base._RegisterInterfaces(owner);

            // todo: register interfaces to be accessed by other components
            // E.g.,
            // Owner.RegisterCachedInterface("float", "interface name", this, _ourInterface);
        }

        #endregion

        //======================================================================
        #region Private, protected, internal fields

        bool _isPossessed = false;

        #endregion
    }

    #endregion
}
