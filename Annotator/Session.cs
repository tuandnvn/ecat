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
        internal const string PREDICATES = "predicates";
        internal const string PREDICATE = "predicate";
        internal const string ARGUMENTS = "arguments";
        internal const string IDS = "ids";

        public List<Event> events { get; private set; }

        /// <summary>
        /// Id for next object added into this session
        /// </summary>
        public int nextObjectId { get; set; } = 0;          
        private Dictionary<string, Object> objects;  // list of objects in videos
        public String name { get; private set; }     //session name
        public Project project { get; private set; }         //session's project 
        public bool edited { get; set; }            //true if session is currently edited
        private List<VideoReader> videoReaders;
        private List<BaseDepthReader> depthReaders;
        internal SortedSet<String> filesList;
        public SortedSet<PredicateMark> predicates { get; private set;  }
        private String metadataFile;      //parameters file name
        private String tempMetadataFile;
        private int annotationID;      // annotation ID
        private Lazy<int> _sessionLength;

        private long lastOpenTime;

        public int sessionLength
        {
            get
            {
                return _sessionLength.Value;
                //if (_sessionLength.IsValueCreated)
                //    return _sessionLength.Value;
                //return 0;
            }
        }

        public DateTime? startWriteRGB { get; set; }

        /// <summary>
        /// Different from sessionLength
        /// Duration could be the time to record the video
        /// </summary>
        public long duration { get; set; }

        public enum LOAD_STATUS
        {
            NOT_LOADED,
            LOADING,
            LOADED,
        }

        private LOAD_STATUS loaded = LOAD_STATUS.NOT_LOADED;

        internal string path;

        //Constructor
        public Session(String sessionName, Project project, String workspaceDir)
        {
            this.name = sessionName;
            this.project = project;
            //this.workspaceDir = workspaceDir;
            //If session file list exist load files list
            path = workspaceDir + Path.DirectorySeparatorChar + project.name + Path.DirectorySeparatorChar + sessionName + Path.DirectorySeparatorChar;
            metadataFile = path + "files.param";
            tempMetadataFile = path + "~files.param";
            resetVariables();

            // Default
            lastOpenTime = -1;
        }

        public void resetLastOpenTime()
        {
            lastOpenTime = Environment.TickCount;
        }

        private void resetVariables()
        {
            this.filesList = new SortedSet<String>();
            this.videoReaders = new List<VideoReader>();
            this.depthReaders = new List<BaseDepthReader>();
            resetAnnotationVariables();
        }

        private void resetAnnotationVariables()
        {
            // Start counting object from 0
            this.nextObjectId = 0;
            this.events = new List<Event>();
            this.objects = new Dictionary<string, Object>();
            this.predicates = new SortedSet<PredicateMark>(new PredicateMarkComparer());
        }

        internal void loadIfNotLoaded()
        {
            if (loaded == LOAD_STATUS.NOT_LOADED)
            {
                reload();
            }
        }

        internal void reload()
        {
            loaded = LOAD_STATUS.LOADING;
            resetVariables();
            loadSession();
            loaded = LOAD_STATUS.LOADED;
        }

        /// <summary>
        /// Remove all annotation 
        /// but remains the files added
        /// </summary>
        internal void resetAnnotation()
        {
            resetAnnotationVariables();
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
            if (o.id == "" || o.id == null) { o.id = "o" + ++nextObjectId; }
            objects[o.id] = o;
        }

        //Get objects list
        public List<Object> getObjects()
        {
            return objects.Values.ToList();
        }

        public Object getObject(string objectRefId)
        {
            try
            {
                return objects[objectRefId];
            }
            catch (Exception)
            {
                return null;
            }
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
                    writer.WriteAttributeString("name", name);
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
                                if (fileName.Substring(0, path.Length) == path)
                                {
                                    tempoFileName = fileName.Substring(path.Length);
                                }
                            }

                            writer.WriteElementString(FILE, tempoFileName);
                        }
                        writer.WriteEndElement();
                    }
                    saveAnnotation(writer);

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
                MessageBox.Show("Exception when writing session " + e.ToString());
            }
        }

        private void saveAnnotation(XmlWriter writer)
        {
            {
                writer.WriteStartElement(OBJECTS);
                writer.WriteAttributeString("no", nextObjectId + "");
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

            // Write predicate instead of writing links
            {
                writer.WriteStartElement(PREDICATES);
                foreach (var predicate in predicates)
                {
                    predicate.writeToXml(writer);
                }
                writer.WriteEndElement();
            }
        }

        //Load files list
        public void loadSession()
        {
            try
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

                    _sessionLength = new Lazy<int>(() => int.Parse(xmlDocument.DocumentElement.Attributes["length"].Value));

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

                    loadAnnotation(xmlDocument);

                    myFile.Attributes |= FileAttributes.Hidden;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        /// <summary>
        /// Clear out the current session, and load the content of 
        /// xmlDocument into annotation metadata.
        /// </summary>
        /// <param name="xmlDocument"></param>
        private void loadAnnotation(XmlDocument xmlDocument)
        {
            try
            {
                {
                    XmlNode objectsNode = xmlDocument.DocumentElement.SelectSingleNode(OBJECTS);
                    this.objects = new Dictionary<string, Object>();

                    if (objectsNode != null)
                    {
                        nextObjectId = int.Parse(objectsNode.Attributes["no"].Value);
                        var tempoObjects = Object.readObjectsFromXml(this, objectsNode);

                        foreach (Object o in tempoObjects)
                        {
                            this.addObject(o);
                        }
                    }
                }

                //// Load predicate
                {
                    XmlNode predicatesNode = xmlDocument.DocumentElement.SelectSingleNode(PREDICATES);
                    predicates = new SortedSet<PredicateMark>(new PredicateMarkComparer());
                    if (predicatesNode != null)
                    {
                        foreach (XmlNode predicateNode in predicatesNode.SelectNodes(PREDICATE))
                        {
                            var pm = PredicateMark.loadFromXml(this, predicateNode);
                            if (pm != null)
                                predicates.Add(pm);
                        }
                    }
                }

                {
                    XmlNode annotationsNode = xmlDocument.DocumentElement.SelectSingleNode(ANNOTATIONS);
                    events = new List<Event>();

                    if (annotationsNode != null)
                        events = Event.readFromXml(this, annotationsNode);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in loading annotation");
                Console.WriteLine(e);
                throw e;
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

            loaded = LOAD_STATUS.NOT_LOADED;
        }

        internal void removeObject(string objectId)
        {
            var o = objects[objectId];
            objects.Remove(objectId);

            // Remove predicate relate to this object
            predicates.RemoveWhere(predicate => Array.IndexOf(predicate.objects, o) != -1);
        }

        internal void removePredicate(string predicateStrForm)
        {
            predicates.RemoveWhere(t => t.ToString().Equals(predicateStrForm));
        }

        /// <summary>
        /// Query predicate that take o as the 'subject' argument
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        internal SortedList<int, LinkMark> queryLinkMarks(Object o)
        {
            var linkMarks = new SortedList<int, LinkMark>();

            foreach (var predicate in predicates)
            {
                var indexInObjects = Array.IndexOf(predicate.objects, o);
                var indexOfX = Array.IndexOf(predicate.predicate.combination.values, 1);
                if (indexInObjects != -1 && indexOfX != -1 && indexInObjects == indexOfX)
                {
                    if (!linkMarks.ContainsKey(predicate.frame))
                    {
                        linkMarks[predicate.frame] = new LinkMark(o, predicate.frame);
                    }
                    linkMarks[predicate.frame].addPredicate(predicate);
                }
            }

            return linkMarks;
        }

        internal void addPredicate(int frameNumber, bool qualified, Predicate predicate, Object[] os)
        {
            PredicateMark pm = new PredicateMark(frameNumber, qualified, predicate, this, os);

            // Remove the previously added predicate at the same frame that is conflict
            predicates.RemoveWhere(m => Options.getOption().predicateConstraints.Any(constraint => constraint.isConflict(m, pm) && m.frame == frameNumber));

            predicates.Add(pm);
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
                _sessionLength = new Lazy<int> (() => v.frameCount );
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
                fullFileName = path + fileName;
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
        internal void addEvent(Event e)
        {
            if (events.Find(ev => ev.id == e.id) != null)
                return;

            events.Add(e);
            e.id = "a" + ++annotationID;
        }

        /// <summary>
        /// Match the object names with text span in text description of event
        /// </summary>
        /// <param name="e"></param>
        internal void findObjectsByNames(Event e)
        {
            if (events.Find(ev => ev.id == e.id) == null)
                return;

            // Some text preprocessing 
            foreach (var o in objects.Values)
            {
                if (o.name != "")
                {
                    if (e.text.IndexOf(o.name) != -1)
                    {
                        int startRef = e.text.IndexOf(o.name);
                        int endRef = startRef + o.name.Length - 1;
                        e.addTempoReference(startRef, endRef, o.id);
                    }
                }
            }
        }

        /// <summary>
        /// Match the object names with text span in text description of all events
        /// </summary>
        internal void findObjectsByNames()
        {
            foreach (Event e in events)
            {
                resetTempoEmpty(e);
                findObjectsByNames(e);
                e.save();
            }
        }

        internal void resetTempo(Event e)
        {
            // Event must exist
            if (events.Find(ev => ev.id == e.id) == null)
                return;

            e.resetTempo();
        }

        internal void resetTempoEmpty(Event e)
        {
            // Event must exist
            if (events.Find(ev => ev.id == e.id) == null)
                return;

            e.resetTempoToEmpty();
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

                switch (overwriteMode)
                {
                    case Options.OverwriteMode.ADD_SEPARATE:
                        this.addEvent(e);
                        this.findObjectsByNames(e);
                        addedEvents.Add(e);
                        break;
                    case Options.OverwriteMode.REMOVE_EXISTING:
                        this.addEvent(e);
                        this.findObjectsByNames(e);
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
                        this.findObjectsByNames(e);
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

        public MemoryStream saveToMemento()
        {
            MemoryStream ms = new MemoryStream();

            using (XmlWriter writer = XmlWriter.Create(ms))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement(SESSION);
                saveAnnotation(writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            ms.Flush();

            //var sr = new StreamReader(ms);
            //var myStr = sr.ReadToEnd();
            //Console.WriteLine(myStr);
            //Console.WriteLine(ms.ToString());

            return ms;
        }

        public void restoreFromMemento(MemoryStream memento)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memento);

            loadAnnotation(xmlDoc);
        }
    }
}
