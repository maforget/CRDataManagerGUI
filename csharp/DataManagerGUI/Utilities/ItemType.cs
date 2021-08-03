using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataManagerGUI
{
    public enum ItemType : int
    {
        NothingUseable = 0,
        Group = 1,
        Ruleset = 2,
        Rule = 3,
        Action = 4, 
        ListOfRules = 5,
        ListOfActions = 6        
    }
}
