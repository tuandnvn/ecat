using Annotator.ObjectRecognitionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class RecordPanel
    {
        List<Object> detectedObjects;
        List<IObjectRecogAlgo> objectRecognizers;
        /// <summary>
        /// After some default rows,
        /// this is the first row index to set whether to include object recoginizer
        /// </summary>
        int objectRecognizerStartRow = 10;

        /// <summary>
        /// Each value corresponds to whether to include the object recognizer or not
        /// </summary>
        Dictionary<IObjectRecogAlgo, bool> objectRecognizerIncluded;

        public void InitializeObjectRecognizers()
        {
            objectRecognizers = new List<IObjectRecogAlgo>();
            objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizers.Add(new GlyphBoxObjectRecognition(null, new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 }, 5));
        }

        private void changeObjectRecognizerIncluded(DataGridViewCellEventArgs e)
        {
            var objectRecognizer = objectRecognizers[e.RowIndex - objectRecognizerStartRow];
            bool recognizerIncluded = (bool)optionsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            objectRecognizerIncluded[objectRecognizer] = recognizerIncluded;
        }

        public void InitializeOptionsTableDetection()
        {
            foreach (var objectRecognizer in objectRecognizers)
            {
                objectRecognizerIncluded[objectRecognizer] = true;
                int rowIndex = optionsTable.Rows.Add(objectRecognizer.getName(), "False");
                changeRowToTrueFall(optionsTable, rowIndex, 1);
            }
        }

        public async Task DetectObjects()
        {
            detectedObjects = new List<Object>();

            int numberOfSteps = (videoReader.frameCount + 1) * objectRecognizerIncluded.Values.Where(v => v).Count();

            if (numberOfSteps == 0) return;

            ProgressForm pf = new ProgressForm();
            pf.progressBar.Maximum = 100;
            pf.progressBar.Step = 1;

            var progress2 = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                pf.progressBar.Value = v * 100.0 / numberOfSteps <= 100 ? (int)(v * 100.0 / numberOfSteps) : 100;
                if ( v < numberOfSteps)
                {
                    pf.description.Text = "Process at frame " + v;
                } else
                {
                    pf.description.Text = "Save down the objects ";
                }

                if (v == numberOfSteps)
                {
                    pf.Dispose();
                }
            });

            Task t = Task.Run(() =>
            {
                int recognizerCounter = 0;
                foreach (var objectRecognizer in objectRecognizers)
                {
                    if (objectRecognizerIncluded[objectRecognizer])
                    {
                        var progress = new Progress<int>(v =>
                        {
                            (progress2 as IProgress<int>).Report(recognizerCounter * (videoReader.frameCount + 1) + v);
                        });

                        if (videoReader != null && depthReader != null)
                        {
                            var objects = objectRecognizer.findObjects(videoReader, depthReader, this.coordinateMapper.MapColorFrameToCameraSpace, progress);
                            detectedObjects.AddRange(objects);
                        }

                        recognizerCounter++;
                    }
                }
            });

            pf.StartPosition = FormStartPosition.CenterParent;
            pf.ShowDialog();

            await t;
        }
    }
}
