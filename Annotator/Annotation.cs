using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Annotation : UserControl
    {
        public Annotation(int minimum, int maximum, String txt, Form1 frm1, int ID, String videoName)
        {
            
            InitializeComponent();
            //MessageBox.Show("OK1"); 
            try
            {
                //MessageBox.Show(txt);
                if (txt != null)
                    textBox1.Text = new String(txt.ToCharArray());
            }
            catch (Exception ee)
            {
                //MessageBox.Show("EEEEE");

            
            }
           
            this.ID = ID;
            
            this.frm1 = frm1;
            
            this.minimum = minimum;
            this.maximum = maximum;
            this.slider1Move = false;
            this.slider2Move = false;
            
            this.videoName = videoName;
            slider1Position = lineShape2.X2;
            slider2Position = lineShape3.X2;
            lineR = new Rectangle(lineShape2.X1, 9, lineShape3.X1 - lineShape2.X1, 10);
            frameStepX = (684.0) / (maximum - minimum) ;
            //MessageBox.Show("minimum = " + minimum + ", maximum = " + maximum + " stepX = " + frameStepX);
            label1.Text = "Start Frame: " + minimum + ", StopFrame: " + maximum;
            startFrame = minimum;
            endFrame   = maximum;
            
        }
        //Get annotation text
        public String getText()
        {
            return textBox1.Text;
        }
        //Get minimum
        public int getMinimum()
        {
            return minimum;
        }
        //Get maximum
        public int getMaximum(){
            return maximum;
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
        //Get slider2 position
        public int getSlider2Pos()
        {
            return slider2Position;
        }
        //Get slider1 position
        public int getSlider1Pos()
        {
            return slider1Position;
        }
        //Set slider1 position
        public void setSlider1(int pos)
        {
            this.slider1Position = pos;
        }
        //Set slider2 position
        public void setSlider2(int pos)
        {
            this.slider2Position = pos;
        }
        //Set annotation ID
        public void setID(int ID)
        {
            this.ID = ID;
        }
        //Get annotation ID
        public int getID()
        {
            return ID;
        }
        //Get video name
        public String getVideoName()
        {
            return videoName;
        }
        public bool getSelected()
        {
            return selected;
        }
        public void setSelected(bool option)
        {
            this.selected = option;
        }
        private bool selected = false;
        private int referenceID;
        private int startFrame = 0;
        private int endFrame   = 100;
        private int minimum;   //minimum value for left slider
        private int maximum;   //maximum value for right slider
        private int slider1Position;
        private int slider2Position;
        private bool slider1Move; //true if slider1 can change position
        private bool slider2Move; //true if slider1 can change position
        private Point prevM;      //previous mouse point
        private Rectangle lineR;  //line rectangle between sliders
        private int rectangleWidth;
        private double frameStepX;
        private int ID; //anottation ID
        Brush rBrush = new SolidBrush(Color.Yellow);
        private Form1 frm1;
        private String videoName;
        private List<String> references = new List<String>();
        private void lineShape2_MouseDown(object sender, MouseEventArgs e)
        {
            slider1Move = true;
        }

        private void lineShape2_MouseUp(object sender, MouseEventArgs e)
        {
            slider1Move = false;
            MessageBox.Show("false");
        }

        private void Annotation_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X > 12 && e.Location.X < 696 && slider1Position < slider2Position && slider1Move)
            {
                Point newM = e.Location;
                int delta = newM.X - prevM.X;
                lineShape2.X1 += delta;
                lineShape2.X2 += delta;
                if (!(slider1Position < slider2Position) || (lineShape2.X1 < 12))
                {
                    lineShape2.X1 -= delta;
                    lineShape2.X2 -= delta;
                }
                
            }
            if (e.Location.X > 12 && e.Location.X < 696 && slider1Position < slider2Position && slider2Move)
            {
                Point newM = e.Location;
                int delta = newM.X - prevM.X;                
                lineShape3.X1 += delta;
                lineShape3.X2 += delta;
                if (!(slider1Position < slider2Position) || (lineShape3.X1 < 12))
                {
                    lineShape3.X1 -= delta;
                    lineShape3.X2 -= delta;
                }
            }
            prevM = e.Location;
        }

        private void Annotation_MouseUp(object sender, MouseEventArgs e)
        {
            slider1Move = false;
            slider2Move = false;
        }

        private void Annotation_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Location.X > 12 && e.Location.X < 696 && e.Location.X >= lineShape2.X1 - 2 && e.Location.X <= lineShape2.X2 + 2)
            {
                slider1Move = true;
            }
            if (e.Location.X > 12 && e.Location.X < 696 && e.Location.X >= lineShape3.X1 - 2 && e.Location.X <= lineShape3.X2 + 2)
            {
                slider2Move = true;
            }
        }

        private void lineShape2_Move(object sender, EventArgs e)
        {
            slider1Position = lineShape2.X1 - 12;
            rectangleWidth = lineShape3.X1 - lineShape2.X1;
            lineR.X = lineShape2.X1;            
            lineR.Width = rectangleWidth;
            startFrame = (int)(slider1Position / frameStepX) + 1;
            label1.Text = "Start Frame: " + startFrame + ", StopFrame: " + endFrame;
            lineShape1.Invalidate();
            
        }

        private void lineShape3_Move(object sender, EventArgs e)
        {
            slider2Position = lineShape3.X1 - 12;
            rectangleWidth = lineShape3.X1 - lineShape2.X1;            
            lineR.X = lineShape2.X1;            
            lineR.Width = rectangleWidth;
            endFrame = (int)(slider2Position / frameStepX) + 1;
            label1.Text = "Start Frame: " + startFrame + ", StopFrame: " + endFrame;
            lineShape1.Invalidate();
            
            
        }

        private void lineShape1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(rBrush, lineR);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                textBox1.ReadOnly = true;
                frm1.setLabel8Text(textBox1.Text);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != null && textBox1.Text.Contains("|"))
                textBox1.Text.Replace('|', ' ');
            frm1.setLabel8Text(textBox1.Text);

        }
        //Get reference ID
        public int getReferenceID()
        {
            return referenceID;
        }
        //Add reference by entire string - when reading from file
        public void addReference(String line)
        {
            ++referenceID;
            //MessageBox.Show(line);
            references.Add(line);
        }
        //Add reference
        public void addReference(int start, int end, String txt, int refID)
        {
            String reference = this.ID + "|" + ++referenceID + "|" + start + "|" + end + "|" + txt + "|" + refID + "|" + videoName;
            references.Add(reference);
        }//Get references list
        public List<String> getReferences()
        {
            return references;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frm1.setTextbox2(textBox1.Text);            
            frm1.unselectAnnotations();
            setSelected(true);
            
            foreach (String line in references)
            {
                //MessageBox.Show(line + "");
                String [] parameters = line.TrimStart().Split('|');       //MessageBox.Show(line);
                //MessageBox.Show(parameters.Length + "");                
                int id = Convert.ToInt32(parameters[1]);
                int start = Convert.ToInt32(parameters[2]);
                int end = Convert.ToInt32(parameters[3]);
                int refID = Convert.ToInt32(parameters[5]);
                String text = parameters[4];
                frm1.addRightBottomTableReference(id, start, end, text, refID);
            }
        }
        
    }
}
