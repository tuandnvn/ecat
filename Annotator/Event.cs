using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Annotator
{
    public class Event
    {
        private const string DURATION = "duration";
        private const string ANNOTATION = "annotation";
        private const string START_FRAME = "startFrame";
        private const string END_FRAME = "endFrame";
        private const string TEXT = "text";
        private const string ID = "id";
        private const string REFS = "refs";
        private const string REF = "ref";
        private const string START = "start";
        private const string END = "end";
        private const string REF_TO = "refTo";
        private const string EVENTS = "events";
        private const string EVENT = "event";
        private const string SEMANTIC_TYPE = "semanticType";

        public enum EventLinkType
        {
            SUBEVENT,
            CAUSE
        }

        public List<Reference> references { get; }
        public List<Action> events { get; }
        public HashSet<Tuple<EventLinkType, Event>> linkToEvents { get; }
        public String id { get; set; } //anottation ID
        public int startFrame { get; set; }
        public int endFrame { get; set; }
        public string text { get; set; }

        public Event(string id, int startFrame, int endFrame, string text)
        {
            this.id = id;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.text = text;
            references = new List<Reference>();
            events = new List<Action>();
            linkToEvents = new HashSet<Tuple<EventLinkType, Event>>();
        }

        public class Reference
        {
            public Event annotation { get; }
            public String refObjectId { get; }
            public int start { get; }
            public int end { get; }

            public Reference(Event annotation, String refObjectId, int start, int end)
            {
                this.annotation = annotation;
                this.refObjectId = refObjectId;
                this.start = start;
                this.end = end;
            }
        }

        public class Action
        {
            public Event annotation { get; }
            public String semanticType { get; }
            public int start { get; }
            public int end { get; }

            public Action(Event annotation, String semanticType, int start, int end)
            {
                this.annotation = annotation;
                this.semanticType = semanticType;
                this.start = start;
                this.end = end;
            }
        }

        public void addLinkTo(Event ev, EventLinkType type)
        {
            linkToEvents.Add(new Tuple<EventLinkType, Event>(type, ev));
        }

        public void addReference(Reference reference)
        {
            references.Add(reference);
        }

        //Add reference
        public void addReference(int start, int end, String refObjectId)
        {
            references.Add(new Reference(this, refObjectId, start, end));
        }

        public void addEvent(Action _event)
        {
            events.Add(_event);
        }

        public void addEvent(int start, int end, String semanticType)
        {
            Action e = new Action(this, semanticType, start, end);
            events.Add(e);
        }

        /**
        <annotations>
		<annotation id="a1">
			<duration startFrame="1" endFrame="100" />
			<text>Emily is picking up the apple in the middle</text>

			<refs>
				<ref start="0" end="5" refTo="o1" />
				<ref start="20" end="43" refTo="o3" />
			</refs>

			<events>
				<event start="9" end="16" semanticType="pick"/>
			</events>
		</annotation>
	    </annotations>
        */
        public void writeToXml(XmlWriter xmlWriter)
        {
            xmlWriter.WriteStartElement(ANNOTATION);
            xmlWriter.WriteAttributeString(ID, "" + id);
            xmlWriter.WriteStartElement(DURATION);
            xmlWriter.WriteAttributeString(START_FRAME, "" + startFrame);
            xmlWriter.WriteAttributeString(END_FRAME, "" + endFrame);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteElementString(TEXT, this.text);

            xmlWriter.WriteStartElement(REFS);
            foreach (Reference reference in references)
            {
                xmlWriter.WriteStartElement(REF);
                xmlWriter.WriteAttributeString(START, "" + reference.start);
                xmlWriter.WriteAttributeString(END, "" + reference.end);
                xmlWriter.WriteAttributeString(REF_TO, "" + reference.refObjectId);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement(EVENTS);
            foreach (Action _event in events)
            {
                xmlWriter.WriteStartElement(EVENT);
                xmlWriter.WriteAttributeString(START, "" + _event.start);
                xmlWriter.WriteAttributeString(END, "" + _event.end);
                xmlWriter.WriteAttributeString(SEMANTIC_TYPE, "" + _event.semanticType);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
        }

        public static List<Event> readFromXml(Main mainGUI, Session session, XmlNode xmlNode)
        {
            List<Event> annotations = new List<Event>();
            foreach (XmlNode node in xmlNode.SelectNodes(ANNOTATION))
            {
                String id = node.Attributes[ID].Value;
                int start = Int32.Parse(node.SelectSingleNode(DURATION).Attributes[START_FRAME].Value);
                int end = Int32.Parse(node.SelectSingleNode(DURATION).Attributes[END_FRAME].Value);
                String text = node.SelectSingleNode(TEXT).InnerText;

                Event a = new Event(id, start, end, text);
                XmlNode refs = node.SelectSingleNode(REFS);
                foreach (XmlNode _ref in refs.SelectNodes(REF))
                {
                    int refStart = Int32.Parse(_ref.Attributes[START].Value);
                    int refEnd = Int32.Parse(_ref.Attributes[END].Value);
                    String refObjectId = _ref.Attributes[REF_TO].Value;

                    Reference reference = new Reference(a, refObjectId, refStart, refEnd);
                    a.addReference(reference);
                }

                XmlNode events = node.SelectSingleNode(EVENTS);
                foreach (XmlNode _event in events.SelectNodes(EVENT))
                {
                    int eventStart = Int32.Parse(_event.Attributes[START].Value);
                    int eventEnd = Int32.Parse(_event.Attributes[END].Value);
                    String semType = _event.Attributes[SEMANTIC_TYPE].Value;

                    Action e = new Action(a, semType, eventStart, eventEnd);
                    a.addEvent(e);
                }
                annotations.Add(a);
            }
            return annotations;
        }
    }
}
