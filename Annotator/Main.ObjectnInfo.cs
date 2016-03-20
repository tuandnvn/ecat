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
        private string[] defaultRows = { ID, NAME, OBJECT_TYPE, SEMANTIC_TYPE };

        internal void clearInformation()
        {
            objectProperties.Rows.Clear();
        }

        internal void showInformation(Object o)
        {
            lock (this)
            {
                objectProperties.Rows.Clear();
                objectProperties.Rows.Add(ID, o.id);
                objectProperties.Rows.Add(NAME, o.name);
                objectProperties.Rows.Add(OBJECT_TYPE, "2D");
                objectProperties.Rows.Add(SEMANTIC_TYPE, o.semanticType);

                // No change to id 
                objectProperties.Rows[0].Cells[objectProperties.Columns[0].Name].ReadOnly = true;
                objectProperties.Rows[0].Cells[objectProperties.Columns[1].Name].ReadOnly = true;

                objectProperties.Rows[1].Cells[objectProperties.Columns[0].Name].ReadOnly = true;

                objectProperties.Rows[2].Cells[objectProperties.Columns[0].Name].ReadOnly = true;
                objectProperties.Rows[2].Cells[objectProperties.Columns[1].Name].ReadOnly = true;

                objectProperties.Rows[3].Cells[objectProperties.Columns[0].Name].ReadOnly = true;

                foreach (var entry in o.otherProperties)
                {
                    objectProperties.Rows.Add(entry.Key, entry.Value);
                }
            }
        }

        private void objectProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;

            if ( rowIndex >= defaultRows.Count() )
            {
                if (selectedObject != null)
                {
                     selectedObject.addProperty((string)objectProperties.Rows[rowIndex].Cells[0].Value, 
                            (string)objectProperties.Rows[rowIndex].Cells[1].Value);
                    return;
                }
            }

            if ( rowIndex == 1 )
            {
                selectedObject.name = (string)objectProperties.Rows[rowIndex].Cells[1].Value;
            }

            if ( rowIndex == 3 )
            {
                selectedObject.semanticType = (string)objectProperties.Rows[rowIndex].Cells[1].Value;
            }
        }

    }
}
