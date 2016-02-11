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
        //Constructor
        public Session(String sessionName, String projectOwner, String locationFolder, Main frm1)
        {

            this.sessionName = sessionName;
            this.filesList = new List<String>();
            this.edited = false;
            this.videos = new List<Video>();
            this.objectTracks = new List<ObjectTrack>();
            this.objectToObjectTracks = new Dictionary<Object, ObjectTrack>();
            this.annotations = new List<Annotation>();
            this.project = projectOwner;
            this.locationFolder = locationFolder;
            this.mainGUI = frm1;
            //If session file list exist load files list            
            metadataFile = locationFolder + "\\" + project + "\\" + sessionName + "\\files.param";
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

        private const string FILES = "files";
        private const string FILE = "file";
        private const string OBJECTS = "objects";
        private const string ANNOTATIONS = "annotations";
        private const string SESSION = "session";

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
                    foreach (Video v in videos)
                    {
                        writer.WriteStartElement(OBJECTS);
                        foreach (Object o in v.getObjects())
                        {
                            o.writeToXml(writer);
                        }
                        writer.WriteEndElement();
                    }
                }

                {
                    writer.WriteStartElement(ANNOTATIONS);
                    foreach (Annotation a in annotations)
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
        }

        internal void generate3dforObject(Object o)
        {
            
        }

        internal void selectObject(Object o)
        {
            mainGUI.selectObject(o);
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
                    if (v != null)
                    {
                        v.addObject(o);
                        Console.WriteLine("Add object " + o.id);
                        var t = new ObjectTrack(o, this);
                        objectTracks.Add(t);
                        objectToObjectTracks[o] = t;
                    }
                }

                XmlNode annotationsNode = xmlDocument.DocumentElement.SelectSingleNode(ANNOTATIONS);
                annotations = Annotation.readFromXml(mainGUI, this, annotationsNode);

                myFile.Attributes |= FileAttributes.Hidden;
            }
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
                    viewsL.Add(file.Split('\\')[file.Split('\\').Length - 1]);
            }
            return viewsL.ToArray();
        }

        //Add annotation
        public void addAnnotation(Annotation a)
        {
            //1)Check if object exists in objects list
            bool exists = false;
            foreach (Annotation annotation in annotations)
            {
                if (annotation.id == a.id)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                annotations.Add(a);
                a.id = "a" + ++annotationID;
            }
        }

        public List<ObjectTrack> objectTracks { get; set; }
        public Dictionary<Object, ObjectTrack> objectToObjectTracks { get; }
        public List<Annotation> annotations { get; set; }
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
                throw new Exception("No session length");
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
    }
}
