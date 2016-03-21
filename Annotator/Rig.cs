using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public struct Point3F
    {
        float X;
        float Y;
        float Z;
    }

    public class RigFigure<T>
    {
        public Dictionary<int, T> rigJoints { get; }
        public List<Tuple<T, T>> rigBones { get; }

        public RigFigure(Dictionary<int, T> rigJoints, List<Tuple<T, T>> rigBones)
        {
            this.rigBones = rigBones;
            this.rigJoints = rigJoints;
        }
    }

    // Rigs over multiple frames
    public class Rigs <T>
    {
        public RigScheme rigScheme { get; }
        private Dictionary<int, RigFrame<T>> frameToRig;
        public string rigFile { get; }
        public string rigSchemeFile { get; }
        private static Dictionary<Tuple<string, string>, Rigs<T>> sourceFileToRig = new Dictionary<Tuple<string, string>, Rigs<T>>();

        public Rigs(Dictionary<int, RigFrame<T> > frameToRig, RigScheme rigScheme, string rigFile, string rigSchemeFile)
        {
            this.rigScheme = rigScheme;
            this.frameToRig = frameToRig;
            this.rigFile = rigFile;
            this.rigSchemeFile = rigSchemeFile;
        }

        public static Rigs<T> getRigFromSource(String rigFile, String rigSchemeFile)
        {
            var key = new Tuple<string, string>(rigFile, rigSchemeFile);
            if (sourceFileToRig.ContainsKey(key)) {
                return sourceFileToRig[key];
            }

            sourceFileToRig[key] = readFromXml(rigFile, rigSchemeFile);
            return sourceFileToRig[key];
        }

        private static Rigs<T> readFromXml(String rigFile, String rigSchemeFile)
        {
            if (typeof(T) != typeof(Point) && typeof(T) != typeof(PointF) && typeof(T) != typeof(Point3F))
                return null;
            var frameToRig = new Dictionary<int, RigFrame<T>>();
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

                        if ( !frameToRig.ContainsKey(frameNo) )
                        {
                            frameToRig[frameNo] = new RigFrame<T>();
                        }

                        int rigId = int.Parse(dataFrame.SelectSingleNode(rc.rigNoPath).Value);
                        
                        XmlNode jointPoints = dataFrame.SelectSingleNode(rc.rigPointsPath);

                        var joints = new Dictionary<int, T>();
                        switch ( rc.rigPointFormatType)
                        {
                            case "flat":
                                string[] components = jointPoints.InnerText.Split(new string[] { rc.rigPointFormatSeparated }, System.StringSplitOptions.RemoveEmptyEntries);
                                foreach (int jointIndex in rc.jointToJointName.Keys)
                                {
                                    switch (typeof(T).ToString())
                                    {
                                        case "System.Drawing.Point":
                                            joints[jointIndex] = (T) Activator.CreateInstance(typeof(T), new object[] { (int)float.Parse(components[2 * jointIndex]), (int) float.Parse(components[2 * jointIndex + 1]) });
                                            break;
                                        case "System.Drawing.PointF":
                                            joints[jointIndex] = (T) Activator.CreateInstance(typeof(T), new object[] { float.Parse(components[3 * jointIndex]), float.Parse(components[3 * jointIndex + 1]), float.Parse(components[3 * jointIndex + 2]) });
                                            break;
                                        case "Annotator.Point3F":
                                            joints[jointIndex] = (T)Activator.CreateInstance(typeof(T), new object[] { float.Parse(components[2 * jointIndex]), float.Parse(components[2 * jointIndex + 1]) });
                                            break;
                                    }
                                }
                                break;
                            case "node":
                                break;
                        }

                        frameToRig[frameNo].addRigFrame(rigId, joints);
                    }
                    break;
                case "json":
                    break;
            }

            // Problem with using recorded session from CwC apparatus
            // You might need to deflate the frameToRig dictionary 
            // so that frameNo are consecutive numbers
            frameToRig = deflat(frameToRig);

            var specificRigType = typeof(Rigs<>).MakeGenericType(typeof(T));
            return (Rigs<T>) Activator.CreateInstance(specificRigType, new object[] { frameToRig, rc, rigFile, rigSchemeFile });
        }

        private static Dictionary<int, RigFrame<T>> deflat(Dictionary<int, RigFrame<T>> frameToRig)
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
            RigFrame<T> rigFrame = frameToRig[frame];
            if (rigFrame != null)
            {
                return rigFrame.joints.Keys.ToList();
            }

            return new List<int>();
        }

        public Dictionary<int, T> getRigJoints(int frame, int rigIndex)
        {
            RigFrame<T> rigFrame = frameToRig[frame];
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                return rigFrame.joints[rigIndex];
            }
            return new Dictionary<int, T>();
        }

        public List<Tuple<T, T>> getRigBones(int frame, int rigIndex)
        {
            RigFrame<T> rigFrame = frameToRig[frame];
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                var joints = rigFrame.joints[rigIndex];
                var bones = new List<Tuple<T, T>>();
                foreach (Point boneScheme in rigScheme.bones)
                {
                    var bone = new Tuple<T, T>(joints[boneScheme.X], joints[boneScheme.Y]);
                    bones.Add(bone);
                }

                return bones;
            }
            return new List<Tuple<T, T>>();
        }

        public RigFigure<T> getRigFigure ( int frame, int rigIndex)
        {
            return new RigFigure<T>(getRigJoints(frame, rigIndex), getRigBones(frame, rigIndex));
        }

        public Dictionary<int, Object> generateObjects ( String videoFile )
        {
            var rigObjects = new Dictionary<int, Object>();

            foreach ( int frame in frameToRig.Keys)
            {
                RigFrame<T> rf = frameToRig[frame];
                foreach ( int rigIndex in rf.joints.Keys)
                {
                    if ( !rigObjects.ContainsKey(rigIndex)      )
                    {
                        rigObjects [rigIndex] = new Object("", Color.Green, 1, videoFile);
                        rigObjects[rigIndex].otherProperties["rigIndex"] = "" + rigIndex;
                        rigObjects[rigIndex].genType = Object.GenType.PROVIDED;
                        rigObjects[rigIndex].semanticType = "bodyRig";
                        rigObjects[rigIndex].otherProperties["source"] = rigFile;
                        rigObjects[rigIndex].otherProperties["sourceScheme"] = rigSchemeFile;
                    }

                    if (typeof(T) == typeof(Point))
                    {
                        rigObjects[rigIndex].setBounding(frame, (RigFigure<Point>) (object) getRigFigure(frame, rigIndex), 1, new Point());
                    }
                }
            }

            return rigObjects;
        }

        public static void loadDataForRig(String rigFile, String rigSchemeFile, int rigIndex, Object o)
        {
            var rigs = Rigs<Point>.getRigFromSource(rigFile, rigSchemeFile);

            foreach (int frame in rigs.frameToRig.Keys)
            {
                RigFrame <Point> rf = rigs.frameToRig[frame];

                if (rf.joints.ContainsKey(rigIndex) )
                {
                    o.setBounding(frame, (RigFigure<Point>)(object)rigs.getRigFigure(frame, rigIndex), 1, new Point());
                }
            }
        }
    }

    public class RigScheme
    {
        // From numeric joint to name and meaning
        public Dictionary<int, Tuple< String, String> > jointToJointName
        {
            get; set;
        }

        // Which joint link to which
        public List<Point> bones { get; set; }

        public static Dictionary<int, Tuple<String, String>> kinectV2jointToJointName = new Dictionary<int, Tuple<String, String>> {
                { 0, new Tuple<string, string>("SpineBase", "Base of the spine") },
                { 1, new Tuple<string, string>("SpineMid", "Middle of the spine") },
                { 2, new Tuple<string, string>("Neck", "Neck") },
                { 3, new Tuple<string, string>("Head", "Head") },
                { 4, new Tuple<string, string>("ShoulderLeft", "Left shoulder") },
                { 5, new Tuple<string, string>("ElbowLeft", "Left elbow") },
                { 6, new Tuple<string, string>("WristLeft", "Left wrist") },
                { 7, new Tuple<string, string>("HandLeft", "Left hand") },
                { 8, new Tuple<string, string>("ShoulderRight", "Right shoulder") },
                { 9, new Tuple<string, string>("ElbowRight", "Right elbow") },
                { 10, new Tuple<string, string>("WristRight", "Right wrist") },
                { 11, new Tuple<string, string>("HandRight", "Right hand") },
                { 12, new Tuple<string, string>("HipLeft", "Left hip") },
                { 13, new Tuple<string, string>("KneeLeft", "Left knee") },
                { 14, new Tuple<string, string>("AnkleLeft", "Left ankle") },
                { 15, new Tuple<string, string>("FootLeft", "Left foot") },
                { 16, new Tuple<string, string>("HipRight", "Right hip") },
                { 17, new Tuple<string, string>("KneeRight", "Right knee") },
                { 18, new Tuple<string, string>("AnkleRight", "Right ankle") },
                { 19, new Tuple<string, string>("FootRight", "Right foot") },
                { 20, new Tuple<string, string>("SpineShoulder", "Spine at the shoulder") },
                { 21, new Tuple<string, string>("HandTipLeft", "Tip of the left hand") },
                { 22, new Tuple<string, string>("ThumbLeft", "Left thumb") },
                { 23, new Tuple<string, string>("HandTipRight", "Tip of the right hand") },
                { 24, new Tuple<string, string>("ThumbRight", "Right thumb") }
            };

        public static List<Point> kinectv2bones = new List<Point> {
                new Point(0, 1), new Point(1,20), new Point(20, 2), new Point(2,3),  // Torso
                new Point(20, 4),new Point(4,5),new Point(5,6),new Point(6,7),new Point(7,21),new Point(7,22), // Left hand
                new Point(20, 8),new Point(8,9),new Point(9,10),new Point(10,11),new Point(11,23),new Point(11,24), // Right hand
                new Point(0, 12),new Point(12, 13),new Point(13, 14),new Point(14, 15), // Left leg
                new Point(0, 16),new Point(16, 17),new Point(17, 18),new Point(18, 19), // Right leg
            };

        ///<summary>
        ///Input type of body data stream, currently the only 
        ///supported format is xml, but it would be extended to json
        ///</summary>
        public string inputType { get; set; } = "";

        ///<summary>
        ///Xml tag name that cover the whole sequence of frames
        ///</summary>
        public string sequenceRoot { get; set; } = "";

        ///<summary>
        ///For each frame, all frame infomation is stored inside this node
        ///</summary>
        public string frameRoot { get; set; } = "";

        ///<summary>
        ///The relative Xpath to all rig points
        ///</summary>
        public string rigPointsPath { get; set; } = "";

        ///<summary>
        ///The relative Xpath to get the frame number
        ///</summary>
        public string frameNoPath { get; set; } = "";

        /// <summary>
        ///The relative Xpath to get rig index
        /// </summary>
        public string rigNoPath { get; set; } = "";

        ///<summary>
        ///Format of rigType could be xml or flat
        ///If it is flat, all rig points will be laid out as values separated by 'rigPointFormatSeparated' at rigPointsPath
        ///If it is node, each rig point is inside its own tag that has tagName = 'rigPointTag'
        ///</summary>
        public string rigPointFormatType { get; set; } = "";

        /// <summary>
        /// rigPointFormatSeparated accompanying rigPointFormatType='flat'
        /// </summary>
        public string rigPointFormatSeparated { get; set; } = "";

        /// <summary>
        /// rigPointFormatSeparated accompanying rigPointFormatType='node'
        /// </summary>
        public string rigPointPath { get; set; } = "";

        /*
        <rigPointInput>
		    <inputType>xml</inputType>
		    <sequenceRoot>BodyDataSequence</sequenceRoot>
		    <frameRoot>Body_Data</frameRoot>
		    <rigPointsPath>Skeleton_ImgPlane_Joint_Locations/Pts</rigPointsPath>
		    <frameNoPath> 
			    Timestamp@frame
		    </frameNoPath>
		    <rigNoPath>Subject@id</rigNoPath>      
		    <rigPointFormat type = "flat" separated=","/> // All rig points are laid out as either comma separated or space separated values
            <rigPoints>    
                <rigPoint name="WristRight" id="10" description="Right wrist"/>
                    ...
		    </rigPoints>
	    </rigPointInput>
        */
        public static RigScheme readFromXml(XmlNode xmlNode)
        {
            try
            {
                XmlNode rigPointInput = xmlNode.SelectSingleNode(".//rigPointInput");
                string inputType = rigPointInput.SelectNodes("inputType").Item(0).InnerText;
                string sequenceRoot = rigPointInput.SelectNodes("sequenceRoot").Item(0).InnerText;
                string frameRoot = rigPointInput.SelectNodes("frameRoot").Item(0).InnerText;
                string rigPointsPath = rigPointInput.SelectNodes("rigPointsPath").Item(0).InnerText;
                string frameNoPath = rigPointInput.SelectNodes("frameNoPath").Item(0).InnerText;
                string rigNoPath = rigPointInput.SelectNodes("rigNoPath").Item(0).InnerText;
                string rigPointFormatType = rigPointInput.SelectNodes("rigPointFormat").Item(0).Attributes["type"].Value;
                string rigPointFormatSeparated = "";
                if (rigPointFormatType == "flat")
                    rigPointFormatSeparated = rigPointInput.SelectNodes("rigPointFormat").Item(0).Attributes["separated"].Value;

                string rigPointPath = "";
                if (rigPointFormatType == "node")
                    rigPointPath = rigPointInput.SelectNodes("rigPointFormat").Item(0).Attributes["path"].Value;

                var jointToJointName = new Dictionary<int, Tuple<String, String>>();
                foreach (XmlNode rigPoint in rigPointInput.SelectNodes(".//rigPoint"))
                {
                    string name = rigPoint.Attributes["name"].Value;
                    string id = rigPoint.Attributes["id"].Value;
                    string description = rigPoint.Attributes["description"].Value;
                    jointToJointName[int.Parse(id)] = new Tuple<string, string>(name, description);
                }

                var bones = new List<Point>();
                foreach (XmlNode rigBone in xmlNode.SelectNodes(".//rigBone"))
                {
                    bones.Add(new Point(int.Parse(rigBone.Attributes["from"].Value), int.Parse(rigBone.Attributes["to"].Value)));
                }

                return new RigScheme()
                {
                    jointToJointName = jointToJointName,
                    bones = bones,
                    inputType = inputType,
                    sequenceRoot = sequenceRoot,
                    frameRoot = frameRoot,
                    rigPointsPath = rigPointsPath,
                    frameNoPath = frameNoPath,
                    rigNoPath = rigNoPath,
                    rigPointFormatType = rigPointFormatType,
                    rigPointFormatSeparated = rigPointFormatSeparated,
                    rigPointPath = rigPointPath
                };
            }
            catch (NullReferenceException e)
            {
                System.Windows.Forms.MessageBox.Show("Input rig scheme file has problem " + e);
            }
            return null;
        }
    }

    public class RigFrame <T>
    {
        /// <summary>
        /// Rigs(s) for each frame. 
        /// There might be multiple rigs, the first integer key is rig index (as tracked by Kinect API)
        /// The second integer key is joint id
        /// T is the location of joint (could be 2d or 3d)
        /// </summary>
        public Dictionary<int, Dictionary<int , T> > joints { get; }

        public RigFrame ()
        {
            joints = new Dictionary<int, Dictionary<int, T>>();
        }

        public void addRigFrame(int rigIndex, Dictionary<int, T> rig)
        {
            joints[rigIndex] = rig;
        }

    }
}
