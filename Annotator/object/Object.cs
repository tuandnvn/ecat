﻿using Accord.Math;
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
        internal const string LINK = "link";
        internal const string QUALIFIED = "q";
        internal const string LINKTO = "linkTo";
        internal const string OTHER_SESSION = "otherSession";

        public string videoFile { get; }

        public SortedList<int, LocationMark2D> objectMarks
        {
            get; protected set;
        }

        public SortedList<int, LocationMark3D> object3DMarks
        {
            get; protected set;
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
        public string name { get; set; } = "";           // Object's name
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
            Console.WriteLine("Object constructor : SEssion = " + session);
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
            //this.linkMarks = new SortedList<int, LinkMark>();
            this.name = "";
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
            int nextMarker = objectMarks.Keys.FirstOrDefault(x => x > frameNumber);

            // Forward copy
            if (nextMarker > 0)
            {
                if (objectMarks[nextMarker].GetType() == typeof(RectangleLocationMark))
                {
                    var ob = new RectangleLocationMark(frameNumber, ((RectangleLocationMark)objectMarks[nextMarker]).boundingBox);

                    objectMarks[frameNumber] = ob;
                }

                if (objectMarks[nextMarker].GetType() == typeof(PolygonLocationMark2D))
                {
                    var ob = new PolygonLocationMark2D(frameNumber, ((PolygonLocationMark2D)objectMarks[nextMarker]).boundingPolygon);

                    objectMarks[frameNumber] = ob;
                }

                return;
            }

            // Backward copy
            try
            {
                int prevMarker = objectMarks.Keys.Last(x => x < frameNumber && objectMarks[x].GetType() != typeof(DeleteLocationMark));

                if (objectMarks[prevMarker].GetType() == typeof(RectangleLocationMark))
                {
                    var ob = new RectangleLocationMark(frameNumber, ((RectangleLocationMark)objectMarks[prevMarker]).boundingBox);

                    objectMarks[frameNumber] = ob;
                }

                if (objectMarks[prevMarker].GetType() == typeof(PolygonLocationMark2D))
                {
                    var ob = new PolygonLocationMark2D(frameNumber, ((PolygonLocationMark2D)objectMarks[prevMarker]).boundingPolygon);

                    objectMarks[frameNumber] = ob;
                }

                return;
            }
            catch (InvalidOperationException exc)
            {
                Console.WriteLine(exc);
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

        internal LinkMark getLink(int frameNumber)
        {
            var linkMarks = this.session.queryLinkMarks(this);
            if (linkMarks.ContainsKey(frameNumber))
            {
                return linkMarks[frameNumber];
            }
            return null;
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

            // Interpolation might fail
            try
            {
                // Interpolation
                if (prevMarker != nextMarker && objectMarks[prevMarker] != null && objectMarks[nextMarker] != null)
                {
                    if (this is RectangleObject && Options.getOption().interpolationModes[Options.RECTANGLE] == Options.InterpolationMode.LINEAR ||
                        this is GlyphBoxObject && Options.getOption().interpolationModes[Options.GLYPH] == Options.InterpolationMode.LINEAR ||
                        this is RigObject && Options.getOption().interpolationModes[Options.RIG] == Options.InterpolationMode.LINEAR)
                    {
                        var prev = objectMarks[prevMarker].getScaledLocationMark((nextMarker - frameNo) * 1.0f / (nextMarker - prevMarker), new PointF());
                        var next = objectMarks[nextMarker].getScaledLocationMark((frameNo - prevMarker) * 1.0f / (nextMarker - prevMarker), new PointF());
                        return prev.addLocationMark(frameNo, next).getScaledLocationMark(scale, translation);
                    }
                }
            }
            catch (Exception exc)
            {
            }


            return objectMarks[prevMarker].getScaledLocationMark(scale, translation);
        }

        public bool hasMark(int frameNo)
        {
            int prevMarker = objectMarks.Keys.LastOrDefault(x => x <= frameNo);

            if (prevMarker == 0)
            {
                if (objectMarks.ContainsKey(0))
                    return true;
                else
                    return false;
            }

            // The previous marker is not delete marker
            if (!(objectMarks[prevMarker] is DeleteLocationMark))
            {
                return true;
            }

            return false;
        }

        public LocationMark3D getLocationMark3D(int frameNo)
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
                    var prev = object3DMarks[prevMarker].getScaledLocationMark((nextMarker - frameNo) * 1.0f / (nextMarker - prevMarker), new Point3());
                    var next = object3DMarks[nextMarker].getScaledLocationMark((frameNo - prevMarker) * 1.0f / (nextMarker - prevMarker), new Point3());
                    return prev.addLocationMark(frameNo, next);
                }
            }

            return object3DMarks[prevMarker];
        }

        /// <summary>
        /// THe difference between this method and getLocationMark3D 
        /// is that it would return result even before the object has the first mark
        /// which would be useful for tracking objects
        /// </summary>
        /// <param name="frameNo"></param>
        /// <returns></returns>
        public LocationMark3D getLocationMark3DLeftExtrapolated(int frameNo)
        {
            try
            {
                int prevMarker = object3DMarks.Keys.LastOrDefault(x => x <= frameNo);
                int nextMarker = object3DMarks.Keys.FirstOrDefault(x => x >= frameNo);

                if (prevMarker == 0 && !object3DMarks.ContainsKey(0))
                {
                    return object3DMarks[nextMarker];
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
                        LocationMark3D prev = null;
                        LocationMark3D next = null;
                        prev = object3DMarks[prevMarker].getScaledLocationMark((nextMarker - frameNo) * 1.0f / (nextMarker - prevMarker), new Point3());
                        next = object3DMarks[nextMarker].getScaledLocationMark((frameNo - prevMarker) * 1.0f / (nextMarker - prevMarker), new Point3());
                        return prev.addLocationMark(frameNo, next);
                    }
                }

                return object3DMarks[prevMarker];
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
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
            //writeLinks(xmlWriter);

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

        public static List<Object> readObjectsFromXml(Session session, XmlNode xmlNode)
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
                        o = (Object)Activator.CreateInstance(t, session, id, color, borderSize, videoFile);
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
                        o = new RectangleObject(session, id, color, borderSize, videoFile);
                    }
                    else if (shape == "Polygon")
                    {
                        o = new PolygonObject(session, id, color, borderSize, videoFile);
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
                objects.Add(o);
            }

            //foreach (Object o in objects)
            //{
            //    session.addObject(o);
            //}

            //// Load predicates later, after adding all objects
            //foreach (XmlNode objectNode in xmlNode.SelectNodes(OBJECT))
            //{
            //    string id = objectNode.Attributes[ID].Value;

            //    var ob = objects.Where(o => o.id == id).First();
            //    if (ob != null)
            //        ob.readLinks(objectNode);
            //}

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

        /**
         * 
         * WHen generate 3d points for glyph, you only need to generate for frame or faces that don't yet have corresponding 3d data
         */
        internal virtual bool generate3dForGlyph(VideoReader videoReader, IDepthReader depthReader, Action<ushort[], Microsoft.Kinect.CameraSpacePoint[]> mappingFunction)
        {
            if (videoReader == null)
            {
                Console.WriteLine("videoReader is null ");
                return false;
            }

            if (depthReader == null)
            {
                Console.WriteLine("depthReader is null ");
                return false;
            }

            if (mappingFunction == null)
            {
                Console.WriteLine("mappingFunction is null ");
                return false;
            }

            var locationMarkers2dToProjected = new Dictionary<int, List<GlyphFace>>();

            foreach (var entry in objectMarks)
            {
                int frameNo = entry.Key;
                var faces2d = ((GlyphBoxLocationMark2D)entry.Value).faces;

                if (!object3DMarks.ContainsKey(frameNo))
                {
                    locationMarkers2dToProjected[frameNo] = faces2d;
                }
                else
                {
                    var faces3d = ((GlyphBoxLocationMark3D)object3DMarks[frameNo]).faces;
                    List<GlyphFace> toBeProjected = faces2d.Except(faces3d).ToList();

                    if (toBeProjected.Count != 0)
                    {
                        locationMarkers2dToProjected[frameNo] = toBeProjected;
                    }
                }
            }

            Console.WriteLine("locationMarkers2dToProjected");

            foreach (var key in locationMarkers2dToProjected.Keys)
            {
                Console.Write("Frame " + key + " : ");
                foreach (var item in locationMarkers2dToProjected[key])
                {
                    Console.Write("{0}, ", item);
                }
                Console.WriteLine();
            }

            foreach (var entry in locationMarkers2dToProjected)
            {
                try
                {
                    int frameNo = entry.Key;
                    var toBeProjected = entry.Value;

                    // Mapping depth image
                    // At this point we use video frameNo
                    // It's actually just an approximation for the depth frameNo
                    var csps = new Microsoft.Kinect.CameraSpacePoint[videoReader.frameWidth * videoReader.frameHeight];
                    int recordedTimeForRgbFrame = (int)(videoReader.totalMiliTime * frameNo / (videoReader.frameCount - 1));
                    Console.WriteLine("recordedTimeForRgbFrame " + recordedTimeForRgbFrame);
                    ushort[] depthValues = depthReader.readFrameAtTime(recordedTimeForRgbFrame);
                    mappingFunction(depthValues, csps);

                    GlyphBoxLocationMark2D objectMark = (GlyphBoxLocationMark2D)objectMarks[frameNo];


                    var boundingPolygons = objectMark.boundingPolygons;

                    var boundingPolygons3D = new List<List<Point3>>();
                    if (object3DMarks.ContainsKey(frameNo))
                    {
                        boundingPolygons3D = ((GlyphBoxLocationMark3D)object3DMarks[frameNo]).boundingPolygons;
                    }

                    for (int i = 0; i < objectMark.faces.Count; i++)
                    {
                        if (toBeProjected.Contains(objectMark.faces[i]))
                        {
                            var boundingPolygon = boundingPolygons[i];

                            List<Point3> boundingPolygon3D = new List<Point3>();
                            foreach (PointF p in boundingPolygon)
                            {
                                Point3 cameraSpacePoint = getCameraSpacePoint(p, videoReader, csps);

                                if (cameraSpacePoint != null && !cameraSpacePoint.Equals(nullCameraSpacePoint))
                                {
                                    boundingPolygon3D.Add(cameraSpacePoint);
                                }
                                else
                                {
                                    boundingPolygon3D.Add(nullCameraSpacePoint);
                                }
                            }
                            boundingPolygons3D.Insert(i, boundingPolygon3D);
                        }
                    }

                    foreach (var boundingPolygon3D in boundingPolygons3D)
                    {
                        Console.Write("Face : ");
                        foreach (var point in boundingPolygon3D)
                        {
                            Console.Write(point.X + ", " + point.Y + ", " + point.Z + "; ");
                        }
                        Console.WriteLine();
                    }

                    GlyphBoxLocationMark3D objectMark3D = new GlyphBoxLocationMark3D(frameNo, objectMark.glyphSize, boundingPolygons3D, objectMark.faces);

                    set3DBounding(frameNo, objectMark3D);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return true;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="depthReader"></param>
        /// <param name="mappingHelper"></param>
        /// <returns> If the object is 3d-generated </returns>
        internal virtual bool generate3d(VideoReader videoReader, IDepthReader depthReader, Action<ushort[], Microsoft.Kinect.CameraSpacePoint[]> mappingFunction)
        {
            if (videoReader == null)
            {
                Console.WriteLine("videoReader is null ");
                return false;
            }

            if (depthReader == null)
            {
                Console.WriteLine("depthReader is null ");
                return false;
            }

            if (mappingFunction == null)
            {
                Console.WriteLine("mappingFunction is null ");
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
                            var csps = new Microsoft.Kinect.CameraSpacePoint[videoReader.frameWidth * videoReader.frameHeight];

                            int recordedTimeForRgbFrame = (int)(videoReader.totalMiliTime * frameNo / (videoReader.frameCount - 1));
                            ushort[] depthValues = depthReader.readFrameAtTime(recordedTimeForRgbFrame);
                            mappingFunction(depthValues, csps);

                            //Point3[,] colorSpaceToCameraSpacePoint = mappingHelper.projectDepthImageToColor(depthReader.readFrame(frameNo),
                            //    depthReader.getWidth(),
                            //    depthReader.getHeight(),
                            //    session.getVideo(videoFile).frameWidth,
                            //    session.getVideo(videoFile).frameHeight);

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
                                    Point3 cameraSpacePoint = getCameraSpacePoint(p, videoReader, csps);

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
                                        Point3 cameraSpacePoint = getCameraSpacePoint(corner, videoReader, csps);

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

                                                Point3 middleCameraSpacePoint = getCameraSpacePoint(p, videoReader, csps);

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
                                    Point3 cameraSpacePoint = getCameraSpacePoint(p, videoReader, csps);

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

                            Console.Write("Polygon : ");
                            foreach (var point in boundary3d)
                            {
                                Console.Write(point.X + ", " + point.Y + ", " + point.Z + "; ");
                            }
                            Console.WriteLine();

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

        private Point3 getCameraSpacePoint(PointF p, VideoReader videoReader, Microsoft.Kinect.CameraSpacePoint[] csps)
        {
            int x = (int)p.X;
            int y = (int)p.Y;

            return x + y * videoReader.frameWidth >= 0 && x + y * videoReader.frameWidth < videoReader.frameWidth * videoReader.frameHeight ?
                                                                   new Point3(csps[x + y * videoReader.frameWidth].X,
                                                                   csps[x + y * videoReader.frameWidth].Y,
                                                                   csps[x + y * videoReader.frameWidth].Z) : new Point3();
        }

        public String queryTooltip(int frameNo)
        {
            var l = getLink(frameNo);
            if (l != null)
            {
                return l.ToString();
            }
            return "";
        }

        /// <summary>
        /// Loading the details of the object, possibly from outer files
        /// </summary>
        protected virtual void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
        }

        internal HashSet<PredicateMark> getHoldingPredicates(int frame)
        {
            HashSet<PredicateMark> holdingPredicates = new HashSet<PredicateMark>();
            SortedList<int, LinkMark> linkMarks = this.session.queryLinkMarks(this);

            foreach (int frameNo in linkMarks.Keys)
            {
                if (frameNo <= frame)
                    foreach (var predicateMark in linkMarks[frameNo].predicateMarks)
                    {
                        // Only add predicateMark if it is POSITIVE
                        // Otherwise remove its negation
                        if (predicateMark.qualified)
                        {
                            holdingPredicates.RemoveWhere(m => Options.getOption().predicateConstraints.Any(constraint => constraint.isConflict(m, predicateMark)));

                            //Except from IDENTITY relationship
                            // Other relationship only hold when all objects in relationship appears
                            // We still need to consider predicate mark to remove nullified predicates before it
                            // However we don't add it if some object disappears
                            if (!predicateMark.predicate.Equals(Predicate.IdentityPredicate))
                            {
                                bool allExist = true;
                                foreach (var o in predicateMark.objects)
                                {
                                    // This object o still appear in the move
                                    if (!o.hasMark(frame))
                                    {
                                        allExist = false;
                                        break;
                                    }
                                }

                                if (allExist)
                                {
                                    holdingPredicates.Add(predicateMark);
                                }
                            }
                            else
                            {
                                holdingPredicates.Add(predicateMark);
                            }
                        }
                        else
                        {
                            holdingPredicates.RemoveWhere(m => m.isNegateOf(predicateMark));
                        }
                    }
            }

            return holdingPredicates;
        }

    }
}
