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

                renderLinkTypeList();
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

        private void addPredicate_Click(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^([a-zA-Z0-9_]+)\(([X-Y](,[X-Y])?)\)$");

            try
            {
                var newType = objectLinkTypeTxtBox.Text;

                var newPredicate = Predicate.Parse(newType);

                if (newPredicate == null)
                {
                    throw new ArgumentException("new predicate = null. The predicate form has problem!");
                }
                if (this.options.objectPredicates.Contains(newPredicate))
                {
                    throw new ArgumentException("Type exists!");
                }

                this.options.objectPredicates.Add(newPredicate);


                renderLinkTypeList();
                this.objectLinkTypeListBox.SelectedIndex = this.objectLinkTypeListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeLinkType_Click(object sender, EventArgs e)
        {
            var selectIndex = objectLinkTypeListBox.SelectedIndex;
            try
            {
                this.options.objectPredicates.RemoveAt(selectIndex);
                renderLinkTypeList();
            }
            catch
            {
            }
        }

        private void upObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = objectLinkTypeListBox.SelectedIndex;
            try
            {
                if ( selectIndex > 0)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex - 1, swapItem);
                    renderLinkTypeList();
                    this.objectLinkTypeListBox.SelectedIndex = selectIndex - 1;
                }
            }
            catch
            {
            }
        }

        private void downObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = objectLinkTypeListBox.SelectedIndex;
            try
            {
                if (selectIndex < this.options.objectPredicates.Count - 1)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex + 1, swapItem);
                    renderLinkTypeList();
                    this.objectLinkTypeListBox.SelectedIndex = selectIndex + 1;
                }
            }
            catch
            {
            }
        }

        private void renderLinkTypeList()
        {
            this.objectLinkTypeListBox.Items.Clear();
            this.objectLinkTypeListBox.Items.AddRange(this.options.objectPredicates.ToArray());
        }

        private void objectLinkTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = objectLinkTypeListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                upObjLinkTypeBtn.Enabled = true;
                downObjLinkTypeBtn.Enabled = true;
                removeLinkTypeBtn.Enabled = true;
            }
            else
            {
                upObjLinkTypeBtn.Enabled = false;
                downObjLinkTypeBtn.Enabled = false;
                removeLinkTypeBtn.Enabled = false;
            }
        }
    }
}
