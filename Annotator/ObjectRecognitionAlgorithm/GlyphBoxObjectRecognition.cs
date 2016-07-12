using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Emgu.CV;
using System.Drawing;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using Accord.Math;
using System.Diagnostics;

namespace Annotator
{
    public class GlyphBoxObjectRecognition : IObjectRecogAlgo
    {
        /// <summary>
        /// Recognize multiple box only
        /// Boxes should have different faces but share the same glyphSize
        /// </summary>
        protected List<GlyphBoxPrototype> boxPrototypes;
        protected int glyphSize;
        protected Session currentSession;

        // Anchor frame number
        // Object detection algorithm run for the whole frame
        // if the frame is divided by anchorNumber, otherwise 
        // only looking for glyphs in the neighborhood of previously detected
        // frames
        static int anchorNumber = 20;

        // Extension frame 
        static int extensionFrame = 50;

        public GlyphBoxObjectRecognition(Session currentSession, List<GlyphBoxPrototype> boxPrototypes, int glyphSize)
        {
            this.currentSession = currentSession;
            this.boxPrototypes = boxPrototypes;
            this.glyphSize = glyphSize;

            foreach (var boxPrototype in boxPrototypes)
            {
                if (boxPrototype.glyphSize != glyphSize)
                {
                    throw new ArgumentException("Glyph size of all box prototype should be the same as 'glyphSize' ");
                }
            }
        }

        public List<Object> findObjects(VideoReader videoReader, IDepthReader depthReader, Action<ushort[], CameraSpacePoint[]> mappingFunction, IProgress<int> progress)
        {
            var shapeOptimizer = new FlatAnglesOptimizer(160);
            Console.WriteLine("Find glyph box");
            List<Object> objects = new List<Object>();

            if (videoReader == null)
                return objects;

            /// For each frame (int frameNo)
            /// For each recognized glyph in frame (int faceIndex)
            /// Store A tuple of 
            ///              -    A list of bounding points for recognized glyph
            ///              -    A glyphface instance
            var recognizedGlyphs = new Dictionary<int, Dictionary<int, Dictionary<int, Tuple<List<System.Drawing.PointF>, GlyphFace, List<Point3>>>>>();

            Bitmap image = null;
            Mat m = null;
            Bitmap grayImage = null;
            Bitmap edges = null;
            UnmanagedImage grayUI = null;
            Bitmap transformed = null;
            Bitmap transformedOtsu = null;


            //A control flag, true if at the previous frame loop, there is detection of some glyph
            // When this is true, only searching for glyph box in some neighborhood of the previous glyphs
            // if the frame is not an anchor frame
            bool previousFrameDetection = false;

            for (int frameNo = 0; frameNo < videoReader.frameCount; frameNo++)
                //for (int frameNo = 0; frameNo < 1; frameNo++)
            {
                if (progress != null)
                    progress.Report(frameNo);

                Console.WriteLine("=============================================");
                Console.WriteLine("Frame no " + frameNo);
                m = videoReader.getFrame(frameNo);
                if (m == null)
                {
                    break;
                }

                var startPos = new System.Drawing.Point();

                getImageForProcessing(recognizedGlyphs, m, previousFrameDetection, frameNo, ref image, ref startPos);

                // Reset right after using
                previousFrameDetection = false;


                Stopwatch stopwatch = Stopwatch.StartNew();

                /// Adapt from Glyph Recognition Prototyping
                /// Copyright © Andrew Kirillov, 2009-2010
                /// 
                // 1 - Grayscale
                grayImage = Grayscale.CommonAlgorithms.BT709.Apply(image);

                stopwatch.Stop();
                Console.WriteLine("Gray scale time = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();

                // 2 - Edge detection
                DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector();
                edges = edgeDetector.Apply(grayImage);

                stopwatch.Stop();
                Console.WriteLine("Edge detection time = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();

                // 3 - Threshold edges
                // Was set to 20 and the number of detected glyphs are too low
                // Should be set higher
                Threshold thresholdFilter = new Threshold(45);
                thresholdFilter.ApplyInPlace(edges);

                stopwatch.Stop();
                Console.WriteLine("Threshold time = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();

                // 4 - Blob Counter
                BlobCounter blobCounter = new BlobCounter();
                blobCounter.MinHeight = 32;
                blobCounter.MinWidth = 32;
                blobCounter.FilterBlobs = true;
                blobCounter.ObjectsOrder = ObjectsOrder.Size;

                blobCounter.ProcessImage(edges);
                Blob[] blobs = blobCounter.GetObjectsInformation();

                stopwatch.Stop();
                Console.WriteLine("Blob finding time = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();

                //// create unmanaged copy of source image, so we could draw on it
                //UnmanagedImage imageData = UnmanagedImage.FromManagedImage(image);

                // Get unmanaged copy of grayscale image, so we could access it's pixel values
                grayUI = UnmanagedImage.FromManagedImage(grayImage);

                // list of found dark/black quadrilaterals surrounded by white area
                List<List<IntPoint>> foundObjects = new List<List<IntPoint>>();
                // shape checker for checking quadrilaterals
                SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

                Console.WriteLine("edgePoints");

                // 5 - check each blob
                for (int i = 0, n = blobs.Length; i < n; i++)
                {
                    List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);

                    List<IntPoint> corners = null;

                    // does it look like a quadrilateral ?
                    if (shapeChecker.IsQuadrilateral(edgePoints, out corners))
                    {
                        // do some more checks to filter so unacceptable shapes
                        // if ( CheckIfShapeIsAcceptable( corners ) )
                        {

                            // get edge points on the left and on the right side
                            List<IntPoint> leftEdgePoints, rightEdgePoints;
                            blobCounter.GetBlobsLeftAndRightEdges(blobs[i], out leftEdgePoints, out rightEdgePoints);

                            // calculate average difference between pixel values from outside of the shape and from inside
                            float diff = this.CalculateAverageEdgesBrightnessDifference(
                                leftEdgePoints, rightEdgePoints, grayUI);

                            // check average difference, which tells how much outside is lighter than inside on the average
                            if (diff > 20)
                            {
                                //Drawing.Polygon(imageData, corners, Color.FromArgb(255, 255, 0, 0));
                                // add the object to the list of interesting objects for further processing
                                foundObjects.Add(corners);
                            }
                        }
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Finding black quadiralateral surrounded by white area = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();


                int recordedTimeForRgbFrame = (int)(videoReader.totalMiliTime * frameNo / (videoReader.frameCount - 1));

                CameraSpacePoint[] csps = new CameraSpacePoint[videoReader.frameWidth * videoReader.frameHeight];
                if (depthReader != null)
                {
                    ushort[] depthValues = depthReader.readFrameAtTime(recordedTimeForRgbFrame);
                    mappingFunction(depthValues, csps);
                }

                stopwatch.Stop();
                Console.WriteLine("Mapping into 3 dimensional = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();


                // further processing of each potential glyph
                foreach (List<IntPoint> corners in foundObjects)
                {
                    Console.WriteLine("found some corner");
                    // 6 - do quadrilateral transformation
                    QuadrilateralTransformation quadrilateralTransformation =
                        new QuadrilateralTransformation(corners, 20 * (glyphSize + 2), 20 * (glyphSize + 2));

                    transformed = quadrilateralTransformation.Apply(grayImage);

                    // 7 - otsu thresholding
                    OtsuThreshold otsuThresholdFilter = new OtsuThreshold();
                    transformedOtsu = otsuThresholdFilter.Apply(transformed);

                    // +2 for offset
                    int glyphSizeWithBoundary = glyphSize + 2;
                    SquareBinaryGlyphRecognizer gr = new SquareBinaryGlyphRecognizer(glyphSizeWithBoundary);

                    bool[,] glyphValues = gr.Recognize(ref transformedOtsu,
                        new Rectangle(0, 0, 20 * (glyphSize + 2), 20 * (glyphSize + 2)));

                    bool[,] resizedGlyphValues = new bool[glyphSize, glyphSize];

                    for (int i = 0; i < glyphSize; i++)
                        for (int j = 0; j < glyphSize; j++)
                        {
                            resizedGlyphValues[i, j] = glyphValues[i + 1, j + 1];
                        }


                    GlyphFace face = new GlyphFace(resizedGlyphValues, glyphSize);

                    Console.WriteLine("Find glyph face " + face.ToString());

                    // Transfer back to original coordinates
                    List<IntPoint> originalCorners = new List<IntPoint>();
                    foreach (var corner in corners)
                    {
                        IntPoint p = new IntPoint(corner.X + startPos.X, corner.Y + startPos.Y);
                        originalCorners.Add(p);
                    }

                    Console.WriteLine("Corner points");
                    foreach (var corner in originalCorners)
                    {
                        Console.WriteLine(corner);
                    }

                    for (int boxPrototypeIndex = 0; boxPrototypeIndex < boxPrototypes.Count; boxPrototypeIndex++)
                    {
                        var boxPrototype = boxPrototypes[boxPrototypeIndex];
                        foreach (int faceIndex in boxPrototype.indexToGlyphFaces.Keys)
                        {
                            if (face.Equals(boxPrototype.indexToGlyphFaces[faceIndex]))
                            {
                                if (!recognizedGlyphs.ContainsKey(boxPrototypeIndex))
                                {
                                    Console.WriteLine("Detect new type of prototype " + boxPrototypeIndex);
                                    recognizedGlyphs[boxPrototypeIndex] = new Dictionary<int, Dictionary<int, Tuple<List<System.Drawing.PointF>, GlyphFace, List<Point3>>>>();
                                }

                                if (!recognizedGlyphs[boxPrototypeIndex].ContainsKey(frameNo))
                                {
                                    Console.WriteLine("Detect glyph at frame " + frameNo + " for prototype " + boxPrototypeIndex);
                                    if (!previousFrameDetection)
                                    {
                                        previousFrameDetection = true;
                                    }

                                    recognizedGlyphs[boxPrototypeIndex][frameNo] = new Dictionary<int, Tuple<List<System.Drawing.PointF>, GlyphFace, List<Point3>>>();
                                }

                                recognizedGlyphs[boxPrototypeIndex][frameNo][faceIndex] = new Tuple<List<System.Drawing.PointF>, GlyphFace, List<Point3>>(
                                    originalCorners.Select(p => new System.Drawing.PointF(p.X, p.Y)).ToList(),
                                    face,
                                    depthReader != null ?
                                    originalCorners.Select(p => p.X + p.Y * videoReader.frameWidth >= 0 && p.X + p.Y * videoReader.frameWidth < videoReader.frameWidth * videoReader.frameHeight ?
                                                                   new Point3(csps[p.X + p.Y * videoReader.frameWidth].X,
                                                                   csps[p.X + p.Y * videoReader.frameWidth].Y,
                                                                   csps[p.X + p.Y * videoReader.frameWidth].Z) : new Point3()).ToList() :
                                                                   new List<Point3>()
                                    );

                                break;
                            }
                        }
                    }
                }

                foreach (IDisposable o in new IDisposable[] { image, m, grayImage, edges, grayUI, transformed, transformedOtsu })
                {
                    if (o != null)
                    {
                        o.Dispose();
                    }
                }

                stopwatch.Stop();
                Console.WriteLine("Transforming and detect glyph = " + stopwatch.ElapsedMilliseconds);
                stopwatch.Restart();

            }

            if (progress != null)
                progress.Report(videoReader.frameCount);

            if (recognizedGlyphs.Keys.Count != 0)
            {
                foreach (int boxPrototypeIndex in recognizedGlyphs.Keys)
                {
                    Console.WriteLine("For boxPrototypeIndex = " + boxPrototypeIndex + " Found glyph box at " + recognizedGlyphs[boxPrototypeIndex].Keys.Count + " frames");
                    GlyphBoxObject oneBox = null;
                    var boxPrototype = boxPrototypes[boxPrototypeIndex];
                    oneBox = new GlyphBoxObject(currentSession, "", Color.Black, 1, videoReader.fileName);
                    oneBox.boxPrototype = boxPrototype;
                    foreach (int frameNo in recognizedGlyphs[boxPrototypeIndex].Keys)
                    {
                        var glyphs = recognizedGlyphs[boxPrototypeIndex][frameNo];

                        var glyphBounds = new List<List<System.Drawing.PointF>>();
                        var glyph3DBounds = new List<List<Point3>>();
                        var faces = new List<GlyphFace>();

                        foreach (var glyph in glyphs)
                        {
                            glyphBounds.Add(glyph.Value.Item1);
                            faces.Add(glyph.Value.Item2);
                            glyph3DBounds.Add(glyph.Value.Item3);
                        }

                        oneBox.setBounding(frameNo, glyphSize, glyphBounds, faces);
                        oneBox.set3DBounding(frameNo, glyphSize, glyph3DBounds, faces);

                        //Point3 center = new Point3();
                        //Quaternions quaternions = new Quaternions();

                        //oneBox.set3DBounding(frameNo, new CubeLocationMark(frameNo, center, quaternions));
                    }

                    objects.Add(oneBox);
                }
            }

            return objects;
        }

        private static void getImageForProcessing(Dictionary<int, Dictionary<int, Dictionary<int, Tuple<List<System.Drawing.PointF>, GlyphFace, List<Point3>>>>> recognizedGlyphs,
            Mat m, bool previousFrameDetection, int frameNo, ref Bitmap bitmap, ref System.Drawing.Point startPos)
        {
            if (frameNo % anchorNumber == 0)
            {
                bitmap = m.Bitmap;
                startPos.X = startPos.Y = 0;
                return;
            }
            else
            {

                Console.WriteLine("previousFrameDetection " + previousFrameDetection);
                if (previousFrameDetection)
                {
                    float minX = float.MaxValue, minY = float.MaxValue, maxX = -1, maxY = -1;

                    foreach (var boxPrototypeIndex in recognizedGlyphs.Keys)
                    {
                        if (recognizedGlyphs[boxPrototypeIndex].ContainsKey(frameNo - 1))
                        {
                            foreach (var faceIndex in recognizedGlyphs[boxPrototypeIndex][frameNo - 1].Keys)
                            {
                                foreach (var p in recognizedGlyphs[boxPrototypeIndex][frameNo - 1][faceIndex].Item1)
                                {
                                    if (p.X < minX) minX = p.X;
                                    if (p.X > maxX) maxX = p.X;
                                    if (p.Y < minY) minY = p.Y;
                                    if (p.Y > maxY) maxY = p.Y;
                                }
                            }
                        }
                    }

                    Console.WriteLine("Crop rect " + minX + " , " + minY + " , " + maxX + " , " + maxY);

                    if (minX == float.MaxValue || minY == float.MaxValue || maxX == -1 || maxY == -1)
                    {
                        bitmap = m.Bitmap;
                        startPos.X = startPos.Y = 0;
                        return;
                    }


                    minX -= extensionFrame;
                    minY -= extensionFrame;
                    maxX += extensionFrame;
                    maxY += extensionFrame;

                    int iminX = minX < 0 ? 0 : (int)minX;
                    int iminY = minY < 0 ? 0 : (int)minY;
                    int imaxX = maxX > m.Bitmap.Width ? m.Bitmap.Width : (int)maxX;
                    int imaxY = maxY > m.Bitmap.Height ? m.Bitmap.Height : (int)maxY;

                    Console.WriteLine("Crop rect " + iminX + " , " + iminY + " , " + imaxX + " , " + imaxY);
                    Rectangle cropRect = new Rectangle(iminX, iminY, imaxX - iminX, imaxY - iminY);
                    bitmap = new Bitmap(cropRect.Width, cropRect.Height);

                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(m.Bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }

                    startPos.X = iminX;
                    startPos.Y = iminY;

                    return;
                }
                else
                {
                    bitmap = m.Bitmap;
                    startPos.X = startPos.Y = 0;
                    return;
                }
            }
        }

        private const int stepSize = 3;

        // Calculate average brightness difference between pixels outside and inside of the object
        // bounded by specified left and right edge
        private float CalculateAverageEdgesBrightnessDifference(List<IntPoint> leftEdgePoints,
            List<IntPoint> rightEdgePoints, UnmanagedImage image)
        {
            // create list of points, which are a bit on the left/right from edges
            List<IntPoint> leftEdgePoints1 = new List<IntPoint>();
            List<IntPoint> leftEdgePoints2 = new List<IntPoint>();
            List<IntPoint> rightEdgePoints1 = new List<IntPoint>();
            List<IntPoint> rightEdgePoints2 = new List<IntPoint>();

            int tx1, tx2, ty;
            int widthM1 = image.Width - 1;

            for (int k = 0; k < leftEdgePoints.Count; k++)
            {
                tx1 = leftEdgePoints[k].X - stepSize;
                tx2 = leftEdgePoints[k].X + stepSize;
                ty = leftEdgePoints[k].Y;

                leftEdgePoints1.Add(new IntPoint((tx1 < 0) ? 0 : tx1, ty));
                leftEdgePoints2.Add(new IntPoint((tx2 > widthM1) ? widthM1 : tx2, ty));

                tx1 = rightEdgePoints[k].X - stepSize;
                tx2 = rightEdgePoints[k].X + stepSize;
                ty = rightEdgePoints[k].Y;

                rightEdgePoints1.Add(new IntPoint((tx1 < 0) ? 0 : tx1, ty));
                rightEdgePoints2.Add(new IntPoint((tx2 > widthM1) ? widthM1 : tx2, ty));
            }

            // collect pixel values from specified points
            byte[] leftValues1 = image.Collect8bppPixelValues(leftEdgePoints1);
            byte[] leftValues2 = image.Collect8bppPixelValues(leftEdgePoints2);
            byte[] rightValues1 = image.Collect8bppPixelValues(rightEdgePoints1);
            byte[] rightValues2 = image.Collect8bppPixelValues(rightEdgePoints2);

            // calculate average difference between pixel values from outside of the shape and from inside
            float diff = 0;
            int pixelCount = 0;

            for (int k = 0; k < leftEdgePoints.Count; k++)
            {
                if (rightEdgePoints[k].X - leftEdgePoints[k].X > stepSize * 2)
                {
                    diff += (leftValues1[k] - leftValues2[k]);
                    diff += (rightValues2[k] - rightValues1[k]);
                    pixelCount += 2;
                }
            }

            return diff / pixelCount;
        }

        public string getName()
        {
            return "Detect glyph block";
        }
    }
}
