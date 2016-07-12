using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Annotator
{
    public partial class OptionsForm : Form
    {
        private Options options;

        public OptionsForm(Options options)
        {
            InitializeComponent();
            this.options = options;
            InitializeFromConfig();
        }

        private void InitializeFromConfig()
        {
            if ( options != null )
            {
                // There is a saved option config file
                switch (options.detectionMode)
                {
                    case Options.GlyphDetectionMode.ADD_SEPARATE:
                        radioButton1.Checked = true;
                        break;
                    case Options.GlyphDetectionMode.OVERWRITE:
                        radioButton2.Checked = true;
                        break;
                    case Options.GlyphDetectionMode.NO_OVERWRITE:
                        radioButton3.Checked = true;
                        break;
                }
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if ( options == null )
            {
                options = new Options();
            }

            saveDetection();
            options.save();

            this.Dispose();
        }

        private void saveDetection()
        {
            if (radioButton1.Checked)
            {
                options.detectionMode = Options.GlyphDetectionMode.ADD_SEPARATE;
            }
            else if (radioButton2.Checked)
            {
                options.detectionMode = Options.GlyphDetectionMode.OVERWRITE;
            }
            else if (radioButton3.Checked)
            {
                options.detectionMode = Options.GlyphDetectionMode.NO_OVERWRITE;
            }
        }
    }
}
