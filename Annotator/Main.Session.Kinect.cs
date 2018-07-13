using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class Main
    {
        CountdownEvent isAvailable;
        volatile bool sensorAvailabel = false;
        volatile bool colorFrameArrived = false;
        volatile bool depthFrameArrived = false;
        volatile bool currentlySetupKinect = false;
        KinectSensor kinectSensor;
        internal CoordinateMapper coordinateMapper;
        DepthCoordinateMappingReader mappingReader;


        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (!sensorAvailabel && currentlySetupKinect)
            {
                isAvailable.Signal();
                sensorAvailabel = true;
            }
        }

        private void Reader_DepthFrameArrived(object sender, DepthFrameArrivedEventArgs e)
        {
            if (!depthFrameArrived && currentlySetupKinect)
            {
                isAvailable.Signal();
                depthFrameArrived = true;
            }
        }

        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            if (!colorFrameArrived && currentlySetupKinect)
            {
                isAvailable.Signal();
                colorFrameArrived = true;
            }
        }


        private void detectGlyphBox2dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(currentSession, options.prototypeList, 5);
                var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
                objectRecognizerIncluded[objectRecognizer] = true;

                Task t = Task.Run(async () =>
                {
                    List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
                    null,
                    new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                    null);

                    // Run on UI thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        addDetectedObjects(detectedObjects);
                    });
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void detectGlyphBox3dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(currentSession, options.prototypeList, 5);
                var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
                objectRecognizerIncluded[objectRecognizer] = true;
                setupKinectIfNeeded();

                Task t = Task.Run(async () =>
                {
                    waitForKinect(5000);

                    if (isAvailable.IsSet)
                    {
                        List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
                            currentSession.getDepth(0),
                            new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                            coordinateMapper.MapColorFrameToCameraSpace
                        );

                        // Run on UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            addDetectedObjects(detectedObjects);
                        });
                    }
                    else
                    {
                        // Run on UI thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Waiting for a Kinect sensor in 5 seconds. There is no  Kinect sensor available.",
                            "Generation error",
                            MessageBoxButtons.OK);
                        });
                    }
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        private void waitForKinect(int mili)
        {
            if (currentlySetupKinect)
            {
                Console.WriteLine("Await");
                isAvailable.Wait(mili);

                currentlySetupKinect = false;
            }
        }

        internal void setupKinectIfNeeded()
        {
            if (kinectSensor == null || !kinectSensor.IsAvailable)
            {
                isAvailable = new CountdownEvent(3);
                sensorAvailabel = false;
                colorFrameArrived = false;
                depthFrameArrived = false;
                kinectSensor = KinectSensor.GetDefault();
                coordinateMapper = kinectSensor.CoordinateMapper;
                var colorFrameReader = this.kinectSensor.ColorFrameSource.OpenReader();
                var depthFrameReader = this.kinectSensor.DepthFrameSource.OpenReader();
                colorFrameReader.FrameArrived += this.Reader_ColorFrameArrived;
                depthFrameReader.FrameArrived += this.Reader_DepthFrameArrived;
                kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
                kinectSensor.Open();
                currentlySetupKinect = true;
            }
        }

        private void addDetectedObjects(List<Object> detectedObjects)
        {
            addObjectsIntoSession(detectedObjects);
            refreshUI(detectedObjects);
        }

        private void refreshUI(List<Object> detectedObjects)
        {
            // Redraw object annotation panel
            if (detectedObjects.Count != 0)
            {
                foreach (Object o in detectedObjects)
                {
                    addObjectAnnotation(o);
                }
                invalidatePictureBoard();
            }
        }

        private void addObjectsIntoSession(List<Object> detectedObjects)
        {
            // Handle adding identical objects or not
            switch (options.detectionMode)
            {
                case Options.OverwriteMode.ADD_SEPARATE:
                    foreach (var detectedObject in detectedObjects)
                    {
                        currentSession.addObject(detectedObject);
                    }
                    break;
                case Options.OverwriteMode.NO_OVERWRITE:
                    foreach (GlyphBoxObject detectedObject in detectedObjects)
                    {
                        bool exist = false;
                        foreach (var existObject in currentSession.getObjects())
                        {
                            if (existObject is GlyphBoxObject && detectedObject.boxPrototype.Equals(((GlyphBoxObject)existObject).boxPrototype))
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (!exist)
                        {
                            currentSession.addObject(detectedObject);
                        }
                    }
                    break;
                case Options.OverwriteMode.OVERWRITE:
                    foreach (GlyphBoxObject detectedObject in detectedObjects)
                    {
                        Object exist = null;
                        foreach (var existObject in currentSession.getObjects())
                        {

                            if (existObject is GlyphBoxObject && detectedObject.boxPrototype.Equals(((GlyphBoxObject)existObject).boxPrototype))
                            {
                                exist = existObject;
                                break;
                            }
                        }

                        if (exist != null)
                        {
                            currentSession.removeObject(exist.id);
                        }
                        currentSession.addObject(detectedObject);
                    }
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///This part is used to generate some data from session, but not it is better to generate directly from param files.///////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                LearningDataGenerator g = new SimpleLearningDataGenerator();

                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    String fullFileName = saveFileDialog.FileName;
                    g.writeExtractedDataIntoFile(currentProject, fullFileName);
                }
            }
        }

        /// <summary>
        /// Deprecated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentProject != null)
            {
                LearningDataGenerator g = new EventLearningDataGenerator();

                System.Windows.Forms.SaveFileDialog saveFileDialog = new SaveFileDialog();
                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK) // Test result.
                {
                    String fullFileName = saveFileDialog.FileName;
                    g.writeExtractedDataIntoFile(currentProject, fullFileName);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////This part is used to generate objects without using Kinect camera.//////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        private void sessionOfflineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(currentSession, options.prototypeList, 5);
            var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizerIncluded[objectRecognizer] = true;

            if (mappingReader == null)
            {
                try
                {
                    mappingReader = new DepthCoordinateMappingReader("coordinateMapping.dat");
                }
                catch (Exception exc)
                {
                    MessageBox.Show("3D coordinations of this object could not be generated. It is very likely that there is no depth file, or the depth file is corrupted or not of the correct format",
                        "Generation error",
                        MessageBoxButtons.OK);
                    return;
                }
            }

            Task t = Task.Run(async () =>
            {
                List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
                currentSession.getDepth(0),
                new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                (depthImage, result) => mappingReader.projectDepthImageToCameraSpacePoint(depthImage,
                    currentSession.getDepth(0).depthWidth,
                    currentSession.getDepth(0).depthHeight,
                    currentSession.getVideo(0).frameWidth,
                    currentSession.getVideo(0).frameHeight, result)
                    );

                // Run on UI thread
                this.Invoke((MethodInvoker)delegate
                {
                    addDetectedObjects(detectedObjects);
                });

            });
        }

    }
}
