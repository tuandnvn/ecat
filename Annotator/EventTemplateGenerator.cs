using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class EventTemplateGenerator : Form
    {
        private Main main;
        private bool isProjectMode = false;
        private ToolTip startFrameTxtBoxTt = new ToolTip();
        private ToolTip skippingLengthTxtBoxTt = new ToolTip();
        private ToolTip durationTxtBoxTt = new ToolTip();

        public EventTemplateGenerator(Main main, bool isProjectMode)
        {
            this.main = main;
            this.isProjectMode = isProjectMode;
            InitializeComponent();
        }

        private void genBtn_Click(object sender, EventArgs e)
        {
            bool satisfied = true;

            int startFrame = default(int);
            int skippingLength = default(int);
            int duration = default(int);

            try
            {
                startFrame = int.Parse(startFrameTxtBox.Text);

                if (startFrame < 0)
                    throw new ArgumentOutOfRangeException("startFrame < 0");

                if (!isProjectMode && main.currentSession != null && startFrame >= main.currentSession.frameLength)
                    throw new ArgumentOutOfRangeException("startFrame >= session length");
            }
            catch (Exception exc)
            {
                startFrameTxtBoxTt.Show("Value need to be an integer >=0 and < " + main.currentSession.frameLength, startFrameTxtBox, 194, 0);
                satisfied &= false;
            }

            try
            {
                skippingLength = int.Parse(skippingLengthTxtBox.Text);

                if (skippingLength <= 0)
                    throw new ArgumentOutOfRangeException("skippingLength <= 0");

                if (!isProjectMode && main.currentSession != null && skippingLength >= main.currentSession.frameLength)
                    throw new ArgumentOutOfRangeException("skippingLength >= session length");
            }
            catch (Exception exc)
            {
                skippingLengthTxtBoxTt.Show("Value need to be an integer >=0 and < " + main.currentSession.frameLength, skippingLengthTxtBox, 194, 0);
                satisfied &= false;
            }


            try
            {
                duration = int.Parse(durationTxtBox.Text);

                if (duration <= 0)
                    throw new ArgumentOutOfRangeException("duration <= 0");

                if (!isProjectMode && main.currentSession != null && duration >= main.currentSession.frameLength)
                    throw new ArgumentOutOfRangeException("duration out of range");
            }
            catch (Exception exc)
            {
                durationTxtBoxTt.Show("Value need to be an integer >=0 and < " + main.currentSession.frameLength, durationTxtBox, 194, 0);
                satisfied &= false;
            }

            string templateDescription = templateDescriptionTxtBox.Text;

            var overwriteMode = default(Options.OverwriteMode);
            if (separateEventRb.Checked)
            {
                overwriteMode = Options.OverwriteMode.ADD_SEPARATE;
            }
            else if (overwriteEventRb.Checked)
            {
                overwriteMode = Options.OverwriteMode.OVERWRITE;
            }
            else if (ignoreEventRb.Checked)
            {
                overwriteMode = Options.OverwriteMode.NO_OVERWRITE;
            }
            else if (removeExistingRb.Checked)
            {
                overwriteMode = Options.OverwriteMode.REMOVE_EXISTING;
            }

            if (!satisfied)
                return;

            if (!isProjectMode && main.currentSession != null)
            {
                var addedEvents = main.currentSession.addEventTemplate(startFrame, skippingLength, duration, templateDescription, overwriteMode);
                foreach (var ev in addedEvents)
                    main.addAnnotation(ev);
            }


            if (isProjectMode && main.currentProject != null)
                foreach (var sess in main.currentProject.sessions)
                {
                    main.currentSession = sess;
                    main.currentSession.loadIfNotLoaded();
                    main.currentSession.addEventTemplate(startFrame, skippingLength, duration, templateDescription, overwriteMode);
                    main.currentSession.saveSession();
                }
            this.Dispose();
        }

        private void startFrameTxtBox_Enter(object sender, EventArgs e)
        {
            startFrameTxtBoxTt.Hide(startFrameTxtBox);
        }

        private void skippingLengthTxtBox_Enter(object sender, EventArgs e)
        {
            skippingLengthTxtBoxTt.Hide(skippingLengthTxtBox);
        }

        private void durationTxtBox_Enter(object sender, EventArgs e)
        {
            durationTxtBoxTt.Hide(durationTxtBox);
        }
    }
}
