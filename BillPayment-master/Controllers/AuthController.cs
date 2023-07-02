namespace BillPayment.Controllers
{
    using BillPayment.Models;
    using BillPayment.ViewModels;
    using System.Web.Mvc;

 
    public class AuthController : Controller
    {
        /// <summary>
        /// This action has been used to return view of home page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            if (Session["access_token"] != null)
            {
                switch (Session["user_role"])
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin");
                    case "Customer":
                        return RedirectToAction("Index", "Customer");
                    case "Vendor":
                        return RedirectToAction("Index", "Vendor");

                }
            }
            return View();
        }

        /// <summary>
        /// This action has been used to return view of Admin login.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult AdminLogin()
        {
            return View();
        }

        /// <summary>
        /// This method has been used for validating admin login.
        /// </summary>
        /// <param name="admin">Admin object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult AdminLogin(User admin)
        {
            AdminController adminController = new AdminController();
            Login login = adminController.Login(admin.UserName, admin.Password);
            if (login.LoggedIn)
            {
                Session["access_token"] = login.Token;
                Session["user_id"] = login.UserId;
                Session["user_role"] = login.Role;
                Session["fullname"] = login.Name;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.errorAL = login.ErrorMessage;
                ViewBag.ResultAL = login.LoggedIn;
                return View();
            }
        }

        /// <summary>
        /// This action has been used for retrurning admin registration page.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult AdminRegistration()
        {
            return View();
        }

        /// <summary>
        /// This method has been used to save admin registration details.
        /// </summary>
        /// <param name="admin">Admin object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult AdminRegistration(User admin)
        {
            AdminController adminController = new AdminController();
            bool CheckRegistration = adminController.Registration(admin);
            if (CheckRegistration)
            {
                ViewBag.ResultAR = true;
                ModelState.Clear();
                return View();
            }
            else
            {
                ViewBag.errorAR = "Username " + admin.UserName + " already exists";
                ViewBag.ResultAR = false;
                return View();

            }
        }

        /// <summary>
        /// This action has been used to return Customer Login View.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult CustomerLogin()
        {
            return View();
        }

        /// <summary>
        /// This method has been used to save customer redistration details.
        /// </summary>
        /// <param name="customer">Customer object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult CustomerLogin(User customer)
        {

            CustomerController customerController = new CustomerController();
            Login login = customerController.Login(customer.UserName, customer.Password);

            if (login.LoggedIn)
            {
                Session["access_token"] = login.Token;
                Session["user_id"] = login.UserId;
                Session["user_role"] = login.Role;
                Session["fullname"] = login.Name;

                return RedirectToAction("Index", "Customer");
            }
            else
            {
                ViewBag.ResultCL = login.LoggedIn;
                ViewBag.errorCL = login.ErrorMessage;
                return View();
            }
        }

        /// <summary>
        /// This action has been used for returning customer registration view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult CustomerRegistration()
        {
            return View();
        }

        /// <summary>
        /// This method has been used for registering a new vendor.
        /// </summary>
        /// <param name="customer">Customer object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult CustomerRegistration(User customer)
        {
            CustomerController customerController = new CustomerController();
            bool CheckRegistration = customerController.Registration(customer);
            if (CheckRegistration)
            {
                ViewBag.ResultCR = true;
                ModelState.Clear();
                return View();
            }
            else
            {
                ViewBag.errorCR = "Username " + customer.UserName + " already exists";
                ViewBag.ResultCR = false;
                return View();

            }
        }

        /// <summary>
        /// This action has been used for returning vendor login view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult VendorLogin()
        {
            return View();
        }

        /// <summary>
        /// This method has been used for validating a vendor credential.
        /// </summary>
        /// <param name="vendor">Vendor Object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult VendorLogin(User vendor)
        {
            VendorController vendorController = new VendorController();
            Login login = vendorController.Login(vendor.UserName, vendor.Password);

            if (login.LoggedIn)
            {
                Session["access_token"] = login.Token;
                Session["user_id"] = login.UserId;
                Session["user_role"] = login.Role;
                Session["fullname"] = login.Name;

                return RedirectToAction("Index", "Vendor");
            }
            else
            {
                ViewBag.errorVL = login.ErrorMessage;
                ViewBag.ResultVL = false;
                return View();
            }
        }

        /// <summary>
        /// This action has been used for returning vendor registration view.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult VendorRegistration()
        {
            return View();
        }

        /// <summary>
        /// This methid has been used for registering a new vendor.
        /// </summary>
        /// <param name="vendor">Vendor object.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public ActionResult VendorRegistration(User vendor)
        {
            VendorController vendorController = new VendorController();
            bool CheckRegistration = vendorController.Registration(vendor);
            if (CheckRegistration)
            {
                ViewBag.ResultVR = true;
                ModelState.Clear();
                return View();
            }
            else
            {
                ViewBag.errorVR = "Username " + vendor.UserName + " already exists";
                ViewBag.ResultVR = false;
                return View();

            }
        }

        /// <summary>
        /// This action a been used to perform logout.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Auth");
        }
    }
}
