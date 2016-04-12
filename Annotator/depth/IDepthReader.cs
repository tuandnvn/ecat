using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.depth
{
    public interface IDepthReader : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="milisecondFromStart"></param>
        /// <returns></returns>
        ushort[] readFrameAtTime(int milisecondFromStart);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        ushort[] readFrame(int frame);

        int getFrameCount();

        int getWidth();

        int getHeight();
    }
}
