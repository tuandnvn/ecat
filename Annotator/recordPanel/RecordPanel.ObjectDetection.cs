using Annotator.ObjectRecognitionAlgorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annotator
{
    public partial class RecordPanel
    {
        List<Object> detectedObjects;
        List<IObjectRecogAlgo> objectRecognizers;

        public void InitializeObjectRecognizers()
        {
            objectRecognizers = new List<IObjectRecogAlgo>();
            objectRecognizers.Add(new GlyphBoxObjectRecognition(null, new List<GlyphBoxPrototype> { GlyphBoxPrototype.prototype2, GlyphBoxPrototype.prototype3 }, 5));
        }

        public void InitializeOptionsTableDetection()
        {
            int counter = 10;

            foreach (var objectRecognizer in objectRecognizers)
            {
                optionsTable.Rows.Add(objectRecognizer.getName(), "False");
                changeRowToTrueFall(optionsTable, counter, 1);
                counter++;
            }
        }

        public void DetectObjects()
        {
            detectedObjects = new List<Object>();
            foreach (var objectRecognizer in objectRecognizers)
            {
                if (videoReader != null && depthReader != null)
                {
                    detectedObjects.AddRange(objectRecognizer.findObjects(videoReader, depthReader, this.coordinateMapper.MapColorFrameToCameraSpace));
                }
            }
        }
    }
}
