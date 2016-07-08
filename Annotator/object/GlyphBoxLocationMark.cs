using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    class PointFComparer : IComparer<PointF>
    {
        public int Compare(PointF x, PointF y)
        {
            if (x.X < y.X) return -1;
            if (x.X > y.X) return 1;
            if (x.Y < y.Y) return -1;
            if (x.Y > y.Y) return 1;
            return 0;

        }
    }
    class GlyphBoxLocationMark : DrawableLocationMark
    {
        private const string FACE = "face";
        private const string BOUNDING = "bounding";
        private const string BOUNDING3D = "bounding3d";
        private const string CODE = "code";
        private const string GLYPH_SIZE = "glyphSize";

        public List<List<PointF>> boundingPolygons { get; private set; }
        public List<List<Point3>> bounding3DPolygons { get; private set; }
        public List<GlyphFace> faces { get; private set; }
        public int glyphSize { get; private set; }

        public GlyphBoxLocationMark(int frameNo) : base(frameNo)
        {
            this.boundingPolygons = new List<List<PointF>>();
            bounding3DPolygons = new List<List<Point3>>();
            this.faces = new List<GlyphFace>();
            this.glyphSize = 0;
        }

        public GlyphBoxLocationMark(int frameNo, int glyphSize, List<List<PointF>> boundingPolygons, List<GlyphFace> faces, List<List<Point3>> bounding3DPolygons) : base(frameNo)
        {
            this.boundingPolygons = boundingPolygons;
            this.bounding3DPolygons = bounding3DPolygons;
            this.faces = faces;
            this.glyphSize = glyphSize;
        }

        /// <summary>
        /// Sort the boundingPolygons and bounding3DPolygons so that the order of polygons are consistent
        /// Polygon points are sort on x + y first, than x
        /// </summary>
        private void resort()
        {
            var tempoBoundingPolygons = new List<List<PointF>>();
            var tempoBounding3DPolygons = new List<List<Point3>>();

            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                IEnumerable<PointF> smallestXplusYs = boundingPolygons[i].Where(point => point.X + point.Y == boundingPolygons[i].Min(p => p.X + p.Y));
                PointF select = smallestXplusYs.Where(point => point.X == smallestXplusYs.Min(p => p.X)).First();
                var index = boundingPolygons[i].FindIndex(point => point.Equals(select));

                if (index >=0)
                {
                    var t1 = new List<PointF>();
                    var t2 = new List<Point3>();

                    t1.AddRange(boundingPolygons[i].GetRange(index, boundingPolygons[i].Count - index));
                    t2.AddRange(bounding3DPolygons[i].GetRange(index, boundingPolygons[i].Count - index));

                    t1.AddRange(boundingPolygons[i].GetRange(0, index));
                    t2.AddRange(bounding3DPolygons[i].GetRange(0, index));

                    tempoBoundingPolygons.Add(t1);
                    tempoBounding3DPolygons.Add(t2);
                }
            }
            boundingPolygons = tempoBoundingPolygons;
            bounding3DPolygons = tempoBounding3DPolygons;
        }

        public override float Score(Point testPoint)
        {
            List<PointF> convexHull = Utils.getConvexHull(boundingPolygons.SelectMany(i => i).ToList());

            if (!Utils.IsPointInPolygonL(convexHull, testPoint))
            {
                return 0;
            }

            float score = 0;
            // Average of reveresed distance to polygon points
            foreach (PointF p in convexHull)
            {
                score += (float)(1f / Math.Sqrt(Math.Pow(p.X - testPoint.X, 2) + Math.Pow(p.Y - testPoint.Y, 2) + 1));
            }
            score /= convexHull.Count;
            return score;
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            foreach (var boundingPolygon in boundingPolygons)
            {
                g.DrawPolygon(p, boundingPolygon.ToArray());
            }
        }

        public override List<Rectangle> getCornerSelectBoxes(int boxSize)
        {
            List<Rectangle> selectBoxes = new List<Rectangle>();
            foreach (var boundingPolygon in boundingPolygons)
            {
                foreach (var p in boundingPolygon)
                {
                    selectBoxes.Add(new Rectangle((int)(p.X - (boxSize - 1) / 2),
                            (int)(p.Y - (boxSize - 1) / 2), boxSize, boxSize));
                }
            }
            return selectBoxes;
        }

        public override DrawableLocationMark getScaledLocationMark(double scale, Point translation)
        {
            List<List<PointF>> scaledBoundingPolygons = boundingPolygons.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

            return new GlyphBoxLocationMark(frameNo, this.glyphSize, scaledBoundingPolygons, faces, this.bounding3DPolygons);
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            resort();
            xmlWriter.WriteAttributeString(GLYPH_SIZE, "" + glyphSize);
            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                xmlWriter.WriteStartElement(FACE);

                xmlWriter.WriteStartElement(BOUNDING);
                xmlWriter.WriteString(string.Join(",", boundingPolygons[i].ConvertAll(p => p.X + "," + p.Y)));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(BOUNDING3D);
                if (bounding3DPolygons[i] != null)
                {
                    xmlWriter.WriteString(string.Join(",", bounding3DPolygons[i].ConvertAll(p => p.X + "," + p.Y + "," + p.Z)));
                }
                
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(CODE);
                xmlWriter.WriteString(faces[i].ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            glyphSize = int.Parse(xmlNode.Attributes[GLYPH_SIZE].Value);
            foreach (XmlNode faceNode in xmlNode.SelectNodes(FACE))
            {
                // Get boundings
                var boundingNode = faceNode.SelectSingleNode(BOUNDING);
                if (boundingNode != null)
                {
                    String[] parts = boundingNode.InnerText.Split(',');
                    List<PointF> points = new List<PointF>();
                    if (parts.Length % 2 == 0)
                    {
                        for (int i = 0; i < parts.Length / 2; i++)
                        {
                            PointF p = new Point(int.Parse(parts[2 * i].Trim()), int.Parse(parts[2 * i + 1].Trim()));
                            points.Add(p);
                        }
                    }

                    boundingPolygons.Add(points);
                }

                // Get bounding 3d
                var boundingNode3d = faceNode.SelectSingleNode(BOUNDING3D);
                if (boundingNode3d != null)
                {
                    var parts = boundingNode3d.InnerText.Split(',');
                    var point3ds = new List<Point3>();
                    if (parts.Length % 3 == 0)
                    {
                        for (int i = 0; i < parts.Length / 3; i++)
                        {
                            Point3 p = new Point3(float.Parse(parts[3 * i].Trim()), float.Parse(parts[3 * i + 1].Trim()), float.Parse(parts[3 * i + 2].Trim()));
                            point3ds.Add(p);
                        }
                    }

                    bounding3DPolygons.Add(point3ds);
                }
                

                // Get face
                var glyphNode = faceNode.SelectSingleNode(CODE);
                if (glyphNode != null)
                {
                    var parts = glyphNode.InnerText.Split(',');
                    bool[,] glyphValues = new bool[glyphSize, glyphSize];
                    if (parts.Length == glyphSize * glyphSize)
                    {
                        for (int i = 0; i < glyphSize; i++)
                            for (int j = 0; j < glyphSize; j++)
                            {
                                glyphValues[i, j] = int.Parse(parts[i * glyphSize + j].Trim()) == 1;
                            }
                    }
                    GlyphFace gf = new GlyphFace(glyphValues, glyphSize);

                    faces.Add(gf);
                }
            }

        }
    }
}
