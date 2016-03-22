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

namespace MinimalTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Capture capture = null;
            try
            {
                capture = new Capture("kinect_local_rgb_raw_synced.avi");
                capture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, 0);
                pictureBox.Image = capture.QueryFrame().Bitmap;
            }
            finally
            {
                if (capture != null)
                    capture.Dispose();
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
