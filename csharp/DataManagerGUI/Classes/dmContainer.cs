using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DataManagerGUI
{
    [Serializable]
    public abstract class dmContainer: dmNode
    {
        public int GroupCount
        {
            get { return Groups.Count; }
        }
        public int RulesetCount
        {
            get { return this.Rulesets.Count; }
        }

        public SortableBindingList<dmGroup> Groups { get; set; }
        public SortableBindingList<dmRuleset> Rulesets { get; set; }

        public void AddGroup(dmGroup rsgGroup)
        {
            this.Groups.Add(rsgGroup);
        }
        public void AddGroupRange(IEnumerable<dmGroup> items)
        {
            for (int i = 0; i < items.Count(); i++)
            {
                AddGroup(items.ElementAt(i));
            }
        }
        public void RemoveGroupAt(int index)
        {
            Groups.RemoveAt(index);
        }
        public bool RemoveGroup(dmGroup item)
        {
            return Groups.Remove(item);
        }
        public void InsertGroup(int index, dmGroup item)
        {
            Groups.Insert(index, item);
        }
        public bool ContainsGroup(dmGroup item)
        {
            return Groups.Contains(item);
        }
        public int IndexOfGroup(dmGroup item)
        {
            return Groups.IndexOf(item);
        }

        public bool ContainsRuleset(dmRuleset item)
        {
            return Rulesets.Contains(item);
        }
        public bool RemoveRuleset(dmRuleset item)
        {
            return Rulesets.Remove(item);
        }
        public void RemoveRulesetAt(int index)
        {
            Rulesets.RemoveAt(index);
        }
        public void InsertRuleset(int index, dmRuleset item)
        {
            Rulesets.Insert(index, item);
        }
        public int IndexOfRuleset(dmRuleset item)
        {
            return Rulesets.IndexOf(item);
        }
        public void AddRuleset(dmRuleset rsRuleset)
        {
            Rulesets.Add(rsRuleset);
        }

        public dmContainer(dmContainer dmcParent)
            :base(dmcParent)
        {
            Clear();
        }

        public virtual String[] Compile()
        {
            List<string> tmpLines = new List<string>();

            for (int i = 0; i < GroupCount; i++)
            {
                //compile the groups
                tmpLines.AddRange(Groups[i].Compile());
            }

            for (int i = 0; i < RulesetCount; i++)
            {
                tmpLines.AddRange(Rulesets[i].Compile());
            }

            return tmpLines.ToArray();
        }

        protected int ParseGroup(string[] strArray, int nLineStart, dmContainer Parent)
        {
            int nReturn = nLineStart;
            dmGroup newGroup = new dmGroup(this);
            Dictionary<string, string> itemParameters = GetParameters(strArray[nReturn]);

            foreach (KeyValuePair<string, string> item in itemParameters)
            {
                switch (item.Key)
                {
                    case "FILTERSANDDEFAULTS":
                        newGroup.FiltersAndDefaults.Parse(item.Value);
                        break;
                    case "GROUP":
                        newGroup.Name = item.Value;
                        break;
                    case "COMMENT":
                        newGroup.Comment = item.Value;
                        break;
                }
            }
            nReturn++;

            //parses until it reaches the end of the group and returns the line number after the end of the group
            while (nReturn < strArray.Length && IdentifyLineType(strArray[nReturn]) != LineParseType.GroupEndLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(strArray[nReturn]))
                {
                    case LineParseType.GroupStartLine:
                        nReturn = ParseGroup(strArray, nReturn, newGroup);
                        break;
                    case LineParseType.RulesetLine:
                    case LineParseType.RulesetNameLine:
                        nReturn = ParseRuleset(strArray, nReturn, newGroup);
                        break;
                    default:
                        nReturn++;
                        break;
                }
            }
            nReturn++;
            Parent.AddGroup(newGroup);

            return nReturn;
        }


        public int MergeGroup(string[] strArray, int nLineStart, dmContainer Parent)
        {
            int nReturn = nLineStart;
            dmGroup newGroup = new dmGroup(this);
            Dictionary<string, string> itemParameters = GetParameters(strArray[nReturn]);

            foreach (KeyValuePair<string, string> item in itemParameters)
            {
                switch (item.Key)
                {
                    case "FILTERSANDDEFAULTS":
                        newGroup.FiltersAndDefaults.Parse(item.Value);
                        break;
                    case "GROUP":
                        newGroup.Name = item.Value;
                        break;
                    case "COMMENT":
                        newGroup.Comment = item.Value;
                        break;
                }
            }

            IEnumerable<dmGroup> oldGroup = Parent.Groups.Where(x => x.Name == newGroup.Name);

            if (oldGroup.Count() > 0)
            {
                newGroup = oldGroup.ElementAt(0);
            }
            else
            {
                Parent.AddGroup(newGroup);
            }
            nReturn++;

            //parses until it reaches the end of the group and returns the line number after the end of the group
            while (nReturn < strArray.Length && IdentifyLineType(strArray[nReturn]) != LineParseType.GroupEndLine)
            {
                string[] tmpVariable = new string[0];
                switch (IdentifyLineType(strArray[nReturn]))
                {
                    case LineParseType.GroupStartLine:
                        nReturn = ParseGroup(strArray, nReturn, newGroup);
                        break;
                    case LineParseType.RulesetLine:
                    case LineParseType.RulesetNameLine:
                        nReturn = MergeRuleset(strArray, nReturn, newGroup);
                        break;
                    default:
                        nReturn++;
                        break;
                }
            }
            nReturn++;
            //Parent.AddGroup(newGroup);

            return nReturn;
        }

        protected Dictionary<string, string> GetParameters(string string0)
        {
            Dictionary<string, string> tmpReturn = new Dictionary<string, string>();
            string delimiter = $"({Global.GroupHeader})|({Global.RulesetNameHeader})|({Global.CommentHeader})|({Global.GroupFilterAndDefaults})";
            var parts = Regex.Split(string0, delimiter)
                 .Where(p => p != string.Empty)
                 .ToList();

            for (int i = 0; i < parts.Count; i++)
            {
                string item = parts[i];
                string key = item.TrimStart(new char[] { '#', '@' }).Trim();
                if ((i + 1) < parts.Count && (item.StartsWith("#") || item.StartsWith("@")))
                {
                    tmpReturn.Add(key, parts[i + 1]);
                    i++;
                }
            }

            return tmpReturn;
        }

        /// <summary>
        /// Parses Orphaned Ruleset
        /// </summary>
        public int ParseRuleset(string[] strArray, int nLineStart, dmContainer dmcParent)
        {
            int nReturn = nLineStart;
            //parses until it reaches the end of the group and returns the line number after the end of the group
            dmRuleset newRS = new dmRuleset(this);


            string tmpString = strArray[nReturn];
            LineParseType tmpLPT = IdentifyLineType(tmpString);
            if (tmpLPT == LineParseType.RulesetNameLine)
            {
                Dictionary<string, string> itemParameters = GetParameters(strArray[nReturn]);
                foreach (KeyValuePair<string, string> item in itemParameters)
                {
                    switch (item.Key)
                    {
                        case "NAME":
                            newRS.Name = item.Value;
                            break;
                        case "COMMENT":
                            newRS.Comment = item.Value;
                            break;
                    }
                }
                nReturn++;
            }

            newRS.Parse(strArray[nReturn]);

            dmcParent.AddRuleset(newRS);
            nReturn++;

            return nReturn;
        }


        public int MergeRuleset(string[] strArray, int nLineStart, dmContainer dmcParent)
        {
            int nReturn = nLineStart;
            //parses until it reaches the end of the group and returns the line number after the end of the group
            dmRuleset newRS = new dmRuleset(this);


            string tmpString = strArray[nReturn];
            LineParseType tmpLPT = IdentifyLineType(tmpString);
            if (tmpLPT == LineParseType.RulesetNameLine)
            {
                Dictionary<string, string> itemParameters = GetParameters(strArray[nReturn]);
                foreach (KeyValuePair<string, string> item in itemParameters)
                {
                    switch (item.Key)
                    {
                        case "NAME":
                            newRS.Name = item.Value;
                            break;
                        case "COMMENT":
                            newRS.Comment = item.Value;
                            break;
                    }
                }
                nReturn++;
            }

            newRS.Parse(strArray[nReturn]);

            if (!dmcParent.Rulesets.Contains(newRS))
            {
                dmcParent.AddRuleset(newRS);
            }
            nReturn++;

            return nReturn;
        }

        public bool Contains(dmGroup item)
        {
            return ContainsGroup(item);
        }

        public static LineParseType IdentifyLineType(string strLine)
        {
            LineParseType lptReturn = LineParseType.BlankLine;
            if (strLine.StartsWith("#@"))
            {
                if (strLine.StartsWith(Global.GroupHeader))
                {
                    lptReturn = LineParseType.GroupStartLine;
                }
                else if (strLine.StartsWith(Global.EndOfGroup))
                {
                    lptReturn = LineParseType.GroupEndLine;
                }
                else if (strLine.StartsWith(Global.RulesetNameHeader))
                {
                    lptReturn = LineParseType.RulesetNameLine;
                }
                else if (strLine.StartsWith(Global.VariableHeader))
                {
                    lptReturn = LineParseType.SetVariableLine;
                }
                else if (strLine.StartsWith(Global.EndOfRules))
                {
                    lptReturn = LineParseType.EndRulesLine;
                }
                else if (strLine.StartsWith(Global.NotesHeader))
                {
                    lptReturn = LineParseType.NotesStartLine;
                }
                else if (strLine.StartsWith(Global.EndOfNotes))
                {
                    lptReturn = LineParseType.NotesEndLine;
                }
                else if (strLine.StartsWith(Global.AuthorHeader))
                {
                    lptReturn = LineParseType.AuthorLine;
                }
            }
            else if (string.IsNullOrEmpty(strLine) || strLine == "#")
            {
                lptReturn = LineParseType.BlankLine;
            }
            else if (strLine.StartsWith("# -"))
            {
                lptReturn = LineParseType.DividerLine;
            }
            else if (strLine.StartsWith("<<"))
            {
                lptReturn = LineParseType.RulesetLine;
            }

            return lptReturn;
        }

        #region Overrides
        public override void FromXML(System.Xml.Linq.XElement xParameters)
        {
            base.FromXML(xParameters);

            foreach (XElement group in xParameters.Elements("group"))
                this.Groups.Add(new dmGroup(this, group));
            foreach (XElement ruleset in xParameters.Elements("ruleset"))
                this.Rulesets.Add(new dmRuleset(this, ruleset));
        }

        public override XElement ToXML(string strElementName)
        {
            XElement xReturn = base.ToXML(strElementName);
            //add groups & rulesets
            foreach (dmGroup group in Groups)
                xReturn.Add(group.ToXML("group"));
            foreach (dmRuleset ruleset in this.Rulesets)
                xReturn.Add(ruleset.ToXML("ruleset"));
            return xReturn;
        }

        public override void Clear()
        {
            base.Clear();
            this.Groups = new SortableBindingList<dmGroup>();
            this.Rulesets = new SortableBindingList<dmRuleset>();
        }

        #endregion

    }
}
