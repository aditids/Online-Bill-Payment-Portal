using BillPayment.Models;
using BillPayment.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace BillPayment.Context
{
    public class BillPaymentContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Payment> Payments { get; set; }

    }
}