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
    public class VideoReader : IDisposable
    {
        private Capture capture;
        public String fileName { get; }       // full video path
        public double aspectRatio { get; }    // scale: frame width/frame height
        public int frameCount { get; private set; }
        public int frameWidth { get; private set; }
        public int frameHeight { get; private set; }
        public int totalMiliTime { get; private set; }

        //constructor
        public VideoReader(String fileName, int totalMiliTime)
        {
            this.fileName = fileName;
            this.totalMiliTime = totalMiliTime;
            capture = new Capture(fileName);
            frameCount = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
            frameWidth = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            frameHeight = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            this.aspectRatio = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth) / capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
        }

        //Get specific frame
        public Mat getFrame(double frameNum)
        {
            try
            {
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameNum);
                Mat m = capture.QueryFrame();

                int mili = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec);
                Console.WriteLine("Frame no = " + frameNum + "; Frame time = " + mili);

                return m;
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            if (capture != null)
            {
                capture.Dispose();
            }
        }
    }
}
