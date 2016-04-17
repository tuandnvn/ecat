using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class RectangleLocationMark : LocationMark2D
    {
        public Rectangle boundingBox { get; private set;  }              //Object bounding box;

        public RectangleLocationMark(int frameNo, Rectangle boundingBox) : base(frameNo)
        {
            this.boundingBox = boundingBox;
        }

        public override float Score(Point testPoint)
        {
            if (!boundingBox.Contains(testPoint))
                return 0;
            float score = 0;
            foreach (Point p in new Point[] { new Point(boundingBox.Top, boundingBox.Left), new Point(boundingBox.Top, boundingBox.Right),
                    new Point(boundingBox.Bottom, boundingBox.Left), new Point(boundingBox.Bottom, boundingBox.Right) })
            {
                score += (float)(1f / Math.Sqrt(Math.Pow(p.X - testPoint.X, 2) + Math.Pow(p.Y - testPoint.Y, 2) + 1));
            }
            score /= 4;
            return score;
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            g.DrawRectangle(p, boundingBox);
        }

        public override LocationMark2D getScaledLocationMark(double scale, Point translation)
        {
            return new RectangleLocationMark(frameNo, boundingBox.scaleBound(scale, translation));
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteString(this.boundingBox.X + "," + this.boundingBox.Y + "," +
                                       this.boundingBox.Width + "," + this.boundingBox.Height);
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            String parameters = xmlNode.InnerText;
            String[] parts = parameters.Split(',');
            if (parts.Length == 4)
            {
                int x = int.Parse(parts[0].Trim());
                int y = int.Parse(parts[1].Trim());
                int width = int.Parse(parts[2].Trim());
                int height = int.Parse(parts[3].Trim());
                boundingBox = new Rectangle(x, y, width, height);
            }
        }
    }
}
