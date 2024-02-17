namespace DataManagerGUI
{
    partial class ParameterManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParameterManager));
            this.cmbField = new System.Windows.Forms.ComboBox();
            this.cmbModifier = new System.Windows.Forms.ComboBox();
            this.cmbLimitedValues = new System.Windows.Forms.ComboBox();
            this.pnlField = new System.Windows.Forms.Panel();
            this.pnlLimitedValues = new System.Windows.Forms.Panel();
            this.pnlMultiValues = new System.Windows.Forms.Panel();
            this.btnMultiValueEdit = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtMultiValue = new System.Windows.Forms.TextBox();
            this.pnlNumericRange = new System.Windows.Forms.Panel();
            this.numNumericRangeLower = new System.Windows.Forms.NumericUpDown();
            this.numNumericRangeUpper = new System.Windows.Forms.NumericUpDown();
            this.lblNumericRangeFrom = new System.Windows.Forms.Label();
            this.lblNumericRangeTo = new System.Windows.Forms.Label();
            this.pnlNumericValue = new System.Windows.Forms.Panel();
            this.numNumericValue = new System.Windows.Forms.NumericUpDown();
            this.pnlDateTimeRange = new System.Windows.Forms.Panel();
            this.dtDateTimeRangeUpper = new DataManagerGUI.DateTimeSlicker();
            this.dtDateTimeRangLower = new DataManagerGUI.DateTimeSlicker();
            this.lblDateTimeRangeTo = new System.Windows.Forms.Label();
            this.lblDateTimeRangeFrom = new System.Windows.Forms.Label();
            this.pnlTextValue = new System.Windows.Forms.Panel();
            this.txtTextValue = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCut = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddFieldValue = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlModifier = new System.Windows.Forms.Panel();
            this.pnlDateTimeValue = new System.Windows.Forms.Panel();
            this.dtDateTimeValue = new DataManagerGUI.DateTimeSlicker();
            this.pnlReplaceValue = new System.Windows.Forms.Panel();
            this.txtReplaceNewValue = new System.Windows.Forms.TextBox();
            this.txtReplaceOldValue = new System.Windows.Forms.TextBox();
            this.lblReplaceValueWith = new System.Windows.Forms.Label();
            this.pnlCalcValue = new System.Windows.Forms.Panel();
            this.txtCalcValue = new System.Windows.Forms.TextBox();
            this.pnlSelectableValue = new System.Windows.Forms.Panel();
            this.cmbTextValue = new System.Windows.Forms.ComboBox();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.regExVarReplace1 = new DataManagerGUI.RegExVarReplace();
            this.pnlField.SuspendLayout();
            this.pnlLimitedValues.SuspendLayout();
            this.pnlMultiValues.SuspendLayout();
            this.pnlNumericRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumericRangeLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumericRangeUpper)).BeginInit();
            this.pnlNumericValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNumericValue)).BeginInit();
            this.pnlDateTimeRange.SuspendLayout();
            this.pnlTextValue.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlModifier.SuspendLayout();
            this.pnlDateTimeValue.SuspendLayout();
            this.pnlReplaceValue.SuspendLayout();
            this.pnlCalcValue.SuspendLayout();
            this.pnlSelectableValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbField
            // 
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Location = new System.Drawing.Point(3, 3);
            this.cmbField.Name = "cmbField";
            this.cmbField.Size = new System.Drawing.Size(214, 21);
            this.cmbField.TabIndex = 0;
            this.cmbField.TextUpdate += new System.EventHandler(this.cmbField_SelectedIndexChanged);
            this.cmbField.TextChanged += new System.EventHandler(this.cmbField_SelectedIndexChanged);
            this.cmbField.Validated += new System.EventHandler(this.cmbField_Validated);
            // 
            // cmbModifier
            // 
            this.cmbModifier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModifier.FormattingEnabled = true;
            this.cmbModifier.Location = new System.Drawing.Point(3, 3);
            this.cmbModifier.Name = "cmbModifier";
            this.cmbModifier.Size = new System.Drawing.Size(214, 21);
            this.cmbModifier.TabIndex = 0;
            this.cmbModifier.SelectedIndexChanged += new System.EventHandler(this.cmbModifier_SelectedIndexChanged);
            // 
            // cmbLimitedValues
            // 
            this.cmbLimitedValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLimitedValues.FormattingEnabled = true;
            this.cmbLimitedValues.Location = new System.Drawing.Point(3, 3);
            this.cmbLimitedValues.Name = "cmbLimitedValues";
            this.cmbLimitedValues.Size = new System.Drawing.Size(214, 21);
            this.cmbLimitedValues.TabIndex = 0;
            this.cmbLimitedValues.SelectedIndexChanged += new System.EventHandler(this.cmbLimitedValues_SelectedIndexChanged);
            // 
            // pnlField
            // 
            this.pnlField.Controls.Add(this.cmbField);
            this.pnlField.Location = new System.Drawing.Point(3, 3);
            this.pnlField.Name = "pnlField";
            this.pnlField.Size = new System.Drawing.Size(220, 27);
            this.pnlField.TabIndex = 0;
            this.pnlField.SizeChanged += new System.EventHandler(this.pnlField_SizeChanged);
            // 
            // pnlLimitedValues
            // 
            this.pnlLimitedValues.Controls.Add(this.cmbLimitedValues);
            this.pnlLimitedValues.Location = new System.Drawing.Point(449, 3);
            this.pnlLimitedValues.Name = "pnlLimitedValues";
            this.pnlLimitedValues.Size = new System.Drawing.Size(220, 27);
            this.pnlLimitedValues.TabIndex = 8;
            this.pnlLimitedValues.SizeChanged += new System.EventHandler(this.pnlLimitedValues_SizeChanged);
            // 
            // pnlMultiValues
            // 
            this.pnlMultiValues.Controls.Add(this.btnMultiValueEdit);
            this.pnlMultiValues.Controls.Add(this.txtMultiValue);
            this.pnlMultiValues.Location = new System.Drawing.Point(449, 3);
            this.pnlMultiValues.Name = "pnlMultiValues";
            this.pnlMultiValues.Size = new System.Drawing.Size(220, 27);
            this.pnlMultiValues.TabIndex = 7;
            this.pnlMultiValues.SizeChanged += new System.EventHandler(this.pnlMultiValues_SizeChanged);
            // 
            // btnMultiValueEdit
            // 
            this.btnMultiValueEdit.ImageIndex = 0;
            this.btnMultiValueEdit.ImageList = this.imageList1;
            this.btnMultiValueEdit.Location = new System.Drawing.Point(193, 2);
            this.btnMultiValueEdit.Name = "btnMultiValueEdit";
            this.btnMultiValueEdit.Size = new System.Drawing.Size(23, 23);
            this.btnMultiValueEdit.TabIndex = 1;
            this.btnMultiValueEdit.UseVisualStyleBackColor = true;
            this.btnMultiValueEdit.Click += new System.EventHandler(this.btnMultiValueEdit_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "edit.png");
            // 
            // txtMultiValue
            // 
            this.txtMultiValue.ContextMenuStrip = this.contextMenuStrip1;
            this.txtMultiValue.HideSelection = false;
            this.txtMultiValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtMultiValue.Location = new System.Drawing.Point(3, 3);
            this.txtMultiValue.Name = "txtMultiValue";
            this.txtMultiValue.Size = new System.Drawing.Size(187, 20);
            this.txtMultiValue.TabIndex = 0;
            this.txtMultiValue.TabStop = false;
            this.txtMultiValue.TextChanged += new System.EventHandler(this.txtMultiValue_TextChanged);
            // 
            // pnlNumericRange
            // 
            this.pnlNumericRange.Controls.Add(this.numNumericRangeLower);
            this.pnlNumericRange.Controls.Add(this.numNumericRangeUpper);
            this.pnlNumericRange.Controls.Add(this.lblNumericRangeFrom);
            this.pnlNumericRange.Controls.Add(this.lblNumericRangeTo);
            this.pnlNumericRange.Location = new System.Drawing.Point(449, 3);
            this.pnlNumericRange.Name = "pnlNumericRange";
            this.pnlNumericRange.Size = new System.Drawing.Size(220, 27);
            this.pnlNumericRange.TabIndex = 6;
            this.pnlNumericRange.SizeChanged += new System.EventHandler(this.pnlNumericRange_SizeChanged);
            // 
            // numNumericRangeLower
            // 
            this.numNumericRangeLower.DecimalPlaces = 2;
            this.numNumericRangeLower.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numNumericRangeLower.Location = new System.Drawing.Point(39, 3);
            this.numNumericRangeLower.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            131072});
            this.numNumericRangeLower.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericRangeLower.Name = "numNumericRangeLower";
            this.numNumericRangeLower.Size = new System.Drawing.Size(77, 20);
            this.numNumericRangeLower.TabIndex = 10;
            this.numNumericRangeLower.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNumericRangeLower.ThousandsSeparator = true;
            this.numNumericRangeLower.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericRangeLower.ValueChanged += new System.EventHandler(this.numNumericRangeLower_ValueChanged);
            // 
            // numNumericRangeUpper
            // 
            this.numNumericRangeUpper.DecimalPlaces = 2;
            this.numNumericRangeUpper.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numNumericRangeUpper.Location = new System.Drawing.Point(143, 3);
            this.numNumericRangeUpper.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            131072});
            this.numNumericRangeUpper.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericRangeUpper.Name = "numNumericRangeUpper";
            this.numNumericRangeUpper.Size = new System.Drawing.Size(77, 20);
            this.numNumericRangeUpper.TabIndex = 9;
            this.numNumericRangeUpper.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNumericRangeUpper.ThousandsSeparator = true;
            this.numNumericRangeUpper.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericRangeUpper.ValueChanged += new System.EventHandler(this.numNumericRangeUpper_ValueChanged);
            // 
            // lblNumericRangeFrom
            // 
            this.lblNumericRangeFrom.Location = new System.Drawing.Point(3, 3);
            this.lblNumericRangeFrom.Name = "lblNumericRangeFrom";
            this.lblNumericRangeFrom.Size = new System.Drawing.Size(30, 20);
            this.lblNumericRangeFrom.TabIndex = 7;
            this.lblNumericRangeFrom.Text = "From";
            this.lblNumericRangeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNumericRangeTo
            // 
            this.lblNumericRangeTo.Location = new System.Drawing.Point(121, 3);
            this.lblNumericRangeTo.Name = "lblNumericRangeTo";
            this.lblNumericRangeTo.Size = new System.Drawing.Size(20, 20);
            this.lblNumericRangeTo.TabIndex = 8;
            this.lblNumericRangeTo.Text = "To";
            this.lblNumericRangeTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlNumericValue
            // 
            this.pnlNumericValue.Controls.Add(this.numNumericValue);
            this.pnlNumericValue.Location = new System.Drawing.Point(449, 3);
            this.pnlNumericValue.Name = "pnlNumericValue";
            this.pnlNumericValue.Size = new System.Drawing.Size(220, 27);
            this.pnlNumericValue.TabIndex = 2;
            this.pnlNumericValue.SizeChanged += new System.EventHandler(this.pnlNumericValue_SizeChanged);
            // 
            // numNumericValue
            // 
            this.numNumericValue.DecimalPlaces = 2;
            this.numNumericValue.Location = new System.Drawing.Point(3, 3);
            this.numNumericValue.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            131072});
            this.numNumericValue.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericValue.Name = "numNumericValue";
            this.numNumericValue.Size = new System.Drawing.Size(214, 20);
            this.numNumericValue.TabIndex = 0;
            this.numNumericValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numNumericValue.Value = new decimal(new int[] {
            100,
            0,
            0,
            -2147352576});
            this.numNumericValue.ValueChanged += new System.EventHandler(this.numNumericValue_ValueChanged);
            // 
            // pnlDateTimeRange
            // 
            this.pnlDateTimeRange.Controls.Add(this.dtDateTimeRangeUpper);
            this.pnlDateTimeRange.Controls.Add(this.dtDateTimeRangLower);
            this.pnlDateTimeRange.Controls.Add(this.lblDateTimeRangeTo);
            this.pnlDateTimeRange.Controls.Add(this.lblDateTimeRangeFrom);
            this.pnlDateTimeRange.Location = new System.Drawing.Point(449, 3);
            this.pnlDateTimeRange.Name = "pnlDateTimeRange";
            this.pnlDateTimeRange.Size = new System.Drawing.Size(220, 27);
            this.pnlDateTimeRange.TabIndex = 5;
            this.pnlDateTimeRange.SizeChanged += new System.EventHandler(this.pnlDateTimeRange_SizeChanged);
            // 
            // dtDateTimeRangeUpper
            // 
            this.dtDateTimeRangeUpper.CustomFormat = "yyyy/MM/dd";
            this.dtDateTimeRangeUpper.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateTimeRangeUpper.Location = new System.Drawing.Point(141, 3);
            this.dtDateTimeRangeUpper.Name = "dtDateTimeRangeUpper";
            this.dtDateTimeRangeUpper.ShowCheckBox = true;
            this.dtDateTimeRangeUpper.Size = new System.Drawing.Size(77, 20);
            this.dtDateTimeRangeUpper.TabIndex = 1;
            this.dtDateTimeRangeUpper.Value = new System.DateTime(2013, 5, 15, 18, 33, 16, 742);
            this.dtDateTimeRangeUpper.ValueChanged += new System.EventHandler(this.dtDateTimeRangeUpper_ValueChanged);
            // 
            // dtDateTimeRangLower
            // 
            this.dtDateTimeRangLower.CustomFormat = "yyyy/MM/dd";
            this.dtDateTimeRangLower.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateTimeRangLower.Location = new System.Drawing.Point(39, 3);
            this.dtDateTimeRangLower.Name = "dtDateTimeRangLower";
            this.dtDateTimeRangLower.ShowCheckBox = true;
            this.dtDateTimeRangLower.Size = new System.Drawing.Size(77, 20);
            this.dtDateTimeRangLower.TabIndex = 0;
            this.dtDateTimeRangLower.Value = new System.DateTime(2013, 5, 15, 18, 33, 16, 745);
            this.dtDateTimeRangLower.ValueChanged += new System.EventHandler(this.dtDateTimeRangLower_ValueChanged);
            // 
            // lblDateTimeRangeTo
            // 
            this.lblDateTimeRangeTo.Location = new System.Drawing.Point(119, 3);
            this.lblDateTimeRangeTo.Name = "lblDateTimeRangeTo";
            this.lblDateTimeRangeTo.Size = new System.Drawing.Size(20, 20);
            this.lblDateTimeRangeTo.TabIndex = 3;
            this.lblDateTimeRangeTo.Text = "To";
            this.lblDateTimeRangeTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDateTimeRangeFrom
            // 
            this.lblDateTimeRangeFrom.Location = new System.Drawing.Point(4, 3);
            this.lblDateTimeRangeFrom.Name = "lblDateTimeRangeFrom";
            this.lblDateTimeRangeFrom.Size = new System.Drawing.Size(30, 20);
            this.lblDateTimeRangeFrom.TabIndex = 2;
            this.lblDateTimeRangeFrom.Text = "From";
            this.lblDateTimeRangeFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlTextValue
            // 
            this.pnlTextValue.Controls.Add(this.txtTextValue);
            this.pnlTextValue.Location = new System.Drawing.Point(449, 3);
            this.pnlTextValue.Name = "pnlTextValue";
            this.pnlTextValue.Size = new System.Drawing.Size(220, 27);
            this.pnlTextValue.TabIndex = 4;
            this.pnlTextValue.SizeChanged += new System.EventHandler(this.pnlTextValue_SizeChanged);
            // 
            // txtTextValue
            // 
            this.txtTextValue.ContextMenuStrip = this.contextMenuStrip1;
            this.txtTextValue.HideSelection = false;
            this.txtTextValue.Location = new System.Drawing.Point(4, 3);
            this.txtTextValue.Name = "txtTextValue";
            this.txtTextValue.Size = new System.Drawing.Size(213, 20);
            this.txtTextValue.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopy,
            this.tsmiCut,
            this.tsmiPaste,
            this.tsmiDelete,
            this.toolStripSeparator1,
            this.tsmiAddFieldValue});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(163, 120);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.cmsCalcMenu_Opening);
            // 
            // tsmiCopy
            // 
            this.tsmiCopy.Name = "tsmiCopy";
            this.tsmiCopy.Size = new System.Drawing.Size(162, 22);
            this.tsmiCopy.Text = "Copy";
            this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
            // 
            // tsmiCut
            // 
            this.tsmiCut.Name = "tsmiCut";
            this.tsmiCut.Size = new System.Drawing.Size(162, 22);
            this.tsmiCut.Text = "Cut";
            this.tsmiCut.Click += new System.EventHandler(this.tsmiCut_Click);
            // 
            // tsmiPaste
            // 
            this.tsmiPaste.Name = "tsmiPaste";
            this.tsmiPaste.Size = new System.Drawing.Size(162, 22);
            this.tsmiPaste.Text = "Paste";
            this.tsmiPaste.Click += new System.EventHandler(this.tsmiPaste_Click);
            // 
            // tsmiDelete
            // 
            this.tsmiDelete.Name = "tsmiDelete";
            this.tsmiDelete.Size = new System.Drawing.Size(162, 22);
            this.tsmiDelete.Text = "Delete";
            this.tsmiDelete.Click += new System.EventHandler(this.tsmiDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // tsmiAddFieldValue
            // 
            this.tsmiAddFieldValue.Name = "tsmiAddFieldValue";
            this.tsmiAddFieldValue.Size = new System.Drawing.Size(162, 22);
            this.tsmiAddFieldValue.Text = "Insert Field Value";
            // 
            // pnlModifier
            // 
            this.pnlModifier.Controls.Add(this.cmbModifier);
            this.pnlModifier.Location = new System.Drawing.Point(226, 3);
            this.pnlModifier.Name = "pnlModifier";
            this.pnlModifier.Size = new System.Drawing.Size(220, 27);
            this.pnlModifier.TabIndex = 1;
            this.pnlModifier.SizeChanged += new System.EventHandler(this.pnlModifier_SizeChanged);
            // 
            // pnlDateTimeValue
            // 
            this.pnlDateTimeValue.Controls.Add(this.dtDateTimeValue);
            this.pnlDateTimeValue.Location = new System.Drawing.Point(449, 3);
            this.pnlDateTimeValue.Name = "pnlDateTimeValue";
            this.pnlDateTimeValue.Size = new System.Drawing.Size(220, 27);
            this.pnlDateTimeValue.TabIndex = 3;
            this.pnlDateTimeValue.SizeChanged += new System.EventHandler(this.pnlDateTimeValue_SizeChanged);
            // 
            // dtDateTimeValue
            // 
            this.dtDateTimeValue.Checked = false;
            this.dtDateTimeValue.CustomFormat = "yyyy/MM/dd";
            this.dtDateTimeValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateTimeValue.Location = new System.Drawing.Point(3, 3);
            this.dtDateTimeValue.Name = "dtDateTimeValue";
            this.dtDateTimeValue.ShowCheckBox = true;
            this.dtDateTimeValue.Size = new System.Drawing.Size(214, 20);
            this.dtDateTimeValue.TabIndex = 0;
            this.dtDateTimeValue.Value = ((object)(resources.GetObject("dtDateTimeValue.Value")));
            this.dtDateTimeValue.CheckedChanged += new System.EventHandler(this.dtDateTimeValue_ValueChanged);
            this.dtDateTimeValue.ValueChanged += new System.EventHandler(this.dtDateTimeValue_ValueChanged);
            // 
            // pnlReplaceValue
            // 
            this.pnlReplaceValue.Controls.Add(this.txtReplaceNewValue);
            this.pnlReplaceValue.Controls.Add(this.txtReplaceOldValue);
            this.pnlReplaceValue.Controls.Add(this.lblReplaceValueWith);
            this.pnlReplaceValue.Location = new System.Drawing.Point(449, 3);
            this.pnlReplaceValue.Name = "pnlReplaceValue";
            this.pnlReplaceValue.Size = new System.Drawing.Size(220, 27);
            this.pnlReplaceValue.TabIndex = 9;
            this.pnlReplaceValue.SizeChanged += new System.EventHandler(this.pnlReplaceValue_SizeChanged);
            // 
            // txtReplaceNewValue
            // 
            this.txtReplaceNewValue.ContextMenuStrip = this.contextMenuStrip1;
            this.txtReplaceNewValue.HideSelection = false;
            this.txtReplaceNewValue.Location = new System.Drawing.Point(128, 4);
            this.txtReplaceNewValue.Name = "txtReplaceNewValue";
            this.txtReplaceNewValue.Size = new System.Drawing.Size(89, 20);
            this.txtReplaceNewValue.TabIndex = 2;
            this.txtReplaceNewValue.TextChanged += new System.EventHandler(this.txtReplaceNewValue_TextChanged);
            // 
            // txtReplaceOldValue
            // 
            this.txtReplaceOldValue.ContextMenuStrip = this.contextMenuStrip1;
            this.txtReplaceOldValue.HideSelection = false;
            this.txtReplaceOldValue.Location = new System.Drawing.Point(3, 4);
            this.txtReplaceOldValue.Name = "txtReplaceOldValue";
            this.txtReplaceOldValue.Size = new System.Drawing.Size(89, 20);
            this.txtReplaceOldValue.TabIndex = 0;
            this.txtReplaceOldValue.TextChanged += new System.EventHandler(this.txtReplaceOldValue_TextChanged);
            // 
            // lblReplaceValueWith
            // 
            this.lblReplaceValueWith.Location = new System.Drawing.Point(96, 3);
            this.lblReplaceValueWith.Name = "lblReplaceValueWith";
            this.lblReplaceValueWith.Size = new System.Drawing.Size(30, 20);
            this.lblReplaceValueWith.TabIndex = 1;
            this.lblReplaceValueWith.Text = "With";
            this.lblReplaceValueWith.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCalcValue
            // 
            this.pnlCalcValue.Controls.Add(this.txtCalcValue);
            this.pnlCalcValue.Location = new System.Drawing.Point(449, 3);
            this.pnlCalcValue.Name = "pnlCalcValue";
            this.pnlCalcValue.Size = new System.Drawing.Size(220, 27);
            this.pnlCalcValue.TabIndex = 6;
            this.pnlCalcValue.SizeChanged += new System.EventHandler(this.pnlCalcValue_SizeChanged);
            // 
            // txtCalcValue
            // 
            this.txtCalcValue.CausesValidation = false;
            this.txtCalcValue.ContextMenuStrip = this.contextMenuStrip1;
            this.txtCalcValue.HideSelection = false;
            this.txtCalcValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtCalcValue.Location = new System.Drawing.Point(3, 3);
            this.txtCalcValue.Name = "txtCalcValue";
            this.txtCalcValue.ShortcutsEnabled = false;
            this.txtCalcValue.Size = new System.Drawing.Size(214, 20);
            this.txtCalcValue.TabIndex = 0;
            this.txtCalcValue.TextChanged += new System.EventHandler(this.txtCalcValue_TextChanged);
            // 
            // pnlSelectableValue
            // 
            this.pnlSelectableValue.Controls.Add(this.cmbTextValue);
            this.pnlSelectableValue.Location = new System.Drawing.Point(449, 3);
            this.pnlSelectableValue.Name = "pnlSelectableValue";
            this.pnlSelectableValue.Size = new System.Drawing.Size(220, 27);
            this.pnlSelectableValue.TabIndex = 4;
            this.pnlSelectableValue.SizeChanged += new System.EventHandler(this.pnlSelectableValue_SizeChanged);
            // 
            // cmbTextValue
            // 
            this.cmbTextValue.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbTextValue.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbTextValue.Location = new System.Drawing.Point(3, 3);
            this.cmbTextValue.Name = "cmbTextValue";
            this.cmbTextValue.Size = new System.Drawing.Size(215, 21);
            this.cmbTextValue.TabIndex = 1;
            this.cmbTextValue.TextChanged += new System.EventHandler(this.cmbTextValue_TextChanged);
            // 
            // regExVarReplace1
            // 
            this.regExVarReplace1.Location = new System.Drawing.Point(449, 3);
            this.regExVarReplace1.Margin = new System.Windows.Forms.Padding(0);
            this.regExVarReplace1.Name = "regExVarReplace1";
            this.regExVarReplace1.Results = "";
            this.regExVarReplace1.Size = new System.Drawing.Size(220, 23);
            this.regExVarReplace1.TabIndex = 10;
            this.regExVarReplace1.RegularExpressionChanged += new System.EventHandler(this.regExVarReplace1_RegularExpressionChanged);
            // 
            // ParameterManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTextValue);
            this.Controls.Add(this.regExVarReplace1);
            this.Controls.Add(this.pnlModifier);
            this.Controls.Add(this.pnlField);
            this.Controls.Add(this.pnlDateTimeRange);
            this.Controls.Add(this.pnlReplaceValue);
            this.Controls.Add(this.pnlMultiValues);
            this.Controls.Add(this.pnlNumericRange);
            this.Controls.Add(this.pnlCalcValue);
            this.Controls.Add(this.pnlLimitedValues);
            this.Controls.Add(this.pnlNumericValue);
            this.Controls.Add(this.pnlDateTimeValue);
            this.Controls.Add(this.pnlSelectableValue);
            this.MaximumSize = new System.Drawing.Size(2000, 33);
            this.MinimumSize = new System.Drawing.Size(612, 33);
            this.Name = "ParameterManager";
            this.Size = new System.Drawing.Size(670, 33);
            this.SizeChanged += new System.EventHandler(this.DataManagerParameterManager_SizeChanged);
            this.pnlField.ResumeLayout(false);
            this.pnlLimitedValues.ResumeLayout(false);
            this.pnlMultiValues.ResumeLayout(false);
            this.pnlMultiValues.PerformLayout();
            this.pnlNumericRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numNumericRangeLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNumericRangeUpper)).EndInit();
            this.pnlNumericValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numNumericValue)).EndInit();
            this.pnlDateTimeRange.ResumeLayout(false);
            this.pnlTextValue.ResumeLayout(false);
            this.pnlTextValue.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlModifier.ResumeLayout(false);
            this.pnlDateTimeValue.ResumeLayout(false);
            this.pnlReplaceValue.ResumeLayout(false);
            this.pnlReplaceValue.PerformLayout();
            this.pnlCalcValue.ResumeLayout(false);
            this.pnlCalcValue.PerformLayout();
            this.pnlSelectableValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbField;
        private System.Windows.Forms.ComboBox cmbModifier;
        private System.Windows.Forms.ComboBox cmbLimitedValues;
        private System.Windows.Forms.Panel pnlField;
        private System.Windows.Forms.Panel pnlLimitedValues;
        private System.Windows.Forms.Panel pnlMultiValues;
        private System.Windows.Forms.Button btnMultiValueEdit;
        private System.Windows.Forms.TextBox txtMultiValue;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel pnlNumericRange;
        private System.Windows.Forms.NumericUpDown numNumericRangeLower;
        private System.Windows.Forms.NumericUpDown numNumericRangeUpper;
        private System.Windows.Forms.Label lblNumericRangeTo;
        private System.Windows.Forms.Label lblNumericRangeFrom;
        private System.Windows.Forms.Panel pnlNumericValue;
        private System.Windows.Forms.NumericUpDown numNumericValue;
        private System.Windows.Forms.Panel pnlDateTimeRange;
        private System.Windows.Forms.Label lblDateTimeRangeTo;
        private System.Windows.Forms.Label lblDateTimeRangeFrom;
        private System.Windows.Forms.Panel pnlTextValue;
        private System.Windows.Forms.Panel pnlModifier;
        private System.Windows.Forms.Panel pnlDateTimeValue;
        private System.Windows.Forms.Panel pnlReplaceValue;
        private DateTimeSlicker dtDateTimeRangeUpper;
        private DateTimeSlicker dtDateTimeRangLower;
        private DateTimeSlicker dtDateTimeValue;
        private System.Windows.Forms.Label lblReplaceValueWith;
        private System.Windows.Forms.Panel pnlCalcValue;
        private System.Windows.Forms.TextBox txtCalcValue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiCopy;
        private System.Windows.Forms.ToolStripMenuItem tsmiCut;
        private System.Windows.Forms.ToolStripMenuItem tsmiPaste;
        private System.Windows.Forms.ToolStripMenuItem tsmiDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddFieldValue;
        private System.Windows.Forms.TextBox txtTextValue;
        private System.Windows.Forms.Panel pnlSelectableValue;
        private System.Windows.Forms.ComboBox cmbTextValue;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.TextBox txtReplaceOldValue;
        private System.Windows.Forms.TextBox txtReplaceNewValue;
        private RegExVarReplace regExVarReplace1;
    }
}
