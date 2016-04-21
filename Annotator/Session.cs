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
using System.Globalization;

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
        
        public List<Event> events { get; private set; }
        public int objectCount { get; set; } = 0;          // video objects IDs
        private Dictionary<string, Object> objects;  // list of objects in videos
        private String sessionName;     //session name
        private String project;         //session's project 
        private String locationFolder;  //session location folder
        private bool edited;            //true if session is currently edited
        private List<VideoReader> videos;
        private List<String> filesList;
        private String metadataFile;      //parameters file name
        private String tempMetadataFile;
        public Main mainGUI { get; }
        private int annotationID;      // annotation ID
        private int? _sessionLength;
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
        public DateTime? startWriteRGB { get; set; }
        public long duration { get; set; }

        //Constructor
        public Session(String sessionName, String projectOwner, String locationFolder)
        {
            this.sessionName = sessionName;
            this.filesList = new List<String>();
            this.edited = false;
            this.videos = new List<VideoReader>();
            
            this.events = new List<Event>();
            this.project = projectOwner;
            this.locationFolder = locationFolder;
            objects = new Dictionary<string, Object>();
            //If session file list exist load files list            
            metadataFile = locationFolder + Path.DirectorySeparatorChar + project + Path.DirectorySeparatorChar + sessionName + Path.DirectorySeparatorChar + "files.param";
            tempMetadataFile = locationFolder + Path.DirectorySeparatorChar + project + Path.DirectorySeparatorChar + sessionName + Path.DirectorySeparatorChar + "~files.param";
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
            Console.WriteLine("Add object into session " + o.id);
            objects[o.id] = o;
        }

        //Get objects list
        public List<Object> getObjects()
        {
            return objects.Values.ToList();
        }

        //Save session
        public void saveSession()
        {
            try
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;

                using (XmlWriter writer = XmlWriter.Create(tempMetadataFile, ws))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement(SESSION);
                    writer.WriteAttributeString("name", sessionName);
                    writer.WriteAttributeString("length", "" + sessionLength);
                    if (duration != 0)
                    {
                        writer.WriteAttributeString("duration", "" + duration);
                    }

                    if (startWriteRGB.HasValue)
                    {
                        writer.WriteAttributeString("startWriteRGB", startWriteRGB.Value.ToString(@"yyyy-MM-ddTHH:mm:ssss.ffffffZ"));
                    }

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

                File.Copy(tempMetadataFile, metadataFile, true);

                File.Delete(tempMetadataFile);

                FileInfo myFile = new FileInfo(metadataFile);
                myFile.Attributes = FileAttributes.Hidden;
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal void removeObject(string objectId)
        {
            objects.Remove(objectId);
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
            CultureInfo provider = CultureInfo.CurrentCulture;
            if (File.Exists(metadataFile))
            {
                //Set file as hidden                
                FileInfo myFile = new FileInfo(metadataFile);
                // Remove the hidden attribute of the file
                myFile.Attributes &= ~FileAttributes.Hidden;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(metadataFile);

                sessionLength = int.Parse(xmlDocument.DocumentElement.Attributes["length"].Value);

                try
                {
                    duration = int.Parse(xmlDocument.DocumentElement.Attributes["duration"].Value);
                }
                catch (Exception e) {
                    duration = 0;
                }

                try
                {
                    Console.WriteLine(xmlDocument.DocumentElement.Attributes["startWriteRGB"].Value);
                    startWriteRGB = DateTime.ParseExact(xmlDocument.DocumentElement.Attributes["startWriteRGB"].Value, @"yyyy-MM-ddTHH:mm:ssss.ffffffZ", provider);
                    Console.WriteLine(startWriteRGB);
                }
                catch (Exception e)
                {
                    startWriteRGB = null;
                }

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
                List<Object> objects = Object.readFromXml(this, objectsNode);
                foreach (Object o in objects)
                {
                    addObject(o);
                }

                XmlNode annotationsNode = xmlDocument.DocumentElement.SelectSingleNode(ANNOTATIONS);
                events = Event.readFromXml(mainGUI, this, annotationsNode);

                myFile.Attributes |= FileAttributes.Hidden;
            }
        }

        //Get video by index
        public VideoReader getVideo(int index)
        {
            if (index < 0 || index >= videos.Count)
                return null;
            return videos[index];
        }
        //Get video numbers
        public int videoCount()
        {
            return videos.Count;
        }

        //Add video to session
        public void addVideo(String fileName)
        {
            bool exists = false;
            foreach (VideoReader v in videos)
            {
                if (v.fileName.Contains(fileName))
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                VideoReader v = new VideoReader(fileName, 0);
                videos.Add(v);
                Console.WriteLine(v.frameWidth);
                Console.WriteLine(v.frameHeight);
                Console.WriteLine(sessionLength + " " + fileName + " " + v.frameCount);
                sessionLength = v.frameCount;
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
