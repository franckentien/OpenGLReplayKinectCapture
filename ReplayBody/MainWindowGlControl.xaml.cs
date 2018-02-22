using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace ReplayBody
{
    public partial class MainWindow
    {
        /// <summary>
        ///     initialize all value and event for GlControl
        /// </summary>
        private void WindowsFormsHost_Initialized(object sender, EventArgs e)
        {
            _glControl = new GLControl(new GraphicsMode(30, 8, 0, 0), 0, 0, GraphicsContextFlags.Default);
            _glControl.MakeCurrent();
            _glControl.Paint += GLControl_Paint;
            _glControl.Resize += GlControl_Resize;
            _glControl.MouseMove += GlControl_MouseMove;
            _glControl.MouseWheel += _glControl_MouseWheel;
            var windowsFormsHost = sender as WindowsFormsHost;
            if (windowsFormsHost != null) windowsFormsHost.Child = _glControl;
        }

        #region changeValue/Display

        /// <summary>
        ///     change value of zoom
        ///     0>zoom>180
        /// </summary>
        /// <param name="value">value to be changed on zoom</param>
        internal static void ChangeZoom(float value)
        {
            Zoom -= value;
            if (Zoom >= 180) Zoom = 179.95f;
            if (Zoom <= 0) Zoom = 0.05f;
        }

        #endregion

        #region Paint 

        /// <summary>
        ///     set views and call paint
        /// </summary>
        private void GLControl_Paint(object sender, PaintEventArgs e)
        {
            //if the windows as focus put the _menuWindow on top of all other windows 
            _menuWindow.Topmost = _glControl.Focused;

            //reset view 
            GL.ClearColor(Color.Black);
            GL.ClearDepth(1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //change projection matrix 
            _projMatrix =
                Matrix4.CreatePerspectiveFieldOfView(Zoom*
                                                     ((float) Math.PI/180), (float) _ratioMainWindow, 0.1f, 50f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref _projMatrix);

            //change view matrix 
            _viewMatrix = Matrix4.LookAt(new Vector3(0, 0, -1.5f), Vector3.Zero, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref _viewMatrix);

            //rotate the body 
            GL.Rotate(RotateX, Vector3d.UnitX);
            GL.Rotate(RotateY, Vector3d.UnitY);

            //draw the body 
            if (BodyInformation != null)
            {
                foreach (var body in BodyInformation)
                {
                    DrawSkeleton(body);
                }
            }

            //reset the display for the recall this function 
            _glControl.SwapBuffers();
            _glControl.Invalidate();
        }

        #endregion

        #region UserInteration 

        /// <summary>
        ///     change zoom  if the user use his MouseWheel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void _glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) ChangeZoom(+2);
            else ChangeZoom(-2);
        }

        /// <summary>
        ///     edit display if user move is mouse
        /// </summary>
        private void GlControl_MouseMove(object sender, MouseEventArgs e)
        {
            _current = Mouse.GetState();
            if ((e.Button & MouseButtons.Left) != 0)
            {
                if (_current != _previous)
                {
                    RotateX -= (_current.Y - _previous.Y)/2;
                    RotateY += (_current.X - _previous.X)/2;
                }
            }
            _previous = _current;
        }

        /// <summary>
        ///     if the user resize the windows reset value
        /// </summary>
        private void GlControl_Resize(object sender, EventArgs e)
        {
            var width = _glControl.Width;
            var height = _glControl.Height;

            GL.Viewport(0, 0, width, height);

            _ratioMainWindow = (double) width/height;
        }

        #endregion
    }
}