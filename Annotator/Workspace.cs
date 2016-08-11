using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Annotator
{
    //Workspace class
    class Workspace
    {
        private List<Project> projects;
        private Dictionary<string, Project> projectNameToProject;
        public String locationFolder { get; } //workspace folder
        public bool defaultOption { get; } //default workspace folder option

        //Constructor
        public Workspace(String locationFolder, bool defaultOption)
        {
            this.locationFolder = locationFolder;
            this.defaultOption = defaultOption;
            projects = new List<Project>();
            projectNameToProject = new Dictionary<string, Project>();
        }

        /// <summary>
        /// Add a project into the work space
        /// </summary>
        /// <param name="projectName"> Name of project </param>
        /// <returns> Return the created project </returns>
        public Project addProject(String projectName)
        {
            Project project = new Project(locationFolder, projectName, null);
            //Create new project folder:
            Directory.CreateDirectory(locationFolder + Path.DirectorySeparatorChar + projectName);
            projects.Add(project);
            projectNameToProject[projectName] = project;
            return project;
        }

        //Return project name at given index
        public String getProjectName(int index)
        {
            if (index < 0 || index > projects.Count)
                return null;
            return projects[index].name;
        }

        //Return number of project in workspace
        public int getProjectCount()
        {
            return projects.Count;
        }

        //Get project by name
        public Project getProject(String projectName)
        {
            return projectNameToProject[projectName];
        }

        //Get project by index
        public Project getProject(int index)
        {
            if (index < 0 || index > projects.Count)
                return null;
            return projects[index];
        }

        internal void load()
        {
            // Load projects
            try
            {
                String[] projectNames = Directory.GetDirectories(this.locationFolder);

                foreach (String projectName in projectNames)
                {
                    String prjName = projectName.Split(Path.DirectorySeparatorChar)[projectName.Split(Path.DirectorySeparatorChar).Length - 1];
                    this.addProject(prjName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Load workspace failed! Directory might not exist.");
            }
        }
    }
}
