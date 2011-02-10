#region File description

//------------------------------------------------------------------------------
//GUILayout.cs
//
//Copyright (C) Scott D. Goodwin <Mindcrafters.ca> All rights reserved.
//------------------------------------------------------------------------------

#endregion

#region Namespace imports

#region System

#endregion

#region Microsoft

using GarageGames.Torque.Core;
using GarageGames.Torque.GUI;
using Microsoft.Xna.Framework.Graphics;

#endregion

#region GarageGames

#endregion

#region Mindcrafters

#endregion

#endregion

namespace Mindcrafters.RavenX
{

    #region Class GUILayout

    ///<summary>
    ///class to setup the GUI
    ///</summary>
    public class GUILayout : GUIBasicLayout
    {
        #region Static methods, fields, constructors

        #region Singleton pattern

        private static GUILayout _instance;

        ///<summary>
        ///Private constructor
        ///</summary>
        private GUILayout()
        {
            Assert.Fatal(null == _instance, "Singleton already created.");
            _instance = this;
        }

        ///<summary>
        ///Accessor for the GUILayout singleton instance.
        ///</summary>
        public new static GUILayout Instance
        {
            get
            {
                if (null == _instance)
                    new GUILayout();

                Assert.Fatal(null != _instance,
                             "Singleton instance not set by constructor.");
                return _instance;
            }
        }

        private static void _SplashFinished()
        {
            MyGame.Instance.GamePaused = false;
        }

        #endregion

        #endregion

        #region Constructors

        #endregion

        #region Public methods

        ///<summary>
        ///Initialization. Set <see cref="GUIBasicLayout"/> options here.
        ///This should be called only once (typically, from
        ///<see cref="MyGame"/>.BeginRun)
        ///</summary>
        public new void Setup()
        {
            #region GUIBasicLayout setup

            ////set up GUIBasicLayout options
            //Style.UseSplashScreen = true;
            //SplashBitmap =
            //   @"data\images\Mindcrafters\MindcraftersLogo.jpg";

            Style.ClockVisible = false;
            //Style.FrameRateVisible = false;

            //don't need mouse pointer
            Style.AutoActivateRootMousePointer = false;
            Style.AutoActivateMenuMousePointer = false;

            base.Setup();

            //#if !XBOX
            //           FrameRate.Position = SafePositionNone;
            //#else
            //           FrameRate.Position = SafePositionAction;
            //#endif
            FrameRate.TextComponent.Style.TextColor[ControlColor.ColorBase] =
                Color.Blue;

            #endregion

            if (Style.UseSplashScreen)
            {
                SplashScreen.SplashComponent.OnFadeFinishedDelegate +=
                    _SplashFinished;
            }
            else
            {
                _SplashFinished();
            }
        }

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

    #endregion
}