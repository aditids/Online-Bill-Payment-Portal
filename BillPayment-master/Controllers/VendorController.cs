using BillPayment.Context;
using BillPayment.Models;
using BillPayment.Repository;
using BillPayment.Utils;
using BillPayment.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BillPayment.Controllers
{
    public class VendorController : Controller
    {

        /// <summary>
        /// This method has been used to return a Login object
        /// </summary>
        /// <param name="username">Username of the vendor</param>
        /// <param name="password">Password of the vendor</param>
        /// <returns></returns>
        public Login Login(string username, string password)
        {
            Login login = new Login();
            login.LoggedIn = false;
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {

                    var obj = context.Users.Where(u => u.UserName.Equals(username)).FirstOrDefault();
                    if (obj != null && obj.Validate == true)
                    {
                        var hashCode = obj.VCode;
                        var encodingPasswordString = Helpers.EncodePassword(password, hashCode);
                        var query = (from s in context.Users where (s.UserName == username) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
                        if (query != null)
                        {
                            //Session["access_token"] = obj.VCode;
                            login.Name = query.FirstName + " " + query.LastName;
                            login.LoggedIn = true;
                            login.UserId = query.UserId;
                            login.Token = query.VCode;
                            login.Role = query.Role;

                            return login;
                        }
                        login.ErrorMessage = "Invalid Password";
                        return login;
                    }
                    else if (obj != null && obj.Validate == false)
                    {
                        login.ErrorMessage = "Please wait for admin approval..";
                        return login;
                    }
                }
                login.ErrorMessage = "Incorrect Username.";
                return login;
            }
            catch (Exception ex)
            {

                login.ErrorMessage = ex.Message.ToString();
                return login; ;
            }
            
        }


        /// <summary>
        /// This method has been used to check if registration is successfull or not for Vendor
        /// </summary>
        /// <param name="vendor">User object</param>
        /// <returns>bool</returns>
        public bool Registration(User vendor)
        {
            if (vendor.UserName != null && vendor.Password != null)
            {
                if (ModelState.IsValid)
                {
                    using (BillPaymentContext context = new BillPaymentContext())
                    {
                        if ((context.Users.Where(u => u.UserName.Equals(vendor.UserName)).FirstOrDefault()) == null)
                        {
                            vendor.Validate = false;
                            var salt = Helpers.GeneratePassword(vendor.Password.Length);
                            vendor.Password = Helpers.EncodePassword(vendor.Password, salt);
                            vendor.VCode = salt;
                            vendor.Role = "Vendor";
                            context.Users.Add(vendor);
                            context.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// This action has been used to return a Index view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            if (Session["access_token"] != null)
            {
                ViewBag.uid = Session["user_id"];
                return View();
            }
            else
                return RedirectToAction("VendorLogin", "Auth");
        }

        /// <summary>
        /// This action has been used to return Profile view of a vendor
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Profile()
        {
            if (Session["access_token"] != null)
            {
                VendorRepository vendorRepository = new VendorRepository();
                int user_id = (int)Session["user_id"];
                return View(vendorRepository.GetVendorDetailsById(user_id));
            }
            else
                return RedirectToAction("VendorLogin", "Auth");

        }


        /// <summary>
        /// This action has been used to return a view with profile details
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult Profile( User user)
        {

            VendorRepository vendorRepository = new VendorRepository();

            if (vendorRepository.ProfileUpdate(user) == "Success")
            {
                ViewBag.success = true;
            }
            else
            {
                ViewBag.success = false;
            }
            return View();
        }



    }
}