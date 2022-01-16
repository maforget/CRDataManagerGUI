using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataManagerGUI
{
    public partial class ParameterManager : UserControl
    {
        ParameterType _type;
        dmUserInfo userInfo;
        [Bindable(true)]
        [Category("Data")]

        TextBox txtBeingEdited;

        [Bindable(true)]
        [Category("Data")]
        public ParameterType ParameterTypeRestriction
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    FillFields();
                    FillModifiers();
                }
            }
        }

        private void FillFields()
        {
            cmbField.Items.Clear();
            switch (_type)
            {
                case ParameterType.Action:
                    cmbField.Items.AddRange(Global.GetAcceptedActionFields());
                    break;
                case ParameterType.Rule:
                    cmbField.Items.AddRange(Global.AllowedKeys);
                    break;
            }
            if (cmbField.Items.Count > 0) cmbField.SelectedIndex = 0;

        }

        private void FillModifiers()
        {
            cmbModifier.Items.Clear();
            switch (this._type)
            {
                case ParameterType.Action:
                    cmbModifier.Items.AddRange(Global.GetAllowedValueModifiers(cmbField.Text));
                    break;
                case ParameterType.Rule:
                    cmbModifier.Items.AddRange(Global.GetAllowedKeyModifiers(cmbField.Text));
                    break;
            }
            if (cmbModifier.Items.Count > 0) cmbModifier.SelectedIndex = 0;
        }


        public ParameterManager()
        {
            InitializeComponent();
            FillFields();
        }

        public void SetUserInfo(dmUserInfo uiItem)
        {
            this.userInfo = uiItem;
        }

        private keyType GetKeyType()
        {
            return Global.GetKeyType(cmbField.Text);
        }
        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string PreviousModifier = cmbModifier.Text;

            FillModifiers();

            if (cmbModifier.Items.Contains(PreviousModifier))
                cmbModifier.SelectedItem = PreviousModifier;
        }

        #region Dynamic Sizing

        private void pnlField_SizeChanged(object sender, EventArgs e)
        {
            cmbField.Size = new Size(pnlField.Width - 6, 20);
            cmbField.Location = new Point(3, 3);
        }

        private void pnlModifier_SizeChanged(object sender, EventArgs e)
        {
            cmbModifier.Size = new Size(pnlModifier.Width - 6, 21);
            cmbModifier.Location = new Point(3, 3);
        }

        private void pnlDateTimeValue_SizeChanged(object sender, EventArgs e)
        {
            dtDateTimeValue.Size = new Size(pnlDateTimeValue.Width - 6, 20);
            dtDateTimeValue.Location = new Point(3, 3);
        }

        private void pnlTextValue_SizeChanged(object sender, EventArgs e)
        {
            txtTextValue.Size = new Size(pnlTextValue.Width - 6, 20);
        }

        private void pnlSelectableValue_SizeChanged(object sender, EventArgs e)
        {
            cmbTextValue.Size = new Size(pnlSelectableValue.Width - 6, 20);
        }

        private void pnlMultiValues_SizeChanged(object sender, EventArgs e)
        {
            txtMultiValue.Size = new Size(pnlMultiValues.Width - 32, 20);
            txtMultiValue.Location = new Point(3, 3);
            btnMultiValueEdit.Location = new Point(txtMultiValue.Right + 3, 3);
        }

        private void pnlLimitedValues_SizeChanged(object sender, EventArgs e)
        {
            cmbLimitedValues.Size = new Size(pnlLimitedValues.Width - 6, 21);
            cmbLimitedValues.Location = new Point(3, 3);
        }

        private void pnlDateTimeRange_SizeChanged(object sender, EventArgs e)
        {
            lblDateTimeRangeFrom.Location = new Point(3, 3);
            dtDateTimeRangLower.Size = new Size((pnlDateTimeRange.Width - 65) / 2, 21);
            dtDateTimeRangLower.Location = new Point(lblDateTimeRangeFrom.Right + 3, 3);
            lblDateTimeRangeTo.Location = new Point(dtDateTimeRangLower.Right + 3, 3);
            dtDateTimeRangeUpper.Size = dtDateTimeRangLower.Size;
            dtDateTimeRangeUpper.Location = new Point(lblDateTimeRangeTo.Right + 3, 3);
        }

        private void pnlNumericRange_SizeChanged(object sender, EventArgs e)
        {
            lblNumericRangeFrom.Location = new Point(3, 3);
            numNumericRangeLower.Size = new Size((pnlNumericRange.Width - 65) / 2, 21);
            numNumericRangeLower.Location = new Point(lblNumericRangeFrom.Right + 3, 3);
            lblNumericRangeTo.Location = new Point(numNumericRangeLower.Right + 3, 3);
            numNumericRangeUpper.Size = numNumericRangeLower.Size;
            numNumericRangeUpper.Location = new Point(lblNumericRangeTo.Right + 3, 3);
        }

        private void pnlReplaceValue_SizeChanged(object sender, EventArgs e)
        {
            Size textboxSizes = new Size((pnlReplaceValue.Width - 42) / 2, 20);
            txtReplaceOldValue.Size = textboxSizes;
            txtReplaceNewValue.Size = textboxSizes;
            txtReplaceOldValue.Location = new Point(3, 3);
            lblReplaceValueWith.Location = new Point(txtReplaceOldValue.Right + 3, 3);
            txtReplaceNewValue.Location = new Point(lblReplaceValueWith.Right + 3, 3);
        }

        private void DataManagerParameterManager_SizeChanged(object sender, EventArgs e)
        {
            Size panelsize = new Size((this.Width - 12) / 3, 27);

            //set sizes
            pnlField.Size = panelsize;
            pnlModifier.Size = panelsize;

            pnlDateTimeRange.Size = panelsize;
            pnlDateTimeValue.Size = panelsize;
            pnlLimitedValues.Size = panelsize;
            pnlMultiValues.Size = panelsize;
            pnlNumericRange.Size = panelsize;
            pnlNumericValue.Size = panelsize;
            pnlReplaceValue.Size = panelsize;
            pnlTextValue.Size = panelsize;
            pnlCalcValue.Size = panelsize;
            pnlSelectableValue.Size = panelsize;
            regExVarReplace1.Size = panelsize;

            //line them up            
            pnlField.Location = new Point(3, 3);
            pnlModifier.Location = new Point(pnlField.Right + 3, 3);

            Point panelLocations = new Point(pnlModifier.Right + 3, 3);

            pnlDateTimeRange.Location = panelLocations;
            pnlDateTimeValue.Location = panelLocations;
            pnlLimitedValues.Location = panelLocations;
            pnlMultiValues.Location = panelLocations;
            pnlNumericRange.Location = panelLocations;
            pnlNumericValue.Location = panelLocations;
            pnlReplaceValue.Location = panelLocations;
            pnlTextValue.Location = panelLocations;
            pnlCalcValue.Location = panelLocations;
            pnlSelectableValue.Location = panelLocations;
            regExVarReplace1.Location = panelLocations;
        }

        #endregion

        private void btnMultiValueEdit_Click(object sender, EventArgs e)
        {
            Form Editor = null;

            List<string> strIn = new List<string>(txtMultiValue.Text.Split(new string[] { Global.DELIMITER }, StringSplitOptions.None));
            if (string.IsNullOrEmpty(txtMultiValue.Text)) strIn = new List<string>();
            switch (GetKeyType())
            {
                case keyType.List:
                case keyType.String:
                case keyType.Numeric:
                    Editor = new MultiEdit(strIn.ToArray());
                    break;
                case keyType.LanguageISO:
                    for (int i = 0; i < strIn.Count; i++)
                    {
                        strIn[i] = Global.LanguageISOs.First(n => n.Value == strIn[i]).Key;
                    }
                    Editor = new Language_Add("Add Languages ISO Values", Global.LanguageISOs.Keys.ToArray(), strIn.ToArray());
                    break;
                default:
                    break;
            }

            if (Editor != null)
            {
                if (Editor.ShowDialog() == DialogResult.OK)
                {
                    if (Editor.GetType() == typeof(MultiEdit))
                    {
                        txtMultiValue.Text = ((MultiEdit)Editor).Results;
                    }
                    else
                    {
                        strIn = new List<string>(((Language_Add)Editor).Result);

                        for (int i = 0; i < strIn.Count; i++)
                        {
                            strIn[i] = Global.LanguageISOs[strIn[i]];
                        }
                        txtMultiValue.Text = strIn.ToArray().Join(Global.DELIMITER);
                    }
                }
            }
        }

        private void txtMultiValue_TextChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            keyType FieldType = Global.GetKeyType(cmbField.Text);

            switch (FieldType)
            {
                case keyType.Custom:
                case keyType.String:
                case keyType.List:
                    switch (cmbModifier.Text)
                    {
                        case "IsAnyOf":
                        case "NotIsAnyOf":
                        case "StartsWithAnyOf":
                        case "NotStartsWithAnyOf":
                        case "ContainsAnyOf":
                        case "NotContainsAnyOf":
                        case "ContainsAllOf":
                        case "Add":
                            txtTextValue.Text = txtMultiValue.Text;
                            break;
                        default:
                            break;

                    }
                    break;
                case keyType.Numeric:
                    switch (cmbModifier.Text)
                    {
                        case "IsAnyOf":
                        case "NotIsAnyOf":
                            txtTextValue.Text = txtMultiValue.Text;
                            break;
                        default:
                            break;
                    }
                    break;
                case keyType.LanguageISO:
                    txtTextValue.Text = txtMultiValue.Text;
                    break;
                default:
                    break;
            }
        }

        private void cmbModifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbField.Text)) return;
            keyType FieldType = Global.GetKeyType(cmbField.Text);

            //kill all panels
            pnlDateTimeRange.Visible = false;
            pnlDateTimeValue.Visible = false;
            pnlLimitedValues.Visible = false;
            pnlMultiValues.Visible = false;
            pnlNumericRange.Visible = false;
            pnlNumericValue.Visible = false;
            pnlReplaceValue.Visible = false;
            pnlTextValue.Visible = false;
            pnlCalcValue.Visible = false;
            pnlSelectableValue.Visible = false;
            regExVarReplace1.Visible = false;
            
            //clean all values
            //txtMultiValue.Text = "";
            //txtReplaceNewValue.Text = "";
            //txtReplaceOldValue.Text = "";
            //txtTextValue.Text = "";
            //txtCalcValue.Text = "";
            cmbLimitedValues.Items.Clear();
            numNumericValue.Value = new decimal(0);
            dtDateTimeRangLower.Value = dtDateTimeRangLower.MinDate;
            dtDateTimeRangeUpper.Value = dtDateTimeRangeUpper.MinDate;


            switch (FieldType)
            {
                case keyType.Custom:
                case keyType.String:
                    if (cmbModifier.Text == "Replace" || cmbModifier.Text == "RegexReplace")
                    {
                        pnlReplaceValue.Visible = true;
                        string[] splitStr = txtTextValue.Text.Split(new string[] { Global.DELIMITER }, 2, StringSplitOptions.None);
                        if (splitStr.Length > 0)
                            txtReplaceOldValue.Text = splitStr[0];
                        if (splitStr.Length > 1)
                            txtReplaceNewValue.Text = splitStr[1];
                    }
                    else if (cmbModifier.Text == "RegExVarReplace" || cmbModifier.Text == "RegExVarAppend")
                    {
                        regExVarReplace1.Visible = true;
                        regExVarReplace1.Results = txtTextValue.Text;
                    }
                    else if (Global.MultiParamKeyModifiers.Contains(cmbModifier.Text))
                    {
                        pnlMultiValues.Visible = true;
                        txtMultiValue.Text = txtTextValue.Text;
                        txtMultiValue_TextChanged(sender, e);
                    }
                    else
                        switch (cmbModifier.Text)
                        {
                            case "Calc":
                                pnlCalcValue.Visible = true;
                                txtCalcValue.Text = txtTextValue.Text;
                                break;
                            default:
                                switch (_type)
                                {
                                    case ParameterType.Action:
                                        pnlSelectableValue.Visible = true;
                                        PopulateMultiSelect(Global.GetKeyType(cmbField.Text));
                                        cmbTextValue.Text = txtTextValue.Text;
                                        cmbTextValue.AutoCompleteCustomSource.Clear();
                                        break;
                                    default:
                                        pnlTextValue.Visible = true;
                                        break;
                                }
                                break;
                        }
                    break;
                case keyType.List:
                    if (cmbModifier.Text == "Replace" || cmbModifier.Text == "RegexReplace")
                    {
                        pnlReplaceValue.Visible = true;
                        string[] splitStr = txtTextValue.Text.Split(new string[] { Global.DELIMITER }, 2, StringSplitOptions.None);
                        if (splitStr.Length > 0)
                            txtReplaceOldValue.Text = splitStr[0];
                        if (splitStr.Length > 1)
                            txtReplaceNewValue.Text = splitStr[1];
                    }
                    else if (Global.MultiParamKeyModifiers.Contains(cmbModifier.Text))
                    {
                        pnlMultiValues.Visible = true;
                        txtMultiValue.Text = txtTextValue.Text;
                        txtMultiValue_TextChanged(sender, e);
                    }
                    else
                        switch (cmbModifier.Text)
                        {
                            case "Calc":
                                pnlCalcValue.Visible = true;
                                txtCalcValue.Text = txtTextValue.Text;
                                break;
                            default:
                                switch (_type)
                                {
                                    case ParameterType.Action:
                                        pnlSelectableValue.Visible = true;
                                        PopulateMultiSelect(Global.GetKeyType(cmbField.Text));
                                        cmbTextValue.Text = txtTextValue.Text;
                                        cmbTextValue.AutoCompleteCustomSource.Clear();
                                        break;
                                    default:
                                        pnlTextValue.Visible = true;
                                        break;
                                }
                                break;
                        }
                    break;
                case keyType.Numeric:
                case keyType.NumericString:
                    if (cmbModifier.Text.Contains("Range"))
                    {
                        pnlNumericRange.Visible = true;
                        numNumericRangeUpper_ValueChanged(sender, e);
                    }
                    else if (Global.MultiParamKeyModifiers.Contains(cmbModifier.Text))
                    {
                        pnlMultiValues.Visible = true;
                        txtMultiValue.Text = txtTextValue.Text;
                        txtMultiValue_TextChanged(sender, e);
                    }
                    else
                        switch (cmbModifier.Text)
                        {
                            case "Calc":
                                pnlCalcValue.Visible = true;
                                txtCalcValue.ReadOnly = false;
                                txtCalcValue_TextChanged(sender, e);
                                break;
                            case "SetValue":
                                pnlNumericValue.Visible = true;
                                numNumericValue_ValueChanged(numNumericValue, new EventArgs());
                                break;
                            default:
                                switch (cmbField.Text)
                                {
                                    case "CommunityRating":
                                    case "Rating":
                                    case "BookPrice":
                                        pnlNumericValue.Visible = true;
                                        numNumericValue_ValueChanged(sender, e);
                                        break;
                                    default:
                                        switch (_type)
                                        {
                                            case ParameterType.Action:
                                                pnlSelectableValue.Visible = true;
                                                PopulateMultiSelect(Global.GetKeyType(cmbField.Text));
                                                cmbTextValue.Text = txtTextValue.Text;
                                                break;
                                            default:
                                                pnlTextValue.Visible = true;
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                    break;
                case keyType.DateTime:
                    switch (cmbModifier.Text)
                    {
                        case "Range":
                        case "NotRange":
                            pnlDateTimeRange.Visible = true;
                            dtDateTimeRangLower.Value = dtDateTimeRangLower.MinDate;
                            dtDateTimeRangeUpper.Value = dtDateTimeRangeUpper.MinDate;
                            dtDateTimeRangeUpper_ValueChanged(sender, e);
                            break;
                        case "Calc":
                            pnlCalcValue.Visible = true;
                            txtCalcValue.ReadOnly = false;
                            txtCalcValue_TextChanged(sender, e);
                            break;
                        default:
                            pnlDateTimeValue.Visible = true;
                            dtDateTimeValue_ValueChanged(sender, e);

                            break;
                    }
                    break;
                case keyType.LanguageISO:
                    if (Global.MultiParamKeyModifiers.Contains(cmbModifier.Text))
                    {
                        pnlMultiValues.Visible = true;
                        txtMultiValue.Text = txtTextValue.Text;
                        txtMultiValue_TextChanged(sender, e);
                    }
                    else
                        switch (cmbModifier.Text)
                        {
                            default:
                                pnlLimitedValues.Visible = true;
                                cmbLimitedValues.Items.AddRange(Global.GetAcceptedValues(cmbField.Text));
                                if (cmbLimitedValues.Items.Count > 0)
                                    cmbLimitedValues.SelectedIndex = 0;
                                break;
                        }
                    break;
                case keyType.Bool:
                case keyType.YesNo:
                case keyType.MangaYesNo:
                    pnlLimitedValues.Visible = true;
                    cmbLimitedValues.Items.AddRange(Global.GetAcceptedValues(cmbField.Text));
                    if (cmbLimitedValues.Items.Count > 0)
                        cmbLimitedValues.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        public void SetDataManagerParameter(dmParameters dmpItem)
        {
            //if (dmpItem.Type != this.ParameterTypeRestriction)
            //{
            //    throw new Exception(string.Format("Expected a '{0}', but recieved a '{1}'", Enum.GetName(typeof(ParameterType), ParameterTypeRestriction), Enum.GetName(typeof(ParameterType), dmpItem.Type)));
            //}
            //else
            SetItem(dmpItem);
        }

        private void SetItem(dmParameters dmpItem)
        {
            if (cmbField.Items.Contains(dmpItem.Field))
                cmbField.SelectedItem = dmpItem.Field;
            else
            {
                cmbField.SelectedItem = null;
                cmbField.Text = dmpItem.Field;
            }

            cmbModifier.SelectedItem = dmpItem.Modifier;

            switch (_type)
            {
                case ParameterType.Action:
                    SetActionValue(dmpItem.Value);
                    break;
                case ParameterType.Rule:
                    SetRuleValue(dmpItem.Value);
                    break;
            }
        }

        private void SetActionValue(string strValue)
        {
            string[] tmp = strValue.Split(new string[] { Global.DELIMITER }, StringSplitOptions.None);

            switch (Global.GetKeyType(cmbField.Text))
            {
                #region String Values
                case keyType.Custom:
                case keyType.String:
                    switch (cmbModifier.Text)
                    {
                        case "RegexReplace":
                        case "Replace":
                            if (tmp.Length > 0)
                                txtReplaceOldValue.Text = tmp[0];
                            if (tmp.Length > 1)
                                txtReplaceNewValue.Text = tmp[1];
                            break;
                        case "RegExVarReplace":
                        case "RegExVarAppend":
                            regExVarReplace1.Results = strValue;
                            break;
                        case "Calc":
                            txtCalcValue.Text = strValue;
                            break;
                        default:
                            cmbTextValue.Text = strValue;
                            break;
                    }
                    break;
                #endregion
                #region List Values
                case keyType.List:
                    switch (cmbModifier.Text)
                    {
                        case "Add":
                            txtMultiValue.Text = tmp.Join(Global.DELIMITER);
                            break;
                        case "RegexReplace":
                        case "Replace":
                            {
                                if (tmp.Length > 0)
                                    txtReplaceOldValue.Text = tmp[0];
                                if (tmp.Length > 1)
                                    txtReplaceNewValue.Text = tmp[1];
                            }
                            break;
                        default:
                            cmbTextValue.Text = strValue;
                            break;
                    }
                    break;
                #endregion
                #region Numeric
                case keyType.Numeric:
                case keyType.NumericString:
                    switch (cmbModifier.Text)
                    {
                        case "Calc":
                            txtCalcValue.Text = strValue;
                            break;
                        default:
                            switch (cmbField.Text)
                            {
                                case "CommunityRating":
                                case "Rating":
                                case "BookPrice":
                                    float val;
                                    if (!float.TryParse(strValue, out val))
                                        numNumericValue.Value = new decimal(0);
                                    else
                                        numNumericValue.Value = new decimal(val);
                                    break;
                                default:
                                    if (int.TryParse(strValue, out int val2))
                                        numNumericValue.Value = val2;
                                    cmbTextValue.Text = strValue;
                                    break;
                            }
                            break;
                    }
                    break;
                #endregion
                #region DateTime
                case keyType.DateTime:
                    switch (cmbModifier.Text)
                    {
                        case "Calc":
                            txtCalcValue.Text = strValue;
                            break;
                        default:
                            if (!string.IsNullOrEmpty(strValue))
                            {
                                DateTime tmpDate;
                                if (!DateTime.TryParse(strValue, out tmpDate))
                                    dtDateTimeValue.Value = DBNull.Value;
                                else
                                {
                                    dtDateTimeValue.Checked = true;
                                    dtDateTimeValue.Value = tmpDate;
                                }
                            }
                            else
                                dtDateTimeValue.Value = DBNull.Value;
                            break;
                    }
                    break;
                #endregion
                #region LimitedValues
                case keyType.LanguageISO:
                    cmbLimitedValues.SelectedItem = Global.LanguageISOs.First(n => n.Value == strValue).Key;
                    break;
                case keyType.Bool:
                case keyType.YesNo:
                case keyType.MangaYesNo:
                    cmbLimitedValues.SelectedItem = strValue;
                    break;
                #endregion
                default:
                    txtTextValue.Text = strValue;
                    break;
            }
        }

        private void SetRuleValue(string p)
        {
            if (cmbField.SelectedIndex < 0) return;
            keyType FieldType = Global.GetKeyType(cmbField.Text);

            string[] tmp = p.Split(new string[] { Global.DELIMITER }, StringSplitOptions.None);

            switch (FieldType)
            {
                case keyType.Custom:
                    txtTextValue.Text = p;
                    break;
                case keyType.String:
                    switch (cmbModifier.Text)
                    {
                        case "IsAnyOf":
                        case "NotIsAnyOf":
                        case "StartsWithAnyOf":
                        case "NotStartsWithAnyOf":
                        case "ContainsAnyOf":
                        case "NotContainsAnyOf":
                        case "ContainsAllOf":
                            txtMultiValue.Text = p;
                            break;
                        case "RegexReplace":
                        case "Replace":
                            string[] strSplit = p.Split(new string[] { Global.DELIMITER }, 2, StringSplitOptions.None);
                            if (strSplit.Length > 0)
                                txtReplaceOldValue.Text = strSplit[0];
                            if (strSplit.Length > 1)
                                txtReplaceOldValue.Text = strSplit[1];
                            break;
                        default:
                            switch (_type)
                            {
                                case ParameterType.Action:
                                    cmbTextValue.Text = p;
                                    break;
                                default:
                                    txtTextValue.Text = p;
                                    break;
                            }
                            break;
                    }
                    break;
                case keyType.List:
                    switch (cmbModifier.Text)
                    {
                        case "Add":
                        case "ContainsAnyOf":
                        case "NotContainsAnyOf":
                        case "ContainsAllOf":
                            txtMultiValue.Text = p;
                            break;
                        case "RegexReplace":
                        case "Replace":
                            string[] strSplit = p.Split(new string[] { Global.DELIMITER }, 2, StringSplitOptions.None);
                            if (strSplit.Length > 0)
                                txtReplaceOldValue.Text = strSplit[0];
                            if (strSplit.Length > 1)
                                txtReplaceOldValue.Text = strSplit[1];
                            break;
                        default:
                            switch (_type)
                            {
                                case ParameterType.Action:
                                    cmbTextValue.Text = p;
                                    break;
                                default:
                                    txtTextValue.Text = p;
                                    break;
                            }
                            break;
                    }
                    break;
                case keyType.Numeric:
                case keyType.NumericString:
                    switch (cmbModifier.Text)
                    {
                        case "Range":
                        case "NotRange":
                            float tmp1;
                            float tmp2;
                            if (tmp.Length > 0)
                            {
                                if (!float.TryParse(tmp[0], out tmp1))
                                    tmp1 = 0f;

                                if (tmp.Length > 1)
                                {
                                    if (!float.TryParse(tmp[1], out tmp2))
                                        tmp2 = 1f;
                                }
                                else
                                    tmp2 = 1f;
                            }
                            else
                            {
                                tmp1 = 0f;
                                tmp2 = 1f;
                            }
                            numNumericRangeLower.Value = new decimal(tmp1);
                            numNumericRangeUpper.Value = new decimal(tmp2);
                            break;
                        case "IsAnyOf":
                        case "NotIsAnyOf":
                            txtMultiValue.Text = p;
                            break;
                        default:
                            switch (cmbField.Text)
                            {
                                case "CommunityRating":
                                case "Rating":
                                case "BookPrice":
                                    float tmpFloat;
                                    if (!float.TryParse(p, out tmpFloat))
                                        tmpFloat = 0f;
                                    numNumericValue.Value = new decimal(tmpFloat);
                                    break;
                                default:
                                    switch (_type)
                                    {
                                        case ParameterType.Action:
                                            cmbTextValue.Text = p;
                                            break;
                                        default:
                                            txtTextValue.Text = p;
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                case keyType.DateTime:
                    switch (cmbModifier.Text)
                    {

                        case "Range":
                            DateTime dt1;
                            DateTime dt2;
                            if (tmp.Length > 0)
                            {
                                if (!DateTime.TryParse(tmp[0], out dt1))
                                    dt1 = dtDateTimeRangLower.MinDate;


                                if (tmp.Length > 1)
                                {
                                    if (!DateTime.TryParse(tmp[1], out dt2))
                                        dt2 = dtDateTimeRangeUpper.MinDate;
                                }
                                else
                                    dt2 = dtDateTimeRangeUpper.MinDate;
                            }
                            else
                            {
                                dt1 = dtDateTimeRangLower.MinDate;
                                dt2 = dtDateTimeRangeUpper.MinDate;
                            }

                            dtDateTimeRangLower.Value = dt1;
                            dtDateTimeRangeUpper.Value = dt2;
                            break;
                        default:
                            if (tmp.Length == 1 && !string.IsNullOrEmpty(tmp[0]))
                            {
                                dtDateTimeValue.Checked = true;
                                if (DateTime.TryParse(tmp[0], out DateTime dt))
                                    dtDateTimeValue.Value = dt;
                            }
                            else
                                dtDateTimeValue.Value = DBNull.Value;
                            break;
                    }
                    break;
                case keyType.LanguageISO:
                    switch (cmbModifier.Text)
                    {
                        case "IsAnyOf":
                        case "NotIsAnyOf":
                            txtMultiValue.Text = p;
                            break;
                        default:
                            cmbLimitedValues.SelectedItem = Global.LanguageISOs.First(n => n.Value == p).Key;
                            break;
                    }
                    break;
                case keyType.YesNo:
                case keyType.MangaYesNo:
                    cmbLimitedValues.SelectedItem = p;
                    break;
                default:
                    break;
            }
        }
        private void PopulateMultiSelect(keyType type)
        {
            cmbTextValue.Items.Clear();
            switch (type)
            {
                case keyType.Custom:
                case keyType.String:
                case keyType.List:
                    cmbTextValue.DropDownStyle = ComboBoxStyle.DropDown;
                    for (int i = 0; i < Global.AllowedKeys.Length; i++)
                    {
                        if (!Global.AllowedKeys[i].Equals("Custom"))
                        {
                            cmbTextValue.Items.Add("{" + Global.AllowedKeys[i] + "}");
                        }
                    }
                    break;
                case keyType.Numeric:
                    cmbTextValue.DropDownStyle = ComboBoxStyle.DropDown;
                    List<string> tmpStr = new List<string>(Global.NumericKeys);
                    tmpStr.AddRange(Global.PsuedoNumericKeys);
                    tmpStr.Sort();
                    for (int i = 0; i < tmpStr.Count; i++)
                        cmbTextValue.Items.Add("{" + tmpStr[i] + "}");
                    break;
                case keyType.DateTime:
                    cmbTextValue.DropDownStyle = ComboBoxStyle.DropDownList;
                    for (int i = 0; i < Global.DateTimeKeys.Length; i++)
                        cmbTextValue.Items.Add("{" + Global.DateTimeKeys[i] + "}");
                    break;
                default:
                    break;
            }

        }
        public dmParameters GetDataManagerParameters()
        {
            string tmpString = string.Format("<<{0}.{1}:{2}>>", new string[] { cmbField.Text, cmbModifier.Text, txtTextValue.Text });
            keyType tmpKeyType = Global.GetKeyType(cmbField.Text);

            if (this.ParameterTypeRestriction == ParameterType.Rule)
                return new dmRule(tmpString);
            else
                return new dmAction(tmpString);
        }

        private void pnlNumericValue_SizeChanged(object sender, EventArgs e)
        {
            numNumericValue.Size = new Size(pnlNumericValue.Width - 3, 20);
            numNumericValue.Location = new Point(3, 3);
        }

        private void cmbLimitedValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLimitedValues.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.LanguageISO
                || Global.GetKeyType(cmbField.Text) == keyType.YesNo
                || Global.GetKeyType(cmbField.Text) == keyType.MangaYesNo
                || Global.GetKeyType(cmbField.Text) == keyType.Bool)
                switch (Global.GetKeyType(cmbField.Text))
                {
                    case keyType.LanguageISO:
                        switch (cmbModifier.Text)
                        {
                            case "Is":
                            case "Not":
                            case "SetValue":
                                txtTextValue.Text = Global.LanguageISOs[cmbLimitedValues.Text];
                                break;
                        }
                        break;
                    case keyType.Bool:
                    case keyType.YesNo:
                    case keyType.MangaYesNo:
                        txtTextValue.Text = cmbLimitedValues.Text;
                        break;
                    default:
                        break;
                }
        }

        private void txtReplaceOldValue_TextChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            keyType tmpKey = Global.GetKeyType(cmbField.Text);
            if ((tmpKey == keyType.List || tmpKey == keyType.String || tmpKey == keyType.Custom) && cmbModifier.Text.Contains("Replace"))
                txtTextValue.Text = txtReplaceOldValue.Text + Global.DELIMITER + txtReplaceNewValue.Text;
        }

        private void txtReplaceNewValue_TextChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            keyType tmpKey = Global.GetKeyType(cmbField.Text);
            if ((tmpKey == keyType.List || tmpKey == keyType.String || tmpKey == keyType.Custom) && cmbModifier.Text.Contains("Replace"))
                txtTextValue.Text = txtReplaceOldValue.Text + Global.DELIMITER + txtReplaceNewValue.Text;
        }

        private void dtDateTimeValue_ValueChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.DateTime && cmbModifier.Text != "Range")

                if (dtDateTimeValue.Value != DBNull.Value) txtTextValue.Text = ((DateTime)dtDateTimeValue.Value).ToString("yyyy/MM/dd");
                else txtTextValue.Text = "";
        }

        private void dtDateTimeRangLower_ValueChanged(object sender, EventArgs e)
        {
            dtDateTimeRangeUpper.MinDate = ((DateTime)dtDateTimeRangLower.Value).AddDays(1);
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.DateTime && cmbModifier.Text.Contains("Range"))
                txtTextValue.Text = ((DateTime)dtDateTimeRangLower.Value).ToString("yyyy/MM/dd") + Global.DELIMITER + ((DateTime)dtDateTimeRangeUpper.Value).ToString("yyyy/MM/dd");
        }

        private void dtDateTimeRangeUpper_ValueChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.DateTime && cmbModifier.Text.Contains("Range"))
                txtTextValue.Text = ((DateTime)dtDateTimeRangLower.Value).ToString("yyyy/MM/dd") + Global.DELIMITER + ((DateTime)dtDateTimeRangeUpper.Value).ToString("yyyy/MM/dd");
        }

        private void numNumericRangeLower_ValueChanged(object sender, EventArgs e)
        {
            numNumericRangeUpper.Minimum = numNumericRangeLower.Value + new decimal(0.01f);

            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.Numeric && cmbModifier.Text.Contains("Range"))
                txtTextValue.Text = numNumericRangeLower.Value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + Global.DELIMITER + numNumericRangeUpper.Value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        private void numNumericRangeUpper_ValueChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.Numeric && cmbModifier.Text.Contains("Range"))
                txtTextValue.Text = numNumericRangeLower.Value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + Global.DELIMITER + numNumericRangeUpper.Value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        private void txtCalcValue_TextChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (cmbModifier.Text == "Calc")
                txtTextValue.Text = txtCalcValue.Text;
        }

        private void numNumericValue_ValueChanged(object sender, EventArgs e)
        {
            if (cmbField.SelectedIndex < 0 || cmbModifier.SelectedIndex < 0) return;

            if (Global.GetKeyType(cmbField.Text) == keyType.Numeric)
            {
                switch (cmbModifier.Text)
                {
                    case "Is":
                    case "Not":
                    case "Greater":
                    case "GreaterEq":
                    case "Less":
                    case "LessEq":
                    case "SetValue":
                        txtTextValue.Text = numNumericValue.Value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        break;
                }
            }
        }

        private void pnlCalcValue_SizeChanged(object sender, EventArgs e)
        {
            txtCalcValue.Size = new Size(pnlCalcValue.Width - 6, 20);
            txtCalcValue.Location = new Point(3, 3);
        }

        private void cmsCalcMenu_Opening(object sender, CancelEventArgs e)
        {
            txtBeingEdited = (TextBox)contextMenuStrip1.SourceControl;
            tsmiAddFieldValue.DropDownItems.Clear();
            List<string> tmpStrings = new List<string>();
            switch (Global.GetKeyType(cmbField.Text))
            {
                case keyType.List:
                case keyType.String:
                case keyType.Custom:
                case keyType.NumericString:
                    tsmiCopy.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiCut.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiDelete.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiPaste.Enabled = Clipboard.ContainsText();
                    //we can use all acceptable fields in string
                    tmpStrings = new List<string>(Global.AllowedKeys);

                    for (int i = 0; i < tmpStrings.Count; i++)
                    {
                        if (!tmpStrings[i].Equals("Custom"))
                        {
                            ToolStripMenuItem tmp = new ToolStripMenuItem(tmpStrings[i]);
                            tmp.Click += new EventHandler(MenuItemCalcOther_Click);

                            tsmiAddFieldValue.DropDownItems.Add(tmp);
                        }
                    }
                    break;
                case keyType.Numeric:
                    tsmiCopy.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiCut.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiDelete.Enabled = txtBeingEdited.SelectionLength > 0;
                    tsmiPaste.Enabled = Clipboard.ContainsText();
                    //only allow numeric items
                    tmpStrings = new List<string>(Global.NumericKeys);
                    tmpStrings.AddRange(Global.PsuedoNumericKeys);
                    tmpStrings.Sort();
                    for (int i = 0; i < tmpStrings.Count; i++)
                    {
                        ToolStripMenuItem tmp = new ToolStripMenuItem(tmpStrings[i]);
                        tmp.Click += new EventHandler(MenuItemCalcOther_Click);

                        tsmiAddFieldValue.DropDownItems.Add(tmp);
                    }
                    break;
                case keyType.DateTime:
                    tsmiCut.Enabled = false;
                    tsmiCopy.Enabled = false;
                    tsmiPaste.Enabled = false;
                    tsmiDelete.Enabled = !string.IsNullOrEmpty(txtBeingEdited.Text);
                    tmpStrings = new List<string>(Global.DateTimeKeys);
                    tmpStrings.Remove(cmbField.Text);
                    for (int i = 0; i < tmpStrings.Count; i++)
                    {
                        ToolStripMenuItem tmp = new ToolStripMenuItem(tmpStrings[i]);
                        tmp.Click += new EventHandler(MenuItemCalcOther_Click);

                        tsmiAddFieldValue.DropDownItems.Add(tmp);
                    }
                    break;
                default:

                    break;
            }
        }

        private void MenuItemCalcOther_Click(object sender, EventArgs e)
        {
            if (txtBeingEdited != null)
            {
                TextBox tmpText = txtBeingEdited;
                int selectStart = tmpText.SelectionStart;
                int selectLegnth = tmpText.SelectionLength;
                if (selectLegnth > 0)
                    tmpText.Text = tmpText.Text.Remove(selectStart, selectLegnth);
                if (((ToolStripMenuItem)sender).Text != "Custom")
                    tmpText.Text = tmpText.Text.Insert(selectStart, "{" + ((ToolStripMenuItem)sender).Text + "}");
            }
            txtBeingEdited = null;
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (txtCalcValue.SelectionLength < 1) return;
            if (txtBeingEdited != null)
            {
                txtBeingEdited.Copy();
            }
            txtBeingEdited = null;
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            if (txtCalcValue.SelectionLength < 1) return;
            if (txtBeingEdited != null)
            {
                txtBeingEdited.Cut();
            }
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsText()) return;
            if (txtBeingEdited != null)
            {
                txtCalcValue.Paste();
            }
            txtBeingEdited = null;
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            if (txtBeingEdited != null)
            {
                int selectionStart = txtBeingEdited.SelectionStart;
                int selctionLength = txtBeingEdited.SelectionLength;
                txtBeingEdited.Text = txtBeingEdited.Text.Remove(selectionStart, selctionLength);
            }
            txtBeingEdited = null;
        }

        private void cmbTextValue_TextChanged(object sender, EventArgs e)
        {
            if ((cmbField.SelectedIndex > -1 && (Global.GetKeyType(cmbField.Text) == keyType.String || Global.GetKeyType(cmbField.Text) == keyType.List)
                || Global.GetKeyType(cmbField.Text) == keyType.Custom
                || (Global.GetKeyType(cmbField.Text) == keyType.Numeric && (cmbField.Text != "BookPrice" && cmbField.Text != "Rating" && cmbField.Text != "CommunityRating"))))
                txtTextValue.Text = cmbTextValue.Text;
        }

        private void regExVarReplace1_RegularExpressionChanged(object sender, EventArgs e)
        {
            txtTextValue.Text = regExVarReplace1.Results;
        }

        private void cmbField_Validated(object sender, EventArgs e)
        {
            if (Global.GetKeyType(cmbField.Text) == keyType.Custom && cmbField.Text != "")
                cmbField_SelectedIndexChanged(cmbField, new EventArgs());
        }

    }
}
