using System;
using System.Collections.Generic;



namespace POSAccount.Contract
{
    public class AccountGroup : IContract
    {
        // Constructor 
        public AccountGroup() { }

        // Public Members 

        public string Code { get; set; }

        public string AccountType { get; set; }

        public string Description { get; set; }

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public Int16 SequenceNo { get; set; }

        public List<ChartOfAccount> COAList { get; set; }
    }
}
