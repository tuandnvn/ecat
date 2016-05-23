using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class VoxMLReader
    {
        public string directory { get;  }
        private Dictionary<string, VoxMLType> voxMLTypes;

        // singleton
        private static VoxMLReader voxMLReader;

        private VoxMLReader ( string directory )
        {
            this.directory = directory;

            voxMLTypes = new Dictionary<string, VoxMLType>();

            String[] fileNames = Directory.GetFiles(directory);

            foreach ( String fileName in fileNames )
            {
                var voxMLType = readVoxMLFile(fileName);

                if (voxMLType.HasValue)
                {
                    voxMLTypes[voxMLType.Value.pred] = voxMLType.Value;
                }
            }
        }

        public static VoxMLReader getDefaultVoxMLReader()
        {
            if ( voxMLReader == null )
                voxMLReader = new VoxMLReader("VoxML");
            return voxMLReader;
        }

        public VoxMLType? getVoxMLType(String pred)
        {
            if (voxMLTypes.ContainsKey(pred))
                return voxMLTypes[pred];
            return null;
        }

        private VoxMLType? readVoxMLFile( string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            try
            {
                string pred = xmlDocument.SelectSingleNode("Lex").SelectSingleNode("Pred").InnerText;
                string[] types = xmlDocument.SelectSingleNode("Lex").SelectSingleNode("Type").InnerText.Split('*');
                string concavity = xmlDocument.SelectSingleNode("Type").SelectSingleNode("Concavity").InnerText;

                var voxMLType = new VoxMLType { pred = pred, types = types, concavity = concavity };
                return voxMLType;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    public struct VoxMLType
    {
        public string pred;
        public string[] types;
        public string concavity;
    }
}
