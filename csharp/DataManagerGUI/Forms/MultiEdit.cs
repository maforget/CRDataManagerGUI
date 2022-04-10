using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataManagerGUI
{
    public partial class MultiEdit : Form
    {
        public string Results
        {
            get
            {
                string[] tmp = new string[bindingSource1.Count];
                for (int i = 0; i < bindingSource1.Count; i++)
                {
                    tmp[i] = bindingSource1[i].ToString();
                }
                return tmp.Join(Global.DELIMITER);
            }
        }
        public MultiEdit()
        {
            InitializeComponent();
        }

        public MultiEdit(string[] strStrings)
            : this()
        {
            for (int i = 0; i < strStrings.Length; i++)
                bindingSource1.Add(strStrings[i]);

            for (int i = 0; i < Global.AllowedKeys.Length; i++)
            {
                if (!Global.AllowedKeys[i].Equals("Custom"))
                    cmbBox.Items.Add("{" + Global.AllowedKeys[i] + "}");
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string[] tmpStr = cmbBox.Text.Split(new string[] { Global.DELIMITER }, StringSplitOptions.None);
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    for (int i = 0; i < tmpStr.Length; i++)
                    {
                        Add(tmpStr[i]);
                    }
                    break;

                case Keys.F5:
                    for (int i = 0; i < tmpStr.Length; i++)
                    {
                        if (i == 0)
                            bindingSource1.List[listBox1.SelectedIndex] = tmpStr[i];
                        else
                            Add(tmpStr[i]);
                        cmbBox.Focus();
                    }
                    break;

                default:
                    break;
            }

            void Add(string str)
            {
                if (!bindingSource1.Contains(str))
                {
                    bindingSource1.Add(str);
                    cmbBox.Text = "";
                    cmbBox.Focus();
                }
                else AlreadyExists(str);
            }
        }

        private void AlreadyExists(string p)
        {
            MessageBox.Show("The value'" + p + " already exists in the list", "Value Exists");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            KeyEventArgs KeyPress = new KeyEventArgs(Keys.Enter);
            textBox1_KeyUp(cmbBox, KeyPress);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            KeyEventArgs KeyPress = new KeyEventArgs(Keys.F5);
            textBox1_KeyUp(cmbBox, KeyPress);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            bindingSource1.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            bindingSource1.RemoveCurrent();
        }

        private void Value_Changed(object sender, EventArgs e)
        {
            btnAdd.Enabled = btnUpdate.Enabled = !bindingSource1.Contains(cmbBox.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            int index = bindingSource1.Position;
            if (index < 1) return;

            string tmp = (string)bindingSource1.Current;
            bindingSource1.RemoveCurrent();
            bindingSource1.Insert(index - 1, tmp);
            bindingSource1.Position = index - 1;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            int index = bindingSource1.Position;
            if (index == bindingSource1.Count - 1) return;

            string tmp = (string)bindingSource1.Current;
            bindingSource1.RemoveCurrent();
            bindingSource1.Insert(index + 1, tmp);
            bindingSource1.Position = index + 1;
        }

        private void bindingSource1_CurrentItemChanged(object sender, EventArgs e)
        {

        }

        private void bindingSource1_PositionChanged(object sender, EventArgs e)
        {
            btnUpdate.Enabled = btnRemove.Enabled = bindingSource1.Position > -1;
            btnClear.Enabled = bindingSource1.Count > 0;
            btnMoveDown.Enabled = bindingSource1.Position < bindingSource1.Count - 1 && bindingSource1.Position > -1;
            btnMoveUp.Enabled = bindingSource1.Position > 0;
            if (bindingSource1.Position < 0) return;

            cmbBox.Text = (string)bindingSource1.Current;
        }
    }
}
