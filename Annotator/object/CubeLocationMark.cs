using Accord.Math;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    class CubeLocationMark : LocationMark
    {
        private const string CENTER = "CENTER";
        private const string QUATERNIONS = "QUATERNIONS";

        public Point3 center { get; private set; }
        public Quaternions quaternion { get; private set; }

        public CubeLocationMark(int frameNo) : base(frameNo) { }

        public CubeLocationMark(int frameNo, Point3 center, Quaternions quaternion) : base(frameNo)
        {
            this.center = center;
            this.quaternion = quaternion;
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(CENTER);
            xmlWriter.WriteString(center.X + "," + center.Y + "," + center.Z);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement(QUATERNIONS);
            xmlWriter.WriteString(quaternion.W + "," + quaternion.X + "," + quaternion.Y + "," + quaternion.Z);
            xmlWriter.WriteEndElement();
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            XmlNode centerNode = xmlNode.SelectSingleNode(CENTER);
            String[] parts = centerNode.InnerText.Split(',');
            if (parts.Length == 3)
            {
                center = new Point3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
            }

            XmlNode quaternionsNode = xmlNode.SelectSingleNode(QUATERNIONS);
            parts = quaternionsNode.InnerText.Split(',');
            if (parts.Length ==  4)
            {
                quaternion = new Quaternions(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
            }
        }
    }
}
