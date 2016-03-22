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
        
        public String fileName { get; }       // full video path
        private double aspectRatio;    // scale: frame width/frame height
        public int frameNumber { get; set;  }
        public Session session { get; }
        public double frameWidth { get; set; }
        public double frameHeight { get; set; }

        //constructor
        public Video(Session session, String fileName)
        {
            this.fileName = fileName;
            Capture capture = new Capture(fileName);
            frameNumber = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            frameWidth = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth); 
            frameHeight = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            this.aspectRatio = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth) / capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            capture.Dispose();
            this.session = session;
            //MessageBox.Show(scale + "");
        }

        //Get specific frame
        public Mat GetFrame(double frameNum)
        {
            Capture capture = null;
            try
            {
                capture = new Capture(fileName);
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameNum);
                return capture.QueryFrame();
            } finally
            {
                if ( capture != null )
                    capture.Dispose();
            }
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
    }
}
