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

        private RecordMode recordMode = RecordMode.None;

        public void OptionsTable()
        {
            optionsTable.Rows.Add("RGB video file ext", "AVI");
            optionsTable.Rows.Add("RGB video file size", "Large");
            optionsTable.Rows.Add("RGB video file name", "rgb_[Size]_[DateTime]");
            optionsTable.Rows.Add("Depth file ext", "AVI");
            optionsTable.Rows.Add("Depth file name", "depth_[DateTime]");
            optionsTable.Rows.Add("Show rigs", "True");
            optionsTable.Rows.Add("Record rigs", "True");
            optionsTable.Rows.Add("Detect blocks", "False");
        }

        protected void InitializeButtons()
        {
            toolbarButtonGroup.Add(calibrateButton);
            toolbarButtonGroup.Add(recordButton);
            toolbarButtonGroup.Add(playButton);

            toolbarButtonSelected[calibrateButton] = toolbarButtonSelected[recordButton] = toolbarButtonSelected[playButton] = false;
        }
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
        private const int FRAME_PER_SECOND = 30;
        private BodyFrameReader bodyFrameReader = null;
        private Body[] bodies = null;
        private List<Tuple<JointType, JointType>> bones;
        private List<Pen> bodyColors;
        private readonly Brush trackedJointBrush = new SolidBrush(Color.FromArgb(255, 68, 192, 68));
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        //VideoWriter rgbWriter;
        String tempRgbFileName = "rgb_temp.avi";
        String tempDepthFileName = "depth_temp.dat";
        String tempRigFileName = "rig_temp.json";
        String tempConfigFileName = "config_temp.json";
        
        int depthFrame = 0;


        VideoFileWriter writer = new VideoFileWriter();
        BinaryWriter depthWriter;
        XmlWriter rigWriter;

        Mat matBuffer;

        
        BlockingCollection<Bitmap> bufferedImages;

        Bitmap rgbBitmap;

        public void initiateKinectViewers()
        {
            bufferedImages = new BlockingCollection<Bitmap>();

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

            //depthImage = new Image<Hsv, byte>(depthFrameDescription.Width, depthFrameDescription.Height);
        }

        bool hasTest = false;
        private void testMapping()
        {
            CameraSpacePoint[] csps = new CameraSpacePoint[8];
            // Skeleton_Joint_Locations
            //-0.426651,-0.11075,1.68441,-0.404479,0.103393,1.64627
            csps[0] = new CameraSpacePoint { X = -0.426651f, Y = -0.11075f, Z = 1.68441f };
            csps[1] = new CameraSpacePoint { X = -0.404479f, Y = 0.103393f, Z = 1.64627f };
            // Skeleton_Joint_Locations_Orig
            //-0.456807,-0.213856,1.96254,-0.428633,-0.0252217,1.85457
            csps[2] = new CameraSpacePoint { X = -0.456807f, Y = -0.213856f, Z = 1.96254f };
            csps[3] = new CameraSpacePoint { X = -0.428633f, Y = -0.0252217f, Z = 1.85457f };

            // Capture using aruco 
            // 0.00749806 -0.236892 1.49116 0.00622179 0.0498621 1.52369
            csps[4] = new CameraSpacePoint { X = 0.00749806f, Y = -0.236892f, Z = 1.49116f };
            csps[5] = new CameraSpacePoint { X = 0.00622179f, Y = 0.0498621f, Z = 1.52369f };

            // Capture using this software
            // -0.1901667 -0.5125275 1.233261 -0.1030212 -0.2572153 1.247053
            csps[6] = new CameraSpacePoint { X = -0.1901667f, Y = -0.5125275f, Z = 1.233261f };
            csps[7] = new CameraSpacePoint { X = -0.1030212f, Y = -0.2572153f, Z = 1.247053f };

            foreach (CameraSpacePoint csp in csps)
            {
                ColorSpacePoint c = this.coordinateMapper.MapCameraPointToColorSpace(csp);
                Trace.WriteLine(c.X + " " + c.Y);
            }
        }



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
            if (this.bodies == null) return;

            int penIndex = 0;

            foreach (Body body in this.bodies)
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

            startRecordRgb();
            startRecordDepth();
            startRecordRig();
        }

        private void startRecordRig()
        {
            try
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;

                rigWriter = XmlWriter.Create(tempRigFileName, ws);
                rigWriter.WriteStartDocument();
                rigWriter.WriteStartElement("BodyDataSequence");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void startRecordDepth()
        {
            if (depthFrameDescription != null)
            {
                try
                {
                    depthWriter = new BinaryWriter(File.Open(tempDepthFileName, FileMode.Create));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        
        object writeDepthLock = new object();
        object rigLock = new object();
        

        private void handleRecordButtonOff()
        {
            recordButton.ImageIndex = 0;
            recordMode = RecordMode.Playingback;
            finishWriteRgb();
            finishWriteDepth();

            lock (rigLock)
            {
                if (rigWriter != null)
                {
                    rigWriter.WriteEndElement();
                    rigWriter.WriteEndDocument();
                    rigWriter.Dispose();
                    rigWriter = null;
                }
            }

            playButton_MouseDown(null, null);

            helperTextBox.Text = "Temporary rgb file has " + rgbStreamedFrame + " frames \n"
                + "Temporary depth file has " + depthFrame + " frames \n";
            helperTextBox.Invalidate();
        }

        private void handlePlayButtonOn()
        {
            playButton.ImageIndex = 3;
        }

        private void handlePlayButtonOff()
        {
            playButton.ImageIndex = 2;
        }

    }
}

