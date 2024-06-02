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
    public partial class SettingsConfig : Form
    {
        dmUserInfo userInfo;
        public SettingsConfig()
        {
            InitializeComponent();
        }

        public SettingsConfig(dmUserInfo uiInfo)
            :this()
        {
            userInfo = uiInfo;
            this.checkBox1.Checked = userInfo.ConfirmOverwrite;
            this.checkBox4.Checked = userInfo.ShowStartupDialog;
            this.checkBox2.Checked = userInfo.BreakAfterFirstError;
            this.cbWriteDataManagerProcessed.Checked = userInfo.WriteDataManagerProcessed;
            this.cbCase.Checked = userInfo.CaseSensitive;
            this.cbSaveLocation.Checked = userInfo.SaveLocationInStartupDir;
            this.textBox1.Text = uiInfo.DateTimeFormat;
            radioButton2.Checked = !userInfo.LogBookOnlyWhenValuesChanged;
            radioButton1.Checked = userInfo.LogBookOnlyWhenValuesChanged;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            userInfo.DateTimeFormat = textBox1.Text;
            userInfo.ConfirmOverwrite = checkBox1.Checked;
            userInfo.ShowStartupDialog = checkBox4.Checked;
            userInfo.BreakAfterFirstError = checkBox2.Checked;
            userInfo.WriteDataManagerProcessed = this.cbWriteDataManagerProcessed.Checked;
            userInfo.LogBookOnlyWhenValuesChanged = radioButton1.Checked;
            userInfo.CaseSensitive = cbCase.Checked;
            userInfo.SaveLocationInStartupDir = cbSaveLocation.Checked;
            Close();
        }
    }
}
