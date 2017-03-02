using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSAccount.Contract
{
    public class ProductImage
    {
        public ProductImage() { }
        public string Code { get; set; }

        public byte[] ProductImg { get; set; }
    }
}
