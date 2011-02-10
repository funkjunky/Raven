#region File description

//------------------------------------------------------------------------------
//MyGame.cs
//
//Much of this project is derived from and/or inspired by Mat Buckland's Raven
//program written in C++ with a GDI graphics interface. I ported it to C# and
//integrated it with Torque X. Mat's original code is available for download
//from http://www.wordware.com/files/ai/. Also see his excellent book:
//Programming Game AI by Example for insight into the code and other AI topics.
//
//This project is intended for use in teaching undergraduate AI in conjunction
//with Mat Buckland's book.
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

using System;
using System.Collections.Generic;
using GarageGames.Torque.Core;
using GarageGames.Torque.GameUtil;
using GarageGames.Torque.GUI;
using GarageGames.Torque.T2D;
using GarageGames.Torque.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Mindcrafters.Library.Utility;
using Mindcrafters.RavenX.GameManager;
using Mindcrafters.Tx2D.GameAI;
using Program=Mindcrafters.Library.Components.Program;

#endregion

#region Microsoft

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX
{
    ///<summary>
    ///This is the main game class for your game
    ///</summary>
    public class MyGame : TorqueGame
    {
        #region Static methods, fields, constructors

        ///<summary>
        ///A static property that lets you easily get the Game instance from
        ///any Game class.
        ///</summary>
        public static MyGame Instance
        {
            get { return _myGame; }
        }

        #endregion

        #region Constructors

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================

        ///<summary>
        ///Represents the game's sound bank (a collection of cues)
        ///</summary>
        public SoundBank SoundBank
        {
            get { return _soundBank; }
            set { _soundBank = value; }
        }

        ///<summary>
        ///Represents the game's wave bank (a collection of wave files)
        ///</summary>
        public WaveBank WaveBank
        {
            get { return _waveBank; }
        }

        ///<summary>
        ///Whether the game is paused
        ///TODO: move to GameManager
        ///</summary>
        public bool GamePaused
        {
            get { return _gamePaused; }
            set { _gamePaused = value; }
        }

        #region Public Methods

        ///<summary>
        ///The main class. It all starts here folks (well, mostly).
        ///</summary>
        public static void Main()
        {
            //Create the static game instance.  
            _myGame = new MyGame();

            //begin the game.  Further setup is done in BeginRun()
            _myGame.Run();
        }

        #endregion

        #region Private, protected, internal methods

        ///<summary>
        ///Called after the graphics device is created and before the game is
        ///about to start running.
        ///</summary>
        protected override void BeginRun()
        {
            base.BeginRun();

            //load our scene objects from XML.  Torque X is designed to load
            //game data from XML, but this is not strictly required; anything 
            //in an XML file can also be created manually in C# code.  The 
            //SceneLoader is provided by TorqueGame and can be used to load
            //and unload XML files.
            //SceneLoader.Load(@"data\levels\EmptyLevel.txscene");

            T2DSceneGraph sceneGraph = new T2DSceneGraph(true);
            sceneGraph.Name = "DefaultSceneGraph";

            T2DSceneCamera camera = new T2DSceneCamera();
            camera.Name = "Camera";
            camera.CenterPosition = new Vector2(384, 384);
            camera.Extent = new Vector2(768, 768);
            camera.SceneGraph = sceneGraph;

            GUISceneview sceneview = new GUISceneview();
            sceneview.Name = "DefaultSceneView";
            sceneview.Camera = camera;

            if (null != Engine.SFXDevice)
            {
                _waveBank =
                    new WaveBank(Engine.SFXDevice, @"data\sounds\Sounds.xwb");
                _soundBank =
                    new SoundBank(Engine.SFXDevice, @"data\sounds\Sounds.xsb");
            }

            //Press escape on the keyboard or back on gamepad to exit game
            InputUtil.BindBackEscQuickExit();

            GamePaused = true;

            InitializeLibraries();

            GUILayout.Instance.Setup();

            //new GameManager(@"data\maps\RavenMap");
            //new GameManager(@"data\maps\RavenMapWithDoors");
            //new GameManager(@"data\maps\RavenMapWithDoorsAndItems");
            //new GameManager(@"data\maps\RavenMapWithDoorsAndItems2");
            new GameManager.GameManager(@"data\maps\mindcrafters");
        }

        //TODO: find a home for this in the engine ... (probably in Util)
        ///<summary>
        ///Initialize the default values of the properties of a 
        ///<see cref="TorqueComponent"/> as specified in their
        ///<see cref="TorqueXmlSchemaType"/> attributes
        ///</summary>
        ///<param name="tc">
        ///The <see cref="TorqueComponent"/> to initialize
        ///</param>
        ///<returns>The initialized <see cref="TorqueComponent"/></returns>
        public TorqueComponent Initialize(TorqueComponent tc)
        {
            Type t = tc.GetType();
            //skip anthing that isn't an instantiable reference type
            if (!t.IsClass || t.IsInterface || t.IsAbstract)
                return tc;

            //skip it unless it has a TorqueXmlSchemaType attribute
            object[] attrs = t.GetCustomAttributes(false);

            bool hasSchemaAttr = false;

            foreach (object attr in attrs)
            {
                if (!(attr is TorqueXmlSchemaType))
                    continue;

                hasSchemaAttr = true;
                break;
            }

            if (!hasSchemaAttr)
                return tc;

            TypeInfo ti = TypeUtil.FindTypeInfo(t.FullName);

            if (ti.Type.IsSubclassOf(typeof (TorqueComponent)))
            {
                List<IFieldOrProperty> fieldsAndProperties = ti.FieldsAndProperties;
                foreach (IFieldOrProperty fieldOrProperty in fieldsAndProperties)
                {
                    //retrieve XML schema attribute, if any
                    TorqueXmlSchemaType xmlAttr = null;

                    object[] cAttrs = fieldOrProperty.GetCustomAttributes(true);
                    foreach (object cAttr in cAttrs)
                    {
                        if (!(cAttr is TorqueXmlSchemaType))
                            continue;

                        xmlAttr = cAttr as TorqueXmlSchemaType;
                        break;
                    }

                    if (xmlAttr == null)
                        continue;

                    if (xmlAttr.DefaultValue != null)
                    {
                        fieldOrProperty.SetValue(
                            tc,
                            TypeUtil.GetPrimitiveValue(
                                fieldOrProperty.DeclaredType,
                                xmlAttr.DefaultValue));
                    }
                }
            }

            return tc;
        }

        ///<summary>
        ///Draw the scene, the GUI, and then hook in the
        ///<see cref="GameManager"/> render method
        ///</summary>
        ///<param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GameManager.GameManager.Instance.Render(); //draw on top
        }

        ///<summary>
        ///Game update
        ///</summary>
        ///<param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            //update game first
            //TODO: ElapsedGameTime versus ElapsedRealTime ???
            GameManager.GameManager.Instance.Update(
                (float) gameTime.ElapsedRealTime.TotalSeconds);

            base.Update(gameTime);
        }

        /// <summary>
        ///Initialize libraries and provide necessary hooks or callbacks
        /// </summary>
        private void InitializeLibraries()
        {
            // Initialize the components library
            //Note: we use anonymous delegates for property getter/setters
            Program.Initialize(
                Instance.Engine,
                Instance.SoundBank,
                Instance.Exit,
                delegate { return GamePaused; },
                delegate(bool value) { GamePaused = value; });

            Library.Utility.Program.Initialize(Instance.Engine);
            //Library.Utility.LogUtil.Instance.DefaultLogFacility =
            //    Library.Utility.LogUtil.LogFacility.TorqueConsoleEcho;
            LogUtil.Instance.IsLogging = true;
            TextUtil.Instance.DefaultFontType = @"data\fonts\Arial8";
        }

        #endregion

        #region Private, protected, internal fields

        private static MyGame _myGame;

        //TODO: create a SoundManager class and move there

        private bool _gamePaused; //TODO: move to the GameManager?
        private SoundBank _soundBank;
        private WaveBank _waveBank;

        #endregion

        //======================================================================

        //======================================================================

        //======================================================================
    }
}