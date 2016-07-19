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

    class GlyphBoxLocationMark<T> : DrawableLocationMark
    {
        private const string FACE = "face";
        private const string BOUNDING = "bounding";
        private const string CODE = "code";
        private const string GLYPH_SIZE = "glyphSize";

        public List<List<T>> boundingPolygons { get; private set; }
        public List<GlyphFace> faces { get; private set; }
        public int glyphSize { get; private set; }

        public GlyphBoxLocationMark(int frameNo) : base(frameNo)
        {
            this.boundingPolygons = new List<List<T>>();
            this.faces = new List<GlyphFace>();
            this.glyphSize = 0;
        }

        public GlyphBoxLocationMark(int frameNo, int glyphSize, List<List<T>> boundingPolygons, List<GlyphFace> faces) : base(frameNo)
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
            if (typeof(T) == typeof(PointF))
            {
                var tempoBoundingPolygons = new List<List<PointF>>();

                var castedBoundingPolygons = (List<List<PointF>>)(object)boundingPolygons;

                for (int i = 0; i < boundingPolygons.Count; i++)
                {
                    List<PointF> smallestXplusYs = castedBoundingPolygons[i].minVal(p => p.X + p.Y);
                    PointF select = smallestXplusYs.Where(point => point.X == smallestXplusYs.Min(p => p.X)).First();
                    var index = castedBoundingPolygons[i].FindIndex(point => point.Equals(select));

                    if (index >= 0)
                    {
                        var t1 = new List<PointF>();
                        //var t2 = new List<Point3>();

                        t1.AddRange(castedBoundingPolygons[i].GetRange(index, boundingPolygons[i].Count - index));

                        t1.AddRange(castedBoundingPolygons[i].GetRange(0, index));

                        tempoBoundingPolygons.Add(t1);
                    }
                }
                boundingPolygons = (List<List<T>>)(object)tempoBoundingPolygons;
            }

            // Sort 3d points should be in similar with 2d points,
            // Just need to sort X and Y, leave Z alone
            if (typeof(T) == typeof(Point3))
            {
                var tempoBoundingPolygons = new List<List<Point3>>();

                var castedBoundingPolygons = (List<List<Point3>>)(object)boundingPolygons;

                for (int i = 0; i < boundingPolygons.Count; i++)
                {
                    List<Point3> smallestXplusYs = castedBoundingPolygons[i].minVal(p => p.X + p.Y);
                    Point3 select = smallestXplusYs.Where(point => point.X == smallestXplusYs.Min(p => p.X)).First();
                    var index = castedBoundingPolygons[i].FindIndex(point => point.Equals(select));

                    if (index >= 0)
                    {
                        var t1 = new List<Point3>();
                        //var t2 = new List<Point3>();

                        t1.AddRange(castedBoundingPolygons[i].GetRange(index, boundingPolygons[i].Count - index));

                        t1.AddRange(castedBoundingPolygons[i].GetRange(0, index));

                        tempoBoundingPolygons.Add(t1);
                    }
                }
                boundingPolygons = (List<List<T>>)(object)tempoBoundingPolygons;
            }
        }

        public override float Score(Point testPoint)
        {
            if (typeof(T) == typeof(PointF))
            {
                List<PointF> convexHull = Utils.getConvexHull((List<PointF>)(object)boundingPolygons.SelectMany(i => i).ToList());

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


            return 0;
        }

        public override void drawOnGraphics(Graphics g, Pen p)
        {
            if (typeof(T) == typeof(PointF))
            {
                foreach (var boundingPolygon in boundingPolygons)
                {
                    g.DrawPolygon(p, (PointF[])(object)boundingPolygon.ToArray());
                }
            }
        }

        public override List<Rectangle> getCornerSelectBoxes(int boxSize)
        {
            List<Rectangle> selectBoxes = new List<Rectangle>();
            if (typeof(T) == typeof(PointF))
            {

                foreach (var boundingPolygon in boundingPolygons)
                {
                    foreach (var p in boundingPolygon)
                    {
                        var p_ = (PointF)(object)p;
                        selectBoxes.Add(new Rectangle((int)(p_.X - (boxSize - 1) / 2),
                                (int)(p_.Y - (boxSize - 1) / 2), boxSize, boxSize));
                    }
                }
            }

            return selectBoxes;
        }

        public override DrawableLocationMark getScaledLocationMark(float scale, Point translation)
        {
            if (typeof(T) == typeof(PointF))
            {
                var scaledBoundingPolygons = ((List<List<PointF>>)(object)boundingPolygons).Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

                return new GlyphBoxLocationMark<PointF>(frameNo, this.glyphSize, scaledBoundingPolygons, faces);
            }

            return null;
        }

        /// <summary>
        /// Should be from a GlyphBoxLocationMark<Point3> to a GlyphBoxLocationMark<PointF>
        /// so that the result can be rendered to a graphics 
        /// </summary>
        /// <param name="scale"> size of image board/ size of depth field view</param>
        /// <param name="translation"> alignment offset from the top left corner of image board to where to depth image is rendered</param>
        /// <returns></returns>
        public DrawableLocationMark getDepthViewLocationMark(float scale, Point translation)
        {
            if (typeof(T) == typeof(Point3))
            {
                var boundingPolygonInDepthPixels = ((List<List<Point3>>)(object)boundingPolygons).Select(boundingPolygon => boundingPolygon.Select(p => KinectUtils.projectCameraSpacePointToDepthPixel(p)));
                // Point3 -> PointF
                var flattenBoundingPolygonInDepthPixels = boundingPolygonInDepthPixels.Select(boundingPolygon => boundingPolygon.Select(p => new PointF(p.X, p.Y)).ToList());

                var scaledBoundingPolygons = flattenBoundingPolygonInDepthPixels.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

                return new GlyphBoxLocationMark<PointF>(frameNo, this.glyphSize, scaledBoundingPolygons, faces);
            }
            return null;
        }

        public override void writeToXml(XmlWriter xmlWriter)
        {
            Sort();
            xmlWriter.WriteAttributeString(GLYPH_SIZE, "" + glyphSize);
            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                xmlWriter.WriteStartElement(FACE);

                xmlWriter.WriteStartElement(BOUNDING);

                if (typeof(T) == typeof(PointF))
                {
                    xmlWriter.WriteString(string.Join(",", ((List<PointF>)(object)boundingPolygons[i]).ConvertAll(p => p.X + "," + p.Y)));
                }

                if (typeof(T) == typeof(Point3))
                {
                    xmlWriter.WriteString(string.Join(",", ((List<Point3>)(object)boundingPolygons[i]).ConvertAll(p => p.X + "," + p.Y + "," + p.Z)));
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

                if (typeof(T) == typeof(PointF))
                    if (boundingNode != null)
                    {
                        String[] parts = boundingNode.InnerText.Split(',');
                        var points = new List<PointF>();
                        if (parts.Length % 2 == 0)
                        {
                            for (int i = 0; i < parts.Length / 2; i++)
                            {
                                PointF p = new Point(int.Parse(parts[2 * i].Trim()), int.Parse(parts[2 * i + 1].Trim()));
                                points.Add(p);
                            }
                        }

                        boundingPolygons.Add((List<T>)(object)points);
                    }

                if (typeof(T) == typeof(Point3))
                    if (boundingNode != null)
                    {
                        var parts = boundingNode.InnerText.Split(',');
                        var points = new List<Point3>();
                        if (parts.Length % 3 == 0)
                        {
                            for (int i = 0; i < parts.Length / 3; i++)
                            {
                                Point3 p = new Point3(float.Parse(parts[3 * i].Trim()), float.Parse(parts[3 * i + 1].Trim()), float.Parse(parts[3 * i + 2].Trim()));
                                points.Add(p);
                            }
                        }

                        boundingPolygons.Add((List<T>)(object)points);
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
