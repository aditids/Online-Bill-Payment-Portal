namespace BillPayment.Controllers
{
    using BillPayment.Context;
    using BillPayment.Models;
    using BillPayment.Repository;
    using BillPayment.Utils;
    using BillPayment.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Defines the <see cref="AdminController" />.
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// This method has been used for Admin login operation.
        /// </summary>
        /// <param name="username">.</param>
        /// <param name="password">.</param>
        /// <returns>Login Model.</returns>
        public Login Login(string username, string password)
        {
            Login login = new Login();
            login.LoggedIn = false;
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    var obj = (from s in context.Users where s.UserName == username select s).FirstOrDefault();

                    if (obj != null)
                    {
                        var hashCode = obj.VCode;
                        //Password Hasing Process Call Helper Class Method    
                        var encodingPasswordString = Helpers.EncodePassword(password, hashCode);
                        //Check Login Detail User Name Or Password    
                        var query = (from s in context.Users where (s.UserName == username) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
                        //var query1= from s in context.Admins where (s.UserName == username) select s).FirstOrDefault();
                        if (query != null)
                        {
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
                    login.ErrorMessage = "Incorrect Username.";
                    return login;
                }
            }
            catch (Exception ex)
            {
                login.ErrorMessage = ex.Message.ToString();
                return login;
            }
        }

        /// <summary>
        /// This action has been used for Admin Registration operation.
        /// </summary>
        /// <param name="user">.</param>
        /// <returns>Boolean.</returns>
        public bool Registration(User user)
        {
            if (user.UserName != null && user.Password != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        using (BillPaymentContext context = new BillPaymentContext())
                        {
                            if ((context.Users.Where(u => u.UserName.Equals(user.UserName)).FirstOrDefault()) == null)
                            {
                                var salt = Helpers.GeneratePassword(user.Password.Length);
                                user.Password = Helpers.EncodePassword(user.Password, salt);
                                user.VCode = salt;
                                user.Role = "Admin";
                                context.Users.Add(user);
                                context.SaveChanges();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }


                }
            }
            return false;
        }

        /// <summary>
        /// This action has been used to return a view for the index page of Admin.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            if (Session["access_token"] != null)
            {
                ViewBag.uid = Session["user_id"];
                return View();
            }
            else
                return RedirectToAction("AdminLogin", "Auth");
        }

        /// <summary>
        /// This action has been used ton return a view.
        /// </summary>
        /// <param name="id">Id has been used to pass vendor's id to the View.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult VendorDetails(int id)
        {
            ViewBag.UserId = id;
            return View();
        }

        /// <summary>
        /// This method has been used to get the list of vendors who are yet to be validated admin.
        /// </summary>
        /// <returns>JsonResult has been used to get data from ajax call as a JSON.</returns>
        [HttpGet]
        public JsonResult GetInvalidatedVendorList()
        {
            VendorRepository vendorRepository = new VendorRepository();
            return Json(vendorRepository.GetInvalidatedVendors(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method has been used to update the vendor details.
        /// </summary>
        /// <param name="vendor">Using Vendor view model to get updated vendor information.</param>
        /// <returns>string.</returns>
        [HttpPost]
        public string UpdateVendorList(Vendor vendor)
        {
            VendorRepository vendorRepository = new VendorRepository();
            if (vendor.Validate == true)
            {
                return vendorRepository.UpdateVendor(vendor.VendorId, vendor.Validate);
            }
            else
            {
                return vendorRepository.DeleteVendor(vendor.VendorId, vendor.Validate);
            }
        }

        /// <summary>
        /// This action has been used to get a particular vendor details.
        /// </summary>
        /// <param name="id">Id has been used to uniquely identify a vendor.</param>
        /// <returns>JsonResult.</returns>
        [HttpGet]
        public JsonResult GetVendorDetailsById(int id)
        {
            VendorRepository vendorRepository = new VendorRepository();
            var vendor = vendorRepository.GetVendorDetailsById(id);

            List<Vendor> ObjEmp = new List<Vendor>()
            {    
            //Adding records to list    
            new Vendor { UserName= vendor.UserName, Address=vendor.Address, Category=vendor.Category, ContactNumber=vendor.ContactNumber, FirstName=vendor.FirstName, LastName=vendor.LastName, Maplocation=vendor.Maplocation, VendorId=vendor.UserId, Validate=vendor.Validate}

            };
            return Json(ObjEmp, JsonRequestBehavior.AllowGet);
        }
    }
}
