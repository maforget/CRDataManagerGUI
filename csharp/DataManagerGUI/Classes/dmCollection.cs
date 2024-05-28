using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;

namespace DataManagerGUI
{
    [Serializable]
    public class dmCollection : dmContainer
    {
        Version _version;
        public dmGroup Disabled { get; set; }
        public string Author { get; set; }
        public override string Name
        {
            get
            {
                return Author;
            }
            set
            {
                Author = value;
            }
        }
        public string Notes { get; set; }
        public override string Comment
        {
            get
            {
                return Notes;
            }
            set
            {
                Notes = value;
            }
        }

        public string Version
        {
            get
            {
                if (_version == null)
                    return "";

                string strVersion = this._version.Major.ToString() + ".";
                strVersion += this._version.Minor.ToString() + ".";
                strVersion += this._version.Build.ToString();
                return strVersion;
            }
            private set
            {
                string strVersion = value;
                this._version = new Version(strVersion);
            }
        }

        public string TextVersion
        {
            get
            {
                return GetTextOuput();
            }
            set
            {
                Parse(value.Split(new string[] { "\r", "\n", }, StringSplitOptions.RemoveEmptyEntries));
            }
        }

        private string GetTextOuput()
        {
            string strReturn = "";

            List<string> CompileResults = new List<string>();
            //get the compile
            CompileResults.Add(Global.VersionHeader + Version);
            CompileResults.AddRange(Compile());

            for (int i = 0; i < CompileResults.Count; i++)
            {
                strReturn += CompileResults[i] + "\r\n";
            }

            return strReturn;
        }

        public string GetGroupXML()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            XDocument x = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            x.Add(this.ToXML("group"));
            return x.ToString();
        }

        public dmCollection()
            : base(null)
        {
            Disabled = new dmGroup(this);
            Disabled.Name = "Disabled";
            Disabled.Comment = "Disabled Groups and Rulesets";
        }

        public dmCollection(string strFilePath)
            : this()
        {
            try
            {
                XDocument xParameters = XDocument.Load(strFilePath);

                this.FromXML(xParameters.Root);
            }
            catch
            {
                if (System.IO.File.Exists(strFilePath))
                {
                    string[] masterRules = System.IO.File.ReadAllLines(strFilePath);
                    Parse(masterRules);
                }
            }
            if (this._version == null)
            {
                _version = new Version("1.1.0.0");
            }
        }

        public dmCollection(dmGroup rsGroup)
            : this()
        {
            this.Groups.Add(rsGroup.Clone(this));
        }

        public void Parse(string[] masterRules)
        {
            this.Clear();
            int i = 0;

            if (IdentifyLineType(masterRules[i]) != LineParseType.VersionLine) this._version = new Version("1.1.0.0");
            else
            {
                string strVersion = masterRules[i].Replace(Global.VersionHeader, "") + ".0";
                this._version = new Version(strVersion);
                i++;
            }


            while (i < masterRules.Length && IdentifyLineType(masterRules[i]) != LineParseType.EndRulesLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(masterRules[i]))
                {
                    case LineParseType.AuthorLine:
                        this.Author = masterRules[i].Replace(Global.AuthorHeader, "");
                        break;
                    case LineParseType.VersionLine:
                        this.Version = masterRules[i].Replace(Global.VersionHeader, "");
                        break;
                    case LineParseType.GroupStartLine:
                        i = ParseGroup(masterRules, i, this);
                        break;
                    case LineParseType.SetVariableLine:

                        break;
                    case LineParseType.NotesStartLine:
                        i = ParseNotes(masterRules, i);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        i = ParseRuleset(masterRules, i, this);
                        break;
                    default:
                        i++;
                        break;
                }
            }

            i++;

            if (i < masterRules.Length)
            {
                ParseResidual(masterRules, i, _version);
            }
            //since this function will update to currentversion
            //Set version to current version
            Version = Global.CurrentVersion;
        }

        public void Merge(string[] masterRules)
        {
            int i = 0;
            Version tmpVersion;

            if (IdentifyLineType(masterRules[i]) != LineParseType.VersionLine) tmpVersion = new Version("1.1.0.0");
            else
            {
                string strVersion = masterRules[i].Replace(Global.VersionHeader, "") + ".0";
                tmpVersion = new Version(strVersion);
                i++;
            }

            while (i < masterRules.Length && IdentifyLineType(masterRules[i]) != LineParseType.EndRulesLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(masterRules[i]))
                {
                    case LineParseType.AuthorLine:
                        this.Author = masterRules[i].Replace(Global.AuthorHeader, "");
                        break;
                    case LineParseType.GroupStartLine:
                        i = MergeGroup(masterRules, i, this);
                        break;
                    case LineParseType.SetVariableLine:

                        break;
                    case LineParseType.NotesStartLine:
                        i = ParseNotes(masterRules, i);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        i = ParseRuleset(masterRules, i, this);
                        break;
                    default:
                        break;
                }
                i++;
            }

            i++;

            if (i < masterRules.Length)
            {
                ParseResidual(masterRules, i, tmpVersion);
            }
        }

        public void Import(string[] masterRules)
        {
            int i = 0;

            Version tmpVersion;

            if (IdentifyLineType(masterRules[i]) != LineParseType.VersionLine) tmpVersion = new Version("1.1.0.0");
            else
            {
                string strVersion = masterRules[i].Replace(Global.VersionHeader, "") + ".0";
                tmpVersion = new Version(strVersion);
                i++;
            }

            while (i < masterRules.Length && IdentifyLineType(masterRules[i]) != LineParseType.EndRulesLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(masterRules[i]))
                {
                    case LineParseType.GroupStartLine:
                        i = ParseGroup(masterRules, i, this);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        i = ParseRuleset(masterRules, i, this);
                        break;
                    default:
                        break;
                }
                i++;
            }

            i++;

            if (i < masterRules.Length)
            {
                ParseResidual(masterRules, i, tmpVersion);
            }
        }

        private int ParseNotes(string[] strNotesArray, int nStartFrom)
        {
            int nReturn = nStartFrom + 1;

            while (nReturn < strNotesArray.Length && IdentifyLineType(strNotesArray[nReturn]) != LineParseType.NotesEndLine)
            {
                switch (IdentifyLineType(strNotesArray[nReturn]))
                {
                    case LineParseType.NotesEndLine:
                        return nReturn;
                    default:
                        Notes += strNotesArray[nReturn].Replace("# ", "") + "\r\n";
                        break;
                }
                nReturn++;
            }
            return nReturn;
        }

        private int ParseResidual(string[] strResidualArray, int nStartFrom, Version version)
        {
            int i = nStartFrom;
            while (i < strResidualArray.Length && IdentifyLineType(strResidualArray[i]) != LineParseType.EndRulesLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(strResidualArray[i]))
                {
                    case LineParseType.GroupStartLine:
                        i = ParseGroup(strResidualArray, i, Disabled);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        i = ParseRuleset(strResidualArray, i, Disabled);
                        break;
                    default:
                        break;
                }
                i++;
            }
            return i;
        }

        public override string[] Compile()
        {
            List<string> Compiled = new List<string>(CreateHeader());

            //run base compile
            Compiled.AddRange(base.Compile());


            //END Rules
            Compiled.Add(Global.EndOfRules);
            //re add residual
            Compiled.AddRange(CreateResidual());
            return Compiled.ToArray();
        }

        private string[] CreateHeader()
        {
            List<string> strReturn = new List<string>(new string[] { "#Created by CR Data Manager GUI ", "#Do Not Edit Manually unless you know what you are doing" });

            if (!string.IsNullOrEmpty(Author))
                strReturn.Add(Global.AuthorHeader + Author);
            if (!string.IsNullOrEmpty(Notes))
                strReturn.AddRange(CreateNotes());
            return strReturn.ToArray();
        }

        private string[] CreateResidual()
        {
            List<string> tmpList = new List<string>(Disabled.Compile());

            //strip the headers and footers
            tmpList.RemoveAt(0);
            tmpList.RemoveAt(0);
            tmpList.RemoveAt(tmpList.Count - 1);
            tmpList.RemoveAt(tmpList.Count - 1);

            return tmpList.ToArray();
        }

        private string[] CreateNotes()
        {
            List<string> tmp = new List<string>(Notes.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None));

            tmp.Insert(0, "#");
            tmp.Insert(0, Global.NotesHeader);

            for (int i = 0; i < tmp.Count; i++)
            {
                tmp[i] = tmp[i].Insert(0, "# ");
            }

            tmp.Add(Global.EndOfNotes);

            return tmp.ToArray();
        }

        public void Save(string strFileandPathName)
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            XDocument x = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
            x.Add(this.ToXML("collection"));
            x.Save(strFileandPathName);
        }

        #region Overrides

        public override void Clear()
        {
            base.Clear();
            this.Disabled = new dmGroup(this);
            this.Disabled.Name = "Disabled";
            this.Disabled.Comment = "Disabled Groups and Rulesets";
        }

        public override XElement ToXML(string strElementName)
        {
            XElement xReturn = base.ToXML(strElementName);
            xReturn.Add(new XAttribute("version", this.Version));
            xReturn.Add(Disabled.ToXML("disabled"));
            return xReturn;
        }

        public override void FromXML(XElement xParameters)
        {
            base.FromXML(xParameters);
            this.Version = xParameters.Attribute("version").Value;
            this.Disabled = new dmGroup(this, xParameters.Element("disabled"));
        }

        #endregion
    }

    public enum LineParseType
    {
        AuthorLine,
        BlankLine,
        RulesetLine,
        GroupStartLine,
        GroupEndLine,
        RulesetNameLine,
        SetVariableLine,
        DividerLine,
        Unknown,
        EndRulesLine,
        NotesEndLine,
        NotesStartLine,
        VersionLine
    }
}
