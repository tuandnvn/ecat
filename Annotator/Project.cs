using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace Annotator
{
    public class Project
    {
        //constructor
        public Project(String locationFolder, String projectName, List<Session> sessions){
            this.locationFolder = locationFolder;
            this.sessions = sessions;
            this.projectName = projectName;
            if(this.sessions == null)
                this.sessions = new List<Session>();
            this.selected = false;
        }
        //Get project session at given index
        public Session getSession(int index){
            if (sessions.Count > 0 && index >= 0 && index < sessions.Count)
            {
                return sessions[index];
            }
            else
            {
                return null;
            }
        }
        //Add session to sessions list
        public void addSession(Session session){
            if(session != null){
                sessions.Add(session);
                //MessageBox.Show(session.getSessionName());
                //Update project file
                saveProjectFile(session.getSessionName());
            }
            else{

            }
        }
        //Check if session exists in project sessions:
        public bool checkSessionInProject(String sessionName)
        {
            //Check if project file exists:
            String projectFileName = locationFolder + "\\" + projectName + "\\" + projectName + ".project";
            if (File.Exists(projectFileName))
            {
                StreamReader file = new StreamReader(projectFileName);
                String line = "";
                while (( line = file.ReadLine()) != null)
                {
                    if (line.Contains(sessionName))
                    {
                        file.Close();
                        return true;
                    }
                }
                file.Close();
            }

            foreach (Session s in sessions)
            {
                if (s.getSessionName().Contains(sessionName))
                    return true;
            }
            return false;
        }
        //Save project parameters file
        public void saveProjectFile(String sessionName)
        {
            String projectFileName = locationFolder + "\\" + projectName + "\\" + projectName + ".project";
                if(!File.Exists(projectFileName)){   
                    TextWriter tw = new StreamWriter(projectFileName);
                    foreach (Session s in sessions)
                    {
                        tw.WriteLine(s.getSessionName());
                    }
                    tw.Close();
                }
                else if (File.Exists(projectFileName))
                {
                    //1)Check if session already exists in project file:
                    bool exists = false;
                    StreamReader file = new StreamReader(projectFileName);
                    String line = "";
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line.Contains(sessionName))
                        {
                            file.Close();
                            exists = true;
                            break;
                        }
                    }
                    file.Close();
                    if (!exists)
                    {
                        TextWriter tw = new StreamWriter(projectFileName, true);
                        tw.WriteLine(sessionName);
                        tw.Close();
                    }
                    
                }
        }
        //Remove session from project:
        public void removeSession(String sessionName)
        {
            //1)Remove session from sessions list
            foreach (Session s in sessions)
            {
                if(s.getSessionName().Contains(sessionName)){
                    sessions.Remove(s);
                    break;
                }
            }
            //2)Update .project file
            String projectFileName = locationFolder + "\\" + projectName + "\\" + projectName + ".project";
            try
            {
                TextWriter tw = new StreamWriter(projectFileName);
                foreach (Session s in sessions)
                {
                    tw.WriteLine(s.getSessionName());
                }
                tw.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        //Set project name
        public void setProjectName(String projectName)
        {
            this.projectName = projectName;
        }
        //Get project name
        public String getProjectName()
        {
            return projectName;
        }
        //Get project location == workspace folder
        public String getLocation()
        {
            return locationFolder;
        }
        //Get session by name
        public Session getSession(String sessionName)
        {
            foreach (Session s in sessions)
            {
                if (s.getSessionName().Contains(sessionName))
                    return s;
            }
            return null;
        }
        //Get number of sessions in project
        public int getSessionN()
        {
            return sessions.Count;
        }
        //Set selected
        public void setSelected(bool option)
        {
            this.selected = option;
        }
        //Get selected
        public bool getSelected()
        {
            return selected;
        }
        private List<Session> sessions;//project sessions
        private String locationFolder; //project location folder == workspace folder
        private String projectName;    //project name
        private bool selected = false;  //true if project selected
    }
}
