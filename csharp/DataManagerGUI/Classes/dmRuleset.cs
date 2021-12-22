using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataManagerGUI
{
    [Serializable]
    public class dmRuleset : dmNode, IEquatable<dmRuleset>
    {
        public string QuickView
        {
            get
            {
                return ToString();
            }
        }

        public string OriginalText
        {
            get
            {
                return QuickView;
            }
            set
            {
                this.FromXML(value);
            }
        }
        public RulesetModes RuleMode { get; set; }

        public SortableBindingList<dmRule> Rules { get; set; }
        public SortableBindingList<dmAction> Actions { get; set; }

        public dmRuleset(dmContainer dmcParent)
            : base(dmcParent)
        {
        }

        public dmRuleset(dmContainer dmcParent, string strRule, Version version)
            : this(dmcParent)
        {
            Parse(strRule, version);
        }

        public dmRuleset(dmContainer dmcParent, string strRule)
            : this(dmcParent)
        {
            Parse(strRule);
        }

        public dmRuleset(dmContainer dmcParent, string[] strStrings)
            : this(dmcParent)
        {
            if (strStrings.Length > 1)
            {
                string[] NameAndComment = strStrings[0].Split(new string[] { Global.RulesetNameHeader, Global.CommentHeader }, StringSplitOptions.RemoveEmptyEntries);
                if (NameAndComment.Length > 0)
                    Name = NameAndComment[0];
                if (NameAndComment.Length > 1)
                    Comment = NameAndComment[1];
                Parse(strStrings[1]);
            }
            else
                Parse(strStrings[0]);
        }

        public dmRuleset(dmContainer dmcParent, dmRuleset other)
            : this(dmcParent)
        {
            dmRuleset tmp = new dmRuleset(dmcParent);
            tmp.Name = this.Name;
            tmp.Comment = this.Comment;

            foreach (dmRule item in Rules)
            {
                tmp.Rules.Add((dmRule)item.Clone());
            }

            foreach (dmAction item in Actions)
            {
                tmp.Actions.Add((dmAction)item.Clone());
            }

            this.OriginalText = other.OriginalText;
        }

        public dmRuleset(dmContainer dmcParent, XElement xParameters)
            : base(dmcParent)
        {
            FromXML(xParameters);
        }

        public void Parse(string strRule, Version version)
        {
            this.Actions = new SortableBindingList<dmAction>();
            this.Rules = new SortableBindingList<dmRule>();

            string RuleWithoutMode = strRule;

            if (strRule.StartsWith("|"))
            {
                this.RuleMode = RulesetModes.OR;
                RuleWithoutMode = RuleWithoutMode.Trim(new char[] { '|', ' ' });
            }

            string[] tmpString = RuleWithoutMode.Split(new string[] { "=>" }, StringSplitOptions.None);
            try
            {
                parseRules(tmpString[0].Trim(), version);
                parseActions(tmpString[1].Trim(), version);
            }
            catch 
            {
                FromXML(strRule);
            }
        }

        public void Parse(string strRule)
        {

            this.Actions = new SortableBindingList<dmAction>();
            this.Rules = new SortableBindingList<dmRule>();

            string RuleWithoutMode = strRule;

            if (RuleWithoutMode.StartsWith(Global.InvalidRuleset))
                RuleWithoutMode = RuleWithoutMode.Split(new char[] { '.' }, 2, StringSplitOptions.None)[1];

            OriginalText = RuleWithoutMode;

            if (RuleWithoutMode.StartsWith("|"))
            {
                this.RuleMode = RulesetModes.OR;
                RuleWithoutMode = RuleWithoutMode.Trim(new char[] { '|', ' ' });
            }

            string[] tmpString = RuleWithoutMode.Split(new string[] { " => ", " =>", "=> ", "=>" }, StringSplitOptions.None);
            try
            {
                parseRules(tmpString[0]);
                parseActions(tmpString[1]);
            }
            catch 
            {
                FromXML(strRule);
            }
        }

        private void parseRules(string strRulesToParse)
        {
            if (strRulesToParse.StartsWith("<<"))
            {
                string[] strArrayRules = strRulesToParse.Trim().Split(new string[] { ">> ", ">>", " <<", "<<" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < strArrayRules.Length; i++)
                {
                    if (strArrayRules[i].Trim() != "")
                    {
                        string sRuleToParse = strArrayRules[i];

                        dmRule tmpRule = new dmRule(sRuleToParse);

                        Rules.Add(tmpRule);
                    }
                }
            }
            else throw new Exception("Syntax error");
        }

        private void parseRules(string strRulesToParse, Version version)
        {
            string[] strArrayRules = strRulesToParse.Trim().Split(new string[] { ">> ", ">>", " <<", "<<" }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 0; i < strArrayRules.Length; i++)
            {
                if (strArrayRules[i].Trim() != "")
                {
                    string sRuleToParse = strArrayRules[i];

                    dmRule tmpRule = new dmRule(sRuleToParse, version);

                    Rules.Add(tmpRule);
                }
            }
        }

        private void parseActions(string p, Version version)
        {
            string[] strArrayRules = p.Trim().Split(new string[] { ">> ", ">>", " <<", "<<" }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 0; i < strArrayRules.Length; i++)
            {
                if (strArrayRules[i].Trim() != "")
                {
                    string sRuleToParse = strArrayRules[i];

                    dmAction tmpRule = new dmAction(sRuleToParse, version);

                    Actions.Add(tmpRule);
                }
            }
        }

        private void parseActions(string p)
        {
            string[] strArrayActions = p.Trim().Split(new string[] { ">>", "<<" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < strArrayActions.Length; i++)
            {
                if (strArrayActions[i].Trim() != "")
                {
                    string sActionToParse = strArrayActions[i];

                    dmAction tmpAction = new dmAction(sActionToParse);

                    Actions.Add(tmpAction);
                }
            }
        }

        public override string ToString()
        {
            return ToXML("ruleset").ToString().Replace("\r", "").Replace("\n", "");
        }

        public string[] Compile()
        {
            List<string> CompiledRuleset = new List<string>();
            string tmpString = "";

            tmpString = Global.RulesetNameHeader + Name;

            tmpString += " " + Global.CommentHeader + Comment;

            CompiledRuleset.Add(tmpString);


            CompiledRuleset.Add(ToString());
            return CompiledRuleset.ToArray();
        }

        public dmRuleset Clone(dmContainer dmcParent)
        {
            return new dmRuleset(dmcParent, this.ToXML("ruleset"));
        }

        public void AddRule(dmRule rule)
        {
            Rules.Add(rule);
        }

        public void AddAction(dmAction action)
        {
            Actions.Add(action);
        }

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
                return this.Equals((dmRuleset)obj);
            else
                return false;
        }

        public override int GetHashCode()
        {
            string strHashCalculator = this.Name + this.Comment + ToString();
            int tmpHash = strHashCalculator.GetHashCode();
            return tmpHash;
        }

        public override XElement ToXML(string strElementName)
        {
            XElement xReturn = base.ToXML(strElementName);
            xReturn.Add(new XAttribute("rulesetmode", Enum.GetName(typeof(RulesetModes), this.RuleMode)));
            foreach (dmParameters rule in this.Rules)
                xReturn.Add(rule.ToXML("rule"));
            foreach (dmParameters action in this.Actions)
                xReturn.Add(action.ToXML("action"));
            return xReturn;
        }

        public override void FromXML(XElement xParameters)
        {
            Clear();
            base.FromXML(xParameters);
            this.RuleMode = (RulesetModes)Enum.Parse(typeof(RulesetModes), xParameters.Attribute("rulesetmode").Value);
            foreach (XElement item in xParameters.Elements("rule"))
                this.Rules.Add(new dmRule(item));
            foreach (XElement item in xParameters.Elements("action"))
                this.Actions.Add(new dmAction(item));
        }

        public void FromXML(string xString)
        {
            try
            {
                XElement xParameters = XElement.Parse(xString);
                FromXML(xParameters);
            }
            catch { }
        }

        public override void Clear()
        {
            base.Clear();
            this.RuleMode = RulesetModes.AND;
            this.Actions = new SortableBindingList<dmAction>();
            this.Rules = new SortableBindingList<dmRule>();
        }

        #endregion

        public bool Equals(dmRuleset other)
        {
            int thisHash = GetHashCode();
            int otherHash = other.GetHashCode();
            return thisHash == other.GetHashCode();
        }

        internal void Reparse(string value)
        {
            try
            {
                Parse(value);
            }
            catch { }
        }


    }
}
