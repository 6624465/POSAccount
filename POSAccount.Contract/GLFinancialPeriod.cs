using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSAccount.Contract
{
    public class GLFinancialPeriod:IContract
    {
        public short BranchID { get; set; }
        public string FinancialYear { get; set; }

        public int Period { get; set; }
        public DateTime StartDate { get; set; }

        public bool IsARClosed { get; set; }
        public bool IsAPClosed { get; set; }
        public bool IsGLClosed { get; set; }
        public bool IsCBClosed { get; set; }


        public DateTime ARClosedDate { get; set; }
        public DateTime APClosedDate { get; set; }
        public DateTime GLClosedDate { get; set; }
        public DateTime CBClosedDate { get; set; }


        public string ARClosedBy { get; set; }
        public string APClosedBy { get; set; }
        public string GLClosedBy { get; set; }
        public string CBClosedBy { get; set; }



    }
}
