using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public abstract class LocationMark : ObjectMark
    {
        public LocationMark(int frameNo) : base(frameNo)
        {
        }

        public virtual void writeToXml(XmlWriter xmlWriter)
        {
        }

        public virtual void readFromXml(XmlNode xmlNode)
        {
        }

    }

}
