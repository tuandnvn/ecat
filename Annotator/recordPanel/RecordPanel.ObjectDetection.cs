using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class RecordPanel
    {
        List<Object> detectedObjects;
        List<IObjectRecogAlgo> objectRecognizers;
        /// <summary>
        /// After some default rows,
        /// this is the first row index to set whether to include object recoginizer
        /// </summary>
        int objectRecognizerStartRow = 10;

        /// <summary>
        /// Each value corresponds to whether to include the object recognizer or not
        /// </summary>
        Dictionary<IObjectRecogAlgo, bool> objectRecognizerIncluded;

        public void InitializeObjectRecognizers()
        {
            objectRecognizers = new List<IObjectRecogAlgo>();
            objectRecognizerIncluded = new Dictionary<IObjectRecogAlgo, bool>();
            objectRecognizers.Add(new GlyphBoxObjectRecognition(null, options.prototypeList, 5));
        }

        private void changeObjectRecognizerIncluded(DataGridViewCellEventArgs e)
        {
            var objectRecognizer = objectRecognizers[e.RowIndex - objectRecognizerStartRow];
            bool recognizerIncluded = (bool)optionsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            objectRecognizerIncluded[objectRecognizer] = recognizerIncluded;
        }

        public void InitializeOptionsTableDetection()
        {
            foreach (var objectRecognizer in objectRecognizers)
            {
                objectRecognizerIncluded[objectRecognizer] = true;
                int rowIndex = optionsTable.Rows.Add(objectRecognizer.getName(), "True");
                changeRowToTrueFall(optionsTable, rowIndex, 1);
            }
        }

    }
}
