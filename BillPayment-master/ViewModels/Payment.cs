using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace BillPayment.ViewModels
{
    public class Payment
    {
        public float Amount { get; set; }
        public bool Paid { get; set; }
        public List<Vendor> Vendors { get; set; }
    }
}