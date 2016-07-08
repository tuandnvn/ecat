using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    /// <summary>
    /// Object recogniation algorithm is interface for any method
    /// that find an object in the scene, given the RGB, depth stream,
    /// and rig stream. The data loaded is the same as for playback.
    /// 
    /// </summary>
    public interface IObjectRecogAlgo
    {
        string getName();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="videoReader">Provide the video stream.</param>
        /// <param name="depthReader">Provide the depth stream. If set to null then results don't have 3-d data</param>
        /// <param name="mappingFunction">Mapping function between depth field and camera space point. If depthReader is null, it is ignored</param>
        /// <param name="progress">Because finding objects is a long process, there might be some UI need to be updated</param>
        /// <returns></returns>
        List<Object> findObjects( VideoReader videoReader, IDepthReader depthReader, Action<ushort[], CameraSpacePoint[]> mappingFunction, IProgress<int> progress);


    }
}
