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
    class GlyphBoxLocationMark3D : LocationMark3D
    {
        private const string FACE = "face";
        private const string BOUNDING = "bounding";
        private const string CODE = "code";
        private const string GLYPH_SIZE = "glyphSize";

        public List<List<Point3>> boundingPolygons { get; private set; }
        public List<GlyphFace> faces { get; private set; }
        public int glyphSize { get; private set; }

        public GlyphBoxLocationMark3D(int frameNo) : base(frameNo)
        {
            this.boundingPolygons = new List<List<Point3>>();
            this.faces = new List<GlyphFace>();
            this.glyphSize = 0;
        }

        public GlyphBoxLocationMark3D(int frameNo, int glyphSize, List<List<Point3>> boundingPolygons, List<GlyphFace> faces) : base(frameNo)
        {
            this.boundingPolygons = boundingPolygons;
            this.faces = faces;
            this.glyphSize = glyphSize;
        }

        private void Sort()
        {
            var tempoBoundingPolygons = new List<List<Point3>>();

            for (int i = 0; i < boundingPolygons.Count; i++)
            {
                List<Point3> smallestXplusYs = boundingPolygons[i].minVal(p => p.X + p.Y);
                Point3 select = smallestXplusYs.Where(point => point.X == smallestXplusYs.Min(p => p.X)).First();
                var index = boundingPolygons[i].FindIndex(point => point.Equals(select));

                if (index >= 0)
                {
                    var t1 = new List<Point3>();

                    t1.AddRange(boundingPolygons[i].GetRange(index, boundingPolygons[i].Count - index));
                    t1.AddRange(boundingPolygons[i].GetRange(0, index));

                    tempoBoundingPolygons.Add(t1);
                }
            }
            boundingPolygons = (List<List<Point3>>)(object)tempoBoundingPolygons;
        }


        /// <summary>
        /// Should be from a GlyphBoxLocationMark<Point3> to a GlyphBoxLocationMark<PointF>
        /// so that the result can be rendered to a graphics 
        /// </summary>
        /// <param name="scale"> size of image board/ size of depth field view</param>
        /// <param name="translation"> alignment offset from the top left corner of image board to where to depth image is rendered</param>
        /// <returns></returns>
        public override LocationMark2D getDepthViewLocationMark(float scale, PointF translation)
        {
            var boundingPolygonInDepthPixels = boundingPolygons.Select(boundingPolygon => boundingPolygon.Select(p => KinectUtils.projectCameraSpacePointToDepthPixel(p)));
            // Point3 -> PointF
            var flattenBoundingPolygonInDepthPixels = boundingPolygonInDepthPixels.Select(boundingPolygon => boundingPolygon.Select(p => new PointF(p.X, p.Y)).ToList());

            var scaledBoundingPolygons = flattenBoundingPolygonInDepthPixels.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

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

                xmlWriter.WriteString(string.Join(",", (boundingPolygons[i]).ConvertAll(p => p.X + "," + p.Y + "," + p.Z)));
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
                    var points = new List<Point3>();
                    if (parts.Length % 3 == 0)
                    {
                        for (int i = 0; i < parts.Length / 3; i++)
                        {
                            Point3 p = new Point3(float.Parse(parts[3 * i].Trim()), float.Parse(parts[3 * i + 1].Trim()), float.Parse(parts[3 * i + 2].Trim()));
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

        public override LocationMark3D getScaledLocationMark(float scale, Point3 translation)
        {
            var scaledBoundingPolygons = boundingPolygons.Select(boundingPolygon => boundingPolygon.scaleBound(scale, translation)).ToList();

            return new GlyphBoxLocationMark3D(frameNo, this.glyphSize, scaledBoundingPolygons, faces);
        }

        public override LocationMark3D addLocationMark(int resultFrameNo, LocationMark3D added)
        {
            var addedFaces = (added as GlyphBoxLocationMark3D).faces;

            if (faces.Count != addedFaces.Count)
                throw new ArgumentException(" Glyph box location mark to add needs to have the same number of faces!");

            for (int i = 0; i < faces.Count; i++)
            {
                if (!faces[i].Equals(addedFaces[i]))
                    throw new ArgumentException(" Glyph box location mark to add needs to have the same faces!");
            }

            if (added is GlyphBoxLocationMark3D)
            {
                var addedPolygons = (added as GlyphBoxLocationMark3D).boundingPolygons;
                var newBoundingPolygon = boundingPolygons.
                Zip(addedPolygons, (firstBound, secondBound) => firstBound.Zip(secondBound, (first, second) => first.scalePoint(1, second)).ToList()).ToList();
                return new GlyphBoxLocationMark3D(resultFrameNo, glyphSize, newBoundingPolygon, faces);
            }
            else
            {
                throw new ArgumentException(" Adding location mark needs to be of the same type !");
            }
        }
    }
}
