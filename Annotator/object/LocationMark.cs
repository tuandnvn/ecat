using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class LocationMark : ObjectMark
    {
        public enum LocationMarkType
        {
            Location, // Set the location of the object manually
                      // For automatic detected object, there would be location for the first time the object is detected

            Delete    // The position where the object disappears out of the view
        };

        public LocationMarkType markType { get; }

        public LocationMark(int frameNo, LocationMarkType markType) : base(frameNo)
        {
            this.markType = markType;
        }

        // Delete object mark
        public LocationMark(int frameNo) : base(frameNo)
        {
            this.markType = LocationMarkType.Delete;
        }

        public virtual float Score(Point testPoint)
        {
            return 0;
        }

        public virtual void drawOnGraphics(Graphics g, Pen p) { }

        public virtual Rectangle[] getCornerSelectBoxes(int boxSize) { return new Rectangle[0];  }
    }
}
