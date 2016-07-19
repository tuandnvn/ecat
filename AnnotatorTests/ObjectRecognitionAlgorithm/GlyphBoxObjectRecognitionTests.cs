using Microsoft.VisualStudio.TestTools.UnitTesting;
using Annotator.ObjectRecognitionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator.ObjectRecognitionAlgorithm.Tests
{
    [TestClass()]
    public class GlyphBoxObjectRecognitionTests
    {
        [TestMethod()]
        public void GlyphBoxObjectRecognitionTest()
        {
            //VideoReader vr = new VideoReader("rgb_4_20_2016_11_38_15PM.avi", 4290);
            VideoReader vr = new VideoReader("rgb_6_23_2016_5_09_26PM.avi", 4278);
            var recognizer = new GlyphBoxObjectRecognition(null, new Options().prototypeList, 5);

            recognizer.findObjects(vr, null, null, null);
        }
    }
}