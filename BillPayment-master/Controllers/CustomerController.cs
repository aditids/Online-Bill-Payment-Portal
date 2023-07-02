using BillPayment.Context;
using BillPayment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BillPayment.Utils;
using BillPayment.ViewModels;
using BillPayment.Repository;


namespace BillPayment.Controllers
{
    public class CustomerController : Controller
    {
        /// <summary>
        /// This method has been used to return a Login object
        /// </summary>
        /// <param name="username">Username of the customer</param>
        /// <param name="password">Password of the custoemr</param>
        /// <returns></returns>
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
                        var encodingPasswordString = Helpers.EncodePassword(password, hashCode);
                        var query = (from s in context.Users where (s.UserName == username) && s.Password.Equals(encodingPasswordString) select s).FirstOrDefault();
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
            catch(Exception ex)
            {
                login.ErrorMessage = ex.Message.ToString();
                return login;
            }
            
           
        }

        /// <summary>
        /// This method has been used to check if registration is successfull or not for customer
        /// </summary>
        /// <param name="customer">Customer object</param>
        /// <returns>bool</returns>
        public bool Registration(User customer)
        {
            if (customer.UserName != null && customer.Password != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        using (BillPaymentContext context = new BillPaymentContext())
                        {
                            if ((context.Users.Where(u => u.UserName.Equals(customer.UserName)).FirstOrDefault()) == null)
                            {
                                var salt = Helpers.GeneratePassword(customer.Password.Length);
                                customer.Password = Helpers.EncodePassword(customer.Password, salt);
                                customer.VCode = salt;
                                customer.Role = "Customer";
                                context.Users.Add(customer);
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
        /// This action has been used to return the customer welcome screen view
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
                return RedirectToAction("CustomerLogin", "Auth");
           
        }

        /// <summary>
        /// This action has been used to return the view where customer can pay bill
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PayBill() 
        {
            if (Session["access_token"] != null)
            {
                ViewBag.uid = (int)Session["user_id"];
                return View();
            }
            else
                return RedirectToAction("CustomerLogin", "Auth");
        }

        /// <summary>
        /// This action has been used to return payment history view
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PaymentHistory()
        {
            if (Session["access_token"] != null)
            {
                ViewBag.uid = (int)Session["user_id"];
                CustomerRepository customerRepository = new CustomerRepository();
                var PaymentList = customerRepository.GetAllPayments((int)Session["user_id"]);
                ViewBag.PaymentList = PaymentList;
                return View();
            }
            else
                return RedirectToAction("CustomerLogin", "Auth");
            
        }

        /// <summary>
        /// This is a helper method to check if the parameter passed into it matches with some strings or not
        /// </summary>
        /// <param name="Category">Category of the bill</param>
        /// <returns></returns>
        public bool Check(string Category)
        {
            if(Category.Equals("Electricity") || Category.Equals("Telephone") || Category.Equals("Others"))
                return false;
            return true;
        }

        /// <summary>
        /// This method has been used to get all the payment details from database.
        /// </summary>
        /// <param name="Category">Category of bill</param>
        /// <param name="userId">User id of the loggeding user</param>
        /// <returns>JsonResult</returns>
        [HttpGet]
        public JsonResult GetPaymentDetails(string Category, int userId)
        {
            CustomerController customerController=new CustomerController();
            bool check=customerController.Check(Category);
            VendorRepository vendorRepository=new VendorRepository();
            var vendor=vendorRepository.GetVendorListByCategory(Category);
            PaymentView paymentView =new PaymentView();
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    string currentMonth = DateTime.Now.ToString("MMMM");
                    var obj = (from s in context.Payments where (s.UserId == userId && s.BillType.Equals(Category) && s.PaymentTime == currentMonth) select s).FirstOrDefault();
                    if (obj == null)
                    {
                        if (check == true)
                            paymentView.Amount = 1000;
                        else
                            paymentView.Amount = 0;

                        paymentView.Paid = false;
                        paymentView.Vendors = vendor;
                    }
                    else
                    {
                        paymentView.Paid = true;
                    }
                }

                return Json(paymentView, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return null;
            }
            
        }

        /// <summary>
        /// This method has been used to save the current payment details of a customer
        /// </summary>
        /// <param name="payment">Payment object</param>
        /// <returns>string</returns>
        [HttpPost]
        public string SavePaymentDetails(Payment payment)
        {
            try
            {
                using (BillPaymentContext context = new BillPaymentContext())
                {
                    if (payment.Success)
                    {
                        payment.PaymentTime = DateTime.Now.ToString("MMMM");
                        //p.PaymentTime=
                        context.Payments.Add(payment);
                        context.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        return "Error";
                    }
                }
            }
            catch (Exception)
            {
                return "Error";
            }
            
        }
    }
}