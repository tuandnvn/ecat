﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    class GlyphBoxObject : Object
    {
        protected GlyphBoxPrototype boxPrototype;

        public GlyphBoxObject(Session currentSession, String id, Color color, int borderSize, string videoFile, GlyphBoxPrototype boxPrototype) : base(currentSession, id, color, borderSize, videoFile)
        {
            _borderType = BorderType.Others;
            this.boxPrototype = boxPrototype;
        }

        public void setBounding(int frameNumber, int glyphSize, List<List<PointF>> glyphBounds, List<GlyphFace> faces, double scale, Point translation)
        {
            List<List<PointF>> inversedScaledGlyphBounds = glyphBounds.Select(glyphBound => glyphBound.scaleBound(1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)))).ToList();
            LocationMark2D ob = new GlyphBox2DLocationMark(frameNumber, glyphSize, inversedScaledGlyphBounds, faces);
            objectMarks[frameNumber] = ob;
        }

        protected override void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
            XmlNode markersNode = objectNode.SelectSingleNode(MARKERS);
            foreach (XmlNode markerNode in markersNode.SelectNodes(MARKER))
            {
                int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                String markType = markerNode.Attributes[TYPE].Value;

                switch (markType)
                {
                    case "LOCATION":
                        var lm = new GlyphBox2DLocationMark(frame);
                        lm.readFromXml(markerNode);
                        setBounding(frame, lm);
                        break;
                    case "DELETE":
                        delete(frame);
                        break;
                }
            }

            XmlNode markers3DNodes = objectNode.SelectSingleNode(MARKERS3D);
            foreach (XmlNode markerNode in markers3DNodes.SelectNodes(MARKER))
            {
                int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                var lm = new CubeLocationMark(frame);
                lm.readFromXml(markerNode);
                set3DBounding(frame, lm);
            }
        }
    }
}
