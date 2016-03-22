using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    class MyPictureBox : PictureBox
    {
        public Capture capture { set; get; }
        public Mat mat { set; get; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Console.WriteLine("Before On_Paint");

            //if (mat != null)
            //{
            //    Image = mat.Bitmap;
            //}
            //else if (capture != null)
            //{
            //    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, 0);
            //    Image = capture.QueryFrame().Bitmap;
            //}
            base.OnPaint(pe);

            Console.WriteLine("After On_Paint");
        }
    }
}
