using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSAccount.Contract
{
    public class SearchColumn : IContract
    {
        // Constructor 
        public SearchColumn() { }

        // Public Members 

        public Int64 SearchColumnID { get; set; }

        public string SearchColumnName { get; set; }

        public string SearchColumnDisplay { get; set; }

        public string SearchCategory { get; set; }


    }
}
