using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataManagerGUI
{

    abstract class SearchResults
    {
        public abstract string Match { get; }
        public abstract int Length { get; }
        public abstract string Location { get; set; }
    }

    class TreeSearchResults : SearchResults
    {
        System.Windows.Forms.TreeNode _node;
        
        public string Name
        {
            get
            {
                if (_node != null)
                    return ((dmNode)_node.Tag).Name;
                else
                    return "";
            }
        }
        public string Group
        {
            get
            {
                if (Node.Parent.Tag.GetType() == typeof(dmGroup))
                {
                    return ((dmGroup)Node.Parent.Tag).Name;
                }
                else
                {
                    return "NONE";
                }
            }
        }
        public string Path
        {
            get
            {
                return Node.FullPath;
            }
        }
        public System.Windows.Forms.TreeNode Node
        {
            get
            {
                return _node;
            }
        }
        public override string Location { get; set; }

        public TreeSearchResults(System.Windows.Forms.TreeNode tnNode)
        {
            this._node = tnNode;
        }

        public TreeSearchResults(System.Windows.Forms.TreeNode tnNode, string strLocation)
        	:this(tnNode)
        {
            Location = strLocation;
        }
        
        public override int Length
        {
            get 
            {
                if (Node.Tag.GetType() == typeof(dmGroup))
                {
                    return ((dmGroup)Node.Tag).FiltersAndDefaults.QuickView.Length;
                }
                else
                    return ((dmRuleset)Node.Tag).QuickView.Length;
            }
        }

        public override string Match
        {
            get 
            {
                if (_node.Tag.GetType() == typeof(dmGroup))
                    return ((dmGroup)_node.Tag).FiltersAndDefaults.QuickView;
                else
                    return ((dmRuleset)_node.Tag).QuickView;
            }
        }

    }
}
