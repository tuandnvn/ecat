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
        public enum OverwriteMode
        {
            ADD_SEPARATE,
            OVERWRITE,
            NO_OVERWRITE,
            REMOVE_EXISTING
        }

        public enum ShowRig
        {
            SHOW_ALL,
            SHOW_UPPER
        }

        public enum InterpolationMode
        {
            LEFT_COPY,
            LINEAR
        }

        internal const string RIG = "Rig";
        internal const string RECTANGLE = "Rectangle";
        internal const string GLYPH = "Glyph";
        internal static string TEMP_FILENAME = "~option.txt";
        internal static string FILENAME = "option.txt";

        [DataMember]
        internal string glyphPrototypePath = "";

        [DataMember]
        internal OverwriteMode detectionMode;

        [DataMember]
        internal ShowRig showRigOption;

        [DataMember]
        internal Dictionary<string, InterpolationMode> interpolationModes;

        [DataMember]
        internal List<Predicate> objectPredicates;



        /// <summary>
        /// List of prototypes, should be set by reading from the glyphPrototypePath. otherwise set to default
        /// </summary>
        public List<GlyphBoxPrototype> prototypeList { get; private set; }


        private static Options singletonOptions;

        public static Options getOption()
        {
            if (singletonOptions == null)
            {
                Options tempOptions = createDefaultOption();
                try
                {
                    var s = new DataContractSerializer(typeof(Options));

                    FileStream fs = new FileStream(Options.FILENAME, FileMode.Open);
                    singletonOptions = (Options)s.ReadObject(fs);

                    if (singletonOptions.prototypeList == null)
                        singletonOptions.prototypeList = tempOptions.prototypeList;

                    if (singletonOptions.interpolationModes == null)
                        singletonOptions.interpolationModes = tempOptions.interpolationModes;

                    if (singletonOptions.objectPredicates == null)
                        singletonOptions.objectPredicates = tempOptions.objectPredicates;

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
            options.detectionMode = OverwriteMode.ADD_SEPARATE;
            options.prototypeList = new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 };
            options.showRigOption = ShowRig.SHOW_ALL;
            options.interpolationModes = new Dictionary<string, InterpolationMode>();
            options.interpolationModes[RIG] = InterpolationMode.LEFT_COPY;
            options.interpolationModes[RECTANGLE] = InterpolationMode.LEFT_COPY;
            options.interpolationModes[GLYPH] = InterpolationMode.LEFT_COPY;
            options.objectPredicates = new List<Predicate>();

            return options;
        }

        public void save()
        {
            var s = new DataContractSerializer(typeof(Options));

            var settings = new XmlWriterSettings { Indent = true };

            //using (FileStream fs = new FileStream(Options.TEMP_FILENAME, FileMode.Create))
            using (var fs = XmlWriter.Create(Options.TEMP_FILENAME, settings))
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
