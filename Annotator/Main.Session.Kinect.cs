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



        private void sessionOnlineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(null, options.prototypeList, 5);
                var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
                objectRecognizerIncluded[objectRecognizer] = true;
                setupKinectIfNeeded();

                Task t = Task.Run(async () =>
                {

                    if (currentlySetupKinect)
                    {
                        Console.WriteLine("Await");
                        isAvailable.Wait();
                        currentlySetupKinect = false;
                    }

                    List<Object> detectedObjects = await Utils.DetectObjects("Progress on " + currentSession.sessionName, currentSession.getVideo(0),
                    currentSession.getDepth(0),
                    new List<IObjectRecogAlgo> { objectRecognizer }, objectRecognizerIncluded,
                    coordinateMapper.MapColorFrameToCameraSpace
                        );

                    // Run on UI thread
                    this.Invoke((MethodInvoker)delegate
                    {
                        AddDetectedObjects(detectedObjects);
                    });
                });
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
        }

        internal void setupKinectIfNeeded()
        {
            if (kinectSensor == null)
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

        private void sessionOfflineModeGlyphDetectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IObjectRecogAlgo objectRecognizer = new GlyphBoxObjectRecognition(null, options.prototypeList, 5);
            var objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizerIncluded[objectRecognizer] = true;

            if (mappingReader == null)
            {
                mappingReader = new DepthCoordinateMappingReader("coordinateMapping.dat");
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
                    AddDetectedObjects(detectedObjects);
                });

            });
        }

        private void AddDetectedObjects(List<Object> detectedObjects)
        {
            AddObjectsIntoSession(detectedObjects);
            RefreshUI(detectedObjects);
        }

        private void RefreshUI(List<Object> detectedObjects)
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

        private void AddObjectsIntoSession(List<Object> detectedObjects)
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
    }
}
