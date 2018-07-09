using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
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
            if (options != null)
            {
                // There is a saved option config file
                switch (options.detectionMode)
                {
                    case Options.OverwriteMode.ADD_SEPARATE:
                        detectSeparatedObjRb.Checked = true;
                        break;
                    case Options.OverwriteMode.OVERWRITE:
                        detectOverwriteObjRb.Checked = true;
                        break;
                    case Options.OverwriteMode.NO_OVERWRITE:
                        detectIgnoreObjRb.Checked = true;
                        break;
                }

                switch (options.showRigOption)
                {
                    case Options.ShowRig.SHOW_ALL:
                        showAllRigRb.Checked = true;
                        break;
                    case Options.ShowRig.SHOW_UPPER:
                        upperBodyRb.Checked = true;
                        break;
                }

                switch (options.interpolationModes[Options.RIG])
                {
                    case Options.InterpolationMode.LEFT_COPY:
                        interpolateLeftRigRb.Checked = true;
                        break;
                    case Options.InterpolationMode.LINEAR:
                        interpolateLinearRigRb.Checked = true;
                        break;
                }

                switch (options.interpolationModes[Options.RECTANGLE])
                {
                    case Options.InterpolationMode.LEFT_COPY:
                        interpolateLeftRectRb.Checked = true;
                        break;
                    case Options.InterpolationMode.LINEAR:
                        interpolateLinearRectRb.Checked = true;
                        break;
                }

                switch (options.interpolationModes[Options.GLYPH])
                {
                    case Options.InterpolationMode.LEFT_COPY:
                        interpolateLeftGlyphRb.Checked = true;
                        break;
                    case Options.InterpolationMode.LINEAR:
                        interpolateLinearGlyphRb.Checked = true;
                        break;
                }

                if (options.showMarkerMode)
                {
                    showMarker.Checked = true;
                }
                else
                {
                    donShowMarker.Checked = true;
                }
                    

                showObjPredTypeList();
                showEventPredTypeList();
                showConstraintList();
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (options == null)
            {
                options = new Options();
            }

            saveDetection();
            saveView();

            options.save();

            this.Dispose();
        }

        private void saveView()
        {
            if (showAllRigRb.Checked)
            {
                options.showRigOption = Options.ShowRig.SHOW_ALL;
            }
            else if (upperBodyRb.Checked)
            {
                options.showRigOption = Options.ShowRig.SHOW_UPPER;
            }

            if (interpolateLeftRigRb.Checked)
            {
                options.interpolationModes[Options.RIG] = Options.InterpolationMode.LEFT_COPY;
            }
            else if (interpolateLinearRigRb.Checked)
            {
                options.interpolationModes[Options.RIG] = Options.InterpolationMode.LINEAR;
            }

            if (interpolateLeftGlyphRb.Checked)
            {
                options.interpolationModes[Options.GLYPH] = Options.InterpolationMode.LEFT_COPY;
            }
            else if (interpolateLinearGlyphRb.Checked)
            {
                options.interpolationModes[Options.GLYPH] = Options.InterpolationMode.LINEAR;
            }

            if (interpolateLeftRectRb.Checked)
            {
                options.interpolationModes[Options.RECTANGLE] = Options.InterpolationMode.LEFT_COPY;
            }
            else if (interpolateLinearRectRb.Checked)
            {
                options.interpolationModes[Options.RECTANGLE] = Options.InterpolationMode.LINEAR;
            }

            if (showMarker.Checked)
            {
                options.showMarkerMode = true;
            }
            else
            {
                options.showMarkerMode = false;
            }
        }

        private void saveDetection()
        {
            if (detectSeparatedObjRb.Checked)
            {
                options.detectionMode = Options.OverwriteMode.ADD_SEPARATE;
            }
            else if (detectOverwriteObjRb.Checked)
            {
                options.detectionMode = Options.OverwriteMode.OVERWRITE;
            }
            else if (detectIgnoreObjRb.Checked)
            {
                options.detectionMode = Options.OverwriteMode.NO_OVERWRITE;
            }
        }

    }
}