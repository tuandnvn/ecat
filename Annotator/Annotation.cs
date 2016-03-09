using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Annotator
{
    public partial class Annotation : UserControl
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
        int minLeftPosition;
        int maxLeftPosition;
        private bool selected = false;
        private int startFrame = 0;
        private int endFrame = 100;
        private int start;   //minimum value for left slider
        private int end;   //maximum value for right slider
        private bool slider1Move; //true if slider1 can change position
        private bool slider2Move; //true if slider1 can change position
        private double frameStepX;
        public String id { get; set; } //anottation ID
        Brush rBrush = new SolidBrush(Color.FromArgb(128, Color.Yellow));
        private Main mainGUI;
        private List<Reference> references = new List<Reference>();
        private List<Event> events = new List<Event>();
        private int rectangleYPos = 8;
        private int rectangleSize = 12;

        public class Reference
        {
            public Annotation annotation { get; }
            public String refObjectId { get; }
            public int start { get; }
            public int end { get; }

            public Reference(Annotation annotation, String refObjectId, int start , int end)
            {
                this.annotation = annotation;
                this.refObjectId = refObjectId;
                this.start = start;
                this.end = end;
            }
        }

        public class Event
        {
            Annotation annotation { get; }
            public String semanticType { get;  }
            public int start { get; }
            public int end { get; }

            public Event (Annotation annotation, String semanticType, int start, int end)
            {
                this.annotation = annotation;
                this.semanticType = semanticType;
                this.start = start;
                this.end = end;
            }
        }

        public Annotation(String id, int start, int end, String txt, Main mainGUI, Session session)
        {
            
            InitializeComponent();

            minLeftPosition = axis.X1;
            maxLeftPosition = axis.X2;

            //MessageBox.Show(txt);
            if (txt != null)
                textBox1.Text = new String(txt.ToCharArray());
            this.id = id;
            this.mainGUI = mainGUI;
            this.start = start;
            this.end = end;
            this.slider1Move = false;
            this.slider2Move = false;

            frameStepX = (double)(maxLeftPosition - minLeftPosition) / ( session.sessionLength - 1 );

            leftMarker.X1 = leftMarker.X2 = (int)(minLeftPosition + frameStepX * ( start - 1 ));
            rightMarker.X1 = rightMarker.X2 = (int)(minLeftPosition + frameStepX * ( end - 1 ));
            this.rectangleShape1.Bounds = new Rectangle(leftMarker.X1 + (leftMarker.BorderWidth - 1), rectangleYPos, 
                rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);

            //MessageBox.Show("minimum = " + minimum + ", maximum = " + maximum + " stepX = " + frameStepX);
            label1.Text = "Start Frame: " + start + ", StopFrame: " + end;
            startFrame = start;
            endFrame   = end;
            
        }
        //Get annotation text
        public String getText()
        {
            return textBox1.Text;
        }
        //Get minimum
        public int getMinimum()
        {
            return start;
        }
        //Get maximum
        public int getMaximum(){
            return end;
        }
        //Set start frame
        public void setStartFrame(int startFrame)
        {
            this.startFrame = startFrame;
        }
        //Set end frame
        public void setEndFrame(int endFrame)
        {
            this.endFrame = endFrame;
        }
        //Get start frame
        public int getStartFrame()
        {
            return startFrame;
        }
        //Get end frame
        public int getEndFrame()
        {
            return endFrame;
        }

        public bool getSelected()
        {
            return selected;
        }
        public void setSelected(bool option)
        {
            this.selected = option;
        }

        private void Annotation_MouseMove(object sender, MouseEventArgs e)
        {
            if (slider1Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition :  e.Location.X;

                if (newX < rightMarker.X1)
                {
                    leftMarker.X1 = newX;
                    leftMarker.X2 = newX;
                    this.rectangleShape1.Location = new Point(leftMarker.X1 + leftMarker.BorderWidth - 1, rectangleYPos);
                    this.rectangleShape1.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2), rectangleSize);
                    this.mainGUI.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
            if (slider2Move)
            {
                int newX = (e.Location.X < minLeftPosition) ? minLeftPosition : (e.Location.X > maxLeftPosition) ? maxLeftPosition : e.Location.X;

                if (leftMarker.X1 < newX)
                {
                    rightMarker.X1 = newX;
                    rightMarker.X2 = newX;
                    this.rectangleShape1.Size = new Size(rightMarker.X1 - leftMarker.X1 - (leftMarker.BorderWidth + rightMarker.BorderWidth - 2) , rectangleSize);
                    this.mainGUI.setTrackbarLocation((int)((newX - minLeftPosition) / frameStepX) + 1);
                }
            }
        }

        private void Annotation_MouseUp(object sender, MouseEventArgs e)
        {
            slider1Move = false;
            slider2Move = false;
        }

        private void Annotation_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location.X + " " + leftMarker.X1 + " " + rightMarker.X2);
            if (e.Location.X >= leftMarker.X1 - leftMarker.BorderWidth && e.Location.X <= leftMarker.X1 + leftMarker.BorderWidth)
            {
                slider1Move = true;
            } else
            {
                slider1Move = false;
            }

            if (e.Location.X >= rightMarker.X1 - rightMarker.BorderWidth && e.Location.X <= rightMarker.X1 + rightMarker.BorderWidth)
            {
                slider2Move = true;
            }
            else
            {
                slider2Move = false;
            }

            // Allow only one slider moves at one time
            if (slider1Move && slider2Move)
            {
                slider2Move = false;
            }
        }

        private void lineShape2_Move(object sender, EventArgs e)
        {
            startFrame = (int)((leftMarker.X1 - minLeftPosition) / frameStepX) + 1;
            label1.Text = "Start Frame: " + startFrame + ", StopFrame: " + endFrame;
            mainGUI.Invalidate();
        }

        private void lineShape3_Move(object sender, EventArgs e)
        {
            endFrame = (int)((rightMarker.X1 - minLeftPosition) / frameStepX) + 1;
            label1.Text = "Start Frame: " + startFrame + ", StopFrame: " + endFrame;
            mainGUI.Invalidate();
        }

        private void lineShape1_Paint(object sender, PaintEventArgs e)
        {
            axis.BringToFront();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.ReadOnly = true;
                mainGUI.setAnnotationText(textBox1.Text);
                //Set middle_bottom panel:
                /*
                frm1.setStartFrameLabel(startFrame + "");
                frm1.setEndFrameLabel(endFrame + "");
                frm1.setTextBox1Text(textBox1.Text);*/
            }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.ReadOnly = false;
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
        
        //Get references list
        public List<Reference> getReferences()
        {
            return references;
        }

        public void addEvent(Event _event)
        {
            events.Add(_event);
        }

        public void addEvent( int start, int end, String semanticType )
        {
            Event e = new Event(this, semanticType, start, end);
            events.Add(e);
        }

        public List<Event> getEvents()
        {
            return events;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainGUI.setAnnotationText(textBox1.Text);            
            mainGUI.unselectAnnotations();
            setSelected(true);
            
            foreach (Reference reference in references)
            {
                int start = reference.start;
                int end = reference.end;
                String refID = reference.refObjectId;
                String text = this.getText().Substring(start, end - start);
                mainGUI.addRightBottomTableReference(start, end, text, refID);
            }

            foreach (Event ev in events)
            {
                int start = ev.start;
                int end = ev.end;
                String semanticType = ev.semanticType;
                String text = this.getText().Substring(start, end - start);
                mainGUI.addRightBottomTableReference(start, end, text, semanticType);
            }
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
            xmlWriter.WriteElementString(TEXT, this.getText());

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
            foreach (Event _event in events)
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

        public static List<Annotation> readFromXml(Main mainGUI, Session session, XmlNode xmlNode)
        {
            List<Annotation> annotations = new List<Annotation>();
            foreach (XmlNode node in xmlNode.SelectNodes(ANNOTATION) ){
                String id = node.Attributes[ID].Value;
                int start = Int32.Parse(node.SelectSingleNode(DURATION).Attributes[START_FRAME].Value);
                int end = Int32.Parse(node.SelectSingleNode(DURATION).Attributes[END_FRAME].Value);
                String text = node.SelectSingleNode(TEXT).InnerText;

                Annotation a = new Annotation(id, start, end, text, mainGUI, session);
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

                    Event e = new Event(a, semType, eventStart, eventEnd);
                    a.addEvent(e);
                }
                annotations.Add(a);
            }
            return annotations;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove this annotation?",
                "Remove",
                MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                mainGUI.removeAnnotation(this);
            }
        }

        private void Annotation_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("Repaint annotation");
        }
    }
}
