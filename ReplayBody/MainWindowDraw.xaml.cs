using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ReplayBody
{
    public partial class MainWindow
    {
        /// <summary>
        ///     draw the body on screen
        /// </summary>
        private static void DrawSkeleton(string[] body)
        {
            foreach (var line in body)
            {
                var bodyFrameInformation = line.Split(',');

                int testParseInt;

                if (!int.TryParse(bodyFrameInformation[0], out testParseInt)) continue;
                if (int.Parse(bodyFrameInformation[0]) > FrameId) break;

                if (int.Parse(bodyFrameInformation[0]) != FrameId) continue;

                var x = -((MinX + MaxX)/2);
                var y = -((MinY + MaxY)/2);
                var z = -((MinZ + MaxZ)/2);

                #region tabOfVector 

                //construct all vector for each point 
                Vector3[] bodyJoint =
                {
                    new Vector3(x + float.Parse(bodyFrameInformation[3]), y + float.Parse(bodyFrameInformation[4]),
                        z + float.Parse(bodyFrameInformation[5])), //SpineBase
                    new Vector3(x + float.Parse(bodyFrameInformation[8]), y + float.Parse(bodyFrameInformation[9]),
                        z + float.Parse(bodyFrameInformation[10])), //SpineMid
                    new Vector3(x + float.Parse(bodyFrameInformation[13]), y + float.Parse(bodyFrameInformation[14]),
                        z + float.Parse(bodyFrameInformation[15])), //Neck
                    new Vector3(x + float.Parse(bodyFrameInformation[18]), y + float.Parse(bodyFrameInformation[19]),
                        z + float.Parse(bodyFrameInformation[20])), //Head
                    new Vector3(x + float.Parse(bodyFrameInformation[23]), y + float.Parse(bodyFrameInformation[24]),
                        z + float.Parse(bodyFrameInformation[25])), //ShoulderLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[28]), y + float.Parse(bodyFrameInformation[29]),
                        z + float.Parse(bodyFrameInformation[30])), //ElbowLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[33]), y + float.Parse(bodyFrameInformation[34]),
                        z + float.Parse(bodyFrameInformation[35])), //WristLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[38]), y + float.Parse(bodyFrameInformation[39]),
                        z + float.Parse(bodyFrameInformation[40])), //HandLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[43]), y + float.Parse(bodyFrameInformation[44]),
                        z + float.Parse(bodyFrameInformation[45])), //ShoulderRight
                    new Vector3(x + float.Parse(bodyFrameInformation[48]), y + float.Parse(bodyFrameInformation[49]),
                        z + float.Parse(bodyFrameInformation[50])), //ElbowRight
                    new Vector3(x + float.Parse(bodyFrameInformation[53]), y + float.Parse(bodyFrameInformation[54]),
                        z + float.Parse(bodyFrameInformation[55])), //WristRight
                    new Vector3(x + float.Parse(bodyFrameInformation[58]), y + float.Parse(bodyFrameInformation[59]),
                        z + float.Parse(bodyFrameInformation[60])), //HandRight
                    new Vector3(x + float.Parse(bodyFrameInformation[63]), y + float.Parse(bodyFrameInformation[64]),
                        z + float.Parse(bodyFrameInformation[65])), //HipLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[68]), y + float.Parse(bodyFrameInformation[69]),
                        z + float.Parse(bodyFrameInformation[70])), //KneeLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[73]), y + float.Parse(bodyFrameInformation[74]),
                        z + float.Parse(bodyFrameInformation[75])), //AnkleLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[78]), y + float.Parse(bodyFrameInformation[79]),
                        z + float.Parse(bodyFrameInformation[80])), //FootLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[83]), y + float.Parse(bodyFrameInformation[84]),
                        z + float.Parse(bodyFrameInformation[85])), //HipRight
                    new Vector3(x + float.Parse(bodyFrameInformation[88]), y + float.Parse(bodyFrameInformation[89]),
                        z + float.Parse(bodyFrameInformation[90])), //KneeRight
                    new Vector3(x + float.Parse(bodyFrameInformation[93]), y + float.Parse(bodyFrameInformation[94]),
                        z + float.Parse(bodyFrameInformation[95])), //AnkleRight
                    new Vector3(x + float.Parse(bodyFrameInformation[98]), y + float.Parse(bodyFrameInformation[99]),
                        z + float.Parse(bodyFrameInformation[100])), //FootRight
                    new Vector3(x + float.Parse(bodyFrameInformation[103]), y + float.Parse(bodyFrameInformation[104]),
                        z + float.Parse(bodyFrameInformation[105])), //SpineShoulder
                    new Vector3(x + float.Parse(bodyFrameInformation[108]), y + float.Parse(bodyFrameInformation[109]),
                        z + float.Parse(bodyFrameInformation[110])), //HandTipLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[113]), y + float.Parse(bodyFrameInformation[114]),
                        z + float.Parse(bodyFrameInformation[115])), //ThumbLeft
                    new Vector3(x + float.Parse(bodyFrameInformation[118]), y + float.Parse(bodyFrameInformation[119]),
                        z + float.Parse(bodyFrameInformation[120])), //HandTipRight
                    new Vector3(x + float.Parse(bodyFrameInformation[123]), y + float.Parse(bodyFrameInformation[124]),
                        z + float.Parse(bodyFrameInformation[125])) //ThumbRight
                };

                #endregion

                #region drawPoint

                GL.PointSize(10);
                GL.Begin(BeginMode.Points);
                var checkStateint = -3;
                foreach (var joint in bodyJoint)
                {
                    switch (bodyFrameInformation[checkStateint += 5])
                    {
                        case "Goal":
                            GL.Color3(Color.Red);
                            GL.Vertex3(joint);
                            break;
                        case "Tracked":
                            GL.Color3(Color.Blue);
                            GL.Vertex3(joint);
                            break;
                    }
                }
                GL.End();

                #endregion

                #region drawLine

                GL.Color3(Color.White);
                GL.LineWidth(5);
                GL.Begin(BeginMode.Lines);
                GL.Vertex3(bodyJoint[3]);
                GL.Vertex3(bodyJoint[2]);
                GL.Vertex3(bodyJoint[2]);
                GL.Vertex3(bodyJoint[20]);
                GL.Vertex3(bodyJoint[20]);
                GL.Vertex3(bodyJoint[4]);
                GL.Vertex3(bodyJoint[20]);
                GL.Vertex3(bodyJoint[8]);
                GL.Vertex3(bodyJoint[20]);
                GL.Vertex3(bodyJoint[1]);
                GL.Vertex3(bodyJoint[4]);
                GL.Vertex3(bodyJoint[5]);
                GL.Vertex3(bodyJoint[8]);
                GL.Vertex3(bodyJoint[9]);
                GL.Vertex3(bodyJoint[5]);
                GL.Vertex3(bodyJoint[6]);
                GL.Vertex3(bodyJoint[9]);
                GL.Vertex3(bodyJoint[10]);
                GL.Vertex3(bodyJoint[6]);
                GL.Vertex3(bodyJoint[7]);
                GL.Vertex3(bodyJoint[10]);
                GL.Vertex3(bodyJoint[11]);
                GL.Vertex3(bodyJoint[7]);
                GL.Vertex3(bodyJoint[21]);
                GL.Vertex3(bodyJoint[11]);
                GL.Vertex3(bodyJoint[23]);
                GL.Vertex3(bodyJoint[6]);
                GL.Vertex3(bodyJoint[22]);
                GL.Vertex3(bodyJoint[10]);
                GL.Vertex3(bodyJoint[24]);
                GL.Vertex3(bodyJoint[1]);
                GL.Vertex3(bodyJoint[0]);
                GL.Vertex3(bodyJoint[0]);
                GL.Vertex3(bodyJoint[12]);
                GL.Vertex3(bodyJoint[0]);
                GL.Vertex3(bodyJoint[16]);
                GL.Vertex3(bodyJoint[12]);
                GL.Vertex3(bodyJoint[13]);
                GL.Vertex3(bodyJoint[16]);
                GL.Vertex3(bodyJoint[17]);
                GL.Vertex3(bodyJoint[13]);
                GL.Vertex3(bodyJoint[14]);
                GL.Vertex3(bodyJoint[17]);
                GL.Vertex3(bodyJoint[18]);
                GL.Vertex3(bodyJoint[14]);
                GL.Vertex3(bodyJoint[15]);
                GL.Vertex3(bodyJoint[18]);
                GL.Vertex3(bodyJoint[19]);
                GL.End();

                #endregion
            }
        }
    }
}