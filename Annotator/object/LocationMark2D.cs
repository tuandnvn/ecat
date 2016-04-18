﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public abstract class LocationMark2D : LocationMark
    {
        public LocationMark2D(int frameNo) : base(frameNo)
        {
        }

        public virtual float Score(Point testPoint)
        {
            return 0;
        }

        public virtual void drawOnGraphics(Graphics g, Pen p) { }

        public virtual Rectangle[] getCornerSelectBoxes(int boxSize) { return new Rectangle[0]; }

        public virtual LocationMark2D getScaledLocationMark(double scale, Point translation)
        {
            return null;
        }
    }

    public class DeleteLocationMark : LocationMark2D
    {
        public DeleteLocationMark(int frameNo) : base(frameNo)
        {
        }
    }
}