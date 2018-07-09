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
        public String path
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


        public Project(String path, String projectName)
        {
            this.path = path;
            this.sessions = sessions;
            this.name = projectName;
            this.sessions = new List<Session>();
            this.selected = false;
        }

        /// <summary>
        /// Get project session at given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add session to sessions list
        /// </summary>
        public void addSession(Session session)
        {
            if (session != null)
            {
                sessions.Add(session);
            }
        }

        /// <summary>
        /// Remove session from project:
        /// </summary>
        /// <param name="sessionName"></param>
        public Session removeSession(String sessionName)
        {
            foreach (Session s in sessions)
            {
                if (s.sessionName == sessionName)
                {
                    sessions.Remove(s);
                    return s;
                }
            }
            return null;
        }

        /// <summary>
        /// Get session by name
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        public Session getSession(String sessionName)
        {
            Session s = sessions.Find(t => t.sessionName.Equals(sessionName));

            return s;
        }
    }
}
