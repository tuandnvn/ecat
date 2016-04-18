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

        public SortedList<int, LocationMark> object3DMarks
        {
            get; private set;
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

        protected BorderType? _borderType;
        public string id { get; set; }                //Object's ID
        public string name { get; set; }           // Object's name
        public Color color { get; set; }           //Object's boudnign box color
        public string semanticType { get; set; }           //Object's type
        public int borderSize { get; set; }        //Object bounding box border size
        public double scale { get; set; }          //Object scale
        public BorderType? borderType { get { return _borderType; } }
        public Dictionary<string, string> otherProperties { get; }
        public GenType genType { get; set; }
        public ObjectType objectType { get; set; }
        public Session session { get; set; }

        public enum BorderType { Rectangle, Polygon, Others }

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

        public void set3DBounding(int frameNumber, LocationMark locationMark)
        {
            if (this.object3DMarks == null)
            {
                this.object3DMarks = new SortedList<int, LocationMark>();
            }
            object3DMarks[frameNumber] = locationMark;
        }

        public void setBounding(int frameNumber, Rectangle boundingBox, double scale, Point translation)
        {
            Rectangle inverseScaleBoundingBox = boundingBox.scaleBound(1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            var ob = new RectangleLocationMark(frameNumber, inverseScaleBoundingBox);
            if (this._borderType == null) // First time appear
            {
                this._borderType = BorderType.Rectangle;
            }
            else
            {
                if (this._borderType != BorderType.Rectangle)
                    throw new Exception("Border type not match");
            }
            objectMarks[frameNumber] = ob;
        }

        public void setBounding(int frameNumber, List<PointF> boundingPolygon, double scale, Point translation)
        {
            List<PointF> inverseScaleBoundingPolygon = boundingPolygon.scaleBound(1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            var ob = new PolygonLocationMark(frameNumber, inverseScaleBoundingPolygon);
            if (this._borderType == null) // First time appear
            {
                this._borderType = BorderType.Polygon;
            }
            else
            {
                if (this._borderType != BorderType.Polygon)
                    throw new Exception("Border type not match");
            }
            objectMarks[frameNumber] = ob;
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
        public LocationMark2D getScaledLocationMark(int frameNo, double scale, Point translation)
        {
            int first = objectMarks.Keys.LastOrDefault(x => x <= frameNo);
            
            if (first == 0)
            {
                return null;
            }
            if (_borderType != null)
            {
                if (objectMarks[first].GetType().IsSubclassOf(typeof(LocationMark2D)))
                {
                    Console.WriteLine(objectMarks[first].GetType());
                    return objectMarks[first].getScaledLocationMark(scale, translation);
                }
            }
            return null;
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
            if (_borderType == BorderType.Others)
            {
                xmlWriter.WriteAttributeString(SHAPE, this.GetType().ToString());
            }
            else
            {
                xmlWriter.WriteAttributeString(SHAPE, _borderType.ToString());
            }

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

        protected void write3DLocationMark(XmlWriter xmlWriter)
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

                Object.BorderType borderType = BorderType.Others;
                try
                {
                    borderType = (Object.BorderType)Enum.Parse(typeof(Object.BorderType), shape);
                    o = new Object(currentSession, id, color, borderSize, videoFile);
                    o._borderType = borderType;
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(shape);
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

                readMarkers(objectNode, o, borderType);
                readLinks(objectNode, o);

                // only handle 2d object
                if (objectType == "2D")
                {
                    objects.Add(o);
                }
            }
            return objects;
        }


        private static void readMarkers(XmlNode objectNode, Object o, BorderType borderType)
        {
            XmlNode markersNode = objectNode.SelectSingleNode(MARKERS);

            switch (borderType)
            {
                case BorderType.Rectangle:
                    {
                        if (markersNode == null) return;
                        foreach (XmlNode markerNode in markersNode.SelectNodes(MARKER))
                        {
                            int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                            String markType = markerNode.Attributes[TYPE].Value;

                            switch (markType.ToUpper())
                            {
                                case "LOCATION":
                                    var lm = new RectangleLocationMark(frame, new Rectangle());
                                    lm.readFromXml(markerNode);
                                    o.setBounding(frame, lm);
                                    break;
                                case "DELETE":
                                    o.delete(frame);
                                    break;
                            }
                        }
                        break;
                    }
                case BorderType.Polygon:
                    {
                        if (markersNode == null) return;
                        foreach (XmlNode markerNode in markersNode.SelectNodes(MARKER))
                        {
                            int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                            String markType = markerNode.Attributes[TYPE].Value;

                            switch (markType.ToUpper())
                            {
                                case "LOCATION":
                                    var lm = new PolygonLocationMark(frame, new List<PointF>());
                                    lm.readFromXml(markerNode);
                                    o.setBounding(frame, lm);
                                    break;
                                case "DELETE":
                                    o.delete(frame);
                                    break;
                            }
                        }
                        break;
                    }
                case BorderType.Others:
                    {
                        o.loadObjectAdditionalFromXml(objectNode);
                        break;
                    }
            }
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
