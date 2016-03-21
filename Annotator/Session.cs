using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Xml;

namespace Annotator
{
    //Session class
    public class Session
    {

        private const string FILES = "files";
        private const string FILE = "file";
        private const string OBJECTS = "objects";
        private const string ANNOTATIONS = "annotations";
        private const string SESSION = "session";
        public List<ObjectAnnotation> objectTracks { get; set; }
        public Dictionary<Object, ObjectAnnotation> objectToObjectTracks { get; }
        public List<Event> events { get; set; }
        private String sessionName;     //session name
        private String project;         //session's project 
        private String locationFolder;  //session location folder
        private bool edited;            //true if session is currently edited
        private List<Video> videos;
        private List<String> filesList;
        private String metadataFile;      //parameters file name
        public Main mainGUI { get; }
        private int annotationID;      // annotation ID
        public int? _sessionLength;
        public int sessionLength
        {
            get
            {
                if (_sessionLength.HasValue)
                    return _sessionLength.Value;
                return 0;
            }
            set
            {
                if (_sessionLength.HasValue)
                {
                    if (_sessionLength.Value != value)
                    {
                        throw new Exception("Session length inconsistence");
                    }
                }
                else
                {
                    _sessionLength = value;
                }
            }
        }
        public int objectCount { get; set; } = 0;          // video objects IDs
        private Dictionary<string, Object> objects;  // list of objects in videos

        //Constructor
        public Session(String sessionName, String projectOwner, String locationFolder, Main frm1)
        {
            this.sessionName = sessionName;
            this.filesList = new List<String>();
            this.edited = false;
            this.videos = new List<Video>();
            this.objectTracks = new List<ObjectAnnotation>();
            this.objectToObjectTracks = new Dictionary<Object, ObjectAnnotation>();
            this.events = new List<Event>();
            this.project = projectOwner;
            this.locationFolder = locationFolder;
            this.mainGUI = frm1;
            objects = new Dictionary<string, Object>();
            //If session file list exist load files list            
            metadataFile = locationFolder + Path.DirectorySeparatorChar + project + Path.DirectorySeparatorChar + sessionName + Path.DirectorySeparatorChar + "files.param";
            loadSession();
        }
        //Add file to session filesList
        public void addFile(String fileName)
        {
            //1)check if file already exists in session files list
            bool exists = false;
            foreach (String file in filesList)
            {
                if (file.Contains(fileName))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists && !fileName.Contains("files.param"))
                filesList.Add(fileName);
            if (fileName.Contains(".avi"))
                addVideo(fileName);
            //MessageBox.Show(filesList.Count + "");
        }
        //Get session name
        public String getSessionName()
        {
            return sessionName;
        }

        //Add object
        public void addObject(Object o)
        {
            //1)Check if object exists in objects list
            bool exists = false;
            if (o.id != null && objects.ContainsKey(o.id))
            {
                return;
            }
            if (o.id == "" || o.id == null) { o.id = "o" + ++objectCount; }
            objects[o.id] = o;
            addObjectAnnotation(o);
        }

        //Get objects list
        public List<Object> getObjects()
        {
            return objects.Values.ToList();
        }

        //Save session
        public void saveSession()
        {
            if (File.Exists(metadataFile))
            {
                FileInfo myFile1 = new FileInfo(metadataFile);
                myFile1.Attributes &= ~FileAttributes.Hidden;
            }

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter writer = XmlWriter.Create(metadataFile, ws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(SESSION);
                writer.WriteAttributeString("name", sessionName);
                writer.WriteAttributeString("length", "" + sessionLength);
                {
                    // Write files
                    writer.WriteStartElement(FILES);
                    foreach (String file in filesList)
                    {
                        writer.WriteElementString(FILE, file);
                    }
                    writer.WriteEndElement();
                }

                {
                    writer.WriteStartElement(OBJECTS);
                    writer.WriteAttributeString("no", objectCount + "");
                    foreach (Object o in objects.Values)
                    {
                        o.writeToXml(writer);
                    }
                    writer.WriteEndElement();
                }

                {
                    writer.WriteStartElement(ANNOTATIONS);
                    foreach (Event a in events)
                    {
                        a.writeToXml(writer);
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            FileInfo myFile = new FileInfo(metadataFile);
            myFile.Attributes = FileAttributes.Hidden;
        }

        internal void removeObject(Object o)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this object?",
                "Remove",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Console.WriteLine("Remove object " + o.id);
                mainGUI.removeObject(o);
                objects.Remove(o.id);
                ObjectAnnotation ot = this.objectToObjectTracks[o];
                if (ot != null)
                {
                    this.objectToObjectTracks.Remove(o);
                    this.objectTracks.Remove(ot);
                }

                foreach (ObjectAnnotation other in objectTracks)
                {
                    if (other.Location.Y > ot.Location.Y)
                    {
                        other.Location = new Point(other.Location.X, other.Location.Y - ot.Height - 5);
                    }
                }
                mainGUI.removeObjectTracking(ot);
            }
        }

        internal void generate3dforObject(Object o)
        {
            
        }

        internal void selectObject(Object o)
        {
            //Remove any decoration of other objects
            foreach (Object other in objectToObjectTracks.Keys)
            {
                objectToObjectTracks[other].deselectDeco();
            }
            mainGUI.selectObject(o);
        }

        internal void deselectObject(Object o)
        {
            if (objectToObjectTracks[o] != null)
            {
                objectToObjectTracks[o].deselectDeco();
            }
        }

        //Get edited
        public bool getEdited()
        {
            return edited;
        }
        //Set edited
        public void setEdited(bool edited)
        {
            this.edited = edited;
        }

        //Load files list
        public void loadSession()
        {
            if (File.Exists(metadataFile))
            {
                //Set file as hidden                
                FileInfo myFile = new FileInfo(metadataFile);
                // Remove the hidden attribute of the file
                myFile.Attributes &= ~FileAttributes.Hidden;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(metadataFile);

                sessionLength = int.Parse(xmlDocument.DocumentElement.Attributes["length"].Value);

                XmlNode files = xmlDocument.DocumentElement.SelectSingleNode(FILES);

                foreach (XmlNode node in files.SelectNodes(FILE))
                {
                    string filename = node.InnerText;
                    filesList.Add(node.InnerText);
                    if (filename.Contains(".avi"))
                    {
                        addVideo(filename);
                    }
                }

                XmlNode objectsNode = xmlDocument.DocumentElement.SelectSingleNode(OBJECTS);
                objectCount = int.Parse(objectsNode.Attributes["no"].Value);
                List<Object> objects = Object.readFromXml(objectsNode);
                foreach (Object o in objects)
                {
                    String videoName = o.videoFile;
                    Video v = null;

                    foreach (Video video in videos)
                    {
                        if (video.getFileName().Contains(videoName))
                        {
                            v = video;
                            break;
                        }
                    }
                    addObject(o);
                }

                XmlNode annotationsNode = xmlDocument.DocumentElement.SelectSingleNode(ANNOTATIONS);
                events = Event.readFromXml(mainGUI, this, annotationsNode);

                myFile.Attributes |= FileAttributes.Hidden;
            }
        }

        private void addObjectAnnotation(Object o)
        {
            var t = new ObjectAnnotation(o, this);
            objectTracks.Add(t);
            objectToObjectTracks[o] = t;
        }

        //Get video by index
        public Video getVideo(int index)
        {
            if (index < 0 || index > videos.Count)
                return null;
            return videos[index];
        }
        //Get video numbers
        public int getVideosN()
        {
            return videos.Count;
        }

        //Add video to session
        public void addVideo(String fileName)
        {
            bool exists = false;
            foreach (Video v in videos)
            {
                if (v.getFileName().Contains(fileName))
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                Video v = new Video(this, fileName);
                videos.Add(v);
                sessionLength = v.getFramesNumber();
            }
        }
        //Get session's project
        public String getProject()
        {
            return project;
        }
        //Check if file inside project files list
        public bool checkFileInSession(String fName)
        {
            foreach (String fileName in filesList)
            {
                if (fileName.Contains(fName))
                    return true;
            }
            return false;
        }

        //Get views
        public String[] getViews()
        {
            List<String> viewsL = new List<String>();
            //MessageBox.Show(filesList.Count + "");
            foreach (String file in filesList)
            {
                //MessageBox.Show(file);
                if (file.Contains(".avi"))
                    viewsL.Add(file.Split(Path.DirectorySeparatorChar)[file.Split(Path.DirectorySeparatorChar).Length - 1]);
            }
            return viewsL.ToArray();
        }

        //Add annotation
        internal void addEvent(Event a)
        {
            bool exists = false;
            foreach (Event ev in events)
            {
                if (ev.id == a.id)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                events.Add(a);
                a.id = "a" + ++annotationID;
            }
        }

        internal void removeEvent(Event a)
        {
            events.Remove(a);
        }
    }
}
