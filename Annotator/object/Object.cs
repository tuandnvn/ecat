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
    // 2d object
    public class Object
    {
        internal const string OBJECT = "object";
        internal const string ID = "id";
        internal const string NAME = "name";
        internal const string FILENAME = "filename";
        internal const string OBJECT_TYPE = "objectType";
        internal const string GENERATE = "generate";
        internal const string SEMANTIC_TYPE = "semanticType";
        internal const string MARKERS = "markers";
        internal const string MARKERS3D = "markers3d";
        internal const string MARKER = "marker";
        internal const string TYPE = "type";
        internal const string FRAME = "frame";
        internal const string SHAPE = "shape";
        internal const string BORDER_SIZE = "borderSize";
        internal const string COLOR = "color";
        internal const string LINKS = "links";
        internal const string SPATIAL_LINK = "spatialLink";
        internal const string QUALIFIED = "q";
        internal const string LINKTO = "linkTo";

        public string videoFile { get; }

        public SortedList<int, LocationMark2D> objectMarks
        {
            get; private set;
        }

        public SortedList<int, LocationMark3D> object3DMarks
        {
            get; protected set;
        }

        public SortedList<int, SpatialLinkMark> spatialLinkMarks
        {
            get; private set;
        }

        public enum ObjectType
        {
            _2D,
            _3D
        }

        public enum GenType
        {
            MANUAL,
            PROVIDED,
            TRACKED
        }

        public string id { get; set; }                //Object's ID
        public string name { get; set; }           // Object's name
        public Color color { get; set; }           //Object's boudnign box color
        public string semanticType { get; set; }           //Object's type
        public int borderSize { get; set; }        //Object bounding box border size
        public double scale { get; set; }          //Object scale
        public Dictionary<string, string> otherProperties { get; }
        public GenType genType { get; set; }
        public ObjectType objectType { get; set; }
        public Session session { get; set; }

        /// <summary>
        /// Create an object that is manually drawn on the paintBoard
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <param name="borderSize"></param>
        /// <param name="videoFile"></param>
        public Object(Session session, String id, Color color, int borderSize, string videoFile)
        {
            this.session = session;
            this.id = id;
            this.color = color;
            this.borderSize = borderSize;
            this.videoFile = videoFile;
            this.genType = GenType.MANUAL;
            this.objectType = ObjectType._2D;
            this.otherProperties = new Dictionary<string, string>();
            this.objectMarks = new SortedList<int, LocationMark2D>();
            this.object3DMarks = null;
            this.spatialLinkMarks = new SortedList<int, SpatialLinkMark>();
        }

        public void setBounding(int frameNumber, LocationMark2D locationMark)
        {
            objectMarks[frameNumber] = locationMark;
        }

        public void set3DBounding(int frameNumber, LocationMark3D locationMark)
        {
            if (this.object3DMarks == null)
            {
                this.object3DMarks = new SortedList<int, LocationMark3D>();
            }
            object3DMarks[frameNumber] = locationMark;
        }

        public void setCopyBounding(int frameNumber)
        {
            int first = objectMarks.Keys.FirstOrDefault(x => x > frameNumber);

            if (first > 0)
            {
                if (objectMarks[first].GetType() == typeof(RectangleLocationMark))
                {
                    var ob = new RectangleLocationMark(frameNumber, ((RectangleLocationMark)objectMarks[first]).boundingBox);

                    objectMarks[frameNumber] = ob;
                }

                if (objectMarks[first].GetType() == typeof(PolygonLocationMark2D))
                {
                    var ob = new PolygonLocationMark2D(frameNumber, ((PolygonLocationMark2D)objectMarks[first]).boundingPolygon);

                    objectMarks[frameNumber] = ob;
                }
            }
        }

        public void delete(int frameNumber)
        {
            var ob = new DeleteLocationMark(frameNumber);
            objectMarks[frameNumber] = ob;

            // Delete the next Delete marker
            int first = objectMarks.Keys.FirstOrDefault(x => x > frameNumber);
            if (first > 0 && objectMarks[first].GetType() == typeof(DeleteLocationMark))
            {
                objectMarks.Remove(first);
            }
        }

        public void deleteMarker(int frameNumber)
        {
            objectMarks.Remove(frameNumber);
        }

        public bool hasMarkerAt(int frameNumber)
        {
            return (objectMarks.ContainsKey(frameNumber) && objectMarks[frameNumber] != null);
        }

        public void setSpatialLink(int frameNumber, string objectId, bool qualified, SpatialLinkMark.SpatialLinkType linkType)
        {
            if (!spatialLinkMarks.ContainsKey(frameNumber))
            {
                spatialLinkMarks[frameNumber] = new SpatialLinkMark(frameNumber);
            }

            spatialLinkMarks[frameNumber].addLinkToObject(objectId, qualified, linkType);
        }

        /// <summary>
        /// Linear transformation of bounding to draw into paintBoard.
        /// 
        /// </summary>
        /// <param name="frameNo"> Frame number </param>
        /// <param name="scale"> 
        ///       Scale of paintBoardSize / videoSize
        ///       Aspect ratio remains the same
        /// </param>
        /// <param name="translation">
        ///       Offset between the resized frame and the paintBoard
        /// </param>
        /// <returns></returns>
        public LocationMark2D getScaledLocationMark(int frameNo, float scale, PointF translation)
        {
            int prevMarker = objectMarks.Keys.LastOrDefault(x => x <= frameNo);
            int nextMarker = objectMarks.Keys.FirstOrDefault(x => x >= frameNo);

            if (prevMarker == 0 && !objectMarks.ContainsKey(0))
            {
                return null;
            }

            // No marker to the right
            if (nextMarker == 0)
            {
                return objectMarks[prevMarker].getScaledLocationMark(scale, translation);
            }

            // Interpolation
            if (prevMarker != nextMarker && objectMarks[prevMarker] != null && objectMarks[nextMarker] != null)
            {
                if ( this is RectangleObject && Options.getOption().interpolationModes[Options.RECTANGLE] == Options.InterpolationMode.LINEAR ||
                    this is GlyphBoxObject && Options.getOption().interpolationModes[Options.GLYPH] == Options.InterpolationMode.LINEAR ||
                    this is RigObject && Options.getOption().interpolationModes[Options.RIG] == Options.InterpolationMode.LINEAR)
                {
                    return objectMarks[prevMarker].getScaledLocationMark((nextMarker - frameNo) * 1.0f / (nextMarker - prevMarker), new PointF())
                        .addLocationMark( frameNo,
                         objectMarks[nextMarker].getScaledLocationMark((frameNo - prevMarker) * 1.0f / (nextMarker - prevMarker), new PointF()))
                         .getScaledLocationMark( scale, translation )
                         ;
                }
                
            } 

            return objectMarks[prevMarker].getScaledLocationMark(scale, translation);
        }

        public LocationMark3D getLocationMark3D (int frameNo)
        {
            int prevMarker = object3DMarks.Keys.LastOrDefault(x => x <= frameNo);
            int nextMarker = object3DMarks.Keys.FirstOrDefault(x => x >= frameNo);

            if (prevMarker == 0 && !object3DMarks.ContainsKey(0))
            {
                return null;
            }

            // No marker to the right
            if (nextMarker == 0)
            {
                return object3DMarks[prevMarker];
            }

            // Interpolation
            if (prevMarker != nextMarker && object3DMarks[prevMarker] != null && object3DMarks[nextMarker] != null)
            {
                if (this is RectangleObject && Options.getOption().interpolationModes[Options.RECTANGLE] == Options.InterpolationMode.LINEAR ||
                    this is GlyphBoxObject && Options.getOption().interpolationModes[Options.GLYPH] == Options.InterpolationMode.LINEAR ||
                    this is RigObject && Options.getOption().interpolationModes[Options.RIG] == Options.InterpolationMode.LINEAR)
                {
                    return object3DMarks[prevMarker].getScaledLocationMark((nextMarker - frameNo) * 1.0f / (nextMarker - prevMarker), new Point3())
                        .addLocationMark(frameNo,
                         object3DMarks[nextMarker].getScaledLocationMark((frameNo - prevMarker) * 1.0f / (nextMarker - prevMarker), new Point3()))
                         ;
                }
            }

            return object3DMarks[prevMarker];
        }


        public void addProperty(string propertyKey, string propertyValue)
        {
            if (propertyKey != null && propertyValue != null)
                otherProperties[propertyKey] = propertyValue;
        }

        //<object refid = "o3" name="Apple 1" objectType="2D" generate="manual" semanticType="apple" filename="a.avi">
        //	<marker type = "Location" frame="1" shape="Rectangle"> 50, 50, 10, 10 </marker>
        //	<marker type = "Location" frame="50" shape="Rectangle"> 60, 60, 10, 10 </marker> <!-- Object moving -->
        //	<marker type = "Delete" frame="100"></marker>
        //	<tracking depthFile = "depth.avi" trackFile="apple_1_tracked.out"></tracking>
        //</object>
        public virtual void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(OBJECT);
            xmlWriter.WriteAttributeString(ID, "" + id);
            xmlWriter.WriteAttributeString(NAME, name);
            xmlWriter.WriteAttributeString(FILENAME, videoFile);
            xmlWriter.WriteAttributeString(OBJECT_TYPE, objectType.ToString().Substring(1));
            xmlWriter.WriteAttributeString(GENERATE, genType.ToString());
            xmlWriter.WriteAttributeString(COLOR, "" + color.ToArgb());
            xmlWriter.WriteAttributeString(BORDER_SIZE, "" + borderSize);
            xmlWriter.WriteAttributeString(SEMANTIC_TYPE, semanticType);
            xmlWriter.WriteAttributeString(SHAPE, this.GetType().ToString());

            foreach (String key in otherProperties.Keys)
            {
                xmlWriter.WriteAttributeString(key, otherProperties[key]);
            }

            writeLocationMark(xmlWriter);
            if (objectType == ObjectType._3D)
                write3DLocationMark(xmlWriter);
            writeLinks(xmlWriter);

            xmlWriter.WriteEndElement();
        }

        protected virtual void writeLocationMark(XmlWriter xmlWriter)
        {
            if (objectMarks.Count == 0) return;
            xmlWriter.WriteStartElement(MARKERS);
            foreach (int frame in objectMarks.Keys)
            {
                xmlWriter.WriteStartElement(MARKER);
                if (objectMarks[frame].GetType() != typeof(DeleteLocationMark))
                {
                    xmlWriter.WriteAttributeString(TYPE, "LOCATION");
                }
                else
                {
                    xmlWriter.WriteAttributeString(TYPE, "DELETE");
                }

                xmlWriter.WriteAttributeString(FRAME, "" + frame);
                if (objectMarks[frame].GetType() != typeof(DeleteLocationMark))
                {
                    objectMarks[frame].writeToXml(xmlWriter);
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        protected virtual void write3DLocationMark(XmlWriter xmlWriter)
        {
            // It's not 3d object
            if (object3DMarks == null) return;

            xmlWriter.WriteStartElement(MARKERS3D);
            foreach (int frame in object3DMarks.Keys)
            {
                xmlWriter.WriteStartElement(MARKER);

                xmlWriter.WriteAttributeString(FRAME, "" + frame);
                if (object3DMarks[frame].GetType() != typeof(DeleteLocationMark))
                {
                    object3DMarks[frame].writeToXml(xmlWriter);
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        protected virtual void writeLinks(XmlWriter xmlWriter)
        {
            if (spatialLinkMarks.Count == 0) return;
            xmlWriter.WriteStartElement(LINKS);
            foreach (int frame in spatialLinkMarks.Keys)
            {
                xmlWriter.WriteStartElement(SPATIAL_LINK);
                xmlWriter.WriteAttributeString(FRAME, "" + frame);
                foreach (Tuple<string, bool, SpatialLinkMark.SpatialLinkType> spatialLink in spatialLinkMarks[frame].spatialLinks)
                {
                    xmlWriter.WriteStartElement(LINKTO);
                    xmlWriter.WriteAttributeString(ID, spatialLink.Item1);
                    xmlWriter.WriteAttributeString(QUALIFIED, "" + spatialLink.Item2);
                    xmlWriter.WriteAttributeString(TYPE, spatialLink.Item3.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
        }

        public static List<Object> readFromXml(Session currentSession, XmlNode xmlNode)
        {
            List<Object> objects = new List<Object>();
            foreach (XmlNode objectNode in xmlNode.SelectNodes(OBJECT))
            {
                string id = objectNode.Attributes[ID].Value;
                string name = objectNode.Attributes[NAME].Value;
                string videoFile = objectNode.Attributes[FILENAME].Value;
                string objectType = objectNode.Attributes[OBJECT_TYPE].Value;
                string generate = objectNode.Attributes[GENERATE].Value;
                int borderSize = Int32.Parse(objectNode.Attributes[BORDER_SIZE].Value);
                Color color = Color.FromArgb(Int32.Parse(objectNode.Attributes[COLOR].Value));
                string shape = objectNode.Attributes[SHAPE].Value;
                String semanticType = objectNode.Attributes[SEMANTIC_TYPE].Value;

                Object o = null;

                try
                {
                    Type t = Type.GetType(shape);
                    if (t.IsSubclassOf(typeof(Object)))
                    {
                        o = (Object)Activator.CreateInstance(t, currentSession, id, color, borderSize, videoFile);
                    }
                    else
                    {
                        Console.WriteLine("One object could not be parsed. Id = " + id);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    if (shape == "Rectangle")
                    {
                        o = new RectangleObject(currentSession, id, color, borderSize, videoFile);
                    }
                    else if (shape == "Polygon")
                    {
                        o = new PolygonObject(currentSession, id, color, borderSize, videoFile);
                    }
                }



                o.name = name;
                o.semanticType = semanticType;
                o.genType = (GenType)Enum.Parse(typeof(GenType), generate.ToUpper());
                o.objectType = (ObjectType)Enum.Parse(typeof(ObjectType), "_" + objectType.ToUpper());

                foreach (XmlAttribute attr in objectNode.Attributes)
                {
                    if (!(new List<String> { ID, NAME, FILENAME, OBJECT_TYPE, GENERATE, BORDER_SIZE, COLOR, SEMANTIC_TYPE, SHAPE }).Contains(attr.Name))
                    {
                        o.addProperty(attr.Name, attr.Value);
                    }
                }

                readMarkers(objectNode, o);
                readLinks(objectNode, o);

                objects.Add(o);
            }

            int performerCount = 1;
            // Add performer names to RigObject
            foreach (var o in objects)
            {
                if (o is RigObject)
                {
                    o.name = "Performer " + performerCount++;
                }
            }

            return objects;
        }


        private static void readMarkers(XmlNode objectNode, Object o)
        {
            o.loadObjectAdditionalFromXml(objectNode);
        }

        // When getting back camera space point, if the value is set to -1, -1, -1
        // it means that this value is not initiated
        Point3 nullCameraSpacePoint = new Point3(-1, -1, -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthReader"></param>
        /// <param name="mappingHelper"></param>
        /// <returns> If the object is 3d-generated </returns>
        internal virtual bool generate3d(BaseDepthReader depthReader, DepthCoordinateMappingReader mappingHelper)
        {
            if (depthReader == null)
            {
                Console.WriteLine("depthReader is null ");
                return false;
            }

            if (mappingHelper == null)
            {
                Console.WriteLine("mappingHelper is null ");
                return false;
            }

            switch (this.genType)
            {
                case GenType.MANUAL:
                    var voxMLReader = VoxMLReader.getDefaultVoxMLReader();

                    var voxMLType = voxMLReader.getVoxMLType(this.semanticType);

                    string inform = "";
                    if (voxMLType.HasValue)
                    {
                        inform = "Object 3d will be generated based on VoxML semantic type " + voxMLType.Value.pred;
                    }
                    else
                    {
                        inform = "There is no corresponding VoxML semantic type. The boundary of objects will be projected directly onto 3d. Do you want to continue?";
                    }

                    var result = System.Windows.Forms.MessageBox.Show(inform, "Generate 3D", System.Windows.Forms.MessageBoxButtons.YesNo);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (var entry in objectMarks)
                        {
                            int frameNo = entry.Key;

                            // Mapping depth image
                            // At this point we use video frameNo
                            // It's actually just an approximation for the depth frameNo
                            Point3[,] colorSpaceToCameraSpacePoint = mappingHelper.projectDepthImageToColor(depthReader.readFrame(frameNo),
                                depthReader.getWidth(),
                                depthReader.getHeight(),
                                session.getVideo(videoFile).frameWidth,
                                session.getVideo(videoFile).frameHeight);

                            LocationMark objectMark = entry.Value;

                            List<PointF> boundary = new List<PointF>();
                            List<Point3> boundary3d = new List<Point3>();

                            if (this is RectangleObject)
                            {
                                var boundingBox = ((RectangleLocationMark)objectMark).boundingBox;
                                boundary.Add(new PointF(boundingBox.X, boundingBox.Y));
                                boundary.Add(new PointF(boundingBox.X + boundingBox.Width, boundingBox.Y));
                                boundary.Add(new PointF(boundingBox.X, boundingBox.Y + boundingBox.Height));
                                boundary.Add(new PointF(boundingBox.X + boundingBox.Width, boundingBox.Y + boundingBox.Height));
                            }
                            else if (this is PolygonObject)
                            {
                                boundary.AddRange(((PolygonLocationMark2D)objectMark).boundingPolygon);
                            }

                            // Using flat information if possible
                            if (voxMLType.HasValue && voxMLType.Value.concavity == "Flat")
                            {
                                // First algorithm : has completed
                                // Using the majority of points to infer the exact locations 
                                // of corner points

                                // Divide the diagonal between any two corners
                                // into noOfInner + 1 sub-segments
                                // int noOfInner = 2;

                                // Create a list of inner points for the surface
                                // by linear combination of corners
                                //List<PointF> innerPoints = new List<PointF>();

                                //for (int i = 0; i < boundary.Count; i++)
                                //    for (int j = i + 1; j < boundary.Count; j++) {
                                //        var start = boundary[i];
                                //        var end = boundary[j];

                                //        for ( int k = 0; k < noOfInner; k ++ )
                                //        {
                                //            var p = new PointF((start.X * (k + 1) + end.X * (noOfInner - k)) / (noOfInner + 1),
                                //                (start.Y * (k + 1) + end.Y * (noOfInner - k)) / (noOfInner + 1));

                                //            innerPoints.Add(p);
                                //        }
                                //    }

                                //// Add the original corner points
                                //innerPoints.AddRange(boundary);

                                int tryDivide = 10;
                                // Second algorithm, work only if one corner point is retrievable
                                PointF anchorCorner = new PointF();
                                Point3 anchorPoint = nullCameraSpacePoint;

                                foreach (PointF p in boundary)
                                {
                                    Point3 cameraSpacePoint = getCameraSpacePoint(colorSpaceToCameraSpacePoint, p);

                                    if (cameraSpacePoint != null && !cameraSpacePoint.Equals(nullCameraSpacePoint))
                                    {
                                        anchorCorner = p;
                                        anchorPoint = cameraSpacePoint;
                                        break;
                                    }
                                }

                                if (!anchorPoint.Equals(nullCameraSpacePoint))
                                {
                                    foreach (PointF corner in boundary)
                                    {
                                        Point3 cameraSpacePoint = getCameraSpacePoint(colorSpaceToCameraSpacePoint, corner);

                                        // If point p is out of the depth view 
                                        if (cameraSpacePoint.Equals(nullCameraSpacePoint))
                                        {
                                            var start = anchorCorner;
                                            var end = corner;
                                            var added = false;

                                            // For each value of k, try to get the point 
                                            // between anchorCorner and corner
                                            // that divide the segment into 1 andk k - 1
                                            for (int k = 2; k < tryDivide; k++)
                                            {
                                                var p = new PointF((start.X + end.X * (k - 1)) / k,
                                                   (start.Y + end.Y * (k - 1)) / k);

                                                Point3 middleCameraSpacePoint = getCameraSpacePoint(colorSpaceToCameraSpacePoint, p);

                                                if (middleCameraSpacePoint != null && !middleCameraSpacePoint.Equals(nullCameraSpacePoint))
                                                {
                                                    var inferred_p = new Point3()
                                                    {
                                                        X = anchorPoint.X + (middleCameraSpacePoint.X - anchorPoint.X) * k,
                                                        Y = anchorPoint.Y + (middleCameraSpacePoint.Y - anchorPoint.Y) * k,
                                                        Z = anchorPoint.Z + (middleCameraSpacePoint.Z - anchorPoint.Z) * k,
                                                    };
                                                    boundary3d.Add(inferred_p);
                                                    added = true;
                                                    break;
                                                }
                                            }

                                            // If that doesn't work
                                            if (!added)
                                            {
                                                boundary3d.Add(nullCameraSpacePoint);
                                            }
                                        }
                                        else
                                        {
                                            boundary3d.Add(cameraSpacePoint);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // Just mapping to 3d points
                                foreach (PointF p in boundary)
                                {
                                    Point3 cameraSpacePoint = getCameraSpacePoint(colorSpaceToCameraSpacePoint, p);

                                    if (cameraSpacePoint != null && !cameraSpacePoint.Equals(nullCameraSpacePoint))
                                    {
                                        boundary3d.Add(cameraSpacePoint);
                                    }
                                    else
                                    {
                                        boundary3d.Add(nullCameraSpacePoint);
                                    }
                                }
                            }

                            set3DBounding(frameNo, new PolygonLocationMark3D(frameNo, boundary3d));
                        }
                    }

                    break;
                default:
                    return false;
            }

            this.objectType = ObjectType._3D;
            return true;
        }

        private Point3 getCameraSpacePoint(Point3[,] colorSpaceToCameraSpacePoint, PointF p)
        {
            int x = (int)p.X;
            int y = (int)p.Y;

            Point3 cameraSpacePoint = nullCameraSpacePoint;
            foreach (var i in new int[] { 0, -1, 1, -2, 2 })
                foreach (var j in new int[] { 0, -1, 1, -2, 2 })
                {
                    if (x + i >= 0 && x + i < session.getVideo(videoFile).frameWidth &&
                        y + j >= 0 && y + j < session.getVideo(videoFile).frameHeight)
                    {
                        cameraSpacePoint = colorSpaceToCameraSpacePoint[x + i, y + j];
                        if (!cameraSpacePoint.Equals(nullCameraSpacePoint))
                        {
                            return cameraSpacePoint;
                        }
                    }
                }

            return cameraSpacePoint;
        }

        private static void readLinks(XmlNode objectNode, Object o)
        {
            XmlNode linksNode = objectNode.SelectSingleNode(LINKS);

            if (linksNode == null) return;
            foreach (XmlNode linkNode in linksNode.SelectNodes(SPATIAL_LINK))
            {
                int frame = int.Parse(linkNode.Attributes[FRAME].Value);
                foreach (XmlNode linkto in linkNode.SelectNodes(LINKTO))
                {
                    String linkToObjectId = linkto.Attributes[ID].Value;
                    bool qualified = bool.Parse(linkto.Attributes[QUALIFIED].Value);
                    SpatialLinkMark.SpatialLinkType markType = (SpatialLinkMark.SpatialLinkType)Enum.Parse(typeof(SpatialLinkMark.SpatialLinkType), linkto.Attributes[TYPE].Value, true);
                    o.setSpatialLink(frame, linkToObjectId, qualified, markType);
                }
            }
        }

        public String queryTooltip(int frameNo)
        {
            if (spatialLinkMarks.ContainsKey(frameNo))
            {
                return spatialLinkMarks[frameNo].ToString();
            }
            return "";
        }

        /// <summary>
        /// Loading the details of the object, possibly from outer files
        /// </summary>
        protected virtual void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
        }
    }
}
