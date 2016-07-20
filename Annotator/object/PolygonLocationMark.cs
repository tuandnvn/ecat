using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class PolygonLocationMark : LocationMark2D
    {
        public List<PointF> boundingPolygon { get; private set; }         // Object bounding polygon, if the tool to draw object is polygon tool

        public PolygonLocationMark(int frameNo, List<PointF> boundingPolygon) : base(frameNo)
        {
            this.boundingPolygon = boundingPolygon;
        }

        public override float Score(Point testPoint)
        {
            if (!Utils.IsPointInPolygonL(boundingPolygon, testPoint))
            {
                return 0;
            }

            float score = 0;
            // Average of reveresed distance to polygon points
            foreach (PointF p in boundingPolygon)
            {
                score += (float)(1f / Math.Sqrt(Math.Pow(p.X - testPoint.X, 2) + Math.Pow(p.Y - testPoint.Y, 2) + 1));
            }
            score /= boundingPolygon.Count;
            return score;
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            if (boundingPolygon.Count >= 2)
                g.DrawPolygon(p, boundingPolygon.ToArray());
        }

        public override LocationMark2D getScaledLocationMark(float scale, PointF translation)
        {
            return new PolygonLocationMark(frameNo, boundingPolygon.scaleBound(scale, translation));
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteString(string.Join(",", this.boundingPolygon.ConvertAll(p => p.X + "," + p.Y)));
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            String parameters = xmlNode.InnerText;
            String[] parts = parameters.Split(',');
            List<PointF> points = new List<PointF>();
            if (parts.Length % 2 == 0)
            {
                for (int i = 0; i < parts.Length / 2; i++)
                {
                    PointF p = new PointF(float.Parse(parts[2 * i].Trim()), float.Parse(parts[2 * i + 1].Trim()));
                    points.Add(p);
                }
            }

            boundingPolygon = points;
        }
    }
}
