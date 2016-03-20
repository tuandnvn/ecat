using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
            get; set;
        }

        public SortedList<int, SpatialLinkMark> spatialLinkMarks
        {
            get; set;
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

        BorderType? _borderType;
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

        public class ObjectMark
        {
            public int frameNo { get; }
            public ObjectMark(int frameNo)
            {
                this.frameNo = frameNo;
            }
        }

        public class SpatialLinkMark : ObjectMark
        {
            public enum SpatialLinkType
            {
                ON,
                IN,
                ATTACH_TO,
                NEXT_TO
            };

            // A set of link to other objects at a certain frame
            // Each link is of < objectID , qualified, spatialLink >
            public SortedSet<Tuple<string, bool, SpatialLinkType>> spatialLinks { get; } // By default, there is no spatial configuration attached to an object location

            public SpatialLinkMark(int frameNo) : base(frameNo)
            {
                spatialLinks = new SortedSet<Tuple<string, bool, SpatialLinkType>>();
            }

            public void addLinkToObject(string objectId, bool qualified, SpatialLinkType linkType)
            {
                spatialLinks.Add(new Tuple<string, bool, SpatialLinkType>(objectId, qualified, linkType));
            }

            override public String ToString()
            {
                return String.Join(",", spatialLinks.Select( u => getLiteralForm(u) ));
            }

            private static String getLiteralForm(Tuple<string, bool, SpatialLinkType> t)
            {
                String q = t.Item3 + "( " + t.Item1 + " )";
                if (!t.Item2)
                {
                    q = "NOT( " + q + " )";
                }
                return q;
            }
        }

        public class LocationMark : ObjectMark
        {
            public enum LocationMarkType
            {
                Location, // Set the location of the object manually
                          // For automatic detected object, there would be location for the first time the object is detected

                Delete    // The position where the object disappears out of the view
            };

            public LocationMarkType markType { get; }

            public LocationMark(int frameNo, LocationMarkType markType) : base(frameNo)
            {
                this.markType = markType;
            }

            // Delete object mark
            public LocationMark(int frameNo) : base(frameNo)
            {
                this.markType = LocationMarkType.Delete;
            }
        }

        public class BoundingLocationMark : LocationMark
        {
            public Rectangle boundingBox { get; }              //Object bounding box;

            public BoundingLocationMark(int frameNo, LocationMarkType markType, Rectangle boundingBox) : base(frameNo, markType)
            {
                this.boundingBox = boundingBox;
            }
        }

        public class PolygonLocationMark : LocationMark
        {
            public List<Point> boundingPolygon { get; }         // Object bounding polygon, if the tool to draw object is polygon tool

            public PolygonLocationMark(int frameNo, LocationMarkType markType, List<Point> boundingPolygon) : base(frameNo, markType)
            {
                this.boundingPolygon = boundingPolygon;
            }
        }

        public class RigLocationMark : LocationMark
        {
            public RigFigure<Point> rigFigure { get; }
            public RigLocationMark(int frameNo, LocationMarkType markType, RigFigure<Point> rigFigure) : base(frameNo, markType)
            {
                this.rigFigure = rigFigure;
            }
        }

        public enum BorderType { Rig, Rectangle, Polygon }
        
        //Constructor
        public Object(String id, Color color, int borderSize, string videoFile, double scale, Point translation, int frameNo, Rectangle boundingBox) :
            this(id, color, borderSize, videoFile)
        {
            setBounding(frameNo, boundingBox, scale, translation);
        }

        public Object(String id, Color color, int borderSize, string videoFile, double scale, Point translation, int frameNo, List<Point> boundingPolygon) : 
            this(id, color, borderSize, videoFile)
        {
            setBounding(frameNo, boundingPolygon, scale, translation);
        }

        public Object(String id, Color color, int borderSize, string videoFile)
        {
            this.id = id;
            this.color = color;
            this.borderSize = borderSize;
            this.videoFile = videoFile;
            otherProperties = new Dictionary<string, string>();
            objectMarks = new SortedList<int, LocationMark>();
            spatialLinkMarks = new SortedList<int, SpatialLinkMark>();
        }

        public void setBounding(int frameNumber, Rectangle boundingBox, double scale, Point translation) {
            Rectangle inverseScaleBoundingBox = scaleRectangle(boundingBox, 1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
            LocationMark ob = new BoundingLocationMark(frameNumber, LocationMark.LocationMarkType.Location, inverseScaleBoundingBox);
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

        public void setBounding(int frameNumber, List<Point> boundingPolygon, double scale, Point translation)
        {
            List<Point> inverseScaleBoundingPolygon = scaleListPoint(boundingPolygon, 1 / scale, new Point((int)(-translation.X / scale), (int)(-translation.Y / scale)));
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

        public void setBounding(int frameNumber, RigFigure<Point> boundingRig, double scale, Point translation)
        {
            RigFigure<Point> inverseScaleBoundingRig =  scaleRig(boundingRig, 1 / scale , new Point((int)(-translation.X/scale), (int)(-translation.Y / scale)) );
            LocationMark ob = new RigLocationMark(frameNumber, LocationMark.LocationMarkType.Location, inverseScaleBoundingRig);
            if (this._borderType == null) // First time appear
            {
                this._borderType = BorderType.Rig;
            }
            else
            {
                if (this._borderType != BorderType.Rig)
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
            if ( first > 0 && objectMarks[first].markType == LocationMark.LocationMarkType.Delete)
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
        public object getCurrentBounding(int frameNo , double scale, Point translation)
        {
            int first = objectMarks.Keys.LastOrDefault(x => x <= frameNo);
            if ( first == 0 )
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
                                var casted = (BoundingLocationMark)objectMarks[first];
                                return scaleRectangle(casted.boundingBox, scale, translation);
                            }
                        case BorderType.Polygon:
                            {
                                var casted = (PolygonLocationMark)objectMarks[first];
                                return scaleListPoint(casted.boundingPolygon, scale, translation);
                            }
                        case BorderType.Rig:
                            {
                                var casted = (RigLocationMark)objectMarks[first];
                                return scaleRig(casted.rigFigure, scale, translation);
                            }
                    }
                }
            }
            return null;
        }

        private static Rectangle scaleRectangle(Rectangle original, double scale, Point translation)
        {
            return new Rectangle((int)(original.X * scale + translation.X),
                (int)(original.Y * scale + translation.Y),
                (int)(original.Width * scale),
                (int)(original.Height * scale));
        }

        private static Point scalePoint ( Point original , double scale, Point translation)
        {
            return new Point((int)(original.X * scale + translation.X), (int)(original.Y * scale + translation.Y));
        }

        private static List<Point> scaleListPoint(List<Point> original, double scale, Point translation)
        {
            return original.Select(p => scalePoint( p, scale, translation) ).ToList(); 
        }

        private static RigFigure<Point> scaleRig(RigFigure<Point> original, double scale, Point translation)
        {
            return new RigFigure<Point>(original.rigJoints.ToDictionary(k => k.Key, k => scalePoint(k.Value, scale, translation) ),
                original.rigBones.Select(t => new Tuple<Point, Point>(scalePoint(t.Item1, scale, translation), scalePoint(t.Item2, scale, translation))).ToList() );
        }

        public void addProperty(string propertyKey, string propertyValue)
        {
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
            xmlWriter.WriteAttributeString(NAME,  name);
            xmlWriter.WriteAttributeString(FILENAME, videoFile);
            xmlWriter.WriteAttributeString(OBJECT_TYPE, objectType.ToString().Substring(1) );
            xmlWriter.WriteAttributeString(GENERATE, genType.ToString());
            xmlWriter.WriteAttributeString(COLOR, "" + color.ToArgb());
            xmlWriter.WriteAttributeString(BORDER_SIZE, "" + borderSize);
            xmlWriter.WriteAttributeString(SEMANTIC_TYPE, semanticType);
            xmlWriter.WriteAttributeString(SHAPE, _borderType.ToString());
            foreach (String key in otherProperties.Keys)
            {
                xmlWriter.WriteAttributeString(key, otherProperties[key]);
            }
            
            if ( genType == GenType.MANUAL )
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
                                    var casted = (BoundingLocationMark)objectMarks[frame];
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
                var borderType = (Object.BorderType) Enum.Parse(typeof(Object.BorderType), shape);

                Object o = new Object(id, color, borderSize, videoFile);
                o.name = name;
                o.semanticType = semanticType;
                o.genType = (GenType) Enum.Parse( typeof(GenType), generate.ToUpper());
                o.objectType = (ObjectType) Enum.Parse(typeof(ObjectType), "_" + objectType.ToUpper());

                foreach (XmlAttribute attr in objectNode.Attributes)
                {
                    if (! (new List<String>{ ID, NAME, FILENAME, OBJECT_TYPE, GENERATE, BORDER_SIZE, COLOR, SEMANTIC_TYPE, SHAPE }).Contains(attr.Name)) {
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
                                        List<Point> points = new List<Point>();
                                        if (parts.Length % 2 == 0)
                                        {
                                            for (int i = 0; i < parts.Length / 2; i++)
                                            {
                                                Point p = new Point(int.Parse(parts[2 * i].Trim()), int.Parse(parts[2 * i + 1].Trim()));
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
                    case BorderType.Rig:
                    {
                        string sourceScheme = o.otherProperties["sourceScheme"];
                        string source = o.otherProperties["source"];
                        int rigIndex = int.Parse(o.otherProperties["rigIndex"]);
                        Rigs<Point>.loadDataForRig(source, sourceScheme, rigIndex, o);
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
                if ( objectType == "2D" )
                {
                    objects.Add(o);
                }
            }
            return objects;
        }

        public String queryTooltip ( int frameNo)
        {
            if (spatialLinkMarks.ContainsKey(frameNo))
            {
                return spatialLinkMarks[frameNo].ToString();
            }
            return "";
        }
    }
}
