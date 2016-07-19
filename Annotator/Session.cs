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
        public String sessionName { get; private set; }     //session name
        public String project { get; private set; }         //session's project 
        public String locationFolder { get; private set; }  //session location folder
        private bool edited;            //true if session is currently edited
        private List<VideoReader> videoReaders;
        private List<BaseDepthReader> depthReaders;
        internal SortedSet<String> filesList;
        private String metadataFile;      //parameters file name
        private String tempMetadataFile;
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
                        Console.WriteLine("Warning: Length inconsistence, original length = " + _sessionLength.Value + " ; new length = " + value);
                        _sessionLength = value;
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
        private bool loaded = false;
        private string commonPrefix;

        //Constructor
        public Session(String sessionName, String projectOwner, String locationFolder)
        {
            this.sessionName = sessionName;
            this.project = projectOwner;
            this.locationFolder = locationFolder;
            //If session file list exist load files list
            commonPrefix = locationFolder + Path.DirectorySeparatorChar + project + Path.DirectorySeparatorChar + sessionName + Path.DirectorySeparatorChar;
            metadataFile = commonPrefix + "files.param";
            tempMetadataFile = commonPrefix + "~files.param";

            resetVariables();
        }

        private void resetVariables()
        {
            this.filesList = new SortedSet<String>();
            this.edited = false;
            this.videoReaders = new List<VideoReader>();
            this.depthReaders = new List<BaseDepthReader>();
            this.events = new List<Event>();
            this.objects = new Dictionary<string, Object>();
        }

        internal void loadIfNotLoaded()
        {
            if (!loaded)
            {
                loadSession();
                loaded = true;
            }
        }

        internal void reloadAnnotation()
        {
            resetVariables();
            loadAnnotation();
            loaded = true;
        }

        //Add file to session filesList
        public void addFile(String fileName)
        {
            Console.WriteLine("Add file " + fileName);
            //1)check if file already exists in session files list
            bool exists = false;
            foreach (String file in filesList)
            {
                if (file == fileName)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists && !fileName.Contains("files.param"))
                filesList.Add(fileName);
            if (fileName.isVideoFile())
                addVideo(fileName);
            if (fileName.isDepthFile())
                addDepth(fileName);
        }

        public void removeFile(String fileName)
        {
            filesList.Remove(fileName);
            removeVideo(fileName);
            removeDepthVideo(fileName);
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
        }

        //Get objects list
        public List<Object> getObjects()
        {
            return objects.Values.ToList();
        }

        public Object getObject(string objectRefId)
        {
            return objects[objectRefId];
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
                        foreach (String fileName in filesList)
                        {
                            string tempoFileName = fileName;
                            if (fileName.Contains(Path.DirectorySeparatorChar))
                            {
                                // Remove prefix
                                if (fileName.Substring(0, commonPrefix.Length) == commonPrefix)
                                {
                                    tempoFileName = fileName.Substring(commonPrefix.Length);
                                }
                            }

                            writer.WriteElementString(FILE, tempoFileName);
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

                if (!File.Exists(metadataFile))
                {
                    File.Copy(tempMetadataFile, metadataFile, true);
                }
                else
                {
                    File.SetAttributes(metadataFile, FileAttributes.Normal);
                    File.Copy(tempMetadataFile, metadataFile, true);
                }

                //File.SetAttributes(metadataFile, FileAttributes.Normal);

                File.SetAttributes(tempMetadataFile, FileAttributes.Normal);
                File.Delete(tempMetadataFile);

                File.SetAttributes(metadataFile, FileAttributes.Hidden);
                //FileInfo myFile = new FileInfo(metadataFile);
                //myFile.Attributes = FileAttributes.Hidden;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// When the resources is scarce, we need to release some of video readers
        /// </summary>
        internal void releaseResources()
        {
            foreach (var v in videoReaders)
            {
                v.Dispose();
            }

            foreach (var v in depthReaders)
            {
                v.Dispose();
            }

            videoReaders = new List<VideoReader>();
            depthReaders = new List<BaseDepthReader>();

            loaded = false;
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

            Console.WriteLine(provider);
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
                catch (Exception e)
                {
                    duration = 0;
                }

                try
                {
                    startWriteRGB = DateTime.ParseExact(xmlDocument.DocumentElement.Attributes["startWriteRGB"].Value.Substring(0, xmlDocument.DocumentElement.Attributes["startWriteRGB"].Value.Length - 1), @"yyyy-MM-ddTHH:mm:ssss.ffffff", provider);
                }
                catch (Exception e)
                {
                    startWriteRGB = null;
                }

                Console.WriteLine("startWriteRGB " + startWriteRGB);

                XmlNode files = xmlDocument.DocumentElement.SelectSingleNode(FILES);

                foreach (XmlNode node in files.SelectNodes(FILE))
                {
                    string filename = node.InnerText;
                    filesList.Add(node.InnerText);
                    if (filename.isVideoFile())
                    {
                        addVideo(filename);
                    }

                    if (filename.isDepthFile())
                    {
                        addDepth(filename);
                    }
                }

                loadAnnotation();

                myFile.Attributes |= FileAttributes.Hidden;
            }
        }

        private void loadAnnotation()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(metadataFile);

                XmlNode objectsNode = xmlDocument.DocumentElement.SelectSingleNode(OBJECTS);
                objectCount = int.Parse(objectsNode.Attributes["no"].Value);
                List<Object> objects = Object.readFromXml(this, objectsNode);
                foreach (Object o in objects)
                {
                    addObject(o);
                }

                XmlNode annotationsNode = xmlDocument.DocumentElement.SelectSingleNode(ANNOTATIONS);
                events = Event.readFromXml(this, annotationsNode);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in loading annotation");
                Console.WriteLine(e);
            }
        }

        //Get video by index
        public VideoReader getVideo(int index)
        {
            if (index < 0 || index >= videoReaders.Count)
                return null;
            return videoReaders[index];
        }

        //Get depth by index
        public BaseDepthReader getDepth(int index)
        {
            if (index < 0 || index >= depthReaders.Count)
                return null;
            return depthReaders[index];
        }

        //Get video by filename
        public VideoReader getVideo(String fileName)
        {
            foreach (VideoReader v in videoReaders)
            {
                if (v.fileName.EndsWith(fileName))
                {
                    return v;
                }
            }
            return null;
        }

        //Get depth video by filename
        public BaseDepthReader getDepth(String fileName)
        {
            foreach (BaseDepthReader v in depthReaders)
            {
                if (v.fileName.EndsWith(fileName))
                {
                    return v;
                }
            }
            return null;
        }

        //Get video numbers
        public int videoCount()
        {
            return videoReaders.Count;
        }

        //Add video to session
        public void addVideo(String fileName)
        {
            bool exists = false;
            foreach (VideoReader v in videoReaders)
            {
                if (v.fileName.EndsWith(fileName))
                {
                    exists = true;
                    return;
                }
            }

            {
                VideoReader v = null;
                string fullFileName = getFullName(fileName);

                if (!File.Exists(fullFileName))
                {
                    Console.WriteLine("File " + fullFileName + " could not be found!!");
                    return;
                }

                v = new VideoReader(fullFileName, duration);

                videoReaders.Add(v);
                sessionLength = v.frameCount;
            }
        }

        //Remove video of session
        public void removeVideo(String fileName)
        {
            var v = getVideo(fileName);
            if (v != null)
            {
                videoReaders.Remove(v);
            }
        }

        //Remove video of session
        public void removeDepthVideo(String fileName)
        {
            var v = getDepth(fileName);
            if (v != null)
            {
                depthReaders.Remove(v);
            }
        }

        private string getFullName(string fileName)
        {
            string fullFileName = "";


            if (fileName.Contains(Path.DirectorySeparatorChar))
            {
                fullFileName = fileName;
            }
            else
            {
                // Try resolve it by adding the session location
                fullFileName = commonPrefix + fileName;
            }

            return fullFileName;
        }

        public void addDepth(String fileName)
        {
            bool exists = false;
            foreach (BaseDepthReader v in depthReaders)
            {
                Console.WriteLine(v.fileName);
                if (v.fileName.EndsWith(fileName))
                {
                    exists = true;
                    return;
                }
            }

            {
                BaseDepthReader v = null;

                string fullFileName = getFullName(fileName);

                if (!File.Exists(fullFileName))
                {
                    Console.WriteLine("File " + fullFileName + " could not be found!!");
                    return;
                }

                v = new BaseDepthReader(fullFileName);

                depthReaders.Add(v);
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
            foreach (String file in filesList)
            {
                if (file.isVideoFile() || file.isDepthFile())
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startFrame"></param>
        /// <param name="skipLength"></param>
        /// <param name="duration"></param>
        /// <param name="templateDescription"></param>
        /// <param name="overwriteMode"></param>
        /// <returns></returns>
        internal List<Event> addEventTemplate(int startFrame, int skipLength, int duration, string templateDescription,
            Options.OverwriteMode overwriteMode)
        {
            var addedEvents = new List<Event>();
            int noOfEventTemplate = (this.sessionLength - startFrame - duration) / skipLength + 1;

            /// Process before looping through generated event templates
            /// 
            switch (overwriteMode)
            {
                case Options.OverwriteMode.REMOVE_EXISTING:
                    this.events = new List<Event>();
                    break;
            }

            for (int i = 0; i < noOfEventTemplate; i++)
            {
                int start = startFrame + skipLength * i;
                int end = startFrame + skipLength * i + duration;

                Event e = new Event(null, start, end, templateDescription);

                // Some text preprocessing 
                foreach (var o in objects.Values)
                {
                    if (o.name != "")
                    {
                        if (templateDescription.IndexOf(o.name) != -1)
                        {
                            int startRef = templateDescription.IndexOf(o.name);
                            int endRef = startRef + o.name.Length - 1;
                            e.addReference(new Event.Reference(e, o.id, startRef, endRef));
                        }
                    }
                }

                switch (overwriteMode)
                {
                    case Options.OverwriteMode.ADD_SEPARATE:
                        this.addEvent(e);
                        addedEvents.Add(e);
                        break;
                    case Options.OverwriteMode.REMOVE_EXISTING:
                        this.addEvent(e);
                        addedEvents.Add(e);
                        break;
                    case Options.OverwriteMode.OVERWRITE:
                        foreach (Event ev in this.events)
                        {
                            if (ev.startFrame == e.startFrame && ev.endFrame == e.endFrame)
                            {
                                this.removeEvent(ev);
                                break;
                            }
                        }

                        this.addEvent(e);
                        addedEvents.Add(e);
                        break;
                    case Options.OverwriteMode.NO_OVERWRITE:
                        foreach (Event ev in this.events)
                        {
                            if (ev.startFrame == e.startFrame && ev.endFrame == e.endFrame)
                                break;
                        }
                        break;
                }

            }

            return addedEvents;
        }
    }
}
