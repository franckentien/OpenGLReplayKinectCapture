using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Key = OpenTK.Input.Key;
using Keyboard = OpenTK.Input.Keyboard;

namespace ReplayBody
{
    public partial class MainWindow
    {
        #region contructor 

        /// <summary>
        ///     Initialize component and load menu windows
        /// </summary>
        public MainWindow()
        {
            //chose to put the project in Japanse format for the number
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ja-JP");
            //InitializeComponent
            InitializeComponent();
            //create and load the menuWindows 
            _menuWindow = new MenuWindow();
            _menuWindow.Show();
        }

        #endregion

        #region variable

        //Windows
        /// <summary>
        ///     control for the windows
        /// </summary>
        private static GLControl _glControl;

        /// <summary>
        ///     control for MenuWindow
        /// </summary>
        private readonly MenuWindow _menuWindow;

        //resources
        //view 
        /// <summary>
        ///     define matrix
        /// </summary>
        private Matrix4 _viewMatrix, _projMatrix;

        /// <summary>
        ///     Angle to rotate display of the body
        /// </summary>
        internal static int RotateX, RotateY;

        /// <summary>
        ///     define zoom for diplay
        ///     0>zoom>180
        /// </summary>
        internal static float Zoom = 100f;

        /// <summary>
        ///     get the ration of main windows
        /// </summary>
        private double _ratioMainWindow;

        //initialize center of view 
        internal static float MinX = 100;
        internal static float MaxX = -100;
        internal static float MinY = 100;
        internal static float MaxY = -100;
        internal static float MinZ = 100;
        internal static float MaxZ = -100;

        //display information 
        /// <summary>
        ///     MouseState
        /// </summary>
        private MouseState _current, _previous;

        /// <summary>
        ///     tab of all line of file loaded with all body information
        /// </summary>
        internal static string[][] BodyInformation;

        /// <summary>
        ///     id of frame for load the good line in BodyInformation tab
        /// </summary>
        internal static int FrameId;

        #endregion

        #region EventMainWindows

        /// <summary>
        ///     On Load of the windows
        /// </summary>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //Initialize Opentk 
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
        }

        /// <summary>
        ///     On close of the windows
        /// </summary>
        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            //close all windows 
            Application.Current.Shutdown();
        }

        /// <summary>
        ///     when the user press a key change value of display view
        /// </summary>
        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_glControl.Focused) return;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Key.Delete))
            {
                RotateX = 0;
                RotateY = 0;
                Zoom = 100f;
            }
            else
            {
                if (keyboardState.IsKeyDown(Key.Up) || keyboardState.IsKeyDown(Key.W) ||
                    keyboardState.IsKeyDown(Key.Z)) RotateX += 3;
                if (keyboardState.IsKeyDown(Key.Left) || keyboardState.IsKeyDown(Key.A) ||
                    keyboardState.IsKeyDown(Key.Q)) RotateY -= 3;
                if (keyboardState.IsKeyDown(Key.Down) || keyboardState.IsKeyDown(Key.S)) RotateX -= 3;
                if (keyboardState.IsKeyDown(Key.Right) || keyboardState.IsKeyDown(Key.D)) RotateY += 3;
                if (keyboardState.IsKeyDown(Key.PageUp)) ChangeZoom(1);
                if (keyboardState.IsKeyDown(Key.PageDown)) ChangeZoom(-1);
            }
        }

        #endregion
    }
}