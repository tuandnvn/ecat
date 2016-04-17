using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class RigScheme
    {
        // From numeric joint to name and meaning
        public Dictionary<int, Tuple<String, String>> jointToJointName
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
        ///The relative Xpath to all 2-dimensional rig points (projected on RGB field)
        ///</summary>
        public string rigPointsPath { get; set; } = "";

        ///<summary>
        ///The relative Xpath to all 3-dimensional rig points (camera space)
        ///</summary>
        public string rigPoint3DsPath { get; set; } = "";

        ///<summary>
        ///The relative Xpath to get the frame number
        ///</summary>
        public string frameNoPath { get; set; } = "";

        ///<summary>
        ///The time the rig captured from the start of the session in miliseconds
        ///</summary>
        public string frameTimePath { get; set; } = "";

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
                string rigPoint3DsPath = rigPointInput.SelectNodes("rigPoint3DsPath").Item(0).InnerText;
                string frameNoPath = rigPointInput.SelectNodes("frameNoPath").Item(0).InnerText;
                string frameTimePath = rigPointInput.SelectNodes("frameTimePath").Item(0).InnerText;
                //// For session captured from CwC apparatus, there is no timeFromBegin
                //string timeFromBegin = "";
                //try
                //{
                //    timeFromBegin = rigPointInput.SelectNodes("frameTimePath").Item(0).InnerText;
                //}
                //catch (Exception e) { } 

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
                    rigPoint3DsPath = rigPoint3DsPath,
                    frameNoPath = frameNoPath,
                    frameTimePath = frameTimePath,
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

}
