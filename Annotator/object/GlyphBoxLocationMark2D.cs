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

    class GlyphBoxLocationMark2D : LocationMark2D
    {
        private const string FACE = "face";
        private const string BOUNDING = "bounding";
        private const string CODE = "code";
        private const string GLYPH_SIZE = "glyphSize";

        public List<List<PointF>> boundingPolygons { get; private set; }
        public List<GlyphFace> faces { get; private set; }
        public int glyphSize { get; private set; }

        public GlyphBoxLocationMark2D(int frameNo) : base(frameNo)
        {
            this.boundingPolygons = new List<List<PointF>>();
            this.faces = new List<GlyphFace>();
            this.glyphSize = 0;
        }

        public GlyphBoxLocationMark2D(int frameNo, int glyphSize, List<List<PointF>> boundingPolygons, List<GlyphFace> faces) : base(frameNo)
        {
            this.boundingPolygons = boundingPolygons;
            this.faces = faces;
            this.glyphSize = glyphSize;
        }

        /// <summary>
        /// Sort the boundingPolygons and bounding3DPolygons so that the order of polygons are consistent
        /// Polygon points are sort on x + y first, than x
        /// </summary>
        private void Sort()
        {
            var tempoBoundingPolygons = new List<List<PointF>>();

            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                List<PointF> smallestXplusYs = boundingPolygons[i].minVal(p => p.X + p.Y);
                PointF select = smallestXplusYs.Where(point => point.X == smallestXplusYs.Min(p => p.X)).First();
                var index = boundingPolygons[i].FindIndex(point => point.Equals(select));

                if (index >= 0)
                {
                    var t1 = new List<PointF>();

                    t1.AddRange(boundingPolygons[i].GetRange(index, boundingPolygons[i].Count - index));

                    t1.AddRange(boundingPolygons[i].GetRange(0, index));

                    tempoBoundingPolygons.Add(t1);
                }
            }
            boundingPolygons = (List<List<PointF>>)(object)tempoBoundingPolygons;
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
                g.DrawPolygon(p, (PointF[])(object)boundingPolygon.ToArray());
            }
        }

        public override List<RectangleF> getCornerSelectBoxes(int boxSize)
        {
            List<RectangleF> selectBoxes = new List<RectangleF>();
            foreach (var boundingPolygon in boundingPolygons)
            {
                foreach (var p in boundingPolygon)
                {
                    var p_ = (PointF)(object)p;
                    selectBoxes.Add(new Rectangle((int)(p_.X - (boxSize - 1) / 2),
                            (int)(p_.Y - (boxSize - 1) / 2), boxSize, boxSize));
                }
            }
            return selectBoxes;
        }

        public override LocationMark2D getScaledLocationMark(float scale, PointF translation)
        {
            var scaledBoundingPolygons = boundingPolygons.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

            return new GlyphBoxLocationMark2D(frameNo, this.glyphSize, scaledBoundingPolygons, faces);
        }


        public override void writeToXml(XmlWriter xmlWriter)
        {
            Sort();
            xmlWriter.WriteAttributeString(GLYPH_SIZE, "" + glyphSize);
            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                xmlWriter.WriteStartElement(FACE);

                xmlWriter.WriteStartElement(BOUNDING);

                xmlWriter.WriteString(string.Join(",", ((List<PointF>)(object)boundingPolygons[i]).ConvertAll(p => p.X + "," + p.Y)));
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
                    var points = new List<PointF>();
                    if (parts.Length % 2 == 0)
                    {
                        for (int i = 0; i < parts.Length / 2; i++)
                        {
                            PointF p = new PointF(float.Parse(parts[2 * i].Trim()), float.Parse(parts[2 * i + 1].Trim()));
                            points.Add(p);
                        }
                    }

                    boundingPolygons.Add(points);
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

        public override LocationMark2D addLocationMark(int resultFrameNo, LocationMark2D added)
        {
            var addedFaces = (added as GlyphBoxLocationMark2D).faces;

            if (faces.Count != addedFaces.Count)
                throw new ArgumentException(" Glyph box location mark to add needs to have the same number of faces!");

            for (int i = 0; i < faces.Count; i++)
            {
                if (!faces[i].Equals(addedFaces[i]))
                    throw new ArgumentException(" Glyph box location mark to add needs to have the same faces!");
            }

            if (added is GlyphBoxLocationMark2D)
            {
                var addedPolygons = (added as GlyphBoxLocationMark2D).boundingPolygons;
                var newBoundingPolygon = boundingPolygons.
                Zip(addedPolygons, (firstBound, secondBound) => firstBound.Zip(secondBound, (first, second) => first.scalePoint(1, second)).ToList()).ToList();
                return new GlyphBoxLocationMark2D(resultFrameNo, glyphSize, newBoundingPolygon, faces);
            }
            else
            {
                throw new ArgumentException(" Adding location mark needs to be of the same type !");
            }
        }
    }
}
