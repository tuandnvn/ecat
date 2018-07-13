using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class Main
    {
        internal void removeObject(Object o)
        {
            removeDrawingObject(o);
            currentSession.removeObject(o.id);

            // Remove the annotation corresponding to this object
            // and rearrage all object annotations
            if (this.objectToObjectTracks.ContainsKey(o))
            {
                ObjectAnnotation oa = this.objectToObjectTracks[o];

                this.objectToObjectTracks.Remove(o);

                //// BUGGY
                //if (oa != null)
                //{
                //    var index = this.objectToObjectTracks.Keys.ToList().IndexOf(o);

                //    this.objectToObjectTracks.Remove(o);

                //    middleCenterTableLayoutPanel.Controls.Remove(oa);

                //    // Move all the object annotations following ot up one step
                //    for (var i = index; i < this.objectToObj2wsectTracks.Keys.Count; i++)
                //    {
                //        var moveObjectAnnotation = this.objectToObjectTracks[this.objectToObjectTracks.Keys.ToList()[i]];
                //        middleCenterTableLayoutPanel.Controls.Remove(moveObjectAnnotation);
                //        middleCenterTableLayoutPanel.Controls.Add(moveObjectAnnotation, lastObjectCell.X, i);
                //    }

                //    middleCenterTableLayoutPanel.RowStyles.RemoveAt(middleCenterTableLayoutPanel.RowStyles.Count - 1);
                //    middleCenterTableLayoutPanel.RowCount = lastObjectCell.Y - 1;
                //    middleCenterTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleCenterTableLayoutPanel.RowCount + 4);
                //    lastObjectCell.Y--;
                //}

                lastObjectCell.Y = 0;
                middleCenterTableLayoutPanel.Controls.Clear();
                foreach (Object obj in this.objectToObjectTracks.Keys)
                {
                    renderObjectAnnotation(this.objectToObjectTracks[obj]);
                }

                middleCenterPanel.Invalidate();
            }
        }

        private void renderObjectAnnotation(ObjectAnnotation objectAnnotation)
        {
            renderObjectAnnotationWithoutInvalidate(objectAnnotation);

            middleCenterPanel.Invalidate();
        }

        private void renderObjectAnnotationWithoutInvalidate(ObjectAnnotation objectAnnotation)
        {
            middleCenterTableLayoutPanel.RowCount = lastObjectCell.Y + 1;
            middleCenterTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            middleCenterTableLayoutPanel.Size = new System.Drawing.Size(970, 60 * middleCenterTableLayoutPanel.RowCount + 4);
            middleCenterTableLayoutPanel.Controls.Add(objectAnnotation, lastObjectCell.X, lastObjectCell.Y);
            objectAnnotation.Dock = DockStyle.Fill;

            lastObjectCell.Y = lastObjectCell.Y + 1;
        }

        internal void selectObject(Object o)
        {
            cancelSelectObject();

            //Remove any decoration of other objects
            foreach (Object other in objectToObjectTracks.Keys)
            {
                objectToObjectTracks[other].deselectDeco();
            }
            objectToObjectTracks[o].selectDeco();

            this.selectedObject = o;

            if (selectedObject != null)
            {
                selectObjContextPanel.Visible = true;
            }

            foreach (Button b in drawingButtonGroup)
            {
                selectButtonDrawing(b, drawingButtonGroup, false);
            }

            this.showInformation(o);
            this.showPredicates();

            //polygonDrawing.Enabled = false;
            //rectangleDrawing.Enabled = false;

            invalidatePictureBoard();
        }

        internal void addObjectAnnotation(Object o)
        {
            var objectAnnotation = new ObjectAnnotation(o, this, this.frameTrackBar.Minimum, this.frameTrackBar.Maximum);
            //objectAnnotations.Add(objectAnnotation);
            objectToObjectTracks[o] = objectAnnotation;

            //objectAnnotation.Location = lastObjectCell;
            //if (lastObjectCell.Y >= 0) return;

            renderObjectAnnotation(objectAnnotation);
        }

        public delegate void Map(ushort[] depthImage, CameraSpacePoint[] result);

        internal void generate3D(Object o)
        {
            try
            {
                // Current assumption is that the depth field is the first one
                var videoReader = o.session.getVideo(0);
                var depthReader = o.session.getDepth(0);

                setupKinectIfNeeded();

                if (videoReader != null && depthReader != null)
                {
                    // To run some lazy evaluation
                    depthReader.getWidth();

                    Task t = Task.Run(async () =>
                    {
                        waitForKinect(1000);

                        if (isAvailable.IsSet)
                        {
                            if (o is GlyphBoxObject)
                            {
                                o.generate3dForGlyph(videoReader, depthReader, coordinateMapper.MapColorFrameToCameraSpace);
                            }
                            else
                            {
                                o.generate3d(videoReader, depthReader, coordinateMapper.MapColorFrameToCameraSpace);
                            }
                        }
                        else
                        {
                            // Run on UI thread
                            this.Invoke((MethodInvoker)delegate
                            {
                            MessageBox.Show("Waiting for a Kinect sensor in 1 seconds. Because there is no Kinect sensor, we will use the default mapping method.",
                            "Generation warning",
                            MessageBoxButtons.OK);

                            try
                            {
                                var mappingReader = new DepthCoordinateMappingReader("coordinateMapping.dat");

                                Action<ushort[], CameraSpacePoint[]> mappingFunction = delegate (ushort[] depthImage, CameraSpacePoint[] result)
                                {
                                    mappingReader.projectDepthImageToCameraSpacePoint(depthImage,
                                    currentSession.getDepth(0).depthWidth,
                                    currentSession.getDepth(0).depthHeight,
                                    currentSession.getVideo(0).frameWidth,
                                    currentSession.getVideo(0).frameHeight, result);
                                };

                                if (o is GlyphBoxObject)
                                {
                                    o.generate3dForGlyph(videoReader, depthReader, mappingFunction );
                                }
                                else
                                {
                                    o.generate3d(videoReader, depthReader, mappingFunction);
                                }
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("Mapping file (coordinateMapping.dat) is not available", "Generation error",
                                    MessageBoxButtons.OK);
                            }
                        });
                        }
                    });
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("3D coordinations of this object could not be generated. It is very likely that there is no depth file, or the depth file is corrupted or not of the correct format",
                    "Generation error",
                    MessageBoxButtons.OK);
            }
        }
    }
}
