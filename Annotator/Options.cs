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

        public enum ShowRig
        {
            SHOW_ALL,
            SHOW_UPPER
        }

        internal static string TEMP_FILENAME = "~option.txt";
        internal static string FILENAME = "option.txt";

        [DataMember]
        internal string glyphPrototypePath = "";

        [DataMember]
        internal GlyphDetectionMode detectionMode;

        [DataMember]
        internal ShowRig showRigOption;

        /// <summary>
        /// List of prototypes, should be set by reading from the glyphPrototypePath. otherwise set to default
        /// </summary>
        public List<GlyphBoxPrototype> prototypeList;


        public static Options singletonOptions;

        public static Options getOption()
        {
            if (singletonOptions == null)
            {
                singletonOptions = createDefaultOption();
                try
                {
                    var s = new DataContractSerializer(typeof(Options));

                    FileStream fs = new FileStream(Options.FILENAME, FileMode.Open);
                    singletonOptions = (Options)s.ReadObject(fs);

                    if (singletonOptions.prototypeList == null)
                        singletonOptions.prototypeList = new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 };

                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return singletonOptions;
        }

        /// <summary>
        /// Change the singleton options to default and return
        /// </summary>
        /// <returns></returns>
        public static Options makeDefaultOption()
        {
            singletonOptions = createDefaultOption();
            return singletonOptions;
        }

        /// <summary>
        /// CAUTIONS: This one create another Option object, so it should be private
        /// </summary>
        /// <returns></returns>
        private static Options createDefaultOption()
        {
            Options options = new Options();
            options.glyphPrototypePath = "";
            options.detectionMode = GlyphDetectionMode.ADD_SEPARATE;
            options.prototypeList = new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 };
            options.showRigOption = ShowRig.SHOW_ALL;

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
