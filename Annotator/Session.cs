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
namespace Annotator
{
    //Session class
    public class Session
    {
        //Constructor
        public Session(String sessionName, String projectOwner, String locationFolder, Form1 frm1)
        {
            
            this.sessionName = sessionName;
            this.filesList = new List<String>();
            this.edited     = false;
            this.videos = new List<Video>();
            this.project = projectOwner;
            this.locationFolder = locationFolder;
            this.frm1 = frm1;
            //If session file list exist load files list            
            paramFile = locationFolder + "\\" + project + "\\" + sessionName + "\\files.param";
            loadFilesList();
        }
        //Add file to session filesList
        public void addFile(String fileName)
        {
            try
            {
                //1)check if file already exists in session files list
                bool exists = false;
                foreach (String file in filesList)
                {
                    if(file.Contains(fileName)){
                        exists = true;
                        break;
                    }
                }

                if(!exists && !fileName.Contains("files.param"))
                    filesList.Add(fileName);
                if (fileName.Contains(".avi"))
                    AddVideo(fileName);
                //MessageBox.Show(filesList.Count + "");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        //Get session name
        public String getSessionName()
        {
            return sessionName;
        }
        //Save session
        public void saveSession()
        {
           try
            {
               if(File.Exists(paramFile)){
                   FileInfo myFile1 = new FileInfo(paramFile);
                    myFile1.Attributes &= ~FileAttributes.Hidden;
               }
               TextWriter tw = new StreamWriter(paramFile);
               //1)Save files in filesList
               foreach (String file in filesList)
               {
                   tw.WriteLine(file);
                   //MessageBox.Show(file);
               }
               //2)Save videos and objects connected to this videos:
               foreach (Video v in videos)
               {
                   foreach (Object o in v.getObjects())
                   {
                       String line = "OBJECT: " + v.getFileName() + "|" + o.getID() + "|" + o.getColor().ToString() + "|" + o.getType() + "|" + o.getBoundingBox().X + "|" + o.getBoundingBox().Y + "|" + o.getBoundingBox().Width + "|" + o.getBoundingBox().Height + "|" + o.getBorderSize() + "|" + o.getStartFrame() + "|" + o.getEndFrame() + "|" + o.getScale();
                       tw.WriteLine(line);
                   }
                   foreach (Annotation a in v.getAnnotations())
                   {
                       String line = "ANNOTATION: " + a.getID() + "|" + a.getStartFrame() + "|" + a.getEndFrame() + "|" + a.getText() + "|" + a.getVideoName();
                       tw.WriteLine(line);
                       foreach (String reference in a.getReferences())
                       {
                           tw.WriteLine("ANNOTATION REFERENCE: " + reference);
                       }
                   }
                   
               }
               tw.Close();
               FileInfo myFile = new FileInfo(paramFile);
               myFile.Attributes = FileAttributes.Hidden;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
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
        public void loadFilesList(){
            
            //MessageBox.Show(fileName);
            //Check if file exists
            if(File.Exists(paramFile)){
                //Set file as hidden                
                FileInfo myFile = new FileInfo(paramFile);
                // Remove the hidden attribute of the file
                myFile.Attributes &= ~FileAttributes.Hidden;

                //Read file line by line
                
                string line;

                // Read the file and display it line by line.
                System.IO.StreamReader file =
                   new System.IO.StreamReader(paramFile);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("OBJECT:") == false && line.Contains("ANNOTATION:") == false && line.Contains("ANNOTATION REFERENCE:") == false)
                    {
                        //MessageBox.Show(line + ", " + line.Contains("ANNOTATION REFERENCE:"));
                        filesList.Add(line);
                        if (line.Contains(".avi"))
                            videos.Add(new Video(line));
                    }
                    else if (line.Contains("OBJECT:"))
                    {
                        //Add object to video:
                        String[] parameters = line.Substring(7).TrimStart().Split('|');
                        String videoName = parameters[0];
                        int objectID  = Convert.ToInt32(parameters[1]);                        
                        String color = parameters[2];
                        Color cBuild;
                        if (!color.Contains("="))
                        {
                            color = color.Substring(6).Replace('[', ' ').Replace(']', ' ').TrimStart().TrimEnd();
                            //MessageBox.Show("c=" + color);
                            cBuild = Color.FromName(color);
                        }
                        else
                        {
                            color = color.Substring(6).Replace('[', ' ').Replace(']', ' ').TrimStart().TrimEnd();
                            String[] argb = color.Split(',');
                            int a, r, g, b;                            
                            a = Convert.ToInt32(argb[0].Substring(2));                            
                            r = Convert.ToInt32(argb[1].Substring(3));                     
                            g = Convert.ToInt32(argb[2].Substring(3));
                            b = Convert.ToInt32(argb[3].Substring(3));
                            //MessageBox.Show(a + " " + r + " " + g + " " + b);
                            cBuild = Color.FromArgb(a, r, g, b);
                        }
                        
                        String type = parameters[3];
                        int x = Convert.ToInt32(parameters[4]);
                        int y = Convert.ToInt32(parameters[5]);
                        int w = Convert.ToInt32(parameters[6]);
                        int h = Convert.ToInt32(parameters[7]);
                        int s = Convert.ToInt32(parameters[8]);
                        int startFrame = Convert.ToInt32(parameters[9]);
                        int endFrame = Convert.ToInt32(parameters[10]);
                        double scale = Convert.ToDouble(parameters[11]);
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
                            v.addObject(new Object(objectID, cBuild, type, (startFrame - 1), (endFrame - 1), new Rectangle(x, y, w, h), s, scale));
                        }
                    }
                    else if (line.Contains("ANNOTATION:") && !(line.Contains("ANNOTATION REFERENCE:")))
                    {
                        
                        String[] parameters = line.Substring(11).TrimStart().Split('|');
                        //MessageBox.Show(parameters[0]);
                        int aID = Convert.ToInt32(parameters[0]);
                        int startFrame = Convert.ToInt32(parameters[1]);
                        int endFrame = Convert.ToInt32(parameters[2]);
                        String text = parameters[3];
                        String videoName = parameters[4];
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
                            v.addAnnotation(new Annotation(startFrame, endFrame, text, this.frm1, aID, videoName));                            
                        }
                    }
                    if (line.Contains("ANNOTATION REFERENCE:") && !(line.Contains("OBJECT:")))
                    {
                        String[] parameters = line.Substring(21).TrimStart().Split('|');                  
                        int id = Convert.ToInt32(parameters[0]);
                        String videoName = parameters[6];                        
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
                            //MessageBox.Show("OK");
                            foreach (Annotation a in v.getAnnotations())
                            {
                                if (a.getID() == id)
                                {
                                    a.addReference(line.Substring(21));
                                }
                            }
                        }
                    }
                }
                
                file.Close();
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
        public void AddVideo(String fileName)
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
                videos.Add(new Video(fileName));
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
            List<String> viewsL =  new List<String>();
            //MessageBox.Show(filesList.Count + "");
            foreach(String file in filesList){
                //MessageBox.Show(file);
            if(file.Contains(".avi"))
                viewsL.Add(file.Split('\\')[file.Split('\\').Length-1]);
            }
            return viewsL.ToArray();
        }
        private String sessionName;     //session name
        private String project;         //session's project 
        private String locationFolder;  //session location folder
        private bool edited;            //true if session is currently edited
        private List<Video> videos;
        private List<String> filesList;
        private String paramFile;      //parameters file name
        private Form1 frm1;
    }
}
