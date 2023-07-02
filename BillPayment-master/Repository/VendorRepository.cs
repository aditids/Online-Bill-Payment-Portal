using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillPayment.ViewModels;
using BillPayment.Models;
using BillPayment.Context;
using System.Net;
using System.Data.Entity;

namespace BillPayment.Repository
{
    public class VendorRepository
    {
        public List<User> GetInvalidatedVendors() {
            try {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var rows = (from s in context.Users where s.Role == "Vendor" && s.Validate == false select s).ToList();

                    if (rows != null)
                    {
                        return rows; 
                    }
                }
            }
            catch (Exception) {
                return null;
            }
            return null;
        }
        public User GetVendorDetailsById(int id)
        {
            try
            {

                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var vendor = (from s in context.Users where (s.Role == "Vendor" && s.UserId==id) select s).First();

                    if (vendor != null)
                    {
                        return vendor;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return null;
        }

        public string UpdateVendor(int VendorId,bool Payload)
        {
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var entity = context.Users.FirstOrDefault(user => user.UserId == VendorId);

                    if (entity != null)
                    {
                        entity.Validate = Payload;
                        context.SaveChanges();
                        return "Success";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "error";
        }


        public string ProfileUpdate(User user)
        {
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var entity = context.Users.FirstOrDefault(u => u.UserId == user.UserId);

                    if (entity != null)
                    {
                        entity.FirstName = user.FirstName;
                        entity.LastName = user.LastName;
                        entity.Address = user.Address;
                        entity.Category = user.Category;
                        entity.Maplocation = user.Maplocation;
                        entity.ContactNumber = user.ContactNumber;
                        context.SaveChanges();
                        return "Success";
                    }

                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string DeleteVendor(int VendorId, bool Payload)
        {
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var entity = context.Users.FirstOrDefault(user => user.UserId == VendorId);

                    if (entity != null)
                    {
                        context.Users.Remove(entity);
                        context.SaveChanges();
                        return "Success";
                    }
                    else
                        return "error";

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<User> GetVendorListByCategory(string Category)
            {
            try
            {
                
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var list = (from s in context.Users where (s.Role == "Vendor" && s.Category==Category) select s).ToList();
                    //List<User> list = new List<User>();
                    //list=context.Users.Where(x=>x.Category==Category).ToList();

                    if (list != null)
                    {
                        return list;
                    }
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}