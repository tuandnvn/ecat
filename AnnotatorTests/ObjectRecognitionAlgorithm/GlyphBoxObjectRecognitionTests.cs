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
            VideoReader vr = new VideoReader("rgb_4_20_2016_11_38_15PM.avi", 4290);

            var recognizer = new GlyphBoxObjectRecognition(null, new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3, GlyphBoxPrototype.prototype4 }, 5);

            recognizer.findObjects(vr, null, null, null);
        }
    }
}