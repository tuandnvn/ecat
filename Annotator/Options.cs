using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    [DataContract]
    public class Options
    {
        public enum GlyphDetectionMode
        {
            ADD_SEPARATE,
            OVERWRITE,
            NO_OVERWRITE
        }

        internal static string TEMP_FILENAME = "~option.txt";
        internal static string FILENAME = "option.txt";

        [DataMember]
        internal string glyphPrototypePath = "";

        [DataMember]
        internal GlyphDetectionMode detectionMode;

        /// <summary>
        /// List of prototypes, should be set by reading from the glyphPrototypePath. otherwise set to default
        /// </summary>
        public List<GlyphBoxPrototype> prototypeList;


        public static Options loadOption()
        {
            Options options = getDefaultOption();
            try
            {
                var s = new DataContractSerializer(typeof(Options));

                FileStream fs = new FileStream(Options.FILENAME, FileMode.Open);
                options = (Options)s.ReadObject(fs);
                options.prototypeList = new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 };

                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return options;
        }

        public static Options getDefaultOption()
        {
            Options options = new Options();
            options.glyphPrototypePath = "";
            options.detectionMode = GlyphDetectionMode.ADD_SEPARATE;
            options.prototypeList = new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 };

            return options;
        }

        public void save()
        {
            var s = new DataContractSerializer(typeof(Options));

            using (FileStream fs = new FileStream(Options.TEMP_FILENAME, FileMode.Create))
            {
                try
                {
                    s.WriteObject(fs, this);
                    File.Copy(Options.TEMP_FILENAME, Options.FILENAME, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            try
            {
                File.Delete(Options.TEMP_FILENAME);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }
    }
}
