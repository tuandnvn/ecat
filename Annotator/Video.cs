using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Windows.Forms;
namespace Annotator
{
    //Video class
    public class Video
    {
        //constructor
        public Video(Session session, String fileName)
        {
            objects     = new List<Object>();
            
            this.fileName = fileName;
            capture = new Capture(fileName);
            this.aspectRatio = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth) / capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            this.session = session;
            //MessageBox.Show(scale + "");
        }

        //Get specific frame
        public Mat GetFrame(double frameNum)
        {
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameNum);
            return capture.QueryFrame();
        }
        //get number of frames in video
        public int getFramesNumber(){
            return (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
        }
        //Get video full path
        public String getFileName()
        {
            return fileName;
        }
        //Get video scale
        public double getAspectRatio()
        {
            return aspectRatio;
        }

        //Add object
        public void addObject(Object o)
        {
            //1)Check if object exists in objects list
            bool exists = false;
            foreach (Object obj in objects)
            {
                if (obj.id == o.id)
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
                objects.Add(o);
            if (o.id == "") { o.id = "o" + ++objectID; }
            
        }

        //Get frame width
        public double getFrameWidth()
        {
            return capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
        }
        //Get frame height
        public double getFrameHeight()
        {
            return capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
        }
        //Get objects list
        public List<Object> getObjects()
        {
            return objects;
        }
        private Capture capture;       //capture object from EmguCV library to manage video frames
        private String fileName;       // full video path
        private double aspectRatio ;    // scale: frame width/frame height
        private List<Object> objects;  // list of objects in videos
        public Session session { get; }
        private int objectID = 0;          // video objects IDs
    }
}
