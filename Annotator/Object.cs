using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Annotator
{
    public class Object
    {
        //Constructor
        public Object(int ID, Color color, String type, int startFrame, int endFrame, Rectangle boundingBox, int borderSize, double scale)
        {
            this.color       = color;
            this.type        = type;
            this.ID          = ID;
            this.startFrame  = startFrame;
            this.endFrame    = endFrame;
            this.boundingBox = boundingBox;
            this.borderSize  = borderSize;
            this.scale       = scale;
        }
        //Get object scale
        public double getScale()
        {
            return scale;
        }
        //Set object scale
        public void setScale(double scale)
        {
            this.scale  = scale;
        }
        //Get object id
        public int getID()
        {
            return ID;
        }
        //Get start frame
        public int getStartFrame()
        {
            return startFrame;
        }
        public int getEndFrame()
        {
            return endFrame;
        }
        //Set start frame
        public void setStartFrame(int startFrame)
        {
            this.startFrame = startFrame;
        }
        //Set end frame
        public void setEndFrame(int endFrame)
        {
            this.endFrame = startFrame;
        }
        //Set object type
        public void setType(String type)
        {
            this.type = type;
        }
        //Get object type
        public String getType()
        {
            return type;
        }
        //Set object color
        public void setColor(Color c)
        {
            this.color = c;
        }
        //Get object type
        public Color getColor()
        {
            return color;
        }
        //Get object bounding box
        public Rectangle getBoundingBox()
        {
            return boundingBox;
        }
        //Set bounding box
        public void setBoundingBox(Rectangle r)
        {
            this.boundingBox = r;
        }
        //Set object ID
        public void setID(int ID)
        {
            this.ID = ID;
        }
        //Get border size of bounding box
        public int getBorderSize()
        {
            return borderSize;
        }
        private int ID;                //Object's ID
        private int startFrame;        //Object start frame
        private int endFrame;          //Object end frame
        private Color color;           //Object's boudnign box color
        private String type;           //Object's type
        private Rectangle boundingBox; //Object bounding box;
        private int borderSize;        //Object bounding box border size
        private double scale;          //Object scale
    }
}
