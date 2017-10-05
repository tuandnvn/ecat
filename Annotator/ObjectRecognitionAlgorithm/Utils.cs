using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class Utils
    {
        public static async Task<List<Object>> DetectObjects(String info, VideoReader videoReader,
                                                                IDepthReader depthReader,
                                                                List<IObjectRecogAlgo> objectRecognizers,
                                                                Dictionary<IObjectRecogAlgo, bool> objectRecognizerIncluded,
                                                                Action<ushort[], CameraSpacePoint[]> mappingFunction)
        {
            List<Object> detectedObjects = new List<Object>();

            int numberOfSteps = (videoReader.frameCount + 1) * objectRecognizerIncluded.Values.Where(v => v).Count();

            if (numberOfSteps == 0) return detectedObjects;

            ProgressForm pf = new ProgressForm();
            pf.Text = info;
            pf.progressBar.Maximum = 100;
            pf.progressBar.Step = 1;

            var progress2 = new Progress<int>(v =>
            {
                // This lambda is executed in context of UI thread,
                // so it can safely update form controls
                pf.progressBar.Value = (v + 1) * 100.0 / numberOfSteps <= 100 ? (int)((v + 1) * 100.0 / numberOfSteps) : 100;
                if (v + 1 < numberOfSteps)
                {
                    pf.description.Text = "Process at frame " + v;
                }
                else
                {
                    pf.description.Text = "Save down the objects ";
                    pf.Dispose();
                }
            });

            Task t = Task.Run(() =>
            {
                int recognizerCounter = 0;

                Console.WriteLine(objectRecognizerIncluded);
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
                            var objects = objectRecognizer.findObjects(videoReader, depthReader, mappingFunction, progress);
                            detectedObjects.AddRange(objects);
                        }

                        recognizerCounter++;
                    }
                }
            });

            pf.StartPosition = FormStartPosition.CenterParent;
            pf.ShowDialog();

            await t;

            return detectedObjects;
        }
    }
}
