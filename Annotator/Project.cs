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
        internal List<Session> sessions;//project sessions

        /// <summary>
        /// project location folder == workspace folder
        /// </summary>
        public String locationFolder
        {
            get; set;
        }

        public String name
        {
            get; set;
        }

        public bool selected
        {
            get; set;
        } = false;


        //constructor
        public Project(String locationFolder, String projectName, List<Session> sessions)
        {
            this.locationFolder = locationFolder;
            this.sessions = sessions;
            this.name = projectName;
            if (this.sessions == null)
                this.sessions = new List<Session>();
            this.selected = false;
        }

        //Get project session at given index
        public Session getSession(int index)
        {
            if (sessions.Count > 0 && index >= 0 && index < sessions.Count)
            {
                return sessions[index];
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// Create a session inside project
        ///// </summary>
        ///// <param name="sessionName"> Session name </param>
        ///// <returns> Created session </returns>
        //public Session addSession(string sessionName)
        //{
        //    Session newSession = new Session(main, sessionName, getProjectName(), getLocation());
        //    sessions.Add(newSession);
        //    saveProjectFile(newSession.sessionName);

        //    return newSession;
        //}

        //Add session to sessions list
        public void addSession(Session session)
        {
            if (session != null)
            {
                sessions.Add(session);
                //MessageBox.Show(session.sessionName);
                //Update project file
                saveProjectFile(session.name);
            }
            else {

            }
        }
        //Check if session exists in project sessions:
        public bool checkSessionInProject(String sessionName)
        {
            //Check if project file exists:
            String projectFileName = locationFolder + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar + name + ".project";
            if (File.Exists(projectFileName))
            {
                StreamReader file = new StreamReader(projectFileName);
                String line = "";
                while ((line = file.ReadLine()) != null)
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
                if (s.name.Contains(sessionName))
                    return true;
            }
            return false;
        }
        //Save project parameters file
        public void saveProjectFile(String sessionName)
        {
            String projectFileName = locationFolder + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar + name + ".project";
            if (!File.Exists(projectFileName))
            {
                TextWriter tw = new StreamWriter(projectFileName);
                foreach (Session s in sessions)
                {
                    tw.WriteLine(s.name);
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
                if (s.name.Contains(sessionName))
                {
                    sessions.Remove(s);
                    break;
                }
            }
            //2)Update .project file
            String projectFileName = locationFolder + Path.DirectorySeparatorChar + name + Path.DirectorySeparatorChar + name + ".project";
            try
            {
                TextWriter tw = new StreamWriter(projectFileName);
                foreach (Session s in sessions)
                {
                    tw.WriteLine(s.name);
                }
                tw.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //Get session by name
        public Session getSession(String sessionName)
        {
            Session s = sessions.Find(t => t.name.Equals(sessionName));

            return s;
        }
    }
}
