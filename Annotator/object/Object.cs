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
        private const string OBJECT = "object";
        private const string ID = "id";
        private const string NAME = "name";
        private const string FILENAME = "filename";
        private const string OBJECT_TYPE = "objectType";
        private const string GENERATE = "generate";
        private const string SEMANTIC_TYPE = "semanticType";
        private const string MARKER = "marker";
        private const string TYPE = "type";
        private const string FRAME = "frame";
        private const string SHAPE = "shape";
        private const string BORDER_SIZE = "borderSize";
        private const string COLOR = "color";
        private const string SPATIAL_LINK = "spatialLink";
        private const string QUALIFIED = "q";
        private const string LINKTO = "linkTo";

        public string videoFile { get; }

        public SortedList<int, LocationMark> objectMarks
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

        
        public enum BorderType { Rectangle, Polygon, Others }

        /// <summary>
        /// Create an object that is manually drawn on the paintBoard
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        /// <param name="borderSize"></param>
        /// <param name="videoFile"></param>
        public Object(String id, Color color, int borderSize, string videoFile)
        {
            this.id = id;
            this.color = color;
            this.borderSize = borderSize;
            this.videoFile = videoFile;
            this.genType = GenType.MANUAL;
            this.objectType = ObjectType._2D;
            this.otherProperties = new Dictionary<string, string>();
            this.objectMarks = new SortedList<int, LocationMark>();
            this.object3DMarks = null;
            this.spatialLinkMarks = new SortedList<int, SpatialLinkMark>();
        }

        public void setBounding(int frameNumber, Rectangle boundingBox, double scale, Point translation)
        {
            Rectangle inverseScaleBoundingBox = scaleBound(boundingBox, 1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            LocationMark ob = new RectangleLocationMark(frameNumber, LocationMark.LocationMarkType.Location, inverseScaleBoundingBox);
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
            List<PointF> inverseScaleBoundingPolygon = scaleBound(boundingPolygon, 1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            LocationMark ob = new PolygonLocationMark(frameNumber, LocationMark.LocationMarkType.Location, inverseScaleBoundingPolygon);
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
            LocationMark ob = new LocationMark(frameNumber);
            objectMarks[frameNumber] = ob;

            // Delete the next Delete marker
            int first = objectMarks.Keys.FirstOrDefault(x => x > frameNumber);
            if (first > 0 && objectMarks[first].markType == LocationMark.LocationMarkType.Delete)
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
        public LocationMark getScaledLocationMark(int frameNo, double scale, Point translation)
        {
            int first = objectMarks.Keys.LastOrDefault(x => x <= frameNo);
            if (first == 0)
            {
                return null;
            }
            if (_borderType != null)
            {
                if (objectMarks[first].markType == LocationMark.LocationMarkType.Location)
                {
                    switch (_borderType)
                    {
                        case BorderType.Rectangle:
                            {
                                var casted = (RectangleLocationMark)objectMarks[first];
                                return new RectangleLocationMark(frameNo, objectMarks[first].markType, scaleBound(casted.boundingBox, scale, translation));
                            }
                        case BorderType.Polygon:
                            {
                                var casted = (PolygonLocationMark)objectMarks[first];
                                return new PolygonLocationMark(frameNo, objectMarks[first].markType, scaleBound(casted.boundingPolygon, scale, translation));
                            }
                        case BorderType.Others:
                            {
                                return getScaledLocationMark(objectMarks[first], scale, translation);

                            }
                    }
                }
            }
            return null;
        }

        protected virtual LocationMark getScaledLocationMark(LocationMark locationMark, double scale, Point translation)
        {
            return null;
        }

        protected static Rectangle scaleBound(Rectangle original, double scale, Point translation)
        {
            return new Rectangle((int)(original.X * scale + translation.X),
                (int)(original.Y * scale + translation.Y),
                (int)(original.Width * scale),
                (int)(original.Height * scale));
        }

        protected static Point scalePoint(Point original, double scale, Point translation)
        {
            return new Point((int)(original.X * scale + translation.X), (int)(original.Y * scale + translation.Y));
        }

        protected static PointF scalePoint(PointF original, double scale, Point translation)
        {
            return new PointF((float)(original.X * scale + translation.X), (float)(original.Y * scale + translation.Y));
        }

        private static List<PointF> scaleBound(List<PointF> original, double scale, Point translation)
        {
            return original.Select(p => scalePoint(p, scale, translation)).ToList();
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

        public void writeToXml(XmlWriter xmlWriter)
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
            } else
            {
                xmlWriter.WriteAttributeString(SHAPE, _borderType.ToString());
            }
            
            foreach (String key in otherProperties.Keys)
            {
                xmlWriter.WriteAttributeString(key, otherProperties[key]);
            }

            if (genType == GenType.MANUAL)
            {
                foreach (int frame in objectMarks.Keys)
                {
                    xmlWriter.WriteStartElement(MARKER);
                    xmlWriter.WriteAttributeString(TYPE, objectMarks[frame].markType.ToString());
                    xmlWriter.WriteAttributeString(FRAME, "" + frame);
                    if (objectMarks[frame].markType == LocationMark.LocationMarkType.Location)
                    {

                        switch (_borderType)
                        {
                            case BorderType.Rectangle:
                                {
                                    var casted = (RectangleLocationMark)objectMarks[frame];
                                    xmlWriter.WriteString(casted.boundingBox.X + "," + casted.boundingBox.Y + "," +
                                        casted.boundingBox.Width + "," + casted.boundingBox.Height);
                                    break;
                                }
                            case BorderType.Polygon:
                                {
                                    var casted = (PolygonLocationMark)objectMarks[frame];
                                    xmlWriter.WriteString(string.Join(",", casted.boundingPolygon.ConvertAll(p => p.X + "," + p.Y)));
                                    break;
                                }
                        }
                    }
                    xmlWriter.WriteEndElement();
                }
            }

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

        public static List<Object> readFromXml(XmlNode xmlNode)
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
                    o = new Object(id, color, borderSize, videoFile);
                } catch (ArgumentException e)
                {
                    Type t = Type.GetType(shape);
                    if (t.IsSubclassOf(typeof(Object)))
                    {
                        o = (Object) Activator.CreateInstance(t, id, color, borderSize, videoFile);
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


                switch (borderType)
                {
                    case BorderType.Rectangle:
                        {
                            foreach (XmlNode markerNode in objectNode.SelectNodes(MARKER))
                            {
                                int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                                LocationMark.LocationMarkType markType = (LocationMark.LocationMarkType)Enum.Parse(typeof(LocationMark.LocationMarkType), markerNode.Attributes[TYPE].Value, true);

                                switch (markType)
                                {
                                    case LocationMark.LocationMarkType.Location:
                                        String parameters = markerNode.InnerText;
                                        String[] parts = parameters.Split(',');
                                        if (parts.Length == 4)
                                        {
                                            int x = int.Parse(parts[0].Trim());
                                            int y = int.Parse(parts[1].Trim());
                                            int width = int.Parse(parts[2].Trim());
                                            int height = int.Parse(parts[3].Trim());
                                            Rectangle r = new Rectangle(x, y, width, height);
                                            o.setBounding(frame, r, 1, new Point());
                                        }
                                        break;
                                    case LocationMark.LocationMarkType.Delete:
                                        o.delete(frame);
                                        break;
                                }
                            }
                            break;
                        }
                    case BorderType.Polygon:
                        {
                            foreach (XmlNode markerNode in objectNode.SelectNodes(MARKER))
                            {
                                int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                                LocationMark.LocationMarkType markType = (LocationMark.LocationMarkType)Enum.Parse(typeof(LocationMark.LocationMarkType), markerNode.Attributes[TYPE].Value, true);

                                switch (markType)
                                {
                                    case LocationMark.LocationMarkType.Location:
                                        String parameters = markerNode.InnerText;
                                        String[] parts = parameters.Split(',');
                                        List<PointF> points = new List<PointF>();
                                        if (parts.Length % 2 == 0)
                                        {
                                            for (int i = 0; i < parts.Length / 2; i++)
                                            {
                                                PointF p = new Point(int.Parse(parts[2 * i].Trim()), int.Parse(parts[2 * i + 1].Trim()));
                                                points.Add(p);
                                            }
                                        }
                                        o.setBounding(frame, points, 1, new Point());
                                        break;
                                    case LocationMark.LocationMarkType.Delete:
                                        o.delete(frame);
                                        break;
                                }
                            }
                            break;
                        }
                    case BorderType.Others:
                        {
                            o.loadObjectAdditionalFromXml();
                            break;
                        }
                }

                foreach (XmlNode markerNode in objectNode.SelectNodes(SPATIAL_LINK))
                {
                    int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                    foreach (XmlNode linkto in markerNode.SelectNodes(LINKTO))
                    {
                        String linkToObjectId = linkto.Attributes[ID].Value;
                        bool qualified = bool.Parse(linkto.Attributes[QUALIFIED].Value);
                        SpatialLinkMark.SpatialLinkType markType = (SpatialLinkMark.SpatialLinkType)Enum.Parse(typeof(SpatialLinkMark.SpatialLinkType), linkto.Attributes[TYPE].Value, true);
                        o.setSpatialLink(frame, linkToObjectId, qualified, markType);
                    }
                }

                // only handle 2d object
                if (objectType == "2D")
                {
                    objects.Add(o);
                }
            }
            return objects;
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
        protected virtual void loadObjectAdditionalFromXml()
        {
        }
    }
}
