using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    partial class RecordPanel
    {
        /**
        <BodyDataSequence>
        <Body_Data>
	        <Subject id="003" name="Trainee"/>
	        <Timestamp time="2016-03-20T03:41:21.385519Z" timeFromBegin="513" frame="0" />
	        <Skeleton_Joint_Locations>
		        <Pts unit="m" ref="KC" format="SeqXYZ" schema="MIBA_SJ_v2.0">0.51399,0.163888,1.20627,0.494376,0.362051,1.19127,0.471949,0.557904,1.16773,0.429241,0.67668,1.15982,0.341345,0.492503,1.26928,0.263866,0.307297,1.31598,0.186578,0.146819,1.3238,0.149664,0.0832369,1.32295,0.561353,0.481381,1.11162,0.629317,0.310103,1.06671,0.631272,0.145862,1.03625,0.613708,0.105818,1.00281,0.461851,0.166738,1.22468,0.472659,0.0518708,1.15809,0.400671,0.038645,1.20482,0.350149,0.00618321,1.12578,0.550396,0.178427,1.15067,0.588908,0.0670342,1.08235,0.600085,0.0299289,0.950631,0.565235,0.0235135,0.868779,0.478074,0.509249,1.17511,0.10114,0.0282999,1.30987,0.157374,0.034974,1.32726,0.603486,0.0407229,0.99145,0.604288,0.0912648,0.963943</Pts>
	        </Skeleton_Joint_Locations>
	        <Skeleton_Joint_Locations_Orig>
		        <Pts unit="m" ref="KC" format="SeqXYZ" schema="MIBA_SJ_v2.0">0.534418,-0.159339,1.38631,0.523629,0.0243074,1.30818,0.509604,0.20306,1.22223,0.47055,0.315264,1.17649,0.370516,0.18268,1.34462,0.280264,0.0274687,1.45033,0.190772,-0.117396,1.50927,0.148615,-0.17576,1.52868,0.598848,0.106705,1.19051,0.66086,-0.074354,1.20017,0.653957,-0.240146,1.22203,0.632798,-0.288268,1.20135,0.479973,-0.147494,1.4038,0.484269,-0.279181,1.37381,0.408606,-0.271913,1.42476,0.352288,-0.326285,1.35606,0.572505,-0.166312,1.32608,0.606131,-0.296872,1.29324,0.613615,-0.376834,1.17335,0.575088,-0.40839,1.09354,0.513698,0.159136,1.24512,0.0944486,-0.229384,1.5331,0.154344,-0.220435,1.54836,0.618491,-0.353112,1.21073,0.621424,-0.314551,1.16712</Pts>
	        </Skeleton_Joint_Locations_Orig>
	        <Skeleton_ImgPlane_Joint_Locations>
		        <Pts unit="m" ref="KC" format="SeqXYZ" schema="MIBA_SJ_v2.0">1416.95,672.059,1435.35,530.055,1456.45,373.101,1440.35,264.64,1301.52,405.25,1210.66,529.425,1137.87,632.097,1106.29,671.581,1549.47,454.566,1599.85,616.003,1582.32,759.392,1574.1,805.729,1369.98,661.468,1382.01,765.986,1310.52,752.649,1283.68,805.6,1468.13,683.319,1508.63,794.422,1571.16,892.524,1577.82,948.552,1450.85,413.89,1068.25,708.361,1108.38,700.706,1556.69,860.906,1581.58,837.328</Pts>
	        </Skeleton_ImgPlane_Joint_Locations>
	        <Skeleton_Joint_Confidences>
		        <Vals schema="MIBA_SJ_v2.0">1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0.5,1,1,1,1,1,1,1,1,1</Vals>
	        </Skeleton_Joint_Confidences>
        </Body_Data>
        **/
        List<Body[]> recordedRigs = new List<Body[]>();
        List<int> recordedRigTimePoints = new List<int>();
        private AtomicBoolean rigDetected = new AtomicBoolean(false);

        private void startRecordRig()
        {
            recordedRigs = new List<Body[]>();
            recordedRigTimePoints = new List<int>();
            rigDetected = new AtomicBoolean(false);

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


        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            if (recordMode != RecordMode.Playingback)
            {
                bool dataReceived = false;
                Body[] tempo = null;

                using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        if (this.recordingBodies == null)
                        {
                            this.recordingBodies = new Body[bodyFrame.BodyCount];
                        }
                        tempo = new Body[bodyFrame.BodyCount];

                        // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                        // As long as those body objects are not disposed and not set to null in the array,
                        // those body objects will be re-used.
                        bodyFrame.GetAndRefreshBodyData(this.recordingBodies);
                        bodyFrame.GetAndRefreshBodyData(tempo);

                        dataReceived = true;
                    }
                }

                if (dataReceived)
                {
                    rgbBoard.Invalidate();
                    if (recordMode == RecordMode.Recording && this.rigWriter != null)
                    {
                        if (tmspStartRecording.HasValue)
                        {
                            // Save a copy of rig for later replay
                            
                            recordedRigs.Add(tempo.Where(body => body.IsTracked).ToArray());

                            var currentTime = DateTime.Now.TimeOfDay;
                            TimeSpan elapse = currentTime - tmspStartRecording.Value;
                            recordedRigTimePoints.Add((int)elapse.TotalMilliseconds);

                            WriteRig(elapse);
                        }
                    }
                }
            }
        }


        private async void WriteRigAsync(TimeSpan elapse)
        {
            await Task.Run(() => WriteRig(elapse));
        }

        private void WriteRig(TimeSpan elapse)
        {
            lock (writeRigLock)
            {
                for (int bodyIndex = 0; bodyIndex < this.recordingBodies.Count(); bodyIndex++)
                {
                    Body body = this.recordingBodies[bodyIndex];
                    if (body.IsTracked)
                    {
                        rigDetected.FalseToTrue();
                        rigWriter.WriteStartElement("Body_Data");

                        // Subject
                        rigWriter.WriteStartElement("Subject");
                        rigWriter.WriteAttributeString("id", bodyIndex + "");
                        rigWriter.WriteEndElement();

                        // Timestamp
                        rigWriter.WriteStartElement("Timestamp");
                        rigWriter.WriteAttributeString("time", DateTime.Now.ToString(@"yyyy-MM-ddTHH:mm:ssss.ffffffZ"));
                        rigWriter.WriteAttributeString("timeFromBegin", "" + (int)elapse.TotalMilliseconds);
                        rigWriter.WriteAttributeString("frame", "" + rgbStreamedFrame);
                        rigWriter.WriteEndElement();

                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                        Dictionary<JointType, ColorSpacePoint> colorSpaceJoints = new Dictionary<JointType, ColorSpacePoint>();
                        Dictionary<JointType, DepthSpacePoint> depthSpaceJoints = new Dictionary<JointType, DepthSpacePoint>();
                        Dictionary<JointType, float> confidences = new Dictionary<JointType, float>();

                        foreach (JointType jointType in joints.Keys)
                        {
                            CameraSpacePoint position = joints[jointType].Position;
                            if (position.Z < 0)
                            {
                                position.Z = InferredZPositionClamp;
                            }

                            DepthSpacePoint depthSpacePoint = this.coordinateMapper.MapCameraPointToDepthSpace(position);
                            ColorSpacePoint colorSpacePoint = this.coordinateMapper.MapCameraPointToColorSpace(position);

                            colorSpaceJoints[jointType] = colorSpacePoint;
                            depthSpaceJoints[jointType] = depthSpacePoint;
                            switch (joints[jointType].TrackingState)
                            {
                                case TrackingState.Tracked:
                                    confidences[jointType] = 1.0f;
                                    break;
                                case TrackingState.NotTracked:
                                    confidences[jointType] = 0.5f;
                                    break;
                                case TrackingState.Inferred:
                                    confidences[jointType] = 0.0f;
                                    break;
                            }
                        }

                        // Skeleton_Joint_Locations
                        rigWriter.WriteStartElement("Skeleton_Joint_Locations_Orig");
                        rigWriter.WriteStartElement("Pts");
                        rigWriter.WriteAttributeString("unit", "m");

                        List<string> tempo = new List<string>();
                        for (int i = 0; i < joints.Count; i++)
                        {
                            tempo.Add(joints[(JointType)i].Position.X + "," + joints[(JointType)i].Position.Y + "," + joints[(JointType)i].Position.Z);
                        }
                        rigWriter.WriteString(string.Join(",", tempo));
                        rigWriter.WriteEndElement();
                        rigWriter.WriteEndElement();

                        // Skeleton_Depth_Joint_Locations
                        rigWriter.WriteStartElement("Skeleton_Depth_Joint_Locations");
                        rigWriter.WriteStartElement("Pts");
                        rigWriter.WriteAttributeString("unit", "m");
                        tempo = new List<string>();
                        for (int i = 0; i < joints.Count; i++)
                        {
                            tempo.Add(depthSpaceJoints[(JointType)i].X + "," + depthSpaceJoints[(JointType)i].Y);
                        }
                        rigWriter.WriteString(string.Join(",", tempo));
                        rigWriter.WriteEndElement();
                        rigWriter.WriteEndElement();

                        // Skeleton_ImgPlane_Joint_Locations
                        rigWriter.WriteStartElement("Skeleton_ImgPlane_Joint_Locations");
                        rigWriter.WriteStartElement("Pts");
                        rigWriter.WriteAttributeString("unit", "px");
                        tempo = new List<string>();
                        for (int i = 0; i < joints.Count; i++)
                        {
                            tempo.Add(colorSpaceJoints[(JointType)i].X + "," + colorSpaceJoints[(JointType)i].Y);
                        }
                        rigWriter.WriteString(string.Join(",", tempo));
                        rigWriter.WriteEndElement();
                        rigWriter.WriteEndElement();

                        // Skeleton_Joint_Confidences
                        rigWriter.WriteStartElement("Skeleton_Joint_Confidences");
                        rigWriter.WriteStartElement("Vals");
                        tempo = new List<string>();
                        for (int i = 0; i < joints.Count; i++)
                        {
                            tempo.Add("" + confidences[(JointType)i]);
                        }
                        rigWriter.WriteString(string.Join(",", tempo));
                        rigWriter.WriteEndElement();
                        rigWriter.WriteEndElement();

                        rigWriter.WriteEndElement();
                    }
                }
            }
        }


        private void finishWriteRig()
        {
            lock (writeRigLock)
            {
                if (rigWriter != null)
                {
                    rigWriter.WriteEndElement();
                    rigWriter.WriteEndDocument();
                    rigWriter.Dispose();
                    rigWriter = null;

                    finishRecording.Signal();
                }
            }
        }
    }
}
