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
            detectedObjects = new List<Object>();
            

            objectRecognizers = new List<IObjectRecogAlgo>();
            objectRecognizers.Add(new GlyphBoxObjectRecognition(null, GlyphBoxPrototype.prototype1));
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
