using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Annotator
{
    //Workspace class
    class Workspace
    {
        //Constructor
        public Workspace()
        {
            locationFolder = "";
        }
        //Set default option
        public void setDefaulOption(bool option)
        {
            this.defaultOption = option;
        }
        //Get defaultOption
        public bool getDefaultOption()
        {
            return defaultOption;
        }
        //Set locationFolder
        public void setLocationFolder(String locationFolder)
        {
            this.locationFolder = locationFolder;
        }
        //Get locationFolder
        public String getLocationFolder()
        {
            return locationFolder;
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
            return project;
        }
        
        //Return project name at given index
        public String getProjectName(int index)
        {
            if (index < 0 || index > projects.Count)
                return null;
            return projects[index].getProjectName();
        }
        //Return number of project in workspace
        public int getProjectsSize()
        {
            return projects.Count;
        }

        //Get project by name
        public Project getProject(String projectName)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                if (projects[i].getProjectName().Contains(projectName))
                    return projects[i];
            }

            return null;
        }
        private List<Project> projects = new List<Project>();
        private String locationFolder;//workspace folder
        private bool   defaultOption ;//default workspace folder option
    }
}
