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

        public String fileName { get; }       // full video path
        private Lazy<Capture> capture;
        public long totalMiliTime { get; }

        public double aspectRatio
        {
            get
            {
                return capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth) / capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            }
        }

        private int _frameCount = -1;
        public int frameCount
        {
            get
            {
                if (_frameCount == -1)
                {
                    // This framecount is just an estimation
                    _frameCount = (int)capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);

                    int countBack;
                    for (countBack = 0; countBack < 50; countBack++)
                    {
                        Mat m = getFrame(_frameCount - countBack);
                        if (m != null) break;
                    }
                    _frameCount -= countBack - 1;
                }
                return _frameCount;
            }
        }
        public int frameWidth
        {
            get
            {
                return (int)capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth);
            }
        }
        public int frameHeight
        {
            get
            {
                return (int)capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight);
            }
        }

        public double fps
        {
            get
            {
                return capture.Value.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            }
        }

        //constructor
        public VideoReader(String fileName, long totalMiliTime)
        {
            this.fileName = fileName;
            this.totalMiliTime = totalMiliTime;
            capture = new Lazy<Capture>(
                () =>
                {
                    Console.WriteLine(" Lazy load " + fileName);
                    return new Capture(fileName);
                }
            );
        }

        //Get specific frame
        public Mat getFrame(double frameNum)
        {
            try
            {
                capture.Value.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, frameNum);
                Mat m = capture.Value.QueryFrame();

                return m;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception for frame " + frameNum);
                Console.WriteLine(e);

                return null;
            }
        }

        public void Dispose()
        {
            if (capture != null && capture.IsValueCreated)
            {
                capture.Value.Dispose();
            }
        }
    }
}
