using System;
using System.ComponentModel;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Win32;

namespace ReplayBody
{
    public partial class MenuWindow
    {
        /// <summary>
        ///     constructor
        /// </summary>
        public MenuWindow()
        {
            //loop timer
            _loopTimerClick = new Timer
            {
                Interval = 1000/30, //30 frame per seconds 
                Enabled = false
            };
            _loopTimerClick.Elapsed += loopTimerClick_Elapsed;
            _loopTimerClick.AutoReset = true;
            //play timer
            _loopTimerPlay = new Timer
            {
                Interval = 1000/30, //30 frame per seconds 
                Enabled = false
            };
            _loopTimerPlay.Elapsed += loopTimerPlay_Elapsed;
            _loopTimerPlay.AutoReset = true;

            //initializeComponent and event 
            InitializeComponent();
            Closing += MyWindow_Closing;
        }

        /// <summary>
        ///     ///check if load file was ok
        /// </summary>
        /// <returns>true if file was else false </returns>
        private bool Checkfile(string[] body)
        {
            var idframe = 0;

            foreach (var frameLine in body)
            {
                idframe++;
                var tempCase = 0;
                float testParseFloat;
                int testParseInt;
                var bodyFrameInformation = frameLine.Split(',');

                if (idframe != body.Length)
                {
                    if (bodyFrameInformation.Length < 126)
                    {
                        return false;
                    }
                }

                #region check all value in the line 

                foreach (var jointValue in bodyFrameInformation)
                {
                    switch (tempCase++)
                    {
                        case 0:
                            if (idframe != body.Length)
                            {
                                if (jointValue == "\n" || jointValue == "\r" || jointValue == "")
                                {
                                    return false;
                                }
                                if (!int.TryParse(jointValue, out testParseInt))
                                {
                                    return false;
                                }
                            }
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 8:
                        case 9:
                        case 10:
                        case 13:
                        case 14:
                        case 15:
                        case 18:
                        case 19:
                        case 20:
                        case 23:
                        case 24:
                        case 25:
                        case 28:
                        case 29:
                        case 30:
                        case 33:
                        case 34:
                        case 35:
                        case 38:
                        case 39:
                        case 40:
                        case 43:
                        case 44:
                        case 45:
                        case 48:
                        case 49:
                        case 50:
                        case 53:
                        case 54:
                        case 55:
                        case 58:
                        case 59:
                        case 60:
                        case 63:
                        case 64:
                        case 65:
                        case 68:
                        case 69:
                        case 70:
                        case 73:
                        case 74:
                        case 75:
                        case 78:
                        case 79:
                        case 80:
                        case 83:
                        case 84:
                        case 85:
                        case 88:
                        case 89:
                        case 90:
                        case 93:
                        case 94:
                        case 95:
                        case 98:
                        case 99:
                        case 100:
                        case 103:
                        case 104:
                        case 105:
                        case 108:
                        case 109:
                        case 110:
                        case 113:
                        case 114:
                        case 115:
                        case 118:
                        case 119:
                        case 120:
                        case 123:
                        case 124:
                        case 125:
                            if (!float.TryParse(jointValue, out testParseFloat))
                            {
                                return false;
                            }
                            break;
                        case 2:
                        case 7:
                        case 12:
                        case 17:
                        case 22:
                        case 27:
                        case 32:
                        case 37:
                        case 42:
                        case 47:
                        case 52:
                        case 57:
                        case 62:
                        case 67:
                        case 72:
                        case 77:
                        case 82:
                        case 87:
                        case 92:
                        case 97:
                        case 102:
                        case 107:
                        case 112:
                        case 117:
                        case 122:
                            switch (jointValue)
                            {
                                case "Goal":
                                case "Inferred":
                                case "NotTracked":
                                case "Tracked":
                                    break;
                                default:
                                    return false;
                            }
                            break;
                    }
                }

                #endregion

                if (!int.TryParse(bodyFrameInformation[0], out testParseInt)) continue;
                if (int.Parse(bodyFrameInformation[0]) < ScrollBarFrame.Minimum)
                    ScrollBarFrame.Minimum = int.Parse(bodyFrameInformation[0]);
                else if (int.Parse(bodyFrameInformation[0]) > ScrollBarFrame.Maximum)
                    ScrollBarFrame.Maximum = int.Parse(bodyFrameInformation[0]);

                if (float.Parse(bodyFrameInformation[3]) < MainWindow.MinX)
                    MainWindow.MinX = float.Parse(bodyFrameInformation[3]);
                else if (float.Parse(bodyFrameInformation[3]) > MainWindow.MaxX)
                    MainWindow.MaxX = float.Parse(bodyFrameInformation[3]);

                if (float.Parse(bodyFrameInformation[4]) < MainWindow.MinY)
                    MainWindow.MinY = float.Parse(bodyFrameInformation[4]);
                else if (float.Parse(bodyFrameInformation[4]) > MainWindow.MaxY)
                    MainWindow.MaxY = float.Parse(bodyFrameInformation[4]);

                if (float.Parse(bodyFrameInformation[5]) < MainWindow.MinZ)
                    MainWindow.MinZ = float.Parse(bodyFrameInformation[5]);
                else if (float.Parse(bodyFrameInformation[5]) > MainWindow.MaxZ)
                    MainWindow.MaxZ = float.Parse(bodyFrameInformation[5]);
            }
            return true;
        }

        #region Timer

        /// <summary>
        ///     loop timer for when the user click on button
        /// </summary>
        private static Timer _loopTimerClick;

        /// <summary>
        ///     If the timer is active where the user has click
        /// </summary>
        private void loopTimerClick_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (UpButton.IsMouseOver) MainWindow.RotateX += 3;
            if (LeftButton.IsMouseOver) MainWindow.RotateY -= 3;
            if (DownButton.IsMouseOver) MainWindow.RotateX -= 3;
            if (RightButton.IsMouseOver) MainWindow.RotateY += 3;
            if (ExpandButton.IsMouseOver) MainWindow.ChangeZoom(1);
            if (ReduceButton.IsMouseOver) MainWindow.ChangeZoom(-1);
            if (MainWindow.BodyInformation != null)
            {
                if (PreviousButton.IsMouseOver)
                    if (MainWindow.FrameId > 0)
                    {
                        MainWindow.FrameId--;
                        ScrollBarFrame.Dispatcher.BeginInvoke(
                            new Action(() => { ScrollBarFrame.Value = MainWindow.FrameId; }));
                    }
                if (NextButton.IsMouseOver)
                    ScrollBarFrame.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (MainWindow.FrameId < ScrollBarFrame.Maximum)
                        {
                            MainWindow.FrameId++;
                        }
                        ScrollBarFrame.Value = MainWindow.FrameId;
                    }));
            }
            if (!ResetButton.IsMouseOver) return;
            MainWindow.RotateX = 0;
            MainWindow.RotateY = 0;
            MainWindow.Zoom = 100f;
        }

        /// <summary>
        ///     loop timer for when play button is active
        /// </summary>
        private static Timer _loopTimerPlay;

        /// <summary>
        ///     change the frame id and reset if it's at end
        /// </summary>
        private void loopTimerPlay_Elapsed(object sender, ElapsedEventArgs e)
        {
            ScrollBarFrame.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (MainWindow.FrameId < ScrollBarFrame.Maximum) MainWindow.FrameId++;
                else MainWindow.FrameId = 0;

                ScrollBarFrame.Value = MainWindow.FrameId;
            }));
        }

        #endregion

        #region Event 

        /// <summary>
        ///     cancel close of windows
        /// </summary>
        private static void MyWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        ///     loadButton Event
        /// </summary>
        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FilterIndex = 1,
                Multiselect = true
            };
            // Set filter options and filter index.

            var userClickedOk = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOk != true) return;

            var i = 0;

            MainWindow.BodyInformation = new string[openFileDialog1.FileNames.Length][];

            foreach (var file in openFileDialog1.FileNames)
            {
                try
                {
                    if (MainWindow.BodyInformation == null) continue;

                    MainWindow.BodyInformation[i] = File.ReadAllText(file).Split('\r');

                    if (!Checkfile(MainWindow.BodyInformation[i]))
                    {
                        TextBlockInfo.Text = "Files Not correct";
                    }
                }
                catch (IOException)
                {
                    TextBlockInfo.Text = "Can't read files";
                }
                i++;
            }

            if (MainWindow.BodyInformation != null)
            {
                TextBlockInfo.Text = "Files OK";
                LoadButton.Visibility = Visibility.Hidden;
                EjectButton.Visibility = Visibility.Visible;
            }
            else
            {
                ScrollBarFrame.Minimum = 0;
                ScrollBarFrame.Maximum = 0;
                ScrollBarFrame.Value = 0;
                MainWindow.MinX = 100;
                MainWindow.MaxX = -100;
                MainWindow.MinY = 100;
                MainWindow.MaxY = -100;
                MainWindow.MinZ = 100;
                MainWindow.MaxZ = -100;
            }
        }

        /// <summary>
        ///     EjectButtonEvent
        /// </summary>
        private void EjectButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.BodyInformation = null;
            _loopTimerPlay.Enabled = false;
            EjectButton.Visibility = Visibility.Hidden;
            LoadButton.Visibility = Visibility.Visible;
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Visible;
            TextBlockInfo.Text = "Please load a file.";
            ScrollBarFrame.Minimum = 0;
            ScrollBarFrame.Maximum = 0;
            ScrollBarFrame.Value = 0;
            MainWindow.MinX = 100;
            MainWindow.MaxX = -100;
            MainWindow.MinY = 100;
            MainWindow.MaxY = -100;
            MainWindow.MinZ = 100;
            MainWindow.MaxZ = -100;
            MainWindow.FrameId = 0;
            LoadButton.CaptureMouse();
            LoadButton.ReleaseMouseCapture();
        }

        /// <summary>
        ///     PlayButton Event active _loopTimerPlay;
        /// </summary>
        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindow.BodyInformation == null) return;
            _loopTimerPlay.Enabled = true;
            PlayButton.Visibility = Visibility.Hidden;
            PauseButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     PauseButton Event unactive _loopTimerPlay;
        /// </summary>
        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            _loopTimerPlay.Enabled = false;
            PauseButton.Visibility = Visibility.Hidden;
            PlayButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     the user click on is mouse button active _loopTimerClick;
        /// </summary>
        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _loopTimerClick.Enabled = true;
        }

        /// <summary>
        ///     the user up his mouse Button unactive _loopTimerClick;
        /// </summary>
        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _loopTimerClick.Enabled = false;
        }

        /// <summary>
        ///     the user move the the ScrollBar change frame
        /// </summary>
        private void ScrollBarFrame_OnScroll(object sender, ScrollEventArgs e)
        {
            MainWindow.FrameId = (int) ScrollBarFrame.Value;
        }

        #endregion
    }
}