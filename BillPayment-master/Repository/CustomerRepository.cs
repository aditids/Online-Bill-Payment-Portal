using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BillPayment.Models;
using BillPayment.ViewModels;

using BillPayment.Context;

namespace BillPayment.Repository
{
    public class CustomerRepository
    {
        public List<Payment> GetAllPayments(int id)
        {
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var list = (from s in context.Payments where (s.UserId==id && s.Success==true) select s).ToList();
                    

                    if (list != null)
                    {
                        return list;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}