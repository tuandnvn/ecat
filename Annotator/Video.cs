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
        private Capture capture;       //capture object from EmguCV library to manage video frames
        private String fileName;       // full video path
        private double aspectRatio;    // scale: frame width/frame height
        
        public Session session { get; }
        

        //constructor
        public Video(Session session, String fileName)
        {
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
    }
}
