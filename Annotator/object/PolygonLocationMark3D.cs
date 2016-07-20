using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class PolygonLocationMark3D : LocationMark3D
    {
        public List<Point3> boundingPolygon { get; private set; }         // Object bounding polygon, if the tool to draw object is polygon tool

        public PolygonLocationMark3D(int frameNo, List<Point3> boundingPolygon) : base(frameNo)
        {
            this.boundingPolygon = boundingPolygon;
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            String parameters = xmlNode.InnerText;
            String[] parts = parameters.Split(',');
            List<Point3> points = new List<Point3>();
            if (parts.Length % 3 == 0)
            {
                for (int i = 0; i < parts.Length / 3; i++)
                {
                    Point3 p = new Point3(float.Parse(parts[3 * i].Trim()), float.Parse(parts[3 * i + 1].Trim()), float.Parse(parts[3 * i + 2].Trim()));
                    points.Add(p);
                }
            }

            boundingPolygon = points;
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteString(string.Join(",", this.boundingPolygon.ConvertAll(p => p.X + "," + p.Y + "," + p.Z)));
        }
    }
}
