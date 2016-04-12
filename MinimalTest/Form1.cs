using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Annotator;
using System.IO;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing.Imaging;

namespace MinimalTest
{
    public partial class Form1 : Form
    {
        Capture capture = null;
        Dictionary<string, Point[]> markedPoints;
        int currentFrame = 0;
        int frameCount = 0;

        public Form1()
        {
            InitializeComponent();

            markedPoints = new Dictionary<string, Point[]>();

            try
            {
                capture = new Capture("kinect_local_rgb_raw_synced.avi");
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrame);
                pictureBox.Image = capture.QueryFrame().Bitmap;

                frameCount = (int) capture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                
            } catch ( Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void process ()
        {
            string[] patterns = Directory.GetFiles("pattern");

            foreach (var pattern in patterns)
            {
                Console.WriteLine(pattern);
                if (pattern.EndsWith(".png"))
                {
                    long matchTime;
                    using (Mat modelImage = CvInvoke.Imread(pattern, LoadImageType.Grayscale))
                    using (Mat observedImage = capture.QueryFrame())
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
                                Console.WriteLine("Find homography");
                                //draw a rectangle along the projected model
                                Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                                PointF[] pts = new PointF[]
                                {
                                      new PointF(rect.Left, rect.Bottom),
                                      new PointF(rect.Right, rect.Bottom),
                                      new PointF(rect.Right, rect.Top),
                                      new PointF(rect.Left, rect.Top)
                                };
                                pts = CvInvoke.PerspectiveTransform(pts, homography);

                                Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
                                foreach ( var point in points)
                                {
                                    Console.WriteLine(point);
                                }
                                markedPoints[pattern] = points;
                            }
                        }
                    }
                }
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.KeyCode == Keys.Right)
            {
                if (currentFrame < frameCount - 1)
                {
                    currentFrame++;
                    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrame);
                    pictureBox.Image = capture.QueryFrame().Bitmap;
                    Invalidate();
                }
            }

            if ( e.KeyCode == Keys.Left)
            {
                if (currentFrame > 0)
                {
                    currentFrame--;
                    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, currentFrame);
                    pictureBox.Image = capture.QueryFrame().Bitmap;
                    Invalidate();
                }
            }

            if ( e.KeyCode == Keys.Enter)
            {
                Console.WriteLine("Begin process");
                process();
                Console.WriteLine("End process");
                Invalidate();
            }

            if (e.KeyCode == Keys.D)
            {
                Console.WriteLine("Save image");
                pictureBox.Image.Save("temp.png", ImageFormat.Png);
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Console.WriteLine("Paint");
            Pen pen = new Pen(Color.Blue, 3);

            foreach ( Point[] ps in markedPoints.Values)
            {
                foreach ( Point p in ps )
                {
                    Console.WriteLine("Paint " + p );
                    e.Graphics.DrawEllipse(pen, new RectangleF(p.X - 5, p.Y - 5, 11,11));
                }
            }
        }
    }
}
