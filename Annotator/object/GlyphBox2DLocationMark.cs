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
    class GlyphBox2DLocationMark : LocationMark2D
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

        public GlyphBox2DLocationMark(int frameNo) : base(frameNo)
        {
            this.boundingPolygons = new List<List<PointF>>();
            bounding3DPolygons = new List<List<Point3>>();
            this.faces = new List<GlyphFace>();
            this.glyphSize = 0;
        }

        public GlyphBox2DLocationMark(int frameNo, int glyphSize, List<List<PointF>> boundingPolygons, List<GlyphFace> faces, List<List<Point3>> bounding3DPolygons) : base(frameNo)
        {
            this.boundingPolygons = boundingPolygons;
            this.bounding3DPolygons = bounding3DPolygons;
            this.faces = faces;
            this.glyphSize = glyphSize;
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
            Console.WriteLine("Draw on Graphics of GlyphBox2DLocationMark");
            foreach (var boundingPolygon in boundingPolygons)
            {
                Console.WriteLine(string.Join(",", boundingPolygon));
                g.DrawPolygon(p, boundingPolygon.ToArray());
            }
        }

        public override LocationMark2D getScaledLocationMark(double scale, Point translation)
        {
            List<List<PointF>> scaledBoundingPolygons = boundingPolygons.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

            return new GlyphBox2DLocationMark(frameNo, this.glyphSize, scaledBoundingPolygons, faces, this.bounding3DPolygons);
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteAttributeString(GLYPH_SIZE, "" + glyphSize);
            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                xmlWriter.WriteStartElement(FACE);

                xmlWriter.WriteStartElement(BOUNDING);
                xmlWriter.WriteString(string.Join(",", boundingPolygons[i].ConvertAll(p => p.X + "," + p.Y)));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(BOUNDING3D);
                xmlWriter.WriteString(string.Join(",", bounding3DPolygons[i].ConvertAll(p => p.X + "," + p.Y + "," + p.Z)));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(CODE);
                xmlWriter.WriteString(faces[i].ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
        }

        public override void readFromXml(XmlNode xmlNode)
        {
            int glyphSize = int.Parse(xmlNode.Attributes[GLYPH_SIZE].Value);
            foreach (XmlNode faceNode in xmlNode.SelectNodes(FACE))
            {
                // Get boundings
                var boundingNode = faceNode.SelectSingleNode(BOUNDING);
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

                // Get bounding 3d
                var boundingNode3d = faceNode.SelectSingleNode(BOUNDING3D);
                parts = boundingNode3d.InnerText.Split(',');
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

                // Get face
                var glyphNode = faceNode.SelectSingleNode(CODE);
                parts = glyphNode.InnerText.Split(',');
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
