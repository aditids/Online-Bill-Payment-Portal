using BillPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillPayment.ViewModels
{
    public class PaymentView
    {
        public float Amount { get; set; }
        public bool Paid { get; set; }
        public List<User> Vendors { get; set; }
    }
}