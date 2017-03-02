using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POSAccount.ViewModals.Search
{
    public class LayoutMenu
    {
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public List<POSAccount.Contract.Securables> securablesLst { get; set; }
    }

    public class LayoutMenuRights
    {
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public List<SecurablesRights> securablesLst { get; set; }
    }

    public class RoleRightsMenu 
    {
        // Constructor 
        public RoleRightsMenu() { }

        // Public Members 

        public string RoleCode { get; set; }

        public string SecurableItem { get; set; }

        public bool hasRight { get; set; }
    }

    public class SecurablesRights
    {
        // Constructor 
        public SecurablesRights() { }

        // Public Members 

        public string SecurableItem { get; set; }

        public string GroupID { get; set; }

        public string Description { get; set; }

        public string ActionType { get; set; }

        public string Link { get; set; }

        public string Icon { get; set; }

        public bool hasRight { get; set; }

        public Int32 Sequence { get; set; }

        public Int32 ParentSequence { get; set; }

        public List<SecurablesRights> ActionMenus { get; set; }

    }
}