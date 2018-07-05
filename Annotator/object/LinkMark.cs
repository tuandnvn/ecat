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
        public Object o { get; }
        public SortedSet<PredicateMark> predicateMarks { get; }

        public LinkMark(Object o, int frameNo) : base(frameNo)
        {
            this.o = o;
            predicateMarks = new SortedSet<PredicateMark>(new PredicateMarkComparer());
        }

        public void addLinkToObject(bool qualified, Predicate linkType)
        {
            predicateMarks.Add(new PredicateMark(frameNo, qualified, linkType, o.session, new Object[] { o }));
        }

        public void addLinkToObject(Object otherObject, bool qualified, Predicate linkType)
        {
            predicateMarks.Add(new PredicateMark(frameNo, qualified, linkType, o.session, new Object[] { o, otherObject }));
        }

        public void addPredicate(PredicateMark pm)
        {
            predicateMarks.Add(pm);
        }

        override public String ToString()
        {
            return String.Join(",", predicateMarks.Select(u => u.ToString()).ToArray());
        }

        internal void loadFromHtml(XmlNode linkNode)
        {
            foreach (XmlNode linkto in linkNode.SelectNodes(Object.LINKTO))
            {
                try
                {
                    bool qualified = bool.Parse(linkto.Attributes[Object.QUALIFIED].Value);
                    string predicateForm = linkto.Attributes[Object.TYPE].Value;

                    if (linkto.Attributes[Object.OTHER_SESSION] != null)
                    {
                        String linkToObjectId = linkto.Attributes[Object.ID].Value;
                        String otherSessionName = linkto.Attributes[Object.OTHER_SESSION].Value;

                        Predicate pred = Predicate.Parse(predicateForm);

                        // Try with default binary predicate
                        if (pred == null)
                        {
                            pred = Predicate.ParseToBinary(predicateForm);
                        }

                        if (pred != null)
                        {
                            var otherSession = this.o.session.project.getSession(otherSessionName);
                            if (otherSession != null)
                            {
                                var otherObject = otherSession.getObject(linkToObjectId);
                                if (otherObject == null)
                                {
                                    otherSession.loadIfNotLoaded();
                                    otherObject = otherSession.getObject(linkToObjectId);
                                }

                                if (otherObject != null)
                                    this.addLinkToObject(otherObject, qualified, pred);
                            }

                        }
                    }
                    else
                    {
                        if (linkto.Attributes[Object.ID] != null)
                        {
                            String linkToObjectId = linkto.Attributes[Object.ID].Value;

                            Predicate pred = Predicate.Parse(predicateForm);

                            // Try with default binary predicate
                            if (pred == null)
                            {
                                pred = Predicate.ParseToBinary(predicateForm);
                            }

                            if (pred != null)
                            {
                                var otherObject = this.o.session.getObject(linkToObjectId);
                                if (otherObject != null)
                                    this.addLinkToObject(otherObject, qualified, pred);
                            }
                        }
                        else
                        {
                            Predicate pred = Predicate.Parse(predicateForm);

                            // Try with default binary predicate
                            if (pred == null)
                            {
                                pred = Predicate.ParseToUnary(predicateForm);
                            }

                            this.addLinkToObject(qualified, pred);
                        }
                    }
                }
                catch (Exception e)
                {
                }
            }
        }

        internal void writeToXml(XmlWriter xmlWriter)
        {
            foreach (var predicateMark in predicateMarks)
            {
                xmlWriter.WriteStartElement(Object.LINKTO);
                if (predicateMark.predicate.combination.size == 2)
                {
                    var otherObject = predicateMark.objects[1];
                    if (otherObject.session.sessionName != predicateMark.objects[0].session.sessionName)
                    {
                        xmlWriter.WriteAttributeString(Object.OTHER_SESSION, otherObject.session.sessionName);
                    }

                    xmlWriter.WriteAttributeString(Object.ID, otherObject.id);
                }

                xmlWriter.WriteAttributeString(Object.QUALIFIED, "" + predicateMark.qualified);
                xmlWriter.WriteAttributeString(Object.TYPE, predicateMark.predicate.ToString());
                xmlWriter.WriteEndElement();
            }
        }

        internal bool isEmpty()
        {
            if (predicateMarks.Count != 0) return false;
            return true;
        }
    }
}
