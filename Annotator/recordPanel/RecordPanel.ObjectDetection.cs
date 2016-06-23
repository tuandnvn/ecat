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

        public void InitializeObjectRecognizers()
        {
            objectRecognizers = new List<IObjectRecogAlgo>();
            objectRecognizers.Add(new GlyphBoxObjectRecognition(null, new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 }, 5));
        }

        public void InitializeOptionsTableDetection()
        {
            int counter = 10;

            foreach (var objectRecognizer in objectRecognizers)
            {
                optionsTable.Rows.Add(objectRecognizer.getName(), "False");
                changeRowToTrueFall(optionsTable, counter, 1);
                counter++;
            }
        }

        public async Task DetectObjects()
        {
            detectedObjects = new List<Object>();


            ProgressForm pf = new ProgressForm();
            pf.progressBar.Maximum = 100;
            pf.progressBar.Step = 1;

            var progress = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                pf.progressBar.Value = v * 100.0 / videoReader.frameCount  <= 100 ? (int)(v * 100.0 / videoReader.frameCount) : 100;
                if ( v < videoReader.frameCount)
                {
                    pf.description.Text = "Process at frame " + v;
                } else
                {
                    pf.description.Text = "Save down the objects ";
                }

                if (v == videoReader.frameCount)
                {
                    pf.Dispose();
                }
            });

            Task t = Task.Run(() =>
            {
                foreach (var objectRecognizer in objectRecognizers)
                {
                    if (videoReader != null && depthReader != null)
                    {
                        var objects = objectRecognizer.findObjects(videoReader, depthReader, this.coordinateMapper.MapColorFrameToCameraSpace, progress);
                        Console.WriteLine("objects.Count " + objects.Count);
                        detectedObjects.AddRange(objects);
                        Console.WriteLine("objects.Count " + detectedObjects.Count);
                    }
                }
            });

            pf.StartPosition = FormStartPosition.CenterParent;
            pf.ShowDialog();

            await t;
        }
    }
}
