using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    // Rig over multiple frames
    public class Rig
    {
        RigScheme rigScheme { get; }
        Dictionary<int, RigFrame> frameToRig { get; }
        Func<Tuple<float, float, float>, Point> projectionMethod;

        public Rig(Dictionary<int, RigFrame> frameToRig, RigScheme rigSchem, Func<Tuple<float, float, float>, Point> projectionMethod)
        {
            this.rigScheme = rigScheme;
            this.frameToRig = frameToRig;
            this.projectionMethod = projectionMethod;
        }

        public static void readFromFile(String filename)
        {

        }

        public List<Point> getRigJoints(int frame, int rigIndex)
        {
            var rigJoints = new List<Point>();
            RigFrame rigFrame = frameToRig[frame];
            if (rigFrame != null && rigFrame.joints[rigIndex] != null)
            {
                var joints3D = rigFrame.joints[rigIndex];
                rigJoints = joints3D.Select(x => projectionMethod(x)).ToList();
            }
            return rigJoints;
        }

        public Dictionary<int, List<Tuple<Point, Point>>> getRigBones(int frame)
        {
            var rigBones = new Dictionary<int, List<Tuple<Point, Point>>>();
            RigFrame rigFrame = frameToRig[frame];
            if (rigFrame != null)
            {
                foreach (int rigIndex in rigFrame.joints.Keys)
                {
                    var joints3D = rigFrame.joints[rigIndex];
                    var bones2D = new List<Tuple<Point, Point>>();
                    foreach (Point boneScheme in rigScheme.bones)
                    {
                        var bone = new Tuple<Point, Point>(projectionMethod(joints3D[boneScheme.X]), 
                            projectionMethod(joints3D[boneScheme.Y]));
                        bones2D.Add(bone);
                    }
                    rigBones[rigIndex] = bones2D;
                }
            }
            return rigBones;
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

        public static RigScheme KinectV2Scheme = new RigScheme {
            jointToJointName = new Dictionary<int, Tuple<String, String>> {
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
            },
            bones = new List<Point> {
                new Point(0, 1), new Point(1,20), new Point(20, 2), new Point(2,3),  // Torso
                new Point(20, 4),new Point(4,5),new Point(5,6),new Point(6,7),new Point(7,21),new Point(7,22), // Left hand
                new Point(20, 8),new Point(8,9),new Point(9,10),new Point(10,11),new Point(11,23),new Point(11,24), // Right hand
                new Point(0, 12),new Point(12, 13),new Point(13, 14),new Point(14, 15), // Left leg
                new Point(0, 16),new Point(16, 17),new Point(17, 18),new Point(18, 19), // Right leg
            }
        };
    }

    public class RigFrame
    {
        public Dictionary<int, List<Tuple<float, float, float>>> joints { get; set; }

    }
}
