using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataManagerGUI
{
    public static class Global
    {
        public const string Divider = "# -------------------------------";
        public const string GroupHeader = "#@ GROUP ";
        public const string EndOfGroup = "#@ END_GROUP";
        public const string RulesetNameHeader = "#@ NAME ";
        public const string CommentHeader = "@ COMMENT ";
        public const string VariableHeader = "#@ VAR ";
        public const string EndOfRules = "#@ END_RULES";
        public const string AuthorHeader = "#@ AUTHOR ";
        public const string VersionHeader = "#@ VERSION ";
        public const string NotesHeader = "#@ NOTES";
        public const string EndOfNotes = "#@ END_NOTES";
        public const string InvalidRuleset = "#@ Invalid Ruleset ";
        public const string MultiModUpdateVersion = "1.2.0.0";
        public static string GroupFilterAndDefaults = "@ FILTERSANDDEFAULTS ";
        public static SortedDictionary<string, string> LanguageISOs = new SortedDictionary<string, string>();

        //from dataman.ini

        public static string CurrentVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();
        public static string DELIMITER = "||";
        public static string[] AllowedKeys = new string[0];
        public static string[] AllowedValues = new string[0];
        public static string[] ReadOnlyKeys = new string[0];

        public static string[] StringKeys = new string[0];
        public static string[] StringKeysModifiers = new string[0];
        public static string[] StringValueModifiers = new string[0];


        public static string[] NumericKeys = new string[0];
        public static string[] NumericalKeysModifiers = new string[0];
        public static string[] NumericValueModifiers = new string[0];


        public static string[] PsuedoNumericKeys = new string[0];

        public static string[] ListKeys = new string[0];
        public static string[] ListKeyModifiers = new string[0];
        public static string[] ListValueModifiers = new string[0];

        public static string[] YesNoKeys = new string[0];
        public static string[] YesNoKeyModifiers = new string[0];
        public static string[] YesNoValueModifiers = new string[0];

        public static string[] MangaYesNoKeys = new string[0];
        public static string[] MangaYesNoKeyModifiers = new string[0];
        public static string[] MangaYesNoValueModifiers = new string[0];


        public static string[] LanguageISOKeys = new string[0];
        public static string[] LanguageISOKeyModifiers = new string[0];
        public static string[] LanguageISOValueModifiers = new string[0];

        public static string[] DateTimeKeys = new string[0];
        public static string[] DateTimeKeyModifiers = new string[0];
        public static string[] DateTimeValueModifiers = new string[0];

        public static string[] BoolKeys = new string[0];
        public static string[] BoolKeyModifiers = new string[0];
        public static string[] BoolValueModifiers = new string[0];

        public static string[] CustomKeyModifiers = new string[0];
        public static string[] CustomValueModifiers = new string[0];

        public static string[] AllowedKeyModifiers = new string[0];
        public static string[] AllowedValueModifiers = new string[0];
        public static string[] MultiParamKeyModifiers = new string[] { "IsAnyOf", "NotIsAnyOf", "StartsWithAnyOf", "NotStartsWithAnyOf", "ContainsAnyOf", "NotContainsAnyOf", "ContainsAllOf", "NotContainsAllOf", "Range", "Add", "Replace", "RegexReplace" };
        public static string[] AllowedRegExVarReplace = new string[0];

        private static string[] GetAllowedKeyModifiers(keyType FieldType)
        {
            List<string> arrAllowedModifiers = new List<string>();
            switch (FieldType)
            {
                case keyType.Custom:
                case keyType.String:
                    arrAllowedModifiers = new List<string>(StringKeysModifiers);
                    break;
                case keyType.Bool:
                    arrAllowedModifiers = new List<string>(BoolKeyModifiers);
                    break;
                case keyType.Numeric:
                case keyType.NumericString:
                    arrAllowedModifiers = new List<string>(NumericalKeysModifiers);
                    break;
                case keyType.List:
                    arrAllowedModifiers = new List<string>(ListKeyModifiers);
                    break;
                case keyType.LanguageISO:
                    arrAllowedModifiers = new List<string>(LanguageISOKeyModifiers);
                    break;
                case keyType.YesNo:
                    arrAllowedModifiers = new List<string>(YesNoKeyModifiers);
                    break;
                case keyType.MangaYesNo:
                    arrAllowedModifiers = new List<string>(MangaYesNoKeyModifiers);
                    break;
                case keyType.DateTime:
                    arrAllowedModifiers = new List<string>(DateTimeKeyModifiers);
                    break;
                default:
                    arrAllowedModifiers = new List<string>();
                    break;
            }
            return arrAllowedModifiers.ToArray();
        }
        public static string[] GetAllowedKeyModifiers(string strAllowedKey)
        {
            return GetAllowedKeyModifiers(GetKeyType(strAllowedKey));
        }


        private static string[] GetAllowedValueModifiers(keyType FieldType)
        {
            List<string> arrAllowedModifiers = new List<string>();
            switch (FieldType)
            {
                case keyType.Custom:
                case keyType.String:
                    arrAllowedModifiers = new List<string>(StringValueModifiers);
                    break;
                case keyType.Bool:
                    arrAllowedModifiers = new List<string>(BoolValueModifiers);
                    break;
                case keyType.Numeric:
                case keyType.NumericString:
                    arrAllowedModifiers = new List<string>(NumericValueModifiers);
                    break;
                case keyType.List:
                    arrAllowedModifiers = new List<string>(ListValueModifiers);
                    break;
                case keyType.LanguageISO:
                    arrAllowedModifiers = new List<string>(LanguageISOValueModifiers);
                    break;
                case keyType.YesNo:
                    arrAllowedModifiers = new List<string>(YesNoValueModifiers);
                    break;
                case keyType.MangaYesNo:
                    arrAllowedModifiers = new List<string>(MangaYesNoValueModifiers);
                    break;
                case keyType.DateTime:
                    arrAllowedModifiers = new List<string>(DateTimeValueModifiers);
                    break;
                default:
                    break;
            }
            return arrAllowedModifiers.ToArray();
        }
        public static string[] GetAllowedValueModifiers(string strAllowedKey)
        {
            List<string> AllowedModifiers = !ReadOnlyKeys.Contains(strAllowedKey) ? new List<string>(GetAllowedValueModifiers(GetKeyType(strAllowedKey))) : new List<string>();
            return AllowedModifiers.ToArray();
        }

        private static string[] GetAcceptedValues(keyType ktType)
        {
            List<string> arrAllowedValues = new List<string>();
            switch (ktType)
            {
                case keyType.Bool:
                    arrAllowedValues = new List<string>(new string[] { "True", "False" });
                    break;
                case keyType.LanguageISO:
                    arrAllowedValues = new List<string>(LanguageISOs.Keys.ToArray());
                    break;
                case keyType.YesNo:
                    arrAllowedValues = new List<string>(new string[] { "", "Yes", "No" });
                    break;
                case keyType.MangaYesNo:
                    arrAllowedValues = new List<string>(new string[] { "", "Yes", "No", "YesAndRightToLeft" });
                    break;
                default:
                    break;
            }
            return arrAllowedValues.ToArray();
        }
        public static string[] GetAcceptedValues(string strAllowedKey)
        {
            return GetAcceptedValues(GetKeyType(strAllowedKey));
        }

        public static string[] GetAcceptedActionFields()
        {
            List<string> fieldsReturn = new List<string>(AllowedValues);

            foreach (string item in AllowedRegExVarReplace)
            {
                if (!fieldsReturn.Contains(item))
                {
                    fieldsReturn.Add(item);
                }
            }


            if (fieldsReturn.Contains("HasBeenOpened"))
                fieldsReturn.Remove("HasBeenOpened");

            if (fieldsReturn.Contains("HasBeenRead"))
                fieldsReturn.Remove("HasBeenRead");


            fieldsReturn.Sort();
            return fieldsReturn.ToArray();
        }

        public static string[] GetAllModifiers()
        {
            List<string> allModifiers = new List<string>(AllowedKeyModifiers);

            foreach (string item in AllowedValueModifiers)
            {
                if (!allModifiers.Contains(item))
                    allModifiers.Add(item);
            }

            foreach (string item in ListKeyModifiers)
            {
                if (!allModifiers.Contains(item))
                    allModifiers.Add(item);
            }

            foreach (string item in ListValueModifiers)
            {
                if (!allModifiers.Contains(item))
                    allModifiers.Add(item);
            }

            foreach (string item in NumericalKeysModifiers)
            {
                if (!allModifiers.Contains(item))
                    allModifiers.Add(item);
            }

            foreach (string item in NumericValueModifiers)
            {
                if (!allModifiers.Contains(item))
                    allModifiers.Add(item);
            }

            allModifiers.Sort();
            return allModifiers.ToArray();
        }

        public static string[] GetAllFields()
        {
            List<string> allFields = new List<string>(AllowedKeys);

            foreach (string item in AllowedValues)
            {
                if (!allFields.Contains(item))
                    allFields.Add(item);
            }

            allFields.Sort();

            return allFields.ToArray();
        }

        public static keyType GetKeyType(string strAllowedKey)
        {
            if (NumericKeys.Contains(strAllowedKey))
                return keyType.Numeric;
            else if (BoolKeys.Contains(strAllowedKey))
                return keyType.Bool;
            else if (ListKeys.Contains(strAllowedKey))
                return keyType.List;
            else if (LanguageISOKeys.Contains(strAllowedKey))
                return keyType.LanguageISO;
            else if (YesNoKeys.Contains(strAllowedKey))
                return keyType.YesNo;
            else if (MangaYesNoKeys.Contains(strAllowedKey))
                return keyType.MangaYesNo;
            else if (DateTimeKeys.Contains(strAllowedKey))
                return keyType.DateTime;
            else if (StringKeys.Contains(strAllowedKey))
                return keyType.String;
            else if (PsuedoNumericKeys.Contains(strAllowedKey))
                return keyType.NumericString;
            else return keyType.Custom;
        }

        private static void GetAllLanguages()
        {
            LanguageISOs = new SortedDictionary<string, string>();

            LanguageISOs.Add("", "");
            LanguageISOs.Add("Arabic", "ar");
            LanguageISOs.Add("Bulgarian", "bg");
            LanguageISOs.Add("Catalan", "ca");
            LanguageISOs.Add("Chinese (Simplified)", "zh");
            LanguageISOs.Add("Czech", "cs");
            LanguageISOs.Add("Danish", "da");
            LanguageISOs.Add("German", "de");
            LanguageISOs.Add("Greek", "el");
            LanguageISOs.Add("English", "en");
            LanguageISOs.Add("Spanish", "es");
            LanguageISOs.Add("Finnish", "fi");
            LanguageISOs.Add("French", "fr");
            LanguageISOs.Add("Hebrew", "he");
            LanguageISOs.Add("Hungarian", "hu");
            LanguageISOs.Add("Icelandic", "is");
            LanguageISOs.Add("Italian", "it");
            LanguageISOs.Add("Japanese", "ja");
            LanguageISOs.Add("Korean", "ko");
            LanguageISOs.Add("Dutch", "nl");
            LanguageISOs.Add("Norwegian", "no");
            LanguageISOs.Add("Polish", "pl");
            LanguageISOs.Add("Portuguese", "pt");
            LanguageISOs.Add("Romanian", "ro");
            LanguageISOs.Add("Russian", "ru");
            LanguageISOs.Add("Croatian", "hr");
            LanguageISOs.Add("Slovak", "sk");
            LanguageISOs.Add("Albanian", "sq");
            LanguageISOs.Add("Swedish", "sv");
            LanguageISOs.Add("Thai", "th");
            LanguageISOs.Add("Turkish", "tr");
            LanguageISOs.Add("Urdu", "ur");
            LanguageISOs.Add("Indonesian", "id");
            LanguageISOs.Add("Ukrainian", "uk");
            LanguageISOs.Add("Belarusian", "be");
            LanguageISOs.Add("Slovenian", "sl");
            LanguageISOs.Add("Estonian", "et");
            LanguageISOs.Add("Latvian", "lv");
            LanguageISOs.Add("Lithuanian", "lt");
            LanguageISOs.Add("Persian", "fa");
            LanguageISOs.Add("Vietnamese", "vi");
            LanguageISOs.Add("Armenian", "hy");
            LanguageISOs.Add("Azeri", "az");
            LanguageISOs.Add("Basque", "eu");
            LanguageISOs.Add("Macedonian", "mk");
            LanguageISOs.Add("Afrikaans", "af");
            LanguageISOs.Add("Georgian", "ka");
            LanguageISOs.Add("Faroese", "fo");
            LanguageISOs.Add("Hindi", "hi");
            LanguageISOs.Add("Malay", "ms");
            LanguageISOs.Add("Kazakh", "kk");
            LanguageISOs.Add("Kyrgyz", "ky");
            LanguageISOs.Add("Kiswahili", "sw");
            LanguageISOs.Add("Uzbek", "uz");
            LanguageISOs.Add("Tatar", "tt");
            LanguageISOs.Add("Punjabi", "pa");
            LanguageISOs.Add("Gujarati", "gu");
            LanguageISOs.Add("Tamil", "ta");
            LanguageISOs.Add("Telugu", "te");
            LanguageISOs.Add("Kannada", "kn");
            LanguageISOs.Add("Marathi", "mr");
            LanguageISOs.Add("Sanskrit", "sa");
            LanguageISOs.Add("Mongolian", "mn");
            LanguageISOs.Add("Galician", "gl");
            LanguageISOs.Add("Konkani", "kok");
            LanguageISOs.Add("Syriac", "syr");
            LanguageISOs.Add("Divehi", "dv");
            LanguageISOs.Add("Chinese (Traditional)", "zh");
            LanguageISOs.Add("Serbian", "sr");
        }

        public static void InitializeGlobals(string strUserFile)
        {
            if (System.IO.File.Exists(strUserFile))
            {
                string[] iniSettings = System.IO.File.ReadAllLines(strUserFile);

                for (int i = 0; i < iniSettings.Length; i++)
                {
                    string line = iniSettings[i];
                    if (!line.StartsWith(";"))
                    {
                        string[] tmp = line.Split(new string[] { " = " }, 2, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 1)
                        {
                            string strKey = tmp[0];
                            string strValue = tmp[1];

                            switch (strKey.ToLower())
                            {
                                case "version":
                                    CurrentVersion = strValue;
                                    break;
                                // TODO: find exact name of this key
                                case "listdelimiter":
                                    DELIMITER = strValue;
                                    break;
                                case "multipleparamkeymodifiers":
                                case "multipleparamvalmodifiers":
                                    if (MultiParamKeyModifiers.Length > 0)
                                    {
                                        List<string> tmpString = new List<string>(MultiParamKeyModifiers);
                                        tmpString.AddRange(strValue.Split(new string[] { "," }, StringSplitOptions.None));
                                        MultiParamKeyModifiers = tmpString.ToArray();
                                    }
                                    else
                                        MultiParamKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeys":
                                    AllowedKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "readonlykeys":
                                    ReadOnlyKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "boolkeys":
                                    BoolKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "languageisokeys":
                                    LanguageISOKeys = strValue.Split(new string[] { " , ", "," }, StringSplitOptions.None);
                                    break;
                                case "numericalkeys":
                                    NumericKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "multivaluekeys":
                                    ListKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "pseudonumericalkeys":
                                    PsuedoNumericKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "yesnokeys":
                                    YesNoKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "mangayesnokeys":
                                    MangaYesNoKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "stringkeys":
                                    StringKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "datetimekeys":
                                    DateTimeKeys = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiers":
                                    AllowedKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiersmulti":
                                    ListKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiersnumeric":
                                    NumericalKeysModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifierlanguageiso":
                                    LanguageISOKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifieryesno":
                                    YesNoKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifierbool":
                                    BoolKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiermangayesno":
                                    MangaYesNoKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiersstring":
                                    StringKeysModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiersstring":
                                    StringValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifiercustom":
                                    CustomKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedkeymodifierdatetime":
                                    DateTimeKeyModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvals":
                                    AllowedValues = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiersnumeric":
                                    NumericValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiers":
                                    AllowedValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiersmulti":
                                    ListValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifierlanguageiso":
                                    LanguageISOValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifieryesno":
                                    YesNoValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiermangayesno":
                                    MangaYesNoValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifiercustom":
                                    CustomValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifierdatetime":
                                    DateTimeValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "allowedvalmodifierbool":
                                    BoolValueModifiers = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                                case "regexvarreplacefields":
                                    AllowedRegExVarReplace = strValue.Split(new string[] { "," }, StringSplitOptions.None);
                                    break;
                            }
                        }
                    }
                }
            }
            GetAllLanguages();
        }

        public static string[] GetAllAllowedModifiers(string strField)
        {
            List<string> tmpString = new List<string>(GetAllowedKeyModifiers(strField));
            tmpString.AddRange(GetAllowedValueModifiers(strField));

            tmpString.Sort();
            return tmpString.ToArray();
        }


    }

    public enum keyType : int
    {
        Custom = 0,
        String = 1,
        List = 2,
        Numeric = 3,
        NumericString = 9,
        DateTime = 4,
        LanguageISO = 5,
        YesNo = 6,
        MangaYesNo = 7,
        Bool = 8
    }

    public enum RulesetModes
    {
        AND,
        OR
    }
}