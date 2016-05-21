using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    interface LearningDataGenerator
    {
        /// <summary>
        /// From a project, extract data into an object
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        object extractDataFromProject(Project project);

        /// <summary>
        /// From a project, write down the extracted data into a file
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        void writeExtractedDataIntoFile(Project project, string filename);
    }
}
