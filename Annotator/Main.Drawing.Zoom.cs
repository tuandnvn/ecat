using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class Main
    {
        bool inZoomIn = true;
        IntPtr zoomInPtr;
        IntPtr zoomOutPtr;
        IntPtr rectanglePtr;
        IntPtr polygonPtr;

        private void InitializeZooming()
        {
            Bitmap b = new Bitmap(Properties.Resources.zoom_in, new Size(32, 32));
            b.MakeTransparent(b.GetPixel(0, 0));
            zoomInPtr = b.GetHicon();

            b = new Bitmap(Properties.Resources.zoom_out, new Size(32, 32));
            b.MakeTransparent(b.GetPixel(0, 0));
            zoomOutPtr = b.GetHicon();

            b = new Bitmap(Properties.Resources.rectangle, new Size(32, 32));
            b.MakeTransparent(b.GetPixel(0, 0));
            rectanglePtr = b.GetHicon();

            b = new Bitmap(Properties.Resources.polygon, new Size(32, 32));
            b.MakeTransparent(b.GetPixel(0, 0));
            polygonPtr = b.GetHicon();
        }

        private void whenZoomButtonAndMouseDown(MouseEventArgs e)
        {
            // Zoom in 
            if (inZoomIn)
            {
                var x = e.Location.X;
                var y = e.Location.Y;

                this.pictureBoard.Dock = DockStyle.None;
                this.pictureBoard.Size = new System.Drawing.Size(2 * this.videoPanel.Width, 2 * this.videoPanel.Height);
                this.videoPanel.AutoScrollPosition = new Point( x, y);
            }
            // Zoom out
            else
            {
                this.pictureBoard.Dock = DockStyle.Fill;
            }
           
            inZoomIn = !inZoomIn;
        }

        private void whenZoomButtonAndMouseMove(MouseEventArgs e)
        {
            if (inZoomIn)
            {
                Cursor.Current = new System.Windows.Forms.Cursor(zoomInPtr);
            }
            else
            {
                Cursor.Current = new System.Windows.Forms.Cursor(zoomOutPtr);
            }
        }

        private void zoomDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(zoomDrawing, drawingButtonGroup, !drawingButtonSelected[zoomDrawing]);
            if (inZoomIn)
            {
                Cursor.Current = new System.Windows.Forms.Cursor(zoomInPtr);
            }
            else
            {
                Cursor.Current = new System.Windows.Forms.Cursor(zoomOutPtr);
            }
            cancelDrawing();
        }

        private void rectangleDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(rectangleDrawing, drawingButtonGroup, !drawingButtonSelected[rectangleDrawing]);
            Cursor.Current = new System.Windows.Forms.Cursor(rectanglePtr);
            cancelSelectObject();
            cancelDrawing();
        }

        private void polygonDrawing_MouseDown(object sender, MouseEventArgs e)
        {
            selectButtonDrawing(polygonDrawing, drawingButtonGroup, !drawingButtonSelected[polygonDrawing]);
            Cursor.Current = new System.Windows.Forms.Cursor(polygonPtr);
            cancelSelectObject();
            cancelDrawing();
        }
    }
}
