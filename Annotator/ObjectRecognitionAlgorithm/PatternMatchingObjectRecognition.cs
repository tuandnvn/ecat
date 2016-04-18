using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Annotator.depth;
using Emgu.CV;
using System.IO;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;
using Microsoft.Kinect;

namespace Annotator.ObjectRecognitionAlgorithm
{
    class PatternMatchingObjectRecognition : IObjectRecogAlgo
    {
        public string patternDirectory { get; private set; }

        public PatternMatchingObjectRecognition ( String patternDirectory)
        {
            this.patternDirectory = patternDirectory;
            Initialize();
        }

        private void Initialize()
        {
            string[] patterns = Directory.GetFiles(patternDirectory);

            foreach (var pattern in patterns)
            {
                if (pattern.EndsWith(".png"))
                {
                    using (Mat modelImage = CvInvoke.Imread(pattern, LoadImageType.Grayscale))
                    {

                    }
                }
            }
        }

        public List<Object> findObjects(VideoReader videoReader, IDepthReader depthReader, Action<ushort[], CameraSpacePoint[]> mappingFunction)
        {
            List<Object> objects = new List<Object>();
            var patternNameToFrames = new Dictionary<string, List<int>>();

            for (int i = 0; i < videoReader.frameCount; i++)
            {
                string[] patterns = Directory.GetFiles(patternDirectory);

                foreach (var pattern in patterns)
                {
                    if (pattern.EndsWith(".png"))
                    {
                        long matchTime;
                        using (Mat modelImage = CvInvoke.Imread(pattern, LoadImageType.Grayscale))
                        using (Mat observedImage = videoReader.getFrame(i))
                            using (Mat grayImage = new Mat())
                        {
                            CvInvoke.CvtColor(observedImage, grayImage, ColorConversion.Rgb2Gray);
                            Mat homography;
                            VectorOfKeyPoint modelKeyPoints;
                            VectorOfKeyPoint observedKeyPoints;
                            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
                            {
                                Mat mask;
                                DrawMatches.FindMatch(modelImage, grayImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
                                out mask, out homography);

                                if (homography != null)
                                {
                                    //draw a rectangle along the projected model
                                    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                                    System.Drawing.PointF[] pts = new System.Drawing.PointF[]
                                    {
                                      new System.Drawing.PointF(rect.Left, rect.Bottom),
                                      new System.Drawing.PointF(rect.Right, rect.Bottom),
                                      new System.Drawing.PointF(rect.Right, rect.Top),
                                      new System.Drawing.PointF(rect.Left, rect.Top)
                                    };
                                    pts = CvInvoke.PerspectiveTransform(pts, homography);

                                    Point[] points = Array.ConvertAll<System.Drawing.PointF, Point>(pts, Point.Round);
                                }
                            }
                        }
                    }
                }
            }

            return objects;
        }

        //public Point[] findObjectBoundary(VideoReader videoReader, IDepthReader depthReader, int frame)
        //{

        //}


        public string getName()
        {
            return "Detect CwC block";
        }
    }
}
