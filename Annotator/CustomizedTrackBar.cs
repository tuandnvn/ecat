using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public class CustomizedTrackBar : TrackBar
    {
        /// <summary>
        /// Do not allow the track bar value to be < _minDragVal
        /// Set to Minimum any time Minimum is set
        /// </summary>
        private int _minDragVal;
        public int MinDragVal
        {
            get { return _minDragVal; }
            set { _minDragVal = value;  }
        }

        /// <summary>
        /// Do not allow the track bar value to be > _maxDragVal
        /// Set to Maximum any time Maximum is set
        /// </summary>
        private int _maxDragVal;
        public int MaxDragVal
        {
            get { return _maxDragVal; }
            set { _maxDragVal = value; }
        }

        // Decorator over the original Minimum
        public int Minimum
        {
            get { return (this as TrackBar).Minimum; }
            set {
                (this as TrackBar).Minimum = value;

                MinDragVal = (this as TrackBar).Minimum;

                if (MaxDragVal < (this as TrackBar).Minimum)
                {
                    MaxDragVal = (this as TrackBar).Minimum;
                }
            }
        }

        public int Maximum
        {
            get { return (this as TrackBar).Maximum; }
            set
            {
                (this as TrackBar).Maximum = value;

                if (MinDragVal > (this as TrackBar).Maximum)
                {
                    MinDragVal = (this as TrackBar).Maximum;
                }

                MaxDragVal = (this as TrackBar).Maximum;
            }
        }
    }
}
