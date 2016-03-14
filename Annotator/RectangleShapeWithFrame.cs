using Microsoft.VisualBasic.PowerPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class RectangleShapeWithFrame : RectangleShape
    {
        public int frameNo { get; }
        public RectangleShapeWithFrame(int frameNo) : base()
        {
            this.frameNo = frameNo;
        }
    }
}
