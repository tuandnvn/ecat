using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RectangleLocationMark : LocationMark
    {
        public Rectangle boundingBox { get; }              //Object bounding box;

        public RectangleLocationMark(int frameNo, LocationMarkType markType, Rectangle boundingBox) : base(frameNo, markType)
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
    }
}
