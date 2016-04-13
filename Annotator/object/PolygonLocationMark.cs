using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class PolygonLocationMark : LocationMark
    {
        public List<PointF> boundingPolygon { get; }         // Object bounding polygon, if the tool to draw object is polygon tool

        public PolygonLocationMark(int frameNo, LocationMarkType markType, List<PointF> boundingPolygon) : base(frameNo, markType)
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
            g.DrawPolygon(p, boundingPolygon.ToArray());
        }
    }
}
