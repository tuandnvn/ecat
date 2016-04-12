using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    public partial class SpatialLinkForm : Form
    {
        Session session;
        Object obj;
        int frameNo;
        Main mainFrame;

        public SpatialLinkForm(Main mainFrame, Session session, Object obj, int frameNo)
        {
            InitializeComponent();

            this.mainFrame = mainFrame;
            this.obj = obj;
            this.frameNo = frameNo;
            this.session = session;

            this.objectSelectComboBox.Items.AddRange(session.getObjects().Select( o => o.id ).ToArray() );
            this.qualifiedSelectComboBox.Items.AddRange(new object[] { true, false });
            this.spatialLinkComboBox.Items.AddRange(Enum.GetValues(typeof(SpatialLinkMark.SpatialLinkType)).Cast<object>().ToArray());
        }

        private void addLinkBtn_Click(object sender, EventArgs e)
        {
            var oid = (string) this.objectSelectComboBox.SelectedItem;
            var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
            var spatialLink = (SpatialLinkMark.SpatialLinkType)this.spatialLinkComboBox.SelectedItem;

            obj.setSpatialLink(frameNo, oid, qualified, spatialLink);

            this.mainFrame.redrawObjectMarks();
            this.Hide();
        }
    }
}
