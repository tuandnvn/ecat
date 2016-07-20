using Accord.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    class RigLocationMark3D : LocationMark3D
    {
        public RigFigure<Point3> rigFigure { get; }
        public RigLocationMark3D(int frameNo, RigFigure<Point3> rigFigure) : base(frameNo)
        {
            this.rigFigure = rigFigure;
        }

        public RigLocationMark3D getUpperBody()
        {
            return new RigLocationMark3D(frameNo, Rigs.getUpperBody(rigFigure));
        }

        public override LocationMark2D getDepthViewLocationMark(float scale, PointF translation)
        {
            var mappedJoints = ((Dictionary<string, Point3>)(object)rigFigure.rigJoints).ToDictionary(k => k.Key, k => KinectUtils.projectCameraSpacePointToDepthPixel(k.Value));
            var flattenedMappedJoints = mappedJoints.ToDictionary(k => k.Key, k => new PointF(k.Value.X, k.Value.Y));

            return new RigLocationMark2D(frameNo, new RigFigure<PointF>(flattenedMappedJoints, rigFigure.rigBones)).getScaledLocationMark(scale, translation);
        }

        public override LocationMark3D addLocationMark(int resultFrameNo, LocationMark3D added)
        {
            if (added is RigLocationMark3D)
            {
                var addedRigFigure = (added as RigLocationMark3D).rigFigure;
                return new RigLocationMark3D(resultFrameNo,
                    new RigFigure<Point3>((rigFigure as RigFigure<Point3>).rigJoints.ToDictionary(
                        k => k.Key, k => k.Value.scalePoint(1, addedRigFigure.rigJoints[k.Key]
                 )), rigFigure.rigBones));
            }
            else
            {
                throw new ArgumentException(" Adding location mark needs to be of the same type !");
            }
        }
    }
}
