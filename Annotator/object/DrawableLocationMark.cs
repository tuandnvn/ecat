using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public abstract class DrawableLocationMark : LocationMark
    {
        public DrawableLocationMark(int frameNo) : base(frameNo)
        {
        }

        public virtual float Score(Point testPoint)
        {
            return 0;
        }

        public virtual void drawOnGraphics(Graphics g, Pen p) { }

        public virtual List<Rectangle> getCornerSelectBoxes(int boxSize) { return new List<Rectangle>(); }

        public virtual DrawableLocationMark getScaledLocationMark(float scale, Point translation)
        {
            return null;
        }
    }

    public class DeleteLocationMark : DrawableLocationMark
    {
        public DeleteLocationMark(int frameNo) : base(frameNo)
        {
        }
    }
}
