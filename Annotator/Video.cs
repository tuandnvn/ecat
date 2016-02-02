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
        public Video(String fileName)
        {
            objects     = new List<Object>();
            annotations = new List<Annotation>();
            this.fileName = fileName;
            capture = new Capture(fileName);
            this.aspectRatio = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth) / capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            //MessageBox.Show(scale + "");
        }
        //Return list of annotations
        public List<Annotation> getAnnotations()
        {
            return annotations;
        }
        //Get specific frame
        public Mat GetFrame(double frameNum)
        {
            capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameNum);
            return capture.QueryFrame();
        }
        //get number of frames in video
        public double getFramesNumber(){
            return capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
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
                if (obj.getID() == o.getID())
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
                objects.Add(o);
            o.setID(++objectID);
        }
        //Add annotation
        public void addAnnotation(Annotation a)
        {
            //1)Check if object exists in objects list
            bool exists = false;
            foreach (Annotation annotation in annotations)
            {
                if (annotation.getID() == a.getID())
                {
                    exists = true;
                    break;
                }
            }
            if (!exists)
            {
                annotations.Add(a);
                a.setID(++annotationID);
            }
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
        private double aspectRatio;    // scale: frame width/frame height
        private List<Object> objects;  // list of objects in videos
        private List<Annotation> annotations;
        private int objectID;          // video objects IDs
        private int annotationID;      // annotation ID
    }
}
