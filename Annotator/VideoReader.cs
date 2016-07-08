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
        public long totalMiliTime { get; private set; }
        public double fps { get; private set; }

        //constructor
        public VideoReader(String fileName, long totalMiliTime)
        {
            this.fileName = fileName;
            this.totalMiliTime = totalMiliTime;
            capture = new Capture(fileName);

            Console.WriteLine("fileName " + fileName);

            // This framecount is just an estimation
            frameCount = (int)capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

            Console.WriteLine("frameCount " + frameCount);

            fps = capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);

            Console.WriteLine("fps " + fps);

            int countBack;
            for ( countBack = 0; countBack < 50; countBack ++ )
            {
                Mat m = getFrame(frameCount - countBack);
                if (m != null) break;
            }
            frameCount -= countBack - 1;

            Console.WriteLine("frameCount " + frameCount);

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

                return m;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception for frame " + frameNum);
                Console.WriteLine(e);

                Console.WriteLine("Try reopen the video file");
                // Try reopen the video file
                capture = new Capture(fileName);

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
