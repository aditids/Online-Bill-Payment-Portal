using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillPayment.ViewModels
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Category { get; set; }
        public string ContactNumber { get; set; }

        public string Address { get; set; }
        public string Maplocation { get; set; }
        public string UserName { get; set; }

        public bool Validate { get; set; }

    }
}