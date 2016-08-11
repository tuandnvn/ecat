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

                showLinkTypeList();
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
            try
            {
                var predicateForm = objectLinkTypeTxtBox.Text;

                var newPredicate = Predicate.Parse(predicateForm);

                if (newPredicate == null)
                {
                    throw new ArgumentException("new predicate = null. The predicate form has problem!");
                }
                if (this.options.objectPredicates.Contains(newPredicate))
                {
                    throw new ArgumentException("Type exists!");
                }

                this.options.objectPredicates.Add(newPredicate);


                showLinkTypeList();
                this.predicateFormListBox.SelectedIndex = this.predicateFormListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeLinkType_Click(object sender, EventArgs e)
        {
            var selectIndex = predicateFormListBox.SelectedIndex;
            try
            {
                this.options.objectPredicates.RemoveAt(selectIndex);
                showLinkTypeList();

                upObjLinkTypeBtn.Enabled = false;
                downObjLinkTypeBtn.Enabled = false;
                removeLinkTypeBtn.Enabled = false;
            }
            catch
            {
            }
        }

        private void upObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = predicateFormListBox.SelectedIndex;
            try
            {
                if ( selectIndex > 0)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex - 1, swapItem);
                    showLinkTypeList();
                    this.predicateFormListBox.SelectedIndex = selectIndex - 1;
                }
            }
            catch
            {
            }
        }

        private void downObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = predicateFormListBox.SelectedIndex;
            try
            {
                if (selectIndex < this.options.objectPredicates.Count - 1)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex + 1, swapItem);
                    showLinkTypeList();
                    this.predicateFormListBox.SelectedIndex = selectIndex + 1;
                }
            }
            catch
            {
            }
        }

        private void showLinkTypeList()
        {
            this.predicateFormListBox.Items.Clear();
            this.predicateFormListBox.Items.AddRange(this.options.objectPredicates.ToArray());
        }

        private void objectLinkTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = predicateFormListBox.SelectedIndex;
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

        private void addConstraintBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var constraintForm = constraintTxtBox.Text;

                PredicateConstraint newConstraint = null;
                if (constraintForm.StartsWith("UNIQUE="))
                {
                    newConstraint = UniqueConstraint.Parse(constraintForm.Substring("UNIQUE=".Length));

                    if (this.options.predicateConstraints.Contains(newConstraint))
                    {
                        throw new ArgumentException("Type exists!");
                    }
                }

                if (constraintForm.StartsWith("EXCLUSIVE="))
                {
                    newConstraint = ExclusiveConstraint.Parse(constraintForm.Substring("EXCLUSIVE=".Length));

                    if (this.options.predicateConstraints.Contains(newConstraint))
                    {
                        throw new ArgumentException("Type exists!");
                    }
                }

                if (newConstraint == null)
                {
                    throw new ArgumentException("new predicate = null. The predicate form has problem!");
                }

                this.options.predicateConstraints.Add(newConstraint);

                showConstraintList();
                this.constraintListBox.SelectedIndex = this.constraintListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeConstraintBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = constraintListBox.SelectedIndex;
            try
            {
                this.options.predicateConstraints.RemoveAt(selectIndex);
                showConstraintList();
                removeConstraintBtn.Enabled = false;
            }
            catch
            {
            }
        }

        private void showConstraintList()
        {
            this.constraintListBox.Items.Clear();
            this.constraintListBox.Items.AddRange(this.options.predicateConstraints.ToArray());
        }

        private void constraintListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = predicateFormListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                removeConstraintBtn.Enabled = true;
            }
            else
            {
                removeConstraintBtn.Enabled = false;
            }
        }
    }
}
