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
        public object playbackLock;

        protected override void OnPaint(PaintEventArgs pe)
        {
            //if (mat != null)
            //{
            //    Image = mat.Bitmap;
            //}
            //else if (capture != null)
            //{
            //    capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, 0);
            //    Image = capture.QueryFrame().Bitmap;
            //}
            if (playbackLock != null)
            {
                lock (playbackLock)
                {
                    base.OnPaint(pe);
                }
            } else
            {
                base.OnPaint(pe);
            }
        }
    }
}
