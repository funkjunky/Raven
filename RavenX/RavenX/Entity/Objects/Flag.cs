#region File description

//------------------------------------------------------------------------------
//Flag.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using GarageGames.Torque.Materials;
using GarageGames.Torque.T2D;
using Microsoft.Xna.Framework;
using Mindcrafters.RavenX.Trigger;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX.Entity.Objects
{
    ///<summary>
    ///class for Flag
    ///</summary>
    public class Flag : T2DStaticSprite, IEntitySceneObject
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///associated <see cref="TriggerHealthGiver"/>
        ///</summary>
        public TriggerFlag TriggerFlag
        {
            get { return _triggerFlag; }
        }

        ///<summary>
        ///associated <see cref="TriggerFlag"/> cast as an
        ///<see cref="Entity"/>
        ///</summary>
        public Entity Entity
        {
            get { return _triggerFlag; }
        }

        ///<summary>
        ///scene object
        ///</summary>
        public T2DSceneObject SceneObject
        {
            get { return this; }
        }

        #endregion

        #region Constructors

        ///<summary>
        ///constructor for given team
        ///</summary>
        ///<param name="team"></param>
        public Flag(GameManager.GameManager.Teams team)
        {
            _team = team;
        }

        #endregion

        #region Public methods

        private readonly GameManager.GameManager.Teams _team;

        #endregion

        #region Private, protected, internal methods

        #endregion

        #region Private, protected, internal field

        private TriggerFlag _triggerFlag;

        public void Initialize(Entity entity)
        {
            _triggerFlag = entity as TriggerFlag;
            SimpleMaterial material = new SimpleMaterial();
            material.IsTranslucent = true;
            material.IsColorBlended = true;

            switch (_team)
            {
                case GameManager.GameManager.Teams.Blue:
                    Name = "Flag0";
                    material.TextureFilename = @"data\images\MindCrafters\blueFlag";
                    Material = material;
                    break;
                case GameManager.GameManager.Teams.Red:
                    Name = "Flag1";
                    material.TextureFilename = @"data\images\MindCrafters\redFlag";
                    Material = material;
                    break;
                case GameManager.GameManager.Teams.Green:
                    Name = "Flag2";
                    material.TextureFilename = @"data\images\MindCrafters\greenFlag";
                    Material = material;
                    break;
                case GameManager.GameManager.Teams.Yellow:
                    Name = "Flag3";
                    material.TextureFilename = @"data\images\MindCrafters\yellowFlag";
                    Material = material;
                    break;
            }
            //set the size of the model in the scene
            Layer = 30;
            Size = new Vector2(16, 16);
        }

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================

        //======================================================================
    }
}