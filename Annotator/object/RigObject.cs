using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class RigObject : Object
    {
        public RigObject(Session currentSession, String id, Color color, int borderSize, string videoFile) : base(currentSession, id, color, borderSize, videoFile)
        {
            this.objectType = ObjectType._3D;
        }

        public void setBounding(int frameNumber, RigFigure<PointF> boundingRig, float scale, Point translation)
        {
            RigFigure<PointF> inverseScaleBoundingRig = boundingRig.scaleBound(1 / scale, new PointF((float)(-translation.X / scale), (float)(-translation.Y / scale)));
            var ob = new RigLocationMark2D(frameNumber, inverseScaleBoundingRig);
            objectMarks[frameNumber] = ob;
        }

        protected override void writeLocationMark(XmlWriter xmlWriter)
        {
            // No need to write location mark for rig object
        }

        protected override void write3DLocationMark(XmlWriter xmlWriter)
        {
            // No need to write location mark for rig object
        }

        protected override void loadObjectAdditionalFromXml(XmlNode objectNode)
        {
            string sourceScheme = this.otherProperties["sourceScheme"];
            string source = this.otherProperties["source"];
            int rigIndex = int.Parse(this.otherProperties["rigIndex"]);

            if (!source.Contains(Path.DirectorySeparatorChar))
            {
                source = session.locationFolder + Path.DirectorySeparatorChar + session.project + Path.DirectorySeparatorChar + session.sessionName + Path.DirectorySeparatorChar + source;
            }

            if (!sourceScheme.Contains(Path.DirectorySeparatorChar))
            {
                sourceScheme = session.locationFolder + Path.DirectorySeparatorChar + session.project + Path.DirectorySeparatorChar + session.sessionName + Path.DirectorySeparatorChar + sourceScheme;
            }

            if (!File.Exists(source) || !File.Exists(sourceScheme))
            {
                // It's probably a bug from recording, that rig source and scheme path is absolute path from recording machine
                // Get the relative path and assume the file in inside the session
                source = source.Split(Path.DirectorySeparatorChar)[source.Split(Path.DirectorySeparatorChar).Length - 1];
                source = session.locationFolder + Path.DirectorySeparatorChar + session.project + Path.DirectorySeparatorChar + session.sessionName + Path.DirectorySeparatorChar + source;

                sourceScheme = sourceScheme.Split(Path.DirectorySeparatorChar)[sourceScheme.Split(Path.DirectorySeparatorChar).Length - 1];
                sourceScheme = session.locationFolder + Path.DirectorySeparatorChar + session.project + Path.DirectorySeparatorChar + session.sessionName + Path.DirectorySeparatorChar + sourceScheme;
            }

            if (File.Exists(source) && File.Exists(sourceScheme))
            {
                this.otherProperties["sourceScheme"] = sourceScheme.Split(Path.DirectorySeparatorChar)[sourceScheme.Split(Path.DirectorySeparatorChar).Length - 1];
                this.otherProperties["source"] = source.Split(Path.DirectorySeparatorChar)[source.Split(Path.DirectorySeparatorChar).Length - 1];
            }

            Rigs.loadDataForRig(source, sourceScheme, rigIndex, this);
        }
    }
}
