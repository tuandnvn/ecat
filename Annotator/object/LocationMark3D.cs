using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class LocationMark3D : LocationMark
    {
        public LocationMark3D(int frameNo) : base(frameNo)
        {
        }

        public virtual LocationMark2D getDepthViewLocationMark(float scale, PointF translation)
        {
            return null;
        }

        public virtual LocationMark3D getScaledLocationMark(float scale, Point3 translation)
        {
            return null;
        }

        public virtual LocationMark3D addLocationMark(int resultFrameNo, LocationMark3D added)
        {
            return null;
        }
    }
}
