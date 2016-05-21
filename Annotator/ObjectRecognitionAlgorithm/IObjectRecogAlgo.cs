using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.ObjectRecognitionAlgorithm
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

        List<Object> findObjects( VideoReader videoReader, IDepthReader depthReader, Action<ushort[], CameraSpacePoint[]> mappingFunction);
    }
}
