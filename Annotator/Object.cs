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
        public class ObjectMark
        {
            public enum MarkType{
                Location, // Set the location of the object manually
                          // For automatic detected object, there would be location for the first time the object is detected

                Delete    // The position where the object disappears out of the view
            };
            public int frameNo { get; }
            public MarkType markType { get; }
            public Rectangle boundingBox { get; }              //Object bounding box;
            public List<Point> boundingPolygon { get; }         // Object bounding polygon, if the tool to draw object is polygon tool

            public ObjectMark(int frameNo, MarkType markType, Rectangle boundingBox) {
                this.frameNo = frameNo;
                this.markType = markType;
                this.boundingBox = boundingBox;
            }

            public ObjectMark(int frameNo, MarkType markType, List<Point> boundingPolygon)
            {
                this.frameNo = frameNo;
                this.markType = markType;
                this.boundingPolygon = boundingPolygon;
            }

            // Delete object mark
            public ObjectMark(int frameNo)
            {
                this.frameNo = frameNo;
                this.markType = MarkType.Delete;
            }
        }

        public enum BorderType { Rectangle, Polygon }

        
        //Constructor
        public Object(String id, Color color, int borderSize, double scale, string videoFile, int frameNo, Rectangle boundingBox) :
            this(id, color, borderSize, scale, videoFile)
        {
            setBounding(frameNo, boundingBox);
        }

        public Object(String id, Color color, int borderSize, double scale, string videoFile, int frameNo, List<Point> boundingPolygon) : 
            this(id,color,borderSize,scale,videoFile)
        {
            setBounding(frameNo, boundingPolygon);
        }

        private Object(String id, Color color, int borderSize, double scale, string videoFile)
        {
            this.id = id;
            this.color = color;
            this.borderSize = borderSize;
            this.scale = scale;
            this.videoFile = videoFile;
            otherProperties = new Dictionary<string, string>();
            objectMarks = new SortedList<int,ObjectMark>();
        }

        public void setBounding(int frameNumber, Rectangle boundingBox) {
            ObjectMark ob = new ObjectMark(frameNumber, ObjectMark.MarkType.Location, boundingBox);
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

        public void setBounding(int frameNumber, List<Point> boundingPolygon)
        {
            ObjectMark ob = new ObjectMark(frameNumber, ObjectMark.MarkType.Location, boundingPolygon);
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
            ObjectMark ob = new ObjectMark(frameNumber);
            objectMarks[frameNumber] = ob;
    
            // Delete the next Delete marker
            int first = objectMarks.Keys.FirstOrDefault(x => x > frameNumber);
            if ( first > 0 && objectMarks[first].markType == ObjectMark.MarkType.Delete)
            {
                objectMarks.Remove(first);
            }
        }

        public object getCurrentBounding(int frameNo)
        {
            int first = objectMarks.Keys.LastOrDefault(x => x <= frameNo);
            if ( first == 0 )
            {
                return null;
            }
            if (_borderType != null)
            {
                if (objectMarks[first].markType == ObjectMark.MarkType.Location)
                {
                    switch (_borderType)
                    {
                        case BorderType.Rectangle:
                            return objectMarks[first].boundingBox;
                        case BorderType.Polygon:
                            return objectMarks[first].boundingPolygon;
                    }
                }
            }
            return null;
        }

        public void addProperty(string propertyKey, string propertyValue)
        {
            otherProperties[propertyKey] = propertyValue;
        }

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
        private const string SCALE = "scale";

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
            xmlWriter.WriteAttributeString(OBJECT_TYPE, "2D");
            xmlWriter.WriteAttributeString(GENERATE, "manual");
            xmlWriter.WriteAttributeString(COLOR, "" + color.ToArgb());
            xmlWriter.WriteAttributeString(BORDER_SIZE, "" + borderSize);
            xmlWriter.WriteAttributeString(SCALE, "" + scale);
            xmlWriter.WriteAttributeString(SEMANTIC_TYPE, semanticType);
            foreach (String key in otherProperties.Keys)
            {
                xmlWriter.WriteAttributeString(key, otherProperties[key]);
            }
            
            foreach (int frame in objectMarks.Keys)
            {
                xmlWriter.WriteStartElement(MARKER);
                xmlWriter.WriteAttributeString(TYPE, objectMarks[frame].markType.ToString());
                xmlWriter.WriteAttributeString(FRAME, "" + frame);
                if (objectMarks[frame].markType == ObjectMark.MarkType.Location)
                {
                    xmlWriter.WriteAttributeString(SHAPE, _borderType.ToString());
                    switch(_borderType)
                    {
                        case BorderType.Rectangle:
                            xmlWriter.WriteString(objectMarks[frame].boundingBox.X + "," + objectMarks[frame].boundingBox.Y + "," +
                                objectMarks[frame].boundingBox.Width + "," + objectMarks[frame].boundingBox.Height);
                            break;
                        case BorderType.Polygon:
                            xmlWriter.WriteString(string.Join(",", objectMarks[frame].boundingPolygon.ConvertAll(p => p.X + "," + p.Y)));
                            break;
                    }
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
                double scale = Double.Parse(objectNode.Attributes[SCALE].Value);
                String semanticType = objectNode.Attributes[SEMANTIC_TYPE].Value;

                Object o = new Object(id, color, borderSize, scale, videoFile);
                o.name = name;
                o.semanticType = semanticType;

                foreach (XmlAttribute attr in objectNode.Attributes)
                {
                    if (! (new List<String>{ ID, NAME, FILENAME, OBJECT_TYPE, GENERATE, BORDER_SIZE, COLOR, SCALE, SEMANTIC_TYPE }).Contains(attr.Name)) {
                        o.addProperty(attr.Name, attr.Value);
                    }
                }

                foreach (XmlNode markerNode in objectNode.SelectNodes(MARKER))
                {
                    int frame = int.Parse(markerNode.Attributes[FRAME].Value);
                    ObjectMark.MarkType markType = (ObjectMark.MarkType)Enum.Parse(typeof(ObjectMark.MarkType), markerNode.Attributes[TYPE].Value, true);

                    switch (markType)
                    {
                        case ObjectMark.MarkType.Location:
                            string shape = markerNode.Attributes[SHAPE].Value;

                            if (shape == "Rectangle")
                            {
                                String parameters = markerNode.InnerText;
                                String[] parts = parameters.Split(',');
                                if (parts.Length == 4)
                                {
                                    int x = int.Parse(parts[0].Trim());
                                    int y = int.Parse(parts[1].Trim());
                                    int width = int.Parse(parts[2].Trim());
                                    int height = int.Parse(parts[3].Trim());
                                    Rectangle r = new Rectangle(x, y, width, height);
                                    o.setBounding(frame, r);
                                }
                            }
                            else if (shape == "Polygon")
                            {
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
                                o.setBounding(frame, points);
                            }
                            break;
                        case ObjectMark.MarkType.Delete:
                            o.delete(frame);
                            break;
                    }
                }

                // only handle 2d object
                if ( objectType == "2D" && generate == "manual")
                {
                    objects.Add(o);
                }
            }
            return objects;
        }

        public string videoFile { get; }
        public SortedList<int, ObjectMark> objectMarks
        {
            get; set;
        }
        BorderType? _borderType;
        public string id { get; set; }                //Object's ID
        public string name { get; set; }           // Object's name
        public Color color { get; set; }           //Object's boudnign box color
        public string semanticType { get; set; }           //Object's type
        public int borderSize { get; set; }        //Object bounding box border size
        public double scale { get; set; }          //Object scale
        public BorderType? borderType { get { return _borderType;}  }
        public Dictionary<string, string> otherProperties { get; }
    }
}
