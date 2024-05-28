using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataManagerGUI
{
    [Serializable]
    public class dmGroup : dmContainer
    {
        #region Properties

        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    return "Unnamed Group";
                }
                else
                {
                    return _name;
                }
            }
            set
            {
                if (value == "Unnamed Group")
                    _name = "";
                else
                    _name = value;
            }
        }
        public override string Comment { get; set; }
        public dmRuleset FiltersAndDefaults { get; set; }
        #endregion

        #region Initilization

        public dmGroup(dmContainer dmcParent)
            :base(dmcParent)
        {
            this.FiltersAndDefaults = new dmRuleset(this);
            this.Rulesets = new SortableBindingList<dmRuleset>();
            this.Groups = new SortableBindingList<dmGroup>();
        }

        public dmGroup(dmContainer dmcParent, XElement xParameters)
            :this(dmcParent)
        {
            FromXML(xParameters);
        }

        public dmGroup(dmContainer dmcParent,  string[] strArrayGroup)
            :this(dmcParent)
        {
            Parse(strArrayGroup, 0);
        }
                
        #endregion

        #region Non-Base

        /// <summary>
        /// Parses the Group from header point until group end is identified
        /// </summary>
        /// <param name="strGroupStrings"></param>
        /// <returns>The line number of the end of the group</returns>
        public int Parse(string[] strGroupStrings, int nGroupStartLine)
        {
            int counter = nGroupStartLine;
            Dictionary<string, string> itemParameters = GetParameters(strGroupStrings[counter]);

            foreach (KeyValuePair<string, string> item in itemParameters)
            {
                switch (item.Key)
                {
                    case "FILTERSANDDEFAULTS":
                        this.FiltersAndDefaults.Parse(item.Value);
                        break;
                    case "GROUP":
                        this.Name = item.Value;
                        break;
                    case "COMMENT":
                        this.Comment = item.Value;
                        break;
                }
            }
            counter++;

            while (counter < strGroupStrings.Length && IdentifyLineType(strGroupStrings[counter]) != LineParseType.GroupEndLine)
            {
                switch (IdentifyLineType(strGroupStrings[counter]))
                {
                    case LineParseType.GroupStartLine:
                        counter = ParseGroup(strGroupStrings, counter, this);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        counter = ParseRuleset(strGroupStrings, counter, this);
                        break;
                    default:
                        break;
                }
                counter++;
            }
            return counter;
        }
        
        private int Parse(string[] strGroupStrings, Version version)
        {
            int counter = 0;
            Dictionary<string, string> itemParameters = GetParameters(strGroupStrings[counter]);

            foreach (KeyValuePair<string, string> item in itemParameters)
            {
                switch (item.Key)
                {
                    case "FILTERSANDDEFAULTS":
                        this.FiltersAndDefaults.Parse(item.Value);
                        break;
                    case "GROUP":
                        this.Name = item.Value;
                        break;
                    case "COMMENT":
                        this.Comment = item.Value;
                        break;
                }
            }
            counter++;

            while (counter < strGroupStrings.Length && IdentifyLineType(strGroupStrings[counter]) != LineParseType.GroupEndLine)
            {
                switch (IdentifyLineType(strGroupStrings[counter]))
                {
                    case LineParseType.GroupStartLine:
                        counter = ParseGroup(strGroupStrings, counter, this);
                        break;
                    case LineParseType.RulesetNameLine:
                    case LineParseType.RulesetLine:
                        counter = ParseRuleset(strGroupStrings, counter, this);
                        break;
                    default:
                        break;
                }
                counter++;
            }
            return counter;
        }
        
        public override string[] Compile()
        {
            List<string> CompiledRulesetGroup = new List<string>();


            string GroupHeader = Global.GroupHeader + Name;
            CompiledRulesetGroup.Add(Global.Divider);
            if (!string.IsNullOrEmpty(Comment))
                GroupHeader += " " + Global.CommentHeader + Comment;

            if (FiltersAndDefaults.Rules.Count > 0 || FiltersAndDefaults.Actions.Count > 0)
                GroupHeader += Global.GroupFilterAndDefaults + FiltersAndDefaults.ToString();

            CompiledRulesetGroup.Add(GroupHeader);
            CompiledRulesetGroup.Add(Global.Divider);
            for (int i = 0; i < GroupCount; i++)
            {
                CompiledRulesetGroup.AddRange(Groups[i].Compile());
            }

            for (int i = 0; i < RulesetCount; i++)
            {
                CompiledRulesetGroup.AddRange(Rulesets[i].Compile());
            }
            CompiledRulesetGroup.Add(Global.EndOfGroup + " " + Name);
            CompiledRulesetGroup.Add(Global.Divider);

            return CompiledRulesetGroup.ToArray();
        }

        #endregion

        #region Overrides
        public override void FromXML(XElement xParameters, bool merge = false)
        {
            base.FromXML(xParameters, merge);
            this.FiltersAndDefaults = new dmRuleset(this, xParameters.Element("filtersanddefaults"));
        }

        public override XElement ToXML(string strElementName)
        {
            XElement xReturn =  base.ToXML(strElementName);
            xReturn.AddFirst(new XElement(this.FiltersAndDefaults.ToXML("filtersanddefaults")));
            return xReturn;
        }

        public override void Clear()
        {
            base.Clear();
            this.FiltersAndDefaults = new dmRuleset(this);
        }
        #endregion

        public dmGroup Clone(dmContainer dmcParent)
        {
            return new dmGroup(dmcParent, this.ToXML("group"));
        }
    }
}