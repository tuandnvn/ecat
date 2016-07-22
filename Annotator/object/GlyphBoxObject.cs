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
    public class GlyphBoxObject : Object
    {
        public GlyphBoxPrototype _boxPrototype;
        public GlyphBoxPrototype boxPrototype
        {
            get { return _boxPrototype; }
            set
            {
                _boxPrototype = value;

                if (_boxPrototype != null)
                {
                    this.name = _boxPrototype.prototypeName;
                }
            }
        }

        public GlyphBoxObject(Session currentSession, String id, Color color, int borderSize, string videoFile) : base(currentSession, id, color, borderSize, videoFile)
        {
            this.object3DMarks = new SortedList<int, LocationMark3D>();
            this.objectType = ObjectType._3D;
            this.genType = GenType.TRACKED;
        }

        public void setBounding(int frameNumber, int glyphSize, List<List<PointF>> glyphBounds, List<GlyphFace> faces, float scale = 1, PointF translation = new PointF())
        {
            List<List<PointF>> inversedScaledGlyphBounds = glyphBounds.Select(glyphBound => glyphBound.scaleBound(1 / scale, new PointF(-translation.X / scale, -translation.Y / scale))).ToList();
            LocationMark2D ob = new GlyphBoxLocationMark2D(frameNumber, glyphSize, inversedScaledGlyphBounds, faces);
            objectMarks[frameNumber] = ob;
        }

        public void set3DBounding(int frameNumber, int glyphSize, List<List<Point3>> glyphBounds, List<GlyphFace> faces, float scale = 1, Point3 translation = new Point3())
        {
            List<List<Point3>> inversedScaledGlyphBounds = glyphBounds.Select(glyphBound => glyphBound.scaleBound(1 / scale, new Point3(-translation.X / scale, -translation.Y / scale, -translation.Z / scale))).ToList();
            LocationMark3D ob = new GlyphBoxLocationMark3D(frameNumber, glyphSize, inversedScaledGlyphBounds, faces);
            object3DMarks[frameNumber] = ob;
        }

        protected override void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
            XmlNode markersNode = objectNode.SelectSingleNode(MARKERS);
            if (markersNode != null)
                foreach (XmlNode markerNode in markersNode.SelectNodes(MARKER))
                {
                    int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                    String markType = markerNode.Attributes[TYPE].Value;

                    switch (markType)
                    {
                        case "LOCATION":
                            var lm = new GlyphBoxLocationMark2D(frame);
                            lm.readFromXml(markerNode);
                            setBounding(frame, lm);
                            break;
                        case "DELETE":
                            delete(frame);
                            break;
                    }
                }

            XmlNode markers3DNodes = objectNode.SelectSingleNode(MARKERS3D);

            if (markers3DNodes != null)
                foreach (XmlNode markerNode in markers3DNodes.SelectNodes(MARKER))
                {
                    int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                    var lm = new GlyphBoxLocationMark3D(frame);
                    lm.readFromXml(markerNode);
                    set3DBounding(frame, lm);
                }

            boxPrototype = getPrototype();
        }

        /// <summary>
        /// Get prototype by looking into GlyphBoxLocationMark
        /// Quickly return any matching prototype, without checking for consistency
        /// </summary>
        /// <returns></returns>
        private GlyphBoxPrototype getPrototype()
        {
            foreach (var prototype in Options.getOption().prototypeList)
            {
                foreach (GlyphBoxLocationMark2D objectMark in objectMarks.Values)
                {
                    foreach (var face in objectMark.faces)
                    {
                        if (prototype.indexToGlyphFaces.Values.ToList().Contains(face))
                        {
                            return prototype;
                        }
                    }
                }
            }
            return null;
        }
    }
}
