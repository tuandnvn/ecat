using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public class ObjectMark
    {
        public int frameNo { get; set; }
        public ObjectMark(int frameNo)
        {
            this.frameNo = frameNo;
        }
    }
}
