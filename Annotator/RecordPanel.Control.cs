﻿using Emgu.CV;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class RecordPanel
    {
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
        private Bitmap rgbBitmap;
        private Bitmap depthBitmap;
        private FrameDescription colorFrameDescription = null;
        private FrameDescription depthFrameDescription = null;
        private byte[] rgbValues;
        private ushort[] depthValues;
        private byte[] depthValuesToByte;
        private float scale = 4000 / 256;
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

        VideoWriter rgbWriter;
        String tempRgbFileName = "rgb_temp.avi";
        String tempDepthFileName = "depth_temp.avi";
        String tempRigFileName = "rig_temp.json";
        String tempConfigFileName = "config_tempo.json";

        public void initiateKinectViewers()
        {
            


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

            rgbValues = new byte[colorFrameDescription.Width * colorFrameDescription.Height * 4];
            rgbBitmap = new Bitmap(colorFrameDescription.Width, colorFrameDescription.Height, PixelFormat.Format32bppRgb);

            depthValues = new ushort[depthFrameDescription.Width * depthFrameDescription.Height];
            depthValuesToByte = new byte[depthFrameDescription.Width * depthFrameDescription.Height * 4];
            depthBitmap = new Bitmap(depthFrameDescription.Width, depthFrameDescription.Height, PixelFormat.Format32bppRgb);

            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.cameraStatusLabel.Text = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            //CameraSpacePoint t = new CameraSpacePoint { X = -0.04383623f, Y = -0.4300487f, Z = 0.4517497f };
            //ColorSpacePoint x = this.coordinateMapper.MapCameraPointToColorSpace(t);

            
            //Console.WriteLine(x.X + " " + x.Y);

            //t = new CameraSpacePoint { X = -0.04383623f, Y = -0.1300487f, Z = 0.4517497f };
            //x = this.coordinateMapper.MapCameraPointToColorSpace(t);
            //Console.WriteLine(x.X + " " + x.Y);
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                if (this.bodies.Count() > 0)
                {
                    rgbBoard.Invalidate();
                }
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

        

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
                    {
                        colorFrame.CopyConvertedFrameDataToArray(rgbValues, ColorImageFormat.Bgra);

                        BitmapData bmapdata = rgbBitmap.LockBits(
                             new Rectangle(0, 0, colorFrameDescription.Width, colorFrameDescription.Height),
                             ImageLockMode.WriteOnly,
                             rgbBitmap.PixelFormat);

                        IntPtr ptr = bmapdata.Scan0;
                        Marshal.Copy(rgbValues, 0, ptr, colorFrameDescription.Width * colorFrameDescription.Height * 4);
                        

                        if ( toolbarButtonSelected[recordButton] && this.rgbWriter != null)
                        {
                            Console.WriteLine("Write");
                            this.rgbWriter.Write(new Mat(colorFrameDescription.Height, colorFrameDescription.Width,Emgu.CV.CvEnum.DepthType.Cv8U, 4, ptr, colorFrameDescription.Width * 4));
                        }

                        rgbBitmap.UnlockBits(bmapdata);

                        this.widthAspect = (float)this.rgbBoard.Width / colorFrameDescription.Width;
                        this.heightAspect = (float)this.rgbBoard.Height / colorFrameDescription.Height;
                        this.rgbBoard.Image = rgbBitmap;
                    }
                }
            }
        }

        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            using (DepthFrame depthFrame = e.FrameReference.AcquireFrame())
            {
                if (depthFrame != null)
                {
                    try
                    {
                        using (KinectBuffer depthBuffer = depthFrame.LockImageBuffer())
                        {
                            depthFrame.CopyFrameDataToArray(depthValues);

                            BitmapData bmapdata = depthBitmap.LockBits(
                                 new Rectangle(0, 0, depthFrameDescription.Width, depthFrameDescription.Height),
                                 ImageLockMode.WriteOnly,
                                 depthBitmap.PixelFormat);

                            IntPtr ptr = bmapdata.Scan0;
                            for (int i = 0; i < depthFrameDescription.Width * depthFrameDescription.Height; i ++)
                            {
                                depthValuesToByte[4 * i] = depthValuesToByte[4 * i + 1] = depthValuesToByte[4 * i + 2] = (byte)(depthValues[i] / scale);
                            }
                            Marshal.Copy(depthValuesToByte, 0, ptr, depthFrameDescription.Width * depthFrameDescription.Height * 4);

                            depthBitmap.UnlockBits(bmapdata);

                            this.depthBoard.Image = depthBitmap;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
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

                        if ( !float.IsInfinity(colorSpacePoint.X) && !float.IsInfinity(colorSpacePoint.Y))
                            jointPoints[jointType] = new Point((int) (colorSpacePoint.X * widthAspect), (int) (colorSpacePoint.Y * heightAspect) );
                    }

                    foreach (var bone in this.bones)
                    {
                        if (jointPoints.ContainsKey(bone.Item1) && jointPoints.ContainsKey(bone.Item2))
                        { 
                            Point p1 = jointPoints[bone.Item1];
                            Point p2 = jointPoints[bone.Item2];

                            Console.WriteLine(p1 + " " + p2);
                            e.Graphics.DrawLine(drawPen, p1, p2);
                        }
                    }

                    // Draw the joints
                    foreach (JointType jointType in jointPoints.Keys)
                    {
                        Brush drawBrush = null;

                        TrackingState trackingState = joints[jointType].TrackingState;

                        if (trackingState == TrackingState.Tracked)
                        {
                            drawBrush = this.trackedJointBrush;
                        }
                        else if (trackingState == TrackingState.Inferred)
                        {
                            drawBrush = this.inferredJointBrush;
                        }

                        Pen jointPen = new Pen(drawBrush);

                        if (drawBrush != null)
                        {
                            e.Graphics.DrawEllipse(jointPen, jointPoints[jointType].X - JointThickness, jointPoints[jointType].Y - JointThickness, 2 * JointThickness + 1, 2 * JointThickness + 1); 
                        }
                    }
                }
            }
        }

        private void handleRecordButtonOn()
        {
            recordButton.ImageIndex = 1;

            if (colorFrameDescription != null)
            {
                try
                {
                    rgbWriter = new VideoWriter(tempRgbFileName, -1, FRAME_PER_SECOND, new Size(colorFrameDescription.Width, colorFrameDescription.Height), true);
                    Console.WriteLine("Finish create writer");
                } catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        }

        private void handleRecordButtonOff()
        {
            recordButton.ImageIndex = 0;

            if (rgbWriter != null)
            {
                rgbWriter.Dispose();
                rgbWriter = null;
            }
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

