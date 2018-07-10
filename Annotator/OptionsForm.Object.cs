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

        private void addObjPredType_Click(object sender, EventArgs e)
        {
            try
            {
                var predicateForm = objPredTypeTxtBox.Text;

                var newPredicate = Predicate.Parse(predicateForm);

                if (newPredicate == null)
                {
                    throw new ArgumentException("new predicate = null. The predicate form has problem!");
                }
                if (this.options.objectPredicates.Contains(newPredicate))
                {
                    throw new ArgumentException("Type exists!");
                }

                this.options.objectPredicates.Add(newPredicate);


                showObjPredTypeList();
                this.objPredTypeListBox.SelectedIndex = this.objPredTypeListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeObjPredType_Click(object sender, EventArgs e)
        {
            var selectIndex = objPredTypeListBox.SelectedIndex;
            try
            {
                this.options.objectPredicates.RemoveAt(selectIndex);
                showObjPredTypeList();

                upObjPredTypeBtn.Enabled = false;
                downObjPredTypeBtn.Enabled = false;
                removeObjPredTypeBtn.Enabled = false;
            }
            catch
            {
            }
        }

        private void upObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = objPredTypeListBox.SelectedIndex;
            try
            {
                if (selectIndex > 0)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex - 1, swapItem);
                    showObjPredTypeList();
                    this.objPredTypeListBox.SelectedIndex = selectIndex - 1;
                }
            }
            catch
            {
            }
        }

        private void downObjLinkTypeBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = objPredTypeListBox.SelectedIndex;
            try
            {
                if (selectIndex < this.options.objectPredicates.Count - 1)
                {
                    var swapItem = this.options.objectPredicates[selectIndex];
                    this.options.objectPredicates.RemoveAt(selectIndex);
                    this.options.objectPredicates.Insert(selectIndex + 1, swapItem);
                    showObjPredTypeList();
                    this.objPredTypeListBox.SelectedIndex = selectIndex + 1;
                }
            }
            catch
            {
            }
        }

        private void showObjPredTypeList()
        {
            this.objPredTypeListBox.Items.Clear();
            this.objPredTypeListBox.Items.AddRange(this.options.objectPredicates.ToArray());
        }

        private void objectLinkTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = objPredTypeListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                upObjPredTypeBtn.Enabled = true;
                downObjPredTypeBtn.Enabled = true;
                removeObjPredTypeBtn.Enabled = true;
            }
            else
            {
                upObjPredTypeBtn.Enabled = false;
                downObjPredTypeBtn.Enabled = false;
                removeObjPredTypeBtn.Enabled = false;
            }
        }

        private void addConstraintBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var constraintForm = constraintTxtBox.Text;

                PredicateConstraint newConstraint = null;
                if (constraintForm.StartsWith("UNIQUE="))
                {
                    newConstraint = UniqueConstraint.Parse(constraintForm.Substring("UNIQUE=".Length));

                    if (this.options.predicateConstraints.Contains(newConstraint))
                    {
                        throw new ArgumentException("Type exists!");
                    }
                }

                if (constraintForm.StartsWith("EXCLUSIVE="))
                {
                    newConstraint = ExclusiveConstraint.Parse(constraintForm.Substring("EXCLUSIVE=".Length));

                    if (this.options.predicateConstraints.Contains(newConstraint))
                    {
                        throw new ArgumentException("Type exists!");
                    }
                }

                if (newConstraint == null)
                {
                    throw new ArgumentException("new predicate = null. The predicate form has problem!");
                }

                this.options.predicateConstraints.Add(newConstraint);

                showConstraintList();
                this.constraintListBox.SelectedIndex = this.constraintListBox.Items.Count - 1;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void removeConstraintBtn_Click(object sender, EventArgs e)
        {
            var selectIndex = constraintListBox.SelectedIndex;
            try
            {
                this.options.predicateConstraints.RemoveAt(selectIndex);
                showConstraintList();
                removeConstraintBtn.Enabled = false;
            }
            catch
            {
            }
        }

        private void showConstraintList()
        {
            this.constraintListBox.Items.Clear();
            this.constraintListBox.Items.AddRange(this.options.predicateConstraints.ToArray());
        }

        private void constraintListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectIndex = objPredTypeListBox.SelectedIndex;
            if (selectIndex != -1)
            {
                removeConstraintBtn.Enabled = true;
            }
            else
            {
                removeConstraintBtn.Enabled = false;
            }
        }
    }
}
