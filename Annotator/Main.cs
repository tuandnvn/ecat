using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Threading;
using System.Globalization;
using System.Xml;

namespace Annotator
{
    public partial class Main : Form
    {
        Size previousSize;
        private bool newProject = false;//true if new project is creating
        private bool newSession = false;//true if new session is creating     
        internal Project currentProject = null; //currently selected project
        internal TreeNode currentProjectNode = null;
        internal Session currentSession = null;
        internal TreeNode currentSessionNode = null;
        private VideoReader videoReader = null;      //currently edited video
        private Font myFont = new Font("Microsoft Sans Serif", 5.75f);//font to write not-string colors
        private Point lastAnnotationCell = new Point(94, 0);              // last annotation location for middle-bottom panel
        private Point lastObjectCell = new Point(1, 0);
        //internal List<ObjectAnnotation> objectAnnotations { get; set; }
        internal Dictionary<Object, ObjectAnnotation> objectToObjectTracks { get; set; }
        List<Button> drawingButtonGroup = new List<Button>();
        Dictionary<Button, bool> drawingButtonSelected = new Dictionary<Button, bool>();

        // Increment each time user move frameTrackBar to new Location
        // Keep track of how many bitmaps has not been garbage collected
        // Will use for garbage collection
        private int goToFrameCount = 0;
        private const int GARBAGE_COLLECT_BITMAP_COUNT = 20;

        internal Options options;
        private OptionsForm of;

        bool resizing = false;
        TableLayoutRowStyleCollection rowStyles;
        TableLayoutColumnStyleCollection columnStyles;
        RowStyle[] originalRowStyles;
        ColumnStyle[] originalColumnStyles;

        int colindex = -1;
        int rowindex = -1;

        public Main()
        {
            InitializeComponent();

            // Load options from Options.FILENAME file
            // If the file doesn't exist, or broken, options will be Default
            options = Options.getOption();

            // Initialize some other controls might depends on options
            InitializeOtherControls();

            InitDrawingComponent();
            InitEventAnnoComponent();
            InitMemento();
            
            //Load images to imageList
            //loadImages();

            //comboBox1.SelectedIndex = 0;
            //Just for sample GUI test
            setMinimumFrameTrackBar(0);
            setMaximumFrameTrackBar(100);

            previousSize = this.Size;
            rowStyles = annotateTableLayoutPanel.RowStyles;
            columnStyles = annotateTableLayoutPanel.ColumnStyles;

            originalRowStyles = new RowStyle[rowStyles.Count];
            originalColumnStyles = new ColumnStyle[columnStyles.Count];

            for (int i = 0; i < rowStyles.Count; i++)
            {
                originalRowStyles[i] = new RowStyle(rowStyles[i].SizeType, rowStyles[i].Height);
            }

            for (int i = 0; i < columnStyles.Count; i++)
            {
                originalColumnStyles[i] = new ColumnStyle(columnStyles[i].SizeType, columnStyles[i].Width);
            }

            for (int i = 0; i < rowStyles.Count; i++)
            {
                Console.WriteLine("Row " + i + " " + annotateTableLayoutPanel.GetRowHeights()[i]);
            }

            for (int i = 0; i < columnStyles.Count; i++)
            {
                Console.WriteLine("Column " + i + " " + annotateTableLayoutPanel.GetColumnWidths()[i]);
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        private void InitializeOtherControls()
        {
            // Record panel only for >= windows 8 
            if (System.Environment.OSVersion.Version.Major >= 6 && System.Environment.OSVersion.Version.Minor >= 2)
            {
                this.recordPanel = new Annotator.RecordPanel(options);
            }

            if (recordPanel != null)
            {
                this.recordTab.Controls.Add(this.recordPanel);
            }

            // 
            // recordPanel
            // 
            if (recordPanel != null)
            {
                this.recordPanel.main = this;
                this.recordPanel.Location = new System.Drawing.Point(0, 0);
                this.recordPanel.Name = "recordPanel";
                this.recordPanel.Size = new System.Drawing.Size(1420, 860);
                this.recordPanel.Dock = DockStyle.Fill;
                this.recordPanel.TabIndex = 0;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            recentWorkspaceLocations = new SortedSet<string>();
            //Parameters hidden file open:
            loadParameters();

            //Show workspace launcher at the beginning:
            if (workspace == null)
            {
                WorkspaceLauncher workspaceLauncher = new WorkspaceLauncher(this);
                workspaceLauncher.Show();
            }
            else
            {
                // Try the location folder from the param file
                try
                {
                    loadWorkspace();
                }
                catch (Exception exc)
                {
                    DialogResult r = MessageBox.Show(this, "You will need to use another workspace\n" + exc.ToString(), "Problem open workspace", MessageBoxButtons.OKCancel);

                    if (r == DialogResult.OK)
                    {
                        // If fail, reset workspace
                        WorkspaceLauncher workspaceLauncher = new WorkspaceLauncher(this);
                        workspaceLauncher.Show();
                    }
                }

            }

            this.objectToObjectTracks = new Dictionary<Object, ObjectAnnotation>();
        }

        private void loadImages()
        {
            String folder = Environment.CurrentDirectory + @"\images";
            String[] files = Directory.GetFiles(folder);
            foreach (String imgFileName in files)
            {
                //MessageBox.Show(imgFileName);
                Console.WriteLine(imgFileName);
                imageList1.Images.Add(Image.FromFile(imgFileName));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleEventDataCreateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                SimpleLearningDataGenerator g = new SimpleLearningDataGenerator();

                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    String fullFileName = saveFileDialog.FileName;
                    g.writeExtractedDataIntoFile(currentProject, fullFileName);
                }
            }
        }

        private void setLeftTopPanel()
        {
            if (videoReader != null)
            {
                clearRightBottomPanel();
                //MessageBox.Show(currentVideo.getObjects().Count + "");
                foreach (Object o in currentSession.getObjects())
                {
                    String c = o.color.ToString().Substring(7).Replace(']', ' ');
                    if (c.Contains("="))
                    {
                        c = "(" + o.color.R + "," + o.color.G + "," + o.color.B + ")";
                    }
                    //MessageBox.Show(c);
                    //addObjectToList(o.getID().ToString(), c, o.getType(), o.getBoundingBox().X, o.getBoundingBox().Y, o.getBoundingBox().Width, o.getBoundingBox().Height); 
                }

                clearMiddleCenterPanel();
                clearMidleBottomPanel();
                populateMiddleCenterPanel();
                populateMiddleBottomPanel();
            }
        }


        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                if (recordPanel != null)
                    recordPanel.releaseKinectViewers();
            }

            if (tabs.SelectedIndex == 1)
            {
                if (recordPanel != null)
                    recordPanel.initiateKinectViewers();
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                handleKeyDownOnAnnotatorTab(e);
            }

            if (tabs.SelectedIndex == 1)
            {
                if (e.KeyCode == Keys.D && recordPanel.recordMode != RecordPanel.RecordMode.Playingback && recordPanel != null)
                {
                    recordPanel.rgbBoard.Image.Save("temp.png");
                }
            }
        }

        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            if (tabs.SelectedIndex == 0)
            {
                handleKeyUpOnAnnotatorTab(e);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Optionf form
            of = new OptionsForm(options);
            of.StartPosition = FormStartPosition.CenterParent;
            of.ShowDialog();
        }


        private void annotateTableLayoutPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                rowStyles = annotateTableLayoutPanel.RowStyles;
                columnStyles = annotateTableLayoutPanel.ColumnStyles;
                resizing = true;
            }
        }

        private void annotateTableLayoutPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!resizing)
            {
                float width = 0;
                float height = 0;

                rowindex = -1;
                colindex = -1;
                annotateTableLayoutPanel.Cursor = Cursors.Default;

                //for rows
                for (int i = 0; i < rowStyles.Count; i++)
                {
                    height += annotateTableLayoutPanel.GetRowHeights()[i];
                    if (e.Y > height - 3 && e.Y < height + 3)
                    {
                        rowindex = i;
                        annotateTableLayoutPanel.Cursor = Cursors.HSplit;
                        break;
                    }
                }

                //for columns
                for (int i = 0; i < columnStyles.Count; i++)
                {
                    width += annotateTableLayoutPanel.GetColumnWidths()[i];
                    if (e.X > width - 3 && e.X < width + 3)
                    {
                        colindex = i;
                        if (rowindex > -1)
                            annotateTableLayoutPanel.Cursor = Cursors.Cross;
                        else
                            annotateTableLayoutPanel.Cursor = Cursors.VSplit;
                        break;
                    }
                    else
                    {
                        if (rowindex == -1)
                            annotateTableLayoutPanel.Cursor = Cursors.Default;
                    }
                }
            }

            if (resizing && (colindex > -1 || rowindex > -1))
            {
                float width = e.X;
                float height = e.Y;


                if (colindex > -1)
                {
                    var originalWidth = annotateTableLayoutPanel.GetColumnWidths()[colindex];


                    for (int i = 0; i < colindex; i++)
                    {
                        width -= annotateTableLayoutPanel.GetColumnWidths()[i];
                    }

                    columnStyles[colindex].SizeType = SizeType.Absolute;
                    columnStyles[colindex].Width = width;

                    if (colindex < columnStyles.Count - 1)
                    {
                        var nextColWidth = annotateTableLayoutPanel.GetColumnWidths()[colindex + 1] + originalWidth - width;
                        columnStyles[colindex + 1].SizeType = SizeType.Absolute;
                        columnStyles[colindex + 1].Width = nextColWidth;
                    }
                }

                if (rowindex > -1)
                {
                    var originalHeight = annotateTableLayoutPanel.GetRowHeights()[rowindex];

                    for (int i = 0; i < rowindex; i++)
                    {
                        height -= annotateTableLayoutPanel.GetRowHeights()[i];
                    }

                    var initalRowStyle = rowStyles[rowindex].SizeType;

                    rowStyles[rowindex].SizeType = SizeType.Absolute;
                    rowStyles[rowindex].Height = height;

                    if (rowindex < rowStyles.Count - 1)
                    {
                        var nextRowHeight = annotateTableLayoutPanel.GetRowHeights()[rowindex + 1] + originalHeight - height;
                        rowStyles[rowindex + 1].SizeType = SizeType.Absolute;
                        rowStyles[rowindex + 1].Height = nextRowHeight;
                    }
                }
            }
        }

        private void annotateTableLayoutPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                resizing = false;
                annotateTableLayoutPanel.Cursor = Cursors.Default;

                for (int i = 0; i < rowStyles.Count; i++)
                {
                    if (originalRowStyles[i].SizeType == SizeType.Percent)
                    {
                        rowStyles[i].SizeType = originalRowStyles[i].SizeType;
                        rowStyles[i].Height = originalRowStyles[i].Height;
                    }
                }

                for (int i = 0; i < columnStyles.Count; i++)
                {
                    if (originalColumnStyles[i].SizeType == SizeType.Percent)
                    {
                        columnStyles[i].SizeType = originalColumnStyles[i].SizeType;
                        columnStyles[i].Width = originalColumnStyles[i].Width;
                    }
                }
            }
        }

        private void annotateTableLayoutPanel_MouseLeave(object sender, EventArgs e)
        {
            annotateTableLayoutPanel.Cursor = Cursors.Default;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoForm iff = new InfoForm();
            iff.StartPosition = FormStartPosition.CenterParent;
            iff.ShowDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveParameters();
        }

        private void middleCenterPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
