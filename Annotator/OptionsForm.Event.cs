using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Annotator
{
    partial class OptionsForm
    {
        private void addEventPredTypeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var predicateForm = eventPredTypeTxtBox.Text;

                var newPredicate = Predicate.Parse(predicateForm);

                if (newPredicate == null)
                {
                    throw new ArgumentException("New predicate = null. The predicate form has problem!");
                }
                if (this.options.eventPredicates.Contains(newPredicate))
                {
                    throw new ArgumentException("Type exists!");
                }

                this.options.eventPredicates.Add(newPredicate);

                showEventPredTypeList();
                this.eventPredTypeListBox.SelectedIndex = this.eventPredTypeListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeEventPredTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = eventPredTypeListBox.SelectedIndex;
            try
            {
                this.options.eventPredicates.RemoveAt(selectIndex);
                showEventPredTypeList();

                upEventPredTypeBtn.Enabled = false;
                downEventPredTypeBtn.Enabled = false;
                removeEventPredTypeBtn.Enabled = false;
            }
            catch
            {
            }
        }

        private void upEventPredTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = eventPredTypeListBox.SelectedIndex;
            try
            {
                if (selectIndex > 0)
                {
                    var swapItem = this.options.eventPredicates[selectIndex];
                    this.options.eventPredicates.RemoveAt(selectIndex);
                    this.options.eventPredicates.Insert(selectIndex - 1, swapItem);
                    showEventPredTypeList();
                    this.eventPredTypeListBox.SelectedIndex = selectIndex - 1;
                }
            }
            catch
            {
            }
        }

        private void downEventPredTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = eventPredTypeListBox.SelectedIndex;
            try
            {
                if (selectIndex < this.options.eventPredicates.Count - 1)
                {
                    var swapItem = this.options.eventPredicates[selectIndex];
                    this.options.eventPredicates.RemoveAt(selectIndex);
                    this.options.eventPredicates.Insert(selectIndex + 1, swapItem);
                    showEventPredTypeList();
                    this.eventPredTypeListBox.SelectedIndex = selectIndex + 1;
                }
            }
            catch
            {
            }
        }

        private void eventPredTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = eventPredTypeListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                upEventPredTypeBtn.Enabled = true;
                downEventPredTypeBtn.Enabled = true;
                removeEventPredTypeBtn.Enabled = true;
            }
            else
            {
                upEventPredTypeBtn.Enabled = false;
                downEventPredTypeBtn.Enabled = false;
                removeEventPredTypeBtn.Enabled = false;
            }
        }

        private void showEventPredTypeList()
        {
            this.eventPredTypeListBox.Items.Clear();
            this.eventPredTypeListBox.Items.AddRange(this.options.eventPredicates.ToArray());
        }
    }
}
