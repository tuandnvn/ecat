using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class Main
    {
        public Object selectedObject { set; get; }
        private const string ID = "ID";
        private const string NAME = "Name";
        private const string OBJECT_TYPE = "Object type";
        private const string SEMANTIC_TYPE = "Semantic type";
        private const string GEN_TYPE = "Generate type";
        private const string COLOR = "Color";
        private const string BORDER_SIZE = "Border size";
        private string[] defaultRows = { ID, NAME, GEN_TYPE, OBJECT_TYPE, SEMANTIC_TYPE, COLOR, BORDER_SIZE };

        internal void clearInformation()
        {
            lock (this)
            {
                objectProperties.Rows.Clear();
            }
        }

        internal void showInformation(Object o)
        {
            objectProperties.Rows.Clear();
            objectProperties.Rows.Add(ID, o.id);
            objectProperties.Rows.Add(NAME, o.name);
            objectProperties.Rows.Add(GEN_TYPE, o.genType);
            objectProperties.Rows.Add(OBJECT_TYPE, "");
            objectProperties.Rows.Add(SEMANTIC_TYPE, o.semanticType);
            objectProperties.Rows.Add(COLOR, "");
            objectProperties.Rows.Add(BORDER_SIZE, "" + o.borderSize);

            // No change to id 
            objectProperties.Rows[Array.FindIndex(defaultRows, t => t == ID)].ReadOnly = true;

            // No change to gen type
            objectProperties.Rows[Array.FindIndex(defaultRows, t => t == GEN_TYPE)].Cells[objectProperties.Columns[1].Name].ReadOnly = true;

            // No change to object type
            objectProperties.Rows[Array.FindIndex(defaultRows, t => t == OBJECT_TYPE)].Cells[objectProperties.Columns[1].Name].ReadOnly = true;

            // Change cell for object type to ComboBox
            //DataGridViewComboBoxCell c1 = new DataGridViewComboBoxCell();
            //c1.DataSource = Enum.GetValues(typeof(Object.ObjectType));
            //c1.ValueType = typeof(Object.ObjectType);
            //c1.Value = o.objectType;
            //objectProperties.Rows[Array.FindIndex(defaultRows, t => t == OBJECT_TYPE)].Cells[objectProperties.Columns[1].Name] = c1;
            objectProperties.Rows[Array.FindIndex(defaultRows, t => t == OBJECT_TYPE)].Cells[objectProperties.Columns[1].Name].Value = o.objectType.ToString().Substring(1);

            // Change cell for color to button
            DataGridViewButtonCell c2 = new DataGridViewButtonCell();
            c2.FlatStyle = FlatStyle.Popup;
            c2.Style.BackColor = o.color;
            objectProperties.Rows[Array.FindIndex(defaultRows, t => t == COLOR)].Cells[objectProperties.Columns[1].Name] = c2;

            for (int i = 0; i < defaultRows.Count(); i++)
            {
                objectProperties.Rows[i].Cells[objectProperties.Columns[0].Name].ReadOnly = true;
            }

            foreach (var entry in o.otherProperties)
            {
                objectProperties.Rows.Add(entry.Key, entry.Value);
            }
        }

        private void objectProperties_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            // Handle color click
            if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell &&
                e.RowIndex == Array.FindIndex(defaultRows, t => t == COLOR))
            {
                // Show the color dialog.
                DialogResult result = colorDialog1.ShowDialog();
                // See if user pressed ok.
                if (result == DialogResult.OK)
                {
                    // Set form background to the selected color.
                    selectedObject.color = colorDialog1.Color;
                    senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = selectedObject.color;
                    invalidatePictureBoard();
                }
            }
        }

        private void objectProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if (rowIndex >= defaultRows.Count())
            {
                if (selectedObject != null)
                {
                    selectedObject.addProperty((string)objectProperties.Rows[rowIndex].Cells[0].Value,
                           (string)objectProperties.Rows[rowIndex].Cells[1].Value);
                    return;
                }
            }

            if (rowIndex == Array.FindIndex(defaultRows, t => t == NAME))
            {
                selectedObject.name = (string)objectProperties.Rows[rowIndex].Cells[1].Value;
                objectToObjectTracks[selectedObject].updateInfo();
            }

            if (rowIndex == Array.FindIndex(defaultRows, t => t == SEMANTIC_TYPE))
            {
                selectedObject.semanticType = (string)objectProperties.Rows[rowIndex].Cells[1].Value;
            }

            //if (rowIndex == Array.FindIndex(defaultRows, t => t == OBJECT_TYPE))
            //{
            //    //var objectType = (Object.ObjectType)Enum.Parse(typeof(Object.ObjectType), "" + objectProperties.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            //    //Console.WriteLine(objectType);
            //    selectedObject.objectType = (Object.ObjectType)objectProperties.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //}

            if (rowIndex == Array.FindIndex(defaultRows, t => t == BORDER_SIZE))
            {
                try
                {
                    int borderSize = int.Parse((string)objectProperties.Rows[rowIndex].Cells[1].Value);
                    if (borderSize < 1 || borderSize > 10)
                        throw new ArgumentOutOfRangeException("Border size need to be an integer between 1 and 10");
                    selectedObject.borderSize = borderSize;
                    invalidatePictureBoard();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("Border size need to be an integer between 1 and 10");
                    objectProperties.Rows[rowIndex].Cells[1].Value = "" + selectedObject.borderSize;
                }
            }
        }
    }
}
