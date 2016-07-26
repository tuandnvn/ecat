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
    public partial class ObjectLinkForm : Form
    {
        Session session;
        Object obj;
        int frameNo;
        Main main;

        public ObjectLinkForm(Main main, Session session, Object obj, int frameNo)
        {
            InitializeComponent();

            this.main = main;
            this.obj = obj;
            this.frameNo = frameNo;
            this.session = session;

            this.objectSelectComboBox.Items.AddRange(session.getObjects().Select( o => o.id ).ToArray() );
            this.qualifiedSelectComboBox.Items.AddRange(new object[] { true, false });
            this.linkComboBox.Items.AddRange(Options.getOption().objectLinkTypes.ToArray());
        }

        private void addLinkBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var oid = (string)this.objectSelectComboBox.SelectedItem;
                var qualified = (bool)this.qualifiedSelectComboBox.SelectedItem;
                var link = (string)this.linkComboBox.SelectedItem;

                obj.setLink(frameNo, oid, qualified, link);

                this.main.redrawObjectMarks();
            } catch (Exception exc)
            {
                MessageBox.Show(exc.ToString(), "Problem when adding link");
            }
            
            this.Hide();
        }
    }
}
