using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class PolygonObject : Object
    {
        public PolygonObject(Session session, String id, Color color, int borderSize, string videoFile) : base(session, id, color, borderSize, videoFile)
        {

        }
    }
}
