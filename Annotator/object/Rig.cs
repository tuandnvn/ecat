using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    // Rigs over multiple frames
    public class Rigs
    {
        public RigScheme rigScheme { get; }
        private Dictionary<int, RigFrame<PointF>> frameToRig;
        private Dictionary<int, RigFrame<Point3>> frameTo3DRig;
        public string rigFile { get; }
        public string rigSchemeFile { get; }
        private static Dictionary<Tuple<string, string>, Rigs> sourceFileToRig = new Dictionary<Tuple<string, string>, Rigs>();

        public Rigs(Dictionary<int, RigFrame<PointF>> frameToRig, Dictionary<int, RigFrame<Point3>> frameTo3DRig, RigScheme rigScheme, string rigFile, string rigSchemeFile)
        {
            this.rigScheme = rigScheme;
            this.frameToRig = frameToRig;
            this.frameTo3DRig = frameTo3DRig;
            this.rigFile = rigFile;
            this.rigSchemeFile = rigSchemeFile;
        }

        public static Rigs getRigFromSource(String rigFile, String rigSchemeFile)
        {
            var key = new Tuple<string, string>(rigFile, rigSchemeFile);
            if (sourceFileToRig.ContainsKey(key))
            {
                return sourceFileToRig[key];
            }

            if ( !File.Exists(rigFile) )
            {
                Console.WriteLine("Rig file " + rigFile + " could not be found!");
                return null;
            }

            if (!File.Exists(rigSchemeFile))
            {
                Console.WriteLine("Rig schema file " + rigSchemeFile + " could not be found!");
                return null;
            }

            sourceFileToRig[key] = readFromXml(rigFile, rigSchemeFile);
            return sourceFileToRig[key];
        }

        private static Rigs readFromXml(String rigFile, String rigSchemeFile)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var frameToRig = new Dictionary<int, RigFrame<PointF>>();
            var frameTo3DRig = new Dictionary<int, RigFrame<Point3>>();
            XmlDocument rigSchemeDoc = new XmlDocument();
            rigSchemeDoc.Load(rigSchemeFile);
            RigScheme rc = RigScheme.readFromXml(rigSchemeDoc);
            XmlDocument rigDoc = new XmlDocument();
            rigDoc.Load(rigFile);

            switch (rc.inputType)
            {
                case "xml":
                    XmlNode dataSequence = rigDoc.SelectSingleNode(rc.sequenceRoot);
                    foreach (XmlNode dataFrame in dataSequence.SelectNodes(rc.frameRoot))
                    {
                        int frameNo = int.Parse(dataFrame.SelectSingleNode(rc.frameNoPath).Value);
                        string frameStr = dataFrame.SelectSingleNode(rc.frameTimePath).Value;

                        DateTime dt = DateTime.ParseExact(frameStr.Substring(0, frameStr.Length - 1), @"yyyy-MM-ddTHH:mm:ssss.ffffff", provider);

                        if (!frameToRig.ContainsKey(frameNo))
                        {
                            frameToRig[frameNo] = new RigFrame<PointF>(dt);
                        }

                        if (!frameTo3DRig.ContainsKey(frameNo))
                        {
                            frameTo3DRig[frameNo] = new RigFrame<Point3>(dt);
                        }

                        int rigId = int.Parse(dataFrame.SelectSingleNode(rc.rigNoPath).Value);

                        XmlNode jointPoints = dataFrame.SelectSingleNode(rc.rigPointsPath);

                        var joints = new Dictionary<string, PointF>();
                        switch (rc.rigPointFormatType)
                        {
                            case "flat":
                                string[] components = jointPoints.InnerText.Split(new string[] { rc.rigPointFormatSeparated }, System.StringSplitOptions.RemoveEmptyEntries);
                                foreach (int jointIndex in rc.jointToJointName.Keys)
                                {
                                    string jointName = rc.jointToJointName[jointIndex].Item1;
                                    joints[jointName] = new PointF(float.Parse(components[2 * jointIndex]), float.Parse(components[2 * jointIndex + 1]));
                                }
                                break;
                            case "node":
                                break;
                        }
                        frameToRig[frameNo].addRigFrame(rigId, joints);

                        XmlNode joint3DPoints = dataFrame.SelectSingleNode(rc.rigPoint3DsPath);
                        var joint3Ds = new Dictionary<string, Point3>();
                        switch (rc.rigPointFormatType)
                        {
                            case "flat":
                                string[] components = joint3DPoints.InnerText.Split(new string[] { rc.rigPointFormatSeparated }, System.StringSplitOptions.RemoveEmptyEntries);
                                foreach (int jointIndex in rc.jointToJointName.Keys)
                                {
                                    string jointName = rc.jointToJointName[jointIndex].Item1;
                                    joint3Ds[jointName] = new Point3(float.Parse(components[3 * jointIndex]), float.Parse(components[3 * jointIndex + 1]), float.Parse(components[3 * jointIndex + 2]));
                                }
                                break;
                            case "node":
                                break;
                        }

                        frameTo3DRig[frameNo].addRigFrame(rigId, joint3Ds);
                    }
                    break;
                case "json":
                    break;
            }


            return new Rigs(frameToRig, frameTo3DRig, rc, rigFile, rigSchemeFile);
        }

        private static Dictionary<int, RigFrame<T>> deflat<T>(Dictionary<int, RigFrame<T>> frameToRig)
        {
            Dictionary<int, RigFrame<T>> deflated = new Dictionary<int, RigFrame<T>>();

            SortedList<int, RigFrame<T>> frameNoRigList = new SortedList<int, RigFrame<T>>(frameToRig);

            int count = 0;
            foreach (var frameNoRig in frameNoRigList)
            {
                deflated[++count] = frameNoRig.Value;
            }
            return deflated;
        }

        public List<int> getRigIndices(int frame)
        {
            RigFrame<PointF> rigFrame = frameToRig[frame];
            if (rigFrame != null)
            {
                return rigFrame.joints.Keys.ToList();
            }

            return new List<int>();
        }

        public Dictionary<string, PointF> getRigJoints(int frame, int rigIndex)
        {
            RigFrame<PointF> rigFrame = frameToRig[frame];
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                return rigFrame.joints[rigIndex];
            }
            return new Dictionary<string, PointF>();
        }

        public Dictionary<string, Point3> getRigJoints3D(int frame, int rigIndex)
        {
            RigFrame<Point3> rigFrame = frameTo3DRig[frame];
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                return rigFrame.joints[rigIndex];
            }
            return new Dictionary<string, Point3>();
        }

        public List<Tuple<string, string>> getRigBones(int frame, int rigIndex)
        {
            RigFrame<PointF> rigFrame = frameToRig[frame];
            var bones = new List<Tuple<string, string>>();
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                foreach (Point boneScheme in rigScheme.bones)
                {
                    var bone = new Tuple<string, string>(rigScheme.jointToJointName[boneScheme.X].Item1,
                        rigScheme.jointToJointName[boneScheme.Y].Item1);
                    bones.Add(bone);
                }
            }
            return bones;
        }

        public List<Tuple<string, string>> getRigBones3D(int frame, int rigIndex)
        {
            RigFrame<Point3> rigFrame = frameTo3DRig[frame];
            var bones = new List<Tuple<string, string>>();
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                var joints = rigFrame.joints[rigIndex];
                
                foreach (Point boneScheme in rigScheme.bones)
                {
                    var bone = new Tuple<string, string>(rigScheme.jointToJointName[boneScheme.X].Item1,
                        rigScheme.jointToJointName[boneScheme.Y].Item1);
                    bones.Add(bone);
                }
            }
            return bones;
        }

        public RigFigure<PointF> getRigFigure(int frame, int rigIndex)
        {
            return new RigFigure<PointF>(getRigJoints(frame, rigIndex), getRigBones(frame, rigIndex));
        }

        public RigFigure<Point3> getRigFigure3d(int frame, int rigIndex)
        {
            return new RigFigure<Point3>(getRigJoints3D(frame, rigIndex), getRigBones3D(frame, rigIndex));
        }

        public static RigFigure<T> getUpperBody<T>(RigFigure<T> rigFigure)
        {
            var t = new SortedSet<int>(RigScheme.torsoIndices);
            t.UnionWith(RigScheme.leftHandIndices);
            t.UnionWith(RigScheme.rightHandIndices);

            var listOfJointStrs = RigScheme.kinectV2jointToJointName.Where( p => t.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value).Values.Select( p => p.Item1 ).ToList();
            var upperBodyJoints = rigFigure.rigJoints.Where(p => listOfJointStrs.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);

            var upperBodyBones = rigFigure.rigBones.Where(p => listOfJointStrs.Contains(p.Item1) && listOfJointStrs.Contains(p.Item2)).ToList();

            return new RigFigure<T>(upperBodyJoints, upperBodyBones);
        }

        /// <summary>
        /// Create objects from rig file
        /// </summary>
        /// <param name="session"></param>
        /// <param name="videoFile"></param>
        /// <returns></returns>
        public Dictionary<int, RigObject> generateObjects(Session session, String videoFile)
        {
            normalizeFrames(session);

            var rigObjects = new Dictionary<int, RigObject>();

            foreach (int frame in frameToRig.Keys)
            {
                RigFrame<PointF> rf = frameToRig[frame];
                foreach (int rigIndex in rf.joints.Keys)
                {
                    if (!rigObjects.ContainsKey(rigIndex))
                    {
                        rigObjects[rigIndex] = new RigObject(session, "", Color.Green, 1, videoFile);
                        rigObjects[rigIndex].otherProperties["rigIndex"] = "" + rigIndex;
                        rigObjects[rigIndex].genType = Object.GenType.PROVIDED;
                        rigObjects[rigIndex].semanticType = "bodyRig";
                        rigObjects[rigIndex].otherProperties["source"] = rigFile;
                        rigObjects[rigIndex].otherProperties["sourceScheme"] = rigSchemeFile;
                    }

                    rigObjects[rigIndex].setBounding(frame, getRigFigure(frame, rigIndex), 1, new Point());
                    rigObjects[rigIndex].set3DBounding(frame, new RigLocationMark3D(frame, getRigFigure3d(frame, rigIndex)));
                }
            }

            return rigObjects;
        }


        /// <summary>
        /// After reading params file, you already has created rigObject
        /// Use loadDataForRig to load rig data
        /// </summary>
        /// <param name="rigFile"></param>
        /// <param name="rigSchemeFile"></param>
        /// <param name="rigIndex"></param>
        /// <param name="o"></param>
        public static void loadDataForRig(String rigFile, String rigSchemeFile, int rigIndex, RigObject o)
        {
            var rigs = getRigFromSource(rigFile, rigSchemeFile);

            if (rigs == null) return;

            rigs.normalizeFrames(o.session);

            foreach (int frame in rigs.frameToRig.Keys)
            {
                RigFrame<PointF> rf = rigs.frameToRig[frame];

                if (rf.joints.ContainsKey(rigIndex))
                {
                    o.setBounding(frame, rigs.getRigFigure(frame, rigIndex), 1, new Point());
                    o.set3DBounding(frame, new RigLocationMark3D(frame, rigs.getRigFigure3d(frame, rigIndex)));
                }
            }
        }

        private void normalizeFrames(Session session)
        {
            if (session.duration != 0 && session.startWriteRGB.HasValue)
            {
                // If you have information about the recorded time of the video
                // It means you recorded using Ecat
                // Projecting from recorded frame to playback frame
                int sessionLength = session.frameLength;
                DateTime startWriteRGB = session.startWriteRGB.Value;

                var tempoFrameToRig = new Dictionary<int, RigFrame<PointF>>();
                var tempoFrameTo3DRig = new Dictionary<int, RigFrame<Point3>>();

                foreach (int frame in frameToRig.Keys)
                {
                    DateTime dt = frameToRig[frame].dt;
                    long timeFromStart = (long)dt.Subtract(startWriteRGB).TotalMilliseconds;

                    long timeStepForFrame = session.duration / sessionLength;
                    int rgbFrame = (int) (timeFromStart / timeStepForFrame) + 1;

                    tempoFrameToRig[rgbFrame] = frameToRig[frame];
                    tempoFrameTo3DRig[rgbFrame] = frameTo3DRig[frame];
                }
                frameToRig = tempoFrameToRig;
                frameTo3DRig = tempoFrameTo3DRig;
            }
            else
            {
                // It means you used CwC recorder
                //// Problem with using recorded session from CwC apparatus
                //// You might need to deflate the frameToRig dictionary 
                //// so that frameNo are consecutive numbers
                frameToRig = deflat(frameToRig);
                frameTo3DRig = deflat(frameTo3DRig);
            }
        }
    }

    public class RigFrame<T>
    {
        /// <summary>
        /// Rigs(s) for each frame. 
        /// There might be multiple rigs, the first integer key is rig index (as tracked by Kinect API)
        /// The string key is joint name (shorten version of joint name, for example SpineBase)
        /// T is the location of joint (could be 2d or 3d)
        /// </summary>
        public Dictionary<int, Dictionary<string, T>> joints { get; }
        public DateTime dt { get; }

        public RigFrame(DateTime dt)
        {
            this.dt = dt;
            joints = new Dictionary<int, Dictionary<string, T>>();
        }

        //public RigFrame()
        //{
        //    this.timeFromBegin = 0;
        //    joints = new Dictionary<int, Dictionary<string, T>>();
        //}

        public void addRigFrame(int rigIndex, Dictionary<string, T> rig)
        {
            joints[rigIndex] = rig;
        }

    }
}
