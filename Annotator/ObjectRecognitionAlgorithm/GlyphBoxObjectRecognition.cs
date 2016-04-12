using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Annotator.depth;
using Microsoft.Kinect;
using Emgu.CV;
using System.Drawing;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace Annotator.ObjectRecognitionAlgorithm
{
    public class GlyphBoxObjectRecognition : IObjectRecogAlgo
    {
        public GlyphBoxObjectRecognition()
        {

        }

        public List<Object> findObjects(VideoReader videoReader, IDepthReader depthReader, Action<ushort[], CameraSpacePoint[]> mappingFunction)
        {
            List<Object> objects = new List<Object>();
            for (int frameNo = 0; frameNo < videoReader.frameCount; frameNo++)
            {
                Bitmap image = videoReader.getFrame(frameNo).Bitmap;

                /// Adapt from Glyph Recognition Prototyping
                /// Copyright © Andrew Kirillov, 2009-2010
                /// 
                // 1 - Grayscale
                Bitmap grayImage = Grayscale.CommonAlgorithms.BT709.Apply(image);

                // 2 - Edge detection
                DifferenceEdgeDetector edgeDetector = new DifferenceEdgeDetector();
                Bitmap edges = edgeDetector.Apply(grayImage);

                // 3 - Threshold edges
                Threshold thresholdFilter = new Threshold(20);
                thresholdFilter.ApplyInPlace(edges);

                // 4 - Blob Counter
                BlobCounter blobCounter = new BlobCounter();
                blobCounter.MinHeight = 32;
                blobCounter.MinWidth = 32;
                blobCounter.FilterBlobs = true;
                blobCounter.ObjectsOrder = ObjectsOrder.Size;

                blobCounter.ProcessImage(edges);
                Blob[] blobs = blobCounter.GetObjectsInformation();

                // create unmanaged copy of source image, so we could draw on it
                UnmanagedImage imageData = UnmanagedImage.FromManagedImage(image);

                // Get unmanaged copy of grayscale image, so we could access it's pixel values
                UnmanagedImage grayUI = UnmanagedImage.FromManagedImage(grayImage);

                // list of found dark/black quadrilaterals surrounded by white area
                List<List<IntPoint>> foundObjects = new List<List<IntPoint>>();
                // shape checker for checking quadrilaterals
                SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

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
                                Drawing.Polygon(imageData, corners, Color.FromArgb(255, 255, 0, 0));
                                // add the object to the list of interesting objects for further processing
                                foundObjects.Add(corners);
                            }
                        }
                    }
                }

                int recordedTimeForRgbFrame = (int)(videoReader.totalMiliTime * frameNo / (videoReader.frameCount - 1));
                ushort[] depthValues = depthReader.readFrameAtTime(recordedTimeForRgbFrame);

                CameraSpacePoint[] csps = new CameraSpacePoint[videoReader.frameWidth * videoReader.frameHeight];

                mappingFunction(depthValues, csps);

                int counter = 1;

                // further processing of each potential glyph
                foreach (List<IntPoint> corners in foundObjects)
                {
                    // 6 - do quadrilateral transformation
                    QuadrilateralTransformation quadrilateralTransformation =
                        new QuadrilateralTransformation(corners, 250, 250);

                    Bitmap transformed = quadrilateralTransformation.Apply(grayImage);

                    // 7 - otsu thresholding
                    OtsuThreshold otsuThresholdFilter = new OtsuThreshold();
                    Bitmap transformedOtsu = otsuThresholdFilter.Apply(transformed);

                    int glyphSize = 5;
                    SquareBinaryGlyphRecognizer gr = new SquareBinaryGlyphRecognizer(glyphSize);

                    bool[,] glyphValues = gr.Recognize(ref transformedOtsu,
                        new Rectangle(0, 0, 250, 250));

                    for (int i = 0; i < glyphSize; i++)
                    {
                        StringBuilder sb = new StringBuilder("   ");

                        for (int j = 0; j < glyphSize; j++)
                        {
                            sb.Append((glyphValues[i, j]) ? "1 " : "0 ");
                        }
                    }

                    counter++;
                }

                
            }

            return objects;
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
    }
}
