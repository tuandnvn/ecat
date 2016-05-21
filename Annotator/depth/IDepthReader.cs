using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
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
        /// Turn the depth frame into a bitmap to show on the screen
        /// 
        /// </summary>
        /// <param name="milisecondFromStart"> A time from start of recording session</param>
        /// <param name="depthBitmap"> Output bitmap </param>
        /// <param name="depthValuesToByte"> buffer of size depthFrameDescription.Width * depthFrameDescription.Height * 4, allow reusing of buffer </param>
        /// <param name="scale"> scale from depth range to [0-255], e.g. 8000 / 256 </param>
        void readFrameAtTimeToBitmap(int milisecondFromStart, Bitmap depthBitmap, byte[] depthValuesToByte, float scale);

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
