using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public static class Extensions
    {
        public static Point getCenter(this Rectangle r)
        {
            return new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
        }


        public static Rectangle[] getCornerSelectBoxes(this Rectangle boundingBox, int boxSize)
        {
            int lowerX = boundingBox.X;
            int lowerY = boundingBox.Y;
            int higherX = lowerX + boundingBox.Width;
            int higherY = lowerY + boundingBox.Height;

            Rectangle[] selectBoxes = new Rectangle[] { new Rectangle(lowerX - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(lowerX - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle((lowerX + higherX)/2 - (boxSize - 1)/2, lowerY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle((lowerX + higherX)/2 - (boxSize - 1)/2, higherY - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(lowerX - (boxSize - 1)/2, (lowerY + higherY)/2 - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(higherX - (boxSize - 1)/2, (lowerY + higherY)/2 - (boxSize - 1)/2,boxSize,boxSize),
                                                        new Rectangle(boundingBox.getCenter().X, boundingBox.getCenter().Y,boxSize,boxSize)
            };
            return selectBoxes;
        }

        public static void DrawRig(this Graphics graphics, Pen p, RigFigure<Point> rigFigure)
        {
            // Draw joints
            foreach ( var joint in rigFigure.rigJoints.Values )
            {
                graphics.DrawEllipse(p, joint.X - 2 * p.Width, joint.Y - 2 * p.Width, p.Width * 4, p.Width * 4);
            }

            // Draw bones
            foreach ( var bone in rigFigure.rigBones )
            {
                graphics.DrawLine(p, bone.Item1, bone.Item2);
            }
        }

        public static string ToSString(this ColorSpacePoint scp)
        {
            return "( " + scp.X + ", " + scp.Y + " )"; 
        }

        public static string ToSString(this DepthSpacePoint dsp)
        {
            return "( " + dsp.X + ", " + dsp.Y + " )";
        }

        public static string ToSString(this CameraSpacePoint csp)
        {
            return "( " + csp.X + ", " + csp.Y + ", " + csp.Z + " )";
        }
    }
}
