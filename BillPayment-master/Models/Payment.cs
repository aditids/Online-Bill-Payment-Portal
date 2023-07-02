using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BillPayment.Models
{
    public class Payment
    {
      
            [Key]
            public int Id { get; set; }

            //[ForeignKey("User")]
            public int UserId { get; set; }

            //[ForeignKey("User")]
            public int VendorId { get; set; }

            public string BillType { get; set; }


            public float BillAmount { get; set; }

           // public float PendingAmount { get; set; }

            //public float PaidAmount { get; set; }

            public bool Success { get; set; }

            public string PaymentTime { get; set; }

            public User userObj { get;set; }

    }
}