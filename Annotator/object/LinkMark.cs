using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class LinkMark : ObjectMark
    {
        public string objectId { get; }

        // A set of link to other objects at a certain frame
        // Each link is of < objectID , qualified, spatialLink >
        public SortedSet<Tuple<string, bool, string>> binaryPredicates { get; } // By default, there is no spatial configuration attached to an object location

        // A set of link to other objects of different session at a certain frame
        // Each link is of < sessionName, objectID , qualified, spatialLink >
        public SortedSet<Tuple<string, string, bool, string>> crossSessionBinaryPredicates { get; } // By default, there is no spatial configuration attached to an object location

        public LinkMark(string objectId, int frameNo) : base(frameNo)
        {
            this.objectId = objectId;
            binaryPredicates = new SortedSet<Tuple<string, bool, string>>();
            crossSessionBinaryPredicates = new SortedSet<Tuple<string, string, bool, string>>();
        }

        public void addLinkToObject(string otherObjectId, bool qualified, string linkType)
        {
            binaryPredicates.Add(new Tuple<string, bool, string>(otherObjectId, qualified, linkType));
        }

        public void addLinkToObject(string sessionName, string otherObjectId, bool qualified, string linkType)
        {
            crossSessionBinaryPredicates.Add(new Tuple<string, string, bool, string>(sessionName, otherObjectId, qualified, linkType));
        }

        override public String ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (binaryPredicates.Count != 0)
            {
                sb.Append(String.Join(",", binaryPredicates.Select(u => getLiteralForm(u))));
                if (crossSessionBinaryPredicates.Count != 0)
                    sb.Append("\n");
            }

            if (crossSessionBinaryPredicates.Count != 0)
                sb.Append(String.Join(",", crossSessionBinaryPredicates.Select(u => getLiteralForm(u))));
            return sb.ToString();
        }

        public String getLiteralForm(Tuple<string, bool, string> t)
        {
            String q = t.Item3 + "( " + objectId + ", " + t.Item1 + " )";
            if (!t.Item2)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }

        public String getLiteralForm(Tuple<string, string, bool, string> t)
        {
            String q = t.Item4 + "( " + objectId + ", " + t.Item1 + "/" + t.Item2 + ")";
            if (!t.Item3)
            {
                q = "NOT( " + q + " )";
            }
            return q;
        }

        internal void loadFromHtml(XmlNode linkNode)
        {
            foreach (XmlNode linkto in linkNode.SelectNodes(Object.LINKTO))
            {
                try
                {
                    String linkToObjectId = linkto.Attributes[Object.ID].Value;
                    bool qualified = bool.Parse(linkto.Attributes[Object.QUALIFIED].Value);
                    string markType = linkto.Attributes[Object.TYPE].Value;

                    if (linkto.Attributes[Object.OTHER_SESSION] != null)
                    {
                        String otherSessionName = linkto.Attributes[Object.OTHER_SESSION].Value;
                        this.addLinkToObject(otherSessionName, linkToObjectId, qualified, markType);
                    }
                    else
                    {
                        this.addLinkToObject(linkToObjectId, qualified, markType);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        internal void writeToXml(XmlWriter xmlWriter)
        {
            foreach (Tuple<string, bool, string> link in binaryPredicates)
            {
                xmlWriter.WriteStartElement(Object.LINKTO);
                xmlWriter.WriteAttributeString(Object.ID, link.Item1);
                xmlWriter.WriteAttributeString(Object.QUALIFIED, "" + link.Item2);
                xmlWriter.WriteAttributeString(Object.TYPE, link.Item3.ToString());
                xmlWriter.WriteEndElement();
            }

            foreach (Tuple<string, string, bool, string> link in crossSessionBinaryPredicates)
            {
                xmlWriter.WriteStartElement(Object.LINKTO);
                xmlWriter.WriteAttributeString(Object.OTHER_SESSION, link.Item1);
                xmlWriter.WriteAttributeString(Object.ID, link.Item2);
                xmlWriter.WriteAttributeString(Object.QUALIFIED, "" + link.Item3);
                xmlWriter.WriteAttributeString(Object.TYPE, link.Item4.ToString());
                xmlWriter.WriteEndElement();
            }
        }
    }
}
