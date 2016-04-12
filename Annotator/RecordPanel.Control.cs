using AForge.Video.FFMPEG;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Kinect;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Annotator
{
    partial class RecordPanel
    {
        public enum RecordMode
        {
            None,
            Recording,
            Playingback,
            Calibrating,
        }

        internal RecordMode recordMode = RecordMode.None;

        /// <summary>
        /// 
        /// </summary>
        private CountdownEvent finishRecording = null;

        /// <summary>
        /// 
        /// </summary>
        private CountdownEvent hasArrived = null;
        private bool hasDepthArrived = false;
        private bool hasColorArrived = false;

        private const float InferredZPositionClamp = 0.1f;
        private KinectSensor kinectSensor = null;
        private ColorFrameReader colorFrameReader = null;
        private DepthFrameReader depthFrameReader = null;
        //private Bitmap rgbBitmap;
        private Bitmap depthBitmap;
        private FrameDescription colorFrameDescription = null;
        private FrameDescription depthFrameDescription = null;
        private byte[] rgbValues;
        private ushort[] depthValues;
        private byte[] depthValuesToByte;
        private float scale = 8000 / 256;
        private float widthAspect = 1;
        private float heightAspect = 1;

        private CoordinateMapper coordinateMapper = null;
        private const float JointThickness = 1;

        private BodyFrameReader bodyFrameReader = null;
        private Body[] recordingBodies = null;
        private List<Tuple<JointType, JointType>> bones;
        private List<Pen> bodyColors;
        private readonly Brush trackedJointBrush = new SolidBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        //VideoWriter rgbWriter;
        String tempRgbFileName = "rgb_temp.avi";
        String tempDepthFileName = "depth_temp.dat";
        String tempRigFileName = "rig_temp.json";
        String tempConfigFileName = "config_temp.json";
        Dictionary<string, string> mapFileName;

        XmlWriter rigWriter;
        Mat matBuffer;
        Bitmap rgbBitmap;
        
        object writeRigLock = new object();

        public void InitializeOptionsTable()
        {
            optionsTable.Rows.Add("Temporary file path", ".");
            optionsTable.Rows.Add("RGB video file ext", "avi");
            optionsTable.Rows.Add("RGB Fps", fps + "");
            optionsTable.Rows.Add("RGB bitrate", quality + "");
            optionsTable.Rows.Add("RGB video file name", "rgb_[DateTime]");
            optionsTable.Rows.Add("Depth file ext", "dat");
            optionsTable.Rows.Add("Depth file name", "depth_[DateTime]");
            optionsTable.Rows.Add("Show rigs", "True");
            optionsTable.Rows.Add("Record rigs", "True");
            optionsTable.Rows.Add("Rig file name", "rig_[DateTime]");
            //optionsTable.Rows.Add("Detect blocks", "False");
            mapFileName = new Dictionary<string, string>();

            // No change to RGB video file name
            optionsTable.Rows[4].Cells[1].ReadOnly = true;
            optionsTable.Rows[5].Cells[1].ReadOnly = true;
            optionsTable.Rows[6].Cells[1].ReadOnly = true;

            changeRowToTrueFall(optionsTable, 7, 1);
            changeRowToTrueFall(optionsTable, 8, 1);
        }


        private void optionsTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Handle RGB FPs
            if (e.RowIndex == 2 && e.ColumnIndex == 1)
            {
                try
                {
                    fps = int.Parse((string)optionsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                } catch
                {
                }
            }

            if (e.RowIndex == 3 && e.ColumnIndex == 1)
            {
                try
                {
                    quality = int.Parse((string)optionsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
                catch
                {
                }
            }
        }

        private void changeRowToTrueFall(DataGridView optionsTable, int row, int col)
        {
            List<bool> bools = new List<bool>() { true, false };
            DataGridViewComboBoxCell c = new DataGridViewComboBoxCell();
            c.DataSource = bools;
            c.Value = true;

            optionsTable.Rows[row].Cells[col] = c;
        }

        protected void InitializeButtons()
        {
            toolbarButtonGroup.Add(calibrateButton);
            toolbarButtonGroup.Add(recordButton);
            toolbarButtonGroup.Add(playButton);

            toolbarButtonSelected[calibrateButton] = toolbarButtonSelected[recordButton] = toolbarButtonSelected[playButton] = false;
        }

        public void initiateKinectViewers()
        {
            bufferedImages = new BlockingCollection<Tuple<Bitmap, TimeSpan>>();

            // a bone defined as a line between two joints
            this.bones = new List<Tuple<JointType, JointType>>();

            // Torso
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Head, JointType.Neck));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.Neck, JointType.SpineShoulder));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.SpineMid));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineMid, JointType.SpineBase));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineShoulder, JointType.ShoulderLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.SpineBase, JointType.HipLeft));

            // Right Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderRight, JointType.ElbowRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowRight, JointType.WristRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.HandRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandRight, JointType.HandTipRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristRight, JointType.ThumbRight));

            // Left Arm
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ShoulderLeft, JointType.ElbowLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.ElbowLeft, JointType.WristLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.HandLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HandLeft, JointType.HandTipLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.WristLeft, JointType.ThumbLeft));

            // Right Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipRight, JointType.KneeRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeRight, JointType.AnkleRight));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleRight, JointType.FootRight));

            // Left Leg
            this.bones.Add(new Tuple<JointType, JointType>(JointType.HipLeft, JointType.KneeLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.KneeLeft, JointType.AnkleLeft));
            this.bones.Add(new Tuple<JointType, JointType>(JointType.AnkleLeft, JointType.FootLeft));

            // populate body colors, one for each BodyIndex
            this.bodyColors = new List<Pen>();

            this.bodyColors.Add(new Pen(Brushes.Red, 6));
            this.bodyColors.Add(new Pen(Brushes.Orange, 6));
            this.bodyColors.Add(new Pen(Brushes.Green, 6));
            this.bodyColors.Add(new Pen(Brushes.Blue, 6));
            this.bodyColors.Add(new Pen(Brushes.Indigo, 6));
            this.bodyColors.Add(new Pen(Brushes.Violet, 6));

            this.kinectSensor = KinectSensor.GetDefault();

            // Can't map correctly right after initiated
            // Can only work after first frame arrives
            this.coordinateMapper = this.kinectSensor.CoordinateMapper;

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();
            this.depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();
            this.colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;
            this.depthFrameReader.FrameArrived += this.Reader_DepthFrameArrived;
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

            // create the colorFrameDescription from the ColorFrameSource using Bgra format
            colorFrameDescription = this.kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Bgra);
            depthFrameDescription = this.kinectSensor.DepthFrameSource.FrameDescription;

            matBuffer = new Mat(colorFrameDescription.Height, colorFrameDescription.Width, Emgu.CV.CvEnum.DepthType.Cv8U, 4);


            rgbValues = new byte[colorFrameDescription.Width * colorFrameDescription.Height * 4];

            depthValues = new ushort[depthFrameDescription.Width * depthFrameDescription.Height];
            depthValuesToByte = new byte[depthFrameDescription.Width * depthFrameDescription.Height * 4];
            depthBitmap = new Bitmap(depthFrameDescription.Width, depthFrameDescription.Height, PixelFormat.Format32bppRgb);

            rgbBitmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, PixelFormat.Format32bppRgb);
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.cameraStatusLabel.Text = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            hasArrived = new CountdownEvent(2);

            Task.Run(() => calculateProjections());

            //depthImage = new Image<Hsv, byte>(depthFrameDescription.Width, depthFrameDescription.Height);
        }

        public void calculateProjections()
        {
            hasArrived.Wait();
            KinectUtils.calculateProject(this.coordinateMapper, "out.txt");
        }


        //public Point3F getDepth(Point rgbCoordinate, ushort[] depthValues, int depthWidth, int depthHeight)
        //{
        //    rgbCoordinate.X;

        //}


        public void releaseKinectViewers()
        {
            if (this.colorFrameReader != null)
            {
                this.colorFrameReader.Dispose();
                this.colorFrameReader = null;
            }

            if (this.depthFrameReader != null)
            {
                this.depthFrameReader.Dispose();
                this.depthFrameReader = null;
            }

            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            this.cameraStatusLabel.Text = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;
        }

        List<Button> toolbarButtonGroup = new List<Button>();
        Dictionary<Button, bool> toolbarButtonSelected = new Dictionary<Button, bool>();

        private void calibrateButton_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(calibrateButton, toolbarButtonGroup, !toolbarButtonSelected[calibrateButton]);
        }

        private void recordButton_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(recordButton, toolbarButtonGroup, !toolbarButtonSelected[recordButton]);

            if (toolbarButtonSelected[recordButton])
            {
                handleRecordButtonOn();
            }
            else
            {
                handleRecordButtonOff();
            }
        }

        private void playButton_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(playButton, toolbarButtonGroup, !toolbarButtonSelected[playButton]);

            if (toolbarButtonSelected[playButton])
            {
                handlePlayButtonOn();
            }
            else
            {
                handlePlayButtonOff();
            }
        }


        private void selectButtonDrawing(Button b, List<Button> buttonGroup, bool select)
        {
            toolbarButtonSelected[b] = select;

            if (select)
            {
                foreach (Button b2 in buttonGroup)
                {
                    if (b2 != b)
                    {
                        //selectButtonDrawing(b2, buttonGroup, false);
                        b2.Enabled = false;
                    }
                }
                b.BackColor = Color.White;
            }
            else
            {
                foreach (Button b2 in buttonGroup)
                {
                    if (b2 != b)
                    {
                        //selectButtonDrawing(b2, buttonGroup, false);
                        b2.Enabled = true;
                    }
                }
                b.BackColor = Color.Transparent;
            }
        }


        private void rgbBoard_Paint(object sender, PaintEventArgs e)
        {
            int penIndex = 0;

            Body[] showBodies = null;

            switch (recordMode)
            {
                case RecordMode.None:
                    showBodies = this.recordingBodies;
                    break;
                case RecordMode.Recording:
                    showBodies = this.recordingBodies;
                    break;
                case RecordMode.Playingback:
                    showBodies = this.playbackBodies;
                    break;
                case RecordMode.Calibrating:
                    showBodies = new Body[0];
                    break;
                default:
                    showBodies = new Body[0];
                    break;
            }

            if (showBodies == null) return;

            foreach (Body body in showBodies)
            {
                Pen drawPen = this.bodyColors[penIndex++];

                if (body.IsTracked)
                {
                    IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                    Dictionary<JointType, Point> jointPoints = new Dictionary<JointType, Point>();

                    foreach (JointType jointType in joints.Keys)
                    {
                        CameraSpacePoint position = joints[jointType].Position;
                        if (position.Z < 0)
                        {
                            position.Z = InferredZPositionClamp;
                        }

                        ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);

                        //Console.WriteLine(position.X + " " + position.Y + " " + position.Z + " " + colorSpacePoint.X + " " + colorSpacePoint.Y);

                        if (!float.IsInfinity(colorSpacePoint.X) && !float.IsInfinity(colorSpacePoint.Y))
                            jointPoints[jointType] = new Point((int)(colorSpacePoint.X * widthAspect), (int)(colorSpacePoint.Y * heightAspect));
                    }

                    foreach (var bone in this.bones)
                    {
                        if (jointPoints.ContainsKey(bone.Item1) && jointPoints.ContainsKey(bone.Item2))
                        {
                            Point p1 = jointPoints[bone.Item1];
                            Point p2 = jointPoints[bone.Item2];

                            e.Graphics.DrawLine(drawPen, p1, p2);
                        }
                    }

                    // Draw the joints
                    foreach (JointType jointType in jointPoints.Keys)
                    {
                        Brush drawBrush = null;

                        TrackingState trackingState = joints[jointType].TrackingState;

                        switch (trackingState)
                        {
                            case TrackingState.Tracked:
                                drawBrush = this.trackedJointBrush;
                                break;
                            case TrackingState.Inferred:
                                drawBrush = this.inferredJointBrush;
                                break;
                        }

                        if (drawBrush != null)
                        {
                            Pen jointPen = new Pen(drawBrush);
                            e.Graphics.DrawEllipse(jointPen, jointPoints[jointType].X - JointThickness, jointPoints[jointType].Y - JointThickness, 2 * JointThickness + 1, 2 * JointThickness + 1);
                        }
                    }
                }
            }
        }

        private void handleRecordButtonOn()
        {
            recordButton.ImageIndex = 1;
            recordMode = RecordMode.Recording;

            finishRecording = new CountdownEvent(3);

            // Disable changing to options table
            optionsTable.Enabled = false;

            mapFileName[tempRgbFileName] = ((string)optionsTable.Rows[4].Cells[1].Value).Replace("DateTime", DateTime.Now.ToShortTimeString());
            mapFileName[tempDepthFileName] = ((string)optionsTable.Rows[6].Cells[1].Value).Replace("DateTime", DateTime.Now.ToShortTimeString());
            mapFileName[tempRigFileName] = ((string)optionsTable.Rows[9].Cells[1].Value).Replace("DateTime", DateTime.Now.ToShortTimeString());

            startRecordRgb();
            startRecordDepth();
            startRecordRig();
        }

        private void handleRecordButtonOff()
        {
            recordButton.ImageIndex = 0;

            optionsTable.Enabled = true;
            finishWriteRgb();
            finishWriteDepth();
            finishWriteRig();

            playButton_MouseDown(null, null);
        }


        private void saveRecordedSession_Click(object sender, EventArgs e)
        {
            Project currentProject = main.selectedProject;

            var result = MessageBox.Show(main, "Do you want to add captured session into project " + currentProject.getProjectName() +
                "?. Yes if you do, no if you want to save it into a separate folder", "Save session", MessageBoxButtons.YesNoCancel);

            switch (result)
            {
                case DialogResult.Yes:
                    SessionInfo sessionInfo = new SessionInfo(main, currentProject.getProjectName());
                    sessionInfo.Location = new Point(this.Location.X + (int)(sessionInfo.Width / 2.5), this.Location.Y + sessionInfo.Height / 2);
                    sessionInfo.okButton.Click += new System.EventHandler(this.addSessionOkClick);
                    sessionInfo.Show();
                    break;
                case DialogResult.No:
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult folderResult= fbd.ShowDialog(main);
                    if (folderResult == DialogResult.OK)
                    {
                        string pathToFolder = fbd.SelectedPath;

                        foreach (String fileName in new []{ tempRgbFileName , tempDepthFileName, tempConfigFileName }){
                            string dstFileName = pathToFolder + Path.DirectorySeparatorChar + mapFileName[fileName];
                            if (!File.Exists(dstFileName))
                                File.Copy(fileName, dstFileName);
                        }
                    }
                    break;
                case DialogResult.Cancel:
                    break;
                default:
                    break;
            }

            // Back to annotating
            main.tabs.SelectedIndex = 0;
        }

        private void addSessionOkClick(object sender, EventArgs e)
        {
            Console.WriteLine("addSessionOkClick" );
            if (main.currentSession != null)
            {
                foreach (String fileName in new[] { tempRgbFileName, tempDepthFileName, tempConfigFileName })
                {
                    main.copyFileIntoLocalSession(fileName, mapFileName[fileName]);
                }
            }
        }
    }
}

