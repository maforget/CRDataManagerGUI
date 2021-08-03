using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace DataManagerGUI
{
    public enum ParameterType
    {
        Rule,
        Action
    }

    [Serializable]
    public abstract class dmParameters : IEquatable<dmParameters>
    {
        public bool UsesCustomField
        {
            get
            {
                return !Global.AllowedKeys.Contains(this.Field);
            }
        }

        private string _field;

        public string Field
        {
            get
            {
                return this._field;
            }
            set
            {
                if (Regex.Match(value, "Custom\\(([^)]+)\\)").Success)
                    _field = Regex.Match(value, "Custom\\(([^)]+)\\)").Groups[1].Value;
                else
                    _field = value;
            }
        }
        public string Modifier { get; set; }
        public string Value { get; set; }
        public ParameterType Type { get; private set; }
        public string CustomField { get; set; }
        public string Compile()
        {
            return ToString();
        }

        public override string ToString()
        {
            return ToXML(Type.ToString().ToLowerInvariant()).ToString();
        }

        public dmParameters(ParameterType ptType, string strCompleteField, string strModifier, string strValue)
        {
            this.Type = ptType;
            this.Field = strCompleteField;
            this.Modifier = strModifier;
            this.Value = strValue;
        }
        public dmParameters(ParameterType ptType, string strDataManagerParameters)
            : this(ptType)
        {
            Parse(strDataManagerParameters);
        }
        public dmParameters(ParameterType ptType, string strDataManagerParameters, Version version)
            : this(ptType)
        {
            Parse(strDataManagerParameters, version);
        }
        public dmParameters(ParameterType ptType)
        {
            this.Type = ptType;
            this.CustomField = "";
        }

        internal dmParameters(string[] strParameters)
            : this((ParameterType)Enum.Parse(typeof(ParameterType), strParameters[0]))
        {
            Parse(strParameters[1]);
        }

        public dmParameters(ParameterType parameterType, XElement item)
        {
            this.Type = parameterType;
            FromXML(item);
        }

        public void Parse(string strDataManagerParameters)
        {
            try
            {
                XElement xParameters = XElement.Parse(strDataManagerParameters);
                FromXML(xParameters);
            }
            catch (Exception)
            {
                //remove the <<
                string cleanString = strDataManagerParameters.Trim('<', '>');

                string[] CriteriaAndTestValue = cleanString.Split(new char[] { ':' }, 2);
                string[] CriteriaAndModifier = CriteriaAndTestValue[0].Split(new char[] { '.' }, 2);

                this.Field = CriteriaAndModifier[0];


                if (CriteriaAndModifier.Length < 2)
                {
                    if (this.Type == ParameterType.Action)
                        this.Modifier = "SetValue";
                    else
                        this.Modifier = "Is";
                }
                else
                    this.Modifier = CriteriaAndModifier[1];

                this.Value = CriteriaAndTestValue[1];
            }
        }
        public void Parse(string strDataManagerParameters, Version version)
        {
            //remove the <<
            string cleanString = strDataManagerParameters.Trim(new char[] { '<', '>' });

            string[] CriteriaAndTestValue = cleanString.Split(new char[] { ':' }, 2);
            string[] CriteriaAndModifier = CriteriaAndTestValue[0].Split(new char[] { '.' }, 2);

            this.Field = CriteriaAndModifier[0];


            if (CriteriaAndModifier.Length < 2)
            {
                if (this.Type == ParameterType.Action)
                    this.Modifier = "SetValue";
                else
                    this.Modifier = "Is";
            }
            else
                this.Modifier = CriteriaAndModifier[1];

            this.Value = CriteriaAndTestValue[1];
            if (version < new Version(Global.MultiModUpdateVersion) && Global.MultiParamKeyModifiers.Contains(Modifier))
            {
                this.Value = this.Value.Replace(",", Global.DELIMITER);
            }
        }
        public abstract dmParameters Clone();

        public bool Equals(dmParameters other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return (this.Field + this.Modifier + this.Value).GetHashCode();
        }

        public void Copy(dmParameters other)
        {
            this.Field = other.Field;
            this.Value = other.Value;
            this.Modifier = other.Modifier;
        }

        public string TranslateToPlainEnglish()
        {
            string strReturn = "";
            string[] tmpStrings = new string[3];
            string[] grrrr = Value.Split(new char[] { ',' });
            for (int i = 0; i < grrrr.Length && i < 2; i++)
            {
                tmpStrings[i] = grrrr[i];
            }


            switch (this.Type)
            {
                case ParameterType.Rule:
                    strReturn = this.Field;
                    switch (Modifier)
                    {
                        case "Is":
                            strReturn += ": ";
                            break;
                        case "IsAnyOf":
                            strReturn += " matches exactly and of the following: ";
                            break;
                        case "Not":
                            strReturn += " does not match exactly: ";
                            break;
                        case "NotIsAnyOf":
                            strReturn += " does not match any: ";
                            break;
                        case "Contains":
                            strReturn += " contains the phrase: ";
                            break;
                        case "Greater":
                            strReturn += " is greater than ";
                            break;
                        case "GreaterEq":
                            strReturn += " is greater than or equal to ";
                            break;
                        case "Less":
                            strReturn += " is less than ";
                            break;
                        case "LessEq":
                            strReturn += " is less than or equal to ";
                            break;
                        case "StartsWith":
                            strReturn += " begins with the phrase:";
                            break;
                        case "NotStartsWith":
                            strReturn += " does not begin with phrase: ";
                            break;
                        case "StartsWithAnyOf":
                            strReturn += " begins with any of the phrases: ";
                            break;
                        case "NotStartsWithAnyOf":
                            strReturn += " does not begin with any of the phrases: ";
                            break;
                        case "ContainsAnyOf":
                            strReturn += " contains any of the phrases: ";
                            break;
                        case "NotContainsAnyOf":
                            strReturn += " does not contain any of the phrases: ";
                            break;
                        case "NotContains":
                            strReturn += "does not contain the phrase: ";
                            break;
                        case "ContainsAllOf":
                            strReturn += "contains all of the phrases: ";
                            break;

                        default:
                            strReturn += ": ";
                            break;
                    }
                    strReturn += Value;
                    break;
                case ParameterType.Action:
                    switch (Modifier)
                    {

                        case "SetValue":
                            strReturn = string.Format("Set value of \"{0}\" to \"{1}\"", Field, Value);
                            break;
                        case "Calc":
                            strReturn = string.Format("Calculate value of \"{0}\" using the following statement: \"{1}\"", Field, Value);
                            break;
                        case "Add":
                            strReturn = string.Format("Add the phrase \"{0}\" to \"{1}\"", Value, Field);
                            break;
                        case "Remove":
                            strReturn = string.Format("Remove the phrase \"{0}\" from \"{1}\"", Value, Field);
                            break;
                        case "Replace":
                            tmpStrings[2] = Field;
                            strReturn = string.Format("Replace the phrase \"{0}\" with \"{1}\" in \"{2}\"", tmpStrings);
                            break;
                        case "RemoveLeading":
                            strReturn = string.Format("Remove the leading phrase \"{0}\" from \"{1}\"", Value, Field);
                            break;
                    }
                    break;
                default:
                    break;
            }

            return strReturn;
        }

        public XElement ToXML(string xElementName)
        {
            XElement xReturn = new XElement(xElementName);
            xReturn.Add(new XAttribute("field", this.Field));
            xReturn.Add(new XAttribute("modifier", this.Modifier));
            xReturn.Add(new XAttribute("value", this.Value));
            return xReturn;
        }

        public void FromXML(XElement xParameters)
        {
            this.Field = xParameters.Attribute("field").Value;
            this.Modifier = xParameters.Attribute("modifier").Value;
            this.Value = xParameters.Attribute("value").Value;
        }
    }

    public class dmRule : dmParameters, IEquatable<dmRule>
    {

        public dmRule(XElement xParameters)
            : base(ParameterType.Rule, xParameters)
        {
        }

        public dmRule(string sRuleToParse)
            : base(ParameterType.Rule, sRuleToParse)
        {

        }

        
        public dmRule(string strParameters, Version version)
            :base(ParameterType.Rule, strParameters, version)
        { }

        public override dmParameters Clone()
        {
            return new dmRule(this.ToString());         
        }

        public bool Equals(dmRule other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

    }

    public class dmAction : dmParameters
    {
        public dmAction(XElement xParameters)
            : base(ParameterType.Action, xParameters)
        { }

        public dmAction(string strParameters)
            : base(ParameterType.Action, strParameters)
        { }

        public dmAction(string strParameters, Version version)
            : base(ParameterType.Action, strParameters, version)
        { }

        public bool Equals(dmAction other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }

        public override dmParameters Clone()
        {
            return new dmAction(this.ToString());
        }
    }
}


