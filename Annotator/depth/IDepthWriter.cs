using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.depth
{
    interface IDepthWriter : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milisecondFromStart"></param>
        /// <param name="depthValues"></param>
        void writeFrame(int milisecondFromStart, ushort[] depthValues);
    }
}
